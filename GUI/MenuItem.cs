using System;
using System.Collections.Generic;

using dge.GUI.BaseObjects;

namespace dge.GUI
{
    public class MenuItem : BaseObjects.BaseGuiSurface
    {
        protected dge.G2D.dgFont font;
        protected float f_fontSize;
        protected string s_text;
        internal bool b_textBorder;
        private dgtk.Graphics.Color4 c4_textColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        protected string s_name;
        protected bool b_opened;
        protected float tx_x, tx_y; // Coordenadas de texto
        protected Dictionary<string, uint> d_IdByName;
        public MenuItem(string text) : base()
        {
            this.s_text = text;
            this.s_name = text;
            this.b_opened = false;
            this.d_IdByName = new Dictionary<string, uint>();
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Menu_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.Menu_Texcoords;
            this.tcFrameOffset = new float[]{0f,0f}; //GuiTheme.DefaultGuiTheme.Menu_FrameOffset;

            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.f_fontSize = 12;
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;

            this.UpdateSizeFromText();
        }

        #region METODOS PUBLIC:

        public virtual void Add(string menuName)
        {
            this.Add(new MenuItem(menuName));       
        }

        public virtual void Add(MenuItem menu)
        {
            if (menu.GetType() == typeof(MenuItem))
            {
                base.AddSurface((BaseGuiSurface) menu);
                this.d_IdByName.Add(menu.s_name, menu.ui_id);
            }   
            this.RepositionMenus();        
        }

        public virtual MenuItem GetItem(string itemName)
        {
            if (this.d_IdByName.ContainsKey(itemName))
            {
                return (MenuItem)this.d_guiSurfaces[this.d_IdByName[itemName]];
            }
            return null;
        }

        #endregion

        #region EVENTOS:

        protected override void MUp(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                if ((this.b_opened) && (this.d_guiSurfaces.Count<=0))
                {
                    this.Opened = false;
                    this.CloseParents();
                    base.MUp(sender, e);
                }                
            }
            else
            {                
                this.Opened = this.IsChildID(dge.Core2D.SelectedID);
            }
        }

