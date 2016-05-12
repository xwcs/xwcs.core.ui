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
		
		public override void applyRetrievingAttribute(IDataBindingSource src, FieldRetrievingEventArgs e)
		{
			e.EditorType = typeof(CustomAnyControlEdit); //typeof(UserControlEditor);//typeof(AnyControlEdit);//typeof(UserControlEditor);
		}

		public override void applyRetrievedAttribute(IDataBindingSource src, FieldRetrievedEventArgs e)
		{
			//manage disconnect and create control
			(e.RepositoryItem as RepositoryItemCustomAnyControl).ControlType  = ControlType;

			//handle connect
			IEditorsHostProvider editorsHost = (e.RepositoryItem as RepositoryItemCustomAnyControl).Control as IEditorsHostProvider;
			if(editorsHost != null)
			{
				editorsHost.EditorsHost = src.EditorsHost; // here we connect to main editors host
			}
		}
	}
}