using System;
using System.Collections.Generic;

namespace dge.G2D
{    
    /// <summary>
	/// Class used for writing Text on screen with OpenGL.
	/// It can contain different fonts with which to write the desired text.
	/// The fonts must be in GLFont format (* .glf).
	/// </summary>
    public class Writer
    {
		private dgWindow parentWindow;
        public static Dictionary<string, dgFont> Fonts;
		internal Writer(dgWindow parent)
		{
			this.parentWindow = parent;
			if (Fonts == null ) {Fonts = new Dictionary<string, dgFont>();}
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
		/// <param name="size">Size of the font in pixels.</param>
		public float MeasureString(string fontname, string text, float fsize)
		{
			dgFont font = Fonts[fontname];			
			return this.MeasureString(font, text, fsize);
		}

		/// <summary>
		/// Method used by the writer to know de Horizontal Size o a text.
		/// </summary>
		/// <remarks>Measure Text.</remarks>
		/// <param name="font">Font to Use when Measure text.</param>
		/// <param name="text">Text to Measure.</param>
		/// <param name="size">Size of the font in pixels.</param>
		public float MeasureString(dgFont font, string text, float fsize)
		{
			float retSize = 0;
			
			for (int i=0;i<text.Length;i++)
			{
				dgCharacter ch = font.d_characters[text[i]];
				if (text[i] == ' ')
				{
					retSize += font.f_spaceWidth*(fsize/font.MaxFontSize);
				}
				else if (!font.d_characters.ContainsKey(text[i]))
				{
					retSize += 0; //font.f_spaceWidth*(fsize/font.MaxFontSize);
				}
				else
				{
					retSize += ch.ui_width*(fsize/font.MaxFontSize);
				}
			}
			return retSize;
		}

		private float WriteChar(dgFont font, dgtk.Graphics.Color4 color, char character, float fontsize, float posx, float posy) // Retornamos ancho dle caracter.
		{
			dgCharacter ch = font.d_characters[character];
			//PINTAR:
			this.parentWindow.Drawer2D.Draw(font.TBO_Scan0.ID, color, (int)posx, (int)posy, (uint)(ch.ui_width*(fontsize/font.MaxFontSize)), (uint)(ch.ui_height*(fontsize/font.MaxFontSize)), 0f, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
			return ch.ui_width*(fontsize/font.MaxFontSize);
		}

		private void WriteCharBorder(dgFont font, dgtk.Graphics.Color4 color, char character, float fontsize, float posx, float posy) // Retornamos ancho dle caracter.
		{
			dgCharacter ch = font.d_characters[character];
			//PINTAR:
			this.parentWindow.Drawer2D.Draw(font.TBO_Scan1.ID, color, (int)posx, (int)posy, (uint)(ch.ui_width*(fontsize/font.MaxFontSize)), (uint)(ch.ui_height*(fontsize/font.MaxFontSize)), 0f, ch.f_x0, ch.f_y0, ch.f_x1, ch.f_y1);
		}
    }
}