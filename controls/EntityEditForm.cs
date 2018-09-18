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
using xwcs.core.db;

namespace xwcs.core.ui.controls
{
    public partial class EntityEditForm<ModelType> : DevExpress.XtraEditors.XtraForm where ModelType : BindableObjectBase, new()
	{
        //model
        private ModelType _model;
        private xwcs.core.ui.controls.EntityEditControl<ModelType> _eec;
        public EntityEditForm(ModelType model =null)
        {
            _model = model == null ? new ModelType() : model;
            xwcs.core.evt.SEventProxy.InvokeDelegate = this;
            InitializeComponent();
            _eec = new xwcs.core.ui.controls.EntityEditControl<ModelType>(_model);
            _eec.EditDone += _eec_EditDone;
            _eec.EditCancel += _eec_EditCancel;
            Controls.Add(_eec);
            _eec.Dock = DockStyle.Fill;

        }

        private void _eec_EditCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void _eec_EditDone(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eec.EditDone -= _eec_EditDone;
                _eec.EditCancel -= _eec_EditCancel;
                if (components != null)
                { 
                    components.Dispose();
                }
                xwcs.core.manager.SLogManager.getInstance().Dispose();
            }

            base.Dispose(disposing);
        }
    }

}