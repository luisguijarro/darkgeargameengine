using System;
using dgtk.Graphics;

namespace dge.GUI
{
    public class MouseMoveEventArgs : EventArgs
    {
        private int i_x, i_y, i_ax, i_ay;
        public MouseMoveEventArgs(int x, int y, int absoluteX, int absoluteY)
        {
            this.i_x = x;
            this.i_y = y;
            this.i_ax = absoluteX;
            this.i_ay = absoluteY;
        }
        public int X
        {
            get { return this.i_x; }
        }
        public int Y
        {
            get { return this.i_y; }
        }
        public int AbsoluteX
        {
            get { return this.i_ax; }
        }
        public int AbsoluteY
        {
            get { return this.i_ax; }
        }
    }
}