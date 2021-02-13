using System;
using dgtk.Graphics;

namespace dge.GUI
{
    public class MouseUpEventArgs : EventArgs
    {
        dgtk.MouseButtons mb_button;
        public MouseUpEventArgs(dgtk.MouseButtons button)
        {
            mb_button = button;
        }
        public dgtk.MouseButtons Button
        {
            get { return this.mb_button; }
        }
    }
}