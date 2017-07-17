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

namespace xwcs.core.ui.db.fo
{
    public partial class FilterOptionsControl : DevExpress.XtraEditors.XtraUserControl, core.db.binding.IEditorsHost
    {
        //model
        private core.db.fo.FilterOptions _fo;

        private xwcs.core.db.binding.DataLayoutBindingSource _formBindingSource = null;
        private string _LayoutAssetPath = "";
        private FormSupport _formSupport;

        public FilterOptionsControl(core.db.fo.FilterOptions fo)
        {
            _LayoutAssetPath = SPersistenceManager.GetDefaultAssetsPath(SPersistenceManager.AssetKind.Layout, GetType());
            _fo = fo;
            _formSupport = new FormSupport();
            // editing form will not highlight edited field
            _formSupport.HighlightEditedField = false;
            InitializeComponent();

            simpleButtonOk.Click += SimpleButtonOk_Click;

            _formBindingSource = new xwcs.core.db.binding.DataLayoutBindingSource(this);
            _formBindingSource.LayoutContainer = mainContainer;
            _formBindingSource.DataSource = _fo;
            _formBindingSource.LayoutBaseFileName = "FilterOptions";
            
            //dxErrorProvider.DataSource = _formBindingSource;

            UpdateLayout();
        }

        
        private void UpdateLayout()
        {
            _formBindingSource.ChangeLayout("_Default");
        }

        private void SimpleButtonOk_Click(object sender, EventArgs e)
        {
            if (_fo.IsValid())
            {
                _wes_OptionsDone?.Raise(this, new EventArgs());
            }
        }

        private WeakEventSource<EventArgs> _wes_OptionsDone = null;
        public event EventHandler<EventArgs> OptionsDone
        {
            add
            {
                if (_wes_OptionsDone == null)
                {
                    _wes_OptionsDone = new WeakEventSource<EventArgs>();
                }
                _wes_OptionsDone.Subscribe(value);
            }
            remove
            {
                _wes_OptionsDone?.Unsubscribe(value);
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
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
