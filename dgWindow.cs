using System;
using dgtk;

namespace dge
{
    public class dgWindow : dgtk.dgtk_Window
    {
        public dgWindow(string Title) : base(1024, 600, Title) // Consuctor Básico.
        {
            this.MakeCurrent();
            G2D.Drawer.Init_2D_Drawer();
            this.UnMakeCurrent(); //No debería ser necesario.
        }

        /*protected override void GLConfig()
        {

        }*/
    }
}