using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xwcs.core.ui.controls
{
    /// <summary>
    /// This class handle navigation in some CurrencyManager
    /// </summary>
    public class GridNavigatorManager :  IDisposable
    {
        private BarItem _first;
        private BarItem _prev;
        private BarItem _next;
        private BarItem _last;

        private GridView _gv;

        public GridNavigatorManager(GridView gv, BarItem f, BarItem p, BarItem n, BarItem l)
        {
            _gv = gv;
            _first = f;
            _prev = p;
            _next = n;
            _last = l;
            _gv.FocusedRowChanged += _gv_FocusedRowChanged; 
            if (_first != null){ _first.ItemClick += _first_Click;}
            if (_prev != null) { _prev.ItemClick += _prev_Click; }
            if (_next != null) { _next.ItemClick += _next_Click; }
            if (_last != null) { _last.ItemClick += _last_Click; }

        }

        private void _gv_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Update();
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                Update();
            }
        }
        
        private void _last_Click(object sender, EventArgs e)
        {
            _gv.MoveLast();
        }

        private void _next_Click(object sender, EventArgs e)
        {
            _gv.MoveNext();
        }

        private void _prev_Click(object sender, EventArgs e)
        {
            _gv.MovePrev();
        }

        private void _first_Click(object sender, EventArgs e)
        {
            _gv.MoveFirst();
        }

        private void Update()
        {
            if (_first != null) _first.Enabled = _enabled && !_gv.IsFirstRow;
            if (_prev != null) _prev.Enabled = _enabled && !_gv.IsFirstRow;
            if (_next != null) _next.Enabled = _enabled && !_gv.IsLastRow;
            if (_last != null) _last.Enabled = _enabled && !_gv.IsLastRow;
        }

         #region IDisposable Support
        private bool disposedValue = false; // Per rilevare chiamate ridondanti

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_first != null) _first.ItemClick -= _first_Click;
                    if (_prev != null) _prev.ItemClick -= _prev_Click;
                    if (_next != null) _next.ItemClick -= _next_Click;
                    if (_last != null) _last.ItemClick -= _last_Click;
                    _gv.FocusedRowChanged -= _gv_FocusedRowChanged;
                }

                // TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
                // TODO: impostare campi di grandi dimensioni su Null.

                disposedValue = true;
            }
        }

        // TODO: eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
        // ~NavigatorManager() {
        //   // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
        //   Dispose(false);
        // }

        // Questo codice viene aggiunto per implementare in modo corretto il criterio Disposable.
        public void Dispose()
        {
            // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
            Dispose(true);
            // TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override del finalizzatore.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
