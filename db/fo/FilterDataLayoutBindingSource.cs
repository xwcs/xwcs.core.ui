using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
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
		public FieldRetrievedEventArgs FREA {get; set; }
	}

	public interface IFilterDataLayoutExtender : IDataLayoutExtender
	{
		void onFilterFieldEvent(FilterFieldEventData ffe);
	}

	public enum PopupCloseKind {
		Confirm,
		Cancel
	}

	public class FilterDataLayoutBindingSource : DataLayoutBindingSource, IFilterDataLayoutExtender
	{
		private PopupControlContainer _popup;
		private FieldExpressionControl _fc;
		private BarManager _barManager;
		private PopupCloseKind _popupCloseKind;
		private TextEdit _destEdit;

		//public EventHandler<FilterFieldEventData> FilterFieldEvent;
		private readonly WeakEventSource<FilterFieldEventData> _wes_FilterFieldEvent = new WeakEventSource<FilterFieldEventData>();
		public event EventHandler<FilterFieldEventData> FilterFieldEvent
		{
			add { _wes_FilterFieldEvent.Subscribe(value); }
			remove { _wes_FilterFieldEvent.Unsubscribe(value); }
		}


		public FilterDataLayoutBindingSource(BarManager bm) {
			_barManager = bm;
			_popup = new PopupControlContainer();
			_fc = new FieldExpressionControl();
			_popup.Controls.Add(_fc);
			_fc.Dock = DockStyle.Fill;

			_fc.OnCancel += (s, e) =>
			{
				_popupCloseKind = PopupCloseKind.Cancel;
				_popup.HidePopup();	
			};

			_fc.OnOk += (s, e) =>
			{
				_popupCloseKind = PopupCloseKind.Confirm;
				_popup.HidePopup();
			};

			_popup.CloseUp += (s, e) =>
			{
				if(_popupCloseKind == PopupCloseKind.Confirm && _destEdit != null) {
					_destEdit.Properties.NullValuePrompt = _fc.filterEditorControl.FilterString;
					//set criteria to filter field
					Current.SetPropValueByPathUsingReflection(_fc.CurrentFieldName + "_criteria", _fc.filterEditorControl.FilterCriteria);
				}
				//this will set nul value to the underlaing binding too
				_destEdit = null;	
			};
		}

		public void onFilterFieldEvent(FilterFieldEventData ffe)
		{
			//handle popup opening	
			TextEdit be = ffe.Field as TextEdit;
			//get field
			PropertyDescriptor pd = ReflectionHelper.GetPropertyDescriptorFromPath(Current.GetType(), ffe.FREA.FieldName);

			if (be != null) {
				_destEdit = be;
				DataTable fo = new DataTable();

				Type ut = pd.PropertyType;
				//handle nullable
				try {
					ut = Nullable.GetUnderlyingType(pd.PropertyType) ?? pd.PropertyType;
				}catch(Exception) {
					ut = pd.PropertyType;
				}

				

				fo.Columns.Add(new DataColumn(ffe.FREA.FieldName, ut));

				//connect property to filter popup
				_fc.CurrentFieldName = ffe.FREA.FieldName;

				//set initial criteria
				_fc.filterEditorControl.SourceControl = fo;
				switch(ffe.ActionChar) {
					case '<':
						_fc.filterEditorControl.FilterString = string.Format("[{0}] < '{1}'", ffe.FREA.FieldName, (pd.PropertyType.IsValueType ? Activator.CreateInstance(pd.PropertyType) : ""));
						break;
					case ':':
						_fc.filterEditorControl.FilterString = string.Format("[{0}] between ('{1}', '{1}')", ffe.FREA.FieldName, (pd.PropertyType.IsValueType ? Activator.CreateInstance(pd.PropertyType) : ""));
						break;
					case '>':
						_fc.filterEditorControl.FilterString = string.Format("[{0}] > '{1}'", ffe.FREA.FieldName, (pd.PropertyType.IsValueType ? Activator.CreateInstance(pd.PropertyType) : ""));
						break;
				}
				/*				
				_fc.filterEditorControl.BeforeShowValueEditor += (object ss, ShowValueEditorEventArgs ee) =>
				{
					//ee.CustomRepositoryItem = new RepositoryItemDateEdit();

				};
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

			_wes_FilterFieldEvent.Raise(this, ffe);
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_fc.Dispose();
					_popup.Dispose();
				}
				base.Dispose(disposing);
			}
		}
	}
}
