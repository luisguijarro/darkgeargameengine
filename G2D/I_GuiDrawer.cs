using System;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.OpenGL;
using dgtk.Graphics;

using dge.GLSL;

namespace dge.G2D
{
    interface I_GuiDrawer
    {
        void DefineTransparentColor(Color4 color);

        void DefineViewMatrix(dgtk.Math.Mat4 mat);

        void DefinePerspectiveMatrix(float x, float y, float with, float height);

        void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4);

        void DrawGL(Color4 color, int x, int y, int width, int height, float RotInDegrees);

        void DrawGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, int[]MarginsFromTheEdge, float[]TexCoords, float[]v2_CoordVariation, int Silhouette);
        
        dgtk.Math.Mat4 M4P { get; set; }
    }
}