using System;
using System.Reflection;
using System.Collections.Generic;
using dgtk.Graphics;
using dgtk.OpenGL;

namespace dge.GUI
{
    public class GraphicsUserInterface
    {
        internal GuiTheme gt_ActualGuiTheme;
        private dgWindow parentWindow;
        internal G2D.GuiDrawer GuiDrawer;
        internal G2D.Writer Writer;
        internal G2D.Drawer Drawer;
        internal bool Update;
        //internal static dge.G2D.TextureBufferObject DefaultThemeTBO;
        //internal static dge.G2D.TextureBufferObject DefaultThemeSltTBO;
        private Dictionary<uint, Window> d_Windows; // Todas las Ventanas del Interface.
        internal List<uint> VisibleWindowsOrder; // Orden de las Ventanas Visibles.
        private Dictionary<uint, BaseObjects.Control> d_Controls; // Todos los Controles fuera de ventanas.
        internal List<uint> VisibleControlsOrder; // Orden de los Controles fuera de ventanas.

        internal uint ui_width; // Para calculos internos de ViewPorts De elementos.
        internal uint ui_height; // Para calculos internos de ViewPorts De elementos.

		internal event EventHandler<dgtk.dgtk_MouseButtonEventArgs> MouseDown; // Evento que se da cuando se pulsa un botón del ratón.
		internal event EventHandler<dgtk.dgtk_MouseButtonEventArgs> MouseUp; // Evento que se da cuando se suelta un botón del ratón.
		internal event EventHandler<dgtk.dgtk_MouseMoveEventArgs> MouseMove; // Evento que se da cuando el ratón se mueve.
		internal event EventHandler<dgtk.dgtk_MouseWheelEventArgs> MouseWheel; // Evento que se da cuando se acciona la rueda del ratón.		
        internal event EventHandler<dgtk.dgtk_KeyBoardKeysEventArgs> KeyPulsed; // Evento que se da cuando se pulsa una tecla del teclado.
		internal event EventHandler<dgtk.dgtk_KeyBoardKeysEventArgs> KeyReleased; // Evento que se da cuando se suelta una tecla del teclado.
		internal event EventHandler<dgtk.dgtk_KeyBoardTextEventArgs> KeyCharReturned; // Evento devuelto cuando se pulsa o se suelta una tecla y que devuelve el caracter asociado.
		
        public GraphicsUserInterface()
        {
            if (gt_ActualGuiTheme == null)
            {
                gt_ActualGuiTheme = GuiTheme.DefaultGuiTheme;
                //DefaultThemeTBO = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultTheme.png"), "GuiDefaultTheme");
                //DefaultThemeSltTBO = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultThemeSlt.png"), "GuiDefaultThemeSlt");
            }

            this.Update = true; //Forzamos para pruebas.
            this.d_Windows = new Dictionary<uint, Window>();
            this.VisibleWindowsOrder = new List<uint>();
            this.d_Controls = new Dictionary<uint, BaseObjects.Control>();
            this.VisibleControlsOrder = new List<uint>();
            this.MouseDown += delegate {}; //Inicialización del evento por defecto.
            this.MouseUp += delegate {}; //Inicialización del evento por defecto.
            this.MouseMove += delegate {}; //Inicialización del evento por defecto.
            this.MouseWheel += delegate {}; //Inicialización del evento por defecto.
            this.KeyPulsed += delegate {}; //Inicialización del evento por defecto.
            this.KeyReleased += delegate {}; //Inicialización del evento por defecto.
            this.KeyCharReturned += delegate {}; //Inicialización del evento por defecto.
        }
        ~GraphicsUserInterface()
        {
            if (this.parentWindow != null)
            {
                this.parentWindow.MouseDown -= MDown;
                this.parentWindow.MouseUp -= MUp;
                this.parentWindow.MouseMove -= MMove;
                this.parentWindow.MouseWheel -= MWheel;
                this.parentWindow.KeyPulsed -= KPulsed;
                this.parentWindow.KeyReleased -= KReleased;
                this.parentWindow.KeyCharReturned -= KCharReturned;
            }
        }

        public void AddWindow(Window window)
        {
            if (!this.d_Windows.ContainsKey(window.ID))
            {
                window.GraphicsUserInterface = this; // Adoptar Ventana.
                this.d_Windows.Add(window.ID, window); // Añadir Ventana.
                if (window.Visible)
                {
                    this.VisibleWindowsOrder.Add(window.ID); // Si es Visible que se muestre.
                }
            }
        }

