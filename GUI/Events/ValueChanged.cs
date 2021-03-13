using System;

namespace dge.GUI
{
    public class IntValueChangedEventArgs
    {
        int i_value;
        public IntValueChangedEventArgs(int value)
        {
            this.i_value = value;
        }
        public int Value
        {
            get { return this.i_value; }
        }

    }
}