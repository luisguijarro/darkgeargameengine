using System;
using System.Reflection;
using System.Collections.Generic;

using dgtk.Graphics;
using dge.G2D;
using dgtk;

namespace dge.GUI
{
    public class ListViewer : BaseObjects.Control
    {
        private readonly ScrollBar sbVer;
        private readonly ScrollBar sbHor;

        private dgFont font;
        private float f_FontSize;
        private Color4 c4_fontColor;
        private Color4 c4_BackgroundColor;

        #region THEME ATTRIBUTES:
        private int[] ListViewer_Selection_MarginsFromTheEdge;
        private float[] ListViewer_Selection_Texcoords;
        private int[] ListViewer_Header_MarginsFromTheEdge;
        private float[] FileIcon_TexCoords;
        private float[] FileImageIcon_TexCoords;
        private float[] FileSoundIcon_TexCoords;
        private float [] FolderIcon_TexCoords;

        #endregion

        private readonly Dictionary<string, uint> d_Headers_names; // <Nombre de Header , ID de HEADER>
        private readonly Dictionary<uint, ListViewerHeader> d_Headers; // <ID de Campo , HEADER A MOSTRAR>
        private readonly List<KeyValuePair<uint, object>> l_objects;
        private readonly Dictionary<uint, object> d_id_object;
        private readonly List<uint> l_Ids;
        private uint selectedID;
        private Type t_ObjectType;
        private string s_SortingField;
        private bool b_AscendingOrder;
        private int elmHeight;

        private int headersWidth;
        private object o_SelectedObject;
        //private int i_selectedIndex;
        
        public event EventHandler<ListItemSelectedEventArgs> ItemSelected;

        public ListViewer()
        {
            this.sbVer = new ScrollBar();
            this.sbVer.MaxValue = 0;
            this.sbHor = new ScrollBar();
            this.sbHor.MaxValue = 0;
            this.sbHor.Orientation = Orientation.Horizontal;

            this.font = GuiTheme.DefaultGuiTheme.Default_Font;
            this.f_FontSize = GuiTheme.DefaultGuiTheme.Default_FontSize;
            this.c4_fontColor = GuiTheme.DefaultGuiTheme.Default_TextColor;
            this.c4_BackgroundColor = GuiTheme.DefaultGuiTheme.ListViewer_Default_BackgroundColor;
            
            this.ListViewer_Selection_MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_Selection_MarginsFromTheEdge;
            this.ListViewer_Selection_Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Selection_Texcoords;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ListViewer_Texcoords;
            this.ListViewer_Header_MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ListViewer_Header_MarginsFromTheEdge;

            this.FileIcon_TexCoords = GuiTheme.DefaultGuiTheme.FileIcon_TexCoords;
            this.FileImageIcon_TexCoords = GuiTheme.DefaultGuiTheme.FileImageIcon_TexCoords;
            this.FileSoundIcon_TexCoords = GuiTheme.DefaultGuiTheme.FileSoundIcon_TexCoords;
            this.FolderIcon_TexCoords = GuiTheme.DefaultGuiTheme.FolderIcon_TexCoords;

            this.tcFrameOffset = new float[] {0,0};

            this.d_Headers = new Dictionary<uint, ListViewerHeader>();
            this.d_Headers_names = new Dictionary<string, uint>();
            this.l_Ids = new List<uint>();
            this.l_objects = new List<KeyValuePair<uint, object>>();
            this.d_id_object = new Dictionary<uint, object>();

            this.sbHor.ValueChanged += HorValueChanged;
            this.ItemSelected += delegate{};
            this.t_ObjectType = typeof(Object);

            this.UpdateElementHeight();
        }

        #region PROTECTED OVERRIDE METHODS:

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();
            this.sbVer.GUI = this.gui;
            this.sbHor.GUI = this.gui;
            foreach (ListViewerHeader val in this.d_Headers.Values)
            {
                val.GUI = this.gui;
            }
            this.UpdateScrollBars();
        }

