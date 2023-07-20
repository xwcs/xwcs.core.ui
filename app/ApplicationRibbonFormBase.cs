﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using xwcs.core.evt;
using xwcs.core.manager;
using xwcs.core.plgs;
using xwcs.core.user;
using xwcs.core.ui.controls;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using System.IO;
using xwcs.core.controls;
using DevExpress.Data.Filtering;

namespace xwcs.core.ui.app
{
	public partial class ApplicationRibbonFormBase : DevExpress.XtraBars.Ribbon.RibbonForm, IPluginHost
	{
		private SEventProxy _proxy;         //just local singleton instance copy
		private SWidgetManager _widgetManager; //main instance, who do it first it make it, this is just local singleton instance copy
		private SPluginsLoader _loader;
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


		public ApplicationRibbonFormBase()
		{
            DevExpress.Data.Linq.CriteriaToEFExpressionConverter.EntityFunctionsType = typeof(System.Data.Entity.DbFunctions);
            DevExpress.Data.Linq.CriteriaToEFExpressionConverter.SqlFunctionsType = typeof(System.Data.Entity.SqlServer.SqlFunctions);
            CriteriaOperator.RegisterCustomFunction(new xwcs.core.linq.LikeFunction());



            InitializeComponent();

			/*
			 * Assembly locating
			 */

			if (!DesignMode)
			{
				documentManager.View.DocumentProperties.UseFormIconAsDocumentImage = true;
				documentManager.View.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;
				tabbedView.FloatingDocumentContainer = FloatingDocumentContainer.DocumentsHost;

				// Security context must be call first time in main app and 
				// must have providers set
				_user = SecurityContext.getInstance().CurrentUser;

				_proxy = SEventProxy.getInstance();
				_proxy.addEventHandler<AddToolBarRequestEvent>(EventType.AddToolBarRequestEvent, HandleAddToolbarRequestEvent);
				_proxy.addEventHandler<OpenPanelRequestEvent>(EventType.OpenPanelRequestEvent, HandleOpenPanelRequestEvent);
				_proxy.addEventHandler<VisualControlActionEvent>(EventType.VisualControlActionEvent, HandleVisualControlAction);
				_proxy.addEventHandler<ClosePanelRequestEvent>(EventType.ClosePanelRequestEvent, HandleClosePanelRequestEvent);

				// invocation target for events
				SEventProxy.InvokeDelegate = this;

				//do it here before any other plugin will be loaded!!!!!
				_widgetManager = SWidgetManager.getInstance();

				//now we can load plugins
				_loader = SPluginsLoader.getInstance();
				_loader.LoadPlugins(this, ".");

				controls.ViewBaseEventsHandler.AttachToView(documentManager.View);

				_managerSupport = new DocumentManagerSupport(documentManager,
																new DevExpress.XtraBars.BarItem[] { barButtonItem_FileSave },
																new DevExpress.XtraBars.BarItem[] { barButtonItem_FileSaveAll });
				ribbonControl.SelectedPage = ribbonPagePlugins;


                // some form function tweaks
                // AutoValidate = AutoValidate.EnableAllowFocusChange;
                this.FormClosing += ApplicationRibbonFormBase_FormClosing;
               
            }

        }

        private void ApplicationRibbonFormBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canBeAllDocumentsClosed();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Enabled = false;
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowDefaultWaitForm("Attendere prego", "Chiusura ...");
                // unload plugins
                _loader.Unload();


                // kill loggers
                SLogManager.getInstance().Dispose();

                // state machines clear
                statemachine.StateMachinesDisposer.getInstance().Dispose();
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseDefaultWaitForm();

                // kill rest
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void UpdateRibbons(IVisualControl control)
		{
			ribbonControl.UnMergeRibbon();
			if(!ReferenceEquals(control.Ribbon, null)) {
				ribbonControl.MergeRibbon(control.Ribbon);
				ribbonControl.SelectedPage = ribbonControl.MergedPages[0];
			}			
		}

		private void HandleVisualControlAction(object sender, VisualControlActionEvent e)
		{
			switch (e.RequestData.ActionKind)
			{
				case VisualControlActionKind.Activated:
					UpdateRibbons(e.RequestData.VisualControl);
					break;
				default:
					
					break;
			}
		}

		private void HandleClosePanelRequestEvent(object sender, ClosePanelRequestEvent e)
		{
			BaseDocument document = _managerSupport.getDocumentByControl(sender as VisualControl);
			if (document != null)
			{
				documentManager.View.Controller.Close(document);
			}			
		}

