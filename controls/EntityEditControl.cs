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
using xwcs.core.db;
using xwcs.core.db.binding;
using xwcs.core.manager;
using xwcs.core.evt;
using xwcs.core.ui.db;
using DevExpress.Utils.Behaviors;
using DevExpress.XtraEditors.Controls;

namespace xwcs.core.ui.controls
{
    public partial class EntityEditControl<ModelType> : DevExpress.XtraEditors.XtraUserControl, xwcs.core.db.binding.IEditorsHost, IBehaviorContainer
				where ModelType : BindableObjectBase
	{
        //model
        private ModelType _model;
        private xwcs.core.db.binding.DataLayoutBindingSource _formBindingSource = null;
        private string _LayoutAssetPath = "";
        private FormSupport _formSupport;

        public EntityEditControl(ModelType model)
        {
            _LayoutAssetPath = SPersistenceManager.GetDefaultAssetsPath(SPersistenceManager.AssetKind.Layout, GetType());
            _model = model;
            _formSupport = new FormSupport(this);
            // editing form will not highlight edited field
            _formSupport.HighlightEditedField = false;
            InitializeComponent();
            simpleButtonCancel.CausesValidation = false;
            simpleButtonCancel.AllowFocus = false;
            simpleButtonOk.AllowFocus = false;
            simpleButtonOk.Click += SimpleButtonOk_Click;
            simpleButtonCancel.Click += SimpleButtonCancel_Click;
            _formBindingSource = new xwcs.core.db.binding.DataLayoutBindingSource(this);
            _formBindingSource.LayoutContainer = mainContainer;
            _formBindingSource.DataSource = _model;
            _formBindingSource.LayoutBaseFileName = string.Format("EditControl_{0}", _model.GetType().Name);
            
            //dxErrorProvider.DataSource = _formBindingSource;

            UpdateLayout();
        }

        private void SimpleButtonCancel_Click(object sender, EventArgs e)
        {
            _wes_EditCancel?.Raise(this, new EventArgs());
        }

        private void UpdateLayout()
        {
            _formBindingSource.ChangeLayout("_Default");
        }

        private void SimpleButtonOk_Click(object sender, EventArgs e)
        {
            if (_model.IsValid())
            {
                _wes_EditDone?.Raise(this, new EventArgs());
            } else
            {
                string sRet = "";
                foreach (var vr in _model.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(_model)).Cast<Problem>())
                {
                    if (vr.Kind != ProblemKind.None)
                    {
                        sRet = sRet + "\n" + vr.ErrorMessage;
                    }
                }
                MessageBox.Show(sRet);
            }
        }

        private WeakEventSource<EventArgs> _wes_EditDone = null;
        public event EventHandler<EventArgs> EditDone
        {
            add
            {
                if (_wes_EditDone == null)
                {
                    _wes_EditDone = new WeakEventSource<EventArgs>();
                }
                _wes_EditDone.Subscribe(value);
            }
            remove
            {
                _wes_EditDone?.Unsubscribe(value);
            }
        }
        private WeakEventSource<EventArgs> _wes_EditCancel = null;
        public event EventHandler<EventArgs> EditCancel
        {
            add
            {
                if (_wes_EditCancel == null)
                {
                    _wes_EditCancel = new WeakEventSource<EventArgs>();
                }
                _wes_EditCancel.Subscribe(value);
            }
            remove
            {
                _wes_EditCancel?.Unsubscribe(value);
            }
        }

        public DBContextBase DataCtx
        {
            get
            {
                return null;
            }
        }

        public IFormSupport FormSupport
        {
            get
            {
                return _formSupport;
            }
        }

        public string LayoutAssetsPath
        {
            get
            {
                return _LayoutAssetPath;
            }
        }

		public BehaviorManager BehaviorMan
		{
			get
			{
				return behaviorManager;
			}
		}

		public Control GetCustomEditingControl(string ControlName)
        {
            return null;
        }

        public void onGetFieldDisplayText(object sender, CustomColumnDisplayTextEventArgs cc)
        {
        }

        public void onGetOptionsList(object sender, GetFieldOptionsListEventData qd)
        {
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                simpleButtonOk.Click -= SimpleButtonOk_Click;
                simpleButtonCancel.Click -= SimpleButtonCancel_Click;
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void onGridConnected(object sender, GridConnectedEventData data)
        {
            Console.WriteLine("Grid connected for : " + data.DataType.FullName);
        }

        
        public void onSetupLookUpGridEventData(object sender, SetupLookUpGridEventData data)
        {
            //only for imlpement of interface
        }

        public void onButtonEditClick(object sender, ButtonPressedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
