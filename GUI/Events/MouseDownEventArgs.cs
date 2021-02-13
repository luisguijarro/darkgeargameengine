using System;
using dgtk.Graphics;

namespace dge.GUI
{
    public class MouseDownEventArgs : EventArgs
    {
        dgtk.MouseButtons mb_button;
        public MouseDownEventArgs(dgtk.MouseButtons button)
        {
            mb_button = button;
        }
        public dgtk.MouseButtons Button
        {
            get { return this.mb_button; }
        }
    }
}