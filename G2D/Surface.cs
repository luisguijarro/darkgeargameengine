using System;
using dgtk.Graphics;

namespace dge.G2D
{
    public class Surface
    {
        protected int i_width;
        protected int i_height;
        protected int i_x;
        protected int i_y;
        protected float f_Texcoord0x;
        protected float f_Texcoord0y; 
        protected float f_Texcoord1x;
        protected float f_Texcoord1y;
        protected float RotAngleInDegrees;
        protected TextureBufferObject textureBufferObject;
        
        ///<sumary>
        ///Basic Constructor that make a Surface of 32x32 pixels.
        ///</sumary>
        public Surface() : this(32,32)
        {
            
        }

        ///<sumary>
        ///Basic Constructor that make a Surface width define dimensions
        ///</sumary>
        public Surface(int width, int height)
        {
            this.i_width = width;
            this.i_height = height;
            this.f_Texcoord0x = 0f;
            this.f_Texcoord0y = 0f;
            this.f_Texcoord1x = 1f;
            this.f_Texcoord1y = 1f;
        }

        public virtual TextureBufferObject TextureBufferObject
        {
            set { this.textureBufferObject = value;}
            get { return this.textureBufferObject; }
        }

        
        ///<sumary>
        ///Method use to draw Surface.
        ///</sumary>
        internal virtual void Draw(Drawer drawer)
        {
            this.Draw(drawer, this.f_Texcoord0x, this.f_Texcoord0y, this.f_Texcoord1x, this.f_Texcoord1y);
        }

        ///<sumary>
        ///Method use to draw Surface.
        ///</sumary>
        internal virtual void Draw(Drawer drawer, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            drawer.Draw(this.textureBufferObject, this.i_x, this.i_y, this.i_width, this.i_height, RotAngleInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y);
        }
        
        public float RotationDegrees
        {
            get { return this.RotAngleInDegrees; }
            set { this.RotAngleInDegrees = value; }
        }

        public float Texcoord0x
        {
            set { this.f_Texcoord0x  =value; }
            get { return this.f_Texcoord0x; }
        }
        public float Texcoord0y
        {
            set { this.f_Texcoord0y  =value; }
            get { return this.f_Texcoord0y; }
        }
        public float Texcoord1x
        {
            set { this.f_Texcoord1x  =value; }
            get { return this.f_Texcoord1x; }
        }
        public float Texcoord1y
        {
            set { this.f_Texcoord1y  =value; }
            get { return this.f_Texcoord1y; }
        }

        public int X
        {
            set { this.i_x = value;}
            get { return this.i_x; }
        }

        public int Y
        {
            set { this.i_y = value;}
            get { return this.i_y; }
        }

        public int Width
        {
            set { this.i_width = value; }
            get { return this.i_width; }
        }
        public int Height
        {
            set { this.i_height = value; }
            get { return this.i_height; }
        }
    }
}