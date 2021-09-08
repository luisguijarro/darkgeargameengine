using System;
using System.Collections.Generic;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class NumberBox : TextBox
    {
        private int i_value;
        private int i_MinValue;
        private int i_MaxValue;

        float[] NumberBox_ButtonUpTexcoords;
        float[] NumberBox_ButtonDownTexcoords;
        int[] NumberBox_ButtonUpMarginsFromTheEdge;
        int[] NumberBox_ButtonDownMarginsFromTheEdge;
        float[] NumberBox_ButtonUpFrameOffset;
        float[] NumberBox_ButtonDownFrameOffset;
        int[] NumberBox_ButtonsSize;

        bool b_UpPulsed;
        bool b_DownPulsed;

        uint ui_id_Up;
        uint ui_id_Down;

        Color4 C4_id_up;
        Color4 C4_id_down;

        List<int> aceptedChars = new List<int>(){'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-'};

        public event EventHandler<IntValueChangedEventArgs> ValueChanged;
        
        public NumberBox() : this(70, 20, 0) 
        {

        }
        public NumberBox(int width, int height, int value) : base(width, height, value.ToString())
        {
            this.i_MinValue = 0;
            this.i_MaxValue = int.MaxValue;
            this.ApplyMinMaxValues();

            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.f_FontSize = 14;
            this.b_textBorder = false;

            this.Text = value.ToString();
            this.i_value = value;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TextBox_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.TextBox_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f};
            
            this.NumberBox_ButtonUpMarginsFromTheEdge = GuiTheme.DefaultGuiTheme.NumberBox_ButtonUpMarginsFromTheEdge;
            this.NumberBox_ButtonUpTexcoords = GuiTheme.DefaultGuiTheme.NumberBox_ButtonUpTexcoords;
            this.NumberBox_ButtonUpFrameOffset = GuiTheme.DefaultGuiTheme.NumberBox_ButtonUpFrameOffset;

            this.NumberBox_ButtonDownMarginsFromTheEdge = GuiTheme.DefaultGuiTheme.NumberBox_ButtonDownMarginsFromTheEdge;
            this.NumberBox_ButtonDownTexcoords = GuiTheme.DefaultGuiTheme.NumberBox_ButtonDownTexcoords;
            this.NumberBox_ButtonDownFrameOffset = GuiTheme.DefaultGuiTheme.NumberBox_ButtonDownFrameOffset;

            this.NumberBox_ButtonsSize = GuiTheme.DefaultGuiTheme.NumberBox_ButtonsSize;

            this.ui_id_Up = dge.Core2D.GetID();
            this.ui_id_Down = dge.Core2D.GetID();

            this.C4_id_up = new Color4(dge.Core2D.DeUIntAByte4(this.ui_id_Up));
            this.C4_id_down = new Color4(dge.Core2D.DeUIntAByte4(this.ui_id_Down));

            this.ValueChanged += delegate{};
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                dge.Core2D.ReleaseID(this.ui_id_Up);
                dge.Core2D.ReleaseID(this.ui_id_Down);
            }
        }

        protected internal override void UpdateTheme()
        {
            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.DefaultFont.Name)
            {
                this.font = this.gui.GuiTheme.DefaultFont;
            }    

            this.MarginsFromTheEdge = this.gui.GuiTheme.TextBox_MarginsFromTheEdge;            
            this.Texcoords = this.gui.GuiTheme.TextBox_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f};
            
            this.NumberBox_ButtonUpMarginsFromTheEdge = this.gui.GuiTheme.NumberBox_ButtonUpMarginsFromTheEdge;
            this.NumberBox_ButtonUpTexcoords = this.gui.GuiTheme.NumberBox_ButtonUpTexcoords;
            this.NumberBox_ButtonUpFrameOffset = this.gui.GuiTheme.NumberBox_ButtonUpFrameOffset;

            this.NumberBox_ButtonDownMarginsFromTheEdge = this.gui.GuiTheme.NumberBox_ButtonDownMarginsFromTheEdge;
            this.NumberBox_ButtonDownTexcoords = this.gui.GuiTheme.NumberBox_ButtonDownTexcoords;
            this.NumberBox_ButtonDownFrameOffset = this.gui.GuiTheme.NumberBox_ButtonDownFrameOffset;

            this.NumberBox_ButtonsSize = this.gui.GuiTheme.NumberBox_ButtonsSize;
        }

        protected override void Key_Pulsed(object sender, KeyBoardKeysEventArgs e)
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
                        changed = true;
                        this.b_Focus = false;
                        break;
                }

                if (int.TryParse(txt1 + txt2, out this.i_value))
                {
                    // OK
                    this.s_text = txt1 + txt2;
                    if (changed)
                    {
                        if (!this.ApplyMinMaxValues())
                        {
                            this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, false));
                        }
                    }
                }
                else
                {
                    if ((txt1 + txt2).Length == 0)
                    {
                        this.s_text = "";
                    }
                }
            }
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            //base.OnMDown(sender, e);
            if ((Core2D.SelectedID == this.ui_id) && this.b_editable && this.b_IsEnable)
            {
                if (!this.b_Focus) 
                {
                    this.b_Focus = true; 
                    this.cursorPos = this.s_text.Length; 
                }
            }
            else
            {
                if (this.b_Focus)
                {
                    this.b_Focus = false;
                    this.s_text = this.i_value.ToString();
                    if (!this.ApplyMinMaxValues())
                    {
                        this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, false));
                    }
                }
                this.b_UpPulsed = false;
                this.b_DownPulsed = false;   
                if (Core2D.SelectedID == this.ui_id_Up)
                {
                    this.b_UpPulsed = true;
                    this.i_value++;
                    if (!this.ApplyMinMaxValues())
                    {
                        this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, false));
                    }
                    this.s_text = this.i_value.ToString();
                }
                if (Core2D.SelectedID == this.ui_id_Down)
                {
                    this.b_DownPulsed = true;   
                    this.i_value--;     
                    if (!this.ApplyMinMaxValues())
                    {
                        this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, false));
                    }
                    this.s_text = this.i_value.ToString();        
                }
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMUp(sender, e);
            this.b_UpPulsed = false;
            this.b_DownPulsed = false;
        }
        protected override void CharReturned(object sender, KeyBoardTextEventArgs e)
        {
            if (this.b_Focus && this.b_editable && this.b_IsEnable)
            {
                string txt1 = this.s_text.Substring(0, this.cursorPos);
                string txt2 = this.s_text.Substring(this.cursorPos, this.s_text.Length-this.cursorPos);
                if (this.aceptedChars.Contains(e.Character)) // CONTROL NUMERICO
                {
                    if (int.TryParse(txt1+e.Character+txt2, out this.i_value)) // CONTROL LÓGICO
                    {
                        txt1 += e.Character;
                        this.s_text = txt1+txt2;
                        this.cursorPos++;
                        if (!this.ApplyMinMaxValues())
                        {
                            this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, false));
                        }
                    }
                }
            }
            this.b_UpPulsed = false;
            this.b_DownPulsed = false;  
        }

        protected override void InputSizeAlter(int width, int height)
        {
            //base.InputSizeAlter(width, height);
            this.i_width = width-this.NumberBox_ButtonsSize[0];
            this.i_height = height;
        }

        protected override int[] OutputSizeAlter(int width, int height)
        {
            return new int[] { this.i_width+this.NumberBox_ButtonsSize[0], this.i_height} ;
        }

        protected override void pDraw()
        {
            base.pDraw();
            // Dibujar botones:
            this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, dgtk.Graphics.Color4.White, this.X+this.i_width, this.Y, this.NumberBox_ButtonsSize[0], this.NumberBox_ButtonsSize[1], 0f, this.NumberBox_ButtonUpMarginsFromTheEdge, this.NumberBox_ButtonUpTexcoords, this.b_UpPulsed ? this.NumberBox_ButtonUpFrameOffset : new float[]{0f, 0f}, 0);
            this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, dgtk.Graphics.Color4.White, this.X+this.i_width, this.Y+this.NumberBox_ButtonsSize[1], this.NumberBox_ButtonsSize[0], this.NumberBox_ButtonsSize[1], 0f, this.NumberBox_ButtonDownMarginsFromTheEdge, this.NumberBox_ButtonDownTexcoords, this.b_DownPulsed ? this.NumberBox_ButtonDownFrameOffset : new float[]{0f, 0f}, 0);
        }

        protected override void pDrawID()
        {
            base.pDrawID();
            // Dibujar botones:
            dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeTBO.ID, this.C4_id_up, this.X+this.i_width, this.Y, this.NumberBox_ButtonsSize[0], this.NumberBox_ButtonsSize[1], 0f, this.NumberBox_ButtonUpMarginsFromTheEdge, this.NumberBox_ButtonUpTexcoords, this.b_UpPulsed ? this.NumberBox_ButtonDownFrameOffset : new float[]{0f, 0f}, 1);
            dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeTBO.ID, this.C4_id_down, this.X+this.i_width, this.Y+this.NumberBox_ButtonsSize[1], this.NumberBox_ButtonsSize[0], this.NumberBox_ButtonsSize[1], 0f, this.NumberBox_ButtonDownMarginsFromTheEdge, this.NumberBox_ButtonDownTexcoords, this.b_DownPulsed ? this.NumberBox_ButtonDownFrameOffset : new float[]{0f, 0f}, 1);
        }

        private bool ApplyMinMaxValues()
        {
            if (this.i_value < this.i_MinValue)
            {
                this.i_value = this.i_MinValue;
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, true));
                return true;
            }
            if (this.i_value > this.i_MaxValue)
            {
                this.i_value = this.i_MaxValue;
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, true));
                return true;
            }
            this.s_text = this.i_value.ToString();
            return false;
        }

        #region PROPERTIES:
        
        public int Value
        {
            set 
            { 
                if ((this.i_MinValue < value)  && (value < this.i_MaxValue)) 
                { 
                    this.i_value = value; 
                    this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, true));
                }
                else
                {
                    this.i_value = value; 
                    this.ApplyMinMaxValues();
                }
            }
            get { return this.i_value; }
        }

        public int MinValue
        {
            set 
            { 
                this.i_MinValue = value; 
                this.ApplyMinMaxValues(); 
            }
            get { return this.i_MinValue; }
        }

        public int MaxValue
        {
            set 
            { 
                this.i_MaxValue = value; 
                this.ApplyMinMaxValues(); 
            }
            get { return this.i_MaxValue; }
        }
        #endregion
    }
}