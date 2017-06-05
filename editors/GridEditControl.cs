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
	public partial class GridEditControl : XtraUserControl, IAnyControlEdit, IEditorsHostProvider, IDataSourceProvider
    {
		private object _val = null;
		private FilterGridBindingSource _bs;
		private GridViewCustomMenu _gcm;
		public IEditorsHost EditorsHost { get; set; }

       

		public GridEditControl()
		{
			InitializeComponent();
			gridViewMain.OptionsView.ShowGroupPanel = false;
			gridViewMain.PopupMenuShowing += gridViewMain_PopupMenuShowing;
		}

        private void _bs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType != ListChangedType.PropertyDescriptorChanged) return;
            InvalidateValue();
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

			gridViewMain.PopupMenuShowing -= gridViewMain_PopupMenuShowing;
            if(!ReferenceEquals(null, _bs)){
                _bs.ListChanged -= _bs_ListChanged;
                _bs.Dispose();
                _bs = null;
            }
			base.Dispose(disposing);
		}

		
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
				}else {
					//Clear data source
					_bs.Clear();

                    // notify value changed
                    InvalidateValue();
                    return;
				}
				
				// changed content
				if(_val != null) {
					if(_bs != null) {
						_bs.Dispose();
						#if DEBUG_TRACE_LOG_ON
						SLogManager.getInstance().Info("reset grid");
						#endif
					}
					_bs = new FilterGridBindingSource(EditorsHost, barManager);
                    _bs.ListChanged += _bs_ListChanged;
                    _bs.AttachToGrid(gridControl);
					_bs.DataSource = _val;

                    // notify value changed
                    InvalidateValue();
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
                else
                {
                    //Clear data source
                    _bs.Clear();
                    
                    // notify value changed
                    InvalidateValue();
                    
                    return;
                }

                // changed content
                if (_val != null)
                {
                    if (_bs != null)
                    {
                        _bs.Dispose();
#if DEBUG_TRACE_LOG_ON
                        SLogManager.getInstance().Info("reset grid");
#endif
                    }
                    _bs = new FilterGridBindingSource(EditorsHost, barManager);
                    _bs.ListChanged += _bs_ListChanged;
                    _bs.AttachToGrid(gridControl);
                    _bs.DataSource = _val;

                    // notify value changed
                    InvalidateValue();
                }
            }
        }

        //public event EventHandler EditValueChanged;
        private readonly WeakEventSource<EventArgs> _wes_EditValueChanged = new WeakEventSource<EventArgs>();
		public event EventHandler EditValueChanged
		{
			add { _wes_EditValueChanged.Subscribe(value); }
			remove { _wes_EditValueChanged.Unsubscribe(value); }
		}
		

		private void OnEditValueChanged() {
			_wes_EditValueChanged.Raise(this, new EventArgs());
		}

		public void addRow() {
			_bs.AddNew();
			_gcm.Dispose();
			_gcm = null;
		}

		public void remRow()
		{
			_bs.RemoveCurrent();
			_gcm.Dispose();
			_gcm = null;
		}

		private void gridViewMain_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
		{
			if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.EmptyRow ||
				e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell
			)
			{
				GridView view = sender as GridView;
				view.FocusedRowHandle = e.HitInfo.RowHandle;
				_gcm = new GridViewCustomMenu(this, view,
					e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell ?
					PopupMenyType.addRem :
					PopupMenyType.justAdd
				);
				_gcm.Init(e.HitInfo);
				_gcm.Show(e.Point);
			}
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

        private bool _invaidatingValue = false;
        protected void InvalidateValue(bool arrivedFromSize = false)
        {
            if (_invaidatingValue) return;
            _invaidatingValue = true;
            OnEditValueChanged();
            _invaidatingValue = false;
        }
        protected override void SetBoundsCore(
            int x,
            int y,
            int width,
            int height,
            BoundsSpecified specified
        )
        {
            base.SetBoundsCore(x, y, width, height, specified);
            InvalidateValue(true);
        }
    }

	public enum PopupMenyType {
		justAdd,
		addRem
	}

	public class GridViewCustomMenu : GridViewMenu
	{
		private PopupMenyType _mt;	

		//Grid
		private GridEditControl _gec;
		
		//handlers
		private EventHandler eh_addItemClick;
		private EventHandler eh_remItemClick;

		//menu items
		private DXMenuItem addItem;
		private DXMenuItem remItem;



		public GridViewCustomMenu(GridEditControl gec, GridView view, PopupMenyType menuType) : base(view) {

			_mt = menuType;
			_gec = gec;
			//handlers
			eh_addItemClick = (s, e) => {
				_gec.addRow();
			};

			eh_remItemClick = (s, e) =>
			{
				_gec.remRow();
			};

			//items
			addItem = new DXMenuItem("Add");
			addItem.Click += eh_addItemClick;

			if(menuType == PopupMenyType.addRem) {
				remItem = new DXMenuItem("Del");
				remItem.Click += eh_remItemClick;
			}
		}
		// Create menu items. 
		// This method is automatically called by the menu's public Init method. 
		protected override void CreateItems()
		{
			Items.Clear();
			Items.Add(addItem);
			if(_mt == PopupMenyType.addRem) {
				Items.Add(remItem);
			}
		}

		protected override void Dispose(bool disposing)
		{
			Items.Clear();	
			addItem.Click -= eh_addItemClick;
			if (remItem != null) remItem.Click -= eh_remItemClick;
			base.Dispose(disposing);
		}
	}
}
