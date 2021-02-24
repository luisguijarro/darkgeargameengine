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
        private Color4 idColor; // Color para la selección de ID.

        protected int i_x; // Coordenada X de posición de Objeto
        protected int i_y; // Coordenada Y de posición de Objeto)
        protected uint ui_width; // Ancho del Objeto en Pixeles
        protected uint ui_height; // Alto dle objeto en pixeles.

        #region CONTENIDO:
        private uint FrameBuffer; // Frame Buffer del renderizado de objetos contenidos.
        private uint DepthRenderBuffer; // Render Bufer de renderizado de objetos contenidos.
        internal bool contentUpdate; // Indicador de su se debe o no actualizar el conteido del objeto.

        #endregion

        protected TextureBufferObject textureBufferObject; // Textura base del Objeto. En Versión posterior se trasladará al tema del GUI.

        protected float[] Texcoords; // Coordenadas de textura base para el objeto.
        protected Vector2 tcDisplacement; // Variación de las coordenadas para eventos visuales como Pulsaciones de un botón.

        protected Vector4 MarginsFromTheEdge; // Margenes desde el borde al relleno del objeto.

        private bool b_visible; // ¿Es Visible la ventana?
        private GraphicsUserInterface gui; // GUI al que pertenece.
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
            this.ui_id = Core2D.GetID(); // Obtenemos ID de la superficie.
            byte[] colorvalues = Core2D.DeUIntAByte4(this.ui_id); // Obtenemos color a partir del ID.
            this.idColor = new Color4((byte)colorvalues[0], (byte)colorvalues[1], (byte)colorvalues[2], (byte)colorvalues[3]); // Establecemos color de ID.

            this.ui_width = width; // Establecemos ancho del objeto.
            this.ui_height = height; // Establecemos Alto del objeto.

            this.contentUpdate = false; // Por defecto el contenido no actualiza.
            this.FrameBuffer = GL.glGenFramebuffer(); // Generamos el Frame Buffer de renderizado del contenido.
            this.DepthRenderBuffer = GL.glGenRenderbuffer(); // Generamos el Render Buffer del renderizado del contenido.
            this.UpdateFrameAnbdRenderBuffers(); // Actualización de Bufferes de renderizado del contenido.

            this.b_visible = true; // El guiSurface es visible por defecto.
            this.d_guiSurfaces = new Dictionary<uint, BaseGuiSurface>(); // Lista de objetos Hijo.
            this.VisibleSurfaceOrder = new List<uint>(); // Lista de objetos hijo visibles.

            this.MarginsFromTheEdge = new Vector4(2, 2, 2, 2); // Margenes entre el borde y los vertices internos.

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

        private void UpdateFrameAnbdRenderBuffers()
        {
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, this.textureBufferObject.ui_ID);

            GL.glTexImage2D(TextureTarget.GL_TEXTURE_2D, 0, InternalFormat.GL_RGBA, (int)this.textureBufferObject.ui_width, (int)this.textureBufferObject.ui_height, 0, PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, new IntPtr(0));

            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MAG_FILTER, (int)TextureMagFilter.GL_NEAREST);
            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MIN_FILTER, (int)TextureMagFilter.GL_NEAREST);

            GL.glBindRenderbuffer(RenderbufferTarget.GL_RENDERBUFFER, this.DepthRenderBuffer);
            GL.glRenderbufferStorage(RenderbufferTarget.GL_RENDERBUFFER, InternalFormat.GL_DEPTH_COMPONENT, (int)this.textureBufferObject.ui_width, (int)this.textureBufferObject.ui_height);
            GL.glFramebufferRenderbuffer(FramebufferTarget.GL_FRAMEBUFFER, FramebufferAttachment.GL_DEPTH_ATTACHMENT, RenderbufferTarget.GL_RENDERBUFFER, this.DepthRenderBuffer);

            GL.glFramebufferTexture(FramebufferTarget.GL_FRAMEBUFFER, FramebufferAttachment.GL_COLOR_ATTACHMENT0, this.textureBufferObject.ui_ID, 0);
        }

        #endregion

        #region Protected

        protected void AddSurface(BaseGuiSurface surface)
        {
            if (!this.d_guiSurfaces.ContainsValue(surface))
            {
                surface.parentGuiSurface = this; // Adoptar guiSurface
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

        internal virtual void DrawID()
        {
            dge.G2D.IDsDrawer.DrawGL(this.idColor, this.i_x, this.i_y, this.ui_width, this.ui_height, 0); // Pintamos ID de la superficie.
        }

        internal virtual void DrawContent(G2D.GuiDrawer drawer)
        {
            for (int i=0;i<VisibleSurfaceOrder.Count;i++)
            {
                this.d_guiSurfaces[VisibleSurfaceOrder[i]].Draw(drawer);
            }
        }

        protected virtual void DrawContentIDs()
        {
            for (int i=0;i<VisibleSurfaceOrder.Count;i++)
            {
                this.d_guiSurfaces[VisibleSurfaceOrder[i]].DrawID();
            }
        }

        #endregion

        internal virtual void Draw(GuiDrawer drawer)
        {
            if (this.contentUpdate) 
            {
                GL.glBindFramebuffer(FramebufferTarget.GL_FRAMEBUFFER, this.FrameBuffer);
                GL.glViewport(0, 0, (int)this.textureBufferObject.ui_width, (int)this.textureBufferObject.ui_height);
                DrawContent(drawer);
                GL.glBindFramebuffer(FramebufferTarget.GL_FRAMEBUFFER, 0);
            }

            drawer.DrawGL(GraphicsUserInterface.DefaultThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge.ToArray(), Texcoords, this.tcDisplacement.ToArray(), 0);
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

        public int X
        {
            set { this.i_x = value;}
            get { return this.i_x; }
        }

        public int Y
        {
            set { this.i_y = value;}
            get { return this.i_y; }
        }

        public uint Width
        {
            set { this.ui_width = value; }
            get { return this.ui_width; }
        }

        public uint Height
        {
            set { this.ui_height = value; }
            get { return this.ui_height; }
        }

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