        protected internal override void UpdateTheme()
        {
            this.ListViewer_Selection_MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ListViewer_Selection_MarginsFromTheEdge;
            this.ListViewer_Selection_Texcoords = this.gui.gt_ActualGuiTheme.ListViewer_Selection_Texcoords;

            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ListViewer_MarginsFromTheEdge;
            this.Texcoords = this.gui.gt_ActualGuiTheme.ListViewer_Texcoords;

            this.ListViewer_Header_MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ListViewer_Header_MarginsFromTheEdge;

            this.FileIcon_TexCoords = this.gui.gt_ActualGuiTheme.FileIcon_TexCoords;
            this.FileImageIcon_TexCoords = this.gui.gt_ActualGuiTheme.FileImageIcon_TexCoords;
            this.FileSoundIcon_TexCoords = this.gui.gt_ActualGuiTheme.FileSoundIcon_TexCoords;
            this.FolderIcon_TexCoords = this.gui.gt_ActualGuiTheme.FolderIcon_TexCoords;

            this.c4_BackgroundColor = this.gui.gt_ActualGuiTheme.ListViewer_Default_BackgroundColor;

            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.Default_Font.Name)
            {
                this.font = this.gui.GuiTheme.Default_Font;
            }
            if (this.f_FontSize == GuiTheme.DefaultGuiTheme.Default_FontSize)
            {
                this.f_FontSize = this.gui.GuiTheme.Default_FontSize;
            }
            if (this.c4_fontColor == GuiTheme.DefaultGuiTheme.Default_TextColor)
            {
                this.c4_fontColor = this.gui.GuiTheme.Default_TextColor;
            }
            this.UpdateElementHeight();
        }

        protected override void OnResize()
        {
            base.OnResize();
            
            this.ResizeScrollBars();

            this.UpdateScrollBars();  
        }

        protected override void OnReposition()
        {
            base.OnReposition();

            foreach (ListViewerHeader val in this.d_Headers.Values)
            {
                val.intX = this.int_x + this.i_x;
                val.intY = this.int_y + this.i_y;
            }
            
            this.ResizeScrollBars();

            this.UpdateScrollBars(); 
        }

        protected override void InputSizeAlter(int width, int height)
        {
            this.i_width = width-this.sbVer.Width;
            this.i_height = height-this.sbHor.Height;
        }

        protected override int[] OutputSizeAlter(int width, int height)
        {
            int[] ret;
            ret = new int[] {width+this.sbVer.Width, height+this.sbHor.Height};
            return ret;
        }

        #endregion

        #region PROTECTED OVERRIDE INPUT METHODS:

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            if (this.b_IsEnable)
            {
                //this.OnMDown(this, e);
                if ((dge.Core2D.SelectedID == this.ui_id) || this.l_Ids.Contains(e.ID))
                {
                    DateTime dt_now = DateTime.Now;
                    if ((dt_now-this.TimeLastClick).TotalMilliseconds < 200)
                    {
                        this.OnMDoubleClick(this, e);
                    }
                    this.TimeLastClick = dt_now;

                    if (this.l_Ids.Contains(e.ID))
                    {
                        this.selectedID = e.ID;
                        this.o_SelectedObject = this.d_id_object[e.ID];
                        this.ItemSelected(this, new ListItemSelectedEventArgs(this.d_id_object[e.ID]));
                    }
                }
            }
        }

        protected override void OnMWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnMWheel(sender, e);
            if ((e.ID==this.ui_id) || (this.l_Ids.Contains(e.ID)))
            {
                if (this.l_objects.Count > 0)
                {
                    this.sbVer.Value -= (int)((float)((this.l_objects.Count*this.elmHeight)/(float)(this.InnerSize.Height/(float)this.elmHeight)) * e.Delta); //(this.Height / (this.l_objects.Count*2) * (e.Delta > 0? 1 : -1));
                }
            }
        }

        #endregion

        #region PROTECTED OVERRIDE DRAW METHODS:
        protected override void pDraw()
        {
            this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.tbo_ThemeSltTBO.ID, this.c4_BackgroundColor, this.i_x, this.i_y, this.i_width, this.i_height, 0f, this.MarginsFromTheEdge, this.Texcoords, new float[]{0f,0f}, 1);
            base.pDraw();
            DrawIn(this.i_x, this.i_y, this.Width, this.Height, DrawScrollBars);
        }

        protected override void pDrawID()
        {
            base.pDrawID();
            DrawIdIn(this.i_x, this.i_y, this.Width, this.Height, DrawScrollBarsIDs);
        }

        protected override void pDrawContent()
        {
            if (this.contentUpdate) // && l_objects.Count>0) 
            {
                int elmHeight = (int)((this.font.MaxCharacterHeight/this.font.MaxFontSize)*this.f_FontSize);
                this.DrawIn(this.ida_X, this.ida_Y+elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3], this.ida_Width, this.ida_Height-(elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3]), DrawContent);
                this.DrawIn(this.ida_X, this.ida_Y, this.ida_Width, elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3], DrawHeaders);
            } 
        }

        protected override void pDrawContentID()
        {            
            if (this.contentUpdate) // && VisibleSurfaceOrder.Count>0) 
            {
                int elmHeight = (int)((this.font.MaxCharacterHeight/this.font.MaxFontSize)*this.f_FontSize);
                this.DrawIdIn(this.ida_X, this.ida_Y+elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3], this.ida_Width, this.ida_Height-(elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3]), DrawContentIDs); // Elementos
                this.DrawIdIn(this.ida_X, this.ida_Y, this.ida_Width, elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3], DrawHeadersIds); // Cabeceras.
            }
        }

        protected override void DrawContent()
        {
            //base.DrawContent();
            this.DrawObjects();
        }

        protected override void DrawContentIDs()
        {
            //base.DrawContentIDs();
            this.DrawObjectsIDs();
        }

        #endregion

        #region PRIVATE METHODS:

        private void HorValueChanged(object sender, IntValueChangedEventArgs e)
        {
            //UpdateHeaders();
            
            int posX = 0-this.sbHor.Value;
            foreach (ListViewerHeader val in this.d_Headers.Values)
            {
                val.X = posX;
                posX += val.Width; 
            }
        }
        private void UpdateHeaders()
        {
            int posX = 0-this.sbHor.Value;
            foreach (ListViewerHeader val in this.d_Headers.Values)
            {
                val.X = posX;
                posX += val.Width; 
            }
            this.headersWidth = posX+this.sbHor.Value;
        }

        private void UpdateElementHeight()
        {
            elmHeight = (int)((this.font.MaxCharacterHeight/this.font.MaxFontSize)*this.f_FontSize);
        }

        private void UpdateScrollBars()
        {
            if (this.gui != null)
            {
                int elmHeight = (int)((this.font.MaxCharacterHeight/this.font.MaxFontSize)*this.f_FontSize);
                if ((this.l_objects.Count * elmHeight) > (this.InnerSize.Height-(elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3])))
                {
                    this.sbVer.MaxValue = (int)((this.l_objects.Count * elmHeight) - (this.InnerSize.Height-(elmHeight+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[1]+this.gui.GuiTheme.ListViewer_Header_MarginsFromTheEdge[3])));
                }
                else
                {
                    this.sbVer.MaxValue = 0;
                }

                if (this.headersWidth > this.InnerSize.Width)
                {
                    this.sbHor.MaxValue = this.headersWidth-this.InnerSize.Width;
                }
                else
                {
                    this.sbHor.MaxValue = 0;
                }
            }
        }

        private void ResizeScrollBars()
        {            
            this.sbVer.Y = 0; //(this.i_y);
            this.sbVer.Height = this.Height; //(this.i_height);
            this.sbVer.X = /*this.i_x + */this.i_width;

            this.sbHor.Y = (int)(/*this.i_y+*/this.i_height);
            this.sbHor.Width = this.i_width;
            this.sbHor.X = 0; //this.i_x; 
        }

        private void ShortObjects()
        {
            if (this.b_AscendingOrder)
            {
                this.l_objects.Sort(delegate(KeyValuePair<uint, object> o1, KeyValuePair<uint, object> o2)
                {
                    System.Reflection.PropertyInfo objFieldInfo = this.t_ObjectType.GetProperty(this.s_SortingField);
                    return objFieldInfo.GetValue(o1.Value).ToString().CompareTo(objFieldInfo.GetValue(o2.Value).ToString());
                });
            }
            else
            {
                this.l_objects.Sort(delegate(KeyValuePair<uint, object> o2, KeyValuePair<uint, object> o1)
                {
                    System.Reflection.PropertyInfo objFieldInfo = this.t_ObjectType.GetProperty(this.s_SortingField);
                    return objFieldInfo.GetValue(o1.Value).ToString().CompareTo(objFieldInfo.GetValue(o2.Value).ToString());
                });
            }
        }

        private void ResizeheadEvent(object sender, ResizeEventArgs e)
        {
            this.UpdateHeaders();
            this.UpdateScrollBars();
        }


        #endregion

        #region PRIVATE DRAW METHODS:

        private void DrawHeaders()
        {
            foreach(ListViewerHeader header in this.d_Headers.Values)
            {
                header.Draw(); //(this.sbHor.Value, 0);
            }
        }

        private void DrawHeadersIds()
        {
            foreach(ListViewerHeader header in this.d_Headers.Values)
            {
                header.DrawID(); //(this.sbHor.Value, 0);
            }
        }

        private void DrawObjects()
        {
            for(int i =0; i<this.l_objects.Count;i++)
            {
                KeyValuePair<uint, object> val = this.l_objects[i];
                
                int posy = (int)(i * elmHeight ) - this.sbVer.Value; // Alto de elemento
                int posx = 0;
                if (this.d_Headers.Count>0)
                {
                    foreach (uint key in this.d_Headers.Keys)
                    {
                        int px = this.d_Headers[key].X; //posx;//+this.sbHor.Value;

                        string TextToWrite = "";

                        ListViewerHeader head = this.d_Headers[key];
                        //if (this.t_ObjectType != typeof(string))
                        //{
                            if ((val.Value.GetType() == typeof(String)) && (head.FieldToShow.Length>=8) && (head.FieldToShow.Substring(0,8) == "ToString"))
                            {
                                TextToWrite = " "+(string)val.Value;
                            }
                            else
                            {

                                if ((val.Value.GetType() == typeof(System.IO.FileInfo)) && head.FieldToShow == "Name")
                                {
                                    float[] FileTexCoord = this.FileIcon_TexCoords;
                                    switch(((System.IO.FileInfo)val.Value).Name.Substring(((System.IO.FileInfo)val.Value).Name.Length-4, 4))
                                    {
                                        case ".png":
                                        case ".bmp":
                                        case ".jpg":
                                        case "jpeg":
                                        case ".gif":
                                            FileTexCoord = this.FileImageIcon_TexCoords;
                                            break;
                                        case ".ogg":
                                        case ".wav":
                                        case ".mp3":
                                        case "flac":
                                            FileTexCoord = this.FileSoundIcon_TexCoords;
                                            break;
                                        default:
                                            FileTexCoord = this.FileIcon_TexCoords;
                                            break;
                                    }
                                    this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, px, posy, 0, elmHeight, elmHeight, 0f,
                                    FileTexCoord[0],
                                    FileTexCoord[1],
                                    FileTexCoord[2],
                                    FileTexCoord[3]);
                                    px += elmHeight;
                                }
                                if ((val.Value.GetType() == typeof(System.IO.DirectoryInfo)) && head.FieldToShow == "Name")
                                {
                                    this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, px, posy, 0, elmHeight, elmHeight, 0f,
                                    this.FolderIcon_TexCoords[0],
                                    this.FolderIcon_TexCoords[1],
                                    this.FolderIcon_TexCoords[2],
                                    this.FolderIcon_TexCoords[3]);
                                    px += elmHeight;
                                }
                                //System.Reflection.MemberInfo[] mi = this.t_ObjectType.GetMember(head.FieldToShow);   
                                System.Reflection.MemberInfo[] mi = val.Value.GetType().GetMember(head.FieldToShow); 

                                if (mi.Length>0)
                                {
                                    switch(mi[0].MemberType)
                                    {
                                        case MemberTypes.Property:
                                            TextToWrite = " "+((PropertyInfo)mi[0]).GetValue(val.Value).ToString();
                                            break;
                                        case MemberTypes.Field:
                                            TextToWrite = " "+((FieldInfo)mi[0]).GetValue(val.Value).ToString();
                                            break;
                                        case MemberTypes.Method:
                                            TextToWrite = " "+((MethodInfo)mi[0]).Invoke(val.Value, null).ToString();
                                            break;
                                        default:
                                            //Nada.
                                            break;
                                    }
                                }
                                
                            }

                          /*  
                        }
                        else
                        {
                            TextToWrite = (string)val.Value;
                        }*/
                        this.DrawText(px/* - this.sbHor.Value*/, posy, head.Width, elmHeight, TextToWrite, (this.selectedID == val.Key));
                        

                        posx += head.Width;
                    }                
                }
                else
                {
                    this.DrawText(posx - this.sbHor.Value, posy, this.i_width, elmHeight, " "+val.Value.ToString(), (this.selectedID == val.Key));
                }
                    
                if (this.selectedID == val.Key)
                {
                    //Pintar selección
                    int selectwidth = this.InnerSize.Width <= this.headersWidth ? this.InnerSize.Width : this.headersWidth;
                    this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, 0, posy, selectwidth, elmHeight, 0, this.ListViewer_Selection_MarginsFromTheEdge, this.ListViewer_Selection_Texcoords, new float[] {0f, 0f}, 0);
                }             
            }
        }

        private void DrawText(int x, int y, int width, int height, string TextToWrite, bool selected)
        {
            this.DrawIn(x, y, width, height, delegate()
            {
                //this.gui.Drawer.Draw(Color4.Black, 0, 0, width, height, 0f);
                //this.gui.Drawer.Draw(Color4.Gray, 1, 1, width-2, height-2, 0f);
                if (selected)
                {
                    this.gui.Writer.Write(this.font, this.c4_fontColor, TextToWrite, this.f_FontSize, 0, 0, this.c4_fontColor);
                }
                else
                {
                    this.gui.Writer.Write(this.font, this.c4_fontColor, TextToWrite, this.f_FontSize, 0, 0); //this.f_FontSize/2, 0);
                }
            });
        }

        private void DrawObjectsIDs()
        {
            int elmHeight = (int)((this.font.MaxCharacterHeight/this.font.MaxFontSize)*this.f_FontSize);
            for(int i =0; i<this.l_objects.Count;i++)
            {
                int posy = (int)(i * elmHeight ) - this.sbVer.Value;
                dge.G2D.IDsDrawer.DrawGL2D(new Color4(dge.Core2D.DeUIntAByte4(this.l_objects[i].Key)), 0, posy, this.InnerSize.Width, elmHeight, 0f); // Pintamos ID de la superficie.
            }
        }

        private void DrawScrollBars()
        {
            if (this.sbVer.Visible) { this.sbVer.Draw(); }
            if (this.sbHor.Visible) { this.sbHor.Draw(); }
        }

        private void DrawScrollBarsIDs()
        {
            if (this.sbVer.Visible) { this.sbVer.DrawID(); }
            if (this.sbHor.Visible) { this.sbHor.DrawID(); }
        }

        #endregion
    
        #region PUBLIC METHODS:

        public void AddHeader(string name, string fieldname)
        {
            if (!d_Headers_names.ContainsKey(name))
            {
                ListViewerHeader lvh = new ListViewerHeader(name, fieldname);
                lvh.GUI = this.gui;
                lvh.SizeChanged += ResizeheadEvent;
                this.d_Headers.Add(lvh.ID, lvh);
                this.d_Headers_names.Add(name, lvh.ID);
                this.UpdateHeaders();
            }
        }

        public void SetHeaders(string[] name_fieldname)
        {
            if (name_fieldname.Length%2 == 0)
            {
                this.d_Headers_names.Clear();
                foreach (ListViewerHeader val in this.d_Headers.Values)
                {
                    val.SizeChanged -= ResizeheadEvent;
                }
                this.d_Headers.Clear();
                for (int i=0;i<name_fieldname.Length;i+=2)
                {
                    ListViewerHeader lvh = new ListViewerHeader(name_fieldname[i], name_fieldname[i+1]);
                    lvh.GUI = this.gui;
                    lvh.SizeChanged += ResizeheadEvent;
                    this.d_Headers.Add(lvh.ID, lvh);
                    this.d_Headers_names.Add(lvh.Name, lvh.ID);
                }
                this.UpdateHeaders();
            }            
        }

        public void AddObject(object @object)
        {
            bool contains = false;
            for(int i=0;i<this.l_objects.Count;i++)
            {
                if (this.l_objects[i].Value == @object)
                {
                    contains = true;
                    break;
                }
            }
            if (!contains)
            {
                uint tid = dge.Core2D.GetID();
                this.d_id_object.Add(tid, @object);
                this.l_objects.Add(new KeyValuePair<uint, object>(tid, @object));
                this.l_Ids.Add(tid);
                this.UpdateScrollBars();
            }
        }

        public void AddObjects(object[] objects)
        {
            for (int i=0;i<objects.Length;i++)
            {
                this.AddObject(objects[i]);
            }
        }

        public void SetObjects(object[] objects)
        {
            if (objects.Length>0)
            {
                this.ClearList();
                this.t_ObjectType = objects[0].GetType();
                this.s_SortingField = "ToString()";
                for (int i=0;i<objects.Length;i++)
                {
                    this.AddObject(objects[i]);
                }
                this.UpdateScrollBars();
            }            
        }

        public void RemoveObject(object @object)
        {
            bool contains = false;
            int toDelete = 0;
            for(int i=0;i<this.l_objects.Count;i++)
            {
                if (this.l_objects[i].Value == @object)
                {
                    contains = true;
                    toDelete = i;
                    break;
                }
            }
            if (contains)
            {
                dge.Core2D.ReleaseID(this.l_objects[toDelete].Key);
                this.d_id_object.Remove(this.l_objects[toDelete].Key);
                this.l_Ids.Remove(this.l_objects[toDelete].Key);
                this.l_objects.RemoveAt(toDelete);
                this.UpdateScrollBars();
            }

            //this.UpdateSbVerMaxValue();
        }

        public void ClearList()
        {
            for (int i=0;i<this.l_objects.Count;i++)
            {
                dge.Core2D.ReleaseID(this.l_objects[i].Key);
            }
            this.d_id_object.Clear();
            this.l_objects.Clear();
            this.l_Ids.Clear();
            this.UpdateScrollBars();
        }

        public void SelectItem(int index)
        {
            if (this.l_objects.Count > index)
            {
                //this.i_selectedIndex = index;
                this.o_SelectedObject = this.l_objects[index].Value;
                this.ItemSelected(this, new ListItemSelectedEventArgs(this.o_SelectedObject));
            }            
        }

        public void SetHeaderWidth(string head, uint width)
        {
            if (this.d_Headers_names.ContainsKey(head))
            {
                this.d_Headers[this.d_Headers_names[head]].Width = (int)width;
                this.UpdateHeaders();
            }
        }

        #endregion

        #region PROPERTIES:

        public Type ObjectType
        {
            get { return this.t_ObjectType; }
            set 
            { 
                this.t_ObjectType = value; 
                // Limpiar diccionarios.
            }
        }

        public object SelectedObject
        {
            get { return this.o_SelectedObject; }
        }

        public string ShorttingField
        {
            set { this.s_SortingField = value; }
            get { return this.s_SortingField; }
        }

        public bool AscendingOrder
        {
            set { this.b_AscendingOrder = value; }
            get { return this.b_AscendingOrder; }
        }

        #endregion
    }
}