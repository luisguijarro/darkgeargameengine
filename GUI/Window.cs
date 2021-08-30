using System;
using System.Collections.Generic;
using dgtk.Graphics;
using dgtk.OpenGL;

using dge.G2D;
using dgtk;

namespace dge.GUI
{
    public class Window : dge.GUI.BaseObjects.BaseGuiSurface
    {
        //private bool b_ShowTitleBar; //Indica si se muestra o no la barra de título..
        private bool b_full_id; //Indica si la parte interactiva del control es igual a toda su superficie.
        private bool b_TitlePulsed;
        private int LastMPosX; // ultima posicion del ratçon registrada.
        private int LastMPosY; // ultima posicion del ratçon registrada.
        private string s_text;
        private dgtk.Graphics.Color4 c4_textColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private bool b_textBorder;
        private float f_FontSize;
        private dgFont font;
        protected Button CloseButton;
        private Menu m_menu;

        public Window() : this(480, 270)
        {

        }

        public Window(uint width, uint height) : base(width, height)
        {
            //this.b_ShowTitleBar = true;
            this.b_full_id = false;
            this.b_TitlePulsed = false;
            this.s_text = "Window";
            this.f_FontSize = 16;
            this.c4_textColor = dgtk.Graphics.Color4.Black;
            this.c4_textBorderColor = dgtk.Graphics.Color4.Black;
            this.b_textBorder = true;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Window_MarginsFromTheEdge;
            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;
            //float mult = (1f/256f);
            this.Texcoords = GuiTheme.DefaultGuiTheme.Window_Texcoords;
            this.tcFrameOffset  = GuiTheme.DefaultGuiTheme.Window_FrameOffset;

            this.CloseButton = new Button(17, 17, "X");
            this.CloseButton.GUI = this.gui;
            this.CloseButton.intX = this.i_x+this.int_x;
            this.CloseButton.intY = this.i_y+this.int_y;
            this.CloseButton.X = (int)this.Width-20;
            this.CloseButton.Y = 3;
            this.CloseButton.FontSize = 14;
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.Window_MarginsFromTheEdge;
            this.font = this.gui.gt_ActualGuiTheme.DefaultFont;
            this.Texcoords = this.gui.gt_ActualGuiTheme.Window_Texcoords;
            this.tcFrameOffset  = this.gui.gt_ActualGuiTheme.Window_FrameOffset;

        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);
            if (Core2D.SelectedID == this.ui_id)
            {
                int absX = (this.i_x+this.int_x);
                int absY = (this.i_y+this.int_y);
                if ((e.X>absX) && (e.X<absX+this.Width))
                {
                    if ((e.Y>absY) && (e.Y<absY+this.MarginTop))
                    {
                        this.LastMPosX = e.X;
                        this.LastMPosY = e.Y;
                        this.b_TitlePulsed = true;
                    }
                }
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMUp(sender, e);
            if (Core2D.SelectedID == this.ui_id)
            {
                this.b_TitlePulsed = false;
            }
        }

        protected override void OnMMove(object sender, MouseMoveEventArgs e)
        {
            base.OnMMove(sender, e);
            if (Core2D.SelectedID == this.ui_id)
            {
                if (this.b_TitlePulsed)
                {
                    this.X += (e.X-this.LastMPosX);
                    this.Y += (e.Y-this.LastMPosY);
                    this.LastMPosX = e.X;
                    this.LastMPosY = e.Y;
                }
            }
        }

