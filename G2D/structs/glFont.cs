using System;
using System.Collections.Generic;

namespace dge.G2D
{    
    public struct glFont
    {
        internal string s_Name;
        internal float f_MaxFontSize;
        internal float f_borderWidth;
		internal TextureBufferObject TBO_Scan0; // Textura con el relleno de la fuente
		internal TextureBufferObject TBO_Scan1; // Textura con el borde de la fuente
        internal Dictionary<char, glCharacter> d_characters;

        internal glFont(string name, float max_font_size, float borderWidth, char[] charkeys, glCharacter[] charvalues, TextureBufferObject scan0, TextureBufferObject scan1)
        {
            this.s_Name = name;
            this.f_MaxFontSize = max_font_size;
            this.f_borderWidth = borderWidth;
            this.TBO_Scan0 = scan0;
            this.TBO_Scan1 = scan1;
            if (charkeys.Length != charvalues.Length)
            {
                throw new Exception("glFont Constructor Error: The numbar of keys and values is not the same.");
            }
            this.d_characters = new Dictionary<char, glCharacter>();
            for (int i=0;i<charkeys.Length;i++)
            {
                this.d_characters.Add(charkeys[i], charvalues[i]);
            }
        }

        #region PROPERTIES

        public string Name
        {
            get {return this.s_Name;}
        }

        public float MaxFontSize
        {
            get { return this.f_MaxFontSize; }
        }

        public float BorderWidth
        {
            get { return this.f_borderWidth; }
        }

        #endregion
    }
}