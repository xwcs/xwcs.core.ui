using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
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
		public string ActionChars { get; set; } = "<:>*";
		public IDataBindingSource Src { get; private set;  }
		public string FieldName { get; private set; } = null;
		public RepositoryItem Ri { get; private set; } = null;

		//layout like
		public override void applyRetrievedAttribute(IDataBindingSource src, FieldRetrievedEventArgs e)
		{
			setupRle(src, e.RepositoryItem, e.FieldName);
		}

		// grid like container
		public override void applyCustomEditShown(IDataBindingSource src, ViewEditorShownEventArgs e)
		{
			setupRle(src, e.RepositoryItem, e.FieldName);
		}

		private void setupRle(IDataBindingSource src, RepositoryItem ri, string fn)
		{
			//first detach eventual old
			if (Ri != null)
			{
				Ri.KeyPress -= repItemKeyPressHandler;
			}
			Src = src;
			Ri = ri;
			FieldName = fn;
			//get field
			RepositoryItemTextEdit rte = Ri as RepositoryItemTextEdit;
			if (rte != null)
			{
				FilterObjectbase fo = src.Current as FilterObjectbase;
				if (fo != null)
				{
					ICriteriaTreeNode cn = fo.GetFilterFieldByPath(FieldName);
					if (cn.HasCriteria())
					{
						string cond = cn.GetCondition().LegacyToString();
						rte.NullValuePrompt = cond;
					}
				}
			}

			ri.KeyPress += repItemKeyPressHandler;

			//in case of button edit set it editable by hand
			RepositoryItemButtonEdit ribe = (ri as RepositoryItemButtonEdit);
			if (ribe != null)
			{
				ribe.TextEditStyle = TextEditStyles.Standard;
			}
		}


		public override void unbind(IDataBindingSource src)
		{
			if(Ri != null) {
				Ri.KeyPress -= repItemKeyPressHandler;
			}
			base.unbind(src);
		}

		private void repItemKeyPressHandler(object sender, KeyPressEventArgs ke) {
			if (ActionChars.Contains(ke.KeyChar))
			{
				ke.Handled = true;
				IFilterDataBindingSource fe = Src as IFilterDataBindingSource;
				fe?.HandleFilterFiledKeyEvent(new FilterFieldEventData { Field = sender, FieldName = FieldName, ActionChar = ke.KeyChar });
			}
		}
	}
}