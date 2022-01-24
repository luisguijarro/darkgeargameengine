using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{    
    public class ContentViewer : BaseObjects.Control
    {
        private TextureBufferObject tbo_internal;
        private ScaleMode sm_scalemode;
        public ContentViewer() : this(90,90)
        {

        }

        public ContentViewer(int width, int height) : base(width, height)
        {            
            this.sm_scalemode = ScaleMode.Zoom;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ContentViewer_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.ContentViewer_Texcoords;
            this.tcFrameOffset = new float[]{0,0};
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ContentViewer_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.ContentViewer_Texcoords;
        }

        public void LoadImage(string path)
        {
            this.tbo_internal = dge.G2D.Tools.LoadImage(path);
        }

        protected override void pDraw()
        {
            if (this.tbo_internal.ui_ID > 0)
            {
                if (this.sm_scalemode == ScaleMode.Zoom)
                {
                    if (this.tbo_internal.Width > this.tbo_internal.Height)
                    {
                        float rate = this.tbo_internal.Width/this.tbo_internal.Height;
                        this.gui.Drawer.Draw(this.tbo_internal.ui_ID, this.X+this.MarginLeft, this.Y+this.MarginTop, 0,this.i_width - (this.MarginLeft+this.MarginRight), this.i_height - (int)((this.MarginTop+this.MarginBottom)/rate), 0f, 0f, 0f, 1f, 1f);
                    }
                    else
                    {
                        float rate = this.tbo_internal.Height/this.tbo_internal.Width;
                        this.gui.Drawer.Draw(this.tbo_internal.ui_ID, this.X+this.MarginLeft, this.Y+this.MarginTop, 0, (int)((this.i_width - (this.MarginLeft+this.MarginRight))/rate), this.i_height - (this.MarginTop+this.MarginBottom), 0f, 0f, 0f, 1f, 1f);
                    }
                }
                else
                {
                    this.gui.Drawer.Draw(this.tbo_internal.ui_ID, this.X+this.MarginLeft, this.Y+this.MarginTop, 0, this.i_width - (this.MarginLeft+this.MarginRight), this.i_height - (this.MarginTop+this.MarginBottom), 0f, 0f, 0f, 1f, 1f);
                }
            }
            base.pDraw();
        }

        public TextureBufferObject ContentTBO
        {
            set { this.tbo_internal = value; }
            get { return this.tbo_internal; }
        }

        public ScaleMode ScaleMode
        {
            set { this.sm_scalemode = value; }
            get { return this.sm_scalemode; }
        }
    }

    public enum ScaleMode { Zoom = 0, Stretch = 1}
}