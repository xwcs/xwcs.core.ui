using System.Xml.Serialization;
using xwcs.core.manager;
using DevExpress.XtraBars.Docking2010.Views;
using xwcs.core.evt;
using System.Collections.Generic;
using xwcs.core.plgs.persistent;
using System;
using xwcs.core.controls;
using System.Runtime.Serialization;

namespace xwcs.core.ui.controls
{
	/// <summary>
	/// Different kind of document controls layout
	/// </summary>
	public enum DocumentsLayoutKind {
		overlap,		    // 1 doc visible
		horizontal,		    // 2 docs visible side by side
		vertical,           // 2 docs visible one over other
		custom,			    // 2 manually adjusted layout
        TWOvsONEhorisontal, // 2 vertical on left and one big on right
        TWOvsONEvertical    // 2 horisontal above one big
	}

	[DataContract]
	public class DocumentManagerState
    {
        [DataMember]
        public VisualControlInfo[] Documents;

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

        private void HandleDocumentChanged(object sender, DocumentChangedEvent e)
        {
            SLogManager.getInstance().Info("HandleDocumentChanged received in DocumentManagerSupport");
            _controlsForSave.Add(e.RequestData.VisualControl);
            foreach (DevExpress.XtraBars.BarItem item in _saveAllComponents) item.Enabled = true;

            if (e.RequestData.VisualControl == _activeControl)
            {
                foreach (DevExpress.XtraBars.BarItem item in _saveComponents) item.Enabled = true;
            }
        }

        private void HandleVisualControlAction(object sender, VisualControlActionEvent e)
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
				state.Documents = new VisualControlInfo[col.Count];
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
				BaseDocument first = null;
				_manager.BeginUpdate();

				//state is casted _State
				foreach (VisualControlInfo vci in state.Documents)
				{
					//do restore so it will mantain vci
					VisualControl pluginControl = (VisualControl)vci.restoreInstance();
					BaseDocument document = _manager.View.AddDocument(pluginControl);
					document.Caption = vci.Name;
					document.ControlName = vci.Name;
					document.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.False;
					
					// restore control state
					(pluginControl as IPersistentState)?.LoadState();
					(pluginControl as IVisualControl)?.Start(VisualControlStartingKind.StartingPersisted);
					if(first == null) {
						first = document;
					}
				}
				_manager.EndUpdate();
				_manager.View.Controller.Activate(first);
				//release VCI from state
				state.Documents = null;
			}			
        }

		public BaseDocument getDocumentByVCI(xwcs.core.controls.VisualControlInfo visualControlInfo)
		{
			foreach (DevExpress.XtraBars.Docking2010.Views.BaseDocument document in _manager.View.Documents)
			{
				VisualControl vc = (VisualControl)document.Control;
				if ((vc != null) && (vc.VisualControlInfo.GUID == visualControlInfo.GUID)) return document;
			}
			return null;
		}
	}
}
