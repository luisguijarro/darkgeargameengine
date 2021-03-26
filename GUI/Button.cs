using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class Button : BaseObjects.Control
    {
        private bool FirsDraw;
        private float tx_x, tx_y; // Coordenadas de texto
        internal bool b_pulsed;
        private string s_text;
        private dgtk.Graphics.Color4 c4_textColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private bool b_textBorder;
        private float f_FontSize;
        private dgFont font;
        public Button() : this(22,22)
        {
            
        }

        public Button(uint width, uint height) : this(width,height, "Button")
        {
            
        }

        public Button(uint width, uint height, string text) : base(width,height)
        {
            this.FirsDraw = true;
            this.s_text = text;
            this.FontSize = 16;
            this.font = GuiTheme.DefaultGuiTheme.DefaultFont; //dge.G2D.Writer.Fonts["Linux Libertine"];
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;
                        
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Button_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.Button_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Button_FrameOffset;

            this.MouseUp += delegate { this.setPulsed(false); };

            this.setPulsed(false);
        }
        internal Button(uint width, uint height, string text, GraphicsUserInterface gui) : this(width,height, text)
        {
            this.gui = gui;
        }

        private void updateTextCoords(float fsize)
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = ((this.ui_width/2f) - (this.gui.Writer.MeasureString(this.font, this.s_text, fsize)/2f));
                    this.tx_y = (this.ui_height/2.1f) - (fsize/1.2f);
                }
            }
        }

        private void setPulsed(bool pulsed)
        {
            this.b_pulsed = pulsed;
            
            if (this.b_pulsed)
            {
                //this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Button_FrameOffset;
                this.updateTextCoords(this.f_FontSize-1f);
            }
            else
            {
                //this.tcFrameOffset = new float[]{0,0};
                this.updateTextCoords(this.f_FontSize);
            }            
        }

        protected override void MDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.setPulsed(true);
                base.MDown(sender, e);
            }
        }

        protected override void MUp(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MUp(sender, e);
            }
            this.setPulsed(false);
        }

        protected override void OnResize()
        {
            base.OnResize();
            this.updateTextCoords(this.f_FontSize);
        }
        
        private void DrawText()
        {
            float f_fs = this.b_pulsed ? this.f_FontSize-1 : f_FontSize;
            float px = this.tx_x; //(this.b_pulsed ? tx_x+1f : tx_x);
            float py = this.tx_y; //(this.b_pulsed ? tx_y+1f : tx_y);
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, f_fs, px, py, this.c4_textBorderColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, f_fs, px, py);
            }
        }

        internal override void Draw()
        {   
            if (this.FirsDraw) { this.updateTextCoords(this.f_FontSize); this.FirsDraw  =false; };
            if (this.gui != null)
            {
                //base.Draw();
                this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_pulsed ? this.tcFrameOffset : new float[]{0,0}, 0);

                //DrawText();
                //this.updateTextCoords();
                this.DrawIn(this.i_x+(int)this.MarginsFromTheEdge[0],this.i_y+(int)this.MarginsFromTheEdge[1]/*+(int)this.ui_height*/,(int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawText);
            }
        }
        
        #region PROPERTIES:
        
        public dgtk.Graphics.Color4 TextColor
        {
            set { this.c4_textColor = value;  this.updateTextCoords(this.f_FontSize);}
            get { return this.c4_textColor; }
        }

        public dgtk.Graphics.Color4 TextBorderColor
        {
            set { this.c4_textBorderColor = value; }
            get { return this.c4_textBorderColor; }
        }

        public bool TextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
        }

        public float FontSize 
        {
            set { this.f_FontSize = value; this.updateTextCoords(this.f_FontSize); }
            get { return this.f_FontSize; }
        }

        public dgFont Font
        {
            set { this.font = value; this.updateTextCoords(this.f_FontSize); }
            get { return this.font; }
        }

        public string Text
        {
            set { this.s_text = value; this.updateTextCoords(this.f_FontSize); }
            get { return this.s_text; }
        }

        #endregion
    }
}