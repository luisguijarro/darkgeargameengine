using System;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;
using dge.GLSL;

namespace dge.G2D
{    public class Drawer
    {
        internal dgtk.Math.Mat4 m4P; // Para Renicios internos de Perspectiva.
        private bool b_invert_y;
        private readonly uint VAO; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private readonly uint VBO; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private readonly uint EBO; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)
        
        #region Uniforms Ids
        private readonly int idUniformTColor; // ID de Uniform de Color transparente.
        private readonly int idUniformColor; // ID de Uniform de color multiplicante.    
        private readonly int idUniform_texcoords; // ID de Uniform de Coordenadas de Textura Dinamicas.
        private readonly int idUniform_v_size; // ID de Uniform de Tamaño de superficie de dibujo.
        private readonly int idUniformMat_View; // ID de Uniform que contiene la matriz de Projección.
        private readonly int idUniformMat_Per; // ID de Uniform que contiene la matriz de Perspectiva.
        private readonly int idUniformMat_Tra; // ID de Uniform que contiene la matriz de Perspectiva.
        private readonly int idUniformSilhouette; // ID de Uniform que contiene la matriz de Transformación.
        private readonly int idUniformTexturePassed; // Id de Uniform que indica si se está pasando textura o no.

        #endregion

        private readonly Shader BasicShader;

