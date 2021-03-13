using System;
using System.Collections.Generic;

using dge.GUI;
using dge.G2D;
using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;

namespace dge.GUI.BaseObjects
{
    public class BaseGuiSurface
    {
        protected uint ui_id; // ID De objeto 2D.
        protected Color4 idColor; // Color para la selección de ID.

        internal int int_x; // Posiciones X e Y heredados.
        internal int int_y; // Posiciones X e Y heredados.
        protected int i_x; // Coordenada X de posición de Objeto
        protected int i_y; // Coordenada Y de posición de Objeto)
        protected uint ui_width; // Ancho del Objeto en Pixeles
        protected uint ui_height; // Alto dle objeto en pixeles.

        #region CONTENIDO:
        internal bool contentUpdate; // Indicador de su se debe o no actualizar el conteido del objeto.

        #endregion

        //protected TextureBufferObject textureBufferObject; // Textura base del Objeto. En Versión posterior se trasladará al tema del GUI.

        protected float[] Texcoords; // Coordenadas de textura base para el objeto.
        protected float[] tcFrameOffset; // Variación de las coordenadas para eventos visuales como Pulsaciones de un botón.
        protected int[] MarginsFromTheEdge; // Margenes desde el borde al relleno del objeto.

        private bool b_visible; // ¿Es Visible la ventana?
        protected GraphicsUserInterface gui; // GUI al que pertenece.
        private Dictionary<uint, BaseGuiSurface> d_guiSurfaces; // Todos los Controles de la ventana.
        internal List<uint> VisibleSurfaceOrder; // Orden de los Controles de la ventana.
        private BaseGuiSurface parentGuiSurface; // Padre cuando es contenido.
        
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

        public BaseGuiSurface(uint width, uint height) //: base(witdh, height)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            this.ui_id = Core2D.GetID(); // Obtenemos ID de la superficie.
            byte[] colorvalues = Core2D.DeUIntAByte4(this.ui_id); // Obtenemos color a partir del ID.
            this.idColor = new Color4((byte)colorvalues[0], (byte)colorvalues[1], (byte)colorvalues[2], (byte)colorvalues[3]); // Establecemos color de ID.

            this.ui_width = width; // Establecemos ancho del objeto.
            this.ui_height = height; // Establecemos Alto del objeto.

            this.contentUpdate = false; // Por defecto el contenido no actualiza.

            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();

            this.b_visible = true; // El guiSurface es visible por defecto.
            this.d_guiSurfaces = new Dictionary<uint, BaseGuiSurface>(); // Lista de objetos Hijo.
            this.VisibleSurfaceOrder = new List<uint>(); // Lista de objetos hijo visibles.

            /*   // Pasamos a obtenerlo del Tema Visual del GUI
            this.MarginsFromTheEdge = new Vector4(2, 2, 2, 2); // Margenes entre el borde y los vertices internos.
            this.tcFrameOffset = new Vector2(0,0);
            this.Texcoords = new float[]
            {
                0f, 0.33f, 0.66f, 1f, 
                0f, 0.33f, 0.66f, 1f
            };
            */
            this.MouseDown += delegate {}; //Inicialización del evento por defecto.
            this.MouseUp += delegate {}; //Inicialización del evento por defecto.
            this.MouseMove += delegate {}; //Inicialización del evento por defecto.
            this.MouseWheel += delegate {}; //Inicialización del evento por defecto.
            this.KeyPulsed += delegate {}; //Inicialización del evento por defecto.
            this.KeyReleased += delegate {}; //Inicialización del evento por defecto.
            this.KeyCharReturned += delegate {}; //Inicialización del evento por defecto.
        }

        ~BaseGuiSurface()
        {
            Core2D.ReleaseID(this.ui_id); // Liberamos ID de la superficie.
        }

        #region PRIVATES:

        #endregion

        #region Protected

        protected void AddSurface(BaseGuiSurface surface)
        {
            if (!this.d_guiSurfaces.ContainsValue(surface))
            {
                surface.parentGuiSurface = this; // Adoptar guiSurface
                surface.GUI = this.gui; // Asignar mimo GUI.
                surface.int_x = this.int_x+this.i_x;
                surface.int_y = this.int_y+this.i_y;
                this.d_guiSurfaces.Add(surface.ID, surface); // Añadir guiSurface.
                if (surface.Visible)
                {
                    this.VisibleSurfaceOrder.Add(surface.ID); // Si es Visible que se muestre.
                    this.contentUpdate = true;
                }
            }
        }

