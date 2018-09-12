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
	public partial class pdf : UserControl, IContent
	{
		public pdf()
		{
			InitializeComponent();
		}

		private string _PDFFilePath;
		public string PictureFilePath
		{
			get
			{
				return _PDFFilePath;
			}
			set
			{
				_PDFFilePath = value;
			}
		}

		public void LoadContent(string Path, bool force = false)
		{
			_PDFFilePath = Path;
			pdfViewer1.LoadDocument(_PDFFilePath);
		}

		public void Next()
		{
		}
		public void Prev()
		{
		}

		public void Close()
		{
			pdfViewer1.CloseDocument();
		}

		public void First()
		{
		}

		public void Last()
		{
		}

	}
}
