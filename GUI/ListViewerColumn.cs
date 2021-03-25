using System;

namespace dge.GUI
{
    internal class ListViewerColumn : BaseObjects.Control
    {
        private string s_text;
        public ListViewerColumn(string text)
        {
            this.s_text = text;
        }

        public string Text
        {
            set { this.s_text = value; }
            get { return this.s_text; }
        }
    }
}