using System;
using System.Collections.Generic;
using dgtk.Graphics;

namespace dge.GUI
{
    public class Window : dge.GUI.BaseObjects.BaseGuiSurface
    {
        private GraphicsUserInterface gui;
        public Window() : base(480, 270)
        {
            
        }


        public void AddControl(BaseObjects.Control control)
        {
            base.AddSurface((BaseObjects.BaseGuiSurface)control);
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
            }
            get { return base.GUI; }
        }
    }
}