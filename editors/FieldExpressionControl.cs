using System;
using xwcs.core.evt;

namespace xwcs.core.ui.editors
{
	public partial class FieldExpressionControl : DevExpress.XtraEditors.XtraUserControl
	{
		//public EventHandler<EventArgs> OnOk;
		private readonly WeakEventSource<EventArgs> _wes_OnOk = new WeakEventSource<EventArgs>();
		public event EventHandler<EventArgs> OnOk
		{
			add { _wes_OnOk.Subscribe(value); }
			remove { _wes_OnOk.Unsubscribe(value); }
		}
		//public EventHandler<EventArgs> OnCancel;
		private readonly WeakEventSource<EventArgs> _wes_OnCancel = new WeakEventSource<EventArgs>();
		public event EventHandler<EventArgs> OnCancel
		{
			add { _wes_OnCancel.Subscribe(value); }
			remove { _wes_OnCancel.Unsubscribe(value); }
		}


		public string CurrentFieldName { get; set; }

		public FieldExpressionControl()
		{
			InitializeComponent();
			simpleButtonOk.Click += simpleButtonOk_Click;
			simpleButtonCancel.Click += simpleButtonCancel_Click;
		}

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

			simpleButtonOk.Click -= simpleButtonOk_Click;
			simpleButtonCancel.Click -= simpleButtonCancel_Click;

			base.Dispose(disposing);
		}

		private void simpleButtonCancel_Click(object sender, System.EventArgs e)
		{
			_wes_OnCancel.Raise(sender, e);
		}

		private void simpleButtonOk_Click(object sender, System.EventArgs e)
		{
			_wes_OnOk.Raise(sender, e);
		}
	}
}
