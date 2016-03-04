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
using System.IO;

namespace xwcs.core.ui.app
{
    public partial class ApplicationFormBase : Form, IPluginHost // DevExpress.XtraEditors.XtraUserControl, IPluginHost
    {
        private SEventProxy     _proxy;         //just local singleton instance copy
        private SWidgetManager  _widgetManager; //main instance, who do it first it make it, this is just local singleton instance copy
        private SPluginsLoader  _loader;
        private IUser _user;
        private DocumentManagerSupport _managerSupport;

        public SEventProxy eventProxy
        {
            get { return _proxy; }
        }

        public IUser currentUser
        {
            get { return _user; }
        }

        public ApplicationFormBase()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                documentManager1.View.DocumentProperties.UseFormIconAsDocumentImage = false;
                documentManager1.View.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;
                tabbedView1.FloatingDocumentContainer = FloatingDocumentContainer.DocumentsHost;

                // Security context must be caal first time in main app and 
				// must have providers set
                _user = SecurityContext.getInstance().CurrentUser;

                _proxy = SEventProxy.getInstance();
                _proxy.addEventHandler(EventType.AddToolBarRequestEvent, HandleAddToolbarRequestEvent);
                _proxy.addEventHandler(EventType.OpenPanelRequestEvent, HandleOpenPanelRequestEvent);

                //do it here before any other plugin will be loaded!!!!!
                _widgetManager = SWidgetManager.getInstance();

                //now we can load plugins
                _loader = SPluginsLoader.getInstance();
                _loader.LoadPlugins(this, "Plugins");

                _managerSupport = new DocumentManagerSupport(   documentManager1,
                                                                new DevExpress.XtraBars.BarItem[] { barButton_Save },
                                                                new DevExpress.XtraBars.BarItem[] { barButton_SaveAll });
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
                    DockPanel pb = new DockPanel();

                    if (dockManager1.Panels.Count == 0)
                    {
                        pb = dockManager1.AddPanel(DockingStyle.Bottom);
                    }
                    else
                    if (dockManager1.Panels.Count == 1)
                    {
                        pb = dockManager1.AddPanel(DockingStyle.Bottom);
                        DockPanel panelX = dockManager1.Panels[0] as DockPanel;
                        pb.DockAsTab(panelX);
                    }
                    else
                    if (dockManager1.Panels.Count > 1)
                    {
                        pb = dockManager1.AddPanel(DockingStyle.Bottom);
                        DockPanel container = dockManager1.Panels[0].ParentPanel as DockPanel;
                        if (container != null) pb.DockAsTab(container);
                    }

                    pb.ClosedPanel += (senderX, eX) =>
                    {
                        dockManager1.RemovePanel(pb);                        
                    };
                    

                    control.Dock = DockStyle.Fill;
                    pb.ControlContainer.Controls.Add(control);
                    pb.ID = vci.GUID;
                    pb.Name = vci.GUID + "_" + control.Name;
                    pb.Text = vci.Name;
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

        private void addItemToMenu(DevExpress.XtraBars.BarSubItem sourceItem, DevExpress.XtraBars.BarItem newItem)
        {
            DevExpress.XtraBars.BarItemLink linkForDelete = null;
            foreach (DevExpress.XtraBars.BarItemLink link in sourceItem.ItemLinks)
            {
                if (link.Caption == newItem.Caption) { linkForDelete = link; break; }
            }
            if (linkForDelete != null) sourceItem.RemoveLink(linkForDelete);

            int newID = sourceItem.ItemLinks.Count;
            newItem.Id = newID;
            sourceItem.AddItem(newItem);
        }

        private void HandleAddToolbarRequestEvent(Event e)
        {
            AddToolBarRequest ee = (AddToolBarRequest)e.data;
            MenuAddRequest[] menu = ee.content;

            foreach (MenuAddRequest mar in menu)
            {
                switch (mar.destination)
                {
                    case MenuDestination.MENU_file_open:
                    {
                        addItemToMenu(barSubItem_FileOpen, mar.content);
                        break;
                    }                        
                    case MenuDestination.MENU_ViewOtherWindows:
                    {
                        addItemToMenu(barSubItem_OtherWindows, mar.content);
                        break;
                    }                        
                }
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

                    panel.ClosedPanel += (senderX, eX) =>
                    {
                        dockManager1.RemovePanel(panel);                        
                    };
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

        private void loadWorkSpace()
        {
            if (DesignMode) return;

            Stream reader = null;
            try
            {
                reader = xwcs.core.manager.SPersistenceManager.getInstance().getReader("DefaultWorkspace");
                workspaceManager1.LoadWorkspace("DefaultWorkspace", reader);
                workspaceManager1.ApplyWorkspace("DefaultWorkspace");                
            }
            catch (Exception ex)
            {                
                SLogManager.getInstance().Error(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private void saveWorkspace()
        {
            //https://www.devexpress.com/Support/Center/Example/Details/T190543
            if (DesignMode) return;

            Stream writer = null;
            try
            {
                writer = xwcs.core.manager.SPersistenceManager.getInstance().getWriter("DefaultWorkspace");                
                workspaceManager1.CaptureWorkspace("DefaultWorkspace");
                workspaceManager1.SaveWorkspace(workspaceManager1.Workspaces[0].Name, writer, true);
            }
            catch (Exception ex)
            {
                SLogManager.getInstance().Error(ex.Message);
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        protected void ApplicationFormBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWorkspace();
        }

        protected void ApplicationFormBase_Shown(object sender, EventArgs e)
        {
            loadWorkSpace();
            _proxy.fireEvent(new Event(this, EventType.WorkSpaceLoadedEvent, null));
        }

        private void barButton_Createnew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Create new workspace
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    documentManager1.View.Controller.CloseAll();
                    workspaceManager1.RemoveWorkspace("DefaultWorkspace");

                    xwcs.core.manager.SPersistenceManager.getInstance().createWorkSpace(folderBrowserDialog1.SelectedPath);
                }
                catch (Exception ex)
                {
                    SLogManager.getInstance().Error(ex.Message);
                }
            }
        }

        private void barButton_Change_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Change workspace
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    xwcs.core.manager.SPersistenceManager.getInstance().setWorkSpace(folderBrowserDialog1.SelectedPath);

                    DockPanel[] p = new DockPanel[dockManager1.Count];
                    int i = 0;
                    foreach (DockPanel dp in dockManager1.Panels)
                    {
                        p[i++] = dp;
                    }
                    foreach (DockPanel dp in p)
                    {
                        dp.Close();
                    }

                    documentManager1.View.Controller.CloseAll();
                    workspaceManager1.RemoveWorkspace("DefaultWorkspace");
                    loadWorkSpace();
                }
                catch (IOException ex)
                {
                    MessageBox.Show("This is not workspace's folder!");
                    SLogManager.getInstance().Info(ex.Message);
                }
                catch (Exception ex)
                {
                    SLogManager.getInstance().Error(ex.Message);
                }
            }
        }

        private void barButton_SaveAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _managerSupport.SaveChangedControls();
        }

        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _managerSupport.saveSelectedControl();
        }

        private void barButton_Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
