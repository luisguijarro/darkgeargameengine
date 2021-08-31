using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class Dialog : BaseObjects.BaseGuiSurface
    {
        private dgFont font;
        public Dialog() : this(300, 175)
        {

        }

        public Dialog(int width, int height) : base (width, height)
        {
            this.b_visible = false;
        }

        protected virtual void pDraw()
        {
            if (this.b_ShowMe)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);

                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }
        }
        
        public override bool Visible
        {
            set
            {
                base.b_visible = value;
                if (this.b_visible)
                {
                    if (this.gui.ActiveDialog==null)
                    {
                        this.gui.ActiveDialog=this;
                    }
                }
                else
                {
                    if (this.gui.ActiveDialog==this)
                    {
                        this.gui.ActiveDialog = null;
                    }
                }
            }
        }
    }
}