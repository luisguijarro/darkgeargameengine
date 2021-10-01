using System;
using System.Reflection;
using System.Collections.Generic;
using dgtk.Graphics;
using dgtk.OpenGL;

namespace dge.GUI
{
    public class GraphicsUserInterface : IDisposable
    {
        internal Dialog ActiveDialog;
        internal GuiTheme gt_ActualGuiTheme;
        private dgWindow parentWindow;
        internal G2D.GuiDrawer gd_GuiDrawer;
        internal G2D.Writer Writer;
        internal G2D.Drawer Drawer;
        internal bool Update;
        private readonly Dictionary<uint, Window> d_Windows; // Todas las Ventanas del Interface.
        internal List<uint> VisibleWindowsOrder; // Orden de las Ventanas Visibles.
        private readonly Dictionary<uint, BaseObjects.Control> d_Controls; // Todos los Controles fuera de ventanas.
        internal List<uint> VisibleControlsOrder; // Orden de los Controles fuera de ventanas.
        internal Dictionary<string,Menu> m_menu;
        internal List<string> l_menus;

        internal Dictionary<string, Dialog> d_Dialogs;

        protected int i_width; // Para calculos internos de ViewPorts De elementos.
        protected int i_height; // Para calculos internos de ViewPorts De elementos.

        private int mheight; // Alto MainMenu.

        private int i_LastPosX;
        private int i_LastPosY;

        private Color4 c4_MenuBackgroundColor;

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
            this.m_menu = new Dictionary<string, Menu>();
            this.l_menus = new List<string>();
            this.Update = true; //Forzamos para pruebas.
            this.d_Windows = new Dictionary<uint, Window>();
            this.VisibleWindowsOrder = new List<uint>();
            this.d_Controls = new Dictionary<uint, BaseObjects.Control>();
            this.VisibleControlsOrder = new List<uint>();
            this.d_Dialogs = new Dictionary<string, Dialog>();

            if (gt_ActualGuiTheme == null)
            {
                gt_ActualGuiTheme = GuiTheme.DefaultGuiTheme;
                this.UpdateTheme();
            }

            this.MouseDown += delegate {}; //Inicialización del evento por defecto.
            this.MouseUp += delegate {}; //Inicialización del evento por defecto.
            this.MouseMove += delegate {}; //Inicialización del evento por defecto.
            this.MouseWheel += delegate {}; //Inicialización del evento por defecto.
            this.KeyPulsed += delegate {}; //Inicialización del evento por defecto.
            this.KeyReleased += delegate {}; //Inicialización del evento por defecto.
            this.KeyCharReturned += delegate {}; //Inicialización del evento por defecto.
            this.Resized += delegate {}; //Inicialización del evento por defecto.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
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
        }

        protected virtual void UpdateTheme()
        {
            mheight = (int)((this.gt_ActualGuiTheme.Default_Font.MaxCharacterHeight*(12/this.gt_ActualGuiTheme.Default_Font.MaxFontSize))+(this.gt_ActualGuiTheme.Menu_MarginsFromTheEdge[1]+this.gt_ActualGuiTheme.Menu_MarginsFromTheEdge[3]));
            if (this.c4_MenuBackgroundColor == GuiTheme.DefaultGuiTheme.Default_MenuBackgroundColor)
            {
                this.c4_MenuBackgroundColor = this.GuiTheme.Default_MenuBackgroundColor;
            }

            foreach(Window win in this.d_Windows.Values)
            {
                win.UpdateTheme();
            }
            foreach(BaseObjects.Control ctrl in this.d_Controls.Values)
            {
                ctrl.UpdateTheme();
            }
            foreach(MenuItem mnt in this.m_menu.Values)
            {
                mnt.UpdateTheme();
            }
        }

