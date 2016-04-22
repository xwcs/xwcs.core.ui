using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.db.binding.attributes;
using xwcs.core.db.fo;

namespace xwcs.core.ui.editors.attributes
{
	

	public class ResizableToolStripControlHost : ToolStripControlHost {
		public ResizableToolStripControlHost(UserControl uc) : base(uc) {
		}

		public void AdjustSize(ToolStripDropDown parent) {
			Size = new Size(parent.Width - 3, parent.Height - 3);
			Control.SuspendLayout();
			Control.Size = Size;
			Control.ResumeLayout();
		}
	}

	public class ResizableToolStripDropDown : PopupControlContainer // ToolStripDropDown
	{
		

		public ResizableToolStripDropDown()
		{
			AutoSize = false;
			DoubleBuffered = true;
			//TopLevel = false;
			//CanOverflow = true;
			//AutoClose = true;
			//DropShadowEnabled = true;

			SizeChanged += (object ss, EventArgs ee) =>
			{
				//Console.WriteLine(string.Format("dd {0},{1}", Size.Width, Size.Height));
				//(Items[0] as ResizableToolStripControlHost)?.AdjustSize(this);
			};
		}

		

		public void AddUserControl(UserControl uc) {
			Controls.Add(uc);
			/*
			Items.Clear();
			Items.Add(new ResizableToolStripControlHost(uc)
			{
				Margin = Padding.Empty,
				Padding = Padding.Empty,
				AutoSize = false
			});
			(Items[0] as ResizableToolStripControlHost)?.AdjustSize(this);
			*/
		}

		#region RESIZE

		private Rectangle BottomGripBounds
		{
			get
			{
				Rectangle rect = ClientRectangle;
				rect.Y = rect.Bottom - 4;
				rect.Height = 8;
				return rect;
			}
		}

		private Rectangle RightGripBounds
		{
			get
			{
				Rectangle rect = ClientRectangle;
				rect.X = rect.Right - 4;
				rect.Width = 8;
				return rect;
			}
		}

		private Rectangle BottomRightGripBounds
		{
			get
			{
				Rectangle rect = BottomGripBounds;
				rect.X = rect.Width - 4;
				rect.Width = 8;
				return rect;
			}
		}


		protected override void WndProc(ref Message m)
		{

			if (m.Msg == NativeMethods.WM_NCHITTEST)
			{
				// fetch out X & Y out of the message
				//int x = NativeMethods.LOWORD(m.LParam);
				//int y = NativeMethods.HIWORD(m.LParam);

//				Console.WriteLine(string.Format("{0},{1}", x, y));
				// convert to client coords
				Point clientLocation = PointToClient(new Point(NativeMethods.LOWORD(m.LParam), NativeMethods.HIWORD(m.LParam)));
				// prefer bottom right check
				if (BottomRightGripBounds.Contains(clientLocation))
				{
					m.Result = (IntPtr)NativeMethods.HTBOTTOMRIGHT;
					return;
				}
				// the bottom check
				if (BottomGripBounds.Contains(clientLocation))
				{
					m.Result = (IntPtr)NativeMethods.HTBOTTOM;
					return;
				}

				// the bottom check
				if (RightGripBounds.Contains(clientLocation))
				{
					m.Result = (IntPtr)NativeMethods.HTRIGHT;
					return;
				}
				// else, let the base WndProc handle it.
			}
			base.WndProc(ref m);
		}

		internal class NativeMethods
		{
			internal const int
							WM_NCHITTEST = 0x0084,
							HTBOTTOM = 15,
							HTRIGHT = 11,
							HTBOTTOMRIGHT = 17,
							WS_OVERLAPPED = 0x00000000,
							WS_POPUP = unchecked((int)0x80000000),
							WS_CHILD = 0x40000000,
							WS_MINIMIZE = 0x20000000,
							WS_VISIBLE = 0x10000000,
							WS_DISABLED = 0x08000000,
							WS_CLIPSIBLINGS = 0x04000000,
							WS_CLIPCHILDREN = 0x02000000,
							WS_MAXIMIZE = 0x01000000,
							WS_CAPTION = 0x00C00000,
							WS_BORDER = 0x00800000,
							WS_DLGFRAME = 0x00400000,
							WS_VSCROLL = 0x00200000,
							WS_HSCROLL = 0x00100000,
							WS_SYSMENU = 0x00080000,
							WS_THICKFRAME = 0x00040000,
							WS_TABSTOP = 0x00010000,
							WS_MINIMIZEBOX = 0x00020000,
							WS_MAXIMIZEBOX = 0x00010000,
							WS_EX_DLGMODALFRAME = 0x00000001,
							WS_EX_MDICHILD = 0x00000040,
							WS_EX_TOOLWINDOW = 0x00000080,
							WS_EX_CLIENTEDGE = 0x00000200,
							WS_EX_CONTEXTHELP = 0x00000400,
							WS_EX_RIGHT = 0x00001000,
							WS_EX_LEFT = 0x00000000,
							WS_EX_RTLREADING = 0x00002000,
							WS_EX_LEFTSCROLLBAR = 0x00004000,
							WS_EX_CONTROLPARENT = 0x00010000,
							WS_EX_STATICEDGE = 0x00020000,
							WS_EX_APPWINDOW = 0x00040000,
							WS_EX_LAYERED = 0x00080000,
							WS_EX_TOPMOST = 0x00000008,
							WS_EX_LAYOUTRTL = 0x00400000,
							WS_EX_NOINHERITLAYOUT = 0x00100000;
			internal static int HIWORD(int n)
			{
				return (n >> 16) & 0xffff;
			}
			internal static int HIWORD(IntPtr n)
			{
				return HIWORD(unchecked((int)(long)n));
			}
			internal static int LOWORD(int n)
			{
				return n & 0xffff;
			}
			internal static int LOWORD(IntPtr n)
			{
				return LOWORD(unchecked((int)(long)n));
			}
		}

