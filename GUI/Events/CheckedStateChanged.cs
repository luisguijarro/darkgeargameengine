using System;

namespace dge.GUI
{
    public class CheckedStateChanged
    {
        bool b_value;
        public CheckedStateChanged(bool value)
        {
            this.b_value = value;
        }
        public bool Checked
        {
            get { return this.b_value; }
        }

    }
}