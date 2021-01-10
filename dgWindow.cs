using System;
using dgtk;

namespace dge
{
    public partial class dgWindow : dgtk.dgtk_Window
    {
        private SndSystem sndSystem;
        public dgWindow(string Title) : base(1024, 600, Title) // Consuctor Básico.
        {
            sndSystem = new SndSystem(base.OpenALContext);
            this.MakeCurrent();
            G2D.Drawer.Init_2D_Drawer();
            this.UnMakeCurrent(); //No debería ser necesario.
        }

        public SndSystem SoundSystem
        {
            get { return this.sndSystem; }
        }
    }
}