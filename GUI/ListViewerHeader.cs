using System;

using dge.G2D;
using dgtk;

namespace dge.GUI
{
    internal class ListViewerHeader : BaseObjects.Control
    {
        private int lastPosX;
        private string s_text;
        private dgtk.Graphics.Color4 c4_textColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private bool b_textBorder;
        private float f_FontSize;
        private dgFont font;
        internal bool b_pulsed;
        internal bool b_Dibider_pulsed;
        private bool FirsDraw;
        private float tx_x, tx_y; // Coordenadas de texto
        internal float[] ListViewer_Dibider_Texcoords; // n=4
        internal int ListViewer_Dibider_Width;
        public ListViewerHeader(string text)
        {
            this.s_text = text;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_Header_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Header_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.ListViewer_Header_FrameOffset;
            this.ListViewer_Dibider_Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Texcoords;
            this.ListViewer_Dibider_Width = GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Width;
        }

        #region PRIVATES:

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

        private void setPulsed(bool pulsed, int mouseX, int mouseY)
        {
            this.lastPosX = mouseX;
            this.b_pulsed = pulsed;
            this.b_Dibider_pulsed = false;
            
            if (this.b_pulsed)
            {
                if (this.MouseIn((int)(this.int_x+this.i_x+(this.Width-this.ListViewer_Dibider_Width)), this.int_y+this.i_y, this.ListViewer_Dibider_Width, (int)this.Height, mouseX, mouseY))
                {
                    this.b_pulsed = false;
                    this.b_Dibider_pulsed = true;
                    this.updateTextCoords(this.f_FontSize); // ¿Es necesario?
                }
                else
                {
                    this.updateTextCoords(this.f_FontSize-1f);
                }
            }
            else
            {
                //this.tcFrameOffset = new float[]{0,0};
                this.updateTextCoords(this.f_FontSize);
            }            
        }

        private bool MouseIn(int x, int y, int width, int height, int mouseX, int mouseY)
        {
            bool ret = false;
            if ((mouseX > x) && (mouseX < x+width))
            {
                if ((mouseY > y) && (mouseY < y+height))
                {
                    return true; 
                }
            }
            return ret;
        }

        protected override void MDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.setPulsed(true, e.X, e.Y);
                base.MDown(sender, e);
            }
        }

        protected override void MUp(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MUp(sender, e);
            }
            this.setPulsed(false, e.X, e.Y);
        }

        protected override void MMove(object sender, dgtk_MouseMoveEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MMove(sender, e);
                if (this.b_Dibider_pulsed)
                {
                    int i_diference = (e.X-this.lastPosX);
                    if ((this.ui_width + i_diference) > this.ListViewer_Dibider_Width+2)
                    {
                        this.ui_width = (uint)(this.ui_width +i_diference);
                    }
                }
                this.lastPosX = e.X;
            }
        }

        #endregion

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