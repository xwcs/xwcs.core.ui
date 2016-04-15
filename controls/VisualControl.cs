using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.evt;
using xwcs.core.manager;
using System.Reflection;

namespace xwcs.core.ui.controls
{
	
	public abstract class VisualControl : DevExpress.XtraEditors.XtraUserControl, core.controls.IVisualControl, plgs.ISavable, plgs.persistent.IPersistentState
	{
		public core.controls.VisualControlInfo VisualControlInfo { get; private set; }

		public string ControlName { get { return VisualControlInfo.Name;  } }

		public VisualControl(core.controls.VisualControlInfo vci)
		{
			VisualControlInfo = vci;

			Enter += (s, e) =>
			{
				SEventProxy.getInstance().fireEvent(new VisualControlActionEvent(this, new VisualControlActionEventData(this, VisualControlActionKind.Activated)));
			};
			Disposed += (s, e) =>
			{
				SEventProxy.getInstance().fireEvent(new VisualControlActionEvent(this, new VisualControlActionEventData(this, VisualControlActionKind.Disposed)));
			};
		}

		public virtual void Start(core.controls.VisualControlStartingKind startingKind = core.controls.VisualControlStartingKind.StartingNew) {

		}

		#region IPersistentState

		protected object _State;

		protected virtual void BeforeSaveState() { }

		protected virtual void AfterLoadState() { }

		public string GetPersistorKey(string fn = "control")
		{
			return GetType().FullName + "/" + VisualControlInfo.InstanceGUID + "/" + fn;
		}

		public void SaveState()
		{
			BeforeSaveState();

			if (_State == null) return; //nothing to save

			MethodInfo method = typeof(SPersistenceManager).GetMethod("SaveObject");
			MethodInfo generic = method.MakeGenericMethod(_State.GetType());
			object[] pms = { GetPersistorKey(), _State };
			generic.Invoke(SPersistenceManager.getInstance(), pms);
		}

		public void LoadState()
		{
			MethodInfo method = typeof(SPersistenceManager).GetMethod("LoadObject");
			MethodInfo generic = method.MakeGenericMethod(_State.GetType());
			object[] pms = { GetPersistorKey(), _State };

			_State = null;

			if((bool)generic.Invoke(SPersistenceManager.getInstance(), pms)) {
				_State = pms[1];
			}

			AfterLoadState();
		}
		#endregion



		#region ISavable
		/// <summary>
		/// Default saving procedure
		/// </summary>
		virtual public void SaveChanges() { }
		#endregion
	}
}
