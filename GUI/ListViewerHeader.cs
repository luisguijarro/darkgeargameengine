using System;

using dge.G2D;
using dgtk;
using dgtk.Graphics;

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
        internal string s_Member;
        public ListViewerHeader(string text, string member) : base(50, 22)
        {
            this.s_text = text;
            this.s_Member = member;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_Header_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Header_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.ListViewer_Header_FrameOffset;
            this.ListViewer_Dibider_Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Texcoords;
            this.ListViewer_Dibider_Width = GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Width;

            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;
            this.f_FontSize = 14;
            this.updateTextCoords(14);
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ListViewer_Header_MarginsFromTheEdge;
            this.Texcoords = this.gui.gt_ActualGuiTheme.ListViewer_Header_Texcoords;
            this.tcFrameOffset = this.gui.gt_ActualGuiTheme.ListViewer_Header_FrameOffset;
            this.ListViewer_Dibider_Texcoords = this.gui.gt_ActualGuiTheme.ListViewer_Dibider_Texcoords;
            this.ListViewer_Dibider_Width = this.gui.gt_ActualGuiTheme.ListViewer_Dibider_Width;
            this.font = this.gui.gt_ActualGuiTheme.DefaultFont;
        }

        #region PRIVATES:

        private void updateTextCoords(float fsize)
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    this.tx_x = ((this.ui_width/2f) - (G2D.Writer.MeasureString(this.font, this.s_text, fsize)[0]/2f));
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
                if (this.MouseIn((int)(this.int_x+this.i_x+(this.Width-(this.ListViewer_Dibider_Width*2))), this.int_y+this.i_y, this.ListViewer_Dibider_Width*2, (int)this.Height, mouseX, mouseY))
                {
                    this.b_pulsed = false;
                    this.b_Dibider_pulsed = true;
                    //this.updateTextCoords(this.f_FontSize); // ¿Es necesario?
                }
                else
                {
                    this.updateTextCoords(this.f_FontSize-1f);
                }
            }
            else
            {
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

        protected override void MDown(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.setPulsed(true, e.X, e.Y);
                base.MDown(sender, e);
            }
        }

        protected override void MUp(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MUp(sender, e);
            }
            this.setPulsed(false, e.X, e.Y);
        }

        protected override void MMove(object sender, MouseMoveEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MMove(sender, e);
                if (this.b_Dibider_pulsed)
                {
                    int i_diference = (e.X-this.lastPosX);
                    if ((this.ui_width + i_diference) > this.ListViewer_Dibider_Width+2)
                    {
                        this.Width = (uint)(this.ui_width +i_diference); // Se llama a la propiedad para lanzar evento Resize
                    }
                    this.lastPosX = e.X;
                }
            }
        }

        #endregion

        #region VIRTUAL/OVERRIDE:

        protected override void pDraw()
        {   
            //if (this.gui != null)
            //{
                if (this.FirsDraw) { this.updateTextCoords(this.f_FontSize); this.FirsDraw  =false; };
                //base.Draw();
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_pulsed ? this.tcFrameOffset : new float[]{0,0}, 0);

                //DrawText();
                this.DrawIn(this.i_x+(int)this.MarginsFromTheEdge[0],this.i_y+(int)this.MarginsFromTheEdge[1],(int)this.ui_width-(int)this.MarginsFromTheEdge[0], (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawText);

                this.gui.Drawer.Draw(this.gui.GuiTheme.ThemeTBO.ID, this.i_x+(int)(this.Width-this.ListViewer_Dibider_Width), 0, (uint)(this.ListViewer_Dibider_Width), this.Height, 0f, this.ListViewer_Dibider_Texcoords[0], this.ListViewer_Dibider_Texcoords[1], this.ListViewer_Dibider_Texcoords[2], this.ListViewer_Dibider_Texcoords[3]);
            //}
        }

        internal override void DrawID()
        {
            base.DrawID();
            //dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeTBO.ID, this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, new float[]{0,0}, 1);
            //this.gui.DrawIds.DrawID(this.gui.GuiTheme.ThemeTBO.ID, this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_pulsed ? this.tcFrameOffset : new float[]{0,0}, 1);
        }

        private void DrawText()
        {
            float f_fs = this.b_pulsed ? this.f_FontSize-1 : f_FontSize;
            float px = this.tx_x;
            float py = this.tx_y;
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, f_fs, px, py, this.c4_textBorderColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, f_fs, px, py);
            }
            
        }

        protected override void OnResize()
        {
            this.updateTextCoords(this.f_FontSize);
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