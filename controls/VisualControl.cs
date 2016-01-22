namespace xwcs.core.ui.controls
{
    public partial class VisualControl : DevExpress.XtraEditors.XtraUserControl, xwcs.core.ui.controls.IControl
    {
        protected xwcs.core.ui.controls.ControlInfo _controlInfo;

        public ControlInfo controlInfo
        {
            get { return _controlInfo; }
        }
    }
}
