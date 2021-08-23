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
        internal G2D.GuiDrawer gd_GuiDrawer;
        internal G2D.Writer Writer;
        internal G2D.Drawer Drawer;
        internal bool Update;
        //internal static dge.G2D.TextureBufferObject DefaultThemeTBO;
        //internal static dge.G2D.TextureBufferObject DefaultThemeSltTBO;
        private readonly Dictionary<uint, Window> d_Windows; // Todas las Ventanas del Interface.
        internal List<uint> VisibleWindowsOrder; // Orden de las Ventanas Visibles.
        private readonly Dictionary<uint, BaseObjects.Control> d_Controls; // Todos los Controles fuera de ventanas.
        internal List<uint> VisibleControlsOrder; // Orden de los Controles fuera de ventanas.
        internal Dictionary<string,Menu> m_menu;
        internal List<string> l_menus;

        private uint ui_width; // Para calculos internos de ViewPorts De elementos.
        private uint ui_height; // Para calculos internos de ViewPorts De elementos.

        private int mheight; // Alto MainMenu.

		internal event EventHandler<MouseButtonEventArgs> MouseDown; // Evento que se da cuando se pulsa un botón del ratón.
		internal event EventHandler<MouseButtonEventArgs> MouseUp; // Evento que se da cuando se suelta un botón del ratón.
		internal event EventHandler<MouseMoveEventArgs> MouseMove; // Evento que se da cuando el ratón se mueve.
		internal event EventHandler<MouseWheelEventArgs> MouseWheel; // Evento que se da cuando se acciona la rueda del ratón.		
        internal event EventHandler<KeyBoardKeysEventArgs> KeyPulsed; // Evento que se da cuando se pulsa una tecla del teclado.
		internal event EventHandler<KeyBoardKeysEventArgs> KeyReleased; // Evento que se da cuando se suelta una tecla del teclado.
		internal event EventHandler<KeyBoardTextEventArgs> KeyCharReturned; // Evento devuelto cuando se pulsa o se suelta una tecla y que devuelve el caracter asociado.
		internal event EventHandler<ResizeEventArgs> Resized; // Evento devuelto cuando se reescala la Ventana.
		
        public GraphicsUserInterface()
        {
            if (gt_ActualGuiTheme == null)
            {
                gt_ActualGuiTheme = GuiTheme.DefaultGuiTheme;
                //DefaultThemeTBO = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultTheme.png"), "GuiDefaultTheme");
                //DefaultThemeSltTBO = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultThemeSlt.png"), "GuiDefaultThemeSlt");
                this.UpdateTheme();
            }
            this.m_menu = new Dictionary<string, Menu>();
            this.l_menus = new List<string>();
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
            this.Resized += delegate {}; //Inicialización del evento por defecto.
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
                this.parentWindow.WindowSizeChange -= WResized;
            }
        }

        private void UpdateTheme()
        {
            mheight = (int)((this.gt_ActualGuiTheme.DefaultFont.MaxCharacterHeight*(12/this.gt_ActualGuiTheme.DefaultFont.MaxFontSize))+(this.gt_ActualGuiTheme.Menu_MarginsFromTheEdge[1]+this.gt_ActualGuiTheme.Menu_MarginsFromTheEdge[3]));
        }

        #region PUBLIC:
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

        public Window GetWindow(uint index)
        {
            if (this.d_Windows.ContainsKey(index))
            {
                return this.d_Windows[index];
            }
            return null;
        }

        public void AddMenu(string MenuName)
        {
            if (!this.m_menu.ContainsKey(MenuName))
            {
                this.m_menu.Add(MenuName, new Menu(MenuName));
                this.m_menu[MenuName].GUI = this;
                this.l_menus.Add(MenuName);
                this.ReorderMenus();
            }
        }

        public void AddMenu(Menu menu)
        {
            if (!this.m_menu.ContainsValue(menu))
            {
                this.m_menu.Add(menu.Name, menu);
                this.m_menu[menu.Name].GUI = this;
                this.l_menus.Add(menu.Name);
                this.ReorderMenus();
            }
        }

        public void RemoveMenu(string MenuName)
        {
            if (this.m_menu.ContainsKey(MenuName))
            {
                this.m_menu[MenuName].GUI = null;
                this.m_menu.Remove(MenuName);
                this.l_menus.Remove(MenuName);
                this.ReorderMenus();
            }
        }

        private void ReorderMenus()
        {
            int posx = 0;
            for (int i=0;i<this.l_menus.Count;i++)
            {
                this.m_menu[this.l_menus[i]].X = posx;
                this.m_menu[this.l_menus[i]].RepositionMenus();
                posx+=(int)this.m_menu[this.l_menus[i]].Width;
            }
        }

        public Menu GetMenu(string MenuName)
        {
            if (this.m_menu.ContainsKey(MenuName))
            {
                return this.m_menu[MenuName];
            }
            return null;
        }

        #endregion
        
        internal void Draw()
        {
            GL.glViewport(0, 0, this.parentWindow.Width, this.parentWindow.Height);
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
            //GL.glViewport(0, 0, this.parentWindow.Width, this.parentWindow.Height);
            if (this.m_menu.Count > 0)
            {
                this.DrawMenu();
            }
        }

        internal void DrawIds()
        {
            dge.Core2D.UpdateIdsMap((uint)this.ui_width, (uint)this.ui_height, this.DrawContentIds); 
        }

        private void DrawMenu()
        {
            //mheight = (int)(this.gt_ActualGuiTheme.DefaultFont.MaxCharacterHeight*(12/this.gt_ActualGuiTheme.DefaultFont.MaxFontSize));
            this.gd_GuiDrawer.DrawGL(Color4.Gray, 0, 0, (uint)this.ParentWindow.Width, (uint)(mheight), 0);
            foreach (string key in this.m_menu.Keys)
            {
                this.m_menu[key].Draw();
            }
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
            if (this.m_menu.Count > 0) 
            { 
                foreach (string key in this.m_menu.Keys)
                {
                    this.m_menu[key].DrawID(); 
                }
            }
        }

        #region Events:

        internal void MDown(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            this.DrawIds();
            dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseDown(this, new MouseButtonEventArgs(e.X, e.Y, (MouseButtons)e.Buttons, (PushRelease)e.Action));
        }

        internal void MUp(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            //this.DrawIds(); // Comentanos de momento
            dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseUp(this, new MouseButtonEventArgs(e.X, e.Y, (MouseButtons)e.Buttons, (PushRelease)e.Action));
        }

        internal void MMove(object sender, dgtk.dgtk_MouseMoveEventArgs e)
        {
            //this.DrawIds();
            //dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseMove(this, new MouseMoveEventArgs(e.X, e.Y, e.X_inScreen, e.Y_inScreen));
        }

        internal void MWheel(object sender, dgtk.dgtk_MouseWheelEventArgs e)
        {
            this.DrawIds();
            dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseWheel(this, new MouseWheelEventArgs(e.X, e.Y, e.Delta));
        }

        internal void KPulsed(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            this.KeyPulsed(this, new KeyBoardKeysEventArgs(new KeyBoard_Status((KeyCode)e.KeyStatus.KeyCode, (PushRelease)e.KeyStatus.KeyStatus)));
        }

        internal void KReleased(object sender, dgtk.dgtk_KeyBoardKeysEventArgs e)
        {
            this.KeyReleased(this, new KeyBoardKeysEventArgs(new KeyBoard_Status((KeyCode)e.KeyStatus.KeyCode, (PushRelease)e.KeyStatus.KeyStatus)));
        }

        internal void KCharReturned(object sender, dgtk.dgtk_KeyBoardTextEventArgs e)
        {
            this.KeyCharReturned(this, new KeyBoardTextEventArgs(e.Character));
        }

        internal void WResized(object sender, dgtk.dgtk_ResizeEventArgs e)
        {
            this.Resized(this, new ResizeEventArgs(e.Width, e.Height));
            this.OnResized(this, e);
        }

        protected virtual void OnResized(object sender, dgtk.dgtk_ResizeEventArgs e)
        {

        }

        #endregion

        internal dgWindow iParentWindow
        {
            get { return this.parentWindow; }
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
                    this.parentWindow.WindowSizeChange -= WResized;
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
                    this.parentWindow.WindowSizeChange += WResized;
                }
                this.ui_width = (uint)parentWindow.Width;
                this.ui_height = (uint)parentWindow.Height;
            }
        }

        public GuiTheme GuiTheme
        {
            set { this.gt_ActualGuiTheme = value; this.UpdateTheme(); }
            get { return this.gt_ActualGuiTheme; }
        }
        
        internal uint ui_Width // Para calculos internos de ViewPorts De elementos.
        {
            set
            {
                this.ui_width = value;
                //Meter definir perspectiva;
                this.gd_GuiDrawer.DefinePerspectiveMatrix(0,0,this.ui_width, this.ui_height);
                this.Writer.DefinePerspectiveMatrix(0,0,this.ui_width, this.ui_height, true);
            }
            get { return this.ui_width;}
        }
        
        internal uint ui_Height // Para calculos internos de ViewPorts De elementos.
        {
            set
            {
                this.ui_height = value;
                //Meter definir perspectiva;
                this.gd_GuiDrawer.DefinePerspectiveMatrix(0,0,this.ui_width, this.ui_height);
                this.Writer.DefinePerspectiveMatrix(0,0,this.ui_width, this.ui_height, true);
            }
            get { return this.ui_height;}
        }
        
        public dgWindow ParentWindow
        {
            get { return this.parentWindow; }
        }

        public G2D.GuiDrawer GuiDrawer
        {
            get { return this.gd_GuiDrawer; }
        }
    }
}