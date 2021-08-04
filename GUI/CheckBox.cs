using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class CheckBox : BaseObjects.Control
    {
        private bool b_Checked;
        public event EventHandler<CheckedStateChanged> CheckedStateChanged;
        public CheckBox() : base(22, 22)
        {
            this.b_Checked = false;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.CheckBox_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.CheckBox_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.CheckBox_FrameOffset;

            this.CheckedStateChanged += delegate{};
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.CheckBox_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.CheckBox_Texcoords;
            this.tcFrameOffset = this.gui.gt_ActualGuiTheme.CheckBox_FrameOffset;
        }

        protected override void MDown(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.b_Checked = !this.b_Checked;
                this.CheckedStateChanged(this, new CheckedStateChanged(this.b_Checked));
                base.MDown(sender, e);
            }
        }

        protected override void pDraw()
        {
            if (this.gui != null)
            {
                //base.Draw();
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_Checked ? this.tcFrameOffset : new float[]{0,0}, 0);
            }
        }

        public bool Checked
        {
            set { this.b_Checked = value; }
            get { return this.b_Checked; }
        }
    }
}