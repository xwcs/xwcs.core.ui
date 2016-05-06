namespace xwcs.core.ui.editors
{
	partial class GridEditControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.gridControl = new DevExpress.XtraGrid.GridControl();
			this.gridViewMain = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewMain)).BeginInit();
			this.SuspendLayout();
			// 
			// gridControl
			// 
			this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridControl.Location = new System.Drawing.Point(0, 0);
			this.gridControl.MainView = this.gridViewMain;
			this.gridControl.Name = "gridControl";
			this.gridControl.Size = new System.Drawing.Size(355, 273);
			this.gridControl.TabIndex = 0;
			this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMain});
			// 
			// gridViewMain
			// 
			this.gridViewMain.GridControl = this.gridControl;
			this.gridViewMain.Name = "gridViewMain";
			this.gridViewMain.OptionsView.ShowGroupPanel = false;
			this.gridViewMain.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridViewMain_PopupMenuShowing);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Name = "contextMenuStrip1";
			this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
			// 
			// GridEditControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridControl);
			this.MinimumSize = new System.Drawing.Size(0, 150);
			this.Name = "GridEditControl";
			this.Size = new System.Drawing.Size(355, 273);
			((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridControl;
		private DevExpress.XtraGrid.Views.Grid.GridView gridViewMain;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
	}
}
