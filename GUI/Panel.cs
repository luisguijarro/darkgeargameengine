using System;

namespace dge.GUI
{
    public class Panel : BaseObjects.Control
    {
        private readonly ScrollBar sb_Hor;
        private readonly ScrollBar sb_Ver;
        private BorderStyle bs_border;
        public Panel()
        {
            this.bs_border = BorderStyle.None;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Panel_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.Panel_Texcoords;
            this.UpdateBorder();
            //this.tcFrameOffset = new float[]{0f,0f}; //GuiTheme.DefaultGuiTheme.Menu_FrameOffset;

            this.sb_Hor = new ScrollBar();
            this.sb_Ver = new ScrollBar();
            this.sb_Hor.Orientation = Orientation.Horizontal;
            this.sb_Ver.Orientation = Orientation.Vertical;
            this.sb_Hor.Visible = false;
            this.sb_Ver.Visible = false;            
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.sb_Hor.Dispose();
                this.sb_Ver.Dispose();
            }
        }

        private void UpdateBorder()
        {
            switch(this.bs_border)
            {
                case BorderStyle.In:
                    this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Panel_BorderInFrameOffset;
                    break;
                case BorderStyle.Out:
                    this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Panel_BorderOutFrameOffset;
                    break;
                case BorderStyle.None:
                default:
                    this.tcFrameOffset = new float[]{0f,0f};
                    break;
            }
        }

        #region PROTECTED METHODS:

        protected override void OnResize()
        {
            base.OnResize();
            this.ResizeScrollBars();
        }

        protected override void OnReposition()
        {
            base.OnReposition();
            this.ResizeScrollBars();
        }

        #endregion

        private void ResizeScrollBars()
        {
            this.sb_Ver.X = (int)(this.X+this.Height-this.sb_Ver.Width+this.MarginTop);
            this.sb_Ver.Y = this.Y;
            this.sb_Ver.Height = (uint)(this.Height-(this.MarginTop+this.MarginBottom));

            this.sb_Hor.X = (int)this.X+this.MarginLeft;
            this.sb_Hor.Y = (int)(this.Y+this.Height-this.sb_Hor.Height-this.MarginBottom);
            this.sb_Hor.Width = (uint)((int)this.Height- (int)(this.MarginLeft + this.MarginRight + (int)((this.sb_Ver.Visible) ? this.sb_Ver.Width : 0)));
        }

        private void UpdateScrollBars()
        {
            int max_X = 0;
            int max_Y = 0;
            foreach(BaseObjects.BaseGuiSurface bgs in this.d_guiSurfaces.Values)
            {
                int ix = (int)(bgs.X+bgs.Width);
                int iy = (int)(bgs.Y+bgs.Height);
                max_X = (max_X < ix) ? ix : max_X;
                max_Y = (max_Y < iy) ? iy : max_Y;
            }

            //Bar Horizontal
            if (max_X > this.Width)
            {
                this.sb_Hor.Visible = true;
                this.sb_Hor.MaxValue = (int)(max_X - this.Width);
            }
            else
            {
                this.sb_Hor.Visible = false;
            }

            //Bar Vertical
            if (max_Y > this.Height)
            {
                this.sb_Ver.Visible = true;
                this.sb_Ver.MaxValue = (int)(max_Y-this.Height);
            }
            else
            {
                this.sb_Ver.Visible = false;
            }
        }

        protected override void pDrawContent()
        {
            base.pDrawContent();
            if (this.sb_Hor.Visible) { this.sb_Hor.Draw(); }
            if (this.sb_Ver.Visible) { this.sb_Ver.Draw(); }
        }

        protected override void pDrawContentID()
        {
            base.pDrawContentID();
            if (this.sb_Hor.Visible) { this.sb_Hor.DrawID(); }
            if (this.sb_Ver.Visible) { this.sb_Ver.DrawID(); }
        }

        public BorderStyle BorderStyle
        {
            set 
            { 
                this.bs_border = value;
                this.UpdateBorder();
            }
            get { return this.bs_border; }
        }
    }
}
