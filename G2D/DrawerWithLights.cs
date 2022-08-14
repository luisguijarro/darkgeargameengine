using System;
using System.Collections.Generic;

using dgtk.Graphics;

namespace dge.G2D
{   
    public partial class Drawer
    {
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
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
        {
            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, 0, 0, 1, 1, 0, lights);
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
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees, bool FlipH, bool FlipV, Light2D[] lights)
        {
            Draw(TextureID, Color4.White, x, y, depth, width, height, RotInDegrees, 0, 0, 1, 1, FlipH, FlipV, lights);
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
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, Light2D[] lights)
        {
            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0, lights);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, Light2D[] lights)
        {
            DrawGL(TextureID, Color, x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0, lights);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
        {
            DrawGL(TextureID, Color, x, y, depth, width, height, RotInDegrees, 0, 0, 1, 1, 0, lights);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees, bool FlipH, bool FlipV, Light2D[] lights)
        {
            Draw(TextureID, Color, x, y, depth, width, height, RotInDegrees,0, 0, 1, 1, FlipH, FlipV, lights);
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
        public void Draw(uint TextureID, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(TextureID, Color, x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, Light2D[] lights)
        {
            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, tbo.Width, tbo.Height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, Light2D[] lights)
        {
            DrawGL(tbo.ui_ID, Color, x, y, depth, tbo.Width, tbo.Height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, tbo.Width, tbo.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, Color, x, y, depth, tbo.Width, tbo.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, Light2D[] lights)
        {
            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, Light2D[] lights)
        {
            DrawGL(tbo.ui_ID, Color, x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, depth, width, height, rotateX, rotateY, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int depth, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, Color, x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0, lights);
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
        public void Draw(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
        {
            DrawGL(color, x, y, depth, width, height, RotInDegrees, lights);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, float RotInDegrees, Light2D[] lights)
        {
            DrawGL(silhouette.ui_ID, color, x, y, depth, silhouette.Width, silhouette.Height, RotInDegrees, 0f, 0f, 1f, 1f, 1, lights);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, float RotInDegrees, float[] TexCoords, Light2D[] lights)
        {
            DrawGL(silhouette.ui_ID, color, x, y, depth, silhouette.Width, silhouette.Height, RotInDegrees, TexCoords[0], TexCoords[1], TexCoords[2], TexCoords[3], 1, lights);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, float RotInDegrees, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? 1 : 0;
            float tc0Y = FlipV ? 1 : 0;
            float tc1X = FlipH ? 0 : 1;
            float tc1Y = FlipV ? 0 : 1;

            DrawGL(silhouette.ui_ID, color, x, y, depth, silhouette.Width, silhouette.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 1, lights);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
        {
            DrawGL(silhouette.ui_ID, color, x, y, depth, width, height, RotInDegrees, 0f, 0f, 1f, 1f, 1, lights);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int depth, int width, int height, float RotInDegrees, bool FlipH, bool FlipV, Light2D[] lights)
        {
            float tc0X = FlipH ? 1 : 0;
            float tc0Y = FlipV ? 1 : 0;
            float tc1X = FlipH ? 0 : 1;
            float tc1Y = FlipV ? 0 : 1;

            DrawGL(silhouette.ui_ID, color, x, y, depth, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 1, lights);
        }

        #endregion

        private void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
        {
            Instance.DrawGL(color, x, y, depth, width, height, RotInDegrees, lights);
        }
        
        private void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
        {
            Instance.DrawGL(tboID, color, x, y, depth, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, Silhouette, lights);
        }

        private void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
        {
            Instance.DrawGL(tboID, color, x, y, depth, width, height, rotateX, rotateY, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, Silhouette, lights);
        }

        private void SendUniformLights(Light2D[] lights)
        {
            Instance.SendUniformLights(lights);
        }
    }
}