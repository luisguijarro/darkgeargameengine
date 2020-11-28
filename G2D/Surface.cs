using System;

namespace dge.G2D
{
    public class Surface
    {
        private int i_id;
        private uint i_width;
        private uint i_height;

        
        ///<sumary>
        ///Basic Constructor that make a Surface of 32x32 pixels.
        ///</sumary>
        public Surface() : this(32,32)
        {
            
        }

        ///<sumary>
        ///Basic Constructor that make a Surface width define dimensions
        ///</sumary>
        public Surface(uint width, uint height)
        {
            this.i_id = Core2D.GetID();
            this.i_width = width;
            this.i_height = height;
        }
        ~Surface()
        {
            Core2D.ReleaseID(this.i_id);
        }

        ///<sumary>
        ///Method use to draw Surface.
        ///</sumary>
        public void Draw()
        {
            //Drawer.Draw();
        }

    }
}