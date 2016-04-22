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
using System.Data;
using DevExpress.XtraEditors.Controls;
using System.Reflection;
using System.Windows.Forms;

namespace xwcs.core.ui.editors
{
	[UserRepositoryItem("RegisterDateExtEdit")]
	public class RepositoryItemDateExtEdit : RepositoryItemDateEdit
	{
		#region custom data
		DataTable _filterObject;

		public DataTable Fo { get { return _filterObject; } }
		#endregion

		static RepositoryItemDateExtEdit()
		{
			RegisterDateExtEdit();
		}

		public const string CustomEditName = "DateExtEdit";

		public RepositoryItemDateExtEdit()
		{
			_filterObject = new DataTable();
			PopupResizeMode = ResizeMode.LiveResize;
			Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton());
		}

		public override string EditorTypeName
		{
			get
			{
				return CustomEditName;
			}
		}

		public static void RegisterDateExtEdit()
		{
			Image img = null;
			EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(DateExtEdit), typeof(RepositoryItemDateExtEdit), typeof(DateExtEditViewInfo), new DateExtEditPainter(), true, img));
		}

		public override void Assign(RepositoryItem item)
		{
			BeginUpdate();
			try
			{
				base.Assign(item);
				RepositoryItemDateExtEdit source = item as RepositoryItemDateExtEdit;
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
	public class DateExtEdit : DateEdit
	{

		

		static DateExtEdit()
		{
			RepositoryItemDateExtEdit.RegisterDateExtEdit();
		}

		public DateExtEdit()
		{
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new RepositoryItemDateExtEdit Properties
		{
			get
			{
				return base.Properties as RepositoryItemDateExtEdit;
			}
		}

		public override string EditorTypeName
		{
			get
			{
				return RepositoryItemDateExtEdit.CustomEditName;
			}
		}

		protected override PopupBaseForm CreatePopupForm()
		{
			this.Properties.PopupResizeMode = ResizeMode.LiveResize;
			return new DateExtEditPopupForm(this);
		}
	}

	public class DateExtEditViewInfo : DateEditViewInfo
	{
		public DateExtEditViewInfo(RepositoryItem item) : base(item)
		{

			
		}
	}

	public class DateExtEditPainter : ButtonEditPainter
	{
		public DateExtEditPainter()
		{
		}
	}

	public class DateExtEditPopupForm : CustomBlobPopupForm
	{
		private FieldExpressionControl popup;
		

		public DateExtEditPopupForm(DateExtEdit ownerEdit) : base(ownerEdit)
		{
			popup = popup ?? new FieldExpressionControl();
			popup.Dock = System.Windows.Forms.DockStyle.Fill;
			Controls.Add(popup);
			
		}

		
		

		protected override void Dispose(bool disposing)
		{
			if (disposing && base.Controls.Contains(this.PopupControl))
			{
				base.Controls.Remove(this.PopupControl);
			}
			base.Dispose(disposing);
		}

		

		/*
		[Browsable(false)]
		public PopupContainerEdit OwnerEdit =>
			(base.OwnerEdit as PopupContainerEdit);
			*/

		protected virtual UserControl PopupControl => popup;
		
	}

}
