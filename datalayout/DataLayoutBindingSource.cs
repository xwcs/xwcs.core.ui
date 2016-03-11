using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraDataLayout;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Data;
using System.ComponentModel;
using Hyper.ComponentModel;

namespace xwcs.core.ui.datalayout
{
	public class GetFieldQueryableEventData
	{
		public object DataSource { get; set; }
		public string FieldName { get; set; }
	}

	public interface IDataLayoutExtender {
		void onGetQueryable(GetFieldQueryableEventData qd);
    }


	
	public class DataLayoutBindingSource: BindingSource, IDataLayoutExtender, IDisposable
	{
		
		private DataLayoutControl _cnt;

		private Type _currentDataSourceType;
		
		private Dictionary<string, List<attributes.CustomAttribute>> _customAttributes;
		
		public EventHandler<GetFieldQueryableEventData> GetFieldQueryable;

		
		

		private class scan_context {
			private class ctx_elem {
				public Type type;
				public Type proxiedType;
				public string name;
			}
		
			private Stack<ctx_elem> _curentTypesChain;

			public scan_context() {
				_curentTypesChain = new Stack<ctx_elem>();
			}

			public scan_context(scan_context orig) {
				_curentTypesChain = new Stack<ctx_elem>(orig._curentTypesChain.Reverse());
			}

			public string Name { get { string n = _curentTypesChain.Peek().name;  return n != "" ? n + "." : n; } }
			public Type Type { get { return _curentTypesChain.Peek().type; } }

			public bool pushContext(Type t, string name) {
				if(_curentTypesChain.Count > 0) { 
					// cycle check 
					if ((from e in _curentTypesChain where (e.type == t || e.proxiedType == t)select e).Count() > 0) return false; 
				}
				// new in chain
				if (t.BaseType != null && t.Namespace == "System.Data.Entity.DynamicProxies")
				{
					_curentTypesChain.Push(new ctx_elem { type = t, proxiedType = t.BaseType, name = name });
				}
				else
				{
					_curentTypesChain.Push(new ctx_elem { type = t, proxiedType = t, name = name });
				}

				return true;				
			}

			public void popContext() {
				_curentTypesChain.Pop();
			}
		
		}

		private class morphable_context
		{
			public string name;
			public scan_context ctx;
		}

		private Dictionary<string, morphable_context> _morphablePaths;

		private scan_context _ctx;







		public DataLayoutBindingSource() : base() {
			start();
		}
		public DataLayoutBindingSource(IContainer c) : base(c) {
			start();
		}
		public DataLayoutBindingSource(object o, string s) : base(o, s) {
			start();
		}

		private void start() {
			//register handlers
			CurrentChanged += handleCurrentChanged;
        }

		/* wrap original */
		/*
		public new object DataSource { 
			get {
				return base.DataSource; 
			} 
			set {
				base.DataSource = value;
			}
		}
		*/

		private void handleCurrentChanged(object sender, object args) {
			init();

			
			bool needRelayout = false;
			//check eventual types morphing

			foreach (KeyValuePair<string, morphable_context> entry in _morphablePaths)
			{
				object val = Current.GetPropValueByPath(entry.Key);
				if (val != null)
				{
					Type valT = val.GetType();

					//take correct PD from types
					ChainingPropertyDescriptor cd = TypeDescriptor.GetProperties(entry.Value.ctx.Type).Find(entry.Value.name, false) as ChainingPropertyDescriptor;
					if(cd != null) {
						if (cd.ForcedPropertyType != valT)
						{
							cd.ForcedPropertyType = valT;
							needRelayout = true;

							//restore context
							_ctx = entry.Value.ctx;
							//append type in scanned attributes
							scanCustomAttributes(valT, entry.Key);
						}
					}
				}
			}

			//test 
			needRelayout = true;

			if (needRelayout && _cnt != null)
			{
				_cnt.RetrieveFields();
			}
		}

		private void init() {

			Type t = null;

            if (Current != null) {
				t = Current.GetType();
			}
			else {
				object tmpDs = null;

				//read annotations
				//here it depends what we have as DataSource, it can be Object, Type or IList Other we will ignore
				BindingSource bs = base.DataSource as BindingSource;
				if (bs == null)
				{
					//no binding source
					tmpDs = base.DataSource;
				}
				else
				{
					tmpDs = bs.DataSource;
				}

				t = tmpDs as Type;
				if (t == null)
				{
					//lets try another way, maybe IList
					IList someList = tmpDs as IList;
					if (someList != null)
					{
						//try to obtain element type
						t = someList.GetType().GetGenericArguments()[0];
					}
					else
					{
						//it should be plain object and try to take type
						if ((tmpDs as DataSet) == null &&
						   (tmpDs as DataTable) == null &&
						   (tmpDs as DataView) == null &&
						   (tmpDs as DataViewManager) == null &&
						   (tmpDs as object) != null
						)
						{
							t = tmpDs.GetType();
						}
						else
						{
							Console.WriteLine("Missing DataSource for data layout");
							return; // no valid binding arrived so we skip 
						}
					}
				}
			}	
			
			if(_currentDataSourceType != t) {

				_customAttributes = new Dictionary<string, List<attributes.CustomAttribute>>();
				_morphablePaths = new Dictionary<string, morphable_context>();
				_ctx = new scan_context();

				_currentDataSourceType = t;				

				if (t.BaseType != null && t.Namespace == "System.Data.Entity.DynamicProxies"){
					HyperTypeDescriptionProvider.Add(t.BaseType);
				}else {
					HyperTypeDescriptionProvider.Add(t);
				}

				scanCustomAttributes(t, "");
			}			
		}



