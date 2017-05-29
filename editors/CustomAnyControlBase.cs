using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.CustomEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xwcs.core.ui.editors
{
    public class CustomAnyControlBase : XtraUserControl, IAnyControlEdit
    {
        private int _fakeEditValue = 0;

        #region IAnyControlEdit

        //public event EventHandler EditValueChanged;
        private readonly evt.WeakEventSource<EventArgs> _wes_EditValueChanged = new evt.WeakEventSource<EventArgs>();
        public event EventHandler EditValueChanged
        {
            add { _wes_EditValueChanged.Subscribe(value); }
            remove { _wes_EditValueChanged.Unsubscribe(value); }
        }
        private void OnEditValueChanged()
        {
            _wes_EditValueChanged.Raise(this, new EventArgs());
        }

        public object EditValue
        {
            get
            {
                return _fakeEditValue;
            }

            set
            {
                return;
            }
        }

        public bool SupportsDraw
        {
            get
            {
                return false;
            }
        }

        public bool AllowBorder
        {
            get
            {
                return false;
            }
        }

        public bool AllowBitmapCache
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// control need return size so layout know how to render it
        /// this will be forced by layout item size if bigger
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public Size CalcSize(Graphics g)
        {
            return this.Size;
        }

        public void SetupAsDrawControl() { }
        public void SetupAsEditControl() { }
        public string GetDisplayText(object EditValue)
        {
            return RepositoryItemAnyControl.GetBasicDisplayText(EditValue);
        }
        public bool IsNeededKey(KeyEventArgs e) { return false; }
        public bool AllowClick(Point point) { return true; }

        public void Draw(GraphicsCache cache, AnyControlEditViewInfo viewInfo)
        {
            //throw new NotImplementedException();
        }
        #endregion

        private bool _invaidatingValue = false;
        protected void InvalidateValue(bool arrivedFromSize = false)
        {
            if (_invaidatingValue) return;
            _invaidatingValue = true;
            
            _fakeEditValue += 1;
            OnEditValueChanged();

            _invaidatingValue = false;
        }
        protected override void SetBoundsCore(
            int x,
            int y,
            int width,
            int height,
            BoundsSpecified specified
        )
        {
            base.SetBoundsCore(x, y, width, height, specified);
            InvalidateValue(true);
        }
    }
}
