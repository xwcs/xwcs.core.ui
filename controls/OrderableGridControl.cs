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
using xwcs.core;
using xwcs.core.db;
//using lib.db.doc.niterdoc;
using DevExpress.XtraGrid.Columns;
using System.Reflection;
using System.Linq.Expressions;
using xwcs.core.evt;
using System.Data.Entity.Infrastructure;
using xwcs.core.ui.controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.CustomEditor;
using xwcs.core.ui.editors;

namespace xwcs.core.ui.controls
{
    public partial class OrderableGridControl : CustomAnyControlBase, IGridControl, xwcs.core.db.binding.IDataSourceProvider
    {
        protected static xwcs.core.manager.ILogger _logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(typeof(OrderableGridControl));
		protected xwcs.core.db.binding.GridBindingSource _bs;
		protected string _propertyName;
        protected string _orederPropertyName;
		protected EntityBase _container;
		protected Type _propertyType;
        private xwcs.core.db.binding.IEditorsHost _host;

        // methods
        protected MethodInfo addRowMethod;
		protected MethodInfo deleteRowMethod;
        protected MethodInfo moveUpMethod;
        protected MethodInfo moveDownMethod;

        //getter setter for order column
        object _getter = null;
        object _setter = null;



        public OrderableGridControl(xwcs.core.db.binding.IEditorsHost host, Type pt, string pn, string opn) : base ()
        {
            try
            {
                _skipInvalidate = true;

                InitializeComponent();
                _propertyType = pt;
                _propertyName = pn;
                _orederPropertyName = opn;
                _host = host;
                _bs = new xwcs.core.db.binding.GridBindingSource(host);
                _bs.AttachToGrid(gridControl);
                simpleButton_UP.Click += moveUp;
                simpleButton_DOWN.Click += moveDown;
                simpleButton_ADD.Click += addRow;
                simpleButton_DELETE.Click += deleteRow;

                //getter setter
                MethodInfo makeGS = GetType().GetMethod("makeGEtterSetter", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
                makeGS.Invoke(this, null);
                // make template function hook
                addRowMethod = GetType().GetMethod("addRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
                deleteRowMethod = GetType().GetMethod("deleteRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
                moveUpMethod = GetType().GetMethod("moveUpGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
                moveDownMethod = GetType().GetMethod("moveDownGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);

                gridView.InvalidValueException += GridView_InvalidValueException;
                //gridView.ValidatingEditor += GridView_ValidatingEditor;

                _bs.ListChanged += _bs_ListChanged;
                gridView.OptionsSelection.MultiSelect = true;
                gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
                gridView.SelectionChanged += GridView_SelectionChanged;
                ResizeRedraw = true;
            }
            finally
            {
                _skipInvalidate = false;
            }

            
        }

        private void GridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView.OptionsBehavior.ReadOnly) return;
            simpleButton_UP.Enabled = (gridView.SelectedRowsCount == 1);
            simpleButton_DOWN.Enabled = (gridView.SelectedRowsCount == 1);
            simpleButton_DELETE.Enabled = (gridView.SelectedRowsCount == 1);
        }

        private void makeGEtterSetter<T>() where T : class
        {
            // expression :  variable of type T
            ParameterExpression lambdaArg = Expression.Parameter(typeof(T));

            // expression : variable of type int with name _orederPropertyName
            ParameterExpression lambdaArgInt = Expression.Parameter(typeof(int), _orederPropertyName);

            // expression : access to property with name _orederPropertyName  in object of type T  (T._orederPropertyName)
            Expression propertyAccess = Expression.Property(lambdaArg, _orederPropertyName);

            /*Func<T,int>*/
            _getter = Expression.Lambda<Func<T, int>>(propertyAccess, lambdaArg).Compile();

            // setter
            /*Action<T, int>*/
            _setter = Expression.Lambda<Action<T, int>>(Expression.Assign(propertyAccess, lambdaArgInt), lambdaArg, lambdaArgInt).Compile();
        }

        private void gridView_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {
            (e.Panel.Parent as Form).StartPosition = FormStartPosition.CenterScreen;
            (e.Panel.Parent as Form).Tag = gridView.OptionsBehavior.ReadOnly;
        }

        protected override void OnRootVisibleChnaged()
        {
            _bs.ForceInitializeGrid();
        }


        /// <summary>
        /// we ned invalidate value so eventual Layout container will handle resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.PropertyDescriptorChanged) return;
            InvalidateValue();
        }

        /*
        private void GridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;
            if(view.FocusedColumn.Name == "colid_autori")
            {
                if((int)e.Value  < 0)
                {
                    e.Valid = false;
                    e.ErrorText = "Missing autor";
                }
            }
        }
        */

        private void GridView_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.DisplayError;
        }

        public void PostChanges()
        {
            gridView.PostEditor();
            gridView.UpdateCurrentRow();
        }
        
        public xwcs.core.db.binding.GridBindingSource BindingSource
        {
            get
            {
                return _bs;
            }
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


            simpleButton_UP.Click -= moveUp;
            simpleButton_DOWN.Click -= moveDown;
            simpleButton_ADD.Click -= addRow;
            simpleButton_DELETE.Click -= deleteRow;
            _bs.ListChanged -= _bs_ListChanged;

            base.Dispose(disposing);
        }


        public void SetReadOnly(bool bOn)
        {
            gridView.OptionsSelection.EnableAppearanceFocusedCell = !bOn;
            //gridView.OptionsBehavior.Editable = !bOn;
            gridView.OptionsBehavior.ReadOnly = bOn;
            simpleButton_UP.Enabled = !bOn;
            simpleButton_DOWN.Enabled = !bOn;
            simpleButton_ADD.Enabled = !bOn;
            simpleButton_DELETE.Enabled = !bOn;
        }

		protected bool RefreshGrid(int movePosition, bool force = false)
        {
            int bookmark = _bs.Position;

            if (_container != null)
            {
                // load only if not loaded
                SEventProxy.BlockModelEvents();
                object l = _host.DataCtx.LazyLoadOrDefaultCollection(_container, _propertyName, force);
                SEventProxy.AllowModelEvents();
                
                //this will add columns or reset grid if is null
                _bs.DataSource = l;

                GridColumn column = null;
                if (ReferenceEquals(column,  null)) column = gridView.Columns.ColumnByFieldName("ordine");
                if (!ReferenceEquals(column, null)) column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            }else
            {
                _bs.DataSource = null;
            }
			
			if (bookmark != -1)
			{
				bookmark += movePosition;
				_bs.Position = bookmark;
			}

            

            //Return false if empty
            return (_bs.Count == 0)?false:true;
		}

        public bool RefreshGrid(EntityBase cnt, int movePosition, bool force = false)
        {
            _container = cnt;
            return RefreshGrid(movePosition, force);
        }

        public class RowEditEventArgs : EventArgs
        {
            public object Data = null;
            public bool IsNew = false;

        }

        private WeakEventSource<RowEditEventArgs> _wes_RowEdit = null;



        public event EventHandler<RowEditEventArgs> RowEdit
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


        // EVENTS
        protected void moveUp(object sender, EventArgs e)
        {
            if (gridView.SelectedRowsCount != 1) return;
            moveUpMethod.Invoke(this, null);
        }


        protected void moveUpGeneric<T>() where T : class
        {
            if (gridView.SelectedRowsCount != 1) return;
            int iRowSelect = gridView.FocusedRowHandle;

            if (iRowSelect >= 0)
            {
                var actualData = gridView.GetRow(iRowSelect);
                var beforeData = gridView.GetRow(iRowSelect - 1);

                if ((beforeData == null) || (((Func<T, int>)_getter)((T)actualData) == ((Func<T, int>)_getter)((T)beforeData))) return;

                // actualData.ordine--;
                ((Action<T, int>)_setter)((T)actualData, ((Func<T, int>)_getter)((T)actualData) - 1);

                // beforeData.ordine++;
                ((Action<T, int>)_setter)((T)beforeData, ((Func<T, int>)_getter)((T)beforeData) + 1);
            }
        }
        protected void moveDown(object sender, EventArgs e)
        {
            if (gridView.SelectedRowsCount != 1) return;
            moveDownMethod.Invoke(this, null);
        }


        protected void moveDownGeneric<T>() where T : class
        {
            if (gridView.SelectedRowsCount != 1) return;
            int iRowSelect = gridView.FocusedRowHandle;

            if (iRowSelect >= 0)
            {
                var actualData = gridView.GetRow(iRowSelect);
                var nextData = gridView.GetRow(iRowSelect + 1);

                if ((nextData == null) || (((Func<T, int>)_getter)((T)actualData) == ((Func<T, int>)_getter)((T)nextData))) return;

                // actualData.ordine++;
                ((Action<T, int>)_setter)((T)actualData, ((Func<T, int>)_getter)((T)actualData) + 1);

                // nextData.ordine--;
                ((Action<T, int>)_setter)((T)nextData, ((Func<T, int>)_getter)((T)nextData) - 1);
            }
        }

        protected void addRow(object sender, EventArgs e)
        {
            addRowMethod.Invoke(this, null);
        }
        protected void addRowGeneric<T>() where T : class
        {
            IList<T> tmp = (_container.GetPropertyByName(_propertyName) as IList<T>);
            int maxOrdine = tmp.Count > 0 ? tmp.Max(element => ((Func<T,int>)_getter)(element)) : 0;
            
            T curr = _bs.Current as T;
            _bs.AddNew();
            T newCurr = _bs.Current as T;

            // handle entity connect to context
            EntityBase eb = newCurr as EntityBase;
            if(eb != null)
            {
                eb.SetCtx(_host.DataCtx);
            }

            if (curr != null)
            {
                // newCurr.ordine = maxOrdine + 1;
                ((Action<T, int>)_setter)(newCurr, maxOrdine + 1);
            }
            else
            {
                // newCurr.ordine = maxOrdine + 1;
                ((Action<T, int>)_setter)(newCurr, maxOrdine + 1);
            }
			_wes_RowEdit?.Raise(this, new RowEditEventArgs() { Data = _bs.Current, IsNew = true });

			gridView.MoveLast();
        }

		protected void deleteRow(object sender, EventArgs e)
        {
            if (gridView.SelectedRowsCount != 1) return;
            deleteRowMethod.Invoke(this, null);
        }

		protected void deleteRowGeneric<T>() where T : class
        {
            if (gridView.SelectedRowsCount != 1) return;
            if (ReferenceEquals(null, _bs.Current)) return;
            //SEventProxy.BlockModelEvents();
            _host.DataCtx.DeleteRowGeneric<T>(_propertyName, _bs.Current as T);
            //SEventProxy.AllowModelEvents();
            RefreshGrid(0);
			updateOrdersInGrid();

		}

		private void updateOrdersInGrid()
		{
			GridColumn column = gridView.Columns.ColumnByFieldName("ordine");

			if (column == null) return;
			for (int i = 0; i < gridView.DataRowCount; i++)
			{
				gridView.SetRowCellValue(i, column, (i + 1));
			}
		}
        #region IDataSourceProvider
        public object DataSource
        {
            get
            {
                return _bs.DataSource;
            }

            set
            {
                return; // cant set anything
            }
        }
        #endregion
    }
}
