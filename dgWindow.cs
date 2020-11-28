using System;
using dgtk;

namespace dge
{
    public class dgWindow : dgtk.dgtk_Window
    {
        public dgWindow(string Title) : this(1024, 600, Title) // Consuctor Básico.
        {
            
        }

        public dgWindow(uint Width, uint Height, string Title) : base(Width, Height, Title) //Constructor completo.
        {
            this.MakeCurrent();
            G2D.Drawer.Init_2D_Drawer();
            this.UnMakeCurrent(); //No debería ser necesario.
        }
    }
}