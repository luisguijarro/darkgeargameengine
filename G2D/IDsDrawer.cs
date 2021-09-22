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
        internal static dgtk.Math.Mat4 m4P;
        internal static bool inicialized;
        private static bool b_invert_y;
        private static uint VAO; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private static uint VBO; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private static uint EBO; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)
        private static uint VAO2; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private static uint VBO2; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private static uint EBO2; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)
        
        #region Uniforms Ids BasicShader
        private static int idUniformTColor; // ID de Uniform de Color transparente.
        private static int idUniformColor; // ID de Uniform de color multiplicante.    
        private static int idUniform_texcoords; // ID de Uniform de Coordenadas de Textura Dinamicas.
        private static int idUniform_v_size; // ID de Uniform de Tamaño de superficie de dibujo.
        private static int idUniformMat_View; // ID de Uniform que contiene la matriz de Projección.
        private static int idUniformMat_Per; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniformMat_Tra; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniformSilhouette; // ID de Uniform que contiene la matriz de Transformación.
        private static int idUniformTexturePassed; // Id de Uniform que indica si se está pasando textura o no.

        #endregion

        private static Shader BasicShader;

        #region Uniforms Ids BasicGuiShader
        private static int idUniformTColor2; // ID de Uniform de Color transparente.
        private static int idUniformColor2; // ID de Uniform de color multiplicante.    
        private static int idUniform_texcoords2; // ID de Uniform de Coordenadas de Textura Dinamicas.
        private static int idUniform_v_size2; // ID de Uniform de Tamaño de superficie de dibujo.
        private static int idUniformMat_View2; // ID de Uniform que contiene la matriz de Projección.
        private static int idUniformMat_Per2; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniformMat_Tra2; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniformSilhouette2; // ID de Uniform que contiene la matriz de Transformación.
        private static int idUniformTexturePassed2; // Id de Uniform que indica si se está pasando textura o no.
        private static int idUniformtcDisplacement2; // Desplazamiento de coordenadas de textura en caso de evento.
        private static int idUniform_MarginsFromTheEdge2; // margenes en pixeles desde el borde, para posición de vertices internos.

        #endregion

        private static Shader BasicGuiShader;

        public static void Init_IDs_Drawer()
        {
            if (!inicialized)
            {
                dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
                dgtk.OpenGL.GL.glEnable(dgtk.OpenGL.EnableCap.GL_VERTEX_ARRAY);
                dgtk.OpenGL.GL.glEnableClientState(dgtk.OpenGL.EnableCap.GL_VERTEX_ARRAY);
                dgtk.OpenGL.GL.glEnable(dgtk.OpenGL.EnableCap.GL_TEXTURE_COORD_ARRAY);
                dgtk.OpenGL.GL.glEnableClientState(dgtk.OpenGL.EnableCap.GL_TEXTURE_COORD_ARRAY);
                dge.Core2D.PixelBufferObject_Select = GL.glGenBuffer();
                //Console.WriteLine("Init_IDs_Drawer 0: "+(ErrorCode)GL.glGetError());

                #region VAO 1

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

                #endregion VAO 1
                
                #region VAO 2

                TVertex2D[] vertices2 = new TVertex2D[16]
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

                UInt32[] indices2 = new uint[]
                {
                    00, 01, 05, 04, 00, 05,    04, 05, 09, 08, 04, 09,    08, 09, 13, 12, 08, 13, //Fila 1 de Dobles Triangulos
                    01, 02, 06, 05, 01, 06,    05, 06, 10, 09, 05, 10,    09, 10, 14, 13, 09, 14, //Fila 2 de Dobles Triangulos
                    02, 03, 07, 06, 02, 07,    06, 07, 11, 10, 06, 11,    10, 11, 15, 14, 10, 15  //Fila 3 de Dobles Triangulos
                };

                VAO2 = GL.glGenVertexArray(); 
                VBO2 = GL.glGenBuffer();
                EBO2 = GL.glGenBuffer();

                GL.glBindVertexArray(VAO2);
                GL.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO2);
                GL.glBufferData<TVertex2D>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertex2D))*16, vertices2, BufferUsageARB.GL_STATIC_DRAW);

                GL.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO2);
                GL.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indices2.Length, indices2, BufferUsageARB.GL_STATIC_DRAW);
                
                GL.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(0)); // ID
                GL.glEnableVertexAttribArray(0);

                GL.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr(sizeof(int))); // Vertex Position
                GL.glEnableVertexAttribArray(1);

                GL.glVertexAttribPointer(2, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertex2D)), new IntPtr((sizeof(float)*2)+sizeof(int)/*Vector2 de posicion tiene dos float +  el uint de ID*/)); // Texcoords.
                GL.glEnableVertexAttribArray(2);

                GL.glBindVertexArray(0);

                #endregion

                #region BasicShader

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

                #endregion BasicShader

                #region BasicGuiShader

                BasicGuiShader = new Shader(ShadersSources.BasicGUIvs, ShadersSources.BasicGUIfs);

                idUniform_texcoords2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "utexcoords");
                idUniform_MarginsFromTheEdge2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "MarginsFromTheEdge");
                idUniform_v_size2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "v_size");
                idUniformTColor2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "tColor");
                idUniformColor2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "Color");
                idUniformMat_View2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "view");
                idUniformMat_Per2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "perspective");
                idUniformMat_Tra2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "trasform");
                idUniformSilhouette2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "Silhouette");
                idUniformTexturePassed2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "TexturePassed");
                idUniformtcDisplacement2 = GL.glGetUniformLocation(BasicGuiShader.ui_id, "tcDisplacement");

                #endregion

                DefineTransparentColor(new Color4(1f, 0f, 1f, 1f));
                DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

                GL.glEnable(EnableCap.GL_BLEND);
                GL.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);
                //Console.WriteLine("Init_IDs_Drawer 1: "+(ErrorCode)GL.glGetError());

                dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
                //GL.glEnable(EnableCap.GL_TEXTURE_2D);
                inicialized = true;
            }
            
        }

        public static void DefineTransparentColor(Color4 color)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            BasicShader.Use();
            GL.glUniform3f(idUniformTColor, color.R, color.G, color.B);
            BasicGuiShader.Use();
            GL.glUniform3f(idUniformTColor2, color.R, color.G, color.B);
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }

        public static void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE, mat);
            BasicGuiShader.Use();
            GL.glUniformMatrix(idUniformMat_View2, dgtk.OpenGL.Boolean.GL_FALSE, mat);
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }


        internal static void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            //dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            m4P = m4;
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            BasicGuiShader.Use();
            GL.glUniformMatrix(idUniformMat_Per2, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            //dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }

        public static void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            b_invert_y = invert_y;
            m4P = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, with, invert_y ? height : y, invert_y ? y : height, -100f, 100f);
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            BasicGuiShader.Use();
            GL.glUniformMatrix(idUniformMat_Per2, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            //dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }

        #region Draw2D

        public static void DrawGL2D(Color4 color, int x, int y, int width, int height, float RotInDegrees)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.

            BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 1, new float[]{0f, 0f, 1f, 1f});
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
        
        public static void DrawGL2D(Color4 color, int x, int y, int width, int height, int rotateX, int rotateY, float RotInDegrees)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(rotateX, rotateY));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.

            BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 1, new float[]{0f, 0f, 1f, 1f});
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
        
        public static void DrawGL2D(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
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

        #endregion Draw2D

        #region DrawGui

        public static void DrawGuiGL(uint tboID, Color4 color, int x, int y, int width, int height, float RotInDegrees, int[/*4*/]MarginsFromTheEdge, float[/*8*/]TexCoords, float[/*2*/]v2_CoordVariation, int Silhouette)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((-RotInDegrees), new Vector2(width/2f, height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            BasicGuiShader.Use();
            GL.glUniform4fv(idUniform_texcoords2, 2, TexCoords);
            GL.glUniform2fv(idUniformtcDisplacement2, 1, v2_CoordVariation);
            GL.glUniform4iv(idUniform_MarginsFromTheEdge2, 1, MarginsFromTheEdge);
            GL.glUniform2f(idUniform_v_size2, width, height);
            GL.glUniform1i(idUniformSilhouette2, Silhouette);
            GL.glUniform1i(idUniformTexturePassed2, 1);
            GL.glUniformMatrix(idUniformMat_Tra2, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor2, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
            GL.glBindVertexArray(VAO2);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 54, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        public static void DrawGuiGL(Color4 color, int x, int y, int width, int height, float RotInDegrees)
        {
            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((-RotInDegrees), new Vector2(width/2f, height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);

            float[] TexCoords = new float[]
            {
                0f, 0f, 1f, 1f, 
                0f, 0f, 1f, 1f
            };
            
            BasicGuiShader.Use();
            GL.glUniform4fv(idUniform_texcoords2, 2, TexCoords);
            GL.glUniform2fv(idUniformtcDisplacement2, 1, new float[]{0,0});
            GL.glUniform4iv(idUniform_MarginsFromTheEdge2, 1, new int[]{0,0,0,0});
            GL.glUniform2f(idUniform_v_size2, width, height);
            GL.glUniform1i(idUniformSilhouette2, 0);
            GL.glUniform1i(idUniformTexturePassed2, 1);
            GL.glUniformMatrix(idUniformMat_Tra2, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor2, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
            GL.glBindVertexArray(VAO2);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 54, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        #endregion DrawGui
    }
}