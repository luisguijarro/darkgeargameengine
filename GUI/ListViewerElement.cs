using System;
using System.Collections.Generic;

namespace dge.GUI
{   public class ListViewerElement
    {
        private int i_id;
        private string s_text;
        private List<string> l_SubElements;
        public ListViewerElement() : this("")
        {

        }
        public ListViewerElement(string text)
        {
            this.s_text = text;
            this.l_SubElements = new List<string>();
        }

        #region PUBLIC:

        public void AddSubElement(string text)
        {
            this.l_SubElements.Add(text);
        }

        public void RemoveSubElement(string text)
        {
            if (this.l_SubElements.Contains(text))
            {
                this.l_SubElements.Remove(text);
            }            
        }

        #endregion

        #region PROPERTIES:

        public int ID
        {
            set { this.i_id = value; }
            get { return this.i_id; }
        }

        public string Text
        {
            set { this.s_text = value; }
            get { return this.s_text; }
        }
        
        #endregion
    }
}