using System;
using System.Collections.Generic;
using dgtk.Graphics;

namespace dge.GUI.BaseObjects
{
    public class Control : dge.GUI.BaseObjects.BaseGuiSurface
    {
        //private Control parentcontrol;
        private Window parentwindow;
        public Control() : this(160, 90)
        {            
            
        }

        public Control(int witdh, int height) : base(witdh, height)
        {
            //this.textureBufferObject = this.gui.GuiTheme.ThemeTBO; // Provisional
        }

        #region Public Methods:


        public virtual void AddSubControl(Control control)
        {
           base.AddSurface((BaseGuiSurface) control);
        }


        public virtual void RemoveSubControl(uint id)
        {
            base.RemoveSurface(id);
        }

        public virtual void RemoveSubControl(Control control)
        {
            this.RemoveSubControl(control.ui_id); // Eliminar Control Hijo.
        }

        #endregion

        #region PROPERTIES:

        public Control ParentControl
        {
            set 
            { 
                if (this.ParentGuiSurface != null)
                {
                    this.ParentControl.RemoveSurface(this.ui_id); // Si nuestro Padre cambia, Sacar d ela lista del Padre anterior.
                }
                this.ParentGuiSurface = value; 
            }
            get { return (Control)this.ParentGuiSurface; }
        }

        public Window ParentWindow
        {
            set 
            { 
                if (this.parentwindow != null)
                {
                    this.ParentWindow.RemoveControl(this.ui_id); // Si nuestro Padre cambia, Sacar d ela lista del Padre anterior.
                }
                this.parentwindow = value; 
            }
            get { return this.parentwindow; }
        }

        #endregion
    }
}