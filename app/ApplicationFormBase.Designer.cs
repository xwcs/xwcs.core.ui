namespace  xwcs.core.ui.app
{
    partial class ApplicationFormBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationFormBase));
            DevExpress.Utils.Animation.PushTransition pushTransition2 = new DevExpress.Utils.Animation.PushTransition();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem_File = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem_FileOpen = new DevExpress.XtraBars.BarSubItem();
            this.barButton_Save = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_SaveAll = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_Close = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_CloseAll = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem_Workspace = new DevExpress.XtraBars.BarSubItem();
            this.barButton_Createnew = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_Change = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_Exit = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem_Edit = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem_View = new DevExpress.XtraBars.BarSubItem();
            this.barButton_Startpage = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_Properties = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_Status = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem_OtherWindows = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem_Tools = new DevExpress.XtraBars.BarSubItem();
            this.barButton_Options = new DevExpress.XtraBars.BarButtonItem();
            this.barWorkspaceMenuItem1 = new DevExpress.XtraBars.BarWorkspaceMenuItem();
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager();
            this.barSubItem_About = new DevExpress.XtraBars.BarSubItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem_File,
            this.barSubItem_Edit,
            this.barSubItem_View,
            this.barSubItem_Tools,
            this.barSubItem_About,
            this.barSubItem_FileOpen,
            this.barButton_Save,
            this.barButton_SaveAll,
            this.barButton_Close,
            this.barButton_CloseAll,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barSubItem_Workspace,
            this.barButton_Createnew,
            this.barButton_Change,
            this.barButton_Exit,
            this.barButton_Startpage,
            this.barButton_Properties,
            this.barButton_Status,
            this.barButton_Options,
            this.barWorkspaceMenuItem1,
            this.barSubItem_OtherWindows});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 30;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            resources.ApplyResources(this.bar1, "bar1");
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_File),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_Edit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_View),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_Tools),
            new DevExpress.XtraBars.LinkPersistInfo(this.barWorkspaceMenuItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_About)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.bar2, "bar2");
            // 
            // barSubItem_File
            // 
            resources.ApplyResources(this.barSubItem_File, "barSubItem_File");
            this.barSubItem_File.Id = 6;
            this.barSubItem_File.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_FileOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Save),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_SaveAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Close),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_CloseAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_Workspace),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Exit)});
            this.barSubItem_File.Name = "barSubItem_File";
            // 
            // barSubItem_FileOpen
            // 
            resources.ApplyResources(this.barSubItem_FileOpen, "barSubItem_FileOpen");
            this.barSubItem_FileOpen.Id = 12;
            this.barSubItem_FileOpen.Name = "barSubItem_FileOpen";
            // 
            // barButton_Save
            // 
            resources.ApplyResources(this.barButton_Save, "barButton_Save");
            this.barButton_Save.Enabled = false;
            this.barButton_Save.Id = 13;
            this.barButton_Save.Name = "barButton_Save";
            this.barButton_Save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSave_ItemClick);
            // 
            // barButton_SaveAll
            // 
            resources.ApplyResources(this.barButton_SaveAll, "barButton_SaveAll");
            this.barButton_SaveAll.Enabled = false;
            this.barButton_SaveAll.Id = 14;
            this.barButton_SaveAll.Name = "barButton_SaveAll";
            this.barButton_SaveAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_SaveAll_ItemClick);
            // 
            // barButton_Close
            // 
            resources.ApplyResources(this.barButton_Close, "barButton_Close");
            this.barButton_Close.Id = 15;
            this.barButton_Close.Name = "barButton_Close";
            // 
            // barButton_CloseAll
            // 
            resources.ApplyResources(this.barButton_CloseAll, "barButton_CloseAll");
            this.barButton_CloseAll.Id = 16;
            this.barButton_CloseAll.Name = "barButton_CloseAll";
            // 
            // barSubItem_Workspace
            // 
            resources.ApplyResources(this.barSubItem_Workspace, "barSubItem_Workspace");
            this.barSubItem_Workspace.Id = 19;
            this.barSubItem_Workspace.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Createnew),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Change)});
            this.barSubItem_Workspace.Name = "barSubItem_Workspace";
            // 
            // barButton_Createnew
            // 
            resources.ApplyResources(this.barButton_Createnew, "barButton_Createnew");
            this.barButton_Createnew.Id = 20;
            this.barButton_Createnew.Name = "barButton_Createnew";
            this.barButton_Createnew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_Createnew_ItemClick);
            // 
            // barButton_Change
            // 
            resources.ApplyResources(this.barButton_Change, "barButton_Change");
            this.barButton_Change.Id = 21;
            this.barButton_Change.Name = "barButton_Change";
            this.barButton_Change.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_Change_ItemClick);
            // 
            // barButton_Exit
            // 
            resources.ApplyResources(this.barButton_Exit, "barButton_Exit");
            this.barButton_Exit.Id = 22;
            this.barButton_Exit.Name = "barButton_Exit";
            this.barButton_Exit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_Exit_ItemClick);
            // 
            // barSubItem_Edit
            // 
            resources.ApplyResources(this.barSubItem_Edit, "barSubItem_Edit");
            this.barSubItem_Edit.Id = 7;
            this.barSubItem_Edit.Name = "barSubItem_Edit";
            // 
            // barSubItem_View
            // 
            resources.ApplyResources(this.barSubItem_View, "barSubItem_View");
            this.barSubItem_View.Id = 8;
            this.barSubItem_View.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Startpage),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Properties),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Status),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_OtherWindows)});
            this.barSubItem_View.Name = "barSubItem_View";
            // 
            // barButton_Startpage
            // 
            resources.ApplyResources(this.barButton_Startpage, "barButton_Startpage");
            this.barButton_Startpage.Id = 23;
            this.barButton_Startpage.Name = "barButton_Startpage";
            // 
            // barButton_Properties
            // 
            resources.ApplyResources(this.barButton_Properties, "barButton_Properties");
            this.barButton_Properties.Id = 24;
            this.barButton_Properties.Name = "barButton_Properties";
            // 
            // barButton_Status
            // 
            resources.ApplyResources(this.barButton_Status, "barButton_Status");
            this.barButton_Status.Id = 25;
            this.barButton_Status.Name = "barButton_Status";
            // 
            // barSubItem_OtherWindows
            // 
            resources.ApplyResources(this.barSubItem_OtherWindows, "barSubItem_OtherWindows");
            this.barSubItem_OtherWindows.Id = 29;
            this.barSubItem_OtherWindows.Name = "barSubItem_OtherWindows";
            // 
            // barSubItem_Tools
            // 
            resources.ApplyResources(this.barSubItem_Tools, "barSubItem_Tools");
            this.barSubItem_Tools.Id = 9;
            this.barSubItem_Tools.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Options)});
            this.barSubItem_Tools.Name = "barSubItem_Tools";
            // 
            // barButton_Options
            // 
            resources.ApplyResources(this.barButton_Options, "barButton_Options");
            this.barButton_Options.Id = 26;
            this.barButton_Options.Name = "barButton_Options";
            // 
            // barWorkspaceMenuItem1
            // 
            resources.ApplyResources(this.barWorkspaceMenuItem1, "barWorkspaceMenuItem1");
            this.barWorkspaceMenuItem1.Id = 27;
            this.barWorkspaceMenuItem1.Name = "barWorkspaceMenuItem1";
            this.barWorkspaceMenuItem1.ShowSaveLoadCommands = true;
            this.barWorkspaceMenuItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barWorkspaceMenuItem1.WorkspaceManager = this.workspaceManager1;
            // 
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition2;
            this.workspaceManager1.AfterApplyWorkspace += new System.EventHandler(this.workspaceManager1_AfterApplyWorkspace);
            this.workspaceManager1.WorkspaceSaved += new DevExpress.Utils.WorkspaceEventHandler(this.workspaceManager1_WorkspaceSaved);
            // 
            // barSubItem_About
            // 
            resources.ApplyResources(this.barSubItem_About, "barSubItem_About");
            this.barSubItem_About.Id = 10;
            this.barSubItem_About.Name = "barSubItem_About";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.bar3, "bar3");
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
            this.dockManager1.Load += new System.EventHandler(this.dockManager1_Load);
            // 
            // barButtonItem5
            // 
            resources.ApplyResources(this.barButtonItem5, "barButtonItem5");
            this.barButtonItem5.Id = 17;
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // barButtonItem6
            // 
            resources.ApplyResources(this.barButtonItem6, "barButtonItem6");
            this.barButtonItem6.Id = 18;
            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.MenuManager = this.barManager1;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // ApplicationFormBase
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ApplicationFormBase";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationFormBase_FormClosing);
            this.Shown += new System.EventHandler(this.ApplicationFormBase_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.BarSubItem barSubItem_File;
        private DevExpress.XtraBars.BarSubItem barSubItem_FileOpen;
        private DevExpress.XtraBars.BarButtonItem barButton_Save;
        private DevExpress.XtraBars.BarSubItem barSubItem_Edit;
        private DevExpress.XtraBars.BarSubItem barSubItem_View;
        private DevExpress.XtraBars.BarSubItem barSubItem_Tools;
        private DevExpress.XtraBars.BarSubItem barSubItem_About;
        private DevExpress.XtraBars.BarButtonItem barButton_SaveAll;
        private DevExpress.XtraBars.BarButtonItem barButton_Close;
        private DevExpress.XtraBars.BarButtonItem barButton_CloseAll;
        private DevExpress.XtraBars.BarSubItem barSubItem_Workspace;
        private DevExpress.XtraBars.BarButtonItem barButton_Createnew;
        private DevExpress.XtraBars.BarButtonItem barButton_Change;
        private DevExpress.XtraBars.BarButtonItem barButton_Exit;
        private DevExpress.XtraBars.BarButtonItem barButton_Startpage;
        private DevExpress.XtraBars.BarButtonItem barButton_Properties;
        private DevExpress.XtraBars.BarButtonItem barButton_Status;
        private DevExpress.XtraBars.BarButtonItem barButton_Options;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarWorkspaceMenuItem barWorkspaceMenuItem1;
        private DevExpress.Utils.WorkspaceManager workspaceManager1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevExpress.XtraBars.BarSubItem barSubItem_OtherWindows;
    }
}