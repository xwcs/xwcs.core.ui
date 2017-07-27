using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

			//Must be loaded by stream because .Net picturebox locking file using load()
			System.Drawing.Image image1 = null;
			using (FileStream stream = new FileStream(_PictureFilePath, FileMode.Open))
			{
				image1 = System.Drawing.Image.FromStream(stream);
			}
			pictureBox1.Image = image1;
		}

		void IContent.Next()
		{		
		}

		void IContent.Prev()
		{		
		}

		void IContent.Close()
		{
			if (pictureBox1.Image != null) pictureBox1.Image = null;
		}

		void IContent.First()
		{
		}

		void IContent.Last()
		{
		}

	}
}
