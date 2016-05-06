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

namespace xwcs.core.ui.editors
{
	public partial class GridEditControl : XtraUserControl, IAnyControlEdit
	{
		private object _val = null;
		private GridBindingSource _bs;
		

		public GridEditControl()
		{
			InitializeComponent();
			gridViewMain.OptionsView.ShowGroupPanel = false;
			RecommendedSize = new Size(0,150);
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
				}else {
					//Clear datasource
					(_val as IList)?.Clear();
				}
				if(_val != null) {
					if(_bs != null) {
						_bs.Dispose();
						#if DEBUG
						SLogManager.getInstance().Info("reset grid");
						#endif
					}
					_bs = new GridBindingSource();
					_bs.Grid = gridControl;
					_bs.DataSource = _val;
					_bs.GetFieldQueryable += (object s, GetFieldQueryableEventData e) =>
					{
						GetFieldQueryable?.Invoke(this, e);
					};
				}
			}
		}		

		public event EventHandler EditValueChanged;

		public event EventHandler<GetFieldQueryableEventData> GetFieldQueryable;

		public void addRow() {
			Console.WriteLine("Add row called!");
			_bs.AddNew();
		}

		public void remRow()
		{
			Console.WriteLine("Rem row called!");
			_bs.RemoveCurrent();
		}

		private void gridViewMain_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
		{
			if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.EmptyRow ||
				e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell
			)
			{
				GridView view = sender as GridView;
				view.FocusedRowHandle = e.HitInfo.RowHandle;
				GridViewCustomMenu gcm = new GridViewCustomMenu(this, view,
					e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell ?
					PopupMenyType.addRem :
					PopupMenyType.justAdd
				);
				gcm.Init(e.HitInfo);
				gcm.Show(e.Point);
			}
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
