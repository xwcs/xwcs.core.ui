namespace xwcs.core.ui.editors
{
	partial class FieldExpressionControl
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
			DevExpress.XtraLayout.RowDefinition rowDefinition1 = new DevExpress.XtraLayout.RowDefinition();
			DevExpress.XtraLayout.RowDefinition rowDefinition2 = new DevExpress.XtraLayout.RowDefinition();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.filterEditorControl = new DevExpress.XtraFilterEditor.FilterEditorControl();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButtonOk = new DevExpress.XtraEditors.SimpleButton();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.panelControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl1
			// 
			this.layoutControl1.AllowCustomization = false;
			this.layoutControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
			this.layoutControl1.Controls.Add(this.panelControl2);
			this.layoutControl1.Controls.Add(this.panelControl1);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1022, 304, 796, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(359, 223);
			this.layoutControl1.TabIndex = 3;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// panelControl2
			// 
			this.panelControl2.Controls.Add(this.filterEditorControl);
			this.panelControl2.Location = new System.Drawing.Point(12, 12);
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(335, 161);
			this.panelControl2.TabIndex = 5;
			// 
			// filterEditorControl
			// 
			this.filterEditorControl.Appearance.BackColor = System.Drawing.SystemColors.Control;
			this.filterEditorControl.Appearance.Options.UseBackColor = true;
			this.filterEditorControl.AppearanceEmptyValueColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceFieldNameColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceGroupOperatorColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceOperatorColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceValueColor = System.Drawing.Color.Empty;
			this.filterEditorControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.filterEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filterEditorControl.Location = new System.Drawing.Point(2, 2);
			this.filterEditorControl.Margin = new System.Windows.Forms.Padding(50);
			this.filterEditorControl.Name = "filterEditorControl";
			this.filterEditorControl.Size = new System.Drawing.Size(331, 157);
			this.filterEditorControl.TabIndex = 1;
			this.filterEditorControl.Text = "filterEditorControl";
			this.filterEditorControl.UseMenuForOperandsAndOperators = false;
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.simpleButtonCancel);
			this.panelControl1.Controls.Add(this.simpleButtonOk);
			this.panelControl1.Location = new System.Drawing.Point(12, 177);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(335, 34);
			this.panelControl1.TabIndex = 4;
			// 
			// simpleButtonCancel
			// 
			this.simpleButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.simpleButtonCancel.Location = new System.Drawing.Point(192, 6);
			this.simpleButtonCancel.Name = "simpleButtonCancel";
			this.simpleButtonCancel.Size = new System.Drawing.Size(66, 23);
			this.simpleButtonCancel.TabIndex = 6;
			this.simpleButtonCancel.Text = "Cancel";
			this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
			// 
			// simpleButtonOk
			// 
			this.simpleButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.simpleButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.simpleButtonOk.Location = new System.Drawing.Point(264, 5);
			this.simpleButtonOk.Name = "simpleButtonOk";
			this.simpleButtonOk.Size = new System.Drawing.Size(66, 23);
			this.simpleButtonOk.TabIndex = 5;
			this.simpleButtonOk.Text = "OK";
			this.simpleButtonOk.Click += new System.EventHandler(this.simpleButtonOk_Click);
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.layoutControlItem1});
			this.layoutControlGroup1.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table;
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			columnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent;
			columnDefinition1.Width = 100D;
			this.layoutControlGroup1.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(new DevExpress.XtraLayout.ColumnDefinition[] {
            columnDefinition1});
			rowDefinition1.Height = 100D;
			rowDefinition1.SizeType = System.Windows.Forms.SizeType.Percent;
			rowDefinition2.Height = 38D;
			rowDefinition2.SizeType = System.Windows.Forms.SizeType.Absolute;
			this.layoutControlGroup1.OptionsTableLayoutGroup.RowDefinitions.AddRange(new DevExpress.XtraLayout.RowDefinition[] {
            rowDefinition1,
            rowDefinition2});
			this.layoutControlGroup1.Size = new System.Drawing.Size(359, 223);
			this.layoutControlGroup1.TextVisible = false;
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.panelControl1;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 165);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.OptionsTableLayoutItem.RowIndex = 1;
			this.layoutControlItem2.Size = new System.Drawing.Size(339, 38);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			// 
			// emptySpaceItem1
			// 
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
			this.emptySpaceItem1.MaxSize = new System.Drawing.Size(10, 27);
			this.emptySpaceItem1.MinSize = new System.Drawing.Size(10, 27);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(339, 165);
			this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			// 
			// emptySpaceItem2
			// 
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
			this.emptySpaceItem2.Location = new System.Drawing.Point(0, 0);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(339, 165);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.panelControl2;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(339, 165);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			// 
			// FieldExpressionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.layoutControl1);
			this.Name = "FieldExpressionControl";
			this.Size = new System.Drawing.Size(359, 223);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.panelControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
		private DevExpress.XtraEditors.SimpleButton simpleButtonOk;
		public DevExpress.XtraFilterEditor.FilterEditorControl filterEditorControl;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
		private DevExpress.XtraEditors.PanelControl panelControl2;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
	}
}
