using System;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;
using dge.GLSL;

namespace dge.G2D
{    
    interface I_IDsDrawer
    {
        void DefineTransparentColor(Color4 color);

        void DefineViewMatrix(dgtk.Math.Mat4 mat);


        void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4);

        void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y);

        #region Draw2D

        void DrawGL2D(Color4 color, int x, int y, int width, int height, float RotInDegrees);
        
        void DrawGL2D(Color4 color, int x, int y, int width, int height, int rotateX, int rotateY, float RotInDegrees);
        
        void DrawGL2D(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette);

        #endregion Draw2D

        #region DrawGui

        void DrawGuiGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, int[]MarginsFromTheEdge, float[]TexCoords, float[]v2_CoordVariation, int Silhouette);

        #endregion DrawGui

        dgtk.Math.Mat4 M4P
        {
            get; set;
        }
    }
}