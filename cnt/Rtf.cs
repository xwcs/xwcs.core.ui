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
    public partial class Rtf : UserControl, IContent
    {
        public Rtf()
        {
            InitializeComponent();
        }

        private string _RtfFilePath;
        public string RtfFilePath
        {
            get {
                return _RtfFilePath;       
            }
            set {
                _RtfFilePath = value;
            }
        }

		public void LoadContent(string Path, bool force = false)
		{
			_RtfFilePath = Path;
			richEditControl1.RtfText = File.ReadAllText(_RtfFilePath);			
		}

		public void Next()
		{
		}
		public void Prev()
		{
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
