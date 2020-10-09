using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xwcs.core.ui.app
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        public WindowWrapper()
        {
            //Get FriendlyName from Application Domain
            string strFriendlyName = AppDomain.CurrentDomain.FriendlyName;

            //Get process collection by the application name without extension (.exe)
            Process[] pro = Process.GetProcessesByName(
                strFriendlyName.Substring(0, strFriendlyName.LastIndexOf('.')));

            _hwnd = pro[0].MainWindowHandle;
        }

        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
