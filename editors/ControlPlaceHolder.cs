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
	
    public interface HasEditValue
    {
        object EditValue { get; set; }
    }
    
    // this class will be used as custom editor, it will do parent editors host 
	// poxing, so all edits here will call main Editors host component
	// instead of local
	public partial class ControlPlaceHolder : XtraUserControl, INamedControl, IAnyControlEdit, IEditorsHostProvider//, IDataSourceProvider
    {
		public string ControlName { get; set; } // this will send to editors host for external component creation
        private Control _control;

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
                _control = _host.GetCustomEditingControl(ControlName);

                if (!ReferenceEquals(null, _control))
                {
                    Controls.Add(_control);
                    _control.Dock = DockStyle.Fill;
                }
            }
        }

       

		public ControlPlaceHolder()
		{
			InitializeComponent();
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

		public object EditValue
		{
			get
			{
                if(_control is HasEditValue)
				    return (_control as HasEditValue)?.EditValue;

                return null;
			}

			set
			{
                if (_control is HasEditValue && !ReferenceEquals(null, _control))
                {
                    (_control as HasEditValue).EditValue = value;
                    OnEditValueChanged();
                }                    
			}
		}

        public object DataSource
        {
            get
            {
                return null;
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
			return this.Size;
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
