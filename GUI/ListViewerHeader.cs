using System;
using System.Reflection;

using dge.G2D;
using dgtk;
using dgtk.Graphics;

namespace dge.GUI
{
    
    internal class ListViewerHeader : Button
    {
        private float[] ListViewer_Dibider_Texcoords; // n=4
        private float[] ListViewer_ArrowUp_Texcoords; // n=4
        private float[] ListViewer_ArrowDown_Texcoords; // n=4
        private int[] ListViewer_Arrow_Size; // n=2
        private int ListViewer_Dibider_Width;
        private readonly string s_FieldToShow;
        private bool b_DibiderPulsed;
        //private readonly string s_name;
        internal bool b_Shorted;
        internal bool b_Ascending;
        public ListViewerHeader(string name, string fieldname)
        {

            /*this.s_name*/ this.Text = name;
            this.s_FieldToShow = fieldname;

            this.b_Shorted = false;
            this.b_Ascending = false;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_Header_MarginsFromTheEdge;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.ListViewer_Header_FrameOffset;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Header_Texcoords;
            this.ListViewer_Dibider_Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Texcoords;
            this.ListViewer_Dibider_Width = GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Width;
            this.ListViewer_ArrowUp_Texcoords  = GuiTheme.DefaultGuiTheme.ListViewer_ArrowUp_Texcoords;
            this.ListViewer_ArrowDown_Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_ArrowDown_Texcoords;
            this.ListViewer_Arrow_Size = GuiTheme.DefaultGuiTheme.ListViewer_Arrow_Size;

            this.s_text = name;
            float[] sf = dge.G2D.Writer.MeasureString(this.font, this.s_text+"  ", this.f_FontSize);
            this.i_width = (int)sf[0];
            this.i_height = (int)sf[1];
        }

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();
        }

        protected internal override void UpdateTheme()
        {
            //base.UpdateTheme();
            this.MarginsFromTheEdge = this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge;
            this.tcFrameOffset = this.gui.GuiTheme.ListViewer_Header_FrameOffset;
            this.Texcoords = this.gui.GuiTheme.ListViewer_Header_Texcoords;
            this.ListViewer_Dibider_Texcoords = this.gui.GuiTheme.ListViewer_Dibider_Texcoords;
            this.ListViewer_Dibider_Width = this.gui.GuiTheme.ListViewer_Dibider_Width;
            this.ListViewer_ArrowUp_Texcoords  = this.gui.GuiTheme.ListViewer_ArrowUp_Texcoords;
            this.ListViewer_ArrowDown_Texcoords = this.gui.GuiTheme.ListViewer_ArrowDown_Texcoords;
            this.ListViewer_Arrow_Size = this.gui.GuiTheme.ListViewer_Arrow_Size;
            
            float[] sf = dge.G2D.Writer.MeasureString(this.font, this.s_text+"  ", this.f_FontSize);
            this.i_width = (int)sf[0];
            this.i_height = (int)sf[1];
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            //base.OnMDown(sender, e);
            if (e.ID == this.ui_id)
            {
                if (this.IsMouseIn(e.X, e.Y, this.InnerSize.Width-(this.ListViewer_Dibider_Width*2), 0, this.InnerSize.Width, this.InnerSize.Height))
                {
                    // Esta sobre El dibisor y debe escalar.
                    this.b_DibiderPulsed = true;
                }
                else
                {
                    base.OnMDown(sender, e);
                }
            }
        }

        protected override void OnMMove(object sender, MouseMoveEventArgs e)
        {
            base.OnMMove(sender, e);
            if (this.b_DibiderPulsed)
            {
                this.Width += e.X-e.LastPosX;
                #if DEBUG
                    Console.WriteLine(this.Width);
                #endif
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMUp(sender, e);
            this.b_DibiderPulsed = false;
        }

        protected override void pDraw()
        {
            base.pDraw();
            //Dibujamos separador.
            this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, this.X+this.Width-this.ListViewer_Dibider_Width, this.Y, this.ListViewer_Dibider_Width, this.Height, 0f, this.ListViewer_Dibider_Texcoords[0], this.ListViewer_Dibider_Texcoords[1], this.ListViewer_Dibider_Texcoords[2], this.ListViewer_Dibider_Texcoords[3], false, false);
        }

        protected override void DrawContent()
        {
            base.DrawContent();

            if (b_Shorted)
            {
                if (b_Ascending)
                {
                    this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, this.Width-((this.ListViewer_Dibider_Width*2) + ListViewer_Arrow_Size[0]), (this.i_height/2) - (this.ListViewer_Arrow_Size[1]/2), this.ListViewer_Arrow_Size[0], this.ListViewer_Arrow_Size[1], 0f, this.ListViewer_ArrowUp_Texcoords[0], this.ListViewer_ArrowUp_Texcoords[1], this.ListViewer_ArrowUp_Texcoords[2], this.ListViewer_ArrowUp_Texcoords[3], false, false);
                }
                else
                {
                    this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, this.Width-((this.ListViewer_Dibider_Width*2) + ListViewer_Arrow_Size[0]), (this.i_height/2) - (this.ListViewer_Arrow_Size[1]/2), this.ListViewer_Arrow_Size[0], this.ListViewer_Arrow_Size[1], 0f, this.ListViewer_ArrowDown_Texcoords[0], this.ListViewer_ArrowDown_Texcoords[1], this.ListViewer_ArrowDown_Texcoords[2], this.ListViewer_ArrowDown_Texcoords[3], false, false);
                }
            }
        }

        public string FieldToShow
        {
            get { return this.s_FieldToShow; }
        }

        public string Name
        {
            get { return this.s_text; } //this.s_name; }
        }
    }
}