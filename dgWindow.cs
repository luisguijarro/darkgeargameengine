using System;
using dgtk;

namespace dge
{
    public partial class dgWindow : dgtk.dgtk_Window
    {
        private readonly G2D.Drawer drawer2D;
        private readonly G2D.GuiDrawer GuiDrawer2D;
        private readonly G2D.Writer writer2D;
        private GUI.GraphicsUserInterface gui;
        private Scene scn_escene;
        private readonly SndSystem sndSystem;
        public dgWindow(string Title) : this(1024, 600, Title) // Consuctor Básico.
        {
                        
        }

        public dgWindow(int width, int height, string Title) : base(width, height, Title) // Consuctor Básico.
        {
            Core.LockObject = base.LockObject;
            dge.SoundSystem.SoundTools.InitStaticSoundSystem();
            sndSystem = new SndSystem(dge.SoundSystem.SoundTools.OpenAL_Context);

            //this.MakeCurrent();
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            dge.G2D.IDsDrawer.Init_IDs_Drawer(); // Iniciamos Código de visualizado de Ids.
            dge.G2D.IDsDrawer.DefinePerspectiveMatrix(0,0,this.Width, this.Height, true);
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();

            this.MakeCurrent();
            this.drawer2D = new G2D.Drawer();
            this.drawer2D.DefinePerspectiveMatrix(0,0,this.Width, this.Height, true);
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
                this.gui.Width = e.Width;
                this.gui.Height = e.Height;
            }
        }

        protected override void OnKeyPulsed(object sender, dgtk_KeyBoardKeysEventArgs e)
        {
            base.OnKeyPulsed(sender, e);
            
            if (System.IO.File.Exists("config.dge"))
            {

            }
            else
            {
                if (e.KeyStatus.KeyCode == KeyCode.F12)
                {
                    DateTime now = DateTime.Now;
                    string name = now.ToString("dd-MM-yyyy_" + now.ToString("T"));
                    dge.G2D.Tools.SaveScreenShot(name+".png", this);
                }
                if (e.KeyStatus.KeyCode == KeyCode.F11)
                {
                    this.FullScreen = !this.FullScreen;
                }
            }
        }

        protected override void OnRenderFrame(object sender, dgtk_OnRenderEventArgs e)
        {
            base.OnRenderFrame(sender, e);
            if (this.scn_escene != null)
            {
                this.scn_escene.InternalDraw(this.drawer2D);
            }
            if (this.gui != null)
            {
                gui.Draw();
            }
        }

        protected override void OnUpdateFrame(object sender, dgtk_OnUpdateEventArgs e)
        {
            base.OnUpdateFrame(sender, e);
            if (this.scn_escene != null)
            {
                this.scn_escene.InternalUpdate();
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
                if (this.gui != null)
                {
                    this.gui.iParentWindow = null;
                    this.gui.gd_GuiDrawer = null;
                    this.gui.Writer = null;
                    this.gui.Drawer = null;
                }
                this.gui = value; 
                this.gui.iParentWindow = this; 
                this.gui.gd_GuiDrawer = this.GuiDrawer2D;
                this.gui.Writer = this.writer2D;
                this.gui.Drawer = this.drawer2D;
                this.gui.Width = this.Width;
                this.gui.Height = this.Height;
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
            set 
            { 
                if (this.scn_escene != null)
                {
                    this.scn_escene.RemParentWindow();
                }
                this.scn_escene = value; 
                this.scn_escene.SetParentWindow(this); 
            }
            get { return this.scn_escene; }
        }
    }
}