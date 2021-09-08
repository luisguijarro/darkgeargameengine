using System;
using System.Collections.Generic;

using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class TabControl : BaseObjects.Control
    {
        private Orientation barOrientation;
        //private int i_TabBarWidth;
        private Dictionary<string,uint> d_Name_Id;
        private List<uint> l_ClosersIDs;
        private dgFont font;
        private float f_fontsize;
        private Color4 c4_BackgroundColor;
        private int totalTabsWidth;
        private uint ActiveTabID;
        private int tabsHeigth;
        private int TabDisplacement; // modificador de coordenadas cuando Las Tabs no caben.
        private bool b_ClosableTabs;
        
        public TabControl() : this(300, 100)
        {

        }
        public TabControl(int width, int height) : base(width, height)
        {
            this.Visible=true;
            this.TabDisplacement=0;
            this.b_ClosableTabs= true;
            this.barOrientation = Orientation.Horizontal;
            this.d_Name_Id = new Dictionary<string, uint>();
            this.l_ClosersIDs = new List<uint>();
            this.f_fontsize = 14f;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TabPage_MarginsFromTheEdge_Hor;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.TabPage_Texcoords_Hor;
            this.tcFrameOffset = new float[]{0f,0f};
            this.c4_BackgroundColor = GuiTheme.DefaultGuiTheme.Default_BackgroundColor; 

            this.font = GuiTheme.DefaultGuiTheme.Default_Font;

            this.UpdateTabsHeight();

            this.MouseDown += this.OnMouseDown;
            this.MouseWheel += this.OnMouseWheel;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.MouseDown -= this.OnMouseDown;
                this.MouseWheel -= this.OnMouseWheel;
                this.d_Name_Id.Clear();
                this.d_Name_Id = null;
                this.l_ClosersIDs.Clear();
                this.l_ClosersIDs = null;
            }
        }

        #region Events:

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Buttons == MouseButtons.Left)
            {
                if (this.d_guiSurfaces.ContainsKey(dge.Core2D.SelectedID))
                {
                    this.ActiveTabID = dge.Core2D.SelectedID;
                }
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (true) //this.totalTabsWidth>this.Width)
            {
                if (this.d_guiSurfaces.ContainsKey(dge.Core2D.SelectedID) || (dge.Core2D.SelectedID == this.ID) || this.l_ClosersIDs.Contains(dge.Core2D.SelectedID))
                {
                    //Desplazar Tabs.
                    int tempdisplace = this.TabDisplacement;
                    int DispCant = (int)(this.totalTabsWidth/this.i_width);
                    if (DispCant > this.i_width) { DispCant = (int)this.i_width; }
                    
                    tempdisplace += (int)(DispCant*(e.Delta*3));
                    
                    if ((tempdisplace <= 0) && (tempdisplace>=(this.Width-this.totalTabsWidth)))
                    {
                        this.TabDisplacement = tempdisplace;
                    }
                    else //Control de posicio final e inicial.
                    {
                        if (tempdisplace > 0)
                        {
                            this.TabDisplacement = 0;
                        }
                        if (tempdisplace<(this.Width-this.totalTabsWidth))
                        {
                            this.TabDisplacement = this.Width-this.totalTabsWidth;
                        }
                    }
                    
                    //Console.WriteLine("Tab Displacement: "+this.TabDisplacement);
                    Console.WriteLine("Delta: "+e.Delta);
                }
            }
        }

        #endregion

        #region PRIVATE METHODS:

        private void UpdateTexCoords()
        {
            if (this.barOrientation==Orientation.Horizontal)
            {
                this.MarginsFromTheEdge = this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Hor;            
                this.Texcoords = this.gui.GuiTheme.TabPage_Texcoords_Hor;
            }
            else
            {
                this.MarginsFromTheEdge = this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Ver;            
                this.Texcoords = this.gui.GuiTheme.TabPage_Texcoords_Ver;
            }
        }

        private void DrawTabBar()
        {
            // Show Tabs Bar
            //this.totalTabsWidth = this.MarginLeft; //0; //this.X;
            //this.tabsHeigth = (uint)(Math.Ceiling(this.font.MaxCharacterHeight*(this.f_fontsize/this.font.MaxFontSize)));
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                if (this.barOrientation == Orientation.Horizontal)
                {
                    
                    TabPage tp = (TabPage)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]];
                    
                    this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.TabDisplacement + tp.i_TabX, 0, /*tmp_width*/ tp.i_TabWidth, this.tabsHeigth, 0, this.MarginsFromTheEdge, Texcoords, (tp.ID == this.ActiveTabID) ? this.gui.GuiTheme.TabPage_FrameOffset_Hor : this.tcFrameOffset, 0);
                    this.gui.Writer.Write(this.gui.GuiTheme.Default_Font, Color4.Black, tp.Text, this.f_fontsize, this.TabDisplacement + this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Hor[0] + tp.i_TabX, 0);
                    if (this.b_ClosableTabs) 
                    { 
                        tp.Draw_X_Button((int)(this.TabDisplacement + tp.i_TabX + /*tmp_width*/ tp.i_TabWidth)-this.MarginRight, (int)(this.MarginTop/2f));
                    }
                }
                else
                {

                }
            }
        }

        private void DrawTabBarIDs()
        {
            // Show Tabs Bar
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                if (this.barOrientation == Orientation.Horizontal)
                {
                    TabPage tp = (TabPage)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]];
                    
                    dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, tp.idColor, this.TabDisplacement + tp.i_TabX, 0, tp.i_TabWidth, this.tabsHeigth, 0, this.MarginsFromTheEdge, Texcoords, (tp.ID == this.ActiveTabID) ? this.gui.GuiTheme.TabPage_FrameOffset_Hor : this.tcFrameOffset, 1); // Pintamos ID de la superficie.
                    if (this.b_ClosableTabs) 
                    { 
                        tp.Draw_X_Button_ID((int)(this.TabDisplacement + tp.i_TabX + tp.i_TabWidth)-this.MarginRight, (int)(this.MarginTop/2f));
                    }
                }
                else
                {
                    
                }
            }
        }

        private void CalculateTotalTabsWidth()
        {
            this.totalTabsWidth = this.MarginLeft;
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                if (this.barOrientation == Orientation.Horizontal)
                {
                    TabPage tp = (TabPage)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]];
                    int tmp_width = (int)Math.Ceiling(dge.G2D.Writer.MeasureString(this.font, tp.Name, this.f_fontsize)[0]);
                    tmp_width += this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Hor[0]+this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Hor[2];
                    tmp_width += this.b_ClosableTabs ? (this.gui.GuiTheme.TabPage_X_Size[0] + this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Hor[0]) : 0;
                    tp.i_TabWidth = tmp_width;
                    tp.i_TabX = this.totalTabsWidth;
                    this.totalTabsWidth += tmp_width+this.MarginLeft;
                }
                else
                {

                }
            }
        }

        private void CalculateTabDisplacement()
        {
            if (this.totalTabsWidth <= this.i_width)
            {
                this.TabDisplacement = 0;
            }
            else
            {
                // Aquí viene lo chungo,
                int WWD = this.totalTabsWidth+this.TabDisplacement;
                if (this.i_width > WWD)
                {
                    this.TabDisplacement -=  WWD-this.i_width;
                }
            }
        }
        
        private void DrawActiveTab()
        {
            if (this.d_guiSurfaces.ContainsKey(this.ActiveTabID)) // Evita que el hilo de render se adelante y pete por un not fount key.
            {
                this.d_guiSurfaces[this.ActiveTabID].Draw();
            }
        }

        private void DrawActiveTabIDs()
        {
            if (this.d_guiSurfaces.ContainsKey(this.ActiveTabID)) // Evita que el hilo de render se adelante y pete por un not fount key.
            {
                this.d_guiSurfaces[this.ActiveTabID].DrawID();
            }
        }

        #endregion

        #region PROTECTED METHODS:
        protected internal override void UpdateTheme()
        {
            if (this.barOrientation == Orientation.Horizontal)
            {
                this.MarginsFromTheEdge = this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Hor;            
                this.Texcoords = this.gui.GuiTheme.TabPage_Texcoords_Hor;
                this.tcFrameOffset = new float[]{0f,0f};
            }
            else
            {
                this.MarginsFromTheEdge = this.gui.GuiTheme.TabPage_MarginsFromTheEdge_Ver;            
                this.Texcoords = this.gui.GuiTheme.TabPage_Texcoords_Ver;
                this.tcFrameOffset = new float[]{0f,0f};
            }

            this.c4_BackgroundColor = this.gui.GuiTheme.Default_BackgroundColor;

            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.Default_Font.Name)
            {
                this.font = this.gui.GuiTheme.Default_Font;
                this.UpdateTabsHeight();
            }   
            if (this.c4_BackgroundColor == GuiTheme.DefaultGuiTheme.Default_BackgroundColor)
            {
                this.c4_BackgroundColor = this.gui.GuiTheme.Default_BackgroundColor;
            }    
            base.UpdateTheme(); // Fuerza el establecimiento del area de dibujo interna.     
        }

        protected override void OnReposition()
        {
            foreach (BaseObjects.BaseGuiSurface value in this.d_guiSurfaces.Values)
            {
                value.X = this.X;
                value.Y = 1 + this.Y + (int)this.tabsHeigth - this.MarginBottom;
            }
        }

        protected override void OnResize()
        {
            base.OnResize();
            this.UpdateTabsHeight();
            this.CalculateTabDisplacement();
        }

        private void UpdateTabsHeight()
        {
            this.tabsHeigth = (int)Math.Ceiling(this.font.MaxCharacterHeight*(this.f_fontsize/this.font.MaxFontSize));
            foreach (BaseObjects.BaseGuiSurface value in this.d_guiSurfaces.Values)
            {
                value.Width = this.Width;
                value.Height = (int)(this.Height-(this.tabsHeigth-this.MarginBottom+1));
            }
        }

        protected override void pDraw()
        {
            if (this.b_ShowMe)
            {
                this.gui.GuiDrawer.DrawGL(c4_BackgroundColor,this.i_x, this.i_y, this.Width, this.Height, 0f);
                
                if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
                {
                    this.DrawActiveTab();
                    this.DrawIn(this.i_x,this.i_y+1,(int)this.i_width, (int)this.i_height, DrawTabBar);
                }
            }
        }

        protected override void pDrawID()
        {
            if (this.b_ShowMe)
            {
                dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, this.idColor, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 1); // Pintamos ID de la superficie.

                if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
                {
                    this.DrawActiveTabIDs();
                    this.DrawIdIn(this.i_x,this.i_y+1,(int)this.i_width, (int)this.i_height, DrawTabBarIDs);
                }
            }
        }

        #endregion

        #region PUBLIC METHODS:
        public void AddTabPage(string TabName)
        {
            if (!this.d_Name_Id.ContainsKey(TabName))
            {
                TabPage tp = new TabPage(TabName);
                this.d_Name_Id.Add(TabName, tp.ID);
                this.ActiveTabID = tp.ID;
                tp.X = this.X;
                tp.Y = 1 + this.Y+(int)this.tabsHeigth-this.MarginBottom;
                tp.Width = this.Width;
                tp.Height = (int)(this.Height-(this.tabsHeigth-this.MarginBottom));
                this.l_ClosersIDs.Add(tp.X_ButtonID);
                base.AddSurface((BaseObjects.BaseGuiSurface)tp);

                this.CalculateTotalTabsWidth();
                this.TabDisplacement = (int)((this.totalTabsWidth>this.Width) ? this.Width-this.totalTabsWidth : 0);
            }
        }
        public void AddTabPage(TabPage page, string TabName)
        {
            if (!this.d_Name_Id.ContainsKey(TabName))
            {
                this.d_Name_Id.Add(TabName, page.ID);
                this.ActiveTabID = page.ID;
                page.X = this.X;
                page.Y = 1 + this.Y+this.tabsHeigth-this.MarginBottom;
                page.Width = this.Width;
                page.Height = this.Height-(this.tabsHeigth-this.MarginBottom);
                this.l_ClosersIDs.Add(page.X_ButtonID);
                base.AddSurface((BaseObjects.BaseGuiSurface)page);

                this.CalculateTotalTabsWidth();
                this.TabDisplacement = (this.totalTabsWidth>this.Width) ? this.Width-this.totalTabsWidth : 0;
            }
        }
        public void RemoveTabPage(string TabName)
        {
            if (this.d_Name_Id.ContainsKey(TabName))
            {
                uint idtab = this.d_Name_Id[TabName];
                int posindex = this.VisibleSurfaceOrder.IndexOf(idtab);
                if ((this.ActiveTabID == idtab) && (this.d_Name_Id.Count>1))
                {
                    if (this.VisibleSurfaceOrder.Count > posindex+1)
                    {
                        this.ActiveTabID = this.VisibleSurfaceOrder[posindex+1];
                    }
                    else
                    {
                        this.ActiveTabID = this.VisibleSurfaceOrder[posindex-1];
                    }
                }
                this.l_ClosersIDs.Remove(((TabPage)this.d_guiSurfaces[idtab]).X_ButtonID);
                base.RemoveSurface(this.d_Name_Id[TabName]);
                this.d_Name_Id.Remove(TabName);

                this.CalculateTotalTabsWidth();
                this.CalculateTabDisplacement();
            }            
        }

        public bool Contains(string TabName)
        {
            return this.d_Name_Id.ContainsKey(TabName);
        }

        public bool Contains(TabPage page)
        {
            return this.d_guiSurfaces.ContainsValue(page);
        }

        #endregion

        #region Deshabilitar Metodos

        public override void AddSubControl(BaseObjects.Control control)
        {
            //Eliminamos ejecución de código.
        }

        public override void RemoveSubControl(uint id)
        {
            //Eliminamos ejecución de código.
        }

        protected override void DrawContent()
        {
            //base.DrawContent();
        }

        protected override void pDrawContent()
        {
            //base.DrawContent();
        }

        #endregion

        #region PROPERTIES:

        public Color4 BackgroundColor
        {
            set { this.c4_BackgroundColor = value; }
            get { return this.c4_BackgroundColor; }
        }
        public Orientation TabBarOrientation
        {
            set { this.barOrientation = value; this.UpdateTexCoords(); }
            get { return this.barOrientation; }
        }

        public int Count
        {
            get { return this.d_guiSurfaces.Count; }
        }

        public dgFont Font
        {
            set
            {
                this.font = value;
                this.UpdateTabsHeight();
            }
            get { return this.font; }
        }

        public float FontSize
        {
            set
            {
                this.f_fontsize = value;
                this.UpdateTabsHeight();
            }
            get { return this.f_fontsize; }
        }

        public bool ClosableTabs
        {
            set { this.b_ClosableTabs = value; }
            get { return this.b_ClosableTabs; }
        }
        #endregion
    }
}