		#endregion
	}

	public class BaseFilterForm : XtraForm
	{

		public BarManager BarMan
		{
			get;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IntervalEditAttribute : CustomAttribute
	{
		//private readonly ResizableToolStripDropDown _toolStripDropDown = new ResizableToolStripDropDown();
		private readonly PopupControlContainer _toolStripDropDown = new PopupControlContainer();

		public override void applyRetrievingAttribute(IDataLayoutExtender host, FieldRetrievingEventArgs e)
		{
			e.EditorType = typeof(DateExtEdit);
		}

		public override void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e)
		{

			//RepositoryItemIntervalEdit rle = e.RepositoryItem as RepositoryItemIntervalEdit;
			RepositoryItemDateExtEdit rle = e.RepositoryItem as RepositoryItemDateExtEdit;
			rle.PopupResizeMode = ResizeMode.LiveResize;
			rle.PopupBorderStyle = PopupBorderStyles.Simple;
			DataTable fo = rle.Fo;
			fo.Columns.Add(new DataColumn(e.FieldName, typeof(DateTime)));

			rle.KeyPress += (object s, KeyPressEventArgs ke) =>
			{
				TextEdit re = (s as TextEdit);
				
				if (ke.KeyChar == '<')
				{
					ke.Handled = true;
					IFilterDataLayoutExtender fe = host as IFilterDataLayoutExtender;
					fe?.onFilterFieldEvent(new FilterFieldEventData { Field = re, FREA = e });
					/*
					FieldExpressionControl fc = new FieldExpressionControl();

					

					fc.filterEditorControl.SourceControl = fo;
					fc.filterEditorControl.FilterString = string.Format("[{0}] < '{1}'", e.FieldName, (typeof(DateTime).IsValueType ? Activator.CreateInstance(typeof(DateTime)) : ""));
					fc.filterEditorControl.BeforeShowValueEditor += (object ss, ShowValueEditorEventArgs ee) =>
					{
						ee.CustomRepositoryItem = new RepositoryItemDateEdit();	
						
					};
					fc.filterEditorControl.FilterControl.PopupMenuShowing += (spm, spe) =>
					{
						int i = 0;
						spe.Menu.BeforePopup += (ms, me) =>
						{
							int a = 0;
						};
					};
					fc.filterEditorControl.FilterControl.ShowOperandTypeIcon = true;

					//_toolStripDropDown.AddUserControl(fc);					 
					_toolStripDropDown.Controls.Add(fc);
					_toolStripDropDown.Size = new Size(re.Width, 200);
					
					Control parent = re.Parent;
					re.EditValue = null;
					re.Properties.NullValuePromptShowForEmptyValue = true;
					re.Properties.NullValuePrompt = "[nrecord] < 0";
					re.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
					re.Properties.ShowNullValuePromptWhenFocused = true;

					Point pt = parent.PointToScreen(new Point(re.Location.X, re.Location.Y + re.Height));

					BaseFilterForm bff = re.Parent.FindForm() as BaseFilterForm;

					
					_toolStripDropDown.ShowPopup(bff.BarMan, new Point(100,100));

					//DevExpress.XtraEditors.Popup.PopupBaseForm.ForceRemotingCompatibilityMode = true;



					fc.filterEditorControl.FilterControl.Focus();
					*/

					
				}

				if (ke.KeyChar == ':')
				{
					ke.Handled = true;

					FieldExpressionControl fc = new FieldExpressionControl();



					fc.filterEditorControl.SourceControl = fo;
					fc.filterEditorControl.FilterString = "[nrecord] between (0,0)";
					fc.filterEditorControl.FilterControl.ShowOperandTypeIcon = true;

					//_toolStripDropDown.AddUserControl(fc);
					_toolStripDropDown.Size = new Size(re.Width, 200);

					Control parent = re.Parent;
					Point pt = parent.PointToScreen(new Point(re.Location.X, re.Location.Y + re.Height));
					BaseFilterForm bff = Form.ActiveForm as BaseFilterForm;
					_toolStripDropDown.ShowPopup(bff.BarMan, pt);
					fc.filterEditorControl.FilterControl.Focus();
				}

				if (ke.KeyChar == '>')
				{
					ke.Handled = true;

					FieldExpressionControl fc = new FieldExpressionControl();



					fc.filterEditorControl.SourceControl = fo;
					fc.filterEditorControl.FilterString = "[nrecord] > 0";
					fc.filterEditorControl.FilterControl.ShowOperandTypeIcon = true;

					//_toolStripDropDown.AddUserControl(fc);
					_toolStripDropDown.Size = new Size(re.Width, 200);

					Control parent = re.Parent;
					Point pt = parent.PointToScreen(new Point(re.Location.X, re.Location.Y + re.Height));
					BaseFilterForm bff = Form.ActiveForm as BaseFilterForm;
					_toolStripDropDown.ShowPopup(bff.BarMan, pt);
					//fc.filterEditorControl.FilterControl.Focus();
				}
			};
			rle.ReadOnly = false;
			rle.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			//rle.OwnerEdit.DataBindings.Add();

			//add field in repository item
			
		}
	}
}
