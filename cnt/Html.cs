using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xwcs.core.ui.cnt
{
	public partial class Html : UserControl, IContent
	{
		public Html()
		{
			InitializeComponent();
		}

		private string _HTMLUrl;
		public string HTMLUrl
		{
			get
			{
				return _HTMLUrl;
			}
			set
			{
				_HTMLUrl = value;
			}
		}

		void IContent.Load(string URL)
		{
			_HTMLUrl = URL;
			webBrowser1.Navigate(_HTMLUrl);
		}
	}
}
