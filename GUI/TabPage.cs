using System;
using dgtk.Graphics;

namespace dge.GUI
{
    public class TabPage : BaseObjects.Control
    {
        string s_name;
        //internal Button Xbutton;
        internal uint X_ButtonID;
        internal Color4 X_ColorID;
        private bool X_Pressed;
        //private float[] XB_Texcoords;
        internal uint ui_TabWidth;
        internal int i_TabX;
        private Color4 c4_MouseOnX_Color;
        private bool b_MouseOnX;
        public TabPage(string name) : base()
        {
            this.s_name = name;
            this.X_Pressed = false;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TabPage_Surface_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.TabPage_Surface_Texcoords;
            this.c4_MouseOnX_Color = GuiTheme.DefaultGuiTheme.TabPage_X_MouseOnColor;
            this.tcFrameOffset = new float[]{0f,0f};
            this.X_ButtonID = Core2D.GetID();
            this.X_ColorID = new Color4(dge.Core2D.DeUIntAByte4(this.X_ButtonID));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {                
                Core2D.ReleaseID(this.X_ButtonID); // Liberamos ID de la superficie.
                #if DEBUG
                    Console.WriteLine("Liberamos ID de Botón X de TabPage");
                #endif
            }
        }


        protected override void MDown(object sender, MouseButtonEventArgs e)
        {
            base.MDown(sender, e);
            if (Core2D.SelectedID == this.X_ButtonID)
            {
                this.X_Pressed = true;
            }
        }

        protected override void MUp(object sender, MouseButtonEventArgs e)
        {
            base.MUp(sender, e);
            this.X_Pressed = false;
            if (Core2D.SelectedID == this.X_ButtonID)
            {                                                                                                                                      
                ((TabControl)this.parentGuiSurface).RemoveTabPage(this.Name);
                this.Dispose();
            }
        }

        protected override void MMove(object sender, MouseMoveEventArgs e)
        {
            //base.MMove(sender, e);
            this.b_MouseOnX = (Core2D.SelectedID == this.X_ButtonID);
        }

        internal void Draw_X_Button(int x, int y)
        {
            if (this.X_Pressed)
            {
                this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO.ID, (this.b_MouseOnX) ? this.c4_MouseOnX_Color : Color4.White, x-gui.GuiTheme.TabPage_X_Size[0], y, (uint)this.gui.GuiTheme.TabPage_X_Size[0], (uint)this.gui.GuiTheme.TabPage_X_Size[1], 0f, 
                this.gui.GuiTheme.TabPage_X_FrameOffset[0]+this.gui.GuiTheme.TabPage_X_Texcoords[0], 
                this.gui.GuiTheme.TabPage_X_FrameOffset[1]+this.gui.GuiTheme.TabPage_X_Texcoords[1], 
                this.gui.GuiTheme.TabPage_X_FrameOffset[0]+this.gui.GuiTheme.TabPage_X_Texcoords[2], 
                this.gui.GuiTheme.TabPage_X_FrameOffset[1]+this.gui.GuiTheme.TabPage_X_Texcoords[3]);
            }
            else
            {
                this.gui.Drawer.Draw(this.gui.GuiTheme.tbo_ThemeTBO.ID, (this.b_MouseOnX) ? this.c4_MouseOnX_Color : Color4.White, x-gui.GuiTheme.TabPage_X_Size[0], y, (uint)this.gui.GuiTheme.TabPage_X_Size[0], (uint)this.gui.GuiTheme.TabPage_X_Size[1], 0f, this.gui.GuiTheme.TabPage_X_Texcoords[0], this.gui.GuiTheme.TabPage_X_Texcoords[1], this.gui.GuiTheme.TabPage_X_Texcoords[2], this.gui.GuiTheme.TabPage_X_Texcoords[3]);
            }
        }

        internal void Draw_X_Button_ID(int x, int y)
        {
            dge.G2D.IDsDrawer.DrawGL2D(this.X_ColorID, x-gui.GuiTheme.TabPage_X_Size[0], y, (uint)this.gui.GuiTheme.TabPage_X_Size[0], (uint)this.gui.GuiTheme.TabPage_X_Size[1], 0f);
        }

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();
            this.MarginsFromTheEdge = this.gui.GuiTheme.TabPage_Surface_MarginsFromTheEdge;            
            this.Texcoords = this.gui.GuiTheme.TabPage_Surface_Texcoords;
            this.c4_MouseOnX_Color = this.gui.GuiTheme.TabPage_X_MouseOnColor;
        }

        public string Name
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }
    }
}