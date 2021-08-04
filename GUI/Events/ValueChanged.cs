using System;

namespace dge.GUI
{
    public class IntValueChangedEventArgs
    {
        int i_value;
        bool b_fromPropertie;
        public IntValueChangedEventArgs(int value, bool fromPropertie)
        {
            this.i_value = value;
            this.b_fromPropertie = fromPropertie;
        }
        public int Value
        {
            get { return this.i_value; }
        }
        public bool fromPropertie
        {
            get { return this.b_fromPropertie; }
        }
    }
}