        internal Drawer()
        {
            //dge.Core2D.PixelBufferObject_Select = GL.glGenBuffer();

            TVertex2D[] vertices = new TVertex2D[4];
            vertices[0] = new TVertex2D(1, new Vector2(0, 0), new Vector2(0,0));
            vertices[1] = new TVertex2D(2, new Vector2(0, 1), new Vector2(0,1));
            vertices[2] = new TVertex2D(3, new Vector2(1, 1), new Vector2(1,1));
            vertices[3] = new TVertex2D(4, new Vector2(1, 0), new Vector2(1,0));

            UInt32[] indices = new uint[]{0, 1, 2, 3, 0, 2};

            VAO = GL.glGenVertexArray(); 
            VBO = GL.glGenBuffer();
            EBO = GL.glGenBuffer();

            GL.glBindVertexArray(VAO);
            GL.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO);
            GL.glBufferData<TVertex2D>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertex2D))*4, vertices, BufferUsageARB.GL_STATIC_DRAW);

            GL.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO);
            GL.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indices.Length, indices, BufferUsageARB.GL_STATIC_DRAW);
            
            GL.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(0)); // ID
            GL.glEnableVertexAttribArray(0);

            GL.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(sizeof(int))); // Vertex Position
            GL.glEnableVertexAttribArray(1);

            GL.glVertexAttribPointer(2, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr((sizeof(float)*2)+sizeof(int)/*Vector2 de posicion tiene dos float +  el uint de ID*/)); // Texcoords.
            GL.glEnableVertexAttribArray(2);

            GL.glBindVertexArray(0);

            BasicShader = new Shader(ShadersSources.Basic2Dvs, ShadersSources.Basic2Dfs);

            idUniform_texcoords = GL.glGetUniformLocation(BasicShader.ui_id, "utexcoords");
            idUniform_v_size = GL.glGetUniformLocation(BasicShader.ui_id, "v_size");
            idUniformTColor = GL.glGetUniformLocation(BasicShader.ui_id, "tColor");
            idUniformColor = GL.glGetUniformLocation(BasicShader.ui_id, "Color");
            idUniformMat_View = GL.glGetUniformLocation(BasicShader.ui_id, "view");
            idUniformMat_Per = GL.glGetUniformLocation(BasicShader.ui_id, "perspective");
            idUniformMat_Tra = GL.glGetUniformLocation(BasicShader.ui_id, "trasform");
            idUniformSilhouette = GL.glGetUniformLocation(BasicShader.ui_id, "Silhouette");
            idUniformTexturePassed = GL.glGetUniformLocation(BasicShader.ui_id, "TexturePassed");

            DefineTransparentColor(new Color4(0f, 0.9f, 0f, 1f));
            DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

            GL.glEnable(EnableCap.GL_BLEND);
            GL.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);
        }

        /// <sumary>
        /// Method use to define de color that not will be drawer when use Draw Methods. The alpha channel is independent of this configuration.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="color">Color4 that will be use like Transparent color.</param>
        public void DefineTransparentColor(Color4 color)
        {
            //c4_TransparentColor = color;
            BasicShader.Use();
            GL.glUniform3f(idUniformTColor, color.R, color.G, color.B);
        }

        /// <sumary>
        /// Method use to define de 4x4 Matrix of View to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de projectión information.</param>
        public void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            BasicShader.Use();
            GL.glUniformMatrix(this.idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE, mat);
        }

        /// <sumary>
        /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de perspective information.</param>
        public void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            b_invert_y = invert_y;
            this.m4P = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, with, invert_y ? height : y, invert_y ? y : height, -100f, 100f);
            BasicShader.Use();
            GL.glUniformMatrix(this.idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
        }

        internal void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            this.m4P = m4;
            BasicShader.Use();
            GL.glUniformMatrix(this.idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
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
        public void Draw(uint TextureID, int x, int y, int width, int height, float RotInDegrees)
        {
            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, width, height, RotInDegrees, 0, 0, 1, 1, 0);
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
        public void Draw(uint TextureID, int x, int y, int width, int height, float RotInDegrees, bool FlipH, bool FlipV)
        {
            Draw(TextureID, Color4.White, x, y, width, height, RotInDegrees, 0, 0, 1, 1, FlipH, FlipV);
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
        public void Draw(uint TextureID, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(TextureID, Color, x, y, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int width, int height, float RotInDegrees)
        {
            DrawGL(TextureID, Color, x, y, width, height, RotInDegrees, 0, 0, 1, 1, 0);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int width, int height, float RotInDegrees, bool FlipH, bool FlipV)
        {
            Draw(TextureID, Color, x, y, width, height, RotInDegrees,0, 0, 1, 1, FlipH, FlipV);
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
        public void Draw(uint TextureID, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(TextureID, new Color4(1f, 1f, 1f, 1f), x, y, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(uint TextureID, Color4 Color, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(TextureID, Color, x, y, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(TextureBufferObject tbo, int x, int y, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, tbo.Width, tbo.Height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, Color, x, y, tbo.Width, tbo.Height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
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
        public void Draw(TextureBufferObject tbo, int x, int y, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, tbo.Width, tbo.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, Color, x, y, tbo.Width, tbo.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y)
        {
            DrawGL(tbo.ui_ID, Color, x, y, width, height, RotInDegrees, Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y, 0);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(TextureBufferObject tbo, int x, int y, int width, int height, int rotateX, int rotateY, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, new Color4(1f, 1f, 1f, 1f), x, y, width, height, rotateX, rotateY, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(TextureBufferObject tbo, Color4 Color, int x, int y, int width, int height, float RotInDegrees,float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? Texcoord1x : Texcoord0x;
            float tc0Y = FlipV ? Texcoord1y : Texcoord0y;
            float tc1X = FlipH ? Texcoord0x : Texcoord1x;
            float tc1Y = FlipV ? Texcoord0y : Texcoord1y;

            DrawGL(tbo.ui_ID, Color, x, y, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 0);
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
        public void Draw(Color4 color, int x, int y, int width, int height, float RotInDegrees)
        {
            DrawGL(color, x, y, width, height, RotInDegrees);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, float RotInDegrees)
        {
            DrawGL(silhouette.ui_ID, color, x, y, silhouette.Width, silhouette.Height, RotInDegrees, 0f, 0f, 1f, 1f, 1);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, float RotInDegrees, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? 1 : 0;
            float tc0Y = FlipV ? 1 : 0;
            float tc1X = FlipH ? 0 : 1;
            float tc1Y = FlipV ? 0 : 1;

            DrawGL(silhouette.ui_ID, color, x, y, silhouette.Width, silhouette.Height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 1);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int width, int height, float RotInDegrees)
        {
            DrawGL(silhouette.ui_ID, color, x, y, width, height, RotInDegrees, 0f, 0f, 1f, 1f, 1);
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
        public void DrawSilhouette(Color4 color, TextureBufferObject silhouette, int x, int y, int width, int height, float RotInDegrees, bool FlipH, bool FlipV)
        {
            float tc0X = FlipH ? 1 : 0;
            float tc0Y = FlipV ? 1 : 0;
            float tc1X = FlipH ? 0 : 1;
            float tc1Y = FlipV ? 0 : 1;

            DrawGL(silhouette.ui_ID, color, x, y, width, height, RotInDegrees, tc0X, tc0Y, tc1X, tc1Y, 1);
        }

        #endregion

        private void DrawGL(Color4 color, int x, int y, int width, int height, float RotInDegrees)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.

            BasicShader.Use();
            //GL.glUniform4fv(idUniform_texcoords, 1, new float[]{0f, 0f, 1f, 1f});
            GL.glUniform2f(idUniform_v_size, width, height);
            GL.glUniform1i(idUniformSilhouette, 0);
            GL.glUniform1i(idUniformTexturePassed, 0);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }
        
        private void DrawGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width/2f, height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
            GL.glUniform2f(idUniform_v_size, width, height);
            GL.glUniform1i(idUniformSilhouette, Silhouette);
            GL.glUniform1i(idUniformTexturePassed, 1);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        private void DrawGL(uint tboID, Color4 color, int x, int y, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(rotateX, rotateY));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
            GL.glUniform2f(idUniform_v_size, width, height);
            GL.glUniform1i(idUniformSilhouette, Silhouette);
            GL.glUniform1i(idUniformTexturePassed, 1);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }


        public Color4 TransparentColor
        {
            get 
            { 
                BasicShader.Use();
                float[] fret = GL.glGetUniformfv(BasicShader.ui_id, idUniformTColor, 4);
                return new Color4(fret[0], fret[1], fret[2], fret[3]); 
            }
        }
    }
}