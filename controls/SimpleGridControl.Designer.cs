namespace xwcs.core.ui.controls
{
	partial class SimpleGridControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;



		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleGridControl));
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.gridControl = new DevExpress.XtraGrid.GridControl();
			this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.simpleButton_ADD = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButton_DELETE = new DevExpress.XtraEditors.SimpleButton();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel3.Controls.Add(this.gridControl, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(660, 420);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// gridControl
			// 
			this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridControl.Location = new System.Drawing.Point(3, 3);
			this.gridControl.MainView = this.gridView;
			this.gridControl.Name = "gridControl";
			this.gridControl.Size = new System.Drawing.Size(604, 414);
			this.gridControl.TabIndex = 0;
			this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
			this.gridView});
			// 
			// gridView
			// 
			this.gridView.GridControl = this.gridControl;
			this.gridView.Name = "gridView";
			this.gridView.OptionsBehavior.AllowSortAnimation = DevExpress.Utils.DefaultBoolean.False;
			this.gridView.OptionsBehavior.Editable = false;
			this.gridView.OptionsBehavior.ReadOnly = true;
			this.gridView.OptionsCustomization.AllowFilter = false;
			this.gridView.OptionsCustomization.AllowGroup = false;
			this.gridView.OptionsCustomization.AllowSort = false;
			this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
			this.gridView.OptionsView.ShowGroupPanel = false;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.simpleButton_ADD, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.simpleButton_DELETE, 0, 3);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(613, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(44, 414);
			this.tableLayoutPanel4.TabIndex = 1;
			// 
			// simpleButton_ADD
			// 
			this.simpleButton_ADD.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.simpleButton_ADD.Enabled = false;
			this.simpleButton_ADD.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_ADD.Image")));
			this.simpleButton_ADD.Location = new System.Drawing.Point(3, 34);
			this.simpleButton_ADD.Name = "simpleButton_ADD";
			this.simpleButton_ADD.Size = new System.Drawing.Size(38, 34);
			this.simpleButton_ADD.TabIndex = 2;
			this.simpleButton_ADD.Text = "simpleButtonEdizione_INSERT";
			// 
			// simpleButton_DELETE
			// 
			this.simpleButton_DELETE.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.simpleButton_DELETE.Enabled = false;
			this.simpleButton_DELETE.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_DELETE.Image")));
			this.simpleButton_DELETE.Location = new System.Drawing.Point(3, 344);
			this.simpleButton_DELETE.Name = "simpleButton_DELETE";
			this.simpleButton_DELETE.Size = new System.Drawing.Size(38, 34);
			this.simpleButton_DELETE.TabIndex = 3;
			this.simpleButton_DELETE.Text = "simpleButtonEdizione_DELETE";
			// 
			// SimpleGridControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "SimpleGridControl";
			this.Size = new System.Drawing.Size(660, 420);
			this.tableLayoutPanel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		public DevExpress.XtraGrid.GridControl gridControl;
		public DevExpress.XtraGrid.Views.Grid.GridView gridView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private DevExpress.XtraEditors.SimpleButton simpleButton_ADD;
		private DevExpress.XtraEditors.SimpleButton simpleButton_DELETE;
	}
}
