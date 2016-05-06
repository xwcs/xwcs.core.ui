using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
	public class IntervalEditAttribute : CustomAttribute
	{
		public string ActionChars { get; set; } = "<:>";
		public override void applyRetrievingAttribute(IDataLayoutExtender host, FieldRetrievingEventArgs e)
		{
		}

		public override void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e)
		{
			//get field
			RepositoryItemTextEdit rte = e.RepositoryItem as RepositoryItemTextEdit;
			if (rte != null)
			{
				FilterObjectbase fo = host.Current as FilterObjectbase;
				if (fo != null)
				{
					ICriteriaTreeNode cn = fo.GetFilterFieldByPath(e.FieldName);
					if (cn.HasCriteria())
					{
						string cond = cn.GetCondition().LegacyToString();
						rte.NullValuePrompt = cond;
					}
				}
			}

			// default set italic
			//(e.RepositoryItem as RepositoryItemTextEdit).Appearance.Font = new Font(DevExpress.Utils.AppearanceObject.DefaultFont, FontStyle.Italic);

			// events
			/*
			e.RepositoryItem.EditValueChanging += (object s, ChangingEventArgs ce) =>
			{
				TextEdit te = s as TextEdit;
				if(ce.NewValue != null) {
					te.Properties.Appearance.Font = new Font(DevExpress.Utils.AppearanceObject.DefaultFont, FontStyle.Regular);
				}
				else {
					te.Properties.Appearance.Font = new Font(DevExpress.Utils.AppearanceObject.DefaultFont, FontStyle.Italic);
				}	
			};
			*/
			
			e.RepositoryItem.KeyPress += (object s, KeyPressEventArgs ke) =>
			{
				
				if (ActionChars.Contains(ke.KeyChar))
				{
					ke.Handled = true;
					IFilterDataLayoutExtender fe = host as IFilterDataLayoutExtender;
					fe?.onFilterFieldEvent(new FilterFieldEventData { Field = s, FREA = e, ActionChar = ke.KeyChar });
				}
			};
			

			RepositoryItemButtonEdit ribe = (e.RepositoryItem as RepositoryItemButtonEdit);
			if(ribe != null) {
				ribe.TextEditStyle = TextEditStyles.Standard;
			}			
		}
	}
}