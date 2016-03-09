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
	
	public class DataLayoutExtender : IDataLayoutExtender
	{
		private DataLayoutControl _cnt;
		private Dictionary<string, List<attributes.CustomAttribute>> _customAttributes;
		private bool _attributesLoaded;

		public EventHandler<GetFieldQueryableEventData> GetFieldQueryable;
      
		private class scan_context {
			private class ctx_elem {
				public Type type;
				public string name;
			}
		
			private Stack<ctx_elem> _curentTypesChain;

			public scan_context() {
				_curentTypesChain = new Stack<ctx_elem>();
			}

			public string Name { get { string n = _curentTypesChain.Peek().name;  return n != "" ? n + "." : n; } }
			public Type Type { get { return _curentTypesChain.Peek().type; } }

			public bool pushContext(Type t, string name) {
				if(_curentTypesChain.Count == 0) {
					_curentTypesChain.Push(new ctx_elem { type = t, name = name });
				}
				else {
					if ((from e in _curentTypesChain where e.type == t select e).Count() > 0) return false; //cycle
					_curentTypesChain.Push(new ctx_elem { type = t, name = Name + name });
				}
				
				return true;				
			}

			public void popContext() {
				_curentTypesChain.Pop();
			}
		
		}

		private scan_context _ctx;

		public DataLayoutExtender(DataLayoutControl dest) {
			_customAttributes = new Dictionary<string, List<attributes.CustomAttribute>>();
			_cnt = dest;
			_cnt.AllowGeneratingNestedGroups = DevExpress.Utils.DefaultBoolean.True;
			_attributesLoaded = false;
			_ctx = new scan_context();


			_cnt.AutoRetrieveFields = true;
			_cnt.FieldRetrieving += (sender, e) =>
			{
                if (!_attributesLoaded)
                {
                    //read annotations
                    //here it depends what we have as DataSource, it can be Object, Type or IList Other we will ignore
                    BindingSource bs = _cnt.DataSource as BindingSource;
                    if (bs == null)
                    {
                        Console.WriteLine("Missing BindingSource for data layout");
                        return; // no valid binding arrived so we skip 
                    }
                    Type t = bs.DataSource as Type;
                    if (t == null)
                    {
                        //lets try another way, maby IList
                        IList someList = bs.DataSource as IList;
                        if (someList != null)
                        {
                            //try to obtain element type
                            t = someList.GetType().GetGenericArguments()[0];
                        }
                        else {
                            //it should be plain object and try to take type
                            if ((bs.DataSource as DataSet) == null &&
                               (bs.DataSource as DataTable) == null &&
                               (bs.DataSource as DataView) == null &&
                               (bs.DataSource as DataViewManager) == null &&
                               (bs.DataSource as object) != null
                            )
                            {
                                t = bs.DataSource.GetType();
                            }
                            else {
                                Console.WriteLine("Missing BindingSource for data layout");
                                return; // no valid binding arrived so we skip 
                            }
                        }
                    }

                    scanCustomAttributes(t, "");
                    _attributesLoaded = true;
                }
                applyRetrivingPhase(e);
                onFieldRetrieving(e);
                // fixed things
                e.DataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged;
                e.Handled = true;
            };
			_cnt.FieldRetrieved += (sender, e) =>
			{
				applyRetrivedPhase(e);
				onFieldRetrieved(e);
			};
		}
		
		public void onGetQueryable(GetFieldQueryableEventData qd)
		{
			GetFieldQueryable(this, qd);
		}

		public DataLayoutControl DataLayout
		{
			get
			{
				return _cnt;
			}
		}


		/* PRIVATE */
		private void scanCustomAttributes(Type t, string name) {
			// make context 
			if (!_ctx.pushContext(t, name)) return;	
		
			//handle eventual MetadataType annotation which will add annotations from surrogate object
			try {
				foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(
																  //take MetadataType properties
																  TypeDescriptor.GetAttributes(t)
																				.OfType<MetadataTypeAttribute>()
																				.Single()
																				.MetadataClassType
												  )
				){
					handleOneProperty(pd);
				}
			}catch(Exception ex) {
				Console.WriteLine(ex.Message);
			}
			
			//now own properties => these are later then those from surrogated so locals will do override
			// t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(t))
			{
				handleOneProperty(pd);
			}

			// remove one context level
			_ctx.popContext();
		}

		private void handleOneProperty(PropertyDescriptor t) {
			//we can have complex types
			if (t.PropertyType.FullName == "System.String"	|| t.PropertyType.IsPrimitive || t.PropertyType.FullName == "System.DateTime" || t.PropertyType.IsValueType)
            {
				string key = _ctx.Name + t.Name;
				Console.WriteLine("Examine attrs for : " + key);
				
				List<attributes.CustomAttribute> goodAtts = t.Attributes.OfType<attributes.CustomAttribute>().ToList();
					
				if (goodAtts.Count > 0) {
					Console.WriteLine("Attr for : " + key);
					//mix with existing attrs for that name
					if(_customAttributes.ContainsKey(key)) {
						_customAttributes[key].AddRange(goodAtts);	
					}
					else {
						_customAttributes.Add(key, goodAtts);
					}						
				}
			}
			else if(t.PropertyType.IsClass) //do recursion only for classes
			{
				scanCustomAttributes(t.PropertyType, t.Name); //here inside it will handle eventual cycles!!!
			}			
		}

		protected virtual void onFieldRetrieving(FieldRetrievingEventArgs e){}
		protected virtual void onFieldRetrieved(FieldRetrievedEventArgs e){}
		
		protected void applyRetrivingPhase(FieldRetrievingEventArgs e) {
			if(_customAttributes.ContainsKey(e.FieldName)){
				foreach (attributes.CustomAttribute a in _customAttributes[e.FieldName])
				{
					a.applyRetrievingAttribute(this, e);
				}
			}
		}

		protected void applyRetrivedPhase(FieldRetrievedEventArgs e)
		{

			if (_customAttributes.ContainsKey(e.FieldName))
			{
				foreach (attributes.CustomAttribute a in _customAttributes[e.FieldName])
				{
					a.applyRetrievedAttribute(this, e);
				}
			}
		}	
	}
}
