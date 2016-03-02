using System;
using DevExpress.XtraDataLayout;

namespace xwcs.core.ui.datalayout.attributes
{
	public class CustomAttribute : Attribute
	{
		public virtual void applyRetrievingAttribute(IDataLayoutExtender host, FieldRetrievingEventArgs e) { }
		public virtual void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e) { }
	}
}
