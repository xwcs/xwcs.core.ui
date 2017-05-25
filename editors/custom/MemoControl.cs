using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace xwcs.core.ui.editors.custom
{
    public partial class MemoControl : DevExpress.XtraEditors.XtraUserControl, HasEditValue
    {
        public object EditValue { get; set; }

        public MemoControl()
        {
            InitializeComponent();
            memoEdit1.DataBindings.Add("EditValue", this, "EditValue", true);
        }
    }
}
