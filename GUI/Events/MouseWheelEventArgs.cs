using  System;

namespace dge.GUI
{
    public class MouseWheelEventArgs : EventArgs
    {
        private int i_posx;
        private int i_posy;
        float f_delta;
        public MouseWheelEventArgs(int PosX, int PosY, float delta)
        {
            this.i_posx = PosX;
            this.i_posy = PosY;
            this.f_delta = delta;
        }
        public float Delta
        {
            get { return this. f_delta; }
        }
        public int X
        {
            get { return this.i_posx; }
        }
        public int Y
        {
            get { return this.i_posy; }
        }
    }
}
