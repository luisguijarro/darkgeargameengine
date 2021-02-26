using System;
using System.Collections.Generic;
using dgtk.Graphics;

namespace dge.GUI
{
    public class Window : dge.GUI.BaseObjects.BaseGuiSurface
    {
        public Window() : base(480, 270)
        {
            this.MarginsFromTheEdge = new dgtk.Math.Vector4(2, 22, 2, 2);
            float mult = (1f/256f);
            this.Texcoords = new float[]
            {
                mult*2f, mult*3, mult*21, mult*23f, 
                mult*121f, mult*143f, mult*163, mult*165f
            };
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