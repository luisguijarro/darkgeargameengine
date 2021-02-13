using System;
using dgtk.Graphics;

namespace dge.G2D
{
    public class Surface
    {
        protected uint i_width;
        protected uint i_height;
        protected int i_x;
        protected int i_y;
        
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
            this.i_width = width;
            this.i_height = height;
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