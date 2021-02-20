using System;
using System.Collections.Generic;

using dge.GUI;

namespace dge.GUI.BaseObjects
{
    public class BaseGuiSurface : dge.G2D.InteractiveSurfaceContainer
    {
        private bool b_visible; // ¿Es Visible la ventana?
        private GraphicsUserInterface gui;
        private Dictionary<uint, BaseGuiSurface> d_guiSurfaces; // Todos los Controles de la ventana.
        internal List<uint> VisibleSurfaceOrder; // Orden de los Controles de la ventana.
        private BaseGuiSurface parentGuiSurface;
        
		public event EventHandler<dgtk.dgtk_MouseButtonEventArgs> MouseDown; // Evento que se da cuando se pulsa un botón del ratón.
		public event EventHandler<dgtk.dgtk_MouseButtonEventArgs> MouseUp; // Evento que se da cuando se suelta un botón del ratón.
		public event EventHandler<dgtk.dgtk_MouseMoveEventArgs> MouseMove; // Evento que se da cuando el ratón se mueve.
		public event EventHandler<dgtk.dgtk_MouseWheelEventArgs> MouseWheel; // Evento que se da cuando se acciona la rueda del ratón.		
        public event EventHandler<dgtk.dgtk_KeyBoardKeysEventArgs> KeyPulsed; // Evento que se da cuando se pulsa una tecla del teclado.
		public event EventHandler<dgtk.dgtk_KeyBoardKeysEventArgs> KeyReleased; // Evento que se da cuando se suelta una tecla del teclado.
		public event EventHandler<dgtk.dgtk_KeyBoardTextEventArgs> KeyCharReturned; // Evento devuelto cuando se pulsa o se suelta una tecla y que devuelve el caracter asociado.
		
        public BaseGuiSurface() : this(160, 90)
        {            
            
        }

        public BaseGuiSurface(uint witdh, uint height) : base(witdh, height)
        {
            this.b_visible = true; // El guiSurface es visible por defecto.
            this.d_guiSurfaces = new Dictionary<uint, BaseGuiSurface>();
            this.VisibleSurfaceOrder = new List<uint>();
            this.MouseDown += delegate {}; //Inicialización del evento por defecto.
            this.MouseUp += delegate {}; //Inicialización del evento por defecto.
            this.MouseMove += delegate {}; //Inicialización del evento por defecto.
            this.MouseWheel += delegate {}; //Inicialización del evento por defecto.
            this.KeyPulsed += delegate {}; //Inicialización del evento por defecto.
            this.KeyReleased += delegate {}; //Inicialización del evento por defecto.
            this.KeyCharReturned += delegate {}; //Inicialización del evento por defecto.
        }

        #region Publics

        protected void AddSurface(BaseGuiSurface surface)
        {
            if (!this.d_guiSurfaces.ContainsValue(surface))
            {
                surface.parentGuiSurface = this; // Adoptar guiSurface
                this.d_guiSurfaces.Add(surface.ID, surface); // Añadir guiSurface.
                if (surface.Visible)
                {
                    this.VisibleSurfaceOrder.Add(surface.ID); // Si es Visible que se muestre.
                    this.ContentUpdate = true;
                }
            }
        }

        protected void RemoveSurface(uint id)
        {
            if (this.d_guiSurfaces.ContainsKey(id))
            {
                this.d_guiSurfaces[id].parentGuiSurface = null; // Repudiar guiSurface
                this.d_guiSurfaces.Remove(id); // Eliminar guiSurface
                if (this.VisibleSurfaceOrder.Contains(id))
                {
                    this.VisibleSurfaceOrder.Remove(id); // Sacar de la lista de controles Visibles.
                    this.ContentUpdate = true;
                }
            }
        }

        protected void RemoveSurface(BaseGuiSurface surface)
        {
            this.RemoveSurface(surface.ID);
        }

        protected bool ContainsSurface(uint id)
        {
            return this.d_guiSurfaces.ContainsKey(id);
        }

        protected bool ContainsSurface(BaseGuiSurface surface)
        {
            return this.d_guiSurfaces.ContainsKey(surface.ID);
        }

        protected override void DrawContent(G2D.Drawer drawer)
        {
            base.DrawContent(drawer);
            for (int i=0;i<VisibleSurfaceOrder.Count;i++)
            {
                this.d_guiSurfaces[VisibleSurfaceOrder[i]].Draw(drawer);
            }
        }

        protected override void DrawContentIDs()
        {
            base.DrawContentIDs();
            for (int i=0;i<VisibleSurfaceOrder.Count;i++)
            {
                this.d_guiSurfaces[VisibleSurfaceOrder[i]].DrawID();
            }
        }

        #endregion

        #region Events:

        protected virtual void MDown(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            this.MouseDown(this, e);
        }

