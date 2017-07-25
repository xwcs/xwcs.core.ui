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

		void IContent.Load(string Path)
		{
			_PDFFilePath = Path;
			pdfViewer1.LoadDocument(_PDFFilePath);
		}

		void IContent.Next()
		{
		}
		void IContent.Prev()
		{
		}

		void IContent.Close()
		{
			pdfViewer1.CloseDocument();
		}
	}
}
