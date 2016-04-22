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
	public partial class CustomIntervalPopupControl : PopupContainerControl //UserControl
	{
		private IntervalEdit intervalEdit;

	
		public CustomIntervalPopupControl(IntervalEdit intervalEdit)
		{
			InitializeComponent();
			this.intervalEdit = intervalEdit;
		}
	}
}
