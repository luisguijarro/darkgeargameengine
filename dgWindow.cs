using System;
using dgtk;

namespace dge
{
    public partial class dgWindow : dgtk.dgtk_Window
    {
        private G2D.Drawer drawer2D;
        private G2D.GuiDrawer GuiDrawer2D;
        private G2D.Writer writer2D;
        private GUI.GraphicsUserInterface gui;
        private Scene scn_escene;
        private SndSystem sndSystem;
        public dgWindow(string Title) : this(1024, 600, Title) // Consuctor Básico.
        {
                        
        }

        public dgWindow(uint width, uint height, string Title) : base(width, height, Title) // Consuctor Básico.
        {
            Core.LockObject = base.LockObject;
            dge.SoundSystem.SoundTools.InitStaticSoundSystem();
            sndSystem = new SndSystem(base.OpenALContext);

            dge.G2D.IDsDrawer.Init_IDs_Drawer(); // Iniciamos Código de visualizado de Ids.
            dge.G2D.IDsDrawer.DefinePerspectiveMatrix(0,0,this.Width, this.Height, true);

            this.MakeCurrent();
            this.drawer2D = new G2D.Drawer();
            this.GuiDrawer2D = new G2D.GuiDrawer();
            this.GuiDrawer2D.DefinePerspectiveMatrix(0,0,this.Width, this.Height);
            this.writer2D = new G2D.Writer();
            this.UnMakeCurrent(); //No debería ser necesario.
        }

        protected override void OnWindowSizeChange(object sender, dgtk_ResizeEventArgs e)
        {
            base.OnWindowSizeChange(sender, e);
            if (this.gui != null)
            {
                this.gui.ui_width = (uint)e.Width;
                this.gui.ui_height = (uint)e.Height;
            }
            while(this.GuiDrawer2D == null)
            {}
            this.GuiDrawer2D.DefinePerspectiveMatrix(0,0,this.Width, this.Height);  
            while(this.Drawer2D == null)
            {}      
            this.Drawer2D.DefinePerspectiveMatrix(0,0,this.Width, this.Height, true);
        }

        protected override void OnRenderFrame(object sender, dgtk_OnRenderEventArgs e)
        {
            base.OnRenderFrame(sender, e);
            if (this.scn_escene != null)
            {
                this.scn_escene.InternalDraw();
            }
            if (this.gui != null)
            {
                gui.Draw();
            }
        }

        public SndSystem SoundSystem
        {
            get { return this.sndSystem; }
        }

        public GUI.GraphicsUserInterface GUI
        {
            set 
            { 
                this.gui = value; 
                this.gui.ParentWindow = this; 
                this.gui.GuiDrawer = this.GuiDrawer2D;
                this.gui.Writer = this.writer2D;
                this.gui.Drawer = this.drawer2D;
            }
            get { return this.gui; }
        }

        public G2D.Drawer Drawer2D
        {
            get { return this.drawer2D; }
        }

        public G2D.Writer Writer2D
        {
            get { return this.writer2D; }
        }

        public Scene Scene
        {
            set { this.scn_escene = value; this.scn_escene.SetParentWindow(this); }
            get { return this.scn_escene; }
        }
    }
}