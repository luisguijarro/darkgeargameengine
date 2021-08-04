using System;
using System.Collections.Generic;
using dgtk.Graphics;
using dge.G2D;
using dgtk;

namespace dge.GUI
{
    public class ListViewer : BaseObjects.Control
    {
        private ScrollBar sbVer;
        private ScrollBar sbHor;
        private float f_FontSize;
        private dgFont font;
        private List<object> l_ObjectsElements;
        private List<int> l_VisualOrder; // ID
        private Dictionary<string, ListViewerHeader> d_ListViewerColumns;
        private bool b_showColumns;
        private int i_selectedIndex;
        private object o_SelectedItem;

        public ListViewer()
        {
            this.sbVer = new ScrollBar();
            this.sbHor = new ScrollBar();
            this.sbHor.Orientation = Orientation.Horizontal;

            this.sbVer.MaxValue = 0; // Ocultas por defecto.
            this.sbHor.MaxValue = 0; // Ocultas por defecto.

            this.sbVer.Visible = true; // Ocultas por defecto.
            this.sbHor.Visible = false; // Ocultas por defecto.

            this.sbHor.ValueChanged += ScrollBarHorChange;

            this.AddSurface(this.sbVer);
            this.AddSurface(this.sbHor);

            this.l_VisualOrder = new List<int>();
            this.l_ObjectsElements = new List<object>();
            this.d_ListViewerColumns = new Dictionary<string, ListViewerHeader>();

            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.f_FontSize = 16;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Texcoords;
            this.tcFrameOffset = new float[] {0,0};
            this.b_showColumns = true;
        }


        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ListViewer_MarginsFromTheEdge;
            this.Texcoords = this.gui.gt_ActualGuiTheme.ListViewer_Texcoords;
        }

        #region PUBLIC:

        public void AddObject(object @object)
        {
            if (!this.l_ObjectsElements.Contains(@object))
            {
                this.l_ObjectsElements.Add(@object);
            }
            this.UpdateSbVerMaxValue();
        }

        public void AddObjects(object[] objects)
        {
            for (int i=0;i<objects.Length;i++)
            {
                this.AddObject(objects[i]);
            }
        }

        public void RemoveObject(object @object)
        {
            if (!this.l_ObjectsElements.Contains(@object))
            {
                this.l_ObjectsElements.Remove(@object);
            }
            this.UpdateSbVerMaxValue();
        }

        public void ClearList()
        {
            this.l_ObjectsElements.Clear();
            this.UpdateSbVerMaxValue();
        }

        public void AddColumn(string Name, string member)
        {
            if (!d_ListViewerColumns.ContainsKey(Name))
            {
                this.d_ListViewerColumns.Add(Name, new ListViewerHeader(Name, member));
                this.d_ListViewerColumns[Name].intX  =this.int_x+this.X;
                this.d_ListViewerColumns[Name].intY  =this.int_y+this.Y;
                this.d_ListViewerColumns[Name].SizeChanged += UpdateColumns;
                this.AddSurface(this.d_ListViewerColumns[Name]);
                this.UpdateColumns(null,null);
                this.b_showColumns = true;
            }
        }

        public void DefineColumns(string MembersColumns)
        {
            string[] colsMembers = MembersColumns.Replace(" ", "").Split(new char[]{'|'});
            if (colsMembers.Length%2 == 0)
            {
                for (int i=0;i<colsMembers.Length;i+=2)
                {
                    if (!d_ListViewerColumns.ContainsKey(colsMembers[i]))
                    {
                        this.d_ListViewerColumns.Add(colsMembers[i], new ListViewerHeader(colsMembers[i], colsMembers[i+1]));
                        this.d_ListViewerColumns[colsMembers[i]].intX  =this.int_x+this.X;
                        this.d_ListViewerColumns[colsMembers[i]].intY  =this.int_y+this.Y;
                        this.d_ListViewerColumns[colsMembers[i]].SizeChanged += UpdateColumns;
                        this.AddSurface(this.d_ListViewerColumns[colsMembers[i]]);
                        this.UpdateColumns(null,null);
                    }
                }
            }
        }

        public void RemoveColumn(string Column)
        {
            if (d_ListViewerColumns.ContainsKey(Column))
            {
                this.d_ListViewerColumns[Column].SizeChanged -= UpdateColumns;
                this.RemoveSurface(this.d_ListViewerColumns[Column].ID);
                this.d_ListViewerColumns.Remove(Column);
                this.UpdateColumns(null,null);
            }
        }

        public void SetColumnWidth(string column, int width)
        {
            if (this.d_ListViewerColumns.ContainsKey(column))
            {
                this.d_ListViewerColumns[column].Width = (uint)(width > GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Width ? width : GuiTheme.DefaultGuiTheme.ListViewer_Dibider_Width);
                this.UpdateColumnPos();
            }
        }

        public void SetColumnsWidth(string[] columns, int[] widths)
        {
            if (columns.Length == widths.Length)
            {
                for (int i=0;i<columns.Length;i++)
                {
                    this.SetColumnWidth(columns[i], widths[i]);
                }
            }
        }

        #endregion

