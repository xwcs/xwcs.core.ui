using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace xwcs.core.ui.editors
{
	public partial class TextInterval : PopupContainerControl // UserControl
	{
		private IntervalEdit _owner;

		public TextInterval(IntervalEdit owner)
		{
			_owner = owner;
			InitializeComponent();
			radioGroup1.Properties.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem(0, ">"));
			radioGroup1.Properties.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "<>"));
			radioGroup1.Properties.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "<"));


			ConfirmButton.Click += (s, e) =>
			{
				_owner.ClosePopup();
			};
		}
	}
}
