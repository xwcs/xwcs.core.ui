﻿using DevExpress.Data.Filtering;
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
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using xwcs.core.db.binding;
using xwcs.core.db.binding.attributes;
using xwcs.core.db.fo;
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
        public Keys Keys { get; set; }
//		public FieldRetrievedEventArgs FREA {get; set; }
	}

	public interface IFilterDataBindingSource : IDataBindingSource
	{
		// handle interval criteria popup start
		void HandleFilterFiledKeyEvent(FilterFieldEventData ffe);
		
		
		// We need reset criteria when there is not normal value in the edit
		// due to underlaying system value validation
		// if we decide insert criteria into existing value
		// there must be value set to null, and validating system resend this value 
		// at the end of validation to the field , this is done just after we set criteria
		// so there is a situation, when we loose filter value
		// so we need reseting criteria mechanics, which will reset field criteria
		// so we can ignore value set if criteria is commanding ( criteria presence is stronger )
		// so when we dont need criteria we need reset it
		// this must be done on first non action char hit on editor with null real value
		//void HandleResetCriteria(string fn);
	}

	public enum PopupCloseKind {
		Confirm,
		Cancel
	}

	public class FilterAspectForBindingSource : IDisposable {
        private static xwcs.core.manager.ILogger _logger = xwcs.core.manager.SLogManager.getInstance().getClassLogger(typeof(FilterAspectForBindingSource));

        private PopupControlContainer _popup;
		private FieldExpressionControl _fc;
		private BarManager _barManager;
		private PopupCloseKind _popupCloseKind;
		private TextEdit _destEdit;
		private IDataBindingSource _ds;
        private IEditorsHost _eh;
        
        public FilterAspectForBindingSource(IDataBindingSource ds, IEditorsHost eh, BarManager bm){
			_ds = ds;
			_barManager = bm;
			_popup = new PopupControlContainer();
            _popup.CloseUp += popup_CloseUp;
            // _fc will be done when needed
            _fc = null;
            _eh = eh;
		}
		

        // we need lazy creation
        private void make_fc()
        {
            if (_fc != null)
            {
                return;
            }

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

            _fc.filterEditorControl.FilterControl.BeforeShowValueEditor += showvalueEditor_handler;
        }

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
                    if (_fc != null)
                    {
                        // clean only if it was created
                        _fc.filterEditorControl.FilterControl.BeforeShowValueEditor -= showvalueEditor_handler;
                        _fc.Dispose();
                    }
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
            // _fc must exist
            if (_popupCloseKind == PopupCloseKind.Confirm && _destEdit != null)
			{
                if(_fc.CurrentFieldName == "v_xwbo_iter.extra" || _fc.CurrentFieldName == "v_xwbo_note.extra")
                {
                    _destEdit.Properties.NullValuePrompt = "";
                    _ds.Current.SetPropValueByPathUsingReflection(_fc.CurrentFieldName, _fc.filterEditorControl.FilterCriteria.ToString() );
                }
                else
                {
                    _destEdit.Properties.NullValuePrompt = _fc.filterEditorControl.FilterString;
                    //set criteria to filter field
                    _ds.Current.SetPropValueByPathUsingReflection(_fc.CurrentFieldName + "_criteria", _fc.filterEditorControl.FilterCriteria);
                }
			}
			_destEdit = null;
		}

		private void showvalueEditor_handler(object ss, ShowValueEditorEventArgs ee)
		{
			using (WaitCursorHelper.NewWaitCursor())
			{
                // be sere fc is created
                make_fc();

                if (_ds.AttributesCache.ContainsKey(_fc.CurrentFieldName))
				{
					foreach (CustomAttribute a in _ds.AttributesCache[_fc.CurrentFieldName])
					{
						a.applyCustomEditShownFilterControl(_ds, ee);
					}
				}
			}
		}

		public void HandleFilterFiledKeyEvent(FilterFieldEventData ffe)
		{
			//handle popup opening	
			TextEdit be = ffe.Field as TextEdit;
			//get field
			PropertyDescriptor pd = ReflectionHelper.GetPropertyDescriptorFromPath(_ds.Current.GetType(), ffe.FieldName);

			if (be != null)
			{
                // handle changed style
                be.StyleController = _eh.FormSupport.ModifiedStyle;

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


                if (ffe.FieldName == "v_xwbo_iter.extra" || ffe.FieldName == "v_xwbo_note.extra")
                {
                    fo.Columns.Add(new DataColumn("i.Text", typeof(string)));
                    fo.Columns.Add(new DataColumn("i.OTA", typeof(string)));
                    fo.Columns.Add(new DataColumn("n.Note", typeof(string)));
                }
                else
                {
                    fo.Columns.Add(new DataColumn(ffe.FieldName, ut));
                }
				

                // be sere fc is created
                make_fc();

                //connect property to filter popup
                _fc.CurrentFieldName = ffe.FieldName;


                //set initial criteria
                _fc.filterEditorControl.FilterCriteria = CriteriaOperator.Parse("");
                _fc.filterEditorControl.SourceControl = fo;

                //we can take old value if present
                ICriteriaTreeNode field = (_ds.Current as FilterObjectbase)?.GetFilterFieldByPath(ffe.FieldName);

                if(_fc.CurrentFieldName == "v_xwbo_iter.extra" || _fc.CurrentFieldName == "v_xwbo_note.extra")
                {
                    try
                    {
                        _fc.filterEditorControl.FilterCriteria = CriteriaOperator.Parse(field.ToString());
                    } catch(Exception e)
                    {
                        _logger.Error(e.ToString());
                        _fc.filterEditorControl.FilterCriteria = null;
                    }
                }   
                else
                {
                    if (field != null && field.HasCriteria())
                    {
                        _fc.filterEditorControl.FilterCriteria = field.GetCondition();
                    }
                    else
                    {
                        switch (ffe.ActionChar)
                        {
                            case '<':
                                _fc.filterEditorControl.FilterString = string.Format("[{0}] < ?", ffe.FieldName);
                                break;
                            case ':':
                                _fc.filterEditorControl.FilterString = string.Format("[{0}] between (?, ?)", ffe.FieldName);
                                break;
                            case '>':
                                _fc.filterEditorControl.FilterString = string.Format("[{0}] > ?", ffe.FieldName);
                                break;
                            case '*':
                                _fc.filterEditorControl.FilterString = string.Format("contains([{0}], ?)", ffe.FieldName);
                                break;
                            case '=':
                                _fc.filterEditorControl.FilterString = string.Format("[{0}] = ?", ffe.FieldName);
                                break;
                        }
                    }
                }

                	
				

				/*
				_fc.filterEditorControl.FilterControl.BeforeCreateValueEditor += (object ss, CreateValueEditorEventArgs ee) =>
				{
					//ee.CustomRepositoryItem = new RepositoryItemDateEdit();
					//ee.RepositoryItem =
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

				_popup.Size = new Size(Math.Max(be.Width, 400), 200);
				_popup.ShowCloseButton = false;
				_popup.ShowSizeGrip = true;

				be.EditValue = null; // null current value
				be.Properties.NullValuePromptShowForEmptyValue = true;
				be.Properties.NullValuePrompt = _fc.filterEditorControl.FilterString;
				be.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
				be.Properties.ShowNullValuePromptWhenFocused = true;

				Point pt = be.PointToScreen(new Point(0, be.Height));

				_popup.ShowPopup(_barManager, pt);

				_fc.filterEditorControl.FilterControl.Focus();
				//wait eventual shift pressed
				DateTime twoSec = DateTime.Now.AddSeconds(2);

				while (DateTime.Now < twoSec && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))){
					//Console.WriteLine(string.Format("Waiting! {0}, {1}, L({2}) R({3}) ", DateTime.Now, twoSec, Keyboard.GetKeyStates(Key.LeftShift), Keyboard.GetKeyStates(Key.LeftShift)));
					SendKeys.Flush();
					Thread.Sleep(100);
				}
				if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {
					MessageBox.Show("Ma lo molli sto shift?");
				}else {
					SendKeys.Flush();
					SendKeys.Send("{RIGHT}");
					SendKeys.Send("{RIGHT}");
					SendKeys.Send("{RIGHT}");
					SendKeys.Send("{ENTER}");
				}

				// set future closing motive
				// so user must confirm with ok click
				_popupCloseKind = PopupCloseKind.Cancel;
			}
		}

	}
}
