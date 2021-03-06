using System;

namespace dge.G2D
{    
    public struct dgCharacter
    {
        public float f_x0, f_y0, f_x1, f_y1;
        public short i_width, i_height;
		public float f_width;
		public char ch_character;
        public dgCharacter(char character, float x0, float y0, float x1, float y1, short width, short height, float charwidth)
        {
            this.ch_character = character;
            this.f_x0 = x0;
            this.f_y0 = y0;
            this.f_x1 = x1;
            this.f_y1 = y1;
            this.i_width = width;
            this.i_height = height;
            this.f_width = charwidth;
        }
    }
}