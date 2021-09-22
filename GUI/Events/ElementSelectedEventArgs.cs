using  System;

namespace dge.GUI
{
    public class ElementSelectedEventArgs : EventArgs
    {
        TreeViewerElement tve;
        public ElementSelectedEventArgs(TreeViewerElement element)
        {
            this.tve = element;
        }
        public TreeViewerElement ElementSelected
        {
            get { return this.tve; }
        }
    }
}