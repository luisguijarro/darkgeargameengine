using  System;

namespace dge.GUI
{
    public class MouseButtonEventArgs : EventArgs
    {
        private uint i_id;
        private int i_posx;
        private int i_posy;
        private MouseButtons mb_buttons;
        private PushRelease pr_action;
        public MouseButtonEventArgs(int PosX, int PosY, MouseButtons buttons, PushRelease action, uint id)
        {
            this.i_posx = PosX;
            this.i_posy = PosY;
            this.mb_buttons = buttons;
            this.pr_action = action;
            this.i_id = id;
        }
        public int X
        {
            get { return this.i_posx; }
        }
        public int Y
        {
            get { return this.i_posy; }
        }
        public MouseButtons Buttons
        {
            get { return this.mb_buttons; }
        }
        public PushRelease Action
        {
            get { return this.pr_action; }
        }
        public uint ID 
        {
            get { return this.i_id; }
        }
    }
}
