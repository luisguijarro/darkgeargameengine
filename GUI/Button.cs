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
        private TextureBufferObject tbo_image;
        public Button() : this(22,22)
        {
            
        }

        public Button(int width, int height) : this(width,height, "Button")
        {
            
        }

        public Button(int width, int height, string text) : base(width,height)
        {
            this.FirsDraw = true;
            this.s_text = text;
            this.FontSize = 16;
            this.font = GuiTheme.DefaultGuiTheme.Default_Font; //dge.G2D.Writer.Fonts["Linux Libertine"];
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;
                        
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Button_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.Button_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Button_FrameOffset;

            this.tbo_image = TextureBufferObject.Null;

            this.MouseUp += delegate { this.setPulsed(false); };

            this.setPulsed(false);
            //Console.WriteLine("forzar compilación");
        }
        internal Button(int width, int height, string text, GraphicsUserInterface gui) : this(width,height, text)
        {
            this.gui = gui;
            this.UpdateTheme();
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.Button_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.Button_Texcoords;
            this.tcFrameOffset = this.gui.gt_ActualGuiTheme.Button_FrameOffset;
            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.Default_Font.Name)
            {
                this.font = this.gui.GuiTheme.Default_Font;
            }
            base.UpdateTheme(); // Fuerza el establecimiento del area de dibujo interna.
        }

        private void updateTextCoords(float fsize)
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = (this.i_width/2f) - (dge.G2D.Writer.MeasureString(this.font, this.s_text, fsize)[0]/2f);
                    this.tx_y = (this.i_height/2.1f) - (fsize/1.2f);
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

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.setPulsed(true);
                base.OnMDown(sender, e);
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.OnMUp(sender, e);
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
            float px = this.tx_x;
            float py = this.tx_y;
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, (this.b_IsEnable) ? this.c4_textColor : this.gui.GuiTheme.Default_DisableTextColor, this.s_text, f_fs, px, py, (this.b_IsEnable) ? this.c4_textBorderColor : this.gui.GuiTheme.Default_DisableTextColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, (this.b_IsEnable) ? this.c4_textColor: this.gui.GuiTheme.Default_DisableTextColor, this.s_text, f_fs, px, py);
            }
        }

        protected override void pDraw()
        {   
            if (this.FirsDraw) { this.updateTextCoords(this.f_FontSize); this.FirsDraw  =false; };
            if (this.gui != null)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_pulsed ? this.tcFrameOffset : new float[]{0,0}, 0);

                if (tbo_image.ID>0)
                {
                    this.gui.Drawer.Draw(this.tbo_image.ui_ID, this.X+this.MarginLeft, this.Y+this.MarginTop, this.i_width-(this.MarginLeft+this.MarginRight), this.i_height-(this.MarginTop+this.MarginBottom), 0f, 0f, 0f, 1f, 1f);
                }
                else
                {
                    this.DrawIn(this.i_x+this.MarginsFromTheEdge[0],this.i_y+this.MarginsFromTheEdge[1],this.i_width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), this.i_height-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawText);
                }
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

        public TextureBufferObject Image
        {
            set { this.tbo_image = value; }
            get { return this.tbo_image; }
        }

        #endregion
    }
}