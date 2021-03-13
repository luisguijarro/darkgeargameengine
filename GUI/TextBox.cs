using System;

using dge.G2D;
using dgtk;

namespace dge.GUI
{
    public class TextBox : BaseObjects.Control
    {
        private bool b_Focus;
        private bool b_editable;
        private string s_text;
        private int cursorPos;
        private float tx_x, tx_y; // Coordenadas de texto
        private dgtk.Graphics.Color4 c4_textColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private bool b_textBorder;
        private float f_FontSize;
        private dgFont font;
        private TextAlign ta_textAlign;
        public TextBox() : this(70, 20, "TextBox") 
        {

        }
        public TextBox(uint width, uint height, string text) : base(width, height)
        {
            this.b_editable = true;
            this.b_Focus = false;
            this.s_text = text;
            this.f_FontSize = 14;
            this.ta_textAlign = TextAlign.Left;
            this.font = dge.G2D.Writer.Fonts["Linux Libertine"];
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TextBox_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.TextBox_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f};

            this.KeyPulsed += Key_Pulsed;
            this.KeyCharReturned += CharReturned;
        }
        ~ TextBox()
        {
            this.KeyPulsed -= Key_Pulsed;
            this.KeyCharReturned -= CharReturned;
        }

        protected override void MDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            base.MDown(sender, e);
            if (Core2D.SelectedID == this.ui_id)
            {
                if (!this.b_Focus) 
                {
                    this.b_Focus = true; 
                    this.cursorPos = this.s_text.Length; 
                }
            }
            else
            {
                this.b_Focus = false;
            }
        }

        private void Key_Pulsed(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            if (this.b_Focus)
            {
                string txt1 = this.s_text.Substring(0, this.cursorPos);
                string txt2 = this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                switch (e.KeyStatus.KeyCode) // == dgtk.KeyCode.BackSpace)
                {
                    case dgtk.KeyCode.BackSpace:
                        if (txt1.Length>0)
                        {
                            txt1 =  txt1.Substring(0, txt1.Length-1);
                            //this.s_text = txt1 + txt2;
                            this.cursorPos--;
                        }
                        break;
                    case dgtk.KeyCode.LEFT:
                        if (this.cursorPos>0)
                        {
                            this.cursorPos--;
                        }
                        break;
                    case dgtk.KeyCode.RIGHT:
                        if (this.s_text.Length>this.cursorPos)
                        {
                            this.cursorPos++;
                        }
                        break;
                    case dgtk.KeyCode.Del:
                        if (txt2.Length>0)
                        {
                            txt2 = txt2.Substring(1, txt2.Length-1);
                        }
                        break;
                }
                this.s_text = txt1 + txt2;
                Console.WriteLine("KeyCode: "+(dgtk.KeyCode)e.KeyStatus.KeyCode);
            }
        }

        private void CharReturned(object sender, dgtk.dgtk_KeyBoardTextEventArgs e)
        {
            if (this.b_Focus)
            {
                string txt1 = this.s_text.Substring(0, this.cursorPos);
                string txt2 = this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                txt1 += e.Character;
                this.s_text = txt1+txt2;
                this.cursorPos++;
            }
        }

        private void updateTextCoords()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    float txtsize = this.gui.Writer.MeasureString(this.font, this.s_text, this.f_FontSize);
                    if (this.b_Focus)
                    {
                        string s_txt = this.s_text.Substring(0, this.cursorPos) + GuiTheme.DefaultGuiTheme.TextBox_CursorChar + this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);                        
                        txtsize = this.gui.Writer.MeasureString(this.font, this.s_text, this.f_FontSize);
                    }

                    switch(this.ta_textAlign)
                    {
                        case TextAlign.Left:
                            this.tx_x = (this.MarginsFromTheEdge[0] * 2);
                            break;
                        case TextAlign.Center:
                            this.tx_x = ((this.ui_width/2f) - (txtsize/2f));
                            break;
                        case TextAlign.Right:
                            this.tx_x = (this.ui_width-(this.MarginsFromTheEdge[2]*2)) - txtsize;
                            break;
                    }

                    this.tx_y = (this.ui_height/2.1f) - (this.f_FontSize/1.2f);
                }
            }
        }

        internal override void Draw()
        {
            base.Draw();
            DrawIn(this.i_x+this.MarginsFromTheEdge[0], this.i_y+this.MarginsFromTheEdge[1], (int)(this.ui_width-(this.MarginsFromTheEdge[0]+(this.MarginsFromTheEdge[2]))), (int)(this.ui_height-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[2])), WriteText);
        }

        private void WriteText()
        {
            if (this.b_Focus)
            {
                string s_txt = this.s_text.Substring(0, this.cursorPos) + GuiTheme.DefaultGuiTheme.TextBox_CursorChar + this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                this.gui.Writer.Write(this.font, this.c4_textColor, s_txt, this.f_FontSize, tx_x, tx_y, this.c4_textBorderColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, this.f_FontSize, tx_x, tx_y, this.c4_textBorderColor);
            }
        }

        internal override GraphicsUserInterface GUI
        {
            set { base.GUI = value; this.updateTextCoords(); }
            get { return base.GUI; }
        }

        public bool TextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
        }

        public float FontSize 
        {
            set { this.f_FontSize = value; this.updateTextCoords(); }
            get { return this.f_FontSize; }
        }

        public dgFont Font
        {
            set { this.font = value; this.updateTextCoords(); }
            get { return this.font; }
        }

        public string Text
        {
            set { this.s_text = value; this.updateTextCoords(); }
            get { return this.s_text; }
        }

        public dgtk.Graphics.Color4 TextColor
        {
            set { this.c4_textColor = value; }
            get { return this.c4_textColor; }
        }

        public dgtk.Graphics.Color4 TextBorderColor
        {
            set { this.c4_textBorderColor = value; }
            get { return this.c4_textBorderColor; }
        }

        public TextAlign TextAlign
        {
            set { this.ta_textAlign = value; this.updateTextCoords(); }
            get { return this.ta_textAlign; }
        }

        public bool Editable
        {
            set { this.b_editable = value; }
            get { return this.Editable; }
        }
    }
}