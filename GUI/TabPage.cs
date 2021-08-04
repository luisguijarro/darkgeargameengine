using System;

namespace dge.GUI
{
    public class TabPage : BaseObjects.Control
    {
        string s_name;
        public TabPage(string name) : base()
        {
            this.s_name = name;
        }

        protected virtual void pDraw()
        {
            if (this.b_ShowMe)
            {
                // this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }
        }

        public string Name
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }
    }
}