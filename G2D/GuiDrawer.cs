using System;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.OpenGL;
using dgtk.Graphics;

using dge.GLSL;

namespace dge.G2D
{
    public partial class GuiDrawer
    {
        private I_GuiDrawer Instance;
        private bool isGLes;
        
        internal GuiDrawer(bool isGLES)
        {
            this.isGLes = isGLES;
            if (isGLES)
            {
                Instance = new GuiDrawerGLES();
            }
            else
            {
                Instance = new GuiDrawerGL();
            }
        }
        internal void DefineTransparentColor(Color4 color)
        {
            Instance.DefineTransparentColor(color);
        }

        internal void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            Instance.DefineViewMatrix(mat);
        }

        internal void DefinePerspectiveMatrix(float x, float y, float with, float height)
        {
            Instance.DefinePerspectiveMatrix(x, y, with, height);
        }

        internal void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            Instance.DefinePerspectiveMatrix(m4);
        }

        public void DrawGL(Color4 color, int x, int y, int width, int height, float RotInDegrees)
        {
            Instance.DrawGL(color, x, y, width, height, RotInDegrees);
        }

        public void DrawGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, int[/*4*/]MarginsFromTheEdge, float[/*8*/]TexCoords, float[/*2*/]v2_CoordVariation, int Silhouette)
        {
            Instance.DrawGL(tboID, color, x, y, width, height, RotInDegrees, MarginsFromTheEdge, TexCoords, v2_CoordVariation, Silhouette);
        }

        public dgtk.Math.Mat4 m4P
        {
            get { return this.Instance.M4P; }
        }
        public bool IsGLES
        {
            get { return this.isGLes; }
        }
    }
}