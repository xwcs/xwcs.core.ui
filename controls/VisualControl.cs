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

namespace xwcs.core.ui.controls
{
    public class VisualControl : DevExpress.XtraEditors.XtraUserControl, xwcs.core.controls.IVisualControl, xwcs.core.plgs.ISavable
    {
        //TODO
        //ISerializableComp podobne ako documentManager kuknut, implementnut na uroven VisualControl
        //2 metody beforeSave, afterLoad


        private xwcs.core.controls.VisualControlInfo _visualControlInfo;
        public xwcs.core.controls.VisualControlInfo VisualControlInfo { get { return _visualControlInfo; } set { _visualControlInfo = value; } }
        virtual public void SaveChanges() {; }

        public VisualControl()
        {
            Enter += VisualControl_Enter;
        } 

        public void VisualControl_Enter(object sender, EventArgs e)
        {
            DocumentActivatedRequest requestData = new DocumentActivatedRequest(this);
            DocumentActivatedEvent documentActivatedEvent = new DocumentActivatedEvent(this, requestData);
            SEventProxy.getInstance().fireEvent(documentActivatedEvent);
        }

		virtual public void initialize(object dataObject) {;}

    }
}
