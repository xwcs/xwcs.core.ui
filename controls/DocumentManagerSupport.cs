using System.Xml.Serialization;
using xwcs.core.manager;
using DevExpress.XtraBars.Docking2010.Views;
using xwcs.core.evt;
using System.Collections.Generic;
using xwcs.core.plgs.persistent;
using System;
using xwcs.core.controls;

namespace xwcs.core.ui.controls
{

	[XmlRoot("Documents", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class DocumentManagerState
    {
        [XmlArray("Items")]
        public core.controls.VisualControlInfo[] Documents;

        public DocumentManagerState() {}
    }

    [cfg.attr.Config("MainAppConfig")]
    public partial class DocumentManagerSupport : PersistentStateBase
    {
        private DevExpress.XtraBars.Docking2010.DocumentManager _manager;
        private SEventProxy _proxy;
		// Cast internal state
        protected DocumentManagerState state { get { return (DocumentManagerState)_State; } }
        private List<core.controls.IVisualControl> _controlsForSave = new List<core.controls.IVisualControl>();
        private DevExpress.XtraBars.BarItem[] _saveComponents;
        private DevExpress.XtraBars.BarItem[] _saveAllComponents;


		/// <summary>
		/// Each time some document is activated this is set
		/// So we can route global button events to correct document
		/// </summary>
        private xwcs.core.controls.IVisualControl _activeControl;

        public DocumentManagerSupport(DevExpress.XtraBars.Docking2010.DocumentManager manager, DevExpress.XtraBars.BarItem[] saveComponents, DevExpress.XtraBars.BarItem[] saveAllComponents)
        {
            _manager = manager;
            _State = new DocumentManagerState();

            _proxy = SEventProxy.getInstance();
            _proxy.addEventHandler<DocumentChangedEvent>(EventType.DocumentChangedEvent, HandleDocumentChanged);
            _proxy.addEventHandler<VisualControlActionEvent>(EventType.VisualControlActionEvent, HandleVisualControlAction);
            _saveComponents = saveComponents;
            _saveAllComponents = saveAllComponents;
        }

        private void HandleDocumentChanged(DocumentChangedEvent e)
        {
            SLogManager.getInstance().Info("HandleDocumentChanged received in DocumentManagerSupport");
            _controlsForSave.Add(e.RequestData.VisualControl);
            foreach (DevExpress.XtraBars.BarItem item in _saveAllComponents) item.Enabled = true;

            if (e.RequestData.VisualControl == _activeControl)
            {
                foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = true;
            }
        }

        private void HandleVisualControlAction(VisualControlActionEvent e)
        {
            SLogManager.getInstance().Info("HandleVisualControlAction received in DocumentManagerSupport");

			switch(e.RequestData.ActionKind) {
				case VisualControlActionKind.Activated:
					_activeControl = null;

					foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = false;

					core.controls.IVisualControl vc = e.RequestData.VisualControl;
					core.controls.VisualControlInfo vci = vc.VisualControlInfo;

					if (vci != null)
					{
						if (_controlsForSave.Find(x => x.VisualControlInfo == vci) != null)
						{
							foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = true;
						}
						_activeControl = vc;
					}
					break;
				case VisualControlActionKind.Disposed:
				default:

					_activeControl = null;
					foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = false;
					break;
			}
            
        }

        public void SaveChangedControls()
        {
            BaseDocumentCollection col = _manager.View.Documents;

            foreach (BaseDocument document in col)
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
                BaseDocumentCollection col = _manager.View.Documents;

                foreach (BaseDocument document in col)
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

        protected override void BeforeSaveState()
        {
            BaseDocumentCollection col = _manager.View.Documents;

			try {
				state.Documents = new core.controls.VisualControlInfo[col.Count];
				int i = 0;
				foreach (BaseDocument document in col)
				{
					VisualControl vc = (VisualControl)document.Control;
					if (vc != null)
					{
						// save control state
						(vc as IPersistentState)?.SaveState();
						state.Documents[i++] = vc.VisualControlInfo;
					}
				}
			}catch(Exception ex) {
				SLogManager.getInstance().Error(ex.Message);
			}            
        }

        protected override void AfterLoadState()
        {
			//state can be not loaded!!!!
            if(_State == null) {
				_State = new DocumentManagerState();
			}else {
				foreach (core.controls.VisualControlInfo vci in state.Documents)
				{
					VisualControl pluginControl = (VisualControl)vci.restoreInstance();
					_manager.BeginUpdate();
					BaseDocument document = _manager.View.AddDocument(pluginControl);
					document.Caption = vci.Name;
					document.ControlName = vci.Name;
					_manager.EndUpdate();

					// restore control state
					(pluginControl as IPersistentState)?.LoadState();
					(pluginControl as IVisualControl)?.Start(VisualControlStartingKind.StartingPersisted);
				}
			}			
        }
    }
}