        Queue<OpenPanelRequestEvent> _openRequests = new Queue<OpenPanelRequestEvent>();
        bool _working = false;
        private void ConsumeQueue()
        {
            if (_working) return;
            _working = true;

            try
            {
                if (_openRequests.Count == 0) return;
                OpenPanelInternal(_openRequests.Dequeue());
            }
            finally
            {
                // it need to be surely turn off
                _working = false;
            }

            ConsumeQueue();
        }
        private void HandleOpenPanelRequestEvent(object sender, OpenPanelRequestEvent e)
        {
            _openRequests.Enqueue(e);
            ConsumeQueue();
        }
        private void OpenPanelInternal(OpenPanelRequestEvent e)
		{
			OpenPanelRequest ee = (OpenPanelRequest)e.Data;
			core.controls.VisualControlInfo vci = ee.Vci;

			// handle eventual multi opening
			if (!vci.AllowMulti)
			{
				BaseDocument existingDocument = _managerSupport.getDocumentByVCI(vci);
				if (existingDocument != null)
				{
					documentManager.View.Controller.Activate(existingDocument);
					VisualControl existingVisualControl = (VisualControl)existingDocument.Control;
					if ((existingVisualControl != null) && (ee.DataObject != null))
					{
						existingVisualControl.Start(core.controls.VisualControlStartingKind.ActivateOpened, ee.DataObject);
					}
					return;
				}
			}

			VisualControl control = (VisualControl)vci.createInstance();
			if (control != null)
			{
                BaseDocument document = null;

                if (vci.DockStyle == core.controls.ControlDockStyle.PLGT_document)
				{
					documentManager.BeginUpdate();
					document = documentManager.View.AddDocument(control);
					document.Caption = control.VisualControlInfo.Name;
					document.ControlName = control.VisualControlInfo.Name;
					document.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.False;
					documentManager.EndUpdate();
					documentManager.View.Controller.Activate(document);
				}
				else if (vci.DockStyle == core.controls.ControlDockStyle.PLGT_status)
				{
					DockPanel pb = new DockPanel();

					if (dockManager.Panels.Count == 0)
					{
						pb = dockManager.AddPanel(DockingStyle.Bottom);
					}
					else
					if (dockManager.Panels.Count == 1)
					{
						pb = dockManager.AddPanel(DockingStyle.Bottom);
						DockPanel panelX = dockManager.Panels[0] as DockPanel;
						pb.DockAsTab(panelX);
					}
					else
					if (dockManager.Panels.Count > 1)
					{
						pb = dockManager.AddPanel(DockingStyle.Bottom);
						DockPanel container = dockManager.Panels[0].ParentPanel as DockPanel;
						if (container != null) pb.DockAsTab(container);
					}

					pb.ClosedPanel += (senderX, eX) =>
					{
						dockManager.RemovePanel(pb);
					};


					control.Dock = DockStyle.Fill;
					pb.ControlContainer.Controls.Add(control);
					pb.ID = vci.GUID;
					pb.Name = vci.GUID + "_" + control.Name;
					pb.Text = vci.Name;
				}
				else
				{
					DockPanel dockPanel1 = dockManager.AddPanel(DockingStyle.Top);
					dockPanel1.ID = ee.Vci.GUID;
					dockPanel1.Text = control.VisualControlInfo.Name;
					dockPanel1.Height = 400;
					dockPanel1.FloatSize = new Size(500, 400);

					control.Dock = DockStyle.Fill;
					dockPanel1.ControlContainer.Controls.Add(control);
				}

                //start control
                if (!control.Start(core.controls.VisualControlStartingKind.StartingNew, ee.DataObject))
                {
                    // remove control it was not good start
                    if (vci.DockStyle == core.controls.ControlDockStyle.PLGT_document)
                    {
                        documentManager.BeginUpdate();
                        documentManager.View.Controller.Close(document);
                        documentManager.EndUpdate();
                    }
                }   
			}
		}

		private void addItemToRibonGroup(RibbonPageGroup sourceItem, DevExpress.XtraBars.BarItem newItem)
		{
			IEnumerable<BarItemLink> l = sourceItem.ItemLinks.Where(i => i.Caption == newItem.Caption).ToList();
			if(l.Count() > 0) {
				sourceItem.ItemLinks.Remove(l.First());
			}

			int newID = sourceItem.ItemLinks.Count;
			newItem.Id = newID;
			sourceItem.ItemLinks.Add(newItem);
		}

		private void HandleAddToolbarRequestEvent(object sender, AddToolBarRequestEvent e)
		{
			AddToolBarRequest ee = (AddToolBarRequest)e.Data;
			MenuAddRequest[] menu = ee.content;

			foreach (MenuAddRequest mar in menu)
			{
				switch (mar.destination)
				{
					case MenuDestination.MENU_file_open:
						{
							addItemToRibonGroup(ribbonPageGroupPlgs_generic , mar.content);
							break;
						}
					case MenuDestination.MENU_ViewOtherWindows:
						{
							addItemToRibonGroup(ribbonPageGroupPlgs_generic, mar.content);
							break;
						}
					case MenuDestination.MENU_tool_bar:
						{
							addItemToRibonGroup(ribbonPageGroupPlgs_generic, mar.content);
							break;
						}
				}
			}
		}

