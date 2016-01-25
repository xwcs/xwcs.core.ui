using System.Xml.Serialization;
using xwcs.core.manager;
using xwcs.core.plgs;
using DevExpress.XtraBars.Docking2010.Views;

namespace xwcs.core.ui.controls
{

    [XmlRootAttribute("Documents", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class DocumentManagerState : ManagerStateBase
    {
        [XmlArrayAttribute("Items")]
        public xwcs.core.controls.VisualControlInfo[] Documents;
    }

    [xwcs.core.cfg.attr.Config("MainAppConfig")]
    public partial class DocumentManagerSupport : ManagerWithStateBase
    {
        private DevExpress.XtraBars.Docking2010.DocumentManager _manager;
        private SPluginsLoader _loader;

        protected DocumentManagerState state { get { return (DocumentManagerState)_managerState; } }

        public DocumentManagerSupport(DevExpress.XtraBars.Docking2010.DocumentManager manager)
        {
            _manager = manager;
            _loader = SPluginsLoader.getInstance();
            _managerState = new DocumentManagerState();
        }

        protected override void beforeSave()
        {
            DevExpress.XtraBars.Docking2010.Views.BaseDocumentCollection col = _manager.View.Documents;

            state.Documents = new xwcs.core.controls.VisualControlInfo[col.Count];
            int i = 0;
            foreach (DevExpress.XtraBars.Docking2010.Views.BaseDocument document in col)
            {
                VisualControl vc = (VisualControl)document.Control;
                if (vc != null)
                {
                    state.Documents[i++] = vc.VisualControlInfo;
                }
            }
        }

        protected override void afterLoad()
        {
            foreach(xwcs.core.controls.VisualControlInfo vci in state.Documents)
            {
                VisualControl pluginControl = (VisualControl)vci.createInstance();
                _manager.BeginUpdate();
                BaseDocument document = _manager.View.AddDocument(pluginControl);
                document.Caption = vci.Name;
                document.ControlName = vci.Name;
                _manager.EndUpdate();

                /*
                IVisualPlugin plugin = (IVisualPlugin)_loader.getPluginByGuid(ci.GUID);
                if (plugin != null)
                {
                    VisualControl pluginControl = (VisualControl)plugin.getControlByGuid(ci.GUID);
                    if (pluginControl != null)
                    {
                        _manager.BeginUpdate();
                        BaseDocument document = _manager.View.AddDocument(pluginControl);
                        document.Caption = pluginControl.controlInfo.Name;
                        document.ControlName = pluginControl.controlInfo.Name;
                        _manager.EndUpdate();
                    }
                }
                */
            }
        }
    }
}
