using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.db.binding.attributes;
using xwcs.core.db.fo;
using xwcs.core.db.model;
using xwcs.core.evt;
using xwcs.core.manager;
using xwcs.core.ui.editors;

namespace xwcs.core.ui.db.fo
{
	public class FilterGridBindingSource : GridBindingSource, IFilterDataBindingSource
	{
		private FilterAspectForBindingSource _filterAspect;

		public FilterGridBindingSource(BarManager bm) : this((IEditorsHost)null, bm) { }
		public FilterGridBindingSource(BarManager bm, IContainer c) : this(null, bm, c) { }
		public FilterGridBindingSource(BarManager bm, object o, string s) : this(null, bm, o, s) { }
		public FilterGridBindingSource(IEditorsHost eh, BarManager bm) : base(eh) { start(bm); }
		public FilterGridBindingSource(IEditorsHost eh, BarManager bm, IContainer c) : base(eh, c) { start(bm); }
		public FilterGridBindingSource(IEditorsHost eh, BarManager bm, object o, string s) : base(eh, o, s) { start(bm); }

		private void start(BarManager bm)
		{
			HandleCustomColumnDisplayText = true; //this will do event for text override for columns ( we need it for eventual criteria render )	
			_filterAspect = new FilterAspectForBindingSource(this, EditorsHost, bm);
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_filterAspect.Dispose();
				}
				base.Dispose(disposing);
			}
		}

		public void HandleFilterFiledKeyEvent(FilterFieldEventData ffe)
		{
			_filterAspect.HandleFilterFiledKeyEvent(ffe);
		}

	
		protected override void resetSlavesOfModifiedProperty(ResetSlavesAttribute att)
		{
			//should be overridden
			//we need reset slaves chain
			FilterObjectbase fo = Current as FilterObjectbase;
			att.Slaves.ToList().ForEach(
				s =>
				{
					fo.GetFilterFieldByPath(s)?.Reset();
				}
			);
		}

		protected override void GetFieldDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
		{
			try {
				FilterObjectbase fo = this[e.ListSourceRowIndex] as FilterObjectbase;
				if (fo != null)
				{
					ICriteriaTreeNode cn = fo.GetFilterFieldByPath(e.Column.FieldName);
					if (cn.HasCriteria())
					{
						string cond = cn.GetCondition().LegacyToString();
						e.DisplayText = cond;
					}
				}
			}
#if DEBUG
            catch(Exception ex) {
                SLogManager.getInstance().getClassLogger(GetType()).Debug(ex.Message);
#else
            catch (Exception ) {
#endif
            } //just silently skip problems

    }

		
	}
}
