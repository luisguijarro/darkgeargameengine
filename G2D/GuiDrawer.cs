using System;
using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.OpenGL;
using dgtk.Graphics;

using dge.GLSL;

namespace dge.G2D
{
    public class GuiDrawer
    {
        internal dgtk.Math.Mat4 m4P; // Para Renicios internos de Perspectiva.
        private uint VAO; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private uint VBO; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private uint EBO; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)
        private Shader BasicShader;

        #region Uniforms Ids
        private int idUniformTColor; // ID de Uniform de Color transparente.
        private int idUniformColor; // ID de Uniform de color multiplicante.    
        private int idUniform_texcoords; // ID de Uniform de Coordenadas de Textura Dinamicas.
        private int idUniform_v_size; // ID de Uniform de Tamaño de superficie de dibujo.
        private int idUniformMat_View; // ID de Uniform que contiene la matriz de Projección.
        private int idUniformMat_Per; // ID de Uniform que contiene la matriz de Perspectiva.
        private int idUniformMat_Tra; // ID de Uniform que contiene la matriz de Perspectiva.
        private int idUniformSilhouette; // ID de Uniform que contiene la matriz de Transformación.
        private int idUniformTexturePassed; // Id de Uniform que indica si se está pasando textura o no.
        private int idUniformtcDisplacement; // Desplazamiento de coordenadas de textura en caso de evento.
        private int idUniform_MarginsFromTheEdge; // margenes en pixeles desde el borde, para posición de vertices internos.

        #endregion

