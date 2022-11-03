using DevExpress.XtraBars;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xwcs.core.db;
using xwcs.core.db.binding;
using xwcs.core.db.binding.attributes;
using xwcs.core.db.fo;
using xwcs.core.db.model;
using xwcs.core.evt;
using xwcs.core.ui.editors;

namespace xwcs.core.ui.db.fo
{
	public class FilterDataLayoutBindingSource : DataLayoutBindingSource, IFilterDataBindingSource
	{
		private FilterAspectForBindingSource _filterAspect;
        private List<RepositoryItem> _repositoryItemsWithKeyDownHandler = new List<RepositoryItem>();

       
		public FilterDataLayoutBindingSource(BarManager bm) : this((IEditorsHost)null, bm) { start(); }
		public FilterDataLayoutBindingSource(BarManager bm, IContainer c) : this(null, bm, c) { start(); }
		public FilterDataLayoutBindingSource(BarManager bm, object o, string s) : this(null, bm, o, s) { start(); }
		public FilterDataLayoutBindingSource(IEditorsHost eh, BarManager bm) : base(eh) { start(bm); }
		public FilterDataLayoutBindingSource(IEditorsHost eh, BarManager bm, IContainer c) : base(eh, c) { start(bm); }
		public FilterDataLayoutBindingSource(IEditorsHost eh, BarManager bm, object o, string s) : base(eh, o, s) { start(bm); }

        private void start(BarManager bm = null)
		{

            LayoutBaseFileName = "search_form";
            if (ReferenceEquals(null, bm)) return;
            _filterAspect = new FilterAspectForBindingSource(this, EditorsHost, bm);

		}

        protected override void FieldRetrievedHandler(object sender, FieldRetrievedEventArgs e)
        {
            // call parent
            base.FieldRetrievedHandler(sender, e);
            // custom key down handling
            _repositoryItemsWithKeyDownHandler.Add(e.RepositoryItem);
            e.RepositoryItem.KeyDown += repItemKeyDownHandler;
        }

        private void repItemKeyDownHandler(object sender, KeyEventArgs ke)
        {
            if (ke.KeyCode == Keys.Delete || ke.KeyCode == Keys.Back)
            {
                if (
                    (ke.Control)
                    || ((sender as TextEdit)?.Text?.Length == 0) 
                    || !(sender is TextEdit)
                   )

                {
                    // reset field
                    FilterObjectbase fo = Current as FilterObjectbase;
                    if (fo != null)
                    {
                        // get field using binding
                        if ((sender as Control).DataBindings.Count > 0)
                        {
                            string FieldName = (sender as Control).DataBindings[0].BindingMemberInfo.BindingMember;

                            // avoid value kick back due to binder will react on PropertyChanged event and will pull 
                            // control data back from UI to model
                            // UI is not cleared 
                            (sender as Control).DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.Never;
                            // reset field
                            fo.ResetFieldByName(FieldName);
                            // turn back DS update mode
                            (sender as Control).DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                            // eventual null prompt
                            TextEdit te = sender as TextEdit;
                            if (te != null)
                            {
                                te.Properties.NullValuePrompt = "";

                                te.StyleController = EditorsHost.FormSupport.DefaultStyles.ContainsKey(te) ? EditorsHost.FormSupport.DefaultStyles[te] : null;
                            }
                            ke.Handled = true;
                        }
                    }
                }
            }
            /*
            else
            {
                TextEdit te = sender as TextEdit;
                if (te != null)
                {
                    te.StyleController = _ModifiedStyle;
				}
            } 
            */  
        }

        protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_filterAspect.Dispose();
                    foreach(RepositoryItem ri in _repositoryItemsWithKeyDownHandler)
                    {
                        ri.KeyDown -= repItemKeyDownHandler;
                    }
                    _repositoryItemsWithKeyDownHandler = null;
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

		public void Reset()
		{
            (Current as ICriteriaTreeNode)?.Reset();
		}

        

    }
}
