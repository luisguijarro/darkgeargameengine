using System;
using System.Collections.Generic;

namespace dge
{
    public class Scene2D : Scene
    {
        protected int i_width;
        protected int i_height;
        protected G2D.Drawer drawer2D;
        protected G2D.Writer writer2D;
        protected dgtk.Graphics.Color4 c4_BackGroundColor;

        public Scene2D(int width, int height, string name) : base(name)
        {
            this.i_width = width;
            this.i_height = height;
            c4_BackGroundColor = dgtk.Graphics.Color4.Gray;
        }
        internal override void SetParentWindow(dgWindow win)
        {
            base.SetParentWindow(win);
            this.drawer2D = this.parentWin.Drawer2D;
            this.writer2D = this.parentWin.Writer2D;
        }
    }
}