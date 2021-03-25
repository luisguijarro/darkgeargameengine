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
        public ContentViewer() : this(90,90)
        {

        }

        public ContentViewer(uint width, uint height) : base(width, height)
        {            
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ContentViewer_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.ContentViewer_Texcoords;
            this.tcFrameOffset = new float[]{0,0};
        }

        public void LoadImage(string path)
        {
            this.tbo_internal = dge.G2D.Tools.LoadImage(path);
        }

        internal override void Draw()
        {
            if (this.tbo_internal.ui_ID > 0)
            {
                this.gui.Drawer.Draw(this.tbo_internal.ui_ID, this.X, this.Y, this.ui_width, this.ui_height, 0f, 0f, 0f, 1f, 1f);
            }
            base.Draw();
        }

        public TextureBufferObject ContentTBO
        {
            set { this.tbo_internal = value; }
            get { return this.tbo_internal; }
        }
    }
}