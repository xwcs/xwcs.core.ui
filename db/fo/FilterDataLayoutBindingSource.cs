using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.db.fo;
using xwcs.core.db.model;
using xwcs.core.evt;
using xwcs.core.ui.editors;

namespace xwcs.core.ui.db.fo
{
	public class FilterDataLayoutBindingSource : DataLayoutBindingSource, IFilterDataBindingSource
	{
		private FilterAspectForBindingSource _filterAspect;

		public FilterDataLayoutBindingSource(BarManager bm) : this((IEditorsHost)null, bm) { }
		public FilterDataLayoutBindingSource(BarManager bm, IContainer c) : this(null, bm, c) { }
		public FilterDataLayoutBindingSource(BarManager bm, object o, string s) : this(null, bm, o, s) { }
		public FilterDataLayoutBindingSource(IEditorsHost eh, BarManager bm) : base(eh) { start(bm); }
		public FilterDataLayoutBindingSource(IEditorsHost eh, BarManager bm, IContainer c) : base(eh, c) { start(bm); }
		public FilterDataLayoutBindingSource(IEditorsHost eh, BarManager bm, object o, string s) : base(eh, o, s) { start(bm); }

		private void start(BarManager bm)
		{
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

		/*
		public void HandleResetCriteria(string fn)
		{
			_filterAspect.HandleResetCriteria(fn);
		}
		*/

		public void ResetCurrentFo()
		{
			(Current as ICriteriaTreeNode)?.Reset();
		}
	}
}
