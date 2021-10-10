using System;
using System.Collections.Generic;

using dgtk.Graphics;

namespace dge.GUI
{   public class TreeViewerElement : IDisposable
    {
        internal uint ui_id;
        internal uint ui_id_Button;
        internal Color4 colorid;
        internal Color4 coloridButton;
        private string s_text;
        private readonly Dictionary<string, TreeViewerElement> d_SubElements;
        private string s_name;
        private object o_AsociateObject;
        internal bool b_collapse;
        internal TreeViewerElement parentTVE;

        public TreeViewerElement(object asociate_object) : this(asociate_object.ToString())
        {
            this.o_AsociateObject = asociate_object;
        }

        public TreeViewerElement(string name) : this(name, name)
        {

        }

        public TreeViewerElement(string name, string text)
        {
            this.s_name = name;
            this.s_text = text;
            this.d_SubElements = new Dictionary<string, TreeViewerElement>();
            this.ui_id = dge.Core2D.GetID();
            this.ui_id_Button = dge.Core2D.GetID();
            this.colorid = new Color4(dge.Core2D.DeUIntAByte4(this.ui_id));
            this.coloridButton = new Color4(dge.Core2D.DeUIntAByte4(this.ui_id_Button));
            this.b_collapse = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                dge.Core2D.ReleaseID(this.ui_id);
                dge.Core2D.ReleaseID(this.ui_id_Button);
                foreach (TreeViewerElement val in this.d_SubElements.Values)
                {
                    val.Dispose();
                }
            }
        }

        #region PUBLIC METHODS:

        public uint AddSubElement(string name)
        {
            return this.AddSubElement(name, name);
        }

        public uint AddSubElement(string name, string text)
        {
            return this.AddSubElement(new TreeViewerElement(name, text));
        }

        public uint AddSubElement(TreeViewerElement element)
        {
            if (!this.d_SubElements.ContainsKey(element.Name))
            {
                this.d_SubElements.Add(element.Name, element);
                element.parentTVE = this;
                return element.ID;
            }
            return 0;
        }

        public bool RemoveSubElement(string name)
        {
            if (this.d_SubElements.ContainsKey(name))
            {
                this.d_SubElements.Remove(name);
                return true;
            }
            return false;
        }

        public bool RemoveSubElement(TreeViewerElement element)
        {
            if (this.d_SubElements.ContainsValue(element))
            {
                this.d_SubElements.Remove(element.Name);
                return true;
            }
            return false;
        }

        public TreeViewerElement[] GetSubElements()
        {
            TreeViewerElement[] ret = new TreeViewerElement[this.d_SubElements.Count];
            this.d_SubElements.Values.CopyTo(ret, 0);
            return ret;
        }

        public TreeViewerElement GetSubElement(string elementName)
        {
            if (this.d_SubElements.ContainsKey(elementName))
            {
                return this.d_SubElements[elementName];
            }
            return null;
        }

        #endregion

        #region PROPERTIES:

        public uint ID
        {
            set { this.ui_id = value; }
            get { return this.ui_id; }
        }

        public string Text
        {
            set { this.s_text = value; }
            get { return this.s_text; }
        }

        public string Name 
        {
            get{ return this.s_name; }
        }

        public int ChildsCount
        {
            get { return this.d_SubElements.Count; }
        }

        public object AsociatedObject
        {
            get { return this.o_AsociateObject; }
        }
        
        #endregion
    }
}