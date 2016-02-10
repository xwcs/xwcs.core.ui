using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xwcs.core.ui.controls
{
    public partial class DockPanelBase : DevExpress.XtraBars.Docking.DockPanel
    {
        public DockPanelBase() : base(true, DevExpress.XtraBars.Docking.DockingStyle.Float, null)
        {
            this.ClosedPanel += DockPanelBase_ClosedPanel; ;
        }

        private void DockPanelBase_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            DockManager.RemovePanel(this);
        }

        public DockPanelBase(bool createControlContainer, DevExpress.XtraBars.Docking.DockingStyle dock, DevExpress.XtraBars.Docking.DockManager dockManager)
            : base(createControlContainer, dock, dockManager)
        { }
    }
}
