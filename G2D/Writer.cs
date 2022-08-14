using System;
using System.Collections.Generic;

namespace dge.G2D
{    
    /// <summary>
	/// Class used for writing Text on screen with OpenGL.
	/// It can contain different fonts with which to write the desired text.
	/// The fonts must be in GLFont format (* .glf).
	/// </summary>
    public partial class Writer
    {
		private I_Writer Instance;

		//private dgWindow parentWindow;
        public static Dictionary<string, dgFont> Fonts;
		
		internal Writer(/*dgWindow parent*/bool isGLES)
		{
			if (isGLES)
			{
				this.Instance = new WriterGLES();
			}
			else
			{
				this.Instance = new WriterGL();
			}
			//this.parentWindow = parent;
			if (Fonts == null ) {Fonts = new Dictionary<string, dgFont>();}
		}
		

        /// <sumary>
        /// Method use to define de 4x4 Matrix of View to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de projectión information.</param>
        public void DefineViewMatrix(dgtk.Math.Mat4 mat)
        {
            Instance.DefineViewMatrix(mat);
        }

        /// <sumary>
        /// Method use to turn On or Turn Off Antialiasing in text.
        /// </sumary>
        /// <remarks>Turn On or Turn Off Antialiasing in text</remarks>
        /// <param name="bool">Turn On?</param>
        public void AA_OnOff(bool SetOn)
        {
            Instance.AA_OnOff(SetOn);
        }

        /// <sumary>
        /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de perspective information.</param>
        public void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y)
        {
            Instance.DefinePerspectiveMatrix(x, y, with, height, invert_y);
        }

