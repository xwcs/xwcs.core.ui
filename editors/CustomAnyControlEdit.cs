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
using DevExpress.XtraEditors.CustomEditor;
using System.Reflection;

namespace xwcs.core.ui.editors
{
	[UserRepositoryItem("RegisterCustomAnyControl")]
	public class RepositoryItemCustomAnyControl : RepositoryItemAnyControl
	{
		static RepositoryItemCustomAnyControl()
		{
			RegisterCustomAnyControl();
		}

		public const string CustomEditName = "CustomAnyControlEdit";

		public RepositoryItemCustomAnyControl()
		{
		}

		public override string EditorTypeName
		{
			get
			{
				return CustomEditName;
			}
		}

		public static void RegisterCustomAnyControl()
		{
			Image img = null;
			EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(CustomAnyControlEdit), typeof(RepositoryItemCustomAnyControl), typeof(AnyControlEditViewInfo), new AnyControlEditPainter(), true, img));
		}

		public override void Assign(RepositoryItem item)
		{
			BeginUpdate();
			try
			{
				base.Assign(item);
				RepositoryItemCustomAnyControl source = item as RepositoryItemCustomAnyControl;
				if (source == null) return;
				//
			}
			finally
			{
				EndUpdate();
			}
		}

		private Type _controlType;
		public Type ControlType { 
			get
			{
				return _controlType;
			} 
			set
			{
				if (value == null && base.Control == null) return;
				if (value == null && base.Control != null) {
					base.Control = null;
					return;
				}
                /*
				ConstructorInfo cConstructor = value.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.NonPublic, null, new Type[] { }, null);
				if (cConstructor == null)
				{
					base.Control = null;
					return;
				}
				base.Control = cConstructor.Invoke(null) as IAnyControlEdit;
                */
                base.Control = (IAnyControlEdit)Activator.CreateInstance(value);
                _controlType = value;
			} 
		}

		protected override void Dispose(bool disposing)
		{
			if(Control != null) {
				(Control as XtraUserControl)?.Dispose();
				ControlType = null;
			}
			base.Dispose(disposing);
		}
	}

	[ToolboxItem(true)]
	public class CustomAnyControlEdit : AnyControlEdit
	{
        
        
        static CustomAnyControlEdit()
		{
			RepositoryItemCustomAnyControl.RegisterCustomAnyControl();
		}

		public CustomAnyControlEdit()
		{
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new RepositoryItemCustomAnyControl Properties
		{
			get
			{
				return base.Properties as RepositoryItemCustomAnyControl;
			}
		}

		public override string EditorTypeName
		{
			get
			{
				return RepositoryItemCustomAnyControl.CustomEditName;
			}
		}

		public override object EditValue
		{
			get
			{
				return base.EditValue;
			}
			set
			{

				if (Properties.Control != null)
				{
					(Properties.Control as IAnyControlEdit).EditValue = value;
					base.EditValue = (Properties.Control as IAnyControlEdit).EditValue;
				}
				else
				{
					base.EditValue = value;
				}

			}
		}

        /*
         * Added cause devexpress form 16.2 support collections as fields in data layout
         * and it require DataSource field of control in case of collection field
         */
        public object DataSource
        {
            get
            {
                if (Properties.Control != null && Properties.Control as xwcs.core.db.binding.IDataSourceProvider != null)
                {
                    return (Properties.Control as xwcs.core.db.binding.IDataSourceProvider).DataSource;
                }

                throw new ArgumentException("CustomAnyControlEdit Control is not IDataSourceProvider");
            }
            set
            {

                if (Properties.Control != null && Properties.Control as xwcs.core.db.binding.IDataSourceProvider != null)
                {
                    (Properties.Control as xwcs.core.db.binding.IDataSourceProvider).DataSource = value;

                    return;
                }

                throw new ArgumentException("CustomAnyControlEdit Control is not IDataSourceProvider");
            }
        }
    }
}
