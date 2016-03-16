using System;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors.Repository;

namespace xwcs.core.ui.datalayout.attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class Layout : CustomAttribute
	{
        public string DisplayMember { set; get; } = "Test";
        public string ValueMember { set; get; } = "Test";

		public override void applyRetrievingAttribute(IDataLayoutExtender host, FieldRetrievingEventArgs e)
		{
			e.EditorType = typeof(DevExpress.XtraDataLayout.DataLayoutControl);
		}

		public override void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e)
		{
            /*
            RepositoryItemLookUpEdit rle = e.RepositoryItem as RepositoryItemLookUpEdit;
			rle.DisplayMember = DisplayMember;
			rle.ValueMember = ValueMember;
			GetFieldQueryableEventData qd = new GetFieldQueryableEventData { DataSource = null, FieldName = e.FieldName };
			host.onGetQueryable(qd);
			if (qd.DataSource != null)
			{
				rle.DataSource = qd.DataSource;
			}
            */
		}
	}
}