        protected override void MDown(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.Opened = !this.Opened;
            }
            else
            {                
                this.Opened = this.IsChildID(dge.Core2D.SelectedID);
            }
            
        }

        #endregion

        protected virtual void CloseParents()
        {
            ((MenuItem)this.parentGuiSurface).Opened = false;
            ((MenuItem)this.parentGuiSurface).CloseParents();
        }

        protected bool IsChildID(uint id)
        {
            if ((id == this.ui_id)) // && (this.d_guiSurfaces.Count>0))
            {
                return true;
            }
            else
            {
                if (this.d_guiSurfaces.Count>0)
                {
                    foreach(MenuItem mi in this.d_guiSurfaces.Values)
                    {
                        if (mi.IsChildID(id))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }               
            }
            return false;
        }

        protected override void OnGuiUpdate()
        {
            this.UpdateSizeFromText();
        }

        protected override void pDraw()
        {
            base.pDraw();
            if (this.gui != null)
            {
                //DrawText();
                this.DrawIn(this.X+(int)this.MarginLeft,this.Y+(int)this.MarginTop,(int)this.ui_width, (int)this.ui_height, DrawText);
            }
        }

        protected override void pDrawContent()
        {
            if (this.b_opened)
            {
                for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
                {
                    this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].Draw();
                }
            }
        }

        protected override void pDrawContentID()
        {
            if (this.b_opened)
            {
                for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
                {
                    this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].DrawID();
                }
            }
        }

        protected virtual void UpdateTextCoords()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    float tWidth = G2D.Writer.MeasureString(this.font, " "+this.s_text+" ", this.f_fontSize)[0]; //Obtenemos tamaño de texto.
                    this.tx_x = ((this.ui_width/2f) - (tWidth/2f));
                    this.tx_y = (this.ui_height/2.0f) - (this.f_fontSize/1.2f);
                }
            }
        }

        protected virtual void DrawText()
        {
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, " "+this.s_text+" ", f_fontSize, tx_x, tx_y, this.c4_textBorderColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, " "+this.s_text+" ", f_fontSize, tx_x, tx_y);
            }
        }

        internal virtual void UpdateSizeFromText()
        {
            if (this.gui == null)
            {
                this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            }
            else
            {
                if (this.font.TBO_Scan0.ID == 0) // si no tiene fuente asignada.
                {
                    this.font = this.gui.GuiTheme.DefaultFont;
                }
            }
            float[] textSize = dge.G2D.Writer.MeasureString(this.font, " " + this.s_text + " ", this.f_fontSize);
            this.ui_width = (uint)(textSize[0]+(this.MarginLeft+this.MarginRight));
            this.ui_height = (uint)(textSize[1]+(this.MarginTop+this.MarginBottom));

            this.UpdateTextCoords();
        }

        internal virtual void RepositionMenus()
        {
            uint maxwidth = 0;
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                ((MenuItem)(this.d_guiSurfaces[this.VisibleSurfaceOrder[i]])).Text = ((MenuItem)(this.d_guiSurfaces[this.VisibleSurfaceOrder[i]])).Text;
                uint tmpwidth = this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].Width;
                maxwidth = (tmpwidth > maxwidth) ? tmpwidth : maxwidth;
            }
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                MenuItem item = (MenuItem)(this.d_guiSurfaces[this.VisibleSurfaceOrder[i]]);
                item.X = (int)(this.X+this.Width);
                item.Y = (int)(this.Height*(i))+this.Y;
                item.Width = maxwidth;
            }
        }

        #region PROPERTIES:

        public string Text
        {
            set { this.s_text = value; }
            get { return this.s_text; }
        }

        public string Name
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }

        protected virtual bool Opened
        {
            set
            {
                this.b_opened = value;
                if (this.b_opened)
                {
                    this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Menu_FrameOffset;
                }
                else
                {
                    this.tcFrameOffset = new float[]{0f,0f};
                    //
                }
                // = this.b_opened;
            }
            get { return this.b_opened; }
        }

        #endregion
    }
}