        internal void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4)
        {
            Instance.DefinePerspectiveMatrix(m4);
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
		public void Write(dgFont font, dgtk.Graphics.Color4 color, string text, float fontsize, float posx, float posy, float lineWidth)
		{
			string[] s_words = text.Split(' ');

			float tmp_linewidth = 0;
			int n_lines = 0;
			float tmp_posx = posx;
			for (int w=0;w<s_words.Length;w++)
			{
				if (s_words[w].Length>0)
				{
					float[] word_size = MeasureString(font/*.Name*/, s_words[w], fontsize);
					if (word_size[0] > lineWidth)
					{
						//METER CÓDIGO de division de palabras largas.					
						if (tmp_linewidth > 0)
						{
							tmp_posx = posx;
							tmp_linewidth = 0;
							n_lines++;
						}
						for (int i=0;i<s_words[w].Length;i++)
						{							
							dgCharacter ch = font.d_characters[s_words[w][i]];
							if (tmp_linewidth + ch.f_width*(fontsize/font.MaxFontSize) > lineWidth)
							{
								tmp_posx = posx;
								tmp_linewidth = 0;
								n_lines++;
							}
							WriteChar(font, color, ch.ch_character, fontsize, tmp_posx, posy + (int)(((font.MaxCharacterHeight/fontsize)*1.5f)*n_lines));
							tmp_posx += ch.f_width*(fontsize/font.MaxFontSize);
							tmp_linewidth += ch.f_width*(fontsize/font.MaxFontSize);
						}
						
					}
					else
					{
						if ((tmp_linewidth + word_size[0]) > lineWidth)
						{
							tmp_linewidth = word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize));
							tmp_posx = posx;
							n_lines++;
						}
						else
						{
							tmp_linewidth += word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize)); // Palabra + Espacio.
						}
						for (int i=0;i<s_words[w].Length;i++)
						{
							dgCharacter ch = font.d_characters[s_words[w][i]];
							float increment = ch.f_width*(fontsize/font.MaxFontSize);
							WriteChar(font, color, ch.ch_character, fontsize, tmp_posx, posy + (int)(((font.MaxCharacterHeight/fontsize)*1.5f)*n_lines));
							tmp_posx += increment;
						}
					}
					if (w<s_words.Length-1) // añadimos espacio si no es la ultima palabra.
					{ 
						tmp_posx += font.f_spaceWidth*(fontsize/font.MaxFontSize); 
					}
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
		/// <param name="lineWidth">Max line width of the text.</param>
		public void Write(dgFont font, dgtk.Graphics.Color4 color, string text, float fontsize, float posx, float posy, dgtk.Graphics.Color4 BorderColor, float lineWidth)
		{
			string[] s_words = text.Split(' ');

			float tmp_linewidth = 0;
			int n_lines = 0;
			float tmp_posx = posx;
			for (int w=0;w<s_words.Length;w++)
			{
				if (s_words[w].Length>0)
				{
					float[] word_size = MeasureString(font.Name, s_words[w], fontsize);
					if (word_size[0] > lineWidth)
					{
						//METER CÓDIGO de division de palabras largas.					
						if (tmp_linewidth > 0)
						{
							tmp_posx = posx;
							tmp_linewidth = 0;
							n_lines++;
						}
						for (int i=0;i<s_words[w].Length;i++)
						{							
							dgCharacter ch = font.d_characters[s_words[w][i]];
							if (tmp_linewidth + ch.f_width*(fontsize/font.MaxFontSize) > lineWidth)
							{
								tmp_posx = posx;
								tmp_linewidth = 0;
								n_lines++;
							}
							WriteChar(font, color, ch.ch_character, fontsize, tmp_posx, posy + (int)(((font.MaxCharacterHeight/fontsize)*1.5f)*n_lines));
							WriteCharBorder(font, BorderColor, ch.ch_character, fontsize, tmp_posx, posy + (int)(((font.MaxCharacterHeight/fontsize)*1.5f)*n_lines));
							tmp_posx += ch.f_width*(fontsize/font.MaxFontSize);
							tmp_linewidth += ch.f_width*(fontsize/font.MaxFontSize);
						}
						
					}
					else
					{
						if ((tmp_linewidth + word_size[0]) > lineWidth)
						{
							tmp_linewidth = word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize));
							tmp_posx = posx;
							n_lines++;
						}
						else
						{
							tmp_linewidth += word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize)); // Palabra + Espacio.
						}
						for (int i=0;i<s_words[w].Length;i++)
						{
							dgCharacter ch = font.d_characters[s_words[w][i]];
							float increment = ch.f_width*(fontsize/font.MaxFontSize);
							WriteChar(font, color, ch.ch_character, fontsize, tmp_posx, posy + (int)(((font.MaxCharacterHeight/fontsize)*1.5f)*n_lines));
							WriteCharBorder(font, BorderColor, ch.ch_character, fontsize, tmp_posx, posy + (int)(((font.MaxCharacterHeight/fontsize)*1.5f)*n_lines));
							tmp_posx += increment;
						}
					}
					if (w<s_words.Length-1) // añadimos espacio si no es la ultima palabra.
					{ 
						tmp_posx += font.f_spaceWidth*(fontsize/font.MaxFontSize); 
					}
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
						retSize[0] += ch.i_width/*f_width*/*(fontsize/font.MaxFontSize);
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
			float tmp_posx = 0;
			int n_lines = 1;
			//float maxheight = 0f;
			for (int w=0;w<s_words.Length;w++)
			{
				if (s_words[w].Length>0)
				{
					float[] word_size = MeasureString(font.Name, s_words[w], fontsize);
					if (word_size[0] > lineWidth)
					{
						//METER CÓDIGO de division de palabras largas.					
						if (tmp_linewidth > 0)
						{
							tmp_posx = 0;
							tmp_linewidth = 0;
							n_lines++;
						}
						for (int i=0;i<s_words[w].Length;i++)
						{							
							dgCharacter ch = font.d_characters[s_words[w][i]];
							if (tmp_linewidth + ch.f_width*(fontsize/font.MaxFontSize) > lineWidth)
							{
								tmp_posx = 0;
								tmp_linewidth = 0;
								n_lines++;
							}
							tmp_posx += ch.f_width*(fontsize/font.MaxFontSize);
							tmp_linewidth += ch.f_width*(fontsize/font.MaxFontSize);
						}
						/*
						float tmp_posx = tmp_linewidth;
						for (int i=0;i<s_words[w].Length;i++)
						{
							dgCharacter ch = font.d_characters[s_words[w][i]];
							if (tmp_posx + ch.f_width*(fontsize/font.MaxFontSize) > lineWidth)
							{
								tmp_posx = 0;
								n_lines++;
							}
							tmp_posx += ch.f_width*(fontsize/font.MaxFontSize);
						}
						tmp_linewidth = tmp_posx;
						*/
					}
					else
					{
						if ((tmp_linewidth + word_size[0]) > lineWidth)
						{
							tmp_linewidth = word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize));
							n_lines++;
						}
						else
						{
							tmp_linewidth += word_size[0] + (font.f_spaceWidth*(fontsize/font.MaxFontSize)); // Palabra + Espacio.
						}	
					}

					
					//maxheight = (maxheight < word_size[1]) ? word_size[1] : maxheight;
				}
			}
			
			return new float[]{lineWidth, n_lines*((font.MaxCharacterHeight/fontsize)*1.5f)};
		}

		private float WriteChar(dgFont font, dgtk.Graphics.Color4 color, char character, float fontsize, float posx, float posy) // Retornamos ancho dle caracter.
		{
			dgCharacter ch = font.d_characters[character];
			//PINTAR:
			
			float f_width = (float)ch.i_width*(fontsize/font.MaxFontSize);
			float f_height = (float)ch.i_height*(fontsize/font.MaxFontSize);
			this.WriteCharGL(font.TBO_Scan0.ui_ID, color, posx, posy, f_width, f_height, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
			
			return ch.f_width*(fontsize/font.MaxFontSize);
		}

		private void WriteCharBorder(dgFont font, dgtk.Graphics.Color4 color, char character, float fontsize, float posx, float posy) // Retornamos ancho dle caracter.
		{
			dgCharacter ch = font.d_characters[character];
			//PINTAR:
			//this.parentWindow.Drawer2D.Draw(font.TBO_Scan1.ID, color, (int)posx, (int)posy, (uint)(ch.ui_width*(fontsize/font.MaxFontSize)), (uint)(ch.ui_height*(fontsize/font.MaxFontSize)), 0f, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
			float f_width = (float)ch.i_width*(fontsize/font.MaxFontSize);
			float f_height = (float)ch.i_height*(fontsize/font.MaxFontSize);
			this.WriteCharGL(font.TBO_Scan1.ui_ID, color, posx, posy, f_width, f_height, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
		}

		private void WriteCharGL(uint tboId, dgtk.Graphics.Color4 color, float posx, float posy, float f_width, float f_height, float f_x0, float f_y0, float f_x1, float f_y1)
		{
			Instance.WriteCharGL(tboId, color, posx, posy, f_width, f_height, f_x0, f_y0, f_x1, f_y1);
		}

		public dgtk.Math.Mat4 m4P
		{
			get { return this.Instance.M4P; }
		}
    }
}