        public void RemoveWindow(uint id)
        {
            if (this.d_Windows.ContainsKey(id))
            {
                this.d_Windows[id].GraphicsUserInterface = null; // Repudiar Ventana.
                this.d_Windows.Remove(id);
                if (this.VisibleWindowsOrder.Contains(id))
                {
                    this.VisibleWindowsOrder.Remove(id); // Sacar de la lista de Ventanas Visibles.
                }
            }            
        }

        public void RemoveWindow(Window window)
        {
            this.RemoveWindow(window.ID);
        }

        public void AddControl(BaseObjects.Control control)
        {
            if (!this.d_Controls.ContainsKey(control.ID))
            {
                control.GUI = this; // Adoptar Control.
                this.d_Controls.Add(control.ID, control); // Añadir Control.
                if (control.Visible)
                {
                    this.VisibleControlsOrder.Add(control.ID); // Si es Visible que se muestre.
                }
            }
        }

        public void RemoveControl(uint id)
        {
            if (this.d_Controls.ContainsKey(id))
            {
                this.d_Controls[id].GUI = null; // Repudiar Control.
                this.d_Controls.Remove(id);
                if (this.VisibleWindowsOrder.Contains(id))
                {
                    this.VisibleWindowsOrder.Remove(id); // Sacar de la lista de Controles Visibles.
                }
            }      
        }

        public void RemoveControl(BaseObjects.Control control)
        {
            this.RemoveControl(control.ID); // Eliminar Control Hijo.
        }

        internal void Draw()
        {
            if (this.Update)
            {
                for (int i=0;i<VisibleWindowsOrder.Count;i++)
                {
                    this.d_Windows[VisibleWindowsOrder[i]].Draw(); // Pintamos Ventanas Visibles.
                }

                for (int i=0;i<VisibleControlsOrder.Count;i++)
                {
                    this.d_Controls[VisibleControlsOrder[i]].Draw(); // Pintamos Controles Visibles.
                }
            }
        }

        internal void DrawIds()
        {
            dge.Core2D.UpdateIdsMap((uint)this.ui_width, (uint)this.ui_height, this.DrawContentIds); 
        }

        private void DrawContentIds()
        {            
            for (int i=0;i<VisibleWindowsOrder.Count;i++)
            {
                this.d_Windows[VisibleWindowsOrder[i]].DrawID(); // Pintamos Ventanas Visibles.
            }

            for (int i=0;i<VisibleControlsOrder.Count;i++)
            {
                this.d_Controls[VisibleControlsOrder[i]].DrawID(); // Pintamos Controles Visibles.
            }
        }

        #region Events:

        internal void MDown(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            this.DrawIds();
            dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseDown(this, e);
        }

        internal void MUp(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            //this.DrawIds(); // Comentanos de momento
            dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseUp(this, e);
        }

        internal void MMove(object sender, dgtk.dgtk_MouseMoveEventArgs e)
        {
            this.MouseMove(this, e);
        }

        internal void MWheel(object sender, dgtk.dgtk_MouseWheelEventArgs e)
        {
            this.MouseWheel(this, e);
        }

        internal void KPulsed(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            this.KeyPulsed(this, e);
        }

        internal void KReleased(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            this.KeyReleased(this, e);
        }

        internal void KCharReturned(object sender, dgtk.dgtk_KeyBoardTextEventArgs e)
        {
            this.KeyCharReturned(this, e);
        }

        #endregion

        internal dgWindow ParentWindow
        {
            get { return this.ParentWindow; }
            set 
            {
                if (this.parentWindow != null)
                {
                    this.parentWindow.MouseDown -= MDown;
                    this.parentWindow.MouseUp -= MUp;
                    this.parentWindow.MouseMove -= MMove;
                    this.parentWindow.MouseWheel -= MWheel;
                    this.parentWindow.KeyPulsed -= KPulsed;
                    this.parentWindow.KeyReleased -= KReleased;
                    this.parentWindow.KeyCharReturned -= KCharReturned;
                }
                this.parentWindow = value; 
                if (this.parentWindow != null)
                {
                    this.parentWindow.MouseDown += MDown;
                    this.parentWindow.MouseUp += MUp;
                    this.parentWindow.MouseMove += MMove;
                    this.parentWindow.MouseWheel += MWheel;
                    this.parentWindow.KeyPulsed += KPulsed;
                    this.parentWindow.KeyReleased += KReleased;
                    this.parentWindow.KeyCharReturned += KCharReturned;
                }
                this.ui_width = (uint)parentWindow.Width;
                this.ui_height = (uint)parentWindow.Height;
            }
        }

        public GuiTheme GuiTheme
        {
            set { this.gt_ActualGuiTheme = value; }
            get { return this.gt_ActualGuiTheme; }
        }
    }
}