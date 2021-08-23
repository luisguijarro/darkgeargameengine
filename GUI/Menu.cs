using System;
using System.Collections.Generic;


namespace dge.GUI
{
    
    public class Menu : MenuItem
    {
        public Menu(string Text) : base(Text)
        {

        }

        protected override void UpdateTextCoords()
        {
            //base.UpdateTextCoords();
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = this.MarginLeft;
                    this.tx_y = (this.ui_height/2.0f) - (this.f_fontSize/1.2f);
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
            uint maxwidth = this.Width;
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                //((MenuItem)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]]).Text = ((MenuItem)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]]).Text;
                ((MenuItem)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]]).UpdateSizeFromText();
                uint tmpwidth = this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].Width;
                maxwidth = tmpwidth > maxwidth ? tmpwidth : maxwidth;
            }
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                MenuItem item = (MenuItem)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]];
                item.X = (int)(this.X);
                item.Y = (int)((this.Height*(i+1)));
                item.Width = maxwidth;
                item.RepositionMenus();
            }
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