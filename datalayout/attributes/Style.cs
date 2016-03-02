using System;
using DevExpress.XtraDataLayout;

namespace xwcs.core.ui.datalayout.attributes
{
	[AttributeUsage(AttributeTargets.Property,	AllowMultiple = true)]
	public class Style : CustomAttribute
	{
		UInt32 _backGrndColor;
		bool _backGrndColorUsed;

		public Style() {
			_backGrndColorUsed = false; //default
		}

		public UInt32 BackgrounColor
		{
			get { return _backGrndColor; }
			set { _backGrndColorUsed = true;  _backGrndColor = value; }
		}

		public override void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e) {
			if (_backGrndColorUsed) e.Control.BackColor = System.Drawing.Color.FromArgb((int)_backGrndColor);	 
		}
	}	
}
