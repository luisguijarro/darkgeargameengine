using  System;

using dge.G2D;

namespace dge.GUI
{
    public class ImageSelectedEventArgs : EventArgs
    {
        TextureBufferObject tbo;
        public ImageSelectedEventArgs(TextureBufferObject image)
        {
            this.tbo = image;
        }
        public TextureBufferObject Image
        {
            get { return this.tbo; }
        }
    }
}