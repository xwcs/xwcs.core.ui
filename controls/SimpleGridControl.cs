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
using System.Data.Entity;
using xwcs.core.db;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using xwcs.core.evt;
using xwcs.core;


namespace xwcs.core.ui.controls
{
	public partial class SimpleGridControl : DevExpress.XtraEditors.XtraUserControl
	{
		protected static xwcs.core.manager.ILogger _logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(typeof(SimpleGridControl));

		protected DbContext _docDataContext;
		protected xwcs.core.db.binding.GridBindingSource _bs;
		private string _propertyName;
		private EntityBase _container;
		private Type _propertyType;


		// methods
		MethodInfo addRowMethod;
		MethodInfo deleteRowMethod;

		

		public SimpleGridControl(xwcs.core.db.binding.IEditorsHost host, DbContext ctx, Type pt, string pn)
		{
			InitializeComponent();
			_docDataContext = ctx;
			_propertyType = pt;
			_propertyName = pn;
			_bs = new xwcs.core.db.binding.GridBindingSource(host);			
			
			_bs.Grid = gridControl;
			simpleButton_ADD.Click += addRow;
			simpleButton_DELETE.Click += deleteRow;

			// make template function hook
			addRowMethod = GetType().GetMethod("addRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
			deleteRowMethod = GetType().GetMethod("deleteRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
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

			simpleButton_ADD.Click -= addRow;
			simpleButton_DELETE.Click -= deleteRow;			

			base.Dispose(disposing);
		}

		public xwcs.core.db.binding.GridBindingSource BindingSource
		{
			get
			{
				return _bs;
			}
		}

		public void readOnly(bool bOn)
		{
			gridView.OptionsSelection.EnableAppearanceFocusedCell = !bOn;
			gridView.OptionsBehavior.Editable = !bOn;
			gridView.OptionsBehavior.ReadOnly = bOn;
			simpleButton_ADD.Enabled = !bOn;
			simpleButton_DELETE.Enabled = !bOn;
		}

		public  virtual bool RefreshGrid(int movePosition)
		{
			int bookmark = _bs.Position;

			if (_container != null)
			{
				GridColumn column = gridView.Columns.ColumnByFieldName("id");
				//Check if exists column 'id' -> means there are right data. If not exists -> need clear old columns
				if (column == null)
					gridView.Columns.Clear();

				SEventProxy.BlockModelEvents();				
				_docDataContext.Entry(_container).Collection(_propertyName).Load();
				SEventProxy.AllowModelEvents();

				_bs.DataSource = _container.GetPropertyByName(_propertyName);

				if (column == null) column = gridView.Columns.ColumnByFieldName("id");
				if (column != null) column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
				else _logger.Error("Can not find 'order' column");
			}

			if (bookmark != -1)
			{
				bookmark += movePosition;
				_bs.Position = bookmark;
			}
			//Return false if empty
			return (_bs.Count == 0)?false:true;
		}

		public bool RefreshGrid(EntityBase cnt, int movePosition)
		{
			_container = cnt;
			return RefreshGrid(movePosition);
		}


		// EVENTS

		protected void addRow(object sender, EventArgs e)
		{
			addRowMethod.Invoke(this, null);
		}

		protected void addRowGeneric<T>() where T : class
		{
			T curr = _bs.Current as T;
			_bs.AddNew();
			T newCurr = _bs.Current as T;

			gridView.MoveLast();
		}

		protected void deleteRow(object sender, EventArgs e)
		{
			deleteRowMethod.Invoke(this, null);
		}


		protected void deleteRowGeneric<T>() where T : class
		{
			(_docDataContext.GetPropertyByName(_propertyName) as DbSet<T>).Remove(_bs.Current as T);

			RefreshGrid(0);
		}

		public CurrencyManager getCurrencyManager()
		{
			return _bs.CurrencyManager;
		}

		public void saveChanges()
		{

			// save DB
			int iItemsSaved = _docDataContext.SaveChanges();
			_logger.Debug(string.Format("Items saved : {0}", iItemsSaved));

		}

	}


	
}
