using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db.binding;

namespace xwcs.core.ui.db
{


    public class FormSupport : IFormSupport
    {
        protected static manager.ILogger _logger = manager.SLogManager.getInstance().getClassLogger(typeof(FormSupport));

        private DynamicFormActions _actions = new DynamicFormActions();
        
        public void RegisterAction(DynamicFormActionType t, DynamicFormAction a)
        {
#if DEBUG
            _logger.Debug(string.Format("Dynamic action registered {0} - {1} [{2}]", t, a.FieldName, a.Param));
#endif
            _actions[t].Add(a);
        }
    }
}
