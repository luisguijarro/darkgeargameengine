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

        protected int int_x; // Posiciones X e Y heredados.
        protected int int_y; // Posiciones X e Y heredados.
        protected internal int i_x; // Coordenada X de posición de Objeto
        protected internal int i_y; // Coordenada Y de posición de Objeto)
        protected uint ui_width; // Ancho del Objeto en Pixeles
        protected uint ui_height; // Alto dle objeto en pixeles.

        #region CONTENIDO:
        protected internal bool contentUpdate; // Indicador de su se debe o no actualizar el conteido del objeto.

        #endregion

        protected bool b_ShowMe;

        //protected TextureBufferObject textureBufferObject; // Textura base del Objeto. En Versión posterior se trasladará al tema del GUI.

        protected internal float[] Texcoords; // Coordenadas de textura base para el objeto.
        protected internal float[] tcFrameOffset; // Variación de las coordenadas para eventos visuales como Pulsaciones de un botón.
        protected internal int[] MarginsFromTheEdge; // Margenes desde el borde al relleno del objeto. Left | Top | Right | Bottom

        protected int ida_X, ida_Y, ida_Width, ida_Height; //Coordenadas de dibujado de contenido.

        private bool b_visible; // ¿Es Visible la ventana?
        protected GraphicsUserInterface gui; // GUI al que pertenece.
        protected Dictionary<uint, BaseGuiSurface> d_guiSurfaces; // Todos los Controles de la ventana.
        protected internal List<uint> VisibleSurfaceOrder; // Orden de los Controles de la ventana.
        private BaseGuiSurface parentGuiSurface; // Padre cuando es contenido.
        
		public event EventHandler<MouseButtonEventArgs> MouseDown; // Evento que se da cuando se pulsa un botón del ratón.
		public event EventHandler<MouseButtonEventArgs> MouseUp; // Evento que se da cuando se suelta un botón del ratón.
		public event EventHandler<MouseMoveEventArgs> MouseMove; // Evento que se da cuando el ratón se mueve.
		public event EventHandler<MouseWheelEventArgs> MouseWheel; // Evento que se da cuando se acciona la rueda del ratón.		
        public event EventHandler<KeyBoardKeysEventArgs> KeyPulsed; // Evento que se da cuando se pulsa una tecla del teclado.
		public event EventHandler<KeyBoardKeysEventArgs> KeyReleased; // Evento que se da cuando se suelta una tecla del teclado.
		public event EventHandler<KeyBoardTextEventArgs> KeyCharReturned; // Evento devuelto cuando se pulsa o se suelta una tecla y que devuelve el caracter asociado.
        public event EventHandler<ResizeEventArgs> SizeChanged;
		
        public BaseGuiSurface() : this(160, 90)
        {            
            
        }

        public BaseGuiSurface(uint width, uint height) //: base(witdh, height)
        {
            this.MarginsFromTheEdge = new int[] {0,0,0,0};
            this.b_ShowMe = true;
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

            this.MouseDown += delegate {}; //Inicialización del evento por defecto.
            this.MouseUp += delegate {}; //Inicialización del evento por defecto.
            this.MouseMove += delegate {}; //Inicialización del evento por defecto.
            this.MouseWheel += delegate {}; //Inicialización del evento por defecto.
            this.KeyPulsed += delegate {}; //Inicialización del evento por defecto.
            this.KeyReleased += delegate {}; //Inicialización del evento por defecto.
            this.KeyCharReturned += delegate {}; //Inicialización del evento por defecto.
            this.SizeChanged += ResizeEvent;

            this.SetInternalDrawArea(this.i_x+(int)this.MarginLeft, this.i_y+(int)this.MarginTop, (int)this.ui_width-(int)(this.MarginLeft+this.MarginRight), (int)this.ui_height-(int)(this.MarginTop+this.MarginBottom));
        }

        ~BaseGuiSurface()
        {
            Core2D.ReleaseID(this.ui_id); // Liberamos ID de la superficie.
            this.SizeChanged += ResizeEvent;
        }

        #region PRIVATES:

        private void ResizeEvent(object sender, ResizeEventArgs e)
        {
            this.OnResize();
        }

        #endregion

        #region Protected

        protected void AddSurface(BaseGuiSurface surface)
        {
            if (!this.d_guiSurfaces.ContainsValue(surface))
            {
                surface.parentGuiSurface = this; // Adoptar guiSurface
                surface.GUI = this.gui; // Asignar mimo GUI.
                surface.intX = this.int_x+this.i_x;
                surface.intY = this.int_y+this.i_y;
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

        protected virtual void DrawContent()
        {
            //if (this.gui != null)
            //{
                for (int i=0;i<VisibleSurfaceOrder.Count;i++)
                {
                    this.d_guiSurfaces[VisibleSurfaceOrder[i]].Draw();
                }
            //}
        }

        protected virtual void DrawContentIDs()
        {            
            if (this.gui != null)
            {
                for (int i=0;i<VisibleSurfaceOrder.Count;i++)
                {
                    this.d_guiSurfaces[VisibleSurfaceOrder[i]].DrawID();
                }
            }
        }

        protected void DrawIn(int x, int y, int width, int height, Action action)
        {
            //if (this.gui != null)
            //{
                int[] iv_VP = GL.glGetViewport(); // Conservamos valores de ViewPort Actual.

                int mX = 0; // modificador de coordenadas de proyección.
                int ivp_x = iv_VP[0]+x;
                if (ivp_x < iv_VP[0])
                {
                    ivp_x = iv_VP[0];
                    mX = x;
                }
                int ivp_width = (iv_VP[0]+x+width > iv_VP[0]+iv_VP[2]) ? width-((iv_VP[0]+x+width)-(iv_VP[0]+iv_VP[2])) : width;
                ivp_width = ivp_width+mX;


                int mY = 0; // modificador de coordenadas de proyección.
                int ivp_y = iv_VP[1]+iv_VP[3]-(int)(y+height);
                int ivp_height = height;
                if (ivp_y < iv_VP[1])
                {
                    ivp_y = iv_VP[1];
                    ivp_height = height - (iv_VP[1] - (iv_VP[1]+iv_VP[3]-(int)(y+height)));
                }
                if ((ivp_y + ivp_height) > iv_VP[1]+iv_VP[3])
                {
                    ivp_height = ivp_height - ((ivp_y + ivp_height) - (iv_VP[1]+iv_VP[3]));
                    mY = y;
                }

                if ((ivp_width > 0) && (ivp_height > 0)) // Si el ancho y el alto del area a dibujar es mayor que 0
                {
                    GL.glViewport(ivp_x, ivp_y, ivp_width, ivp_height); // Establecemos nuevo ViewPort

                    dgtk.Math.Mat4 m4 = this.gui.Drawer.m4P;
                    dgtk.Math.Mat4 m4g = this.gui.gd_GuiDrawer.m4P;
                    dgtk.Math.Mat4 m4w = this.gui.Writer.m4P;

                    this.gui.Drawer.DefinePerspectiveMatrix(-mX, -mY, ivp_width-mX, ivp_height-mY, true);
                    this.gui.gd_GuiDrawer.DefinePerspectiveMatrix(-mX, -mY, ivp_width-mX, ivp_height-mY);
                    this.gui.Writer.DefinePerspectiveMatrix(-mX, -mY, ivp_width-mX, ivp_height-mY, true);

                    action();

                    GL.glViewport(iv_VP[0], iv_VP[1], iv_VP[2], iv_VP[3]); // Restauramos antiguo ViewPort.
                    this.gui.Drawer.DefinePerspectiveMatrix(m4);
                    this.gui.gd_GuiDrawer.DefinePerspectiveMatrix(m4g);
                    this.gui.Writer.DefinePerspectiveMatrix(m4w);
                }
            //}
        }

        protected void DrawIdIn(int x, int y, int width, int height, Action action)
        {
            if (this.gui != null)
            {
                
                int[] iv_VP = GL.glGetViewport(); // Conservamos valores de ViewPort Actual.

                int mX = 0; // modificador de coordenadas de proyección.
                int ivp_x = iv_VP[0]+x;
                if (ivp_x < iv_VP[0])
                {
                    ivp_x = iv_VP[0];
                    mX = x;
                }
                int ivp_width = (iv_VP[0]+x+width > iv_VP[0]+iv_VP[2]) ? width-((iv_VP[0]+x+width)-(iv_VP[0]+iv_VP[2])) : width;
                ivp_width = ivp_width+mX;


                int mY = 0; // modificador de coordenadas de proyección.
                int ivp_y = iv_VP[1]+iv_VP[3]-(int)(y+height);
                int ivp_height = height;
                if (ivp_y < iv_VP[1])
                {
                    ivp_y = iv_VP[1];
                    ivp_height = height - (iv_VP[1] - (iv_VP[1]+iv_VP[3]-(int)(y+height)));
                }
                if ((ivp_y + ivp_height) > iv_VP[1]+iv_VP[3])
                {
                    ivp_height = ivp_height - ((ivp_y + ivp_height) - (iv_VP[1]+iv_VP[3]));
                    mY = y;
                }

                if ((ivp_width > 0) && (ivp_height > 0)) // Si el ancho y el alto del area a dibujar es mayor que 0
                {
                    GL.glViewport(ivp_x, ivp_y, ivp_width, ivp_height);

                    dgtk.Math.Mat4 m4 = dge.G2D.IDsDrawer.m4P;

                    dge.G2D.IDsDrawer.DefinePerspectiveMatrix(-mX, -mY, ivp_width-mX, ivp_height-mY, true);

                    action();

                    GL.glViewport(iv_VP[0], iv_VP[1], iv_VP[2], iv_VP[3]);
                    dge.G2D.IDsDrawer.DefinePerspectiveMatrix(m4);   
                }
            }
        }

        #endregion

        public void SetInternalDrawArea(int x, int y, int width, int height)
        {
            this.ida_X = x;
            this.ida_Y = y;
            this.ida_Width = width;
            this.ida_Height = height;
        }

        public int[] GetInternalDrawArea()
        {
            return new int[] {this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height};
        }

        protected internal virtual void UpdateTheme()
        {

        }

        internal void Draw()
        {   
            if (this.gui != null)
            {
                if (this.b_visible)
                {
                    this.pDraw();
                    this.pDrawContent();
                    /*if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
                    {
                        DrawIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContent);
                    } */
                }
            }      
        }

        protected virtual void pDrawContent()
        {
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                DrawIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContent);
            } 
        }

        protected virtual void pDraw()
        {
            if (this.b_ShowMe)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }
        }

        internal virtual void DrawID()
        {
            if (this.gui != null)
            { 
                if (this.b_visible)
                {
                    this.pDrawID();
                    this.pDrawContentID();
                    /*if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
                    {
                        this.DrawIdIn(this.i_x+(int)this.MarginLeft, this.i_y+(int)this.MarginTop, (int)this.ui_width-(int)(this.MarginLeft+this.MarginRight), (int)this.ui_height-(int)(this.MarginTop+this.MarginBottom), DrawContentIDs);
                    }*/
                }
            }
        }

        protected virtual void pDrawContentID()
        {
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                this.DrawIdIn(this.i_x+(int)this.MarginLeft, this.i_y+(int)this.MarginTop, (int)this.ui_width-(int)(this.MarginLeft+this.MarginRight), (int)this.ui_height-(int)(this.MarginTop+this.MarginBottom), DrawContentIDs);
            }
        }

        protected virtual void pDrawID()
        {          
            if (this.b_ShowMe)
            {
                dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 1); // Pintamos ID de la superficie.
            }
            else
            {
                dge.G2D.IDsDrawer.DrawGuiGL(this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0); // Pintamos ID de la superficie.
            }
        }

        #region Protected Input Events:

        protected virtual void MDown(object sender, MouseButtonEventArgs e)
        {
            this.MouseDown(this, e);
        }

        protected virtual void MUp(object sender, MouseButtonEventArgs e)
        {
            this.MouseUp(this, e);
        }

        protected virtual void MMove(object sender, MouseMoveEventArgs e)
        {
            this.MouseMove(this, e);
        }

        protected virtual void MWheel(object sender, MouseWheelEventArgs e)
        {
            this.MouseWheel(this, e);
        }

        protected virtual void KPulsed(object sender, KeyBoardKeysEventArgs e)
        {
            this.KeyPulsed(this, e);
        }

        protected virtual void KReleased(object sender, KeyBoardKeysEventArgs e)
        {
            this.KeyReleased(this, e);
        }

        protected virtual void KCharReturned(object sender, KeyBoardTextEventArgs e)
        {
            this.KeyCharReturned(this, e);
        }

        #endregion

        #region Protected state Events:

        protected virtual void OnResize()
        {
            this.SetInternalDrawArea(this.i_x+(int)this.MarginLeft, this.i_y+(int)this.MarginTop, (int)this.ui_width-(int)(this.MarginLeft+this.MarginRight), (int)this.ui_height-(int)(this.MarginTop+this.MarginBottom));
        }

        protected virtual void OnReposition()
        {
            this.SetInternalDrawArea(this.i_x+(int)this.MarginLeft, this.i_y+(int)this.MarginTop, (int)this.ui_width-(int)(this.MarginLeft+this.MarginRight), (int)this.ui_height-(int)(this.MarginTop+this.MarginBottom));
            foreach(BaseGuiSurface surf in this.d_guiSurfaces.Values)
            {
                surf.intX = this.int_x+this.i_x+this.MarginsFromTheEdge[0];
                surf.intY = this.int_y+this.i_y+this.MarginsFromTheEdge[1];
            }
        }

        #endregion

        #region Properties

        public uint ID
        {
            get { return this.ui_id; }
        }

        public int X
        {
            set 
            { 
                this.i_x = value;
                foreach(BaseGuiSurface srf in this.d_guiSurfaces.Values)
                {
                    srf.intX = this.int_x+this.i_x;
                }
                this.OnReposition();
            }
            get { return this.i_x; }
        }

        internal int intX
        {
            set { this.int_x = value; this.OnReposition(); }
        }

        public int Y
        {
            set 
            { 
                this.i_y = value;
                foreach(BaseGuiSurface srf in this.d_guiSurfaces.Values)
                {
                    srf.intY = this.int_y+this.i_y;
                }
                this.OnReposition();
            }
            get { return this.i_y; }
        }

        internal int intY
        {
            set { this.int_y = value; this.OnReposition(); }
        }

        public uint Width
        {
            set { this.ui_width = value; this.SizeChanged(this, new ResizeEventArgs((int)this.ui_width, (int)this.ui_height)); }
            get { return this.ui_width; }
        }

        public uint Height
        {
            set { this.ui_height = value; this.SizeChanged(this, new ResizeEventArgs((int)this.ui_width, (int)this.ui_height)); }
            get { return this.ui_height; }
        }

        public int MarginTop
        {
            get { return this.MarginsFromTheEdge[1]; }
        }

        public int MarginBottom
        {
            get { return this.MarginsFromTheEdge[3]; }
        }

        public int MarginLeft
        {
            get { return this.MarginsFromTheEdge[0]; }
        }

        public int MarginRight
        {
            get { return this.MarginsFromTheEdge[2]; }
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

        protected virtual void OnGuiUpdate()
        {
            foreach(BaseGuiSurface surf in this.d_guiSurfaces.Values)
            { 
                surf.GUI = this.gui;
            }
        }

        internal /*virtual */GraphicsUserInterface GUI
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
                this.OnGuiUpdate();
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