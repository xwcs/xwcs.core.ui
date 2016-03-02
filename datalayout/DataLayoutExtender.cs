using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraDataLayout;
using System.ComponentModel.DataAnnotations;

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
        private Type _entityType;



		public DataLayoutExtender(DataLayoutControl dest, Type entityType) {
			_customAttributes = new Dictionary<string, List<attributes.CustomAttribute>>();
			_cnt = dest;
			_cnt.AllowGeneratingNestedGroups = DevExpress.Utils.DefaultBoolean.True;
			_attributesLoaded = false;
            _entityType = entityType;


            _cnt.AutoRetrieveFields = true;
			_cnt.FieldRetrieving += (sender, e) =>
			{
				if(!_attributesLoaded) {
                    //read annotations
                    //scanCustomAttributes((_cnt.DataSource as BindingSource).DataSource as Type, "");
                    scanCustomAttributes(_entityType, "");
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
		private void scanCustomAttributes(Type t, string nameContext) {
			//handle eventual MetadataType annotation which will add annotations from surrogate object
			try {
				MetadataTypeAttribute mt = t.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
											.Cast<MetadataTypeAttribute>()
											.Single();
				if (mt != null)
				{
					//we have MetadataType forwarding so handle it first
					Type metaType = mt.MetadataClassType;
					PropertyInfo[] mpis = metaType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
					foreach (PropertyInfo pi in mpis)
					{
						handleOneProperty(pi, nameContext);
					}
				}
			}catch(Exception ex) {
				Console.WriteLine(ex.Message);
			}
			
			//now own properties => these are later then those from surrogated so locals will do override
			PropertyInfo[] pis = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo pi in pis)
			{
				handleOneProperty(pi, nameContext);
			}
		}


		private void handleOneProperty(System.Reflection.PropertyInfo pi, string nameContext) {
			//we can have complex types
			if (pi.PropertyType.FullName == "System.String"	|| pi.PropertyType.IsPrimitive || pi.PropertyType.FullName == "System.DateTime" || pi.PropertyType.IsValueType)
            {
				if (pi != null)
				{
					string key = nameContext + pi.Name;
					Console.WriteLine("Examine attrs for : " + key);

					List<attributes.CustomAttribute> goodAtts = pi.GetCustomAttributes(typeof(attributes.CustomAttribute), true)
																		  .Cast<attributes.CustomAttribute>()
																		  .ToList();
					
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
			}
			else if(pi.PropertyType.IsClass) //do recursion only for classes
			{
				scanCustomAttributes(pi.PropertyType, pi.Name + ".");
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
