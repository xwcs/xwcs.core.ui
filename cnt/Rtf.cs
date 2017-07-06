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

        public void Load(string Path)
        {
            _RtfFilePath = Path;
            richEditControl1.RtfText = File.ReadAllText(_RtfFilePath);

        }
    }
}
