using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xwcs.core.ui.controls
{
	public class Content<V> : XtraUserControl where V : VisualControl
	{
		public V Root { get; set; } = null;
		public ContentSource CntSrc { get; set; } = null;
		
	}

	public interface IContentFactory {
		UserControl GetContent(ContentSource cntsrc);
	}

	/// <summary>
	/// Factory for content creation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ContentFactory<T, V> : IContentFactory where V : VisualControl where T : Content<V>, new() 
	{
		private V _root = null;

		public ContentFactory(V root) {
			_root = root;
		}

		public UserControl GetContent(ContentSource cntsrc) {
			T ret = new T();
			ret.Root = _root;
			ret.CntSrc = cntsrc;
			return ret;
		}	
	}
	/// <summary>
	/// Class for visual content providing
	/// There are views and various layouts which can show provided content
	/// this source will contain handle creation and destruction of registered contents
	/// </summary>
	public class ContentSource : IDisposable
	{
		private Dictionary<string, IContentFactory> _factories = new Dictionary<string, IContentFactory>();

		private Dictionary<string, UserControl> _data = new Dictionary<string, UserControl>();

		public void RegisterFactory(string name, IContentFactory fct) {
			if (disposedValue) throw new InvalidOperationException("Source is disposed!");
			_factories.Add(name, fct);
		}

		public UserControl GetContent(string key) {
			if (disposedValue) throw new InvalidOperationException("Source is disposed!");
			if (!_data.ContainsKey(key)) {
				if(_factories.ContainsKey(key)) {
					_data[key] = _factories[key].GetContent(this);
				}else {
					throw new InvalidOperationException(String.Format("Missing factory for content [{0}]!", key));
				}
			}
			return _data[key];			
		}

		#region IDisposable Support
		private bool disposedValue = false; // Per rilevare chiamate ridondanti

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach(IDisposable d in _data.Values) {
						d.Dispose();
					}

					_data.Clear();
					_data = null;
				}

				// TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
				// TODO: impostare campi di grandi dimensioni su Null.

				disposedValue = true;
			}
		}

		// TODO: eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
		// ~ContentSource() {
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
