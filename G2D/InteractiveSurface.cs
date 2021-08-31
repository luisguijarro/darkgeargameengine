using System;
using dgtk.Graphics;

namespace dge.G2D
{
    public class InteractiveSurface : dge.G2D.Surface
    {
        protected uint ui_id;
        private Color4 idColor;

        ///<sumary>
        ///Basic Constructor that make a Surface of 32x32 pixels.
        ///</sumary>
        public InteractiveSurface() : this(32,32)
        {
            
        }

        ///<sumary>
        ///Basic Constructor that make a Surface width define dimensions
        ///</sumary>
        public InteractiveSurface(int width, int height)
        {
            this.ui_id = Core2D.GetID(); // Obtenemos ID de la superficie.
            byte[] colorvalues = Core2D.DeUIntAByte4(this.ui_id); // Establecemos color de ID de la superficie.
            this.idColor = new Color4((byte)colorvalues[0], (byte)colorvalues[1], (byte)colorvalues[2], (byte)colorvalues[3]);
            base.i_width = width;
            base.i_height = height;            
        }
        ~InteractiveSurface()
        {
            Core2D.ReleaseID(this.ui_id); // Liberamos ID de la superficie.
        }

        internal virtual void DrawID()
        {
            //dge.G2D.Drawer.Draw(this.idColor, base.i_x, base.i_y, i_width, i_height, 0); // Pintamos ID de la superficie.
            dge.G2D.IDsDrawer.DrawGL2D(this.idColor, base.i_x, base.i_y, base.i_width, base.i_height, 0); // Pintamos ID de la superficie.
            //dge.G2D.IDsDrawer.DrawGL(this.textureBufferObject.ID, this.idColor, base.i_x, base.i_y, i_width, i_height, 0, this.f_Texcoord0x, this.f_Texcoord0y, this.f_Texcoord1x, this.f_Texcoord1y, 1); // Pintamos ID de la superficie.
        }

        #region PROPERTIES:
        public uint ID
        {
            get { return this.ui_id; }
        }
        #endregion
    }
}