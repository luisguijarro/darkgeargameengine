using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{    
    public class InteractiveProgressBar : ProgressBar
    {
        private bool b_IsPulsed;
        public InteractiveProgressBar() : base (100,22)
        {
            this.b_IsPulsed = false;
            this.Texcoords = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Texcoords;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_MarginsFromTheEdge;
            this.FillingTexCoords = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords;
        }

        protected internal override void UpdateTheme()
        {
            this.Texcoords = this.gui.gt_ActualGuiTheme.InteractiveProgressBar_Hor_Texcoords;
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.InteractiveProgressBar_Hor_MarginsFromTheEdge;
            this.FillingTexCoords = this.gui.gt_ActualGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords;
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);
            if (e.ID == this.ID)
            {
                this.b_IsPulsed = true;
                int xpos = e.X - (this.int_x+this.MarginLeft);
                this.i_value = (int)((float)((float)(this.MaxValue-this.MinValue)/this.InnerSize.Width) * xpos);
                this.internal_Set_Value(this.i_value);
            }
        }

        protected override void OnMMove(object sender, MouseMoveEventArgs e)
        {
            base.OnMMove(sender, e);
            if (this.b_IsPulsed)
            {
                int xpos = e.X - (this.int_x+this.MarginLeft);
                this.i_value = (int)((float)((float)(this.MaxValue-this.MinValue)/this.InnerSize.Width) * xpos);
                this.internal_Set_Value(this.i_value);
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMUp(sender, e);
            this.b_IsPulsed = false;
        }


        protected override void UpdateProgres()
        {
            int valuerange = this.MaxValue - this.MinValue;
            if (this.o_Orientation == Orientation.Horizontal)
            {
                int pixelrange = this.Width;
                float mult = (float)pixelrange/(float)valuerange;
                this.ProgressWidthHeight = (int)(mult*this.i_value);
                this.Texcoords = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Texcoords;
                float ValueRangeOfTexCoords = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords[2] - GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords[0];
                this.FillingTexCoords = new float[]
                {
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords[0],
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords[1],
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords[0] + (ValueRangeOfTexCoords/this.MaxValue) * this.i_value,
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Hor_Filling_Texcoords[3]
                };
            }
            else
            {
                int pixelrange = this.Height;
                float mult = (float)pixelrange/(float)valuerange;
                this.ProgressWidthHeight = (int)(mult*this.i_value);
                this.Texcoords = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Texcoords;
                float ValueRangeOfTexCoords = GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Filling_Texcoords[3] - GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Filling_Texcoords[1];
                this.FillingTexCoords = new float[]
                {
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Filling_Texcoords[0],
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Filling_Texcoords[1] + (ValueRangeOfTexCoords/this.MaxValue) * (this.MaxValue-this.i_value),
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Filling_Texcoords[2],
                    GuiTheme.DefaultGuiTheme.InteractiveProgressBar_Ver_Filling_Texcoords[3]
                };
            }
        }

    }
}