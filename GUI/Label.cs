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
        private TextAlign ta_textAlign;
        public Label() : this(22,22, "Text")
        {
            
        }

        public Label(int width, int height, string text) : base(width, height)
        {
            this.FirsDraw = true;
            this.s_text = text;
            this.FontSize = 14;
            this.ta_textAlign = TextAlign.Left;
            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.c4_forecolor = GuiTheme.DefaultGuiTheme.DefaultTextColor;
            this.c4_bordercolor = GuiTheme.DefaultGuiTheme.DefaultTextBorderColor;
        }

        protected internal override void UpdateTheme()
        {
            this.c4_forecolor = this.gui.GuiTheme.DefaultTextColor;
            this.c4_bordercolor = this.gui.GuiTheme.DefaultTextBorderColor;
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
                this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_forecolor : this.gui.GuiTheme.DefaultDisableTextColor, this.s_text, f_fs, px, py, this.b_IsEnable ? this.c4_bordercolor : this.gui.GuiTheme.DefaultDisableTextColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_forecolor : this.gui.GuiTheme.DefaultDisableTextColor, this.s_text, f_fs, px, py);
            }
        }

        private void UpdateTextCoords()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    float txtsize = G2D.Writer.MeasureString(this.font, this.s_text, this.f_fs)[0];

                    switch(this.ta_textAlign)
                    {
                        case TextAlign.Left:
                            this.tx_x = this.MarginsFromTheEdge[0] * 2;
                            break;
                        case TextAlign.Center:
                            this.tx_x = (this.i_width/2f) - (txtsize/2f);
                            break;
                        case TextAlign.Right:
                            this.tx_x = this.i_width-(this.MarginsFromTheEdge[2]*2) - txtsize;
                            break;
                    }

                    this.tx_y = (this.i_height/2.1f) - (this.f_fs/1.2f);
                }
            }
        }


        protected override void pDraw()
        {   
            if (this.FirsDraw) { this.UpdateTextCoords(); this.FirsDraw  =false; };
            if (this.gui != null)
            {
                this.DrawIn(this.i_x+this.MarginsFromTheEdge[0],this.i_y+this.MarginsFromTheEdge[1],this.i_width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), this.i_height-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawText);
            }
        }
        
        protected override void pDrawID()
        {
            if (this.b_ShowMe)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.idColor, this.i_x, this.i_y, this.i_width, this.i_height, 0f);
            }
        }

        #region PROPERTIES:
        
        public dgtk.Graphics.Color4 TextColor
        {
            set { this.c4_forecolor = value;  this.UpdateTextCoords();}
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
            set { this.f_fs = value; this.UpdateTextCoords(); }
            get { return this.f_fs; }
        }

        public dgFont Font
        {
            set { this.font = value; this.UpdateTextCoords(); }
            get { return this.font; }
        }

        public string Text
        {
            set { this.s_text = value; this.UpdateTextCoords(); }
            get { return this.s_text; }
        }

        public TextAlign TextAlign
        {
            set { this.ta_textAlign = value; this.UpdateTextCoords(); }
            get { return this.ta_textAlign; }
        }


        #endregion

    }

}