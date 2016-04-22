using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Controls;
using System.Data;

namespace xwcs.core.ui.editors
{
	[UserRepositoryItem("RegisterIntervalEdit")]
	public class RepositoryItemIntervalEdit : RepositoryItemPopupContainerEdit
	{
		#region custom data
			DataTable _filterObject;

			public DataTable Fo { get { return _filterObject;  } }
		#endregion

		static RepositoryItemIntervalEdit()
		{
			RegisterIntervalEdit();
		}

		public const string CustomEditName = "IntervalEdit";

		public RepositoryItemIntervalEdit()
		{
			_filterObject = new DataTable();
		}

		public override string EditorTypeName
		{
			get
			{
				return CustomEditName;
			}
		}

		public static void RegisterIntervalEdit()
		{
			Image img = null;
			EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(IntervalEdit), typeof(RepositoryItemIntervalEdit), typeof(IntervalEditViewInfo), new IntervalEditPainter(), true, img));
		}

		public override void Assign(RepositoryItem item)
		{
			BeginUpdate();
			try
			{
				base.Assign(item);
				RepositoryItemIntervalEdit source = item as RepositoryItemIntervalEdit;
				if (source == null) return;
				//
			}
			finally
			{
				EndUpdate();
			}
		}
	}

	[ToolboxItem(true)]
	public class IntervalEdit : PopupContainerEdit
	{
		private CustomIntervalPopupControl popup;

		static IntervalEdit()
		{
			RepositoryItemIntervalEdit.RegisterIntervalEdit();
		}

		public IntervalEdit()
		{
			this.CloseUp += (object s, CloseUpEventArgs e) =>
			{
				if(e.AcceptValue) {
					if(popup != null) {
						//e.Value = string.Format("{0} - {1}", popup.MinVal, popup.MaxVal);
						IntervalEdit ie = s as IntervalEdit;
						ie.Properties.NullValuePromptShowForEmptyValue = true;
						ie.Properties.NullValuePrompt = popup?.filterEditorControl?.FilterString;
						ie.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
						ie.Properties.ShowNullValuePromptWhenFocused = true;
						e.Value = null;
						/*
						* 
						* 



					   */
					}
				}
			};

			this.QueryPopUp += (object s, CancelEventArgs e) => 
			{
				popup = popup ?? new CustomIntervalPopupControl(this);
				//setup Data table to filter and eventual expression
				popup.filterEditorControl.SourceControl = Properties.Fo;
				popup.filterEditorControl.FilterString = "[nrecord] between (0,0)";
				popup.filterEditorControl.FilterControl.ShowOperandTypeIcon = true;
				

			};

			this.Popup += (s, e) =>
			{
				popup.filterEditorControl.FilterControl.Focus();
				//set focus on first edit
				
				/*
				bool found = false;
				var firstChild = popup.filterEditorControl.FilterControl.Controls[0];
				var c = popup.filterEditorControl.FilterControl.GetNextControl(firstChild, true);
				while (c != null && !found)
				{
					if (c.Name == "test")
					{
						found = true;
					}
					c = popup.filterEditorControl.FilterControl.GetNextControl(c, true);
				}
				*/
			};

			this.QueryDisplayText += (object s, QueryDisplayTextEventArgs e) =>
			{
				//e.DisplayText = string.Format("{0}", popup?.filterEditorControl?.FilterCriteria?.ToString());
				//base.Properties.Appearance.Font = new Font(base.Properties.Appearance.Font, FontStyle.Italic);
			};

			this.QueryResultValue += (object s, QueryResultValueEventArgs e) => 
			{
				
				//e.Value = popup?.filterEditorControl?.FilterCriteria;
			};
		}



		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new RepositoryItemIntervalEdit Properties
		{
			get
			{
				return base.Properties as RepositoryItemIntervalEdit;
			}
		}

		public override string EditorTypeName
		{
			get
			{
				return RepositoryItemIntervalEdit.CustomEditName;
			}
		}

		protected override PopupBaseForm CreatePopupForm()
		{
			if (Properties.PopupControl == null)
			{
				Properties.PopupControl = popup;
			}
			return new IntervalEditPopupForm(this);
		}
	}

	public class IntervalEditViewInfo : PopupContainerEditViewInfo
	{
		public IntervalEditViewInfo(RepositoryItem item) : base(item)
		{
		}
	}

	public class IntervalEditPainter : ButtonEditPainter
	{
		public IntervalEditPainter()
		{
		}
	}

	public class IntervalEditPopupForm : PopupContainerForm
	{
		public IntervalEditPopupForm(IntervalEdit ownerEdit) : base(ownerEdit)
		{
		}
	}
}
