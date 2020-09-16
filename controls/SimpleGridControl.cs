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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.CustomEditor;
using DevExpress.Utils.Drawing;
using xwcs.core.ui.editors;
using DevExpress.XtraGrid.Views.Base;

namespace xwcs.core.ui.controls
{
	public partial class SimpleGridControl : CustomAnyControlBase, IGridControl, core.db.binding.IDataSourceProvider
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

		public SimpleGridControl() {; }
		public SimpleGridControl(xwcs.core.db.binding.IEditorsHost host, Type pt, string pn)
		{
            try
            {
                _skipInvalidate = true;

                InitializeComponent();
                _propertyType = pt;
                _propertyName = pn;
                _host = host;
                _bs = new xwcs.core.db.binding.GridBindingSource(host);

                _bs.AttachToGrid(gridControl);
                simpleButton_ADD.Click += addRow;
                simpleButton_DELETE.Click += deleteRow;
                //gridView.Click += GridView_Click;

                // make template function hook
                addRowMethod = GetType().GetMethod("addRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);
                deleteRowMethod = GetType().GetMethod("deleteRowGeneric", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(pt);

                _bs.ListChanged += _bs_ListChanged;
                _bs.CurrentChanged += _bs_CurrentChanged;
                gridView.InvalidRowException += GridView_InvalidRowException;
                gridView.ValidateRow += GridView_ValidateRow;
                gridView.RowUpdated += GridView_RowUpdated;
            }
            finally
            {
                _skipInvalidate = false;
            }
			
		}

        private void GridView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            EntityBase ent = (EntityBase)_bs.Current;
            e.ErrorText = ent.ErrorMessage();
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            gridView.SetColumnError(null, e.ErrorText, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);

        }
        
        private void GridView_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            if (_bs.Current is  EntityBase)
            {
                EntityBase ent = (EntityBase)_bs.Current;
                if (!ent.IsValid())
                {
                    e.Valid = false;
                    e.ErrorText = ent.ErrorMessage();
                    return;
                }
            }
        }

