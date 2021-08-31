using System;
using System.Collections.Generic;


namespace dge.GUI
{
    
    public class Menu : MenuItem
    {
        public Menu(string Text) : base(Text)
        {
            this.IsMain = true;;
        }

        protected override void UpdateTextCoords()
        {
            //base.UpdateTextCoords();
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = this.MarginLeft;
                    this.tx_y = (this.i_height/2.0f) - (this.f_fontSize/1.2f);
                }
            }
            this.OnReposition();
        }

        protected override void CloseParents()
        {
        }

        internal override void RepositionMenus()
        {
            this.UpdateSizeFromText();
            this.maxwidth = this.Width;
            this.maxheight=0;
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                ((MenuItem)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]]).UpdateSizeFromText();
                int tmpwidth = this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].Width;
                this.maxwidth = tmpwidth > this.maxwidth ? tmpwidth : this.maxwidth;
            }
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                MenuItem item = (MenuItem)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]];
                item.X = this.X+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[0];
                item.Y = (this.Height*(i+1))+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[1];
                item.Width = this.maxwidth;
                this.maxheight+=item.Height;
                item.RepositionMenus();
            }
        }


        protected override void pDrawBorder()
        {
            this.gui.gd_GuiDrawer.DrawGL(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO.ID, dgtk.Graphics.Color4.White, this.X, this.Y+this.Height, maxwidth+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[0]+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[2], maxheight+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[1]+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[3], 0f, this.gui.gt_ActualGuiTheme.Menu_BordersWidths, this.gui.gt_ActualGuiTheme.Menu_Border_Texcoords, new float[]{0f,0f}, 0);
        }


        protected override bool Opened
        {
            set
            {
                this.b_opened = value;
                if (this.b_opened)
                {
                    this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Menu_FrameOffset;
                }
                else
                {
                    this.tcFrameOffset = new float[]{0f,0f};
                }
            }
            get { return this.b_opened; }
        }
    }
}