using System;

namespace dge.GUI
{
    public class TextChangedEventArgs
    {
        string s_value;
        bool b_fromPropertie;
        public TextChangedEventArgs(string value, bool fromPropertie)
        {
            this.s_value = value;
            this.b_fromPropertie = fromPropertie;
        }
        public string Value
        {
            get { return this.s_value; }
        }
        public bool fromPropertie
        {
            get { return this.b_fromPropertie; }
        }
    }
}