        protected virtual void MUp(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            this.MouseUp(this, e);
        }

        protected virtual void MMove(object sender, dgtk.dgtk_MouseMoveEventArgs e)
        {
            this.MouseMove(this, e);
        }

        protected virtual void MWheel(object sender, dgtk.dgtk_MouseWheelEventArgs e)
        {
            this.MouseWheel(this, e);
        }

        protected virtual void KPulsed(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            this.KeyPulsed(this, e);
        }

        protected virtual void KReleased(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            this.KeyReleased(this, e);
        }

        protected virtual void KCharReturned(object sender, dgtk.dgtk_KeyBoardTextEventArgs e)
        {
            this.KeyCharReturned(this, e);
        }

        #endregion

        #region Properties

        internal BaseGuiSurface ParentGuiSurface
        {
            set 
            { 
                if (this.parentGuiSurface != null)
                {
                    this.parentGuiSurface.MouseDown -= MDown;
                    this.parentGuiSurface.MouseUp -= MUp;
                    this.parentGuiSurface.MouseMove -= MMove;
                    this.parentGuiSurface.MouseWheel -= MWheel;
                    this.parentGuiSurface.KeyPulsed -= KPulsed;
                    this.parentGuiSurface.KeyReleased -= KReleased;
                    this.parentGuiSurface.KeyCharReturned -= KCharReturned;
                }
                this.parentGuiSurface = value;
                if (this.parentGuiSurface != null)
                {
                    this.parentGuiSurface.MouseDown += MDown;
                    this.parentGuiSurface.MouseUp += MUp;
                    this.parentGuiSurface.MouseMove += MMove;
                    this.parentGuiSurface.MouseWheel += MWheel;
                    this.parentGuiSurface.KeyPulsed += KPulsed;
                    this.parentGuiSurface.KeyReleased += KReleased;
                    this.parentGuiSurface.KeyCharReturned += KCharReturned;
                }
            }
            get { return this.parentGuiSurface; }
        }

        internal GraphicsUserInterface GUI
        {
            set 
            { 
                if (this.gui!= null)
                {
                    this.gui.RemoveWindow(this.ui_id);
                    this.gui.MouseDown -= MDown;
                    this.gui.MouseUp -= MUp;
                    this.gui.MouseMove -= MMove;
                    this.gui.MouseWheel -= MWheel;
                    this.gui.KeyPulsed -= KPulsed;
                    this.gui.KeyReleased -= KReleased;
                    this.gui.KeyCharReturned -= KCharReturned;
                }
                this.gui = value; 
                if (this.gui!= null)
                {
                    this.gui.MouseDown += MDown;
                    this.gui.MouseUp += MUp;
                    this.gui.MouseMove += MMove;
                    this.gui.MouseWheel += MWheel;
                    this.gui.KeyPulsed += KPulsed;
                    this.gui.KeyReleased += KReleased;
                    this.gui.KeyCharReturned += KCharReturned;
                }
            }
            get { return this.gui; }
        }

        public dgtk.Graphics.Color4 BackgroundColor
        {
            get; set;
        }

        public dgtk.Graphics.Color4 TextColor
        {
            get; set;
        }


        public bool Visible
        {
            set 
            { 
                this.b_visible = value;
                if (this.b_visible)
                {
                    if (this.parentGuiSurface != null)
                    {
                        if (!this.parentGuiSurface.VisibleSurfaceOrder.Contains(this.ui_id))
                        {
                            this.parentGuiSurface.VisibleSurfaceOrder.Add(this.ui_id); // Añadir a la lista de Controles a Mostrar de la Ventana Padre.
                        }
                    }
                    if (this.parentGuiSurface != null)
                    {
                        if (!this.parentGuiSurface.VisibleSurfaceOrder.Contains(this.ui_id))
                        {
                            this.parentGuiSurface.VisibleSurfaceOrder.Add(this.ui_id); // Añadir a la lista de Controles a Mostrar del guiSurface Padre.
                        }
                    }
                }
                else
                {
                    if (this.parentGuiSurface != null)
                    {
                        if (this.parentGuiSurface.VisibleSurfaceOrder.Contains(this.ui_id))
                        {
                            this.parentGuiSurface.VisibleSurfaceOrder.Remove(this.ui_id); // Sacar de la lista de Controles a dibujar la Ventana Padre.
                        }
                    }
                    if (this.parentGuiSurface != null)
                    {
                        if (this.parentGuiSurface.VisibleSurfaceOrder.Contains(this.ui_id))
                        {
                            this.parentGuiSurface.VisibleSurfaceOrder.Remove(this.ui_id); // Sacar de la lista de Controles a dibujar por el guiSurface Padre.
                        }
                    }
                }
            }
            get { return this.b_visible; }
        }

        #endregion
    }
}