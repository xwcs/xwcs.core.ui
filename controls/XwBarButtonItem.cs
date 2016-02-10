using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xwcs.core.ui.controls
{
    public class XwBarButtonItem : DevExpress.XtraBars.BarButtonItem
    {
        public XwBarButtonItem(string c, int id)
        {
            Id = id;
            Caption = c;
        }
    }
}
