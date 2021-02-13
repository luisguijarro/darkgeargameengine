using System;
using dgtk.Graphics;

namespace dge.GUI
{
    public class MouseWheelEventArgs : EventArgs
    {
        float f_delta;
        public MouseWheelEventArgs(float delta)
        {
            f_delta = delta;
        }
        public float Delta
        {
            get { return this.f_delta; }
        }
    }
}