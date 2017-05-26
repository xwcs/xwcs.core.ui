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
                    base.Control = null;
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

                // do some settings
                //tmp.Dock = System.Windows.Forms.DockStyle.Fill;
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

        /*
        protected delegate Size MD(Graphics g);
        */
        /*
        protected override Size CalcContentSize(Graphics g)
        {
            
            
            if (this.DrawControlInstance == null)
            {
                return base.CalcContentSize(g);
            }

            return this.DrawControlInstance.CalcSize(g);
        }

        public override void CalcViewInfo(Graphics g)
        {
            base.CalcViewInfo(g);
        }
    */

        /*
        int IHeightAdaptable.CalcHeight(GraphicsCache cache, int width)
        {
            BorderObjectInfoArgs borderObjectInfoArgs = new BorderObjectInfoArgs(cache);
            borderObjectInfoArgs.Bounds = new Rectangle(0, 0, width, 100);
            Rectangle objectClientRectangle = this.BorderPainter.GetObjectClientRectangle((ObjectInfoArgs)borderObjectInfoArgs);
            if (!(this.BorderPainter is EmptyBorderPainter) && !(this.BorderPainter is InplaceBorderPainter))
                objectClientRectangle.Inflate(-1, -1);
            
            return objectClientRectangle.Height;
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

		public CustomAnyControlEdit(){}

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

        /*
        public bool IsCaptionVisible {
            get
            {
                if (Properties.Control != null && Properties.Control is DevExpress.Utils.Controls.IXtraResizableControl)
                {
                    return (Properties.Control as DevExpress.Utils.Controls.IXtraResizableControl).IsCaptionVisible;
                }

                return false;
            }
        }
        //
        // Riepilogo:
        //     When implemented by a control, specifies its default maximum size which is in
        //     effect when the control is displayed within a Layout Control.
        public Size MaxSize
        {
            get
            {
                if (Properties.Control != null && Properties.Control is DevExpress.Utils.Controls.IXtraResizableControl)
                {
                    return (Properties.Control as DevExpress.Utils.Controls.IXtraResizableControl).MaxSize;
                }

                return MaximumSize;
            }
        }
        //
        // Riepilogo:
        //     When implemented by a control, specifies its default minimum size which is in
        //     effect when the control is displayed within a Layout Control.
        public Size MinSize
        {
            get
            {
                if (Properties.Control != null && Properties.Control is DevExpress.Utils.Controls.IXtraResizableControl)
                {
                    return (Properties.Control as DevExpress.Utils.Controls.IXtraResizableControl).MinSize;
                }

                return MaximumSize;
            }
        }
        */
        protected override void UpdateViewInfo(Graphics g)
        {
            base.UpdateViewInfo(g);
        }

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
