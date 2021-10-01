using System;

namespace dge.GUI
{
    public class IntValueChangedEventArgs
    {
        int i_LastValue;
        int i_value;
        bool b_fromProperty;
        public IntValueChangedEventArgs(int value, int lastValue, bool fromProperty)
        {
            this.i_value = value;
            this.i_LastValue = lastValue;
            this.b_fromProperty = fromProperty;
        }
        public int Value
        {
            get { return this.i_value; }
        }
        public int LastValue
        {
            get { return this.i_LastValue; }
        }
        public bool fromProperty
        {
            get { return this.b_fromProperty; }
        }
    }
}