        #region OVERRIDE:

        protected override void MDown(object sender, MouseButtonEventArgs e)
        {
            base.MDown(sender, e);
            
            if (Core2D.SelectedID == this.ui_id)
            {
                int posX = e.X-(this.int_x+this.i_x);
                int posY = e.Y - (this.int_y+this.i_y+this.MarginsFromTheEdge[1]+(this.b_showColumns ? (int)(this.f_FontSize + 4) : 0));
                this.SelectItem(posX, posY);
            }
        }

        protected override void MWheel(object sender, MouseWheelEventArgs e)
        {
            base.MWheel(sender, e);
            if (Core2D.SelectedID==this.ui_id)
            {
                if (this.l_ObjectsElements.Count > 0)
                {
                    this.sbVer.Value -= (int)((this.Height / (this.l_ObjectsElements.Count*2)) * (e.Delta > 0? 1 : -1));
                }
            }
        }

        protected override void pDraw()
        {
            if (this.gui != null)
            {
                uint framewidth = this.sbVer.Visible ? this.Width-this.sbVer.Width : this.Width;
                uint frameHeight = this.sbHor.Visible ? this.Height-this.sbHor.Height : this.Height;
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, framewidth, frameHeight, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
                
                if (this.l_ObjectsElements.Count>0)
                {
                    DrawIn(this.i_x+(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)framewidth-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)frameHeight-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), this.DrawElements);
                }
                if (this.d_ListViewerColumns.Count > 0)
                {
                    DrawIn(this.i_x+(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)framewidth-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)frameHeight-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawColumnHeaders);
                }
                if ((this.sbVer.Visible) || (this.sbHor.Visible))
                {
                    DrawIn(this.i_x, this.i_y, (int)this.ui_width, (int)this.ui_height, DrawScrollBar);
                }
            }
        }

        internal override void DrawID()
        {
            if (this.gui != null)
            {
                uint framewidth = this.sbVer.Visible ? this.Width-this.sbVer.Width : this.Width;
                uint frameHeight = this.sbHor.Visible ? this.Height-this.sbHor.Height : this.Height;
                bool showcols = (this.b_showColumns && this.d_ListViewerColumns.Count >0);

                dge.G2D.IDsDrawer.DrawGL2D(this.idColor, this.i_x, this.i_y + (int)(showcols ? this.f_FontSize+4 : 0), framewidth, frameHeight - (uint)(showcols ? this.f_FontSize+4 : 0), 0); // Pintamos ID de la superficie.
                
                if (this.d_ListViewerColumns.Count > 0)
                {
                    DrawIdIn(this.i_x+(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)framewidth-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)frameHeight-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawColumnHeadersID);
                }
                if ((this.sbVer.Visible) || (this.sbHor.Visible))
                {
                    DrawIdIn(this.i_x, this.i_y, (int)this.ui_width, (int)this.ui_height, DrawScrollBarID);
                }
            }
        }

        protected override void OnReposition()
        {
            base.OnReposition();
        }

        protected override void OnResize()
        {
            base.OnResize();
            this.sbVer.X = (int)(this.Width - this.sbVer.Width);
            this.sbVer.Y = 0;
            this.sbVer.Height = this.Height;
            this.sbHor.X = 0;
            this.sbHor.Y = (int)(this.Height - this.sbHor.Height);
            this.sbHor.Width = this.Width-this.sbVer.Width;
            this.UpdateSbVerMaxValue();
        }

        #endregion

        #region PRIVATE:

        private void DrawScrollBar()
        {
            if (this.sbVer.Visible) { this.sbVer.Draw(); }
            if (this.sbHor.Visible) { this.sbHor.Draw(); }
        }

        private void DrawScrollBarID()
        {
            if (this.sbVer.Visible) { this.sbVer.DrawID(); }
            if (this.sbHor.Visible) { this.sbHor.DrawID(); }
        }

        private void DrawColumnHeaders()
        {
            foreach(string key in this.d_ListViewerColumns.Keys)
            {
                this.d_ListViewerColumns[key].Draw();
            }
        }

        private void DrawColumnHeadersID()
        {
            foreach(string key in this.d_ListViewerColumns.Keys)
            {
                this.d_ListViewerColumns[key].DrawID();
            }
        }

        private void ScrollBarHorChange(object sender, IntValueChangedEventArgs e)
        {
            this.UpdateColumnPos();
        }

        private void UpdateSbVerMaxValue()
        {
            int efectiveHeight = (int)(this.Height-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]));
            efectiveHeight -= this.b_showColumns ? (int)(this.f_FontSize + 4) : 0;
            this.sbVer.MaxValue = (int)System.Math.Max(((this.l_ObjectsElements.Count*(this.f_FontSize+4))-efectiveHeight), 0);
        }

        private void UpdateColumns(object sender, ResizeEventArgs e)
        {
            int total_width = 0;
            total_width = this.UpdateColumnPos();
            if (total_width > this.Width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]) - ((this.sbVer.Visible) ? this.sbVer.Width : 0))
            {
                this.sbHor.Visible = true;
                this.sbHor.MaxValue = (int)(total_width - (this.Width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]) - ((this.sbVer.Visible) ? this.sbVer.Width : 0)));
            }
            else
            {
                this.sbHor.Visible = false;
            }
        }

        private int UpdateColumnPos()
        {
            string s_lastC = "";
            int baseX = -this.sbHor.Value;
            int total_width = 0;
            foreach(string key in this.d_ListViewerColumns.Keys)
            {
                this.d_ListViewerColumns[key].i_x = baseX;
                if (s_lastC.Length > 0)
                {
                    this.d_ListViewerColumns[key].i_x = (int)(this.d_ListViewerColumns[s_lastC].i_x + this.d_ListViewerColumns[s_lastC].Width);
                }
                total_width += (int)this.d_ListViewerColumns[key].Width;
                s_lastC = key;
            }
            return total_width;
        }

        private string ElementText="";
        private void DrawElements()
        {
            for (int i=0;i<this.l_ObjectsElements.Count;i++)
            {
                int posy = (int)(i*(this.f_FontSize+4)) - this.sbVer.Value + ((this.b_showColumns && this.d_ListViewerColumns.Count > 0) ? (int)(this.f_FontSize + 4) : 0);
                int posx = -this.sbHor.Value;
                Type objType = this.l_ObjectsElements[i].GetType();
                if (i==this.i_selectedIndex)
                {
                    this.gui.Drawer.Draw(Color4.Blue, 0, posy, (uint)(this.Width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]+this.sbVer.Width)), (uint)(this.f_FontSize+4), 0);
                }
                if (this.d_ListViewerColumns.Count > 0)
                {
                    foreach ( string key in this.d_ListViewerColumns.Keys)
                    {
                        System.Reflection.PropertyInfo objFieldInfo = objType.GetProperty(this.d_ListViewerColumns[key].s_Member);
                        if (objFieldInfo != null)
                        {
                            ElementText = objFieldInfo.GetValue(this.l_ObjectsElements[i]).ToString();
                        }
                        else
                        {
                            ElementText = "";
                        }
                        DrawIn(0+posx, posy, (int)this.d_ListViewerColumns[key].Width, (int)(this.f_FontSize + 4), DrawElementText);
                        posx+=(int)this.d_ListViewerColumns[key].Width;
                    }
                }
                else // Si no hay columnas...
                {
                    System.Reflection.PropertyInfo objFieldInfo = objType.GetProperty("Name"); //Intentamos obtener propiedad Name...
                    if (objFieldInfo != null) // Si existe obtenemos su valor
                    {
                        ElementText = objFieldInfo.GetValue(this.l_ObjectsElements[i]).ToString();
                    }
                    else // Si no existe...
                    {
                        objFieldInfo = objType.GetProperty("Text"); // Intentamos obtener propiedad Text.
                        if (objFieldInfo != null) // Si existe obtenemos su valor
                        {
                            ElementText = objFieldInfo.GetValue(this.l_ObjectsElements[i]).ToString();
                        }
                        else // Si no existe empleamos metodo To.String();
                        {
                            ElementText = this.l_ObjectsElements[i].ToString();
                        }
                    }                    
                    DrawIn(0+posx, posy, (int)(this.Width-(this.MarginsFromTheEdge[0]+ this.MarginsFromTheEdge[2] + this.sbVer.Width)), (int)(this.f_FontSize + 4), DrawElementText);
                }
            }
        }

        private void DrawElementText()
        {
            this.gui.Writer.Write(this.font, dgtk.Graphics.Color4.Black, this.ElementText, this.f_FontSize, 0, 0);
        }

        private void SelectItem(int mouseX, int mouseY) // seleccionar elemento mostrado por coordenadas.
        {
            this.i_selectedIndex = -1;
            this.o_SelectedItem = null;
           
            int width = (int) (this.Width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]+this.sbVer.Width));
            for (int i=0;i<this.l_ObjectsElements.Count;i++)
            {
                int posy = (int)((i*(this.f_FontSize+4f))-this.sbVer.Value)+this.MarginsFromTheEdge[1];
                if (posy > -this.f_FontSize+4f)
                {
                    if (this.MouseIn(this.MarginsFromTheEdge[0], posy, width, (int)(this.f_FontSize+4f), mouseX, mouseY))
                    {
                        this.i_selectedIndex = i;
                        this.o_SelectedItem = this.l_ObjectsElements[i];
                        //Console.WriteLine("Selected Item: "+i);
                        return;
                    }
                }
            }
        }

        private bool MouseIn(int x, int y, int width, int height, int mX, int mY)
        {
            if ((mX > x) && (mX < x+width))
            {
                if ((mY > y) && (mY < y+height))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region PROPERTIES:

        public float FontSize
        {
            set { this.f_FontSize = value; }
            get { return this.f_FontSize; }
        }

        public bool ShowColumns
        {
            set { this.b_showColumns = value; }
            get { return this.b_showColumns; }
        }

        public int SelectedIndex
        {
            get { return this.i_selectedIndex; }
        }

        public object SelectedItem
        {
            get { return this.o_SelectedItem; }
        }

        #endregion

    }
}