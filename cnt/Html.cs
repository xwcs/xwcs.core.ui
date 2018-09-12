using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.evt;
using System.Text.RegularExpressions;

namespace xwcs.core.ui.cnt
{
	public partial class Html : UserControl, IContent
	{
		public Html()
		{
			InitializeComponent();
		}

		private string _HTMLUrl = "";
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

		public virtual void LoadContent(string URL, bool force = false)
		{
			_HTMLUrl = URL;
			webBrowser1.Navigate(_HTMLUrl);
		}

		public void Next()
		{
			webBrowser1.GoForward();
            progressPanel1.Visible = false;
        }

        public void Prev()
		{
			webBrowser1.GoBack();
            progressPanel1.Visible = false;		
		}

		public void Close()
		{
		}

		public void First()
		{
		}

		public void Last()
		{
		}

	}
}
