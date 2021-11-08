using System;
using dgtk.Graphics;

namespace dge.GUI
{
    public class SelectedColorEventArgs
    {
        Color4 c4_value;
        bool b_fromPropertie;
        public SelectedColorEventArgs(Color4 color, bool fromPropertie)
        {
            this.c4_value = color;
            this.b_fromPropertie = fromPropertie;
        }
        public Color4 Value
        {
            get { return this.c4_value; }
        }
        public bool fromPropertie
        {
            get { return this.b_fromPropertie; }
        }
    }
}