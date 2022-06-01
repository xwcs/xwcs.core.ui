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
    public partial class EntityEditForm<ModelType, FormControlType> : DevExpress.XtraEditors.XtraForm 
        where ModelType : BindableObjectBase, new()
        where FormControlType: EntityEditControl<ModelType>

    {
        //model
        private ModelType _model;
        private FormControlType _eec;
        public EntityEditForm(ModelType model =null)
        {
            _model = model == null ? new ModelType() : model;
            xwcs.core.evt.SEventProxy.InvokeDelegate = this;
            InitializeComponent();
            _eec = (FormControlType)Activator.CreateInstance(typeof (FormControlType), new object[] { model });
            _eec.EditDone += _eec_EditDone;
            _eec.EditCancel += _eec_EditCancel;
            Controls.Add(_eec);
            _eec.Dock = DockStyle.Fill;
            var descriptions = (DescriptionAttribute[])
                        model.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptions.Any())
            {
                this.Text = descriptions.First().Description??_model.GetType().Name;
            } else {
                this.Text = _model.GetType().Name;
            }
            /*
            if (!_model.IsValid())
            {
                this._eec.DisableDone();
                this._model.PropertyChanged += _model_PropertyChanged;
            }
            */
        }

        private void _eec_EditCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void _eec_EditDone(object sender, EventArgs e)
        {
            
            if (_model.IsValid()) { 
                DialogResult = DialogResult.OK;
                Close();
            }/*
            else
            {
                this._eec.DisableDone();
                this._model.PropertyChanged += _model_PropertyChanged;
            }*/
        }
        /*
        private void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this._eec.EnableDone();
            this._model.PropertyChanged -= _model_PropertyChanged;
        }
        */
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

        private void EntityEditForm_Load(object sender, EventArgs e)
        {

        }
    }

}