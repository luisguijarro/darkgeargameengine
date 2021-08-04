using  System;

namespace dge.GUI
{
    public class MouseEnterLeaveEventArgs : EventArgs
    {
        private EnterLeave el_action;
        public MouseEnterLeaveEventArgs(EnterLeave action)
        {
            this.el_action = action;
        }
        public EnterLeave action
        {
            get { return this.el_action; }
        }
    }
}
