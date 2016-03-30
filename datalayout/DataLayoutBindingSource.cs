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
using xwcs.core.ui.datalayout.attributes;

namespace xwcs.core.ui.datalayout
{
	public class GetFieldQueryableEventData
	{
		public object DataSource { get; set; }
		public string FieldName { get; set; }
	}

    public class KeyValuePair
    {
        public object Key;
        public string Value;
    }

    public class GetFieldOptionsListEventData
    {
        public List<KeyValuePair> List { get; set; }
        public string FieldName { get; set; }
    }

    public interface IDataLayoutExtender
	{
		void onGetQueryable(GetFieldQueryableEventData qd);
        void onGetOptionsList(GetFieldOptionsListEventData qd);
    }


	public class DataLayoutBindingSource : BindingSource, IDataLayoutExtender, IDisposable
	{
		private xwcs.core.manager.ILogger _logger;

		private DataLayoutControl _cnt;

		private Type _currentDataSourceType;

		private Dictionary<string, HashSet<attributes.CustomAttribute>> _customAttributes;
		private HashSet<string> _examinedTypes;

		public EventHandler<GetFieldQueryableEventData> GetFieldQueryable;
        public EventHandler<GetFieldOptionsListEventData> GetFieldOptionsList;

        private object _oldCurrent = null;

		private bool _layoutIsValid;


		private class scan_context
		{
			private class ctx_elem
			{
				public Type type;
				public Type proxiedType;
				public string name;
			}

			private Stack<ctx_elem> _curentTypesChain;

			public scan_context()
			{
				_curentTypesChain = new Stack<ctx_elem>();
			}

			public scan_context(scan_context orig)
			{
				_curentTypesChain = new Stack<ctx_elem>(orig._curentTypesChain.Reverse());
			}

			public string Name { get { string n = _curentTypesChain.Peek().name; return n != "" ? n + "." : n; } }
			public Type Type { get { return _curentTypesChain.Peek().type; } }

