using System;
using System.Reflection;
using System.Collections.Generic;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class TreeViewer : BaseObjects.Control
    {
        private dgFont font;
        private float f_FontSize;
        private Color4 c4_fontColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private Color4 c4_BackgroundColor;
        private int i_TextHeight;
        private int i_interline;
        private float f_ContentHeight;
        private float f_ContentWidth;
        private float[] TreeViewer_ExpandCollapseButton_Texcoords;
        private int[] SelectionMarginsFromTheEdge;
        private float[] SelectionTexcoords;
        private readonly ScrollBar sbVer;
        private readonly ScrollBar sbHor;

        private Type t_ObjectType;
        private string s_FieldToShow;

        private readonly Dictionary<string, TreeViewerElement> d_Elements;
        private readonly Dictionary<uint, TreeViewerElement> d_ElementsParent; // Diccionario con IDs de botones de colapso y expansion
        private readonly Dictionary<uint, TreeViewerElement> d_IDs; // Todos los IDs Existentes.
        private uint ui_SelectedID;
        public event EventHandler<ElementSelectedEventArgs> ElementSelected;

        public TreeViewer() : base()
        {
            this.font = GuiTheme.DefaultGuiTheme.Default_Font;
            this.f_FontSize = GuiTheme.DefaultGuiTheme.Default_FontSize;
            this.c4_fontColor = GuiTheme.DefaultGuiTheme.Default_TextColor;
            this.c4_textBorderColor = GuiTheme.DefaultGuiTheme.Default_TextBorderColor;
            this.c4_BackgroundColor = GuiTheme.DefaultGuiTheme.TreeViewer_DefaultBackgroundColor;
            this.i_TextHeight = (int)(this.font.MaxCharacterHeight*(this.f_FontSize/this.font.MaxFontSize));
            this.i_interline = 0;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TreeViewer_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.TreeViewer_Texcoords;
            this.SelectionMarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TreeViewer_Selection_MarginsFromTheEdge;            
            this.SelectionTexcoords = GuiTheme.DefaultGuiTheme.TreeViewer_Selection_Texcoords;
            //this.tcFrameOffset = new float[]{0f,0f};
            this.TreeViewer_ExpandCollapseButton_Texcoords = GuiTheme.DefaultGuiTheme.TreeViewer_ExpandCollapseButton_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.TreeViewer_ExpandCollapseButton_FrameOffset;


            this.sbVer = new ScrollBar();
            this.sbHor = new ScrollBar()
            {
                Orientation = Orientation.Horizontal
            };

            this.sbVer.MaxValue = 0; // Ocultas por defecto.
            this.sbHor.MaxValue = 0; // Ocultas por defecto.

            this.sbVer.Visible = false; // Ocultas por defecto.
            this.sbHor.Visible = false; // Ocultas por defecto.

            //this.sbHor.ValueChanged += ScrollBarHorChange;
            this.d_IDs = new Dictionary<uint, TreeViewerElement>();
            this.d_Elements = new Dictionary<string, TreeViewerElement>();
            this.d_ElementsParent = new Dictionary<uint, TreeViewerElement>();

            this.sbHor.GUI = this.gui;
            this.sbVer.GUI = this.gui;    
            
            this.ui_SelectedID=0;
            this.ElementSelected += delegate{};
            this.UpdateScrollBars();        
        }

        #region PROTECTED METHODS:

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();            

            this.sbHor.GUI = this.gui;
            this.sbVer.GUI = this.gui;
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.TreeViewer_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.TreeViewer_Texcoords;
            this.SelectionMarginsFromTheEdge = this.gui.gt_ActualGuiTheme.TreeViewer_Selection_MarginsFromTheEdge;            
            this.SelectionTexcoords = this.gui.gt_ActualGuiTheme.TreeViewer_Selection_Texcoords;
            this.TreeViewer_ExpandCollapseButton_Texcoords = this.gui.gt_ActualGuiTheme.TreeViewer_ExpandCollapseButton_Texcoords;
            this.tcFrameOffset = this.gui.gt_ActualGuiTheme.TreeViewer_ExpandCollapseButton_FrameOffset;
            if (this.c4_BackgroundColor == this.gui.gt_ActualGuiTheme.TreeViewer_DefaultBackgroundColor)
            {
                this.c4_BackgroundColor = this.gui.gt_ActualGuiTheme.TreeViewer_DefaultBackgroundColor;
            }

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
            this.i_TextHeight = (int)(this.font.MaxCharacterHeight*(this.f_FontSize/this.font.MaxFontSize));

            this.UpdateScrollBars();
        }
        
        protected override void OnResize()
        {
            base.OnResize();

            this.UpdateScrollBars();
        }
        
        protected override void InputSizeAlter(int width, int height)
        {
            this.i_width = /*width-this.sbVer.Width; /*/sbVer.Visible ? width-this.sbVer.Width : width;
            this.i_height = /*height - this.sbHor.Height; /*/sbHor.Visible ? height - this.sbHor.Height : height;
        }

        protected override int[] OutputSizeAlter(int width, int height)
        {
            int[] ret;
            ret = new int[] { /*width+this.sbVer.Width, height + this.sbHor.Height };/*/sbVer.Visible ? width+this.sbVer.Width : width, sbHor.Visible ? height + this.sbHor.Height : height };
            return ret;
        }

        #endregion

        #region PROTECTED DRAW METHODS:

        protected override void pDraw()
        {
            if (this.b_ShowMe)
            {
                this.gui.Drawer.Draw(this.c4_BackgroundColor, this.i_x, this.i_y, 0, this.i_width, this.i_height, 0f);
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, new float[]{0f,0f}, 0);
            }
            DrawIn(this.i_x, this.i_y, this.Width, this.Height, DrawScrollBars);
            //this.DrawScrollBars();
        }
        
        protected override void pDrawContent()
        {
            if (this.contentUpdate && this.d_Elements.Count>0) 
            {
                DrawIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContent);
            } 
        }

        protected override void pDrawContentID()
        {
            if (this.contentUpdate && this.d_Elements.Count>0)
            {
                this.DrawIdIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContentIDs);
            }
        }

        protected override void pDrawID()
        {
            base.pDrawID();
            DrawIdIn(this.i_x, this.i_y, this.Width, this.Height, DrawScrollBarsIDs);
        }

        protected override void DrawContent()
        {
            int cont = 0;
            foreach (TreeViewerElement val in this.d_Elements.Values)
            {
                int y = this.MarginTop+((this.i_TextHeight+this.i_interline)*(cont))-sbVer.Value;
                cont += DrawElement(val, this.MarginLeft-sbHor.Value, y); // Dejamos margenes iniciales para que no esten pegados al centro.
                //cont++;
            }
        }

        protected override void DrawContentIDs()
        {
            int cont = 0;
            foreach (TreeViewerElement val in this.d_Elements.Values)
            {
                cont += this.DrawElementID(val, this.MarginLeft-sbHor.Value, this.MarginTop+((this.i_TextHeight+this.i_interline)*(cont))-sbVer.Value); // Dejamos margenes iniciales para que no esten pegados al centro.
                //cont++;
            }
        }

        #endregion

        #region PRIVATE DRAW METHODS:

        private int DrawElement(TreeViewerElement parentElement, int x, int y)
        {
            int ret=1;
            string TextToShow = parentElement.Text;
            if (parentElement.AsociatedObject != null)
            {
                PropertyInfo pi = this.t_ObjectType.GetProperty(this.FieldToShow);
                TextToShow = pi.GetValue(parentElement.AsociatedObject).ToString();
            }

            int finalX = x;
            if (parentElement.ChildsCount > 0)
            {
                if (parentElement.b_collapse)
                {
                    // Draw Collapse Button
                    this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, x, y+this.i_TextHeight/4, 0, this.i_TextHeight/2, this.i_TextHeight/2, 0f, 
                    this.TreeViewer_ExpandCollapseButton_Texcoords[0],
                    this.TreeViewer_ExpandCollapseButton_Texcoords[1],
                    this.TreeViewer_ExpandCollapseButton_Texcoords[2],
                    this.TreeViewer_ExpandCollapseButton_Texcoords[3]);
                }
                else
                {
                    // Draw Collapse Button
                    this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO, x, y+this.i_TextHeight/4, 0, this.i_TextHeight/2, this.i_TextHeight/2, 0f, 
                    this.TreeViewer_ExpandCollapseButton_Texcoords[0]+this.tcFrameOffset[0],
                    this.TreeViewer_ExpandCollapseButton_Texcoords[1]+this.tcFrameOffset[1],
                    this.TreeViewer_ExpandCollapseButton_Texcoords[2]+this.tcFrameOffset[0],
                    this.TreeViewer_ExpandCollapseButton_Texcoords[3]+this.tcFrameOffset[1]);
                }
                finalX += (int)(this.i_TextHeight/1.5f);
            }

            if (y+this.i_TextHeight>0 && y<this.InnerSize.Height)
            {
                // Dibujamos Elemento:
                if (this.ui_SelectedID == parentElement.ID)
                {
                    int TWidth = (int)dge.G2D.Writer.MeasureString(this.font, " "+TextToShow+" ", this.f_FontSize)[0];
                    this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.tbo_ThemeTBO.ID, Color4.White, finalX, y, TWidth/*(int)(this.f_ContentWidth-(finalX+sbHor.Value))*/, this.i_TextHeight, 0f, this.SelectionMarginsFromTheEdge, this.SelectionTexcoords, new float[]{0f,0f}, 0);
                    this.gui.Writer.Write(this.font, this.c4_fontColor, " "+TextToShow, this.f_FontSize, finalX, y, this.c4_textBorderColor);
                }
                else
                {
                    this.gui.Writer.Write(this.font, this.c4_fontColor, " "+TextToShow, this.f_FontSize, finalX, y);
                }
            }

            if (parentElement.ChildsCount > 0)
            {
                if (!parentElement.b_collapse)
                {
                    TreeViewerElement[] childs = parentElement.GetSubElements();
                    for (int i=0;i<childs.Length;i++)
                    {
                        int lineHy = y+((this.i_TextHeight+this.i_interline)*(ret))+(this.i_TextHeight/2);
                        if (lineHy>0 && lineHy < this.InnerSize.Height)
                        {
                            // Draw Horizontal Line
                            this.gui.Drawer.Draw(Color4.Black, x+(this.i_TextHeight/4), y+((this.i_TextHeight+this.i_interline)*(ret))+(this.i_TextHeight/2), 0, (this.i_TextHeight/2), 1, 0f);
                        }
                        // Draw Vertical Line
                        this.gui.Drawer.Draw(Color4.Black, x+(this.i_TextHeight/4), y+(this.i_TextHeight/4)*3, 1, 0, ((this.i_TextHeight+this.i_interline)*(ret))-(int)(this.i_TextHeight/4f), 0f);

                        ret += this.DrawElement(childs[i], x+(this.i_TextHeight/4)*3, y+((this.i_TextHeight+this.i_interline)*(ret)));
                    }
                }
            }
            return ret;
        }

        private int DrawElementID(TreeViewerElement parentElement, int x, int y)
        {
            int ret=1;
            int finalX = x;
            if (parentElement.ChildsCount > 0)
            {
                dge.G2D.IDsDrawer.DrawGL2D(parentElement.coloridButton, x, y+this.i_TextHeight/4, this.i_TextHeight/2, this.i_TextHeight/2, 0f);
                finalX += (int)(this.i_TextHeight/1.5f);
            }

            dge.G2D.IDsDrawer.DrawGL2D(parentElement.colorid, finalX, y, (int)this.f_ContentWidth-finalX, this.i_TextHeight, 0f);

            if (parentElement.ChildsCount > 0)
            {
                if (!parentElement.b_collapse)
                {
                    TreeViewerElement[] childs = parentElement.GetSubElements();
                    for (int i=0;i<childs.Length;i++)
                    {
                        ret += this.DrawElementID(childs[i], x+(this.i_TextHeight/4)*3, y+((this.i_TextHeight+this.i_interline)*(ret)));
                    }
                }
            }
            return ret;
        }

        #endregion

        #region PROTECTED INPUT EVENTS:

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);
            if (e.Buttons == MouseButtons.Left)
            {
                if (this.d_ElementsParent.ContainsKey(dge.Core2D.SelectedID))
                {
                    this.d_ElementsParent[dge.Core2D.SelectedID].b_collapse = !this.d_ElementsParent[dge.Core2D.SelectedID].b_collapse;
                }
                if (d_IDs.ContainsKey(dge.Core2D.SelectedID))
                {
                    this.ui_SelectedID = dge.Core2D.SelectedID;
                    this.ElementSelected(this, new ElementSelectedEventArgs(d_IDs[this.ui_SelectedID]));
                }
            }
        }

        protected override void OnMWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnMWheel(sender, e);
            if ((this.ui_id ==  dge.Core2D.SelectedID) || this.d_IDs.ContainsKey(dge.Core2D.SelectedID))
            {
                if (this.f_ContentHeight>this.InnerSize.Height)
                {
                    this.sbVer.Value -= (int)(e.Delta*(this.f_ContentHeight/this.InnerSize.Height))*2;
                }
            }
        }

        #endregion

        #region PUBLIC METHODS:

        public uint AddElement(object asociated_object, string fieldShowed)
        {
            this.s_FieldToShow = fieldShowed;
            return this.AddElement(new TreeViewerElement(asociated_object));
        }

        public uint AddElement(string name)
        {
            return this.AddElement(name, name);
        }

        public uint AddElement(string name, string text)
        {
            return this.AddElement(new TreeViewerElement(name, text));
        }

        public uint AddElement(TreeViewerElement element)
        {
            if (!this.d_Elements.ContainsKey(element.Name))
            {
                this.d_Elements.Add(element.Name, element);
                this.UpdateScrollBars();  
                return element.ID;
            }
            return 0;
        }

        public bool RemoveElement(string name)
        {
            if (this.d_Elements.ContainsKey(name))
            {
                this.d_Elements.Remove(name);
                this.UpdateScrollBars();  
                return true;
            }
            return false;
        }

        public bool RemoveElement(TreeViewerElement element)
        {
            if (this.d_Elements.ContainsValue(element))
            {
                this.d_Elements.Remove(element.Name);
                this.UpdateScrollBars();  
                return true;
            }
            return false;
        }

        public TreeViewerElement[] GetElements()
        {
            TreeViewerElement[] ret = new TreeViewerElement[this.d_Elements.Count];
            this.d_Elements.Values.CopyTo(ret, 0);
            return ret;
        }

        public TreeViewerElement GetElement(string elementName)
        {
            if (this.d_Elements.ContainsKey(elementName))
            {
                return this.d_Elements[elementName];
            }
            return null;
        }

        public Type AsociateObjectsType
        {
            set { this.t_ObjectType = value; }
            get { return this.t_ObjectType; }
        }

        public string FieldToShow
        {
            set { this.s_FieldToShow = value; }
            get { return this.s_FieldToShow; }
        }

        public void Clear()
        {
            this.d_Elements.Clear();
        }

        #endregion

        #region PRIVATE METHODS:

        private void UpdateScrollBars()
        {
            this.d_IDs.Clear();
            this.d_ElementsParent.Clear();
            this.f_ContentWidth = 0;
            this.f_ContentHeight = 0;
            int cont = 1;
            foreach (TreeViewerElement val in this.d_Elements.Values)
            {
                this.CalculateContentWidthHeigth(val, this.MarginLeft, this.MarginTop+((this.i_TextHeight+this.i_interline)*(cont)));
                cont++;
            }

            if ( this.sbHor.Visible != (this.f_ContentWidth > this.InnerSize.Width))
            {
                this.sbHor.Visible = (this.f_ContentWidth > this.InnerSize.Width);
                if (this.f_ContentWidth<=this.InnerSize.Width)
                {
                    this.f_ContentWidth = this.InnerSize.Width;
                    //this.i_height = this.i_height;
                    this.sbHor.Visible = false;
                }
                else
                {
                    this.i_height = this.i_height - this.sbHor.Height;
                    this.sbHor.Visible = true;
                }
            }

            if (this.sbVer.Visible != (this.f_ContentHeight > this.InnerSize.Height))
            {
                this.sbVer.Visible = (this.f_ContentHeight > this.InnerSize.Height);
                if (this.f_ContentHeight <= this.InnerSize.Height)
                {
                    this.f_ContentHeight = this.InnerSize.Height;
                    //this.i_width = this.Width;
                    this.sbVer.Visible = false;
                }
                else
                {
                    this.i_width = this.i_width - this.sbVer.Width;
                    this.sbVer.Visible = true;
                }
            }

            /*
            this.sbHor.Visible = (this.f_ContentWidth > this.InnerSize.Width);
            this.sbVer.Visible = (this.f_ContentHeight > this.InnerSize.Height);
            */
            //this.Height = this.Height;
            
            
            
            
            
            //base.InputSizeAlter(this.i_width, this.i_height);
            this.SetInternalDrawArea(this.MarginLeft, this.MarginTop, this.i_width-(this.MarginLeft+this.MarginRight), this.i_height-(this.MarginTop+this.MarginBottom));

            this.sbHor.MaxValue = (this.f_ContentWidth > this.InnerSize.Width) ? (int)this.f_ContentWidth-this.InnerSize.Width : 0;
            this.sbVer.MaxValue = (this.f_ContentHeight > this.InnerSize.Height) ? (int)this.f_ContentHeight-this.InnerSize.Height : 0;


            this.sbVer.Y = 0;
            this.sbVer.Height = this.Height;
            this.sbVer.X = this.i_width;

            this.sbHor.Y = (int)(this.i_height);
            this.sbHor.Width = this.i_width;
            this.sbHor.X = 0;
 
        }

        private void CalculateContentWidthHeigth(TreeViewerElement parentelement, int x, int y)
        {
            string TextToShow = parentelement.Text;
            if (parentelement.AsociatedObject != null)
            {
                PropertyInfo pi = this.t_ObjectType.GetProperty(this.FieldToShow);
                TextToShow = pi.GetValue(parentelement.AsociatedObject).ToString();
            }
            float[] size = dge.G2D.Writer.MeasureString(this.font, /*" "+*/TextToShow, this.f_FontSize);
            int finalX = x;
            if (parentelement.ChildsCount>0)
            {
                finalX += (int)(this.i_TextHeight/1.5f);
            }
            if (this.f_ContentWidth < finalX+size[0])
            {
                this.f_ContentWidth = finalX+size[0];
            }
            this.f_ContentHeight += this.i_TextHeight+this.i_interline;
            TreeViewerElement[] tves = parentelement.GetSubElements();
            if (tves.Length > 0) { this.d_ElementsParent.Add(parentelement.ui_id_Button, parentelement); }
            this.d_IDs.Add(parentelement.ui_id, parentelement);
            for (int i=0;i<tves.Length;i++)
            {
                this.CalculateContentWidthHeigth(tves[i], finalX + (int)(this.i_TextHeight/4), y+((this.i_TextHeight+this.i_interline)*(i+1)));
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

        #region PROPERTIES:

        public dgFont Font
        {
            set 
            { 
                this.font = value; 
                this.UpdateScrollBars();  
            }
            get { return this.font; }
        }

        public float FontSize
        {
            set
            {
                this.f_FontSize = value; 
                this.UpdateScrollBars();  
            }
            get { return this.f_FontSize; }
        }

        public Color4 TextColor
        {
            set { this.c4_fontColor = value; }
            get { return this.c4_fontColor; }
        }

        public Color4 TextBorderColor
        {
            set { this.c4_textBorderColor = value; }
            get { return this.c4_textBorderColor; }
        }

        #endregion
    }
}