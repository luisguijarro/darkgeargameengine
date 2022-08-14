using System;
using System.Linq;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;

using dge.GLSL;

namespace dge.G2D
{
    interface I_Drawer
    {        
        dgtk.Math.Mat4 M4P { get; set; }
        Color4 TransparentColor { get; }

        void DefineTransparentColor(Color4 color);
        void DefineViewMatrix(dgtk.Math.Mat4 mat);
        void DefineGlobalLightColor(Color4 lightcolor);
        void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4);
        void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y);

        void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees);
        
        void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette);

        void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette);


        void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights);
        
        void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights);

        void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights);

        void SendUniformLights(Light2D[] lights);
    }
}