using System;
using System.Runtime.InteropServices;
using dgtk.Math;

namespace dge.G2D
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TVertex2D
    {
        public Vector2 Position;
        public Vector2 TexCoord;
        public TVertex2D (Vector2 position, Vector2 texcoord)
        {
            this.Position = position;
            this.TexCoord = texcoord;
        }
    }
}
