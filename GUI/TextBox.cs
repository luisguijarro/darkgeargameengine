using System;

using dge.G2D;
using dgtk;

namespace dge.GUI
{
    public class TextBox : BaseObjects.Control
    {
        private bool FirsDraw;
        protected bool b_Focus;
        protected bool b_editable;
        protected string s_text;
        protected int cursorPos;
        protected float tx_x, tx_y; // Coordenadas de texto
        protected dgtk.Graphics.Color4 c4_textColor;
        protected dgtk.Graphics.Color4 c4_textBorderColor;
        protected dgtk.Graphics.Color4 c4_BackgroundColor;
        protected bool b_textBorder;
        protected float f_FontSize;
        protected dgFont font;
        protected TextAlign ta_textAlign;

        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler<KeyBoardKeysEventArgs> EnterPulsed;

        public TextBox() : this(70, 20, "TextBox") 
        {

        }
        public TextBox(int width, int height, string text) : base(width, height)
        {
            this.FirsDraw = true;
            this.b_editable = true;
            this.b_Focus = false;
            this.s_text = text;
            this.f_FontSize = GuiTheme.DefaultGuiTheme.Default_FontSize;
            this.ta_textAlign = TextAlign.Left;
            this.font = GuiTheme.DefaultGuiTheme.Default_Font;
            this.c4_textColor = GuiTheme.DefaultGuiTheme.Default_TextColor;
            this.c4_textBorderColor = GuiTheme.DefaultGuiTheme.Default_TextBorderColor;
            this.c4_BackgroundColor = GuiTheme.DefaultGuiTheme.TextBox_Default_BackgroundColor;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TextBox_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.TextBox_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f};

            this.KeyPulsed += Key_Pulsed;
            this.KeyCharReturned += CharReturned;
            this.TextChanged += delegate{};
            this.EnterPulsed += delegate{};
        }

        public void SetFocus()
        {
            this.b_Focus = true;
        }
        
        protected override void Dispose(bool disposing)
        {
            this.KeyPulsed -= Key_Pulsed;
            this.KeyCharReturned -= CharReturned;
        }

