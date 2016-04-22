namespace xwcs.core.ui.editors
{
	partial class CustomIntervalPopupControl
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
			this.filterEditorControl = new DevExpress.XtraFilterEditor.FilterEditorControl();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// filterEditorControl
			// 
			this.filterEditorControl.AppearanceEmptyValueColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceFieldNameColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceGroupOperatorColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceOperatorColor = System.Drawing.Color.Empty;
			this.filterEditorControl.AppearanceValueColor = System.Drawing.Color.Empty;
			this.filterEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filterEditorControl.Location = new System.Drawing.Point(0, 0);
			this.filterEditorControl.Name = "filterEditorControl";
			this.filterEditorControl.Size = new System.Drawing.Size(200, 100);
			this.filterEditorControl.TabIndex = 0;
			this.filterEditorControl.Text = "filterEditorControl";
			this.filterEditorControl.UseMenuForOperandsAndOperators = false;
			// 
			// CustomIntervalPopupControl
			// 
			this.Controls.Add(this.filterEditorControl);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public DevExpress.XtraFilterEditor.FilterEditorControl filterEditorControl;
	}
}
