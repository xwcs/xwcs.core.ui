using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using xwcs.core.plgs;
using xwcs.core.evt;
using DevExpress.XtraEditors;
using xwcs.core.user;
using DevExpress.XtraBars.Docking2010.Views;
using xwcs.core.ui.controls;
using xwcs.core.manager;
using System.Threading;
using System.Globalization;

namespace xwcs.core.ui.app
{
    public partial class ApplicationFormBase : Form, IPluginHost // DevExpress.XtraEditors.XtraUserControl, IPluginHost
    {
        private SEventProxy     _proxy;         //just local singleton instance copy
        private SWidgetManager  _widgetManager; //main instance, who do it first it make it, this is just local singleton instance copy
        private SPluginsLoader  _loader;


        private User _user;
        private DocumentManagerSupport _managerSupport;
        private DockPanel dockPanelProperty;
        private DockPanel dockPanelOutput;

        public ApplicationFormBase()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                documentManager1.View.DocumentProperties.UseFormIconAsDocumentImage = false;
                documentManager1.View.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;
                tabbedView1.FloatingDocumentContainer = FloatingDocumentContainer.DocumentsHost;

                //NESKOR BUDE UROBENY CES LOGIN FORM OD NEJAKEHO USER PROVIDERU
                _user = new User();

                _proxy = SEventProxy.getInstance();
                _proxy.addEventHandler(EventType.AddToolBarRequestEvent, HandleAddToolbarRequestEvent);
                _proxy.addEventHandler(EventType.OpenPanelRequestEvent, HandleOpenPanelRequestEvent);

                //do it here before any other plugin will be loaded!!!!!
                _widgetManager = SWidgetManager.getInstance();

                //now we can load plugins
                _loader = SPluginsLoader.getInstance();
                _loader.LoadPlugins(this, "Plugins");

                _managerSupport = new DocumentManagerSupport(documentManager1);
            }            
        }

        private void HandleOpenPanelRequestEvent(Event e)
        {
            OpenPanelRequest ee = (OpenPanelRequest)e.data;
            xwcs.core.controls.VisualControlInfo vci = (xwcs.core.controls.VisualControlInfo)ee.Vci;

            VisualControl control = (VisualControl)vci.createInstance();
            if (control != null)
            {
                
                if (vci.DockStyle == core.controls.ControlDockStyle.PLGT_document)
                {
                    documentManager1.BeginUpdate();                  
                    BaseDocument document = documentManager1.View.AddDocument(control);
                    document.Caption = control.VisualControlInfo.Name;
                    document.ControlName = control.VisualControlInfo.Name;
                    documentManager1.EndUpdate();
                    documentManager1.View.Controller.Activate(document);
                }
                else if (vci.DockStyle == core.controls.ControlDockStyle.PLGT_status)
                {
                    //be sure panel is on
                    showOutputPanel();

                    dockPanelOutput.ControlContainer.Controls.Add(control);

                    
                }
                else
                {
                    DockPanel dockPanel1 = dockManager1.AddPanel(DockingStyle.Top);
                    dockPanel1.ID = ee.Vci.GUID;
                    dockPanel1.Text = control.VisualControlInfo.Name;
                    dockPanel1.Height = 400;
                    dockPanel1.FloatSize = new Size(500, 400);

                    control.Dock = DockStyle.Fill;
                    dockPanel1.ControlContainer.Controls.Add(control);
                }
            }
        }

        private void HandleAddToolbarRequestEvent(Event e)
        {
            //TODO : dorobit
            AddToolBarRequest ee = (AddToolBarRequest)e.data;
            MenuAddRequest[] menu = ee.content;

            foreach (MenuAddRequest mar in menu)
            {
                switch (mar.destination)
                {
                    case MenuDestination.MENU_file_open: barSubItem_FileOpen.AddItem(mar.content); break;
                    case MenuDestination.MENU_ViewOtherWindows: barSubItem_ViewOtherWindows.AddItem(mar.content); break;
                }
            }
        }

        private void LoadWorkSpaces()
        {
            //workspaceManager1.LoadWorkspace("Paly Space", "ws\\workspace1.xml");
        }

        public SEventProxy eventProxy
        {
            get { return _proxy; }
        }

        public IUser currentUser
        {
            get
            {
                return _user;
            }
        }

        private void dockManager1_Load(object sender, EventArgs e)
        {
            foreach (DockPanel panel in dockManager1.Panels)
            {
                XtraUserControl control = _loader.getControlByGuid(panel.ID);
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;
                    panel.ControlContainer.Controls.Add(control);
                }
            }
        }

        private void workspaceManager1_WorkspaceSaved(object sender, DevExpress.Utils.WorkspaceEventArgs args)
        {
            _managerSupport.save();
        }

        private void workspaceManager1_AfterApplyWorkspace(object sender, EventArgs e)
        {
            _managerSupport.load();
        }

        private void showPropertyPanel()
        {
            //if (dockPanelProperty != null) return;
            int iPos;

            if ((iPos = dockManager1.Panels.IndexOf(dockPanelProperty))  >= 0)
            {
                dockManager1.Panels[iPos].Visibility = DockVisibility.Visible;
            }
            else
            {
                dockPanelProperty = dockManager1.AddPanel(DockingStyle.Left);
                dockPanelProperty.ID = Guid.Parse("3479629d-d423-4f13-851f-13a89c1293e2");
                dockPanelProperty.Text = "Property";
                dockPanelProperty.Width = 200;
                dockPanelProperty.FloatSize = new Size(200, 500);
            }
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showPropertyPanel();
        }

        private void showOutputPanel()
        {
            int iPos;

            if ((iPos = dockManager1.Panels.IndexOf(dockPanelOutput)) >= 0)
            {
                dockManager1.Panels[iPos].Visibility = DockVisibility.Visible;
            }
            else
            {
                dockPanelOutput = dockManager1.AddPanel(DockingStyle.Bottom);
                dockPanelOutput.ID = Guid.Parse("35f26588-5210-4f31-8d91-428e8d4e5b28");
                dockPanelOutput.Text = "Output";
                dockPanelOutput.Height = 200;
                dockPanelOutput.FloatSize = new Size(500, 200);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showOutputPanel();
        }
    }
}
