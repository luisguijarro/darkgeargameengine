using System;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class Dialog : BaseObjects.BaseGuiSurface
    {
        private dgFont font;
        private string s_text;
        private float f_fontsize;
        private float tx_x, tx_y; // Coordenadas de texto
        private bool b_textBorder;
        private Color4 c4_forecolor;
        private Color4 c4_bordercolor;
        private int[] BodyMarginsFromTheEdge;
        private float[] BodyTexcoords;
        private int HeadHeight;
        //private bool b_answered;
        private DialogResult result;
        public event EventHandler<DialogResultEventArgs> Answered;
        public Dialog(GraphicsUserInterface gui) : this(300, 175, "Dialog", gui)
        {

        }

        public Dialog(int width, int height, string text, GraphicsUserInterface gui) : base (width, height)
        {
            this.s_text = text;
            this.f_fontsize = 14;
            this.font = GuiTheme.DefaultGuiTheme.Default_Font;
            this.c4_forecolor = GuiTheme.DefaultGuiTheme.Default_TextColor;
            this.c4_bordercolor = GuiTheme.DefaultGuiTheme.Default_TextBorderColor;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Dialog_Head_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.Dialog_Head_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f};
            this.BodyMarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Dialog_Body_MarginsFromTheEdge;
            this.BodyTexcoords = GuiTheme.DefaultGuiTheme.Dialog_Body_Texcoords;
            //this.CalculateHeadHeight();
            //this.CalculateInternalArea();
            this.Answered = delegate {};
            this.GUI = gui;
        }

        protected override void OnGuiUpdate()
        {            
            this.c4_forecolor = this.gui.GuiTheme.Default_TextColor;
            this.c4_bordercolor = this.gui.GuiTheme.Default_TextBorderColor;
            this.MarginsFromTheEdge = this.gui.GuiTheme.Dialog_Head_MarginsFromTheEdge;
            this.Texcoords = this.gui.GuiTheme.Dialog_Head_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f};
            this.BodyMarginsFromTheEdge = this.gui.GuiTheme.Dialog_Body_MarginsFromTheEdge;
            this.BodyTexcoords = this.gui.GuiTheme.Dialog_Body_Texcoords;
            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.Default_Font.Name)
            {
                this.font = this.gui.GuiTheme.Default_Font;
            } 
            this.CalculateHeadHeight();
            this.CalculateInternalArea();
        }

        protected override void OnResize()
        {
            //base.OnResize(); 
            this.CalculateInternalArea();
        }

        protected override void OnReposition()
        {
            //base.OnReposition();
            this.CalculateInternalArea();
        }

        private void CalculateInternalArea()
        {
            this.SetInternalDrawArea(/*this.i_x + */this.BodyMarginsFromTheEdge[0], /*this.i_y + */this.BodyMarginsFromTheEdge[2]+this.HeadHeight,
            this.Width-(this.BodyMarginsFromTheEdge[0]+this.BodyMarginsFromTheEdge[2]),
            this.Height-(this.BodyMarginsFromTheEdge[1]+this.BodyMarginsFromTheEdge[3]+this.HeadHeight));
        }

        private void CalculateHeadHeight()
        {
            this.HeadHeight = (int)Math.Ceiling(this.font.MaxCharacterHeight*(this.f_fontsize/this.font.MaxFontSize));
            this.HeadHeight += this.MarginTop+this.MarginBottom;
        }


        private void UpdateTextCoords()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = (this.i_width/2f) - (dge.G2D.Writer.MeasureString(this.font, this.s_text, this.f_fontsize)[0]/2f);
                    this.tx_y = (this.HeadHeight/2.1f) - (this.f_fontsize/1.2f);
                }
            }
        }

        private void DrawText()
        {
            float px = this.tx_x;
            float py = this.tx_y;
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_forecolor : this.gui.GuiTheme.Default_DisableTextColor, " "+this.s_text, this.f_fontsize, px, py, this.b_IsEnable ? this.c4_bordercolor : this.gui.GuiTheme.Default_DisableTextColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_forecolor : this.gui.GuiTheme.Default_DisableTextColor, " "+this.s_text, this.f_fontsize, px, py);
            }
        }

        protected void SetResult(DialogResult result)
        {
            this.result = result;

            if (this.gui.ActiveDialog==this)
            {
                this.gui.ActiveDialog=null;
            }
            this.Answered(this, new DialogResultEventArgs(this.result));
        }

        protected override void pDraw()
        {
            if (this.b_ShowMe)
            {
                // Draw Head
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.HeadHeight, 0, 
                this.MarginsFromTheEdge, 
                this.Texcoords, 
                this.tcFrameOffset, 
                0);

                // Draw Title.
                this.DrawIn(this.i_x+this.MarginsFromTheEdge[0],this.i_y+this.MarginsFromTheEdge[1],this.i_width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), this.HeadHeight-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawText);

                // Draw Body
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y+this.HeadHeight, this.i_width, this.i_height-this.HeadHeight, 0, 
                this.BodyMarginsFromTheEdge, 
                this.BodyTexcoords, 
                this.tcFrameOffset, 
                0);
            }
        }


        protected override void pDrawContent()
        {
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                DrawIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContent);
            } 
        }
        
        public virtual void ShowDialog()
        {
            if (this.gui.ActiveDialog==null)
            {
                this.X = (int)(this.gui.Width/2f-this.Width/2f);
                this.Y = (int)(this.gui.Height/2f-this.Height/2f);  
                this.gui.ActiveDialog=this;
            }
        }
        
        public float FontSize 
        {
            set { this.f_fontsize = value; CalculateHeadHeight(); this.UpdateTextCoords(); this.CalculateInternalArea(); }
            get { return this.f_fontsize; }
        }

        public dgFont Font
        {
            set { this.font = value; CalculateHeadHeight(); this.UpdateTextCoords(); this.CalculateInternalArea(); }
            get { return this.font; }
        }

        public string Text
        {
            set { this.s_text = value; this.UpdateTextCoords(); }
            get { return this.s_text; }
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
    }
}