using System;


namespace xwcs.core.ui.editors
{
	public partial class FieldExpressionControl : DevExpress.XtraEditors.XtraUserControl
	{
		public EventHandler<EventArgs> OnOk;
		public EventHandler<EventArgs> OnCancel;

		public string CurrentFieldName { get; set; }

		public FieldExpressionControl()
		{
			InitializeComponent();
		}

		private void simpleButtonCancel_Click(object sender, System.EventArgs e)
		{
			OnCancel?.Invoke(sender, e);
		}

		private void simpleButtonOk_Click(object sender, System.EventArgs e)
		{
			OnOk?.Invoke(sender, e);
		}
	}
}
