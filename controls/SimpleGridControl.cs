﻿using System;
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
using DevExpress.XtraGrid.Views.Grid;

namespace xwcs.core.ui.controls
{
	public partial class SimpleGridControl : DevExpress.XtraEditors.XtraUserControl
	{
		protected static xwcs.core.manager.ILogger _logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(typeof(SimpleGridControl));

		protected xwcs.core.db.binding.GridBindingSource _bs;
		private string _propertyName;
		private EntityBase _container;
		private Type _propertyType;
        private xwcs.core.db.binding.IEditorsHost _host;

        // methods
        MethodInfo addRowMethod;
		MethodInfo deleteRowMethod;

		private DevExpress.XtraGrid.Views.Grid.EditFormUserControl _editControl = null;


		public SimpleGridControl(xwcs.core.db.binding.IEditorsHost host, Type pt, string pn)
		{
			InitializeComponent();
			_propertyType = pt;
			_propertyName = pn;
            _host = host;
			_bs = new xwcs.core.db.binding.GridBindingSource(host);

			_bs.Grid = gridControl;
			simpleButton_ADD.Click += addRow;
			simpleButton_DELETE.Click += deleteRow;
			//gridView.Click += GridView_Click;

			// make template function hook
			addRowMethod = GetType().GetMethod("addRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
			deleteRowMethod = GetType().GetMethod("deleteRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
		}

		public EditFormUserControl EditControl
		{
			get
			{
				return _editControl;
			}

			set
			{
				_editControl = value;
				gridView.OptionsEditForm.CustomEditFormLayout = _editControl;
				gridView.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
			}
		}

		/*
		private void GridView_Click(object sender, EventArgs e)
		{
			//_wes_RowEdit?.Raise(this, new RowEditEventArgs() { Data = _bs.Current });
		}
		*/

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
			//gridView.Click -= GridView_Click;

			base.Dispose(disposing);
		}

		
		private WeakEventSource<RowEditEventArgs> _wes_RowEdit = null;
		public event EventHandler<RowEditEventArgs>  RowEdit
		{
			add
			{
				if (_wes_RowEdit == null)
				{
					_wes_RowEdit = new WeakEventSource<RowEditEventArgs>();
				}
				_wes_RowEdit.Subscribe(value);
			}
			remove
			{
				_wes_RowEdit?.Subscribe(value);
			}
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
                _host.DataCtx.LazyLoadOrDefaultCollection(_container, _propertyName);
                SEventProxy.AllowModelEvents();

                // this should lazy load if necessary
                _bs.DataSource = _container.GetPropertyByName(_propertyName);

                /*
				if (column == null) column = gridView.Columns.ColumnByFieldName("id");
				if (column != null) column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
				else _logger.Error("Can not find 'order' column");
                */
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

			_wes_RowEdit?.Raise(this, new RowEditEventArgs() { Data = _bs.Current, IsNew = true });

			gridView.MoveLast();
		}

		protected void deleteRow(object sender, EventArgs e)
		{
			deleteRowMethod.Invoke(this, null);
		}


		protected void deleteRowGeneric<T>() where T : class
		{
			(_host.DataCtx.GetPropertyByName(_propertyName) as DbSet<T>).Remove(_bs.Current as T);

			RefreshGrid(0);
		}

		public CurrencyManager getCurrencyManager()
		{
			return _bs.CurrencyManager;
		}

        public void saveChanges()
		{
            // save DB
			int iItemsSaved = _host.DataCtx.SaveChanges();
			_logger.Debug(string.Format("Items saved : {0}", iItemsSaved));

		}

		private void gridView_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
		{
			(e.Panel.Parent as Form).StartPosition = FormStartPosition.CenterScreen;			
		}
	}

	
	public class RowEditEventArgs : EventArgs {
		public object Data = null;
		public bool IsNew = false;

	}
	


	
}