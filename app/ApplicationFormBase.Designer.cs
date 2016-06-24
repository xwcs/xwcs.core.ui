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
				controls.ViewBaseEventsHandler.DettachFromView(documentManager.View);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationFormBase));
			DevExpress.Utils.Animation.ShapeTransition shapeTransition1 = new DevExpress.Utils.Animation.ShapeTransition();
			this.barManager = new DevExpress.XtraBars.BarManager();
			this.barToolButtons = new DevExpress.XtraBars.Bar();
			this.barMenu = new DevExpress.XtraBars.Bar();
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
			this.workspaceManager = new DevExpress.Utils.WorkspaceManager();
			this.barSubItem_About = new DevExpress.XtraBars.BarSubItem();
			this.barStatus = new DevExpress.XtraBars.Bar();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.dockManager = new DevExpress.XtraBars.Docking.DockManager();
			this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager();
			this.tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tabbedView)).BeginInit();
			this.SuspendLayout();
			// 
			// barManager
			// 
			this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barToolButtons,
            this.barMenu,
            this.barStatus});
			this.barManager.DockControls.Add(this.barDockControlTop);
			this.barManager.DockControls.Add(this.barDockControlBottom);
			this.barManager.DockControls.Add(this.barDockControlLeft);
			this.barManager.DockControls.Add(this.barDockControlRight);
			this.barManager.DockManager = this.dockManager;
			this.barManager.Form = this;
			this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
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
			this.barManager.MainMenu = this.barMenu;
			this.barManager.MaxItemId = 34;
			this.barManager.StatusBar = this.barStatus;
			// 
			// barToolButtons
			// 
			this.barToolButtons.BarName = "Tools";
			this.barToolButtons.DockCol = 0;
			this.barToolButtons.DockRow = 1;
			this.barToolButtons.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.barToolButtons.OptionsBar.MultiLine = true;
			resources.ApplyResources(this.barToolButtons, "barToolButtons");
			// 
			// barMenu
			// 
			this.barMenu.BarName = "Main menu";
			this.barMenu.DockCol = 0;
			this.barMenu.DockRow = 0;
			this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.barMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_File),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_Edit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_View),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_Tools),
            new DevExpress.XtraBars.LinkPersistInfo(this.barWorkspaceMenuItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem_About)});
			this.barMenu.OptionsBar.MultiLine = true;
			this.barMenu.OptionsBar.UseWholeRow = true;
			resources.ApplyResources(this.barMenu, "barMenu");
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
			this.barSubItem_File.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_File.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_File.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_File.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_File.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_File.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_File.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_File.MenuAppearance.HeaderItemAppearance.Image")));
			this.barSubItem_File.Name = "barSubItem_File";
			// 
			// barSubItem_FileOpen
			// 
			resources.ApplyResources(this.barSubItem_FileOpen, "barSubItem_FileOpen");
			this.barSubItem_FileOpen.Id = 12;
			this.barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_FileOpen.MenuAppearance.HeaderItemAppearance.Image")));
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
			this.barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_Workspace.MenuAppearance.HeaderItemAppearance.Image")));
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
			this.barSubItem_Edit.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_Edit.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_Edit.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_Edit.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_Edit.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_Edit.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_Edit.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_Edit.MenuAppearance.HeaderItemAppearance.Image")));
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
			this.barSubItem_View.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_View.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_View.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_View.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_View.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_View.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_View.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_View.MenuAppearance.HeaderItemAppearance.Image")));
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
			this.barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_OtherWindows.MenuAppearance.HeaderItemAppearance.Image")));
			this.barSubItem_OtherWindows.Name = "barSubItem_OtherWindows";
			// 
			// barSubItem_Tools
			// 
			resources.ApplyResources(this.barSubItem_Tools, "barSubItem_Tools");
			this.barSubItem_Tools.Id = 9;
			this.barSubItem_Tools.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButton_Options)});
			this.barSubItem_Tools.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_Tools.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_Tools.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_Tools.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_Tools.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_Tools.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_Tools.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_Tools.MenuAppearance.HeaderItemAppearance.Image")));
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
			this.barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barWorkspaceMenuItem1.MenuAppearance.HeaderItemAppearance.Image")));
			this.barWorkspaceMenuItem1.Name = "barWorkspaceMenuItem1";
			this.barWorkspaceMenuItem1.ShowSaveLoadCommands = true;
			this.barWorkspaceMenuItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
			this.barWorkspaceMenuItem1.WorkspaceManager = this.workspaceManager;
			// 
			// workspaceManager
			// 
			this.workspaceManager.AllowTransitionAnimation = DevExpress.Utils.DefaultBoolean.True;
			this.workspaceManager.CloseStreamOnWorkspaceLoading = DevExpress.Utils.DefaultBoolean.True;
			this.workspaceManager.CloseStreamOnWorkspaceSaving = DevExpress.Utils.DefaultBoolean.True;
			this.workspaceManager.TargetControl = this;
			shapeTransition1.Parameters.EffectOptions = DevExpress.Utils.Animation.ShapeEffectOptions.CircleOut;
			shapeTransition1.Parameters.FrameCount = 1000;
			this.workspaceManager.TransitionType = shapeTransition1;
			this.workspaceManager.AfterApplyWorkspace += new System.EventHandler(this.workspaceManager1_AfterApplyWorkspace);
			this.workspaceManager.WorkspaceSaved += new DevExpress.Utils.WorkspaceEventHandler(this.workspaceManager1_WorkspaceSaved);
			// 
			// barSubItem_About
			// 
			resources.ApplyResources(this.barSubItem_About, "barSubItem_About");
			this.barSubItem_About.Id = 10;
			this.barSubItem_About.MenuAppearance.HeaderItemAppearance.FontSizeDelta = ((int)(resources.GetObject("barSubItem_About.MenuAppearance.HeaderItemAppearance.FontSizeDelta")));
			this.barSubItem_About.MenuAppearance.HeaderItemAppearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barSubItem_About.MenuAppearance.HeaderItemAppearance.FontStyleDelta")));
			this.barSubItem_About.MenuAppearance.HeaderItemAppearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barSubItem_About.MenuAppearance.HeaderItemAppearance.GradientMode")));
			this.barSubItem_About.MenuAppearance.HeaderItemAppearance.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem_About.MenuAppearance.HeaderItemAppearance.Image")));
			this.barSubItem_About.Name = "barSubItem_About";
			// 
			// barStatus
			// 
			this.barStatus.BarName = "Status bar";
			this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
			this.barStatus.DockCol = 0;
			this.barStatus.DockRow = 0;
			this.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
			this.barStatus.OptionsBar.AllowQuickCustomization = false;
			this.barStatus.OptionsBar.DrawDragBorder = false;
			this.barStatus.OptionsBar.UseWholeRow = true;
			resources.ApplyResources(this.barStatus, "barStatus");
			// 
			// barDockControlTop
			// 
			resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
			this.barDockControlTop.Appearance.FontSizeDelta = ((int)(resources.GetObject("barDockControlTop.Appearance.FontSizeDelta")));
			this.barDockControlTop.Appearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barDockControlTop.Appearance.FontStyleDelta")));
			this.barDockControlTop.Appearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barDockControlTop.Appearance.GradientMode")));
			this.barDockControlTop.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("barDockControlTop.Appearance.Image")));
			this.barDockControlTop.CausesValidation = false;
			// 
			// barDockControlBottom
			// 
			resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
			this.barDockControlBottom.Appearance.FontSizeDelta = ((int)(resources.GetObject("barDockControlBottom.Appearance.FontSizeDelta")));
			this.barDockControlBottom.Appearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barDockControlBottom.Appearance.FontStyleDelta")));
			this.barDockControlBottom.Appearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barDockControlBottom.Appearance.GradientMode")));
			this.barDockControlBottom.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("barDockControlBottom.Appearance.Image")));
			this.barDockControlBottom.CausesValidation = false;
			// 
			// barDockControlLeft
			// 
			resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
			this.barDockControlLeft.Appearance.FontSizeDelta = ((int)(resources.GetObject("barDockControlLeft.Appearance.FontSizeDelta")));
			this.barDockControlLeft.Appearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barDockControlLeft.Appearance.FontStyleDelta")));
			this.barDockControlLeft.Appearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barDockControlLeft.Appearance.GradientMode")));
			this.barDockControlLeft.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("barDockControlLeft.Appearance.Image")));
			this.barDockControlLeft.CausesValidation = false;
			// 
			// barDockControlRight
			// 
			resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
			this.barDockControlRight.Appearance.FontSizeDelta = ((int)(resources.GetObject("barDockControlRight.Appearance.FontSizeDelta")));
			this.barDockControlRight.Appearance.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("barDockControlRight.Appearance.FontStyleDelta")));
			this.barDockControlRight.Appearance.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("barDockControlRight.Appearance.GradientMode")));
			this.barDockControlRight.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("barDockControlRight.Appearance.Image")));
			this.barDockControlRight.CausesValidation = false;
			// 
			// dockManager
			// 
			this.dockManager.Form = this;
			this.dockManager.MenuManager = this.barManager;
			this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
			this.dockManager.Load += new System.EventHandler(this.dockManager1_Load);
			// 
			// documentManager
			// 
			this.documentManager.ContainerControl = this;
			this.documentManager.MenuManager = this.barManager;
			this.documentManager.View = this.tabbedView;
			this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView});
			// 
			// tabbedView
			// 
			this.tabbedView.AppearancePage.Header.FontSizeDelta = ((int)(resources.GetObject("tabbedView.AppearancePage.Header.FontSizeDelta")));
			this.tabbedView.AppearancePage.Header.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("tabbedView.AppearancePage.Header.FontStyleDelta")));
			this.tabbedView.AppearancePage.Header.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("tabbedView.AppearancePage.Header.GradientMode")));
			this.tabbedView.AppearancePage.Header.Image = ((System.Drawing.Image)(resources.GetObject("tabbedView.AppearancePage.Header.Image")));
			this.tabbedView.AppearancePage.HeaderActive.BackColor = ((System.Drawing.Color)(resources.GetObject("tabbedView.AppearancePage.HeaderActive.BackColor")));
			this.tabbedView.AppearancePage.HeaderActive.BackColor2 = ((System.Drawing.Color)(resources.GetObject("tabbedView.AppearancePage.HeaderActive.BackColor2")));
			this.tabbedView.AppearancePage.HeaderActive.FontSizeDelta = ((int)(resources.GetObject("tabbedView.AppearancePage.HeaderActive.FontSizeDelta")));
			this.tabbedView.AppearancePage.HeaderActive.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("tabbedView.AppearancePage.HeaderActive.FontStyleDelta")));
			this.tabbedView.AppearancePage.HeaderActive.GradientMode = ((System.Drawing.Drawing2D.LinearGradientMode)(resources.GetObject("tabbedView.AppearancePage.HeaderActive.GradientMode")));
			this.tabbedView.AppearancePage.HeaderActive.Image = ((System.Drawing.Image)(resources.GetObject("tabbedView.AppearancePage.HeaderActive.Image")));
			this.tabbedView.AppearancePage.HeaderActive.Options.UseBackColor = true;
			this.tabbedView.AppearancePage.HeaderActive.Options.UseFont = true;
			this.tabbedView.FloatingDocumentContainer = DevExpress.XtraBars.Docking2010.Views.FloatingDocumentContainer.DocumentsHost;
			// 
			// folderBrowserDialog
			// 
			resources.ApplyResources(this.folderBrowserDialog, "folderBrowserDialog");
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
			((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tabbedView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar barToolButtons;
        private DevExpress.XtraBars.Bar barMenu;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
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
        private DevExpress.XtraBars.BarWorkspaceMenuItem barWorkspaceMenuItem1;
        private DevExpress.Utils.WorkspaceManager workspaceManager;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private DevExpress.XtraBars.BarSubItem barSubItem_OtherWindows;
    }
}