        private void DrawText()
        {
            if (this.b_textBorder)
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, this.f_FontSize, 4, 2, this.c4_textBorderColor);
            }
            else
            {
                this.gui.Writer.Write(this.font, this.c4_textColor, this.s_text, this.f_FontSize, 4, 0);
            }
        }

        protected override void OnReposition()
        {
            //base.OnReposition();
            foreach(BaseObjects.BaseGuiSurface surf in this.d_guiSurfaces.Values)
            {
                surf.intX = this.int_x+this.i_x+this.MarginsFromTheEdge[0];
                surf.intY = this.int_y+this.i_y+this.MarginsFromTheEdge[1];
            }
            this.CloseButton.intX = this.int_x+this.i_x;
            this.CloseButton.intY = this.int_y+this.i_y;
        }

        protected override void pDraw()
        {
            
            this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x+this.int_x, this.i_y+this.int_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            /*
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                DrawIn(this.i_x+this.int_x+(int)this.MarginsFromTheEdge[0], this.i_y+this.int_y+(int)this.MarginsFromTheEdge[1], (int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawContent);
            }
            */
            base.DrawIn(this.int_x+this.i_x+this.MarginLeft, this.int_y+this.i_y,(int)this.ui_width-(this.MarginLeft+this.MarginRight), (int)this.MarginsFromTheEdge[1], DrawText);

            if (this.CloseButton.Visible)
            {
                base.DrawIn(this.int_x+this.i_x+this.MarginLeft, this.int_y+this.i_y,(int)this.ui_width-(this.MarginLeft+this.MarginRight), (int)this.MarginsFromTheEdge[1], this.CloseButton.Draw);
            }
        }

        private void minidrawId() // Encapsulamos el metodo de pintando para controlar el area en el que se pinta.
        {
            dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, this.idColor, 0, (int)(0), this.ui_width, (uint)(this.ui_height), 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 1); // Pintamos ID de la superficie.
        }

        protected override void pDrawID()
        {
            //Pintamos ID solo en la parte de la barra de título si así está establecido el atributo "this.b_full_id".
            //base.pDrawID();
            base.DrawIdIn(this.int_x+this.i_x, this.int_y+this.i_y,(int)this.ui_width, (this.b_full_id ? (int)this.ui_height : (int)this.MarginsFromTheEdge[1]), minidrawId);
            /*
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                DrawIdIn(this.int_x+this.i_x+(int)this.MarginsFromTheEdge[0], this.int_y+this.i_y+(int)this.MarginsFromTheEdge[1], (int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawContentIDs);
            }
            */
            base.DrawIdIn(this.int_x+this.i_x, this.int_y+this.i_y,(int)this.ui_width, (int)this.MarginsFromTheEdge[1], this.CloseButton.DrawID);
        }

        public void AddControl(BaseObjects.Control control)
        {
            base.AddSurface((BaseObjects.BaseGuiSurface)control);
            this.OnReposition();
        }

        public void RemoveControl(uint id)
        {
            base.RemoveSurface(id);
        }

        public void RemoveControl(BaseObjects.Control control)
        {
            this.RemoveControl(control.ID); // Eliminar Control Hijo.
        }

        public GraphicsUserInterface GraphicsUserInterface
        {
            set 
            { 
                base.GUI = value;
                this.CloseButton.GUI = this.gui;
            }
            get { return base.GUI; }
        }
        public dgtk.Graphics.Color4 TextColor
        {
            set { this.c4_textColor = value; }
            get { return this.c4_textColor; }
        }

        public dgtk.Graphics.Color4 TextBorderColor
        {
            set { this.c4_textColor = value; }
            get { return this.c4_textColor; }
        }

        public bool TextBorder
        {
            set { this.b_textBorder = value; }
            get { return this.b_textBorder; }
        }

        public float FontSize 
        {
            set { this.f_FontSize = value; }
            get { return this.f_FontSize; }
        }

        public string Title
        {
            set { this.s_text = value; }
            get { return this.s_text; }
        }

        public Menu MainMenu
        {
            set 
            { 
                if (this.m_menu != null)
                {
                    this.m_menu.ParentGuiSurface = null;
                }
                this.m_menu = value; 
                this.m_menu.ParentGuiSurface = this;
                this.m_menu.GUI = this.gui;
            }
            get { return this.m_menu; }
        }
    }
}