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
    public class Writer
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

        #endregion

        private readonly Shader BasicShader;

		//private dgWindow parentWindow;
        public static Dictionary<string, dgFont> Fonts;
		
		internal Writer(/*dgWindow parent*/)
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

            BasicShader = new Shader(ShadersSources.BasicWritervs, ShadersSources.BasicWriterfs);

            idUniform_texcoords = GL.glGetUniformLocation(BasicShader.ui_id, "utexcoords");
            idUniform_v_size = GL.glGetUniformLocation(BasicShader.ui_id, "v_size");
            idUniformColor = GL.glGetUniformLocation(BasicShader.ui_id, "Color");
            idUniformMat_View = GL.glGetUniformLocation(BasicShader.ui_id, "view");
            idUniformMat_Per = GL.glGetUniformLocation(BasicShader.ui_id, "perspective");
            idUniformMat_Tra = GL.glGetUniformLocation(BasicShader.ui_id, "trasform");

            DefineViewMatrix(dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

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
        /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de perspective information.</param>
        public void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            //b_invert_y = invert_y;
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


		/// <summary>
		/// Method used by the writer to load .dgf file fonts.
		/// </summary>
		/// <remarks>Load dgFonts.</remarks>
		/// <param name="filepath">Path of the Font file in dgFont format.</param>
		public static string LoadFont(string filepath)
		{
			if (Fonts == null ) {Fonts = new Dictionary<string, dgFont>();}
			dgFont ft = Tools.LoadDGFont(filepath);
			Fonts.Add(ft.Name, ft);
			return ft.Name; // Devolvemos nombre de fuente cargada.
		}

		/// <summary>
		/// Method used by the writer to write text in screen.
		/// </summary>
		/// <remarks>Write Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Write text.</param>
		/// <param name="color">Sets the color in which the text will be written.</param>
		/// <param name="text">Text to Write.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="posx">X coord of Origin of the text.</param>
		/// <param name="posy">Y coord of origin of the text.</param>
		public void Write(string fontname, dgtk.Graphics.Color4 color, string text, float fsize, float posx, float posy)
		{
			this.Write(Fonts[fontname], color, text, fsize, posx, posy);
		}

		/// <summary>
		/// Method used by the writer to write text in screen.
		/// </summary>
		/// <remarks>Write Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Write text.</param>
		/// <param name="color">Sets the color in which the text will be written.</param>
		/// <param name="text">Text to Write.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="posx">X coord of Origin of the text.</param>
		/// <param name="posy">Y coord of origin of the text.</param>
		/// <param name="lineWidth">Max line width of the text.</param>
		public void Write(string fontname, dgtk.Graphics.Color4 color, string text, float fsize, float posx, float posy, float lineWidth)
		{
			this.Write(Fonts[fontname], color, text, fsize, posx, posy, lineWidth);
		}

		/// <summary>
		/// Method used by the writer to write text in screen.
		/// </summary>
		/// <remarks>Write Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Write text.</param>
		/// <param name="color">Sets the color in which the text will be written.</param>
		/// <param name="text">Text to Write.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="posx">X coord of Origin of the text.</param>
		/// <param name="posy">Y coord of origin of the text.</param>
		public void Write(dgFont font, dgtk.Graphics.Color4 color, string text, float fsize, float posx, float posy)
		{
			float actualpos = posx;
			for (int i=0;i<text.Length;i++)
			{
				if (text[i] == ' ')
				{
					actualpos += font.f_spaceWidth*(fsize/font.MaxFontSize);
				}
				else if (!font.d_characters.ContainsKey(text[i]))
				{
					actualpos += font.f_spaceWidth*(fsize/font.MaxFontSize);
				}
				else
				{
					actualpos += WriteChar(font, color, text[i], fsize, actualpos, posy);
				}
			}
		}

		/// <summary>
		/// Method used by the writer to write text in screen.
		/// </summary>
		/// <remarks>Write Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Write text.</param>
		/// <param name="color">Sets the color in which the text will be written.</param>
		/// <param name="text">Text to Write.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="posx">X coord of Origin of the text.</param>
		/// <param name="posy">Y coord of origin of the text.</param>
		/// <param name="lineWidth">Max line width of the text.</param>
		public void Write(dgFont font, dgtk.Graphics.Color4 color, string text, float fsize, float posx, float posy, float lineWidth)
		{
			//Console.WriteLine("Escribir en ancho: "+text);
			string[] s_words = text.Split(' ');

			float tmp_linewidth = 0;
			float actualpos = posx;
			int n_lines = 0;
			for (int w=0;w<s_words.Length;w++)
			{
				if (s_words[w].Length>0)
				{
					string s_word = s_words[w];

					float[] word_size = MeasureString(font, s_word, fsize);
					if ((tmp_linewidth + word_size[0]) > lineWidth)
					{
						tmp_linewidth = word_size[0] + (font.f_spaceWidth*(fsize/font.MaxFontSize));
						actualpos = posx;
						n_lines++;
					}
					else
					{
						tmp_linewidth += word_size[0] + (font.f_spaceWidth*(fsize/font.MaxFontSize)); // Palabra + Espacio.
					}			
					
					for (int i=0;i<s_word.Length;i++)
					{
						if (font.d_characters.ContainsKey(s_word[i]))
						{
							actualpos += WriteChar(font, color, s_word[i], fsize, actualpos, posy + ((font.MaxCharacterHeight*(fsize/font.MaxFontSize))*n_lines));
						}
					}
				}
				if (w<s_words.Length-1) // añadimos espacio si no es la ultima palabra.
				{ 
					actualpos += font.f_spaceWidth*(fsize/font.MaxFontSize); 
				}
			}
		}

		/// <summary>
		/// Method used by the writer to write text in screen with Border.
		/// </summary>
		/// <remarks>Write Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Write text.</param>
		/// <param name="color">Sets the color in which the text will be written.</param>
		/// <param name="text">Text to Write.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="posx">X coord of Origin of the text.</param>
		/// <param name="posy">Y coord of origin of the text.</param>
		/// <param name="BorderColor">Sets the color in which the border will be drawn.</param>
		public void Write(string fontname, dgtk.Graphics.Color4 color, string text, float fsize, float posx, float posy, dgtk.Graphics.Color4 BorderColor)
		{
			this.Write(Fonts[fontname], color, text, fsize, posx, posy, BorderColor);
		}

		/// <summary>
		/// Method used by the writer to write text in screen with Border.
		/// </summary>
		/// <remarks>Write Text.</remarks>
		/// <param name="font">Font to Use when Write text.</param>
		/// <param name="color">Sets the color in which the text will be written.</param>
		/// <param name="text">Text to Write.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="posx">X coord of Origin of the text.</param>
		/// <param name="posy">Y coord of origin of the text.</param>
		/// <param name="BorderColor">Sets the color in which the border will be drawn.</param>
		public void Write(dgFont font, dgtk.Graphics.Color4 color, string text, float fsize, float posx, float posy, dgtk.Graphics.Color4 BorderColor)
		{
			float actualpos = posx;
			for (int i=0;i<text.Length;i++)
			{
				if (text[i] == ' ')
				{
					actualpos += font.f_spaceWidth*(fsize/font.MaxFontSize);
				}
				else if (!font.d_characters.ContainsKey(text[i]))
				{
					actualpos += 0; //font.f_spaceWidth*(fsize/font.MaxFontSize);
				}
				else
				{
					float w = WriteChar(font, color, text[i], fsize, actualpos, posy);
					WriteCharBorder(font, BorderColor, text[i], fsize, actualpos, posy);
					actualpos += w;
				}
			}
		}

		/// <summary>
		/// Method used by the writer to know de Horizontal Size o a text.
		/// </summary>
		/// <remarks>Measure Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Measure text.</param>
		/// <param name="text">Text to Measure.</param>
		/// <param name="fontsize">Size of the font in pixels.</param>
		public static float[] MeasureString(string fontname, string text, float fontsize)
		{
			dgFont font = Fonts[fontname];			
			return MeasureString(font, text, fontsize);
		}


		/// <summary>
		/// Method used by the writer to know de Horizontal Size o a text.
		/// </summary>
		/// <remarks>Measure Text.</remarks>
		/// <param name="fontname">Name of de Font to Use when Measure text.</param>
		/// <param name="text">Text to Measure.</param>
		/// <param name="fontsize">Size of the font in pixels.</param>
		/// <param name="linewidth">Max length for line.</param>
		public static float[] MeasureString(string fontname, string text, float fontsize, float lineWidth)
		{
			dgFont font = Fonts[fontname];			
			return MeasureString(font, text, fontsize, lineWidth);
		}

		/// <summary>
		/// Method used by the writer to know de Horizontal Size o a text.
		/// </summary>
		/// <remarks>Measure Text.</remarks>
		/// <param name="font">Font to Use when Measure text.</param>
		/// <param name="text">Text to Measure.</param>
		/// <param name="size">Size of the font in pixels.</param>
		public static float[] MeasureString(dgFont font, string text, float fontsize)
		{
			float[] retSize = new float[]{0f,0f};
			
			for (int i=0;i<text.Length;i++)
			{
				//if (font.d_characters.ContainsKey(text[i]))
				//{
					
					if (text[i] == ' ')
					{
						retSize[0] += font.f_spaceWidth*(fontsize/font.MaxFontSize);
					}
					else if (!font.d_characters.ContainsKey(text[i]))
					{
						retSize[0] += 0; //font.f_spaceWidth*(fsize/font.MaxFontSize);
					}
					else
					{
						dgCharacter ch = font.d_characters[text[i]];
						retSize[0] += ch.ui_width/*f_width*/*(fontsize/font.MaxFontSize);
					}
				//}
			}
			retSize[1] = font.MaxCharacterHeight*(fontsize/font.MaxFontSize);
			return retSize;
		}

		/// <summary>
		/// Method used by the writer to know de Horizontal Size o a text.
		/// </summary>
		/// <remarks>Measure Text.</remarks>
		/// <param name="font">Font to Use when Measure text.</param>
		/// <param name="text">Text to Measure.</param>
		/// <param name="size">Size of the font in pixels.</param>
		/// <param name="linewidth">Max length for line.</param>
		public static float[] MeasureString(dgFont font, string text, float fontsize, float lineWidth)
		{
			//float retSize = 0;
			
			string[] s_words = text.Split(' ');

			float tmp_linewidth = 0;
			int n_lines = 1;
			//float maxheight = 0f;
			for (int w=0;w<s_words.Length;w++)
			{
				if (s_words[w].Length>0)
				{
					float[] word_size = MeasureString(font.Name, s_words[w], fontsize);
					if ((tmp_linewidth + word_size[0]) > lineWidth)
					{
						tmp_linewidth = word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize));
						n_lines++;
					}
					else
					{
						tmp_linewidth += word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize)); // Palabra + Espacio.
					}	
					//maxheight = (maxheight < word_size[1]) ? word_size[1] : maxheight;
				}
			}
			
			return new float[]{lineWidth, (n_lines)*(font.MaxCharacterHeight*(fontsize/font.MaxFontSize))};
		}

		private float WriteChar(dgFont font, dgtk.Graphics.Color4 color, char character, float fontsize, float posx, float posy) // Retornamos ancho dle caracter.
		{
			dgCharacter ch = font.d_characters[character];
			//PINTAR:
			
			float f_width = ((float)ch.ui_width*(fontsize/font.MaxFontSize));
			float f_height = ((float)ch.ui_height*(fontsize/font.MaxFontSize));
			this.WriteCharGL(font.TBO_Scan0.ui_ID, color, posx, posy, f_width, f_height, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
			
			return ch.f_width*(fontsize/font.MaxFontSize);
		}

		private void WriteCharBorder(dgFont font, dgtk.Graphics.Color4 color, char character, float fontsize, float posx, float posy) // Retornamos ancho dle caracter.
		{
			dgCharacter ch = font.d_characters[character];
			//PINTAR:
			//this.parentWindow.Drawer2D.Draw(font.TBO_Scan1.ID, color, (int)posx, (int)posy, (uint)(ch.ui_width*(fontsize/font.MaxFontSize)), (uint)(ch.ui_height*(fontsize/font.MaxFontSize)), 0f, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
			float f_width = ((float)ch.ui_width*(fontsize/font.MaxFontSize));
			float f_height = ((float)ch.ui_height*(fontsize/font.MaxFontSize));
			this.WriteCharGL(font.TBO_Scan1.ui_ID, color, posx, posy, f_width, f_height, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
		}

		private void WriteCharGL(uint tboId, dgtk.Graphics.Color4 color, float posx, float posy, float f_width, float f_height, float f_x0, float f_y0, float f_x1, float f_y1)
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
    }
}