using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.CustomEditor;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.db.binding.attributes;
using xwcs.core.db.fo;
using xwcs.core.ui.db.fo;

namespace xwcs.core.ui.editors.attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class UserControlEditAttribute : CustomAttribute
	{
		public Type ControlType { get; set; }

		public override void applyRetrievingAttribute(IDataLayoutExtender host, FieldRetrievingEventArgs e)
		{
			e.EditorType = typeof(AnyControlEdit);//typeof(UserControlEditor);
		}

		public override void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e)
		{
			GridEditControl cc = new GridEditControl();
			cc.GetFieldQueryable += (ss, ee) =>
			{
				host.onGetQueryable(ee);
			};
			(e.RepositoryItem as RepositoryItemAnyControl).Control = cc;
			/*
			(e.RepositoryItem as RepositoryItemUserControlEditor).ControlType = ControlType;
			(e.RepositoryItem as RepositoryItemUserControlEditor).GetFieldQueryable += (ss, ee) => {
				host.onGetQueryable(ee);
			};
			*/
		}
	}
}