			public bool pushContext(Type t, string name)
			{
				if (_curentTypesChain.Count > 0)
				{
					// cycle check 
					if ((from e in _curentTypesChain where (e.type == t || e.proxiedType == t) select e).Count() > 0) return false;
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

			public void popContext()
			{
				_curentTypesChain.Pop();
			}

		}

		private class morphable_context
		{
			public string name;
			public scan_context ctx;
			public Type currentObjectType; //this will indicate necessity of re layout
			public PropertyDescriptor pd;
			public attributes.PolymorphFlag attribute;
		}

		private Dictionary<string, morphable_context> _morphablePaths;
		private scan_context _ctx;

		public DataLayoutBindingSource() : base()
        {
			start();
        }
		public DataLayoutBindingSource(IContainer c) : base(c)
		{
			start();
		}
		public DataLayoutBindingSource(object o, string s) : base(o, s)
		{
			start();
		}

		private void start()
		{
			//register handlers
			CurrentChanged += handleCurrentChanged;
			
			_examinedTypes = new HashSet<string>();
			_logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(GetType());// System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		}

		private void resetDataLayout() {
			if (_cnt != null && DataSource != null)
			{
				if (_logger != null && CurrencyManager.Position >= 0)
					_logger.Debug("Reset layout: " + (base.Current != null ? base.Current.GetPropValueByPathUsingReflection("id") : "null"));
				
				_cnt.DataSource = null;
				_cnt.DataBindings.Clear();
				_cnt.Clear();
				_cnt.DataSource = this;
				_layoutIsValid = true;
            }
		}

		protected override void OnListChanged(ListChangedEventArgs e)
		{
			if(_logger!= null && CurrencyManager.Position >= 0)
				_logger.Debug("LC-Current: " + (base.Current != null ? base.Current.GetPropValueByPathUsingReflection("id") : "null"));

			switch (e.ListChangedType)
			{
				case ListChangedType.PropertyDescriptorChanged:
					{
						//we have to refresh attributes cache
						if (DataSource != null && e.PropertyDescriptor == null)
						{
							init();
						}

						break;
					}	
			}

			//orig call
			try {
				base.OnListChanged(e);
			}catch(Exception ex) {
				// we can have problems to bind at form cause it can not match new data
				// so stop exception here, cause we are moving to new record
				if (_logger != null && CurrencyManager.Position >= 0) {
					_logger.Debug("LC-EXCEPT-Current: (" + ex.Message + ") " + (base.Current != null ? base.Current.GetPropValueByPathUsingReflection("id") : "null"));
				}
            }
			

			if (_logger != null && CurrencyManager.Position >= 0)
				_logger.Debug("LC-OUT-Current: " +  (base.Current != null ? base.Current.GetPropValueByPathUsingReflection("id") : "null"));

			
		}

		[Browsable(false)]
		public new object Current
		{
			get
			{
				object cc = base.Current;

				// handle morph ables serialization
				foreach (KeyValuePair<string, morphable_context> entry in _morphablePaths)
				{
					if (entry.Value.attribute.Kind != attributes.PolymorphKind.Undef)
					{
						object actualComplexValue = cc.GetPropValueByPathUsingReflection(entry.Key);
						if(actualComplexValue != null) {
							// here use eventual events, cause we set field which can be saved in DB
							SetPropertyInternal(cc, entry.Value.attribute.SourcePropertyName, actualComplexValue.TypedSerialize(entry.Value.attribute.SourcePropertyName, entry.Value.attribute.Kind));
						}
						else {
							// do not reset serialized value, this value always win, it can be not recognized by de serializer
							// so don't touch original value !!!!
							// SetPropertyInternal(cc, entry.Value.attribute.SourcePropertyName, "");
						}
					}
				}

				return cc;
			}
		}

        public void addNewRecord(object rec)
        {
            AddNew();
            base.Current.CopyFrom(rec);
           
        }

		private void handleCurrentChanged(object sender, object args)
		{
			
			

			_logger.Debug("CC-Current: " + (base.Current != null ? base.Current.GetPropValueByPathUsingReflection("id") : "null"));
			
			
			if (_oldCurrent == base.Current) return;	

			// handle serialization / de-serialization of morph able objects
			// if there is _oldCurrent we have to handle serialization of all morph able fields
			bool doSerialize = _oldCurrent != null && base.Current != _oldCurrent;

			//check eventual types morphing
			foreach (KeyValuePair<string, morphable_context> entry in _morphablePaths)
			{
				object val = base.Current.GetPropValueByPathUsingReflection(entry.Key);
				
				//we need eventually de-serialize if there is connection to source set in morph able attribute
				if (val == null && entry.Value.attribute.Kind != attributes.PolymorphKind.Undef)
				{
					string strValue = (string)base.Current.GetPropValueByPathUsingReflection(entry.Value.attribute.SourcePropertyName);
					val = strValue.TypedDeserialize(entry.Value.attribute.SourcePropertyName, entry.Value.attribute.Kind);
					if(val != null) {
						base.Current.SetPropValueByPathUsingReflection(entry.Key, val);
					}
				}

				
				Type valT = val == null ? typeof(object) : val.GetType();
				if (valT != entry.Value.currentObjectType)
				{
					entry.Value.currentObjectType = valT;
					// notify PropertyDescriptor change
						
					//take correct PD from types
					ChainingPropertyDescriptor cd = TypeDescriptor.GetProperties(base.Current).Find(entry.Value.name, false) as ChainingPropertyDescriptor;
					if (cd != null && cd.PropertyType != valT)
					{
						cd.ForcedPropertyType = valT;
						
						if(val != null) {
							//restore context
							_ctx = entry.Value.ctx;
							//add sub type in hyper
							HyperTypeDescriptionProvider.Add(valT);
							//append type in scanned attributes
							scanCustomAttributes(valT, entry.Key);
						}
					}
					// and if cd is not null notify change
					if(cd != null) {
						_layoutIsValid = false;
						//OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, cd));
					}					
				}
				

				if (doSerialize && entry.Value.attribute.Kind != attributes.PolymorphKind.Undef)
				{
					object oldVal = _oldCurrent.GetPropValueByPathUsingReflection(entry.Key);
					if (oldVal != null)
					{
						// here use eventual events, cause we set field which can be saved in DB
						SetPropertyInternal(_oldCurrent, entry.Value.attribute.SourcePropertyName, oldVal.TypedSerialize(entry.Value.attribute.SourcePropertyName, entry.Value.attribute.Kind));
					}
					else
					{
						// do not reset serialized value, this value always win, it can be not recognized by de serializer
						// so don't touch original value !!!!
						// SetPropertyInternal(cc, entry.Value.attribute.SourcePropertyName, "");
					}
				}
			}

			_oldCurrent = base.Current;

			//if there is no more valid layout reset is
			if (!_layoutIsValid)
			{
				resetDataLayout();
			}

			_logger.Debug("CC-OUT-Current: " + (base.Current != null ? base.Current.GetPropValueByPathUsingReflection("id") : "null"));
		}

		private bool SetPropertyInternal(object target, string name, object value)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(target);
			ChainingPropertyDescriptor pd = (ChainingPropertyDescriptor)pdc.Find(name, false);
			if (pd != null)
			{
				object currVal = pd.GetValue(target);
				if (currVal != value)
				{
					pd.SetValue(target, value);
					return true;
				}
			}

			return false;
		}
		public void SetProperty(string name, object value)
		{
			if (SetPropertyInternal(base.Current, name, value))
			{
				handleCurrentChanged(this, null);
			}
		}

		private void init()
		{

			Type t = null;

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
						_logger.Error("Missing DataSource for data layout");
						return; // no valid binding arrived so we skip 
					}
				}
			}


