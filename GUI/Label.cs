using System;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class Label : BaseObjects.Control
    {
        private string s_text;
        private dgFont font;
        private float f_fs;
        private bool b_textBorder;
        private Color4 c4_forecolor;
        private Color4 c4_bordercolor;
        private float tx_x, tx_y; // Coordenadas de texto
        private bool FirsDraw;
        public Label() : this(22,22, "Text")
        {
            
        }

        public Label(uint width, uint height, string text) : base(width, height)
        {
            this.FirsDraw = true;
            this.s_text = text;
            this.FontSize = 16;
            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.c4_forecolor = dgtk.Graphics.Color4.Black;
            this.c4_bordercolor = dgtk.Graphics.Color4.Black;
        }

        protected internal override void UpdateTheme()
        {
            //this.font = this.gui.gt_ActualGuiTheme.DefaultFont;
            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.DefaultFont.Name)
            {
                this.font = this.gui.GuiTheme.DefaultFont;
            }     
        }

        private void DrawText()
        {
            float px = this.tx_x;
            float py = this.tx_y;
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, (this.b_IsEnable) ? this.c4_forecolor : this.gui.GuiTheme.DefaultDisableTextColor, this.s_text, f_fs, px, py, (this.b_IsEnable) ? this.c4_bordercolor : this.gui.GuiTheme.DefaultDisableTextColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, (this.b_IsEnable) ? this.c4_forecolor : this.gui.GuiTheme.DefaultDisableTextColor, this.s_text, f_fs, px, py);
            }
        }

        private void UpdateTextCoords(float fsize)
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = ((this.ui_width/2f) - (dge.G2D.Writer.MeasureString(this.font, this.s_text, fsize)[0]/2f));
                    this.tx_y = (this.ui_height/2.1f) - (fsize/1.2f);
                }
            }
        }


        protected override void pDraw()
        {   
            if (this.FirsDraw) { this.UpdateTextCoords(this.f_fs); this.FirsDraw  =false; };
            if (this.gui != null)
            {
                //DrawText();this.f_fontSize
                this.DrawIn(this.i_x+(int)this.MarginsFromTheEdge[0],this.i_y+(int)this.MarginsFromTheEdge[1]/*+(int)this.ui_height*/,(int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawText);
            }
        }
        
        protected override void pDrawID()
        {
            if (this.b_ShowMe)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0f);
            }
        }

        #region PROPERTIES:
        
        public dgtk.Graphics.Color4 TextColor
        {
            set { this.c4_forecolor = value;  this.UpdateTextCoords(this.f_fs);}
            get { return this.c4_forecolor; }
        }

        public dgtk.Graphics.Color4 TextBorderColor
        {
            set { this.c4_bordercolor = value; }
            get { return this.c4_bordercolor; }
        }

        public bool TextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
        }

        public float FontSize 
        {
            set { this.f_fs = value; this.UpdateTextCoords(this.f_fs); }
            get { return this.f_fs; }
        }

        public dgFont Font
        {
            set { this.font = value; this.UpdateTextCoords(this.f_fs); }
            get { return this.font; }
        }

        public string Text
        {
            set { this.s_text = value; this.UpdateTextCoords(this.f_fs); }
            get { return this.s_text; }
        }

        #endregion

    }

}