        protected override void OnResize()
        {
            base.OnResize();
            this.UpdateTextCoords();
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.TextBox_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.TextBox_Texcoords;
            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.Default_Font.Name)
            {
                this.font = this.gui.GuiTheme.Default_Font;
            }
            if (this.f_FontSize == GuiTheme.DefaultGuiTheme.Default_FontSize)
            {
                this.f_FontSize = this.gui.GuiTheme.Default_FontSize;
            }
            if (this.c4_textColor == GuiTheme.DefaultGuiTheme.Default_TextColor)
            {
                this.c4_textColor = this.gui.GuiTheme.Default_TextColor;
            }
            if (this.c4_BackgroundColor == GuiTheme.DefaultGuiTheme.TextBox_Default_BackgroundColor)
            {
                this.c4_BackgroundColor = this.gui.GuiTheme.TextBox_Default_BackgroundColor;
            }            
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);
            if ((Core2D.SelectedID == this.ui_id) && this.b_editable && this.b_IsEnable)
            {
                if (!this.b_Focus) 
                {
                    this.b_Focus = true; 
                    this.cursorPos = this.s_text.Length; 
                    this.UpdateTextCoords();
                }
            }
            else
            {
                if (this.b_Focus)
                {
                    this.b_Focus = false;
                    this.UpdateTextCoords();
                    this.TextChanged(this, new TextChangedEventArgs(this.s_text, false));
                }
            }
        }

        protected virtual void Key_Pulsed(object sender, KeyBoardKeysEventArgs e)
        {
            if (this.b_Focus && this.b_editable && this.b_IsEnable)
            {
                string txt1 = this.s_text.Substring(0, this.cursorPos);
                string txt2 = this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                bool changed = false;
                switch (e.KeyStatus.KeyCode) // == KeyCode.BackSpace)
                {
                    case KeyCode.BackSpace:
                        if (txt1.Length>0)
                        {
                            txt1 =  txt1.Substring(0, txt1.Length-1);
                            this.cursorPos--;
                            changed = true;
                        }
                        break;
                    case KeyCode.LEFT:
                        if (this.cursorPos>0)
                        {
                            this.cursorPos--;
                        }
                        break;
                    case KeyCode.RIGHT:
                        if (this.s_text.Length>this.cursorPos)
                        {
                            this.cursorPos++;
                        }
                        break;
                    case KeyCode.Del:
                        if (txt2.Length>0)
                        {
                            txt2 = txt2.Substring(1, txt2.Length-1);
                            changed = true;
                        }
                        break;
                    case KeyCode.Return:
                        if(txt1+txt2 != this.s_text) {changed = true; }
                        this.b_Focus = false;
                        this.EnterPulsed(this, new KeyBoardKeysEventArgs(new KeyBoard_Status(KeyCode.Return, PushRelease.Push)));
                        break;
                }
                this.s_text = txt1 + txt2;
                if (changed)
                {
                    this.TextChanged(this, new TextChangedEventArgs(this.s_text, false));
                }
                this.UpdateTextCoords();
            }
        }

        protected virtual void CharReturned(object sender, KeyBoardTextEventArgs e)
        {
            if (this.b_Focus && this.b_editable && this.b_IsEnable)
            {
                string txt1 = this.s_text.Substring(0, this.cursorPos);
                string txt2 = this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                txt1 += e.Character;
                this.s_text = txt1+txt2;
                this.cursorPos++;
                this.UpdateTextCoords();
                this.TextChanged(this, new TextChangedEventArgs(this.s_text, false));
            }
        }

        protected void UpdateTextCoords()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    if (dge.G2D.Writer.Fonts.ContainsKey(this.font.Name))
                    {
                        float[] txtsize = G2D.Writer.MeasureString(this.font, this.b_Focus ? this.s_text+"|" : this.s_text, this.f_FontSize, this.InnerSize.Width);
                        if (this.b_Focus)
                        {
                            if (this.cursorPos > this.s_text.Length)
                            {
                                this.cursorPos = this.s_text.Length;
                            }
                            //string s_txt = this.s_text.Substring(0, this.cursorPos) + GuiTheme.DefaultGuiTheme.TextBox_CursorChar + this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);                        
                            //txtsize = G2D.Writer.MeasureString(this.font, this.b_Focus ? this.s_text+"|" : this.s_text, this.f_FontSize)[0];
                        }

                        switch(this.ta_textAlign)
                        {
                            case TextAlign.Left:
                                this.tx_x = this.MarginLeft * 2;
                                break;
                            case TextAlign.Center:
                                this.tx_x = (this.InnerSize.Width/2f) - (txtsize[0]/2f);
                                break;
                            case TextAlign.Right:
                                this.tx_x = this.InnerSize.Width-(this.MarginRight + this.MarginLeft + txtsize[0]);
                                break;
                        }

                        this.tx_y = 0; //(this.InnerSize.Height/2f) - (txtsize[1]/2); //(this.f_FontSize/1.5f);
                    }
                }
            }
        }

        protected override void pDraw()
        {
            if (this.FirsDraw) { this.UpdateTextCoords(); this.FirsDraw = false; };
            this.gui.GuiDrawer.DrawGL(this.gui.gt_ActualGuiTheme.tbo_ThemeSltTBO.ID, 
                    this.c4_BackgroundColor, this.i_x, this.i_y, this.i_width, this.i_height, 
                    0f, this.MarginsFromTheEdge, this.Texcoords, new float[]{0f,0f}, 1);
            base.pDraw();
            
            DrawIn(this.i_x+this.MarginsFromTheEdge[0], this.i_y+this.MarginsFromTheEdge[1], this.i_width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), this.i_height-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[2]), WriteText);
        }
        
        private void WriteText()
        {
            if (this.b_textBorder)
            {
                if (this.b_Focus)
                {
                    string s_txt = this.s_text.Substring(0, this.cursorPos) + GuiTheme.DefaultGuiTheme.TextBox_CursorChar + this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                    this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_textColor : this.gui.GuiTheme.Default_DisableTextColor, s_txt, this.f_FontSize, tx_x, tx_y, this.b_IsEnable ? this.c4_textBorderColor : this.gui.GuiTheme.Default_DisableTextColor, this.InnerSize.Width);
                }
                else
                {
                    this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_textColor : this.gui.GuiTheme.Default_DisableTextColor, this.s_text, this.f_FontSize, tx_x, tx_y, this.b_IsEnable ? this.c4_textBorderColor : this.gui.GuiTheme.Default_DisableTextColor, this.InnerSize.Width);
                }
            }
            else
            {
                if (this.b_Focus)
                {
                    string s_txt = this.s_text.Substring(0, this.cursorPos) + GuiTheme.DefaultGuiTheme.TextBox_CursorChar + this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                    this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_textColor : this.gui.GuiTheme.Default_DisableTextColor, s_txt, this.f_FontSize, tx_x, tx_y, this.InnerSize.Width);
                }
                else
                {
                    this.gui.Writer.Write(this.font, this.b_IsEnable ? this.c4_textColor : this.gui.GuiTheme.Default_DisableTextColor, this.s_text, this.f_FontSize, tx_x, tx_y, this.InnerSize.Width);
                }
            }
        }

        #region PROPERTIES:

        public bool TextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
        }

        public float FontSize 
        {
            set { this.f_FontSize = value; this.UpdateTextCoords(); }
            get { return this.f_FontSize; }
        }

        public dgFont Font
        {
            set { this.font = value; this.UpdateTextCoords(); }
            get { return this.font; }
        }

        public string Text
        {
            set 
            { 
                this.s_text = value; 
                this.cursorPos = this.s_text.Length;
                this.UpdateTextCoords();
                this.TextChanged(this, new TextChangedEventArgs(value, true));
            }
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
        public dgtk.Graphics.Color4 BackgroundColor
        {
            set { this.c4_BackgroundColor = value; }
            get { return this.c4_BackgroundColor; }
        }

        public TextAlign TextAlign
        {
            set { this.ta_textAlign = value; this.UpdateTextCoords(); }
            get { return this.ta_textAlign; }
        }

        public bool Editable
        {
            set { this.b_editable = value; }
            get { return this.Editable; }
        }

        #endregion
    }
}