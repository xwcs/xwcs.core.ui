using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db;
using xwcs.core.db.binding;
using xwcs.core.db.fo;
using xwcs.core.db.model;

namespace xwcs.core.ui.db
{


    public class FormSupport : IFormSupport
    {
        protected static manager.ILogger _logger = manager.SLogManager.getInstance().getClassLogger(typeof(FormSupport));

        private DynamicFormActions _actions = new DynamicFormActions();
        private DynamicFormActionTriggers _triggers = new DynamicFormActionTriggers();
        private List<IDataBindingSource> _bindingSources = new List<IDataBindingSource>();

        public void AddBindingSource(IDataBindingSource bs)
        {
            if ((bs as INotifyModelPropertyChanged) != null)
                (bs as INotifyModelPropertyChanged).ModelPropertyChanged += handle_bindingSource_ModelPropertyChanged;
            _bindingSources.Add(bs);
        }

        private void handle_bindingSource_ModelPropertyChanged(object sender, ModelPropertyChangedEventArgs e)
        {
#if DEBUG
            _logger.Debug(string.Format("Form support Model Property: {0} changed in [{1}]", e, (e.PropertyChain[0].Container as FilterObjectbase)?.GetType().Name));
#endif

            // check if there is trigger
            _triggers[e.ToString()].ForEach(o => FireAction(o, sender, e));
        }

        // handle specific action if trigger was called
        private void FireAction(DynamicFormActionTrigger trigger, object sender, ModelPropertyChangedEventArgs e)
        {
            object val = e.Value;
            string vals = "";

            if (val.GetType().IsSubclassOfRawGeneric(typeof(FilterField<>)))
            {
                //vals = (val as FilterField<object>).Value.ToString();
            }
#if DEBUG

            _logger.Debug(string.Format("Form support Trigger: {0} for {1} with value {2}", trigger.ActionType, trigger.FieldName, vals));
#endif
        }

        public void RegisterAction(DynamicFormAction a)
        {
#if DEBUG
            _logger.Debug(string.Format("Dynamic action registered {0} - {1} [{2}]", a.ActionType, a.FieldName, a.Param));
#endif
            _actions[a.ActionType].Add(a);
        }

        public void RegisterActionTrigger(DynamicFormActionTrigger a)
        {
#if DEBUG
            _logger.Debug(string.Format("Dynamic action trigger registered {0} - {1} [{2}]", a.ActionType, a.FieldName, a.Param));
#endif
            _triggers[a.FieldName].Add(a);
        }
    }
}
