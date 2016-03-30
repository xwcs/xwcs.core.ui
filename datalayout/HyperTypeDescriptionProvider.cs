namespace Hyper.ComponentModel
{
    using System;
	using System.Linq;
	using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Permissions;
	using System.ComponentModel.DataAnnotations;




	public sealed class HyperTypeDescriptionProvider : TypeDescriptionProvider
    {
		private xwcs.core.manager.ILogger _logger;

		public static void MorphPropertyType(object obj, string propName)
		{
			try
			{
				Type objectType = obj.GetType();
				PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(objectType);
				ChainingPropertyDescriptor pd = pdc.Find(propName, false) as ChainingPropertyDescriptor;
				if (pd != null)
				{
					object val = pd.GetValue(obj);
                    pd.ForcedPropertyType = val.GetType();
				}

				//now for EF proxy too
				if (objectType.BaseType != null && objectType.Namespace == "System.Data.Entity.DynamicProxies")
				{
					objectType = objectType.BaseType;

					pdc = TypeDescriptor.GetProperties(objectType);
					pd = pdc.Find(propName, false) as ChainingPropertyDescriptor;
					if (pd != null)
					{
						pd.ForcedPropertyType = pd.GetValue(obj).GetType();
					}
				}

				
			}
			catch (Exception) { }

		}

		public static void Add(Type type)
        {
			//Avoid some base types!!!!!!
			if (type.FullName == "System.Object" ||
				type.FullName == "System.String" ||
				type.FullName == "System.DateTime" ||
				type.IsPrimitive ||
				type.IsValueType ||
                !type.IsClass
				)
				return;
			
			// TODO: make smarter filter	
			TypeDescriptionProvider parent = TypeDescriptor.GetProvider(type);
            TypeDescriptor.AddProvider(new HyperTypeDescriptionProvider(parent), type);
        }

        public HyperTypeDescriptionProvider() : this(typeof (object))
        {
			_examinedTypes = new HashSet<string>();
			_logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(GetType());
		}

        public HyperTypeDescriptionProvider(Type type) : this(TypeDescriptor.GetProvider(type))
        {
			_examinedTypes = new HashSet<string>();
			_logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(GetType());
		}

        public HyperTypeDescriptionProvider(TypeDescriptionProvider parent) : base(parent)
        {
			_examinedTypes = new HashSet<string>();
			_logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(GetType());
		}

        public static void Clear(Type type)
        {
            lock (descriptors)
            {
                descriptors.Remove(type);
            }
        }

        public static void Clear()
        {
            lock (descriptors)
            {
                descriptors.Clear();
            }
        }

        private static readonly Dictionary<Type, ICustomTypeDescriptor> descriptors =
            new Dictionary<Type, ICustomTypeDescriptor>();

        public override sealed ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
			lock (descriptors)
            {
                ICustomTypeDescriptor descriptor;
                if (!descriptors.TryGetValue(objectType, out descriptor))
                {
                    try
                    {
                        descriptor = BuildDescriptor(objectType);
                    }
                    catch
                    {
                        return base.GetTypeDescriptor(objectType, instance);
                    }
                }
                return descriptor;
            }
        }

		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
        private ICustomTypeDescriptor BuildDescriptor(Type objectType)
        {
            // NOTE: "descriptors" already locked here

            // get the parent descriptor and add to the dictionary so that
            // building the new descriptor will use the base rather than recursing
            ICustomTypeDescriptor descriptor = base.GetTypeDescriptor(objectType, null);
            descriptors.Add(objectType, descriptor);
            try
            {
                // build a new descriptor from this, and replace the lookup
                descriptor = new HyperTypeDescriptor(descriptor, objectType);
                descriptors[objectType] = descriptor;
                return descriptor;
            }
            catch
            {
                // rollback and throw
                // (perhaps because the specific caller lacked permissions;
                // another caller may be successful)
                descriptors.Remove(objectType);
                throw;
            }
        }

		private class attributes_cache {
			private Dictionary<string, HashSet<xwcs.core.ui.datalayout.attributes.CustomAttribute>> _attrs;

			public void add(string propName, HashSet<xwcs.core.ui.datalayout.attributes.CustomAttribute> atts) {
				if (atts.Count > 0)
				{
					if (_attrs.ContainsKey(propName))
					{
						_attrs[propName].UnionWith(atts);
					}
					else
					{
						_attrs.Add(propName, atts);
					}
				}
			}
		}

		/*
			attributes scan
		*/

		private Dictionary<string, attributes_cache> _customAttributes;
		private HashSet<string> _examinedTypes;
		private scan_context _ctx;

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
				string key = _ctx.Type.Name + ":" + _ctx.Name + t.Name;
				_logger.Debug("Examine attrs for : " + key);

				HashSet< xwcs.core.ui.datalayout.attributes.CustomAttribute> goodAtts = new HashSet<xwcs.core.ui.datalayout.attributes.CustomAttribute>(t.Attributes.OfType<xwcs.core.ui.datalayout.attributes.CustomAttribute>().ToList());

				if (goodAtts.Count > 0)
				{
					_logger.Debug("Attr for : " + key);
					//mix with existing attrs for that name
					if (_customAttributes.ContainsKey(key))
					{
						//_customAttributes[key].UnionWith(goodAtts);
					}
					else
					{
						//_customAttributes.Add(key, goodAtts);
					}
				}
			}
			else if (t.PropertyType.IsClass) //do recursion only for classes
			{
				// morph able eventuality
				try
				{
					List<xwcs.core.ui.datalayout.attributes.PolymorphFlag> pmfl = t.Attributes.OfType<xwcs.core.ui.datalayout.attributes.PolymorphFlag>().ToList();
					ChainingPropertyDescriptor cd = t as ChainingPropertyDescriptor;
					if (pmfl.Count > 0 && cd != null)
					{
						//_morphablePaths.Add(_ctx.Name + t.Name, new morphable_context { name = t.Name, ctx = new scan_context(_ctx), currentObjectType = null, pd = cd, attribute = pmfl[0] });
					}
				}
				catch (Exception) { }

				_logger.Debug("Going into:" + _ctx.Name + t.Name);
				scanCustomAttributes(t.PropertyType, t.Name); //here inside it will handle eventual cycles!!!
			}
		}

	}
}