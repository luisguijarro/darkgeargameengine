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
        private class GuiDrawerGLES : I_GuiDrawer
        {
            private dgtk.Math.Mat4 m4P; // Para Renicios internos de Perspectiva.
            private readonly uint VAO; // Vertex Array Object (indice que contiene toda la info del objeto.)
            private readonly uint VBO; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
            private readonly uint EBO; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)
            private readonly Shader BasicShader;

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
            private readonly int idUniformtcDisplacement; // Desplazamiento de coordenadas de textura en caso de evento.
            private readonly int idUniform_MarginsFromTheEdge; // margenes en pixeles desde el borde, para posición de vertices internos.

            #endregion

            internal GuiDrawerGLES()
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

                VAO = GLES.glGenVertexArray(); // Vertex Array Object. "Contiene" toda la declaracion de VBO y TBO
                VBO = GLES.glGenBuffer(); // Vertex Buffer Object. Objeto que contiene el buffer de vertices
                EBO = GLES.glGenBuffer(); // ElementBuffer Object. Objeto que contiene el indice del orden de vertices.

                GLES.glBindVertexArray(VAO);
                GLES.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO);
                GLES.glBufferData<TVertex2D>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertex2D))*16, vertices, BufferUsageARB.GL_STATIC_DRAW);

                GLES.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO);
                GLES.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indices.Length, indices, BufferUsageARB.GL_STATIC_DRAW);
                
                GLES.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(0)); // ID
                GLES.glEnableVertexAttribArray(0);

                GLES.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(sizeof(int))); // Vertex Position
                GLES.glEnableVertexAttribArray(1);

                GLES.glVertexAttribPointer(2, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr((sizeof(float)*2)+sizeof(int)/*Vector2 de posicion tiene dos float +  el uint de ID*/)); // Texcoords.
                GLES.glEnableVertexAttribArray(2);

                GLES.glBindVertexArray(0);

                Console.WriteLine("GUIDrawer Shader Compilation.");
                BasicShader = new Shader(ShadersSourcesGLES.BasicGUIvs, ShadersSourcesGLES.BasicGUIfs, true);

                idUniform_texcoords = GLES.glGetUniformLocation(BasicShader.ID, "utexcoords");
                idUniform_MarginsFromTheEdge = GLES.glGetUniformLocation(BasicShader.ID, "MarginsFromTheEdge");
                idUniform_v_size = GLES.glGetUniformLocation(BasicShader.ID, "v_size");
                idUniformTColor = GLES.glGetUniformLocation(BasicShader.ID, "tColor");
                idUniformColor = GLES.glGetUniformLocation(BasicShader.ID, "Color");
                idUniformMat_View = GLES.glGetUniformLocation(BasicShader.ID, "view");
                idUniformMat_Per = GLES.glGetUniformLocation(BasicShader.ID, "perspective");
                idUniformMat_Tra = GLES.glGetUniformLocation(BasicShader.ID, "trasform");
                idUniformSilhouette = GLES.glGetUniformLocation(BasicShader.ID, "Silhouette");
                idUniformTexturePassed = GLES.glGetUniformLocation(BasicShader.ID, "TexturePassed");
                idUniformtcDisplacement = GLES.glGetUniformLocation(BasicShader.ID, "tcDisplacement");

                DefineTransparentColor(new Color4(1f, 0f, 1f, 1f));
                DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

                GLES.glEnable(EnableCap.GL_BLEND);
                GLES.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);

            }
            public void DefineTransparentColor(Color4 color)
            {
                //c4_TransparentColor = color;
                BasicShader.Use();
                GLES.glUniform3f(idUniformTColor, color.R, color.G, color.B);
            }

            public void DefineViewMatrix(dgtk.Math.Mat4 mat)
            {
                BasicShader.Use();
                GLES.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE, mat);
            }

            public void DefinePerspectiveMatrix(float x, float y, float with, float height)
            {
                this.m4P = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, with, height, y, -100f, 100f);
                BasicShader.Use();
                GLES.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, this.m4P);
            }

            public void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
            {
                this.m4P = m4;
                BasicShader.Use();
                GLES.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, this.m4P);
            }

            public void DrawGL(Color4 color, int x, int y, int width, int height, float RotInDegrees)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((-RotInDegrees), new Vector2(width/2f, height/2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
                
                GLES.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader.Use();
                GLES.glUniform4fv(idUniform_texcoords, 2, new float[]{0f, 0f, 1f, 1f, 0f, 0f, 1f, 1f});
                GLES.glUniform2fv(idUniformtcDisplacement, 1, new float[]{0f, 0f});
                GLES.glUniform4iv(idUniform_MarginsFromTheEdge, 1, new int[]{0, 0, 0, 0});
                GLES.glUniform2f(idUniform_v_size, width, height);
                GLES.glUniform1i(idUniformSilhouette, 0);
                GLES.glUniform1i(idUniformTexturePassed, 0);
                GLES.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 54, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }

            public void DrawGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, int[/*4*/]MarginsFromTheEdge, float[/*8*/]TexCoords, float[/*2*/]v2_CoordVariation, int Silhouette)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((-RotInDegrees), new Vector2(width/2f, height/2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
                
                GLES.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader.Use();
                GLES.glUniform4fv(idUniform_texcoords, 2, TexCoords);
                GLES.glUniform2fv(idUniformtcDisplacement, 1, v2_CoordVariation);
                GLES.glUniform4iv(idUniform_MarginsFromTheEdge, 1, MarginsFromTheEdge);
                GLES.glUniform2f(idUniform_v_size, width, height);
                GLES.glUniform1i(idUniformSilhouette, Silhouette);
                GLES.glUniform1i(idUniformTexturePassed, 1);
                GLES.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 54, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }

            public dgtk.Math.Mat4 M4P
            {
                get { return this.m4P; }
                set { this.m4P = value; }
            }
        }
    }
}