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
        private SndSystem sndSystem;
        public dgWindow(string Title) : base(1024, 600, Title) // Consuctor Básico.
        {
            sndSystem = new SndSystem(base.OpenALContext);
            this.MakeCurrent();
            this.drawer2D = new G2D.Drawer();
            this.GuiDrawer2D = new G2D.GuiDrawer();
            this.GuiDrawer2D.DefinePerspectiveMatrix(0,0,this.Width, this.Height);
            this.writer2D = new G2D.Writer(this);
            this.UnMakeCurrent(); //No debería ser necesario.
            dge.G2D.IDsDrawer.Init_IDs_Drawer(); // Iniciamos Código de visualizado de Ids.
            
        }

        protected override void OnRenderFrame(object sender, dgtk_OnRenderEventArgs e)
        {
            base.OnRenderFrame(sender, e);
            if (this.gui != null)
            {
                gui.Draw(this.GuiDrawer2D);
            }
        }

        public SndSystem SoundSystem
        {
            get { return this.sndSystem; }
        }

        public GUI.GraphicsUserInterface GUI
        {
            set { this.gui = value; this.gui.ParentWindow = this; }
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
    }
}