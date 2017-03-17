namespace xwcs.core.ui.app
{
	partial class ApplicationRibbonFormBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationRibbonFormBase));
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem_FileSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_FileSaveAll = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_WS_New = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_WS_Change = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_View_Props = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_View_Status = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_View_Home = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Tools_Options = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPagePlugins = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupHomeFile = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupPlgs_generic = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageTools = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupHomeWorkspace = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupHomeView = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupHomeTools = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.workspaceManager = new DevExpress.Utils.WorkspaceManager();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.barButtonItem_FileSave,
            this.barButtonItem_FileSaveAll,
            this.barButtonItem_WS_New,
            this.barButtonItem_WS_Change,
            this.barButtonItem_View_Props,
            this.barButtonItem_View_Status,
            this.barButtonItem_View_Home,
            this.barButtonItem_Tools_Options});
            resources.ApplyResources(this.ribbonControl, "ribbonControl");
            this.ribbonControl.MaxItemId = 12;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPagePlugins,
            this.ribbonPageTools});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl.StatusBar = this.ribbonStatusBar;
            // 
            // barButtonItem_FileSave
            // 
            resources.ApplyResources(this.barButtonItem_FileSave, "barButtonItem_FileSave");
            this.barButtonItem_FileSave.Id = 1;
            this.barButtonItem_FileSave.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.barButtonItem_FileSave.ImageOptions.DisabledImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSave.ImageOptions.DisabledImage")));
            this.barButtonItem_FileSave.ImageOptions.DisabledLargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSave.ImageOptions.DisabledLargeImage")));
            this.barButtonItem_FileSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSave.ImageOptions.Image")));
            this.barButtonItem_FileSave.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSave.ImageOptions.LargeImage")));
            this.barButtonItem_FileSave.Name = "barButtonItem_FileSave";
            this.barButtonItem_FileSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.barButtonItem_FileSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSave_ItemClick);
            // 
            // barButtonItem_FileSaveAll
            // 
            resources.ApplyResources(this.barButtonItem_FileSaveAll, "barButtonItem_FileSaveAll");
            this.barButtonItem_FileSaveAll.Id = 3;
            this.barButtonItem_FileSaveAll.ImageOptions.DisabledLargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSaveAll.ImageOptions.DisabledLargeImage")));
            this.barButtonItem_FileSaveAll.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSaveAll.ImageOptions.Image")));
            this.barButtonItem_FileSaveAll.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_FileSaveAll.ImageOptions.LargeImage")));
            this.barButtonItem_FileSaveAll.Name = "barButtonItem_FileSaveAll";
            this.barButtonItem_FileSaveAll.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.barButtonItem_FileSaveAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_SaveAll_ItemClick);
            // 
            // barButtonItem_WS_New
            // 
            resources.ApplyResources(this.barButtonItem_WS_New, "barButtonItem_WS_New");
            this.barButtonItem_WS_New.Id = 4;
            this.barButtonItem_WS_New.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_WS_New.ImageOptions.Image")));
            this.barButtonItem_WS_New.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_WS_New.ImageOptions.LargeImage")));
            this.barButtonItem_WS_New.Name = "barButtonItem_WS_New";
            this.barButtonItem_WS_New.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_Createnew_ItemClick);
            // 
            // barButtonItem_WS_Change
            // 
            resources.ApplyResources(this.barButtonItem_WS_Change, "barButtonItem_WS_Change");
            this.barButtonItem_WS_Change.Id = 5;
            this.barButtonItem_WS_Change.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_WS_Change.ImageOptions.Image")));
            this.barButtonItem_WS_Change.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_WS_Change.ImageOptions.LargeImage")));
            this.barButtonItem_WS_Change.Name = "barButtonItem_WS_Change";
            this.barButtonItem_WS_Change.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_Change_ItemClick);
            // 
            // barButtonItem_View_Props
            // 
            resources.ApplyResources(this.barButtonItem_View_Props, "barButtonItem_View_Props");
            this.barButtonItem_View_Props.Id = 6;
            this.barButtonItem_View_Props.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_View_Props.ImageOptions.Image")));
            this.barButtonItem_View_Props.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_View_Props.ImageOptions.LargeImage")));
            this.barButtonItem_View_Props.Name = "barButtonItem_View_Props";
            // 
            // barButtonItem_View_Status
            // 
            resources.ApplyResources(this.barButtonItem_View_Status, "barButtonItem_View_Status");
            this.barButtonItem_View_Status.Id = 7;
            this.barButtonItem_View_Status.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_View_Status.ImageOptions.Image")));
            this.barButtonItem_View_Status.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_View_Status.ImageOptions.LargeImage")));
            this.barButtonItem_View_Status.Name = "barButtonItem_View_Status";
            // 
            // barButtonItem_View_Home
            // 
            resources.ApplyResources(this.barButtonItem_View_Home, "barButtonItem_View_Home");
            this.barButtonItem_View_Home.Id = 8;
            this.barButtonItem_View_Home.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_View_Home.ImageOptions.Image")));
            this.barButtonItem_View_Home.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_View_Home.ImageOptions.LargeImage")));
            this.barButtonItem_View_Home.Name = "barButtonItem_View_Home";
            // 
            // barButtonItem_Tools_Options
            // 
            resources.ApplyResources(this.barButtonItem_Tools_Options, "barButtonItem_Tools_Options");
            this.barButtonItem_Tools_Options.Id = 9;
            this.barButtonItem_Tools_Options.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Tools_Options.ImageOptions.Image")));
            this.barButtonItem_Tools_Options.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Tools_Options.ImageOptions.LargeImage")));
            this.barButtonItem_Tools_Options.Name = "barButtonItem_Tools_Options";
            // 
            // ribbonPagePlugins
            // 
            this.ribbonPagePlugins.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupHomeFile,
            this.ribbonPageGroupPlgs_generic});
            this.ribbonPagePlugins.Image = ((System.Drawing.Image)(resources.GetObject("ribbonPagePlugins.Image")));
            this.ribbonPagePlugins.MergeOrder = 1;
            this.ribbonPagePlugins.Name = "ribbonPagePlugins";
            resources.ApplyResources(this.ribbonPagePlugins, "ribbonPagePlugins");
            // 
            // ribbonPageGroupHomeFile
            // 
            this.ribbonPageGroupHomeFile.ItemLinks.Add(this.barButtonItem_FileSave);
            this.ribbonPageGroupHomeFile.ItemLinks.Add(this.barButtonItem_FileSaveAll);
            this.ribbonPageGroupHomeFile.Name = "ribbonPageGroupHomeFile";
            resources.ApplyResources(this.ribbonPageGroupHomeFile, "ribbonPageGroupHomeFile");
            // 
            // ribbonPageGroupPlgs_generic
            // 
            this.ribbonPageGroupPlgs_generic.Name = "ribbonPageGroupPlgs_generic";
            resources.ApplyResources(this.ribbonPageGroupPlgs_generic, "ribbonPageGroupPlgs_generic");
            // 
            // ribbonPageTools
            // 
            this.ribbonPageTools.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupHomeWorkspace,
            this.ribbonPageGroupHomeView,
            this.ribbonPageGroupHomeTools});
            this.ribbonPageTools.Image = ((System.Drawing.Image)(resources.GetObject("ribbonPageTools.Image")));
            this.ribbonPageTools.MergeOrder = 1000;
            this.ribbonPageTools.Name = "ribbonPageTools";
            resources.ApplyResources(this.ribbonPageTools, "ribbonPageTools");
            // 
            // ribbonPageGroupHomeWorkspace
            // 
            this.ribbonPageGroupHomeWorkspace.ItemLinks.Add(this.barButtonItem_WS_New);
            this.ribbonPageGroupHomeWorkspace.ItemLinks.Add(this.barButtonItem_WS_Change);
            this.ribbonPageGroupHomeWorkspace.Name = "ribbonPageGroupHomeWorkspace";
            resources.ApplyResources(this.ribbonPageGroupHomeWorkspace, "ribbonPageGroupHomeWorkspace");
            // 
            // ribbonPageGroupHomeView
            // 
            this.ribbonPageGroupHomeView.ItemLinks.Add(this.barButtonItem_View_Home);
            this.ribbonPageGroupHomeView.ItemLinks.Add(this.barButtonItem_View_Props);
            this.ribbonPageGroupHomeView.ItemLinks.Add(this.barButtonItem_View_Status);
            this.ribbonPageGroupHomeView.Name = "ribbonPageGroupHomeView";
            resources.ApplyResources(this.ribbonPageGroupHomeView, "ribbonPageGroupHomeView");
            // 
            // ribbonPageGroupHomeTools
            // 
            this.ribbonPageGroupHomeTools.ItemLinks.Add(this.barButtonItem_Tools_Options);
            this.ribbonPageGroupHomeTools.Name = "ribbonPageGroupHomeTools";
            resources.ApplyResources(this.ribbonPageGroupHomeTools, "ribbonPageGroupHomeTools");
            // 
            // ribbonStatusBar
            // 
            resources.ApplyResources(this.ribbonStatusBar, "ribbonStatusBar");
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl;
            // 
            // documentManager
            // 
            this.documentManager.ContainerControl = this;
            this.documentManager.MenuManager = this.ribbonControl;
            this.documentManager.View = this.tabbedView;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView});
            // 
            // tabbedView
            // 
            this.tabbedView.RootContainer.Element = null;
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
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
            this.dockManager.Load += new System.EventHandler(this.dockManager_Load);
            // 
            // workspaceManager
            // 
            this.workspaceManager.CloseStreamOnWorkspaceLoading = DevExpress.Utils.DefaultBoolean.True;
            this.workspaceManager.CloseStreamOnWorkspaceSaving = DevExpress.Utils.DefaultBoolean.True;
            this.workspaceManager.TargetControl = this;
            this.workspaceManager.TransitionType = pushTransition1;
            this.workspaceManager.AfterApplyWorkspace += new System.EventHandler(this.workspaceManager_AfterApplyWorkspace);
            this.workspaceManager.WorkspaceSaved += new DevExpress.Utils.WorkspaceEventHandler(this.workspaceManager_WorkspaceSaved);
            // 
            // ApplicationRibbonFormBase
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.True;
            this.Appearance.Options.UseFont = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonControl);
            this.Name = "ApplicationRibbonFormBase";
            this.Ribbon = this.ribbonControl;
            this.StatusBar = this.ribbonStatusBar;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationFormBase_FormClosing);
            this.Shown += new System.EventHandler(this.ApplicationFormBase_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageTools;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupHomeFile;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupHomeWorkspace;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_FileSave;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupHomeView;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupHomeTools;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_FileSaveAll;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_WS_New;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_WS_Change;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_View_Props;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_View_Status;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_View_Home;
		private DevExpress.XtraBars.BarButtonItem barButtonItem_Tools_Options;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPagePlugins;
		private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView;
		private DevExpress.XtraBars.Docking.DockManager dockManager;
		private DevExpress.Utils.WorkspaceManager workspaceManager;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupPlgs_generic;
	}
}