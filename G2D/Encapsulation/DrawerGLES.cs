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
        private partial class DrawerGLES : I_Drawer
        {
            internal dgtk.Math.Mat4 m4P;// Para Renicios internos de Perspectiva.
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
            private readonly int idUniformGlobalLightColor; // ID de Uniform que contiene el color de la iluminación global;

            #endregion

            private readonly Shader BasicShader;

            internal DrawerGLES()
            {
                //dge.Core2D.PixelBufferObject_Select = GLES.glGenBuffer();

                TVertex2D[] vertices = new TVertex2D[4];
                vertices[0] = new TVertex2D(1, new Vector2(0, 0), new Vector2(0,0));
                vertices[1] = new TVertex2D(2, new Vector2(0, 1), new Vector2(0,1));
                vertices[2] = new TVertex2D(3, new Vector2(1, 1), new Vector2(1,1));
                vertices[3] = new TVertex2D(4, new Vector2(1, 0), new Vector2(1,0));

                UInt32[] indices = new uint[]{0, 1, 2, 3, 0, 2};

                VAO = GLES.glGenVertexArray(); 
                VBO = GLES.glGenBuffer();
                EBO = GLES.glGenBuffer();

                GLES.glBindVertexArray(VAO);
                GLES.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO);
                GLES.glBufferData<TVertex2D>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertex2D))*4, vertices, BufferUsageARB.GL_STATIC_DRAW);

                GLES.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO);
                GLES.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indices.Length, indices, BufferUsageARB.GL_STATIC_DRAW);
                
                GLES.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(0)); // ID
                GLES.glEnableVertexAttribArray(0);

                GLES.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(sizeof(int))); // Vertex Position
                GLES.glEnableVertexAttribArray(1);

                GLES.glVertexAttribPointer(2, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr((sizeof(float)*2)+sizeof(int)/*Vector2 de posicion tiene dos float +  el uint de ID*/)); // Texcoords.
                GLES.glEnableVertexAttribArray(2);

                GLES.glBindVertexArray(0);

                BasicShader = new Shader(ShadersSources.Basic2Dvs, ShadersSources.Basic2Dfs, true);

                idUniform_texcoords = GLES.glGetUniformLocation(BasicShader.ID, "utexcoords");
                idUniform_v_size = GLES.glGetUniformLocation(BasicShader.ID, "v_size");
                idUniformTColor = GLES.glGetUniformLocation(BasicShader.ID, "tColor");
                idUniformColor = GLES.glGetUniformLocation(BasicShader.ID, "Color");
                idUniformMat_View = GLES.glGetUniformLocation(BasicShader.ID, "view");
                idUniformMat_Per = GLES.glGetUniformLocation(BasicShader.ID, "perspective");
                idUniformMat_Tra = GLES.glGetUniformLocation(BasicShader.ID, "trasform");
                idUniformSilhouette = GLES.glGetUniformLocation(BasicShader.ID, "Silhouette");
                idUniformTexturePassed = GLES.glGetUniformLocation(BasicShader.ID, "TexturePassed");
                idUniformGlobalLightColor = GLES.glGetUniformLocation(BasicShader.ID, "GlobalLightColor");

                this.InitLightShader();

                DefineTransparentColor(new Color4(0f, 0.9f, 0f, 1f));
                DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));
                DefineGlobalLightColor(Color4.White);

                GLES.glEnable(EnableCap.GL_BLEND);
                GLES.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);
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
                GLES.glUniform3f(idUniformTColor, color.R, color.G, color.B);

                BasicShader_L.Use();
                GLES.glUniform3f(idUniformTColor_L, color.R, color.G, color.B);
            }

            /// <sumary>
            /// Method use to define de 4x4 Matrix of View to use in Draw Methods.
            /// </sumary>
            /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
            /// <param name="mat">Matrix 4x4 with de projectión information.</param>
            public void DefineViewMatrix(dgtk.Math.Mat4 mat)
            {
                BasicShader.Use();
                GLES.glUniformMatrix(this.idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE, mat);

                BasicShader_L.Use();
                GLES.glUniformMatrix(this.idUniformMat_View_L, dgtk.OpenGL.Boolean.GL_FALSE, mat);
            }

            /// <sumary>
            /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
            /// </sumary>
            /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
            /// <param name="mat">Matrix 4x4 with de perspective information.</param>
            public void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
            {
                b_invert_y = invert_y;
                this.m4P = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, with, invert_y ? height : y, invert_y ? y : height, 1000f, -1000f);
                BasicShader.Use();
                GLES.glUniformMatrix(this.idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);

                BasicShader_L.Use();
                GLES.glUniformMatrix(this.idUniformMat_Per_L, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            }

            public void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
            {
                this.m4P = m4;
                BasicShader.Use();
                GLES.glUniformMatrix(this.idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);

                BasicShader_L.Use();
                GLES.glUniformMatrix(this.idUniformMat_Per_L, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            }

            public void DefineGlobalLightColor(Color4 lightcolor)
            {
                BasicShader.Use();
                GLES.glUniform4f(this.idUniformGlobalLightColor, lightcolor);

                BasicShader_L.Use();
                GLES.glUniform4f(this.idUniformGlobalLightColor_L, lightcolor);
            }

            public void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.

                BasicShader.Use();
                //GL.glUniform4fv(idUniform_texcoords, 1, new float[]{0f, 0f, 1f, 1f});
                GLES.glUniform2f(idUniform_v_size, width, height);
                GLES.glUniform1i(idUniformSilhouette, 0);
                GLES.glUniform1i(idUniformTexturePassed, 0);
                GLES.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }
            
            public void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width/2f, height/2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
                
                GLES.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader.Use();
                GLES.glUniform4fv(idUniform_texcoords, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
                GLES.glUniform2f(idUniform_v_size, width, height);
                GLES.glUniform1i(idUniformSilhouette, Silhouette);
                GLES.glUniform1i(idUniformTexturePassed, 1);
                GLES.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }

            public void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(rotateX, rotateY));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
                
                GLES.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader.Use();
                GLES.glUniform4fv(idUniform_texcoords, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
                GLES.glUniform2f(idUniform_v_size, width, height);
                GLES.glUniform1i(idUniformSilhouette, Silhouette);
                GLES.glUniform1i(idUniformTexturePassed, 1);
                GLES.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }


            public Color4 TransparentColor
            {
                get 
                { 
                    BasicShader.Use();
                    float[] fret = GLES.glGetUniformfv(BasicShader.ID, idUniformTColor, 4);
                    return new Color4(fret[0], fret[1], fret[2], fret[3]); 
                }
            }

            public dgtk.Math.Mat4 M4P
            {
                get { return this.m4P; }
                set { this.m4P = value; }
            }
        }
    }
}