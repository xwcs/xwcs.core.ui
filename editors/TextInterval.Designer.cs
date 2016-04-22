namespace xwcs.core.ui.editors
{
	partial class TextInterval
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
			DevExpress.XtraLayout.ColumnDefinition columnDefinition1 = new DevExpress.XtraLayout.ColumnDefinition();
			DevExpress.XtraLayout.ColumnDefinition columnDefinition2 = new DevExpress.XtraLayout.ColumnDefinition();
			DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
			DevExpress.XtraLayout.RowDefinition rowDefinition2 = new DevExpress.XtraLayout.RowDefinition();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.ConfirmButton = new DevExpress.XtraEditors.SimpleButton();
			this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.MinVal = new DevExpress.XtraEditors.TextEdit();
			this.MaxVal = new DevExpress.XtraEditors.TextEdit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
			this.panelControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MinVal.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MaxVal.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl1
			// 
			this.layoutControl1.Controls.Add(this.panelControl3);
			this.layoutControl1.Controls.Add(this.panelControl2);
			this.layoutControl1.Controls.Add(this.panelControl1);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(847, 221, 661, 380);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(287, 151);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// panelControl3
			// 
			this.panelControl3.Controls.Add(this.radioGroup1);
			this.panelControl3.Location = new System.Drawing.Point(12, 12);
			this.panelControl3.Name = "panelControl3";
			this.panelControl3.Size = new System.Drawing.Size(46, 92);
			this.panelControl3.TabIndex = 6;
			// 
			// panelControl2
			// 
			this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl2.Controls.Add(this.ConfirmButton);
			this.panelControl2.Location = new System.Drawing.Point(12, 108);
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(263, 31);
			this.panelControl2.TabIndex = 5;
			// 
			// ConfirmButton
			// 
			this.ConfirmButton.Location = new System.Drawing.Point(185, 2);
			this.ConfirmButton.Name = "ConfirmButton";
			this.ConfirmButton.Size = new System.Drawing.Size(75, 23);
			this.ConfirmButton.TabIndex = 1;
			this.ConfirmButton.Text = "Ok";
			// 
			// radioGroup1
			// 
			this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radioGroup1.Location = new System.Drawing.Point(2, 2);
			this.radioGroup1.Name = "radioGroup1";
			this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
			this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
			this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.radioGroup1.Size = new System.Drawing.Size(42, 88);
			this.radioGroup1.TabIndex = 0;
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.MaxVal);
			this.panelControl1.Controls.Add(this.MinVal);
			this.panelControl1.Location = new System.Drawing.Point(62, 12);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(213, 92);
			this.panelControl1.TabIndex = 4;
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.CustomHeaderButtons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton()});
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
			this.layoutControlGroup1.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			columnDefinition1.SizeType = System.Windows.Forms.SizeType.Absolute;
			columnDefinition1.Width = 50D;
			columnDefinition2.SizeType = System.Windows.Forms.SizeType.Percent;
			columnDefinition2.Width = 100D;
			this.layoutControlGroup1.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1,
            columnDefinition2});
			rowDefinition1.Height = 100D;
			rowDefinition1.SizeType = System.Windows.Forms.SizeType.Percent;
			rowDefinition2.Height = 35D;
			rowDefinition2.SizeType = System.Windows.Forms.SizeType.Absolute;
			this.layoutControlGroup1.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2});
			this.layoutControlGroup1.Size = new System.Drawing.Size(287, 151);
			this.layoutControlGroup1.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.panelControl1;
			this.layoutControlItem1.Location = new System.Drawing.Point(50, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.OptionsTableLayoutItem.ColumnIndex = 1;
			this.layoutControlItem1.Size = new System.Drawing.Size(217, 96);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.panelControl2;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 96);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.OptionsTableLayoutItem.ColumnSpan = 2;
			this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 1;
			this.layoutControlItem2.Size = new System.Drawing.Size(267, 35);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			// 
			// layoutControlItem3
			// 
			this.layoutControlItem3.Control = this.panelControl3;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(50, 96);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			// 
			// MinVal
			// 
			this.MinVal.Location = new System.Drawing.Point(5, 5);
			this.MinVal.Name = "MinVal";
			this.MinVal.Size = new System.Drawing.Size(203, 20);
			this.MinVal.TabIndex = 0;
			// 
			// MaxVal
			// 
			this.MaxVal.Location = new System.Drawing.Point(5, 31);
			this.MaxVal.Name = "MaxVal";
			this.MaxVal.Size = new System.Drawing.Size(203, 20);
			this.MaxVal.TabIndex = 1;
			// 
			// TextInterval
			// 
			this.Controls.Add(this.layoutControl1);
			this.Name = "TextInterval";
			this.Size = new System.Drawing.Size(287, 151);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
			this.panelControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MinVal.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MaxVal.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private DevExpress.XtraEditors.RadioGroup radioGroup1;
		private DevExpress.XtraEditors.PanelControl panelControl3;
		private DevExpress.XtraEditors.SimpleButton ConfirmButton;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
		public DevExpress.XtraEditors.TextEdit MaxVal;
		public DevExpress.XtraEditors.TextEdit MinVal;
	}
}
