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
using xwcs.core.db.binding;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using System.Collections;
using xwcs.core.manager;
using DevExpress.XtraEditors.CustomEditor;
using DevExpress.Utils.Drawing;
using xwcs.core.evt;
using xwcs.core.ui.db.fo;

namespace xwcs.core.ui.editors
{
	// this class will be used as custom editor, it will do parent editors host 
	// poxing, so all edits here will call main Editors host component
	// instead of local
	public partial class ControlPlaceHolder : XtraUserControl, INamedControl, IAnyControlEdit, IEditorsHostProvider, IDataSourceProvider
    {
		private object _val = null;
        public string ControlName { get; set; } // this will send to editors host for external component creation
        private IEditorsHost _host;
        public IEditorsHost EditorsHost {
            get
            {
                return _host;
            }

            set
            {
                if (value == null) return;
                _host = value;
                // add custom control here
                Control tmp = _host.GetCustomEditingControl(ControlName);

                if (!ReferenceEquals(null, tmp))
                {
                    RecommendedSize = tmp.Size;
                    Controls.Add(tmp);
                }
            }
        }

       

		public ControlPlaceHolder()
		{
			InitializeComponent();

            

            RecommendedSize = new Size(0,75);

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
		
			base.Dispose(disposing);
		}

		public Size RecommendedSize { get; set; }

        


        public object EditValue
		{
			get
			{
				return _val;
			}

			set
			{
				if (_val == value) return;
				
				if(value != null) {
					_val = value;
				}
			}
		}

        public object DataSource
        {
            get
            {
                return _val;
            }

            set
            {
                if (_val == value) return;

                if (value != null)
                {
                    _val = value;
                }
            }
        }

        //public event EventHandler EditValueChanged;
        private readonly WeakEventSource<EventArgs> _wes_EditValueChanged = new WeakEventSource<EventArgs>();
		public event EventHandler EditValueChanged
		{
			add { _wes_EditValueChanged.Subscribe(new EventHandler<EventArgs>(value)); }
			remove { _wes_EditValueChanged.Unsubscribe(new EventHandler<EventArgs>(value)); }
		}
		

		private void OnEditValueChanged() {
			_wes_EditValueChanged.Raise(this, new EventArgs());
		}

		public Size CalcSize(Graphics g)
		{
			return RecommendedSize;
		}

		public void Draw(GraphicsCache cache, AnyControlEditViewInfo viewInfo){}
		public void SetupAsDrawControl(){}
		public void SetupAsEditControl(){}

		public string GetDisplayText(object EditValue)
		{
			return RepositoryItemAnyControl.GetBasicDisplayText(EditValue);
		}

		public bool IsNeededKey(KeyEventArgs e)
		{
			return false;
		}

		public bool AllowClick(Point point)
		{
			return true;
		}
		public bool SupportsDraw
		{
			get
			{
				return false;
			}
		}

		public bool AllowBorder
		{
			get
			{
				return false;
			}
		}

		public bool AllowBitmapCache
		{
			get
			{
				return true;
			}
		}
	}
    
}
