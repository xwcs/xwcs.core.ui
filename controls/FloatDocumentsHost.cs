using DevExpress.XtraBars.Docking2010;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views;

namespace xwcs.core.ui.controls
{
	public class ViewBaseEventsHandler
	{
		static DocumentEventHandler deactivateHandler = (object s, DocumentEventArgs docEvt) =>
		{
			Console.WriteLine("Deactivate " + (docEvt.Document.Control != null ? docEvt.Document.Control.Name : ""));
		};

		static DocumentEventHandler activateHandler = (object s, DocumentEventArgs docEvt) =>
		{
			Console.WriteLine("Activated " + (docEvt.Document.Control != null ? docEvt.Document.Control.Name : ""));
		};

		static DocumentEventHandler addedHandler = (object s, DocumentEventArgs docEvt) =>
		{
			Console.WriteLine("Added " + (docEvt.Document.Control != null ? docEvt.Document.Control.Name : ""));
		};

		static DocumentEventHandler deactivatedHandler = (object s, DocumentEventArgs docEvt) =>
		{
			Console.WriteLine("Deactivated " + (docEvt.Document.Control != null ? docEvt.Document.Control.Name : ""));
		};

		static DocumentEventHandler removedHandler = (object s, DocumentEventArgs docEvt) =>
		{
			Console.WriteLine("Removed " + (docEvt.Document.Control != null ? docEvt.Document.Control.Name : ""));
		};

		static DocumentCancelEventHandler closingHandler = (object s, DocumentCancelEventArgs docEvt) =>
		{
			Console.WriteLine("Closing " + (docEvt.Document.Control != null ? docEvt.Document.Control.Name : ""));
		};

		static CustomDocumentsHostWindowEventHandler customDocumentsHandler = (object o, CustomDocumentsHostWindowEventArgs e) =>
		{
			e.Constructor = () => new FloatDocumentsHost();
		};

		static public void AttachToView(BaseView view)
		{
			view.DocumentDeactivated += deactivateHandler;

			view.DocumentActivated += activateHandler;

			view.DocumentAdded += addedHandler;

			view.DocumentDeactivated += deactivatedHandler;

			view.DocumentRemoved += removedHandler;

			view.DocumentClosing += closingHandler;

			view.CustomDocumentsHostWindow += customDocumentsHandler;
		}

		static public void DettachFromView(BaseView view) {
			view.DocumentDeactivated -= deactivateHandler;

			view.DocumentActivated -= activateHandler;

			view.DocumentAdded -= addedHandler;

			view.DocumentDeactivated -= deactivatedHandler;

			view.DocumentRemoved -= removedHandler;

			view.DocumentClosing -= closingHandler;

			view.CustomDocumentsHostWindow -= customDocumentsHandler;
		}
	}

	public class FloatDocumentsHost : Form, IDocumentsHostWindow
	{
		DocumentManager _docMan;

		public FloatDocumentsHost() {
			_docMan = new DocumentManager();
			_docMan.ContainerControl = this;
			_docMan.View.FloatingDocumentContainer = FloatingDocumentContainer.DocumentsHost;
			ViewBaseEventsHandler.AttachToView(_docMan.View);
		} 

		public bool DestroyOnRemovingChildren
		{
			get
			{
				return true;
			}
		}

		public DocumentManager DocumentManager
		{
			get
			{
				return _docMan;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				ViewBaseEventsHandler.DettachFromView(_docMan.View);
				_docMan.Dispose();
			}				
			base.Dispose(disposing);
		}
	}
}
