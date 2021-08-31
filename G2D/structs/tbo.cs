using System;

namespace dge.G2D
{
    public struct TextureBufferObject
    {
        public string s_name; // Nombre de la textura
		internal int i_width, i_height;
        internal uint ui_ID;
		internal string s_HASHCODE;

        public TextureBufferObject(string name, int width, int height, uint id, string hashcode)
        {
            this.s_name = name;
            this.i_width = width;
            this.i_height = height;
            this.s_HASHCODE = hashcode;
            this.ui_ID = id;
        }

        public uint ID
        {
            get { return this.ui_ID; }
        }

        public int Width
        {
            get { return this.i_width; }
        }

        public int Height
        {
            get { return this.i_height; }
        }

        public string hashcode
        {
            get { return this.s_HASHCODE; }
        }

        public static TextureBufferObject Null
        {
            get
            {
                return new TextureBufferObject("", 2, 2, 0, "");
            }
        }
    }
}
