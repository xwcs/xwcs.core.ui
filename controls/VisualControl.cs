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
    public class VisualControl : DevExpress.XtraEditors.XtraUserControl, xwcs.core.controls.IVisualControl
    {
        private xwcs.core.controls.VisualControlInfo _visualControlInfo;

        public  xwcs.core.controls.VisualControlInfo VisualControlInfo { get { return _visualControlInfo; } set { _visualControlInfo = value; } }
    }
}
