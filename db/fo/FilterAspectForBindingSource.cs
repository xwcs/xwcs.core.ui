using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.db.model;
using xwcs.core.evt;
using xwcs.core.ui.editors;

namespace xwcs.core.ui.db.fo
{
	public class FilterFieldEventData
	{
		public object Field { get; set; }
		public char ActionChar { get; set; }
		public string FieldName { get; set; }
//		public FieldRetrievedEventArgs FREA {get; set; }
	}

	public interface IFilterDataBindingSource : IDataBindingSource
	{
		void HandleFilterFiledKeyEvent(FilterFieldEventData ffe);
	}

	public enum PopupCloseKind {
		Confirm,
		Cancel
	}

	public class FilterAspectForBindingSource : IDisposable {
		private PopupControlContainer _popup;
		private FieldExpressionControl _fc;
		private BarManager _barManager;
		private PopupCloseKind _popupCloseKind;
		private TextEdit _destEdit;
		private IDataBindingSource _ds;

		public FilterAspectForBindingSource(IDataBindingSource ds, IEditorsHost eh, BarManager bm){
			_ds = ds;
			_barManager = bm;
			_popup = new PopupControlContainer();
			_fc = new FieldExpressionControl();
			_popup.Controls.Add(_fc);
			_fc.Dock = DockStyle.Fill;

			//weak event
			_fc.OnCancel += (s, e) =>
			{
				_popupCloseKind = PopupCloseKind.Cancel;
				_popup.HidePopup();
			};
			//weak event
			_fc.OnOk += (s, e) =>
			{
				_popupCloseKind = PopupCloseKind.Confirm;
				_popup.HidePopup();
			};

			_popup.CloseUp += popup_CloseUp;
		}
		
		

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_fc.Dispose();
					_popup.CloseUp -= popup_CloseUp;
					_popup.Dispose();
				}

				
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
		#endregion

		private void popup_CloseUp(object sender, EventArgs e)
		{
			if (_popupCloseKind == PopupCloseKind.Confirm && _destEdit != null)
			{
				_destEdit.CustomDisplayText += (object s, CustomDisplayTextEventArgs  ee) =>
				{
					TextEdit te = (s as TextEdit);
					if(te != null) {
						ee.DisplayText = te.Properties.NullValuePrompt;
					}					
				};
				_destEdit.Properties.NullValuePrompt = _fc.filterEditorControl.FilterString;
				//set criteria to filter field
				_ds.Current.SetPropValueByPathUsingReflection(_fc.CurrentFieldName + "_criteria", _fc.filterEditorControl.FilterCriteria);
			}
			_destEdit = null;
		}

		public void HandleFilterFiledKeyEvent(FilterFieldEventData ffe)
		{
			//handle popup opening	
			TextEdit be = ffe.Field as TextEdit;
			//get field
			PropertyDescriptor pd = ReflectionHelper.GetPropertyDescriptorFromPath(_ds.Current.GetType(), ffe.FieldName);

			if (be != null)
			{
				_destEdit = be;
				DataTable fo = new DataTable();

				Type ut = pd.PropertyType;
				//handle nullable
				try
				{
					ut = Nullable.GetUnderlyingType(pd.PropertyType) ?? pd.PropertyType;
				}
				catch (Exception)
				{
					ut = pd.PropertyType;
				}

				fo.Columns.Add(new DataColumn(ffe.FieldName, ut));

				//connect property to filter popup
				_fc.CurrentFieldName = ffe.FieldName;

				//set initial criteria
				_fc.filterEditorControl.SourceControl = fo;
				switch (ffe.ActionChar)
				{
					case '<':
						_fc.filterEditorControl.FilterString = string.Format("[{0}] < '{1}'", ffe.FieldName, (pd.PropertyType.IsValueType ? Activator.CreateInstance(pd.PropertyType) : ""));
						break;
					case ':':
						_fc.filterEditorControl.FilterString = string.Format("[{0}] between ('{1}', '{1}')", ffe.FieldName, (pd.PropertyType.IsValueType ? Activator.CreateInstance(pd.PropertyType) : ""));
						break;
					case '>':
						_fc.filterEditorControl.FilterString = string.Format("[{0}] > '{1}'", ffe.FieldName, (pd.PropertyType.IsValueType ? Activator.CreateInstance(pd.PropertyType) : ""));
						break;
				}

				/*				
				_fc.filterEditorControl.FilterControl.BeforeShowValueEditor += (object ss, ShowValueEditorEventArgs ee) =>
				{
					RepositoryItemLookUpEdit rle = new RepositoryItemLookUpEdit();
					rle.DataSource
					ee.CustomRepositoryItem = 
				};
				
				_fc.filterEditorControl.FilterControl.BeforeCreateValueEditor += (object ss, CreateValueEditorEventArgs ee) =>
				{
					//ee.CustomRepositoryItem = new RepositoryItemDateEdit();
					ee.RepositoryItem =
				};
				*/
				/*
				_fc.filterEditorControl.FilterControl.PopupMenuShowing += (spm, spe) =>
				{
					spe.Menu.BeforePopup += (ms, me) =>
					{
					};
				};
				*/
				_fc.filterEditorControl.FilterControl.ShowOperandTypeIcon = true;

				_popup.Size = new Size(be.Width, 200);
				_popup.ShowCloseButton = false;
				_popup.ShowSizeGrip = true;

				be.EditValue = null;
				be.Properties.NullValuePromptShowForEmptyValue = true;
				be.Properties.NullValuePrompt = _fc.filterEditorControl.FilterString;
				be.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
				be.Properties.ShowNullValuePromptWhenFocused = true;

				Point pt = be.PointToScreen(new Point(0, be.Height));

				_popup.ShowPopup(_barManager, pt);

				_fc.filterEditorControl.FilterControl.Focus();

				// set future closing motive
				// so user must confirm with ok click
				_popupCloseKind = PopupCloseKind.Cancel;
			}
		}

	}
}
