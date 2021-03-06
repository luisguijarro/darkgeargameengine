using System;
using System.Collections.Generic;

namespace dge.G2D
{    
    public struct dgFont
    {
        internal string s_Name;
        internal float f_MaxFontSize;
        internal float f_borderWidth;
        internal float f_spaceWidth;
        internal float f_MaxCharacterHeight;
		public TextureBufferObject TBO_Scan0; // Textura con el relleno de la fuente
		public TextureBufferObject TBO_Scan1; // Textura con el borde de la fuente
        public Dictionary<char, dgCharacter> d_characters;

        internal dgFont(string name, float max_font_size, float borderWidth, float spaceWidth, char[] charkeys, dgCharacter[] charvalues, TextureBufferObject scan0, TextureBufferObject scan1)
        {
            this.s_Name = name;
            this.f_MaxFontSize = max_font_size;
            this.f_borderWidth = borderWidth;
            this.f_spaceWidth = spaceWidth;
            this.f_MaxCharacterHeight = 0;
            this.TBO_Scan0 = scan0;
            this.TBO_Scan1 = scan1;
            if (charkeys.Length != charvalues.Length)
            {
                throw new Exception("glFont Constructor Error: The number of keys and values is not the same.");
            }
            this.d_characters = new Dictionary<char, dgCharacter>();
            for (int i=0;i<charkeys.Length;i++)
            {
                this.d_characters.Add(charkeys[i], charvalues[i]);
            }
        }

        #region PROPERTIES

        public string Name
        {
            set { this.s_Name = value; }
            get {return this.s_Name;}
        }

        public float MaxFontSize
        {
            get { return this.f_MaxFontSize; }
        }

        public float MaxCharacterHeight
        {
            set { this.f_MaxCharacterHeight = value; }
            get { return this.f_MaxCharacterHeight; }
        }

        public float BorderWidth
        {
            get { return this.f_borderWidth; }
        }

        #endregion
    }
}