		/* PRIVATE */
		private void scanCustomAttributes(Type t, string name)
		{
			Console.WriteLine("Scan check for type:" + t.Name);
			
			// make context 
			if (!_ctx.pushContext(t, name)) return;			

			Console.WriteLine("Scan type:" + t.Name + " for " + name);

			//handle eventual MetadataType annotation which will add annotations from surrogate object
			try
			{
				List<MetadataTypeAttribute> l = TypeDescriptor.GetAttributes(t).OfType<MetadataTypeAttribute>().ToList();
				if(l.Count > 0) {
					Console.WriteLine("Scan MetaDataLink ... " + t.Name);
					foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(l.Single().MetadataClassType))
					{
						Console.WriteLine("PD : " + pd.Name);
						handleOneProperty(pd);
					}
					Console.WriteLine("Scan MetaDataLink  DONE ... ");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("Scan own fields ... " + t.Name);
			foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(t))
			{
				Console.WriteLine("PD : " + pd.Name + " of " + t.Name); 
				handleOneProperty(pd);
			}
			Console.WriteLine("Scan own fields DONE ... ");
			// remove one context level
			_ctx.popContext();
		}

		private void handleOneProperty(PropertyDescriptor t)
		{
			//we can have complex types
			if (t.PropertyType.FullName == "System.String" || t.PropertyType.IsPrimitive || t.PropertyType.FullName == "System.DateTime" || t.PropertyType.IsValueType)
			{
				string key = _ctx.Name + t.Name;
				Console.WriteLine("Examine attrs for : " + key);

				List<attributes.CustomAttribute> goodAtts = t.Attributes.OfType<attributes.CustomAttribute>().ToList();

				if (goodAtts.Count > 0)
				{
					Console.WriteLine("Attr for : " + key);
					//mix with existing attrs for that name
					if (_customAttributes.ContainsKey(key))
					{
						_customAttributes[key].AddRange(goodAtts);
					}
					else
					{
						_customAttributes.Add(key, goodAtts);
					}
				}
			}
			else if (t.PropertyType.IsClass) //do recursion only for classes
			{
				// morph able eventuality
				try
				{
					List<attributes.PolymorphFlag> pmfl = t.Attributes.OfType<attributes.PolymorphFlag>().ToList();
					ChainingPropertyDescriptor cd = t as ChainingPropertyDescriptor;
					if (pmfl.Count > 0 && cd != null) {
						_morphablePaths.Add(_ctx.Name + t.Name, new morphable_context { name = t.Name, ctx = new scan_context(_ctx) });
					}
				}
				catch (Exception) { }

				Console.WriteLine("Going into:" + _ctx.Name + t.Name);
				scanCustomAttributes(t.PropertyType, t.Name); //here inside it will handle eventual cycles!!!
			}
		}



		public DataLayoutControl DataLayout {
			get
			{
				return _cnt;
			}
			set
			{
				if (_cnt == value) return;
				//first disconnect eventual old one
				if(_cnt != null) {
					_cnt.FieldRetrieved -= FieldRetrievedHandler;
					_cnt.FieldRetrieving -= FieldRetrievingHandler;
                }
				_cnt = value;
				_cnt.AllowGeneratingNestedGroups = DevExpress.Utils.DefaultBoolean.True;
				_cnt.AutoRetrieveFields = true;
				_cnt.FieldRetrieved += FieldRetrievedHandler;
				_cnt.FieldRetrieving += FieldRetrievingHandler;
				_cnt.DataSource = this;
			}
		} 

		private void FieldRetrievedHandler(object sender, FieldRetrievedEventArgs e) {
			if (_customAttributes.ContainsKey(e.FieldName))
			{
				foreach (attributes.CustomAttribute a in _customAttributes[e.FieldName])
				{
					a.applyRetrievedAttribute(this, e);
				}
			}
			onFieldRetrieved(e);
		}

		private void FieldRetrievingHandler(object sender, FieldRetrievingEventArgs e)
		{
			if (_customAttributes.ContainsKey(e.FieldName))
			{
				foreach (attributes.CustomAttribute a in _customAttributes[e.FieldName])
				{
					a.applyRetrievingAttribute(this, e);
				}
			}
			onFieldRetrieving(e);
			// fixed things
			e.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
			e.Handled = true;
		}

		public void onGetQueryable(GetFieldQueryableEventData qd)
		{
			if(GetFieldQueryable != null) {
				GetFieldQueryable(this, qd);
			}			
		}		

		protected virtual void onFieldRetrieving(FieldRetrievingEventArgs e){}
		protected virtual void onFieldRetrieved(FieldRetrievedEventArgs e){}
		

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					//only if disposing is caled from Dispose patern	
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				//disconnect events in any case
				if (_cnt != null)
				{
					_cnt.FieldRetrieved -= FieldRetrievedHandler;
					_cnt.FieldRetrieving -= FieldRetrievingHandler;
				}

				disposedValue = true;
				//call inherited
				base.Dispose(disposing);
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~DataLayoutBindingSource() {
		   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		   Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		/* INHERITED SO NOT USE IT HERE
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		*/
		#endregion
	}
}