        internal GuiDrawer()
        {
            TVertex2D[] vertices = new TVertex2D[16]
            {
                new TVertex2D(0, new Vector2(0, 0), new Vector2(0,0)),
                new TVertex2D(1, new Vector2(0, 0.33f), new Vector2(0, 0.33f)),
                new TVertex2D(2, new Vector2(0, 0.66f), new Vector2(0, 0.66f)),
                new TVertex2D(3, new Vector2(0, 1), new Vector2(0, 1)),
                new TVertex2D(4, new Vector2(0.33f, 0), new Vector2(0.33f, 0)),
                new TVertex2D(5, new Vector2(0.33f, 0.33f), new Vector2(0.33f, 0.33f)),
                new TVertex2D(6, new Vector2(0.33f, 0.66f), new Vector2(0.33f, 0.66f)),
                new TVertex2D(7, new Vector2(0.33f, 1), new Vector2(0.33f, 1)),
                new TVertex2D(8, new Vector2(0.66f, 0), new Vector2(0.66f, 0)),
                new TVertex2D(9, new Vector2(0.66f, 0.33f), new Vector2(0.66f, 0.33f)),
                new TVertex2D(10, new Vector2(0.66f, 0.66f), new Vector2(0.66f, 0.66f)),
                new TVertex2D(11, new Vector2(0.66f, 1), new Vector2(0.66f, 1)),
                new TVertex2D(12, new Vector2(1, 0), new Vector2(1, 0)),
                new TVertex2D(13, new Vector2(1, 0.33f), new Vector2(1, 0.33f)),
                new TVertex2D(14, new Vector2(1, 0.66f), new Vector2(1, 0.66f)),
                new TVertex2D(15, new Vector2(1, 1), new Vector2(1,1)),
            };

            UInt32[] indices = new uint[]
            {
                00, 01, 05, 04, 00, 05,    04, 05, 09, 08, 04, 09,    08, 09, 13, 12, 08, 13, //Fila 1 de Dobles Triangulos
                01, 02, 06, 05, 01, 06,    05, 06, 10, 09, 05, 10,    09, 10, 14, 13, 09, 14, //Fila 2 de Dobles Triangulos
                02, 03, 07, 06, 02, 07,    06, 07, 11, 10, 06, 11,    10, 11, 15, 14, 10, 15  //Fila 3 de Dobles Triangulos
            };

            VAO = GL.glGenVertexArray(); // Vertex Array Object. "Contiene" toda la declaracion de VBO y TBO
            VBO = GL.glGenBuffer(); // Vertex Buffer Object. Objeto que contiene el buffer de vertices
            EBO = GL.glGenBuffer(); // ElementBuffer Object. Objeto que contiene el indice del orden de vertices.

            GL.glBindVertexArray(VAO);
            GL.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO);
            GL.glBufferData<TVertex2D>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertex2D))*16, vertices, BufferUsageARB.GL_STATIC_DRAW);

            GL.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO);
            GL.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indices.Length, indices, BufferUsageARB.GL_STATIC_DRAW);
            
            GL.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(0)); // ID
            GL.glEnableVertexAttribArray(0);

            GL.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(sizeof(int))); // Vertex Position
            GL.glEnableVertexAttribArray(1);

            GL.glVertexAttribPointer(2, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr((sizeof(float)*2)+sizeof(int)/*Vector2 de posicion tiene dos float +  el uint de ID*/)); // Texcoords.
            GL.glEnableVertexAttribArray(2);

            GL.glBindVertexArray(0);

            BasicShader = new Shader(ShadersSources.BasicGUIvs, ShadersSources.BasicGUIfs);

            idUniform_texcoords = GL.glGetUniformLocation(BasicShader.ui_id, "utexcoords");
            idUniform_MarginsFromTheEdge = GL.glGetUniformLocation(BasicShader.ui_id, "MarginsFromTheEdge");
            idUniform_v_size = GL.glGetUniformLocation(BasicShader.ui_id, "v_size");
            idUniformTColor = GL.glGetUniformLocation(BasicShader.ui_id, "tColor");
            idUniformColor = GL.glGetUniformLocation(BasicShader.ui_id, "Color");
            idUniformMat_View = GL.glGetUniformLocation(BasicShader.ui_id, "view");
            idUniformMat_Per = GL.glGetUniformLocation(BasicShader.ui_id, "perspective");
            idUniformMat_Tra = GL.glGetUniformLocation(BasicShader.ui_id, "trasform");
            idUniformSilhouette = GL.glGetUniformLocation(BasicShader.ui_id, "Silhouette");
            idUniformTexturePassed = GL.glGetUniformLocation(BasicShader.ui_id, "TexturePassed");
            idUniformtcDisplacement = GL.glGetUniformLocation(BasicShader.ui_id, "tcDisplacement");

            DefineTransparentColor(new Color4(1f, 0f, 1f, 1f));
            DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

            GL.glEnable(EnableCap.GL_BLEND);
            GL.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);

        }
        internal void DefineTransparentColor(Color4 color)
        {
            //c4_TransparentColor = color;
            BasicShader.Use();
            GL.glUniform3f(idUniformTColor, color.R, color.G, color.B);
        }

        internal void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE, mat);
        }

        internal void DefinePerspectiveMatrix(float x, float y, float with, float height)
        {
            this.m4P = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, with, height, y, -100f, 100f);
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, this.m4P);
        }

        internal void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            this.m4P = m4;
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, this.m4P);
        }

        public void DrawGL(Color4 color, int x, int y, uint width, uint height, float RotInDegrees)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((-RotInDegrees), new Vector2(width/2f, height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 2, new float[]{0f, 0f, 1f, 1f, 0f, 0f, 1f, 1f});
            GL.glUniform2fv(idUniformtcDisplacement, 1, new float[]{0f, 0f});
            GL.glUniform4iv(idUniform_MarginsFromTheEdge, 1, new int[]{0, 0, 0, 0});
            GL.glUniform2f(idUniform_v_size, width, height);
            GL.glUniform1i(idUniformSilhouette, 0);
            GL.glUniform1i(idUniformTexturePassed, 0);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 54, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        public void DrawGL(uint tboID, Color4 color, int x, int y, uint width, uint height, float RotInDegrees, int[/*4*/]MarginsFromTheEdge, float[/*8*/]TexCoords, float[/*2*/]v2_CoordVariation, int Silhouette)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((-RotInDegrees), new Vector2(width/2f, height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 2, TexCoords);
            GL.glUniform2fv(idUniformtcDisplacement, 1, v2_CoordVariation);
            GL.glUniform4iv(idUniform_MarginsFromTheEdge, 1, MarginsFromTheEdge);
            GL.glUniform2f(idUniform_v_size, width, height);
            GL.glUniform1i(idUniformSilhouette, Silhouette);
            GL.glUniform1i(idUniformTexturePassed, 1);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 54, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }
    }
}