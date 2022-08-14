using System;
using System.Linq;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;

using dge.GLSL;

namespace dge.G2D
{
    public partial class Drawer
    {
        private I_Drawer Instance;
        public dgtk.Math.Mat4 m4P
        {
            get { return this.Instance.M4P; }
            set { this.Instance.M4P = value;}
        } // Para Renicios internos de Perspectiva.

        public bool isGLES { get; }
        
        internal Drawer(bool IsGLES)
        {
            this.isGLES = IsGLES;
            
            if (IsGLES)
            {
                Instance = new DrawerGLES();
            }
            else
            {
                Instance = new DrawerGL();
            }
        }

        /// <sumary>
        /// Method use to define de color that not will be drawer when use Draw Methods. The alpha channel is independent of this configuration.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="color">Color4 that will be use like Transparent color.</param>
        public void DefineTransparentColor(Color4 color)
        {
            Instance.DefineTransparentColor(color);
        }

        /// <sumary>
        /// Method use to define de 4x4 Matrix of View to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de projectión information.</param>
        public void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            Instance.DefineViewMatrix(mat);
        }

        /// <sumary>
        /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de perspective information.</param>
        public void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            Instance.DefinePerspectiveMatrix(x, y, with, height, invert_y);
        }

        public void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            Instance.DefinePerspectiveMatrix(m4);
        }

        public void DefineGlobalLightColor(Color4 lightcolor)
        {
            Instance.DefineGlobalLightColor(lightcolor);
        }

        #region Metodos Draw Publicos.

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>      
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees)
        {
            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, 0, 0, 1, 1, 0);
        }


        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>         
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>    
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees, bool FlipH, bool FlipV)
        {
            Draw(TextureID, Color4.White, x, y, depth, width, height, RotInDegrees, 0, 0, 1, 1, FlipH, FlipV);
        }


        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>        
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>        
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(TextureID, Color, x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>       
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees)
        {
            DrawGL(TextureID, Color, x, y, depth, width, height, RotInDegrees, 0, 0, 1, 1, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>       
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees, bool FlipH, bool FlipV)
        {
            Draw(TextureID, Color, x, y, depth, width, height, RotInDegrees,0, 0, 1, 1, FlipH, FlipV);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>1f
        /// <param name="TextureID">Integral identifier of the texture to be used.</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(TextureID, Color, x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements using de with and height dimensions of Texture attributes.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw, with info of width and Height.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>        
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, tbo.Width, tbo.Height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements using de with and height dimensions of Texture attributes.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw, with info of width and Height.</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>        
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, Color, x, y, depth, tbo.Width, tbo.Height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements using de with and height dimensions of Texture attributes.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw, with info of width and Height.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>        
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, tbo.Width, tbo.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements using de with and height dimensions of Texture attributes.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw, with info of width and Height.</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>        
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, Color, x, y, depth, tbo.Width, tbo.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>1f
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw..</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>        
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw..</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>        
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, Color, x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw..</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw..</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="rotateX">X coord of point of the rotation.</param>
        /// <param name="rotateY">Y coord of point of the rotation.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, rotateX, rotateY, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw Elements.
        /// </sumary>
        /// <remarks>DRAW Textures</remarks>
        /// <param name="tbo">Texture Buffer Object of the texture to draw..</param>
        /// <param name="Color">Color4 to mix with the Texture.</param>
        /// <param name="x">X coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the texture to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="Texcoord0x">The initial X normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord0y">The initial Y normalized coordinate of the texture fraction check box. 0f by default.</param>
        /// <param name="Texcoord1x">End X normalized coordinate of the texture fraction check box. 1f by default.</param>
        /// <param name="Texcoord1y">End Y normalized coordinate of the texture fraction check box. 1f by default.</param>    
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, Color, x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
        }

        /// <sumary>
        /// Method use to draw colored rectangles.
        /// </sumary>
        /// <remarks>DRAW Rectangle of determine Color</remarks>
        /// <param name="color">Color to Draw in screen.</param>
        /// <param name="x">X coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>    
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        public void Draw(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees)
        {
            DrawGL(color, x, y, depth, width, height, RotInDegrees);
        }

        /// <sumary>
        /// Method use to draw silhouettes.
        /// </sumary>
        /// <remarks>DRAW a colored silhouette.</remarks>
        /// <param name="color">Color to use to draw the silhouette in screen.</param>
        /// <param name="silhouette">Texture Buffer Object of the silhouette to draw with info of Width and Height.</param>
        /// <param name="x">X coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, float RotInDegrees)
        {
            DrawGL(silhouette.ui_ID, color, x, y, depth, silhouette.Width, silhouette.Height, RotInDegrees, 0f, 0f, 1f, 1f, 1);
        }
        
        /// <sumary>
        /// Method use to draw silhouettes.
        /// </sumary>
        /// <remarks>DRAW a colored silhouette.</remarks>
        /// <param name="color">Color to use to draw the silhouette in screen.</param>
        /// <param name="silhouette">Texture Buffer Object of the silhouette to draw with info of Width and Height.</param>
        /// <param name="x">X coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="TexCoords">Float array of Texture Coordinates.</param>
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, float RotInDegrees, float[] TexCoords)
        {
            DrawGL(silhouette.ui_ID, color, x, y, depth, silhouette.Width, silhouette.Height, RotInDegrees, TexCoords[0], TexCoords[1], TexCoords[2], TexCoords[3], 1);
        }
        
        /// <sumary>
        /// Method use to draw silhouettes.
        /// </sumary>
        /// <remarks>DRAW a colored silhouette.</remarks>
        /// <param name="color">Color to use to draw the silhouette in screen.</param>
        /// <param name="silhouette">Texture Buffer Object of the silhouette to draw with info of Width and Height.</param>
        /// <param name="x">X coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, float RotInDegrees, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? 1 : 0;
            float tc0Y = FlipV ? 1 : 0;
            float tc1X = FlipH ? 0 : 1;
            float tc1Y = FlipV ? 0 : 1;

            DrawGL(silhouette.ui_ID, color, x, y, depth, silhouette.Width, silhouette.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 1);
        }
        
        /// <sumary>
        /// Method use to draw silhouettes.
        /// </sumary>
        /// <remarks>DRAW a colored silhouette.</remarks>
        /// <param name="color">Color to use to draw the silhouette in screen.</param>
        /// <param name="silhouette">Texture Buffer Object of the silhouette to draw.</param>
        /// <param name="x">X coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>    
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, int width, int height, float RotInDegrees)
        {
            DrawGL(silhouette.ui_ID, color, x, y, depth, width, height, RotInDegrees, 0f, 0f, 1f, 1f, 1);
        }

        /// <sumary>
        /// Method use to draw silhouettes.
        /// </sumary>
        /// <remarks>DRAW a colored silhouette.</remarks>
        /// <param name="color">Color to use to draw the silhouette in screen.</param>
        /// <param name="silhouette">Texture Buffer Object of the silhouette to draw.</param>
        /// <param name="x">X coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="y">Y coordinate of the position on the screen where the color rectangle to be painted will be placed.</param>
        /// <param name="width">Width of the drawing to be painted on the screen.</param>
        /// <param name="height">Height of the drawing to be painted on screen.</param>    
        /// <param name="RotInDegrees">Degrees of the rotation.</param>
        /// <param name="FlipH">Indicates if The texture is drawn horizontally Flipped.</param>        
        /// <param name="FlipV">Indicates if The texture is drawn vertically Flipped.</param>            
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, int width, int height, float RotInDegrees, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? 1 : 0;
            float tc0Y = FlipV ? 1 : 0;
            float tc1X = FlipH ? 0 : 1;
            float tc1Y = FlipV ? 0 : 1;

            DrawGL(silhouette.ui_ID, color, x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 1);
        }

        #endregion

        private void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees)
        {
            Instance.DrawGL(color, x, y, depth, width, height, RotInDegrees);
        }
        
        private void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
        {
            Instance.DrawGL(tboID, color, x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, Silhouette);
        }

        private void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
        {
            Instance.DrawGL(tboID, color, x, y, depth, width, height, rotateX, rotateY, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, Silhouette);
        }


        public Color4 TransparentColor
        {
            get 
            { 
                return Instance.TransparentColor;
            }
        }
    }
}