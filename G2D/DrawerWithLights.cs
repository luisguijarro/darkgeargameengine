using System;
using System.Collections.Generic;
//using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;
using dge.GLSL;

namespace dge.G2D
{   
    public partial class Drawer
    {
        
        #region Uniforms Ids
        private int idUniformTColor_L; // ID de Uniform de Color transparente.
        private int idUniformColor_L; // ID de Uniform de color multiplicante.    
        private int idUniform_texcoords_L; // ID de Uniform de Coordenadas de Textura Dinamicas.
        private int idUniform_v_size_L; // ID de Uniform de Tamaño de superficie de dibujo.
        private int idUniformMat_View_L; // ID de Uniform que contiene la matriz de Projección.
        private int idUniformMat_Per_L; // ID de Uniform que contiene la matriz de Perspectiva.
        private int idUniformMat_Tra_L; // ID de Uniform que contiene la matriz de Perspectiva.
        private int idUniformSilhouette_L; // ID de Uniform que contiene la matriz de Transformación.
        private int idUniformTexturePassed_L; // Id de Uniform que indica si se está pasando textura o no.
        private int idUniformGlobalLightColor_L; // ID de Uniform que contiene el color de la iluminación global;
        private int idUniformlightsPosRange; // ID de Uniform que contiene Posicion y alcance de las Luces
        private int idUniformlightsColor; // ID de Uniform que contiene el Color de las Luces
        private int idUniformlightRotationAngle; // ID de Uniform que contiene si es omnidireccional o no, el angulo de rotación y el angulo de apertura.

        #endregion

        private Shader BasicShader_L;

        private void InitLightShader()
        {
            BasicShader_L = new Shader(ShadersSources.Basic2DIlluminatedvs, ShadersSources.Basic2DIlluminatedfs);

            idUniform_texcoords_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "utexcoords");
            idUniform_v_size_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "v_size");
            idUniformTColor_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "tColor");
            idUniformColor_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "Color");
            idUniformMat_View_L= GL.glGetUniformLocation(BasicShader_L.ui_id, "view");
            idUniformMat_Per_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "perspective");
            idUniformMat_Tra_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "trasform");
            idUniformSilhouette_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "Silhouette");
            idUniformTexturePassed_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "TexturePassed");
            idUniformGlobalLightColor_L = GL.glGetUniformLocation(BasicShader_L.ui_id, "GlobalLightColor");
            idUniformlightsPosRange = GL.glGetUniformLocation(BasicShader_L.ui_id, "lightsPosRange");
            idUniformlightsColor = GL.glGetUniformLocation(BasicShader_L.ui_id, "lightsColor");
            idUniformlightRotationAngle = GL.glGetUniformLocation(BasicShader_L.ui_id, "lightRotationAngle");
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
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.

            BasicShader_L.Use();
            //GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{0f, 0f, 1f, 1f});
            GL.glUniform2f(idUniform_v_size_L, width, height);
            GL.glUniform1i(idUniformSilhouette_L, 0);
            GL.glUniform1i(idUniformTexturePassed_L, 0);
            GL.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
            this.SendUniformLights(lights);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }
        
        private void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width/2f, height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicShader_L.Use();
            GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
            GL.glUniform2f(idUniform_v_size_L, width, height);
            GL.glUniform1i(idUniformSilhouette_L, Silhouette);
            GL.glUniform1i(idUniformTexturePassed_L, 1);
            GL.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
            this.SendUniformLights(lights);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        private void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(rotateX, rotateY));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicShader_L.Use();
            GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
            GL.glUniform2f(idUniform_v_size_L, width, height);
            GL.glUniform1i(idUniformSilhouette_L, Silhouette);
            GL.glUniform1i(idUniformTexturePassed_L, 1);
            GL.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
            this.SendUniformLights(lights);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        private void SendUniformLights(Light2D[] lights)
        {
            List<float> lightsPosRange = new List<float>();
            List<float> lightsColor = new List<float>();
            List<float> lightRotationAngle = new List<float>();
            for (int i=0;i<lights.Length;i++)
            {
                lightsPosRange.AddRange(lights[i].Position.ToArray());
                lightsPosRange.Add(lights[i].LightRange);

                lightsColor.AddRange(lights[i].LightColor.ToRgbaFloatArray());

                lightRotationAngle.Add(lights[i].IsDirectional ? 1f : 0f);
                lightRotationAngle.Add(lights[i].Rotation);
                lightRotationAngle.Add(lights[i].OpeningAngle);
            }

            /*
                    private int idUniformlightsPosRange; // ID de Uniform que contiene Posicion y alcance de las Luces
                    private int idUniformlightsColor; // ID de Uniform que contiene el Color de las Luces
                    private int idUniformlightRotationAngle; // ID de Uniform que contiene si es omnidireccional o no, el angulo de rotación y el angulo de apertura.
            */
            GL.glUniform4fv(idUniformlightsPosRange, lights.Length, lightsPosRange.ToArray());
            GL.glUniform4fv(idUniformlightsColor, lights.Length, lightsColor.ToArray());
            GL.glUniform4fv(idUniformlightRotationAngle, lights.Length, lightRotationAngle.ToArray());
        }
    }
}