using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public bool HighlightEditedField { get; set; } = false;

        private StyleController _ModifiedStyle  { get; set; } = new StyleController();

        public FormSupport()
        {
            //default modified state
            _ModifiedStyle.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            _ModifiedStyle.LookAndFeel.UseDefaultLookAndFeel = false;
            _ModifiedStyle.Appearance.BackColor = Color.FromArgb(230, 230, 190);
        }

        private Dictionary<BaseEdit, IStyleController> _DefaultStyles = new Dictionary<BaseEdit, IStyleController>();
        public Dictionary<BaseEdit, IStyleController> DefaultStyles
        {
            get
            {
                return _DefaultStyles;
            }
        }

		private Dictionary<BaseEdit, ControlMeta> _ControlsMeta = new Dictionary<BaseEdit, ControlMeta>();
		public Dictionary<BaseEdit, ControlMeta> ControlsMeta
		{
			get
			{
				return _ControlsMeta;
			}
		}

		public void AddBindingSource(IDataBindingSource bs)
        {

            // If some property in model was chnaged, this can be broken in case there is child collection with proper BindingSource
            if ((bs as INotifyModelPropertyChanged) != null)
                (bs as INotifyModelPropertyChanged).ModelPropertyChanged += handle_bindingSource_ModelPropertyChanged;

            // This event is fired if BindingSource change currento object to other one, not when object content change!!
            if ((bs as INotifyCurrentObjectChanged) != null)
                (bs as INotifyCurrentObjectChanged).CurrentObjectChanged += handle_bindingSource_CurrentObjectChanged;
            _bindingSources.Add(bs);
        }

        private void handle_bindingSource_CurrentObjectChanged(object sender, CurrentObjectChangedEventArgs e)
        {
#if DEBUG_TRACE_LOG_ON
            _logger.Debug(string.Format("Form support fire triggers at start"));
#endif

            IModelEntity me = e.Current as IModelEntity;

            if (ReferenceEquals(null, me))
                return; // not correct object for execution 
            
            // check if there is trigger
            _triggers.AllTriggerLists().ForEach(
                l => l.ForEach(
                    o =>
                    {
                        //tt(o, me, e)
                        object v = me.GetModelPropertyValueByName(o.FieldName);
                        if (!ReferenceEquals(null, v))
                        {
                            _actions[o.ActionType].ForEach(a => FireAction(
                                o,
                                a,
                                e.Current,
                                new ModelPropertyChangedEventArgs(
                                    o.FieldName,
                                    new ModelPropertyChangedEventArgs.PropertyChangedChainEntry()
                                    {
                                        Container = e.Current,
                                        PropertyName = o.FieldName,
                                        Value = v
                                    }
                                )
                            ));
                        }
                    }
            ));
        }

        

        private void handle_bindingSource_ModelPropertyChanged(object sender, ModelPropertyChangedEventArgs e)
        {
#if DEBUG_TRACE_LOG_ON
            _logger.Debug(string.Format("Form support Model Property: {0} changed in [{1}]", e, (e.PropertyChain[0].Container as FilterObjectbase)?.GetType().Name));
#endif

            // disable layout
            _bindingSources.ForEach(bs => bs.SuspendLayout());

            if (e.HasWildCharInName())
            {
                _triggers.AllTriggerListsByPattern(e.ToRegExp()).ForEach(
                        // we have eventual list of triggers lists
                        t => t.ForEach(
                         o =>
                         _actions[o.ActionType].ForEach(a => FireAction(o, a, sender, e))
                 ));
            }
            else
            {
                // check if there is trigger
                _triggers[e.ToString()].ForEach(
                        o =>
                        _actions[o.ActionType].ForEach(a => FireAction(o, a, sender, e))
                );
            }

            // handle eventually editbox coloring in case edited value changed
            if (HighlightEditedField && e.ChangeKind != ModelPropertyChangedEventKind.Reset)
            {
                TextEdit cnt = FindControlByPropertyName(e.ToString()) as TextEdit;
                if(!ReferenceEquals(null, cnt))
                {
                    cnt.StyleController = _ModifiedStyle;
                }
            }
            
            // enable layout
            _bindingSources.ForEach(bs => bs.ResumeLayout());
        }

        // handle specific action if trigger was called
        private void FireAction(DynamicFormActionTrigger trigger, DynamicFormAction action,  object sender, ModelPropertyChangedEventArgs e)
        {
#if DEBUG_TRACE_LOG_ON

            _logger.Debug(string.Format("Form support Trigger: {0} for {1} with value {2} exec -> {3}", trigger.ActionType, trigger.FieldName, e.Value?.ToString(), action.FieldName));
#endif

            // handle execution of actions
            switch (trigger.ActionType)
            {
                case DynamicFormActionType.MaskedEnable:
                    {
                        MaskedEnable_FireAction(trigger, action, sender, e);
                        break;
                    }
                case DynamicFormActionType.MaskedVisible:
                    {
                        MaskedVisible_FireAction(trigger, action, sender, e);
                        break;
                    }
                default:

                    // skip unknown action
#if DEBUG_TRACE_LOG_ON

                    _logger.Error(string.Format("Form support unknown action! {1}", trigger.ActionType));
#endif
                    break;
            }
        }

        public void RegisterAction(DynamicFormAction a)
        {
#if DEBUG_TRACE_LOG_ON
            _logger.Debug(string.Format("Dynamic action registered {0} - {1} [{2}]", a.ActionType, a.FieldName, a.Param));
#endif
            _actions[a.ActionType].Add(a);
        }

        public void RegisterActionTrigger(DynamicFormActionTrigger a)
        {
#if DEBUG_TRACE_LOG_ON
            _logger.Debug(string.Format("Dynamic action trigger registered {0} - {1} [{2}]", a.ActionType, a.FieldName, a.Param));
#endif
            _triggers[a.FieldName].Add(a);
        }

        public Control FindControlByPropertyName(string name)
        {
            foreach(IDataBindingSource bs in _bindingSources)
            {
                try
                {
                    Control cnt = bs.GetControlByModelProperty(name);
                    if (!ReferenceEquals(null, cnt))
                    {
                        // found so stop search
                        return cnt;
                    }
                }catch(Exception ex)
                {
                    _logger.Error(string.Format("Form support unknown action! {1}", ex.Message));
                }
                
            }

            return null;
        }

        private void MaskedEnable_FireAction(DynamicFormActionTrigger trigger, DynamicFormAction action, object sender, ModelPropertyChangedEventArgs e)
        {
            // find all controls and set enabled / disabled state using proper mask
            Type enumType = trigger.Param as Type;
            if(enumType == null )
            {
                // wrong settings
                _logger.Error(string.Format("MaskedEnable_FireAction: Wrong mask enum type!"));
                return;
            }

            int mask = 0;

            try
            {
                mask = ExtendedEnum.ToInt(enumType, e.Value); //e.Value != null && e.Value.ToString() != "" ? (int)System.Enum.Parse(enumType, e.Value.ToString(), true) : 0;
            }
            catch (Exception)
            {
                //_logger.Error(string.Format("MaskedEnable_FireAction: Wrong mask enum value [{0}]!", e.Value.ToString()));
            }
            

            // get control
            if(ReferenceEquals(null, action.Control))
            {
                action.Control = FindControlByPropertyName(action.FieldName);
            }

            if (!ReferenceEquals(null, action.Control))
            {
                // reset data using binding source of this action
                FilterObjectbase fo = action.BindingSource?.Current as FilterObjectbase;
                if(!ReferenceEquals(fo, null))
                {
                    action.Control.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.Never;
                    // reset field
                    fo.ResetFieldByName(action.FieldName);
                    // turn back DS update mode
                    action.Control.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                }
                // confront field bits with enable mask arrived from trigger
                // control remain enable only if all mask (trigger) bits are present allow bitset of field (action target)
                action.Control.Enabled = (((int)(object)action.Param & (int)(object)mask) == (int)(object)mask);
                // set back defautl style
                if(action.Control is TextEdit)
                {
                    (action.Control as TextEdit).StyleController = DefaultStyles.ContainsKey((action.Control as TextEdit)) ? DefaultStyles[(action.Control as TextEdit)] : null;
                }
                
            }
        }

        private void MaskedVisible_FireAction(DynamicFormActionTrigger trigger, DynamicFormAction action, object sender, ModelPropertyChangedEventArgs e)
        {
            
        }
    }
}
