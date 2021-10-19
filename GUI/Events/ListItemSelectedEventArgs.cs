using System;

namespace dge.GUI
{
    public class ListItemSelectedEventArgs : EventArgs
    {
        private object ret;
        public ListItemSelectedEventArgs(object Object)
        {
            this.ret = Object;
        }
        public object ObjectSelected
        {
            get { return ret; }
        }
    }
}