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
        protected dgtk.Graphics.Color4 c4_textColor;
        protected dgtk.Graphics.Color4 c4_textBorderColor;
        protected string s_name;
        protected bool b_opened;
        protected float tx_x, tx_y; // Coordenadas de texto
        protected Dictionary<string, uint> d_IdByName;
        protected bool IsMain;
        protected uint maxwidth; // Maxima anchura de los subelementos. Se emplea en el pintado del marco.
        protected uint maxheight; // Maxima altura de los subelementos. Se emplea en el pintado del marco.
        public MenuItem(string text) : base()
        {
            this.IsMain = false;
            this.s_text = text;
            this.s_name = text;
            this.b_opened = false;
            this.b_textBorder = false;
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

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                if ((this.b_opened) && (this.d_guiSurfaces.Count<=0))
                {
                    this.Opened = false;
                    this.CloseParents();
                    base.OnMUp(sender, e);
                }                
            }
            else
            {                
                this.Opened = this.IsChildID(dge.Core2D.SelectedID);
            }
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
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
            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.DefaultFont.Name)
            {
                this.font = this.gui.GuiTheme.DefaultFont;
            }   
            this.RepositionMenus();  
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
                if (this.VisibleSurfaceOrder.Count > 0) {this.pDrawBorder(); }
                for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
                {
                    this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].Draw();
                }
            }
        }

        protected virtual void pDrawBorder()
        {
            this.gui.gd_GuiDrawer.DrawGL(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO.ID, dgtk.Graphics.Color4.White, (int)(this.X+this.Width+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[2]), this.Y, (uint)(maxwidth+(this.gui.gt_ActualGuiTheme.Menu_BordersWidths[0]+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[2])), (uint)(maxheight+(this.gui.gt_ActualGuiTheme.Menu_BordersWidths[1]+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[3])), 0f, this.gui.gt_ActualGuiTheme.Menu_BordersWidths, this.gui.gt_ActualGuiTheme.Menu_Border_Texcoords, new float[]{0f,0f}, 0);
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
                this.gui.Writer.Write(this.font, (this.b_IsEnable) ? this.c4_textColor : this.gui.GuiTheme.DefaultDisableTextColor, " "+this.s_text+" ", f_fontSize, tx_x, tx_y, (this.b_IsEnable) ? this.c4_textBorderColor : this.gui.GuiTheme.DefaultDisableTextColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, (this.b_IsEnable) ? this.c4_textColor : this.gui.GuiTheme.DefaultDisableTextColor, " "+this.s_text+" ", f_fontSize, tx_x, tx_y);
            }
            if (!this.IsMain) { this.DrawOpenCloseIcon(); }
        }

        private void DrawOpenCloseIcon()
        {
            if (this.d_guiSurfaces.Count>0)
            {
                if (this.b_opened)
                {
                    this.gui.Drawer.Draw(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO, (int)(this.Width-(this.Height+this.MarginRight)), -this.MarginTop, this.Height, this.Height, 0f, 
                    this.gui.gt_ActualGuiTheme.Menu_Opened_icon_Texcoords[0],
                    this.gui.gt_ActualGuiTheme.Menu_Opened_icon_Texcoords[1],
                    this.gui.gt_ActualGuiTheme.Menu_Opened_icon_Texcoords[2],
                    this.gui.gt_ActualGuiTheme.Menu_Opened_icon_Texcoords[3]);
                }
                else
                {
                    this.gui.Drawer.Draw(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO, (int)(this.Width-(this.Height+this.MarginRight)), -this.MarginTop, this.Height, this.Height, 0f, 
                    this.gui.gt_ActualGuiTheme.Menu_Closed_icon_Texcoords[0],
                    this.gui.gt_ActualGuiTheme.Menu_Closed_icon_Texcoords[1],
                    this.gui.gt_ActualGuiTheme.Menu_Closed_icon_Texcoords[2],
                    this.gui.gt_ActualGuiTheme.Menu_Closed_icon_Texcoords[3]);
                }
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
            float iconsize = ((this.d_guiSurfaces.Count>0) && !this.IsMain) ? textSize[1] : 0;
            this.ui_width = (uint)(textSize[0]+(this.MarginLeft+this.MarginRight)+iconsize);
            this.ui_height = (uint)(textSize[1]+(this.MarginTop+this.MarginBottom));

            this.UpdateTextCoords();
        }

        internal virtual void RepositionMenus()
        {
            /*uint*/ this.maxwidth = 0;
            this.maxheight=0;
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                ((MenuItem)(this.d_guiSurfaces[this.VisibleSurfaceOrder[i]])).Text = ((MenuItem)(this.d_guiSurfaces[this.VisibleSurfaceOrder[i]])).Text;
                uint tmpwidth = this.d_guiSurfaces[this.VisibleSurfaceOrder[i]].Width;
                this.maxwidth = (tmpwidth > this.maxwidth) ? tmpwidth : this.maxwidth;
            }
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                MenuItem item = (MenuItem)(this.d_guiSurfaces[this.VisibleSurfaceOrder[i]]);
                item.X = (int)(this.X+this.Width+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[0]+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[2]);
                item.Y = (int)(this.Height*(i))+this.Y+this.gui.gt_ActualGuiTheme.Menu_BordersWidths[1];
                item.Width = this.maxwidth;
                this.maxheight+=item.Height;
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

        public bool DrawTextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
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
