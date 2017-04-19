using DevExpress.XtraBars;
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
    public class NavigatorManager :  IDisposable
    {
        private BarItem _first;
        private BarItem _prev;
        private BarItem _next;
        private BarItem _last;

        private CurrencyManager _cm;

        public NavigatorManager(CurrencyManager cm, BarItem f, BarItem p, BarItem n, BarItem l)
        {
            _cm = cm;
            _first = f;
            _prev = p;
            _next = n;
            _last = l;
            _cm.PositionChanged += _cm_PositionChanged;
            if (_first != null){ _first.ItemClick += _first_Click;}
            if (_prev != null) { _prev.ItemClick += _prev_Click; }
            if (_next != null) { _next.ItemClick += _next_Click; }
            if (_last != null) { _last.ItemClick += _last_Click; }

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

        private void _cm_PositionChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void _last_Click(object sender, EventArgs e)
        {
            _cm.Position = _cm.Count - 1;
        }

        private void _next_Click(object sender, EventArgs e)
        {
            _cm.Position += 1;
        }

        private void _prev_Click(object sender, EventArgs e)
        {
            _cm.Position -= 1;
        }

        private void _first_Click(object sender, EventArgs e)
        {
            _cm.Position = 0;
        }

        private void Update()
        {
            if (_first != null) _first.Enabled = _enabled && _cm.Position > 0;
            if (_prev != null) _prev.Enabled = _enabled && _cm.Position > 0;
            if (_next != null) _next.Enabled = _enabled && _cm.Position < _cm.Count - 1;
            if (_last != null) _last.Enabled = _enabled && _cm.Position < _cm.Count - 1;
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
                    _cm.PositionChanged -= _cm_PositionChanged;
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
