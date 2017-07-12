using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xwcs.core.evt;
using xwcs.core.manager;

namespace xwcs.core.ui.db.fo
{
    public partial class FilterOptionsForm : DevExpress.XtraEditors.XtraForm
    {
        private xwcs.core.db.fo.FilterOptions _fo;
        private xwcs.core.ui.db.fo.FilterOptionsControl _foc;

        public FilterOptionsForm(xwcs.core.db.fo.FilterOptions opts = null)
        {
            SEventProxy.InvokeDelegate = this;

            _fo = opts == null ? new core.db.fo.FilterOptions() : opts;
            InitializeComponent();
            
            _foc = new xwcs.core.ui.db.fo.FilterOptionsControl(_fo);
            _foc.OptionsDone += _foc_OptionsDone;
            Controls.Add(_foc);
            _foc.Dock = DockStyle.Fill;
        }

        private void _foc_OptionsDone(object sender, EventArgs e)
        {
            //MessageBox.Show(string.Format("Query: {0},  Options: {1}", _fo.ToQueryString(), _fo.ToString()));
           Hide();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // kill loggers
                SLogManager.getInstance().Dispose();
                if (_foc != null) _foc.Dispose();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}