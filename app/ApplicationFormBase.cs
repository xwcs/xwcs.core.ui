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
using System.Threading;
using System.Globalization;

namespace xwcs.core.ui.app
{
    public partial class ApplicationFormBase : Form, IPluginHost // DevExpress.XtraEditors.XtraUserControl, IPluginHost
    {
        private EventProxy _proxy;
        private User _user;
        private PluginsLoader _loader = new PluginsLoader();
        private DocumentManagerSupport _managerSupport;

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

                _proxy = new EventProxy();
                _proxy.addEventHandler(EventType.AddToolBarRequestEvent, HandleAddToolbarRequestEvent);
                _proxy.addEventHandler(EventType.OpenPanelRequestEvent, HandleOpenPanelRequestEvent);

                _loader.LoadPlugins(this, "Plugins");

                _managerSupport = new DocumentManagerSupport(documentManager1, _loader);
            }            
        }

        private void HandleOpenPanelRequestEvent(Event e)
        {
            OpenPanelRequest ee = (OpenPanelRequest)e.data;
            VisualControl pluginControl = (VisualControl)ee.control;

            if (pluginControl != null)
            {
                if (pluginControl.controlInfo.Type == controlType.PLGT_document)
                {
                    documentManager1.BeginUpdate();
                    BaseDocument document = documentManager1.View.AddDocument(pluginControl);
                    document.Caption = pluginControl.controlInfo.Name;
                    document.ControlName = pluginControl.controlInfo.Name;
                    documentManager1.EndUpdate();
                    //tabbedView1.Controller.Activate(document);
                }
                else
                {
                    DockPanel dockPanel1 = dockManager1.AddPanel(DockingStyle.Top);
                    dockPanel1.ID = ee.guid;
                    dockPanel1.Text = pluginControl.controlInfo.Name;
                    dockPanel1.Height = 400;
                    dockPanel1.FloatSize = new Size(500, 400);

                    pluginControl.Dock = DockStyle.Fill;
                    dockPanel1.ControlContainer.Controls.Add(pluginControl);
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
                    case MenuDestination.MENU_file_open: barSubItem6.AddItem(mar.content); break;
                }
            }
        }

        private void LoadWorkSpaces()
        {
            //workspaceManager1.LoadWorkspace("Paly Space", "ws\\workspace1.xml");
        }

        public EventProxy eventProxy
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
                IVisualPlugin plugin = (IVisualPlugin)_loader.getPluginByGuid(panel.ID);

                if (plugin != null)
                {
                    XtraUserControl control = plugin.getControlByGuid(panel.ID);
                    if (control != null)
                    {
                        control.Dock = DockStyle.Fill;
                        panel.ControlContainer.Controls.Add(control);
                    }
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
    }
}
