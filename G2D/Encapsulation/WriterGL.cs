using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dgtk.OpenGL;
using dgtk.Math;
using dge.GLSL;

namespace dge.G2D
{    
    /// <summary>
	/// Class used for writing Text on screen with OpenGL.
	/// It can contain different fonts with which to write the desired text.
	/// The fonts must be in GLFont format (* .glf).
	/// </summary>
    internal class WriterGL : I_Writer
    {
        internal dgtk.Math.Mat4 m4P; // Para Renicios internos de Perspectiva.
        //private bool b_invert_y;
        private readonly uint VAO; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private readonly uint VBO; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private readonly uint EBO; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los vertices.)

		#region Uniforms Ids
        //private int idUniformTColor; // ID de Uniform de Color transparente.
        private readonly int idUniformColor; // ID de Uniform de color multiplicante.    
        private readonly int idUniform_texcoords; // ID de Uniform de Coordenadas de Textura Dinamicas.
        private readonly int idUniform_v_size; // ID de Uniform de Tamaño de superficie de dibujo.
        private readonly int idUniformMat_View; // ID de Uniform que contiene la matriz de Projección.
        private readonly int idUniformMat_Per; // ID de Uniform que contiene la matriz de Perspectiva.
        private readonly int idUniformMat_Tra; // ID de Uniform que contiene la matriz de Perspectiva.
        private readonly int idUniformBool_AA; // ID de Uniform que define si aplicar AA casero o no.

        #endregion

        private readonly Shader BasicShader;

		//private dgWindow parentWindow;
        public static Dictionary<string, dgFont> Fonts;
		
		internal WriterGL(/*dgWindow parent*/)
		{
			//this.parentWindow = parent;
			if (Fonts == null ) {Fonts = new Dictionary<string, dgFont>();}

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

            BasicShader = new Shader(ShadersSources.BasicWritervs, ShadersSources.BasicWriterfs, false);

            idUniform_texcoords = GL.glGetUniformLocation(BasicShader.ID, "utexcoords");
            idUniform_v_size = GL.glGetUniformLocation(BasicShader.ID, "v_size");
            idUniformColor = GL.glGetUniformLocation(BasicShader.ID, "Color");
            idUniformMat_View = GL.glGetUniformLocation(BasicShader.ID, "view");
            idUniformMat_Per = GL.glGetUniformLocation(BasicShader.ID, "perspective");
            idUniformMat_Tra = GL.glGetUniformLocation(BasicShader.ID, "trasform");
			idUniformBool_AA = GL.glGetUniformLocation(BasicShader.ID, "AA");

            DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));
			AA_OnOff(false); 

            GL.glEnable(EnableCap.GL_BLEND);
            GL.glBlendFunc(BlendingFactor.GL_SRC_ALPHA, BlendingFactor.GL_ONE_MINUS_SRC_ALPHA);
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
        /// Method use to turn On or Turn Off Antialiasing in text.
        /// </sumary>
        /// <remarks>Turn On or Turn Off Antialiasing in text</remarks>
        /// <param name="bool">Turn On?</param>
        public void AA_OnOff(bool SetOn)
        {
            BasicShader.Use();
            GL.glUniform1i(this.idUniformBool_AA, SetOn ? 1 : 0);
        }

        /// <sumary>
        /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de perspective information.</param>
        public void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            //b_invert_y = invert_y;
            this.m4P = dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, with, invert_y ? height : y, invert_y ? y : height, 1000f, -1000f);
            BasicShader.Use();
            GL.glUniformMatrix(this.idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
        }

        public void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            this.m4P = m4;
            BasicShader.Use();
            GL.glUniformMatrix(this.idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
        }

		public void WriteCharGL(uint tboId, dgtk.Graphics.Color4 color, float posx, float posy, float f_width, float f_height, float f_x0, float f_y0, float f_x1, float f_y1)
		{

            dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D(0, new Vector2(f_width/2f, f_height/2f));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(posx, posy, 0)); // Creamos la Matriz de traslación.
            
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            
            this.BasicShader.Use();
            GL.glUniform4fv(idUniform_texcoords, 1, new float[]{f_x0, f_y0, f_x1, f_y1});
            GL.glUniform2f(idUniform_v_size, f_width, f_height);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
            GL.glUniform4f(idUniformColor, color.R, color.G, color.B, color.A);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboId);
            GL.glBindVertexArray(VAO);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
		}

		public dgtk.Math.Mat4 M4P
		{
			get { return this.m4P; }
		}
    }
}