			if (_currentDataSourceType != t)
			{

				_customAttributes = new Dictionary<string, HashSet<attributes.CustomAttribute>>();
				_morphablePaths = new Dictionary<string, morphable_context>();
				_ctx = new scan_context();

				_currentDataSourceType = t;

				if (t.BaseType != null && t.Namespace == "System.Data.Entity.DynamicProxies")
				{
					HyperTypeDescriptionProvider.Add(t.BaseType);
				}
				else
				{
					HyperTypeDescriptionProvider.Add(t);
				}

				scanCustomAttributes(t, "");
			}
		}



		/* PRIVATE */
		private void scanCustomAttributes(Type t, string name)
		{
			_logger.Debug("Scan check for type:" + t.Name);
			// test if not done
			if (_examinedTypes.Contains(t.Name)) return; // we did it already
			_examinedTypes.Add(t.Name);

			// make context 
			if (!_ctx.pushContext(t, name)) return;

			_logger.Debug("Scan type:" + t.Name + " for " + name);

			//handle eventual MetadataType annotation which will add annotations from surrogate object
			try
			{
				List<MetadataTypeAttribute> l = TypeDescriptor.GetAttributes(t).OfType<MetadataTypeAttribute>().ToList();
				if (l.Count > 0)
				{
					_logger.Debug("Scan MetaDataLink ... " + t.Name);
					foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(l.Single().MetadataClassType))
					{
						_logger.Debug("PD : " + pd.Name);
						handleOneProperty(pd);
					}
					_logger.Debug("Scan MetaDataLink  DONE ... ");
				}
			}
			catch (Exception ex)
			{
				_logger.Warn(ex.Message);
			}

			_logger.Debug("Scan own fields ... " + t.Name);
			foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(t))
			{
				_logger.Debug("PD : " + pd.Name + " of " + t.Name);
				handleOneProperty(pd);
			}
			_logger.Debug("Scan own fields DONE ... ");
			// remove one context level
			_ctx.popContext();
		}

		private void handleOneProperty(PropertyDescriptor t)
		{
			//we can have complex types
			if (t.PropertyType.FullName == "System.String" || t.PropertyType.IsPrimitive || t.PropertyType.FullName == "System.DateTime" || t.PropertyType.IsValueType)
			{
				string key = _ctx.Name + t.Name;
				_logger.Debug("Examine attrs for : " + key);

				HashSet<attributes.CustomAttribute> goodAtts = new HashSet<attributes.CustomAttribute>(t.Attributes.OfType<attributes.CustomAttribute>().ToList());

				if (goodAtts.Count > 0)
				{
					_logger.Debug("Attr for : " + key);
					//mix with existing attrs for that name
					if (_customAttributes.ContainsKey(key))
					{
						_customAttributes[key].UnionWith(goodAtts);
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
					if (pmfl.Count > 0 && cd != null)
					{
						_morphablePaths.Add(_ctx.Name + t.Name, new morphable_context { name = t.Name, ctx = new scan_context(_ctx), currentObjectType = null, pd = cd, attribute = pmfl[0] });
					}
				}
				catch (Exception) { }

				_logger.Debug("Going into:" + _ctx.Name + t.Name);
				scanCustomAttributes(t.PropertyType, t.Name); //here inside it will handle eventual cycles!!!
			}
		}

		
		public DataLayoutControl DataLayout
		{
			get
			{
				return _cnt;
			}
			set
			{
				if (_cnt == value) return;
				//first disconnect eventual old one
				if (_cnt != null)
				{
					_cnt.FieldRetrieved -= FieldRetrievedHandler;
					_cnt.FieldRetrieving -= FieldRetrievingHandler;
				}
				_cnt = value;
				_cnt.AllowGeneratingNestedGroups = DevExpress.Utils.DefaultBoolean.True;
				_cnt.AutoRetrieveFields = true;
				_cnt.AllowCustomization = false;
				//_cnt.AllowCustomizationMenu = false;
				_cnt.FieldRetrieved += FieldRetrievedHandler;
				_cnt.FieldRetrieving += FieldRetrievingHandler;
				_cnt.DataSource = this;
				_layoutIsValid = false;
            }
		}

		private void FieldRetrievedHandler(object sender, FieldRetrievedEventArgs e)
		{
			_logger.Debug("Retrieving for field:" + e.FieldName);
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
			if(Current != null) {
				PropertyDescriptor pd = ReflectionHelper.GetPropertyDescriptorFromPath(Current.GetType(), e.FieldName);
			}
			
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
			if (GetFieldQueryable != null)
			{
				GetFieldQueryable(this, qd);
			}
		}

        public void onGetOptionsList(GetFieldOptionsListEventData qd)
        {
            if (GetFieldOptionsList != null)
            {
                GetFieldOptionsList(this, qd);
            }
        }

        protected virtual void onFieldRetrieving(FieldRetrievingEventArgs e) { }
		protected virtual void onFieldRetrieved(FieldRetrievedEventArgs e) { }


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
		~DataLayoutBindingSource()
		{
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
