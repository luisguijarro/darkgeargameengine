using System;

namespace dge
{
    public class ParentWindowSettedEventArgs : EventArgs
    {
        dgWindow dgw_Window;
        public ParentWindowSettedEventArgs(dgWindow window)
        {
            this.dgw_Window = window;
        }
        public dgWindow Window
        {
            get { return this.dgw_Window; }
        }
    }
}