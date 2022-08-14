using System;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;
using dge.GLSL;

namespace dge.G2D
{    
    public static class IDsDrawer
    {
        private static I_IDsDrawer Instance;
        public static dgtk.Math.Mat4 m4P
        {
            get { return Instance.M4P; }
            set { Instance.M4P = value; }
        }

        public static void Init_IDs_Drawer(bool IsGLES)
        {
            if (Instance == null)
            {
                if (IsGLES)
                {
                    Instance = new IDsDrawerGLES();
                }
                else
                {
                    Instance = new IDsDrawerGL();
                }
            }
            
        }

        public static void DefineTransparentColor(Color4 color)
        {
            Instance.DefineTransparentColor(color);
        }

        public static void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            Instance.DefineViewMatrix(mat);
        }


        internal static void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
           Instance.DefinePerspectiveMatrix(m4);
        }

        public static void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            Instance.DefinePerspectiveMatrix(x, y, with, height, invert_y);
        }

        #region Draw2D

        public static void DrawGL2D(Color4 color, int x, int y, int width, int height, float RotInDegrees)
        {
            Instance.DrawGL2D(color, x, y, width, height, RotInDegrees);
        }
        
        public static void DrawGL2D(Color4 color, int x, int y, int width, int height, int rotateX, int rotateY, float RotInDegrees)
        {
            Instance.DrawGL2D(color, x, y, width, height, rotateX, rotateY, RotInDegrees);
        }
        
        public static void DrawGL2D(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
        {
            Instance.DrawGL2D(tboID, color, x, y, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, Silhouette);
        }

        #endregion Draw2D

        #region DrawGui

        public static void DrawGuiGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, int[/*4*/]MarginsFromTheEdge, float[/*8*/]TexCoords, float[/*2*/]v2_CoordVariation, int Silhouette)
        {
            Instance.DrawGuiGL(tboID, color, x, y, width, height, RotInDegrees, MarginsFromTheEdge, TexCoords, v2_CoordVariation, Silhouette);
        }

        #endregion DrawGui
    }
}