		private void dockManager_Load(object sender, EventArgs e)
		{
			foreach (DockPanel panel in dockManager.Panels)
			{
				XtraUserControl control = _loader.getControlByGuid(panel.ID);
				if (control != null)
				{
					control.Dock = DockStyle.Fill;
					panel.ControlContainer.Controls.Add(control);

					panel.ClosedPanel += (senderX, eX) =>
					{
						dockManager.RemovePanel(panel);
					};
				}
			}
		}

		private void workspaceManager_WorkspaceSaved(object sender, DevExpress.Utils.WorkspaceEventArgs args)
		{
			_managerSupport.SaveState();
		}

		private void workspaceManager_AfterApplyWorkspace(object sender, EventArgs e)
		{
			_managerSupport.LoadState();
		}
        private bool _useWorkSpace = true;
        public bool UseWorkSpace
        {
            get { return _useWorkSpace; }
            set { _useWorkSpace = value; }
        }
		private void LoadWorkSpace()
		{
			if (DesignMode) return;
            if (!this.UseWorkSpace) return;
            Stream reader = null;
			try
			{
				reader = SPersistenceManager.getInstance().GetReader("DefaultWorkspace");
				workspaceManager.LoadWorkspace("DefaultWorkspace", reader);
				workspaceManager.CloseStreamOnWorkspaceLoading = DevExpress.Utils.DefaultBoolean.True;
				workspaceManager.TransitionType = new DevExpress.Utils.Animation.ShapeTransition();
				workspaceManager.TransitionType.Parameters.FrameCount = 1000;
                workspaceManager.ApplyWorkspace("DefaultWorkspace");
			}
			catch (Exception ex)
			{
				SLogManager.getInstance().Error(ex.ToString());
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
            if (!this.UseWorkSpace) return;
            try
			{
				using(Stream writer = SPersistenceManager.getInstance().GetWriter("DefaultWorkspace")){
                    workspaceManager.CaptureWorkspace("DefaultWorkspace");
                    workspaceManager.CloseStreamOnWorkspaceSaving = DevExpress.Utils.DefaultBoolean.True;
                    workspaceManager.SaveWorkspace(workspaceManager.Workspaces[0].Name, writer, true);
                }
			}
			catch (Exception ex)
			{
				SLogManager.getInstance().Error(ex.ToString());
			}
		}

		protected void ApplicationFormBase_FormClosing(object sender, FormClosingEventArgs e)
		{
			saveWorkspace();
		}

		protected void ApplicationFormBase_Shown(object sender, EventArgs e)
		{
			LoadWorkSpace();
			_proxy.fireEvent(new Event(this, EventType.WorkSpaceLoadedEvent, null));
		}

		private void barButton_Createnew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			//Create new workspace
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					documentManager.View.Controller.CloseAll();
					workspaceManager.RemoveWorkspace("DefaultWorkspace");

					SPersistenceManager.getInstance().CreateWorkSpace(folderBrowserDialog.SelectedPath);
				}
				catch (Exception ex)
				{
					SLogManager.getInstance().Error(ex.ToString());
				}
			}
		}

        private bool canBeAllDocumentsClosed()
        {
            foreach(VisualControl vc in documentManager.View.Documents.Select(e => e.Control).Cast<VisualControl>())
            {
                if (!vc.checkClosable()) return false;
            }

            return true;
        }

		private void barButton_Change_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            if (!canBeAllDocumentsClosed()) return; // skip all if not all closable
           

			//Change workspace
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					SPersistenceManager.getInstance().SetWorkSpace(folderBrowserDialog.SelectedPath);

					DockPanel[] p = new DockPanel[dockManager.Count];
					int i = 0;
					foreach (DockPanel dp in dockManager.Panels)
					{
						p[i++] = dp;
					}
					foreach (DockPanel dp in p)
					{
						dp.Close();
					}

					documentManager.View.Controller.CloseAll();
					workspaceManager.RemoveWorkspace("DefaultWorkspace");
					LoadWorkSpace();
				}
				catch (IOException ex)
				{
					MessageBox.Show("This is not workspace's folder!");
					SLogManager.getInstance().Info(ex.ToString());
				}
				catch (Exception ex)
				{
					SLogManager.getInstance().Error(ex.ToString());
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

        private void tabbedView_DocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            // tab closed request
            if(e.Document.Control is VisualControl)
            {
                // verify if it is possible to close and cancel event if not
                e.Cancel = ! (e.Document.Control as VisualControl).checkClosable();
            }
        }
    }
}