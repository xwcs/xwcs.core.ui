using System.Xml.Serialization;
using xwcs.core.manager;
using xwcs.core.plgs;
using DevExpress.XtraBars.Docking2010.Views;
using xwcs.core.evt;
using System.Collections.Generic;

namespace xwcs.core.ui.controls
{

    [XmlRootAttribute("Documents", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class DocumentManagerState : ManagerStateBase
    {
        [XmlArrayAttribute("Items")]
        public xwcs.core.controls.VisualControlInfo[] Documents;

        public DocumentManagerState() {; }
    }

    [xwcs.core.cfg.attr.Config("MainAppConfig")]
    public partial class DocumentManagerSupport : ManagerWithStateBase
    {
        private DevExpress.XtraBars.Docking2010.DocumentManager _manager;
        private SEventProxy _proxy;
        protected DocumentManagerState state { get { return (DocumentManagerState)_managerState; } }
        private List<xwcs.core.controls.IVisualControl> _controlsForSave = new List<xwcs.core.controls.IVisualControl>();
        private DevExpress.XtraBars.BarItem[] _saveComponents;
        private DevExpress.XtraBars.BarItem[] _saveAllComponents;
        private xwcs.core.controls.IVisualControl _activeControl;

        public DocumentManagerSupport(DevExpress.XtraBars.Docking2010.DocumentManager manager, DevExpress.XtraBars.BarItem[] saveComponents, DevExpress.XtraBars.BarItem[] saveAllComponents)
        {
            _manager = manager;
            _managerState = new DocumentManagerState();

            _proxy = SEventProxy.getInstance();
            _proxy.addEventHandler(EventType.DocumentChangedEvent, HandleDocumentChanged);
            _proxy.addEventHandler(EventType.DocumentActivatedEvent, HandleDocumentActivated);
            _saveComponents = saveComponents;
            _saveAllComponents = saveAllComponents;
        }

        private void HandleDocumentChanged(Event e)
        {
            SLogManager.getInstance().Info("HandleDocumentChanged received in DocumentManagerSupport");
            DocumentChangedRequest ee = (DocumentChangedRequest)e.data;
            _controlsForSave.Add(ee.visualControl);
            foreach (DevExpress.XtraBars.BarItem item in _saveAllComponents) item.Enabled = true;

            if (ee.visualControl == _activeControl)
            {
                foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = true;
            }
        }

        private void HandleDocumentActivated(Event e)
        {
            SLogManager.getInstance().Info("HandleDocumentActivated received in DocumentManagerSupport");

            _activeControl = null;
            foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = false;
            DocumentActivatedRequest ee = (DocumentActivatedRequest)e.data;
            xwcs.core.controls.VisualControlInfo vci = ee.visualControl.VisualControlInfo; 

            if (vci != null)
            {
                if (_controlsForSave.Find(x => x.VisualControlInfo == vci) != null)
                {
                    foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = true;
                }
                _activeControl = ee.visualControl;
            }
        }

        public void SaveChangedControls()
        {
            DevExpress.XtraBars.Docking2010.Views.BaseDocumentCollection col = _manager.View.Documents;

            foreach (DevExpress.XtraBars.Docking2010.Views.BaseDocument document in col)
            {
                VisualControl vc = (VisualControl)document.Control;
                if (vc != null)
                {
                    if (_controlsForSave.Find(x => x.VisualControlInfo == vc.VisualControlInfo) != null)
                    {
                        vc.SaveChanges();
                    }
                }
            }
            _controlsForSave.Clear();

            foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = false;
            foreach (DevExpress.XtraBars.BarItem item in _saveAllComponents) item.Enabled = false;
        }

        public void saveSelectedControl()
        {
            if (_activeControl != null)
            {
                DevExpress.XtraBars.Docking2010.Views.BaseDocumentCollection col = _manager.View.Documents;

                foreach (DevExpress.XtraBars.Docking2010.Views.BaseDocument document in col)
                {
                    VisualControl vc = (VisualControl)document.Control;
                    if (vc.VisualControlInfo == _activeControl.VisualControlInfo)
                    {
                        vc.SaveChanges();
                        _controlsForSave.Remove(_activeControl);
                        foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = false;
                        if (_controlsForSave.Count == 0)
                        {
                            foreach (DevExpress.XtraBars.BarItem item in _saveAllComponents) item.Enabled = false;
                        }
                    }
                }
            }
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
            }
        }
    }
}
