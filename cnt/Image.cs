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
	public partial class Image : UserControl, IContent
	{
		public Image()
		{
			InitializeComponent();
		}

		private string _PictureFilePath;
		public string PictureFilePath
		{
			get
			{
				return _PictureFilePath;
			}
			set
			{
				_PictureFilePath = value;
			}
		}

		void IContent.Load(string Path)
		{
			_PictureFilePath = Path;
			pictureBox1.Load(_PictureFilePath);
		}
	}
}
