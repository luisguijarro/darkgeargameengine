using System;
using System.Collections.Generic;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class ListViewer : BaseObjects.Control
    {
        private ScrollBar sbVer;
        private ScrollBar sbHor;
        private float f_FontSize;
        private dgFont font;
        private TextAlign ta_textAlign;
        private Dictionary<int, ListViewerElement> d_Elements; // ID, Element
        private List<int> l_VisualOrder; // ID
        private Dictionary<string, ListViewerHeader> d_ListViewerColumns;
        private bool b_showColumns;

        public ListViewer()
        {
            this.sbVer = new ScrollBar();
            this.sbHor = new ScrollBar();

            this.sbVer.Visible = false; // Ocultas por defecto.
            this.sbHor.Visible = false; // Ocultas por defecto.

            this.l_VisualOrder = new List<int>();
            this.d_Elements = new Dictionary<int, ListViewerElement>();
            this.d_ListViewerColumns = new Dictionary<string, ListViewerHeader>();

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Texcoords;
            this.tcFrameOffset = new float[] {0,0};

        }

        #region PUBLIC:

        public void Add(ListViewerElement element)
        {
            if (!this.d_Elements.ContainsValue(element))
            {
                for (int i=0;i<this.d_Elements.Count;i++)
                {
                    if (!this.d_Elements.ContainsKey(i))
                    {
                        this.d_Elements.Add(i, element);
                        return;
                    }
                }
                this.d_Elements.Add(this.d_Elements.Count, element);
            }
        }

        public void Remove(ListViewerElement element)
        {
            this.Remove(element.ID);
        }

        public void Remove(int id)
        {
            if (this.d_Elements.ContainsKey(id))
            {
                this.d_Elements.Remove(id);
            }
        }

        public void AddColumn(string Column)
        {
            if (!d_ListViewerColumns.ContainsKey(Column))
            {
                this.d_ListViewerColumns.Add(Column, new ListViewerHeader(Column));
            }
        }

        #endregion

        #region OVERRIDE:

        internal override void Draw()
        {
            uint framewidth = this.sbVer.Visible ? this.Width-this.sbVer.Width : this.Width;
            uint frameHeight = this.sbHor.Visible ? this.Height-this.sbHor.Height : this.Height;
            if (this.gui != null)
            {
                this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, framewidth, frameHeight, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);

                if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
                {
                    DrawIn(this.i_x-(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)framewidth-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)frameHeight-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawContent);
                }
            }
        }

        internal override void DrawID()
        {
            base.DrawID();
        }

        #endregion

        #region PRIVATE:

        private void DrawElements()
        {
            for (int i=0;i<this.l_VisualOrder.Count;i++)
            {

            }
        }

        #endregion

        #region PROPERTIES:

        public bool ShowColumns
        {
            set { this.b_showColumns = value; }
            get { return this.b_showColumns; }
        }

        #endregion

    }
}