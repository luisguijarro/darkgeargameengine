using System;
using System.Collections.Generic;


namespace dge.GUI
{
    
    public class Menu : MenuItem
    {
        public Menu(string Text) : base(Text)
        {

        }
        
        protected override void UpdateSizeFromText()
        {
            //base.UpdateSizeFromText();
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    float tWidth = G2D.Writer.MeasureString(this.font, " "+this.s_text+" ", this.f_fontSize)[0]; //Obtenemos tamaño de texto.
                    this.ui_width = (uint)(tWidth+this.MarginLeft+this.MarginRight);
                    this.ui_height = (uint)((this.font.MaxCharacterHeight*(this.f_fontSize / this.font.f_MaxFontSize)) + this.MarginTop +  this.MarginBottom);
                    //this.tx_x = this.MarginLeft; //((this.ui_width/2f) - (tWidth/2f));
                    //this.tx_y = (this.ui_height/2.0f) - (this.f_fontSize/1.2f);
                    //this.OnReposition();
                }
            }
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
        
        protected override void OnGuiUpdate()
        {
            UpdateSizeFromText();
            UpdateTextCoords();
            OnReposition();
        }

        internal override void RepositionMenus()
        {
            /*this.UpdateSizeFromText();
            uint maxwidth = this.Width;
            for (int i=0;i<this.l_orderItems.Count;i++)
            {
                ((MenuItem)this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]]).Text = ((MenuItem)this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]]).Text;
                uint tmpwidth = this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]].Width;
                maxwidth = tmpwidth > maxwidth ? tmpwidth : maxwidth;
            }
            for (int i=0;i<this.l_orderItems.Count;i++)
            {
                MenuItem item = (MenuItem)this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]];
                item.X = (int)(this.X);
                item.Y = (int)((this.Height*(i+1)));
                item.Width = maxwidth;
            }*/
        }

    }
}