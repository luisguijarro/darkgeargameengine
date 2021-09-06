using System;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class DialogResultEventArgs : EventArgs
    {
        private readonly DialogResult dr_result;
        public DialogResultEventArgs(DialogResult result)
        {
            this.dr_result = result;
        }
        public DialogResult Result
        {
            get { return this.dr_result; }
        }
    }
}