        protected void RemoveSurface(uint id)
        {
            if (this.d_guiSurfaces.ContainsKey(id))
            {
                this.d_guiSurfaces[id].parentGuiSurface = null; // Repudiar guiSurface
                this.d_guiSurfaces[id].GUI = null; // Desasignar GUI
                this.d_guiSurfaces.Remove(id); // Eliminar guiSurface
                if (this.VisibleSurfaceOrder.Contains(id))
                {
                    this.VisibleSurfaceOrder.Remove(id); // Sacar de la lista de controles Visibles.
                    this.contentUpdate = true;
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

        internal virtual void DrawContent()
        {
            for (int i=0;i<VisibleSurfaceOrder.Count;i++)
            {
                this.d_guiSurfaces[VisibleSurfaceOrder[i]].Draw();
            }
        }

        protected virtual void DrawContentIDs()
        {
            for (int i=0;i<VisibleSurfaceOrder.Count;i++)
            {
                this.d_guiSurfaces[VisibleSurfaceOrder[i]].DrawID();
            }
        }

        protected void DrawIn(int x, int y, int width, int height, Action action)
        {
            if (this.gui != null)
            {
                int[] iv_VP = GL.glGetViewport();
                GL.glViewport(iv_VP[0]+x, iv_VP[1]+iv_VP[3]/*(int)this.gui.ui_height*/-(int)(y+height), width, height);

                dgtk.Math.Mat4 m4 = this.gui.Drawer.m4P;
                dgtk.Math.Mat4 m42 = this.gui.GuiDrawer.m4P;

                this.gui.Drawer.DefinePerspectiveMatrix(0, 0, width, height, true);
                this.gui.GuiDrawer.DefinePerspectiveMatrix(0, 0, width, height);

                action();

                GL.glViewport(iv_VP[0], iv_VP[1], iv_VP[2], iv_VP[3]);
                this.gui.Drawer.DefinePerspectiveMatrix(m4);
                this.gui.GuiDrawer.DefinePerspectiveMatrix(m42);
            }
        }

        protected void DrawIdIn(int x, int y, int width, int height, Action action)
        {
            if (this.gui != null)
            {
                int[] iv_VP = GL.glGetViewport();
                GL.glViewport(iv_VP[0]+x, iv_VP[1]+iv_VP[3]/*(int)this.gui.ui_height*/-(int)(y+height), width, height);

                dgtk.Math.Mat4 m4 = dge.G2D.IDsDrawer.m4P;

                dge.G2D.IDsDrawer.DefinePerspectiveMatrix(0,0, width, height, true);

                action();

                GL.glViewport(iv_VP[0], iv_VP[1], iv_VP[2], iv_VP[3]);
                dge.G2D.IDsDrawer.DefinePerspectiveMatrix(m4);   
            }
        }

        #endregion

        internal virtual void Draw()
        {
            
            if (this.gui != null)
            {
                this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);

                if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
                {
                    DrawIn(this.i_x-(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawContent);
                }
            }
            
        }

        internal virtual void DrawID()
        {
            
            dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 1); // Pintamos ID de la superficie.
           
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                this.DrawIdIn(this.i_x-(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawContentIDs);
            }
            
        }

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

        public uint ID
        {
            get { return this.ui_id; }
        }

        public virtual int X
        {
            set 
            { 
                this.i_x = value;
                foreach(BaseGuiSurface srf in this.d_guiSurfaces.Values)
                {
                    srf.int_x = this.int_x+this.i_x;
                }
            }
            get { return this.i_x; }
        }

        public virtual int Y
        {
            set 
            { 
                this.i_y = value;
                foreach(BaseGuiSurface srf in this.d_guiSurfaces.Values)
                {
                    srf.int_y = this.int_y+this.i_y;
                }
            }
            get { return this.i_y; }
        }

        public virtual uint Width
        {
            set { this.ui_width = value; }
            get { return this.ui_width; }
        }

        public virtual uint Height
        {
            set { this.ui_height = value; }
            get { return this.ui_height; }
        }

        internal BaseGuiSurface ParentGuiSurface
        {
            set 
            { 
                this.parentGuiSurface = value;
                this.GUI = this.parentGuiSurface.gui;
            }
            get { return this.parentGuiSurface; }
        }

        internal virtual GraphicsUserInterface GUI
        {
            set 
            { 
                if (this.gui!= null) // Si ya tenemos un GUI eliminamos los eventos del mismo.
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
                if (this.gui!= null) // Si existe el nuevo GUI, registramos los eventos.
                {
                    this.gui.MouseDown += MDown;
                    this.gui.MouseUp += MUp;
                    this.gui.MouseMove += MMove;
                    this.gui.MouseWheel += MWheel;
                    this.gui.KeyPulsed += KPulsed;
                    this.gui.KeyReleased += KReleased;
                    this.gui.KeyCharReturned += KCharReturned;
                }
                foreach (BaseGuiSurface bgs in d_guiSurfaces.Values)
                {
                    bgs.GUI = this.gui; // Asignar mismo GUI a los hijos.
                }
            }
            get { return this.gui; }
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