        private void GridView_RowUpdated(object sender, RowObjectEventArgs e)
        {
            _wes_RowUpdated?.Raise(this, e);
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


        private void _bs_CurrentChanged(object sender, EventArgs e)
        {
            if (!_ReadOnly)
            {
                bool enable_delete = !_ReadOnly;
                if (_bs.Current is IVocabularyElement)
                {
                    IVocabularyElement voc = (IVocabularyElement)_bs.Current;
                    if (voc.Occorrenze > 0 || !voc.IsDeletable())
                    {
                        enable_delete = false;
                    }
                }
                simpleButton_DELETE.Enabled = enable_delete;
            }
            
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

		public void showGroupPanel(bool bShow)
		{
			gridView.OptionsView.ShowGroupPanel = bShow;
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

            _bs.ListChanged -= _bs_ListChanged;
            _bs.CurrentChanged -= _bs_CurrentChanged;
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

        private WeakEventSource<RowAfterEditEventArgs> _wes_AfterRowEdit = null;
        public event EventHandler<RowAfterEditEventArgs> AfterRowEdit
        {
            add
            {
                if (_wes_AfterRowEdit == null)
                {
                    _wes_AfterRowEdit = new WeakEventSource<RowAfterEditEventArgs>();
                }
                _wes_AfterRowEdit.Subscribe(value);
            }
            remove
            {
                _wes_AfterRowEdit?.Subscribe(value);
            }
        }

        private WeakEventSource<RowObjectEventArgs> _wes_RowUpdated = null;
        public event EventHandler<RowObjectEventArgs> RowUpdate
        {
            add
            {
                if (_wes_RowUpdated == null)
                {
                    _wes_RowUpdated = new WeakEventSource<RowObjectEventArgs>();
                }
                _wes_RowUpdated.Subscribe(value);
            }
            remove
            {
                _wes_RowUpdated?.Subscribe(value);
            }
        }

        private WeakEventSource<RowDeleteEventArgs> _wes_BeforeRowDelete = null;
        public event EventHandler<RowDeleteEventArgs> BeforeRowDelete
        {
            add
            {
                if (_wes_BeforeRowDelete == null)
                {
                    _wes_BeforeRowDelete = new WeakEventSource<RowDeleteEventArgs>();
                }
                _wes_BeforeRowDelete.Subscribe(value);
            }
            remove
            {
                _wes_BeforeRowDelete?.Subscribe(value);
            }
        }


        public xwcs.core.db.binding.GridBindingSource BindingSource
		{
			get
			{
				return _bs;
			}
		}


        private bool _ReadOnly = true;
		public void readOnly(bool bOn)
		{
			gridView.OptionsSelection.EnableAppearanceFocusedCell = !bOn;
            // gridView.OptionsBehavior.Editable = !bOn;
            _ReadOnly = bOn;
            gridView.OptionsBehavior.ReadOnly = bOn;
			simpleButton_ADD.Enabled = !bOn;
            simpleButton_DELETE.Enabled = !bOn;
            if (_bs.Current is IVocabularyElement)
            {
                IVocabularyElement voc = (IVocabularyElement)_bs.Current;
                if (voc.Occorrenze > 0 || !voc.IsDeletable())
                {
                    simpleButton_DELETE.Enabled = false;
                }
            }
            
		}

		public  virtual bool RefreshGrid(int movePosition, bool force = false)
		{
			int bookmark = _bs.Position;
			
			if (_container != null)
			{
                SEventProxy.BlockModelEvents();
                object l = _host.DataCtx.LazyLoadOrDefaultCollection(_container, _propertyName, force);
                SEventProxy.AllowModelEvents();

                //this will add columns or reset grid if is null
                _bs.DataSource = l;
            }
            else
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


		// EVENTS

		protected void addRow(object sender, EventArgs e)
		{
			addRowMethod.Invoke(this, null);
		}

		protected void addRowGeneric<T>() where T : class
		{
            // disable eventual filters
            gridView.ActiveFilterEnabled = false;

            T curr = _bs.Current as T;
            if (curr is IVocabularyElement)
            {
                _bs.Add(((IVocabularyElement)curr).GetFirstFree());
            }
            else
            {
                _bs.AddNew();
            }
			T newCurr = _bs.Current as T;

            // handle entity connect to context
            EntityBase eb = newCurr as EntityBase;
            if (eb != null)
            {
                eb.SetCtx(_host.DataCtx);
            }

            _wes_RowEdit?.Raise(this, new RowEditEventArgs() { Data = _bs.Current, IsNew = true });

			gridView.ShowEditForm();

            // this can be use for eventual cleaning after cancel, if newCurr is not the same as _bs.Current => means it was canceled in edit form
            _wes_AfterRowEdit?.Raise(this, new RowAfterEditEventArgs() { Data = newCurr, DoCancel = !ReferenceEquals(newCurr, _bs.Current)});

            // delete object for sure again!!
            EntityState es = _host.DataCtx.Entry(newCurr).State;
            if (!ReferenceEquals(newCurr, _bs.Current) && _host.DataCtx.Entry(newCurr).State == EntityState.Added)
            {
                deleteRowMethod.Invoke(this, new object[] { newCurr });
            }

            gridView.MoveLast();
		}

		protected void deleteRow(object sender, EventArgs e)
		{
			deleteRowMethod.Invoke(this, new object[] { _bs.Current } );
		}


		protected void deleteRowGeneric<T>(object what) where T : class
		{
            if (ReferenceEquals(null, what)) return;
            if (what is IVocabularyElement)
            {
                IVocabularyElement voc = (IVocabularyElement)what;
                if (voc.Occorrenze>0 || !voc.IsDeletable())
                {
                    return;
                }
            }
            _wes_BeforeRowDelete?.Raise(this, new RowDeleteEventArgs() { Data = what });

            // stop events for infinite getters recursion
            // SEventProxy.BlockModelEvents();
            _host.DataCtx.DeleteRowGeneric<T>(_propertyName, what as T);
            // SEventProxy.AllowModelEvents();

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

        protected void LockTable<T>()
        {
            var lr = _host.DataCtx.TableLock(typeof(T).Name);
        }

        protected void UnlockTable<T>()
        {
            var lr = _host.DataCtx.TableUnlock(typeof(T).Name);
        }

		private void gridView_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
		{
            Form tmp = (e.Panel.Parent as Form);
            if (tmp == null) return;
            tmp.StartPosition = FormStartPosition.CenterScreen;
           
            foreach(Control bc in e.BindableControls)
            {
                bc.Enabled = !gridView.OptionsBehavior.ReadOnly;
            }

            tmp.Tag = gridView.OptionsBehavior.ReadOnly;
		}

        public void PostChanges()
        {
            gridView.PostEditor();
            gridView.UpdateCurrentRow();
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

    /// <summary>
    /// Event happen when row is edited
    /// </summary>
    public class RowEditEventArgs : EventArgs {
		public object Data = null;
		public bool IsNew = false;

	}

    /// <summary>
    /// Event happen when row is being deleted
    /// </summary>
    public class RowDeleteEventArgs : EventArgs
    {
        public object Data = null;
    }

    /// <summary>
    /// Event happen after row was edited
    /// </summary>
    public class RowAfterEditEventArgs : EventArgs
    {
        public object Data = null;
        public bool DoCancel = false;

    }
}
