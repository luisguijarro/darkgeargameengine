using System;

namespace dge.G2D
{    
    internal struct dgCharacter
    {
        internal float f_x0, f_y0, f_x1, f_y1;
        internal ushort ui_width, ui_height;
		internal float f_ancho;
		internal char ch_character;
        internal dgCharacter(char character, float x0, float y0, float x1, float y1, ushort width, ushort height, float charwidth)
        {
            this.ch_character = character;
            this.f_x0 = x0;
            this.f_y0 = y0;
            this.f_x1 = x1;
            this.f_y1 = y1;
            this.ui_width = width;
            this.ui_height = height;
            this.f_ancho = charwidth;
        }
    }
}