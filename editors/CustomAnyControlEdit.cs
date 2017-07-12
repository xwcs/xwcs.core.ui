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
using System.Windows.Forms;
using DevExpress.Utils.Drawing;

namespace xwcs.core.ui.editors
{
	[UserRepositoryItem("RegisterCustomAnyControl")]
	public class RepositoryItemCustomAnyControl : RepositoryItemAnyControl, core.db.binding.IEditorsHostProvider
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
			EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(CustomAnyControlEdit), typeof(RepositoryItemCustomAnyControl), typeof(CustomAnyControlEditViewInfo), new AnyControlEditPainter(), true, img));
		}

        /* NOT CALLED */
		public override void Assign(RepositoryItem item)
		{
			BeginUpdate();
			try
			{
				base.Assign(item);
				RepositoryItemCustomAnyControl source = item as RepositoryItemCustomAnyControl;
                if (source != null)
                {
                }
			}
			finally
			{
				EndUpdate();
			}
		}

		private Type _controlType = null;
		public Type ControlType { 
			get
			{
				return _controlType;
			} 
			set
			{
                if (value == null && base.Control == null) return;
                if (value == null && base.Control != null)
                {
                    if (base.Control is IDisposable)
                    {
                        (base.Control as IDisposable).Dispose();
                    }
                    return; // we destroy control setting type to NULL
                }
                _controlType = value;
                CreateControl();
			} 
		}

        /// <summary>
        /// Control can be created also externally
        /// in EditorsHost using this name
        /// this field if this field is set, it 
        /// will try override control creation
        /// and type will be got dynamically
        /// if creation return null
        /// control will be created using type
        /// </summary>
        private string _controlName = "";
        public string ControlName
        {
            get { return _controlName; }
            set {
                _controlName = value;
                CreateControl();
            }
        }

        public core.db.binding.IEditorsHost EditorsHost { get; set; } = null;
        



        /// <summary>
        /// 1: Name win and we try to call EditorsHost for object creation
        /// 2: In name not present create control using type
        /// </summary>
        private void CreateControl()
        {
            if (_controlName.Length > 0)
            {
                if (ReferenceEquals(null, EditorsHost)) throw new ApplicationException("Missing editors host!");
                System.Windows.Forms.Control tmp = EditorsHost.GetCustomEditingControl(_controlName);

                _controlType = tmp.GetType();
                if (!(tmp is IAnyControlEdit)) throw new ApplicationException(string.Format("Control [{0}] must be IAnyControlEdit!", tmp.GetType().Name));
                base.Control = (IAnyControlEdit)tmp;
            }
            else
            {
                base.Control = (IAnyControlEdit)Activator.CreateInstance(_controlType);
            }

            if (!ReferenceEquals(null, EditorsHost) && base.Control is core.db.binding.IEditorsHostProvider)
            {
                (base.Control as core.db.binding.IEditorsHostProvider).EditorsHost = EditorsHost;
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

    public class CustomAnyControlEditViewInfo : AnyControlEditViewInfo//, IHeightAdaptable
    {
        public CustomAnyControlEditViewInfo(RepositoryItem item) : base(item)
        {
        }

        /* used for debug
        protected override void OnEditValueChanged()
        {
            base.OnEditValueChanged();
            if(this.DrawControlInstance != null)
            {
                Console.WriteLine(string.Format("Edit value changed : {0}, {1}", this.EditValue, (this.DrawControlInstance as Control).Size.ToString()));
            }else
            {
                Console.WriteLine(string.Format("Edit value changed : {0}", this.EditValue));
            }
            
        }
        */
    }


    /// <summary>
    /// This class is control proxy
    /// </summary>
    [ToolboxItem(true)]
    public class CustomAnyControlEdit : AnyControlEdit //, DevExpress.Utils.Controls.IXtraResizableControl
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
				}
                base.EditValue = value;
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

        /* USE for debug
         
        protected override void UpdateViewInfo(Graphics g)
        {
            base.UpdateViewInfo(g);
        }
        protected override void OnAfterUpdateViewInfo()
        {
            base.OnAfterUpdateViewInfo();
            this.UpdateControlBounds();
        }
        */

        /// <summary>
        /// This will forward size changed to controll
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="specified"></param>
        protected override void SetBoundsCore(
            int x,
            int y,
            int width,
            int height,
            BoundsSpecified specified
        )
        {
            base.SetBoundsCore(x, y, width, height, specified);
            if(Properties.Control != null && Properties.Control is Control && (specified == BoundsSpecified.Height || specified == BoundsSpecified.Size || specified == BoundsSpecified.All))
            {
                (Properties.Control as Control).Height = height;
            }
        }    
    }
}
