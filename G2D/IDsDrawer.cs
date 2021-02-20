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
        internal static bool inicialized;
        private static bool b_invert_y;
        private static uint VAO; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private static uint VBO; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private static uint EBO; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)
        
        #region Uniforms Ids
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

        public static void Init_IDs_Drawer()
        {
            if (!inicialized)
            {
                dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
                dge.Core2D.PixelBufferObject_Select = GL.glGenBuffer();
                Console.WriteLine("Init_IDs_Drawer 0: "+(ErrorCode)GL.glGetError());

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

                DefineTransparentColor(new Color4(1f, 0f, 1f, 1f));
                DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

                GL.glEnable(EnableCap.GL_BLEND);
                GL.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);
                Console.WriteLine("Init_IDs_Drawer 1: "+(ErrorCode)GL.glGetError());

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
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }

        public static void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE, mat);
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }

        public static void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            b_invert_y = invert_y;
            dgtk.Math.Mat4 m4 = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(0, with, invert_y ? height : 0, invert_y ? 0 : height, -100f, 100f);
            BasicShader.Use();
            GL.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4);
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
        }

        internal static void DrawGL(Color4 color, int x, int y, uint width, uint height, float RotInDegrees)
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
            //Console.WriteLine("IDs DrawGL 0: "+(ErrorCode)GL.glGetError());
        }
        
        internal static void DrawGL(uint tboID, Color4 color, int x, int y, uint width, uint height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette)
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
            //Console.WriteLine("IDs DrawGL 0: "+(ErrorCode)GL.glGetError());
        }
    }
}