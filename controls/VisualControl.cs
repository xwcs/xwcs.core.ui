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
using DevExpress.XtraBars.Ribbon;

namespace xwcs.core.ui.controls
{

	public class VisualControl : DevExpress.XtraEditors.XtraUserControl, core.controls.IVisualControl, plgs.ISavable, plgs.persistent.IPersistentState
	{

		#region CONSTANTS 
		public const bool ALLOW_MULTI = false;
		#endregion

		private IContainer components;

		public core.controls.VisualControlInfo VisualControlInfo { get; private set; }

		public string ControlName { get { return VisualControlInfo.Name; } }

		public virtual RibbonControl Ribbon { get { return null; }}

        protected CmdQueue _commandsQueue = new CmdQueue();

        public void ExecuteLater(CmdQueue.VoidNoParamDelegate d)
        {
            _commandsQueue.ExecuteLater(d);
        }

		/// <summary>
		/// Need just for designer
		/// </summary>
		public VisualControl() {
			InitializeComponent();			
		}

		public VisualControl(core.controls.VisualControlInfo vci) : this()
		{
			VisualControlInfo = vci;
		}

		private void InitializeComponent()
		{
			if (components == null) components = new System.ComponentModel.Container();
			this.SuspendLayout();
			Enter += onEnter_event;
			// 
			// VisualControl
			// 
			this.Name = "VisualControl";
			this.ResumeLayout(false);
		}

        /// <summary>Starts.</summary>
        ///
        /// <author>Laco</author>
        ///
        /// <param name="startingKind">(Optional) The starting kind.</param>
        /// <param name="data">        (Optional) The data.</param>
        ///
        /// <returns>True if it succeeds, false if it fails.</returns>
		public virtual bool Start(
			core.controls.VisualControlStartingKind startingKind = core.controls.VisualControlStartingKind.StartingNew,
			object data = null
		) {
            return true;
		}


         

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

			if (disposing && (components != null))
			{
                _commandsQueue.Dispose();
                components.Dispose();
            }

			Enter -= onEnter_event;

			base.Dispose(disposing);
			SEventProxy.getInstance().fireEvent(new VisualControlActionEvent(this, new VisualControlActionEventData(this, VisualControlActionKind.Disposed)));
		}

		private void onEnter_event(object sender, EventArgs e) {
			SEventProxy.getInstance().fireEvent(new VisualControlActionEvent(this, new VisualControlActionEventData(this, VisualControlActionKind.Activated)));
		}

		#region IPersistentState

		protected object _State;

		protected virtual Type GetStateObjectType() { return typeof(object); }

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
			MethodInfo generic = method.MakeGenericMethod(GetStateObjectType());
			object[] pms = { GetPersistorKey(), _State };
			generic.Invoke(SPersistenceManager.getInstance(), pms);
		}

		public void LoadState()
		{
			MethodInfo method = typeof(SPersistenceManager).GetMethod("LoadObject");
			MethodInfo generic = method.MakeGenericMethod(GetStateObjectType());
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
