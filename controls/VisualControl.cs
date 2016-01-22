using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xwcs.core.ui.controls
{
    public partial class VisualControl : DevExpress.XtraEditors.XtraUserControl, xwcs.core.ui.controls.IControl
    {
        protected xwcs.core.ui.controls.ControlInfo _controlInfo;

        public ControlInfo controlInfo
        {
            get { return _controlInfo; }
        }
    }
}