/*
using System;
using System.Collections.Generic;

namespace dge.GUI
{
    public class MenuItem : BaseObjects.Control
    {
        public Dictionary<string,uint> d_MenuItems;
        public List<string> l_orderItems;
        protected dge.G2D.dgFont font;
        protected float f_fontSize;
        internal bool b_opened;
        protected string s_text;
        private string s_name;
        protected float tx_x, tx_y; // Coordenadas de texto
        private dgtk.Graphics.Color4 c4_textColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private bool b_textBorder;
        private bool FirsDraw;
        
        public MenuItem(string text, string name) : base()
        {
            this.s_text = text;
            this.s_name = name;
            this.d_MenuItems = new Dictionary<string,uint>();
            this.l_orderItems = new List<string>();

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Menu_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.Menu_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Menu_FrameOffset;

            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            this.f_fontSize = 12;
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;

            this.b_textBorder = false;
            this.FirsDraw = true;

            this.UpdateSizeFromText();
            this.UpdateTextCoords();
        }

        public void Add(string ItemText)
        {
            this.Add(new MenuItem(ItemText, ItemText));
        }

        public void Add(MenuItem item)
        {
            if (!this.d_MenuItems.ContainsKey(item.Name))
            {
                item.ParentGuiSurface = this;
                this.d_guiSurfaces.Add(item.ID, item);
                this.d_MenuItems.Add(item.Name, item.ID);
                this.l_orderItems.Add(item.Name);
                this.OnReposition();
            }
        }

        public MenuItem GetItem(string ItemName)
        {
            if (this.d_MenuItems.ContainsKey(ItemName))
            {
                return (MenuItem)this.d_guiSurfaces[this.d_MenuItems[ItemName]];
            }
            return null;
        }


        protected override void MUp(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.b_opened = !this.b_opened!;
                base.MUp(sender, e);
            }
            else
            {
                this.b_opened = false;
            }
        }


        private void DrawText()
        {
            float f_fs = this.b_opened ? this.f_fontSize-1 : f_fontSize;
            float px = this.tx_x;
            float py = this.tx_y;
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, " "+this.s_text+" ", f_fs, px, py, this.c4_textBorderColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, " "+this.s_text+" ", f_fs, px, py);
            }
        }

        protected override void pDraw()
        {
            if (this.b_ShowMe)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, dgtk.Graphics.Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_opened? this.tcFrameOffset : new float[]{0f,0f}, 0);
            }
            if (this.FirsDraw) { this.OnReposition(); this.FirsDraw = false; };
            if (this.gui != null)
            {
                //DrawText();
                this.DrawIn(this.X+(int)this.MarginLeft,this.Y+(int)this.MarginTop,(int)this.ui_width, (int)this.ui_height, DrawText);
            }

            if (this.b_opened)
            {
                for (int i=0;i<this.l_orderItems.Count;i++)
                {
                    this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]].Draw();
                }
            }
        }

        protected override void pDrawID()
        {
            base.pDrawID();
            if (this.b_opened)
            {
                for (int i=0;i<this.l_orderItems.Count;i++)
                {
                    this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]].DrawID();
                }
            }
        }

        protected virtual void UpdateSizeFromText()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    float tWidth = G2D.Writer.MeasureString(this.font, this.s_text, this.f_fontSize)[0]; //Obtenemos tamaño de texto.
                    this.ui_width = (uint)(tWidth+(this.font.f_spaceWidth*2)+this.MarginLeft+this.MarginRight);
                    this.ui_height = (uint)((this.font.MaxCharacterHeight*(this.f_fontSize / this.font.f_MaxFontSize)) + this.MarginTop +  this.MarginBottom);
                    //this.tx_x = ((this.ui_width/2f) - (tWidth/2f));
                    //this.tx_y = (this.ui_height/2.0f) - (this.f_fontSize/1.2f);
                    //this.OnReposition();
                }
            }
        }

        protected virtual void UpdateTextCoords()
        {
            if (this.gui != null)
            {
                if (this.gui.Writer != null)
                {
                    float tWidth = G2D.Writer.MeasureString(this.font, " "+this.s_text+" ", this.f_fontSize)[0]; //Obtenemos tamaño de texto.
                    this.tx_x = ((this.ui_width/2f) - (tWidth/2f));
                    this.tx_y = (this.ui_height/2.0f) - (this.f_fontSize/1.2f);
                }
            }
            //this.OnReposition();
        }

        protected override void OnResize()
        {
            //base.OnResize();
            this.UpdateTextCoords();
        }

        protected override void OnReposition()
        {
            uint maxwidth = 0;
            for (int i=0;i<this.l_orderItems.Count;i++)
            {
                ((MenuItem)(this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]])).Text = ((MenuItem)(this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]])).Text;
                uint tmpwidth = this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]].Width;
                maxwidth = (tmpwidth > maxwidth) ? tmpwidth : maxwidth;
            }
            for (int i=0;i<this.l_orderItems.Count;i++)
            {
                MenuItem item = (MenuItem)(this.d_guiSurfaces[this.d_MenuItems[this.l_orderItems[i]]]);
                item.X = (int)(this.X+this.Width);
                item.Y = (int)(this.Height*(i))+this.Y;
                item.Width = maxwidth;
            }
        }

        #region PROPERTIES:

        public dge.G2D.dgFont Font
        {
            get { return this.font; }
            set 
            { 
                this.font = value;  
                this.UpdateSizeFromText(); 
            }
        }

        public float FontSize
        {
            set 
            { 
                this.f_fontSize = value; 
                this.UpdateSizeFromText(); 
            }
            get { return this.f_fontSize; }
        }

        public string Name
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }

        public string Text
        {
            set 
            { 
                this.s_text = value; 
                this.UpdateSizeFromText();
            }
            get { return this.s_text; }
        }

        public bool ShowTextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
        }

        #endregion
    }
}
*/