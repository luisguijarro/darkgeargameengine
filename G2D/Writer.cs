using System;
using System.Collections.Generic;

namespace dge.G2D
{    
    /// <summary>
	/// Class used for writing Text on screen with OpenGL.
	/// It can contain different fonts with which to write the desired text.
	/// The fonts must be in GLFont format (* .glf).
	/// </summary>
    public static class Writer
    {
        public static Dictionary<string, glFont> Fonts;
		public static void InitGLWriter()
		{
			Fonts = new Dictionary<string, glFont>();
		}
		
		/// <summary>
		/// Method used by the writer to load .glf fonts.
		/// </summary>
		/// <remarks>Load glFonts.</remarks>
		/// <param name="filepath">Path of the Font file in glFont format.</param>
		public static void CargarFuente(string filepath)
		{
			glFont ft = Tools.LoadGLFont(filepath);
			Fonts.Add(ft.Name, ft);
		}
    }
}