        #region PUBLIC:
        public void AddWindow(Window window)
        {
            if (!this.d_Windows.ContainsKey(window.ID))
            {
                window.GraphicsUserInterface = this; // Adoptar Ventana.
                window.intY = (this.l_menus.Count>0) ? this.mheight : 0;
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
                control.intY = (this.l_menus.Count>0) ? this.mheight : 0;
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
                foreach(BaseObjects.Control ctrl in this.d_Controls.Values)
                {
                    ctrl.intY = (this.l_menus.Count>0) ? this.mheight : 0;;
                }
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
                foreach(BaseObjects.Control ctrl in this.d_Controls.Values)
                {
                    ctrl.intY = (this.l_menus.Count>0) ? this.mheight : 0;
                }
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
            GL.glViewport(0, 0, (int)this.i_width, (this.m_menu.Count>0) ? (int)(this.i_height-mheight) : (int)this.i_height);
            this.UpdatePerspective(0, (this.m_menu.Count>0) ? (int)this.mheight : 0, this.i_width, (this.m_menu.Count>0) ? this.i_height-this.mheight : this.i_height);
            
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
            
            if (this.m_menu.Count > 0)
            {
                GL.glViewport(0, 0, this.parentWindow.Width, this.parentWindow.Height);
                this.UpdatePerspective(0, 0, this.parentWindow.Width, this.parentWindow.Height); //this.i_width, this.i_height);
                this.DrawMenu();
            }

            if (this.ActiveDialog!=null) { this.ActiveDialog.Draw(); }
        }

        internal void DrawIds()
        {
            dge.Core2D.UpdateIdsMap(this.i_width, this.i_height, this.DrawContentIds); 
        }

        private void DrawMenu()
        {
            this.gd_GuiDrawer.DrawGL(this.c4_MenuBackgroundColor, 0, 0, this.ParentWindow.Width, this.mheight, 0);
            foreach (string key in this.m_menu.Keys)
            {
                this.m_menu[key].Draw();
            }
        }

        private void DrawMenuID()
        {
            this.gd_GuiDrawer.DrawGL(Color4.Black, 0, 0, this.ParentWindow.Width, this.mheight, 0);
            foreach (string key in this.m_menu.Keys)
            {
                this.m_menu[key].DrawID();
            }
        }

        private void DrawContentIds()
        {     
            GL.glViewport(0, 0, (int)this.i_width, (this.m_menu.Count>0) ? (int)(this.i_height-this.mheight) : (int)this.i_height);
            
            //dge.G2D.IDsDrawer.DefinePerspectiveMatrix(0, (this.m_menu.Count>0) ? (int)this.m_menu[this.l_menus[0]].Height : 0, this.i_width, (this.m_menu.Count>0) ? (this.i_height-this.m_menu[this.l_menus[0]].Height) : this.i_height, true);
            dge.G2D.IDsDrawer.DefinePerspectiveMatrix(0, 0, this.i_width, this.Height, true);
            
            if (this.ActiveDialog == null)
            {
                if (this.Update)
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
            }
            if (this.m_menu.Count > 0)
            {
                GL.glViewport(0, 0, this.parentWindow.Width, this.parentWindow.Height);
                dge.G2D.IDsDrawer.DefinePerspectiveMatrix(0, 0, this.parentWindow.Width, this.parentWindow.Height, true); //this.i_width, this.i_height);
                this.DrawMenuID();
            } 

            if (this.ActiveDialog!=null) { this.ActiveDialog.DrawID(); }
        }

        private void UpdatePerspective(int x, int y, int uwidth, int uheight)
        {
            this.gd_GuiDrawer.DefinePerspectiveMatrix(0,0,(float)uwidth, (float)uheight);
            this.Writer.DefinePerspectiveMatrix(0,0,(float)uwidth, (float)uheight, true);
        }
        

        #region Events:

        internal void MDown(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            this.DrawIds();
            uint idselected = dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseDown(this, new MouseButtonEventArgs(e.X, (this.m_menu.Count<=0) ? e.Y : e.Y-mheight, (MouseButtons)e.Buttons, (PushRelease)e.Action, idselected));
        }

        internal void MUp(object sender, dgtk.dgtk_MouseButtonEventArgs e)
        {
            //this.DrawIds(); // Comentanos de momento
            uint idselected = dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseUp(this, new MouseButtonEventArgs(e.X, (this.m_menu.Count<=0) ? e.Y : e.Y-mheight, (MouseButtons)e.Buttons, (PushRelease)e.Action, idselected));
        }

        internal void MMove(object sender, dgtk.dgtk_MouseMoveEventArgs e)
        {
            uint idselected = 0;
            //this.DrawIds();
            if ((e.X_inScreen>this.parentWindow.X) && (e.X_inScreen<(this.parentWindow.X+this.parentWindow.Width)))
            {
                if ((e.Y_inScreen>this.parentWindow.Y) && (e.Y_inScreen<(this.parentWindow.Y+this.parentWindow.Height)))
                {
                    this.DrawIds();
                    idselected = dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
                }
            }
            this.MouseMove(this, new MouseMoveEventArgs(e.X, (this.m_menu.Count<=0) ? e.Y : e.Y-mheight, this.i_LastPosX, this.i_LastPosY, e.X_inScreen, e.Y_inScreen, idselected));
            this.i_LastPosX = e.X;
            this.i_LastPosY = (this.m_menu.Count<=0) ? e.Y : e.Y-mheight;
        }

        internal void MWheel(object sender, dgtk.dgtk_MouseWheelEventArgs e)
        {
            this.DrawIds();
            uint idselected = dge.Core2D.SelectID(e.X, e.Y, (int)this.parentWindow.Width, (int)this.parentWindow.Height);
            this.MouseWheel(this, new MouseWheelEventArgs(e.X, (this.m_menu.Count<=0) ? e.Y : e.Y-mheight, e.Delta, idselected));
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
            this.OnResized(this, new ResizeEventArgs(e.Width, e.Height));
        }

        protected virtual void OnResized(object sender, ResizeEventArgs e)
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
            }
        }

        #region PROPERTIES:

        public GuiTheme GuiTheme
        {
            set { this.gt_ActualGuiTheme = value; this.UpdateTheme(); }
            get { return this.gt_ActualGuiTheme; }
        }

        public int Width // Para calculos internos de ViewPorts De elementos.
        {
            set
            {
                this.i_width = value;
                //Meter definir perspectiva;
                this.gd_GuiDrawer.DefinePerspectiveMatrix(0,0,this.i_width, this.i_height);
                this.Writer.DefinePerspectiveMatrix(0,0,this.i_width, this.i_height, true);
                this.OnResized(this, new ResizeEventArgs((int)this.i_width, (int)this.i_height));
            }
            get { return this.i_width;}
        }
        
        public int Height // Para calculos internos de ViewPorts De elementos.
        {
            set
            {
                this.i_height = value;
                //Meter definir perspectiva;
                this.gd_GuiDrawer.DefinePerspectiveMatrix(0,0,this.i_width, this.i_height);
                this.Writer.DefinePerspectiveMatrix(0,0,this.i_width, this.i_height, true);
                this.OnResized(this, new ResizeEventArgs((int)this.i_width, (int)this.i_height));
            }
            get 
            { 
                return ((this.m_menu.Count>0) ? this.i_height-this.m_menu[this.l_menus[0]].Height : this.i_height);
            }
        }
        
        public dgWindow ParentWindow
        {
            get { return this.parentWindow; }
        }

        public G2D.GuiDrawer GuiDrawer
        {
            get { return this.gd_GuiDrawer; }
        }

        public Color4 MenuBackgroundColor
        {
            set { this.c4_MenuBackgroundColor = value; }
            get { return this.c4_MenuBackgroundColor; }
        }
    
        #endregion
    }
}