using System;

namespace dge.G2D
{
    public struct TextureBufferObject
    {
        public string s_name; // Nombre de la textura
		internal uint ui_width, ui_height;
        internal uint i_ID;
		internal string s_HASHCODE;

        public TextureBufferObject(string name, uint width, uint height,uint id, string hashcode)
        {
            this.s_name = name;
            this.ui_width = width;
            this.ui_height = height;
            this.s_HASHCODE = hashcode;
            this.i_ID = id;
        }

        public uint ID
        {
            get { return this.i_ID; }
        }

        public uint Width
        {
            get { return this.ui_width; }
        }

        public uint Height
        {
            get { return this.ui_height; }
        }

        public string hashcode
        {
            get { return this.s_HASHCODE; }
        }
    }
}