using System;
using dgtk;
using dgtk.Math;
using dgtk.OpenGL;
using dgtk.Graphics;
using dge.GLSL;
using System.Runtime.InteropServices;

namespace dge.GUI
{
    public class ColorPicker : BaseObjects.Control
    {
        #region StaticsAttributes

        private static bool IniciatedStatics;
        private static uint VAO_Color; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private static uint VAO_Light ; // Vertex Array Object (indice que contiene toda la info del objeto.)
        private static uint VBO_Color; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private static uint VBO_Light ; // Vertex Buffer Object (Indice del buffer Que contiene los atributos de vertice.)
        private static uint EBO_Color; // Element Buffer Object (Indice del buffer que contiene la lista de indices de orden de dibujado de los verticesColor.)
        private static uint EBO_Light;
        private static Shader ColorMapShader;
        private static Shader LightMapShader;
        #endregion //StaticsAttributes


        #region Static Uniforms Ids
        private static int idUniform_p_size2; // ID de Uniform de Tamaño de cada lado del punto a dibujar.
        private static int idUniform_v_size; // ID de Uniform de Tamaño de superficie de dibujo.
        private static int idUniformMat_View; // ID de Uniform que contiene la matriz de Projección.
        private static int idUniformMat_Per; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniformMat_Tra; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniform_v_size2; // ID de Uniform de Tamaño de superficie de dibujo.
        private static int idUniformMat_View2; // ID de Uniform que contiene la matriz de Projección.
        private static int idUniformMat_Per2; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniformMat_Tra2; // ID de Uniform que contiene la matriz de Perspectiva.
        private static int idUniform_InColor;

        #endregion //Static Uniforms Ids

        public float[] ColorPicker_CromSelector_Texcoords; // n=4
        public float[] ColorPicker_Picker_Texcoords; // n=4
        public int[] ColorPicker_CromSelectorMargins; // n=2
        public int[] ColorPicker_PickerMargins; // n=2
        public int[] ColorPicker_CromSelectorSize; // n=2
        public int[] ColorPicker_PickerSize; // n=2
        public int[] SelectorCoords; // n=2
        public int[] PickerCoords; // n=2
        private bool Pulsed_Chrome;
        private bool Pulsed_Light;
        int p_size; //Tamaño de cada lado del punto a dibujar para el Gradient (luz/Saturacion)
        int LightSaturation_Width;
        int LightSaturation_Height;
        float LightSaturation_Compensation_X;
        float LightSaturation_Compensation_Y;
        private Color4 c4_PreColorSelected;
        private Color4 c4_FinalColorSelected;

        public event EventHandler<SelectedColorEventArgs> ColorSelected;


        public ColorPicker() : this(140,160)
        {

        }

        public ColorPicker(int width, int height) : base(width, height)
        {
            this.Pulsed_Chrome = false;
            this.Pulsed_Light = false;
            this.c4_PreColorSelected = Color4.Red;
            this.c4_FinalColorSelected = Color4.Red;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ColorPicker_MarginsFromTheEdge;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ColorPicker_Texcoords;
            this.tcFrameOffset = new float[] {0f, 0f};

            this.ColorPicker_CromSelector_Texcoords = GuiTheme.DefaultGuiTheme.ColorPicker_CromSelector_Texcoords;
            this.ColorPicker_Picker_Texcoords = GuiTheme.DefaultGuiTheme.ColorPicker_Picker_Texcoords;
            this.ColorPicker_CromSelectorMargins = GuiTheme.DefaultGuiTheme.ColorPicker_CromSelectorMargins;
            this.ColorPicker_PickerMargins = GuiTheme.DefaultGuiTheme.ColorPicker_PickerMargins;
            this.ColorPicker_CromSelectorSize = GuiTheme.DefaultGuiTheme.ColorPicker_CromSelectorSize;
            this.ColorPicker_PickerSize = GuiTheme.DefaultGuiTheme.ColorPicker_PickerSize;

            this.PickerCoords = new int[]
            {
                this.i_x+(int)(this.Width-(12+this.MarginLeft+this.MarginRight))+ColorPicker_PickerMargins[0], 
                this.i_y+this.MarginTop-ColorPicker_PickerMargins[1]
            };
            this.ColorSelected += delegate{};

            this.OnResize();
        }
        #region PROTECTED INPUT EVENTS

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);            

            if (e.ID == this.ui_id)
            {
                // Color
                if (this.IsMouseIn(e.X, e.Y, this.Width-(10+this.MarginRight)-1, this.MarginTop, this.Width-this.MarginRight, this.Height-this.MarginRight))
                {
                    this.c4_PreColorSelected = this.ReadColor(e);
                    this.Pulsed_Chrome = true;
                }

                //Light
                if (this.IsMouseIn(e.X, e.Y, this.MarginLeft, this.MarginTop+1, this.MarginLeft+LightSaturation_Width, this.MarginTop+LightSaturation_Height))
                {
                    this.c4_FinalColorSelected = this.ReadColor(e);
                    //this.PickerCoords[0] = e.X-this.ColorPicker_PickerMargins[0];
                    //this.PickerCoords[1] = e.Y-this.ColorPicker_PickerMargins[1];
                    this.ColorSelected(this, new SelectedColorEventArgs(this.c4_FinalColorSelected, false));
                    this.Pulsed_Light = true;
                }
            }         
        }

        protected override void OnMMove(object sender, MouseMoveEventArgs e)
        {
            base.OnMMove(sender, e);
            if (this.Pulsed_Chrome)
            {
                // Color
                if (this.IsMouseIn(e.X, e.Y, this.Width-(10+this.MarginRight)-1, this.MarginTop, this.Width-this.MarginRight, this.Height-this.MarginRight))
                {
                    this.c4_PreColorSelected = this.ReadColor(new MouseButtonEventArgs(e.X, e.Y, MouseButtons.Left, PushRelease.Push, e.ID));
                }
            }
            if (this.Pulsed_Light)
            {
                //Light
                if (this.IsMouseIn(e.X, e.Y, this.MarginLeft, this.MarginTop+1, this.MarginLeft+LightSaturation_Width, this.MarginTop+LightSaturation_Height))
                {
                    this.c4_FinalColorSelected = this.ReadColor(new MouseButtonEventArgs(e.X, e.Y, MouseButtons.Left, PushRelease.Push, e.ID));
                    this.ColorSelected(this, new SelectedColorEventArgs(this.c4_FinalColorSelected, false));
                    //this.PickerCoords[0] = e.X-this.ColorPicker_PickerMargins[0];
                    //this.PickerCoords[1] = e.Y-this.ColorPicker_PickerMargins[1];
                }
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMUp(sender, e);
            this.Pulsed_Chrome = false;
            this.Pulsed_Light = false;
        }

        private byte[] ReadColorBytes(MouseButtonEventArgs e)
        {
            byte[] bytes = new byte[4];
            lock(this.gui.ParentWindow.LockObject)
            {
                this.gui.ParentWindow.MakeCurrent();
                GL.glReadBuffer(ReadBufferMode.GL_FRONT);                

                IntPtr ptr_ret = Marshal.AllocHGlobal(3); //bytes.Length);
                GL.glReadPixels(e.X, (this.gui.ParentWindow.Height-((this.gui.l_menus.Count>0) ? this.gui.mheight : 0))-(e.Y), 1, 1, PixelFormat.GL_RGB, PixelType.GL_UNSIGNED_BYTE, ptr_ret);
                this.gui.ParentWindow.UnMakeCurrent();

                Marshal.Copy(ptr_ret, bytes, 0, 3); //bytes.Length);
                bytes[3] = 255;
                Console.WriteLine(DateTime.Now.ToLongTimeString()+" -> COLOR: R({0}), G({1}), B({2})", bytes[0], bytes[1], bytes[2]);
                Marshal.FreeHGlobal(ptr_ret);
            }

            return bytes;
        }

        private Color4 ReadColor(MouseButtonEventArgs e)
        {
            return new Color4(this.ReadColorBytes(e));
        }

        #endregion

        #region PROTECTED METODS:

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ColorPicker_MarginsFromTheEdge;
            this.Texcoords = this.gui.gt_ActualGuiTheme.ColorPicker_Texcoords;
            this.tcFrameOffset = new float[] {0f, 0f};

            this.ColorPicker_CromSelector_Texcoords = this.gui.gt_ActualGuiTheme.ColorPicker_CromSelector_Texcoords;
            this.ColorPicker_Picker_Texcoords = this.gui.gt_ActualGuiTheme.ColorPicker_Picker_Texcoords;
            this.ColorPicker_CromSelectorMargins = this.gui.gt_ActualGuiTheme.ColorPicker_CromSelectorMargins;
            this.ColorPicker_PickerMargins = this.gui.gt_ActualGuiTheme.ColorPicker_PickerMargins;
            this.ColorPicker_CromSelectorSize = this.gui.gt_ActualGuiTheme.ColorPicker_CromSelectorSize;
            this.ColorPicker_PickerSize = this.gui.gt_ActualGuiTheme.ColorPicker_PickerSize;

            base.UpdateTheme();
        }
        protected override void OnResize()
        {
            base.OnResize();
            LightSaturation_Width = this.InnerSize.Width - (this.MarginLeft+this.MarginRight+11); //Margenes que chocan de los dos recuadros más 10pixeles del croma más un pixel de separación.
            LightSaturation_Height = this.InnerSize.Height-(21);
            if (LightSaturation_Width > LightSaturation_Height)
            {
                this.LightSaturation_Compensation_X = 0;
                this.LightSaturation_Compensation_Y = (LightSaturation_Width-LightSaturation_Height)/2f;
            }
            else
            {
                this.LightSaturation_Compensation_Y = 0;
                this.LightSaturation_Compensation_X = (LightSaturation_Height-LightSaturation_Width)/2f;
            }
            this.p_size = (LightSaturation_Width>LightSaturation_Height) ? LightSaturation_Width : LightSaturation_Height;
        }
        protected override void OnReposition()
        {
            base.OnReposition();
            this.PickerCoords = new int[]{this.X, this.Y};
        }

        protected override void InputSizeAlter(int width, int height)
        {
            this.i_width = width;
            this.i_height = height;
        }

        protected override int[] OutputSizeAlter(int width, int height)
        {
            int[] ret;
            ret = new int[] {width, height};
            return ret;
        }

        #endregion // PROTECTED METHODS

        #region PROTECTED DRAW METHODS:

        protected override void pDraw()
        {
            this.gui.Drawer.m4P = this.gui.GuiDrawer.m4P;
            if (!IniciatedStatics)
            {
                if (this.gui.GuiDrawer.IsGLES)
                {
                    InitColorMapShaderGLES();
                    InitColorMapSurfaceGLES();
                }
                else
                {
                    InitColorMapShaderGL();
                    InitColorMapSurfaceGL();
                }

                IniciatedStatics = true;
            }
            
            if (this.b_ShowMe) 
            {
                // Light:
                this.DrawIn(this.i_x+this.MarginLeft,this.i_y+this.MarginTop, this.LightSaturation_Width, this.LightSaturation_Height, DrawLightMapLauncher);
                // Marco:
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, (int)(this.i_width-(11+this.MarginLeft+this.MarginRight)), this.LightSaturation_Height+this.MarginTop+this.MarginBottom, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);

                // Color:
                this.DrawIn(this.i_x+this.MarginLeft+this.InnerSize.Width - (10),this.i_y+this.MarginTop, 10, this.InnerSize.Height, DrawColorMapLauncher);
                // Marco:
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x+this.Width - (10+this.MarginLeft+this.MarginRight), this.i_y, 10+this.MarginLeft+this.MarginRight, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);

                // Pintar Fondo para alfa:
                this.gui.Drawer.Draw(this.gui.GuiTheme.ThemeTBO.ID, Color4.White,
                this.i_x+this.MarginLeft, this.i_y+this.Height - 20, 0,
                (this.LightSaturation_Width/2)-this.MarginLeft, 20, 0f,
                this.gui.GuiTheme.TransparentBackground_TexCoords[0], this.gui.GuiTheme.TransparentBackground_TexCoords[1],
                this.gui.GuiTheme.TransparentBackground_TexCoords[2], this.gui.GuiTheme.TransparentBackground_TexCoords[3]);
                
                // Pintar Color Final:
                this.gui.Drawer.Draw(this.c4_FinalColorSelected,
                this.i_x+this.MarginLeft, this.i_y+this.Height - 20, 0,
                (this.LightSaturation_Width/2)-this.MarginLeft, 20, 0f);
                
                //Pintar Marco Color Final:
                this.gui.Drawer.m4P = this.gui.GuiDrawer.m4P;
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White,
                this.i_x, this.i_y+this.Height - 20,
                this.LightSaturation_Width/2, 20, 0f,
                this.MarginsFromTheEdge, this.Texcoords, this.tcFrameOffset, 0);
            }
            
            //Draw Pickers:
            /*
            this.gui.Drawer.m4P = this.gui.GuiDrawer.m4P; 
            this.gui.Drawer.Draw(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, 
            this.PickerCoords[0], this.PickerCoords[1], 
            this.ColorPicker_PickerSize[0], this.ColorPicker_PickerSize[1], 0f, 
            this.ColorPicker_Picker_Texcoords[0], this.ColorPicker_Picker_Texcoords[1], this.ColorPicker_Picker_Texcoords[2], this.ColorPicker_Picker_Texcoords[3]);
            
            */
            
        }
        protected override void pDrawID()
        {
            dge.G2D.IDsDrawer.DrawGL2D(this.idColor, this.i_x, this.i_y, this.i_width, this.i_height, 0); // Pintamos ID de la superficie.
        }

        #endregion // PROTECTED DRAW METHODS

        #region PRIVATE  GL METHODS:

        private static void InitColorMapShaderGL()
        {
            ColorMapShader = new Shader(dge.G2D.ShadersSourcesGL.ColorMapvs, dge.G2D.ShadersSourcesGL.ColorMapfs, false);

            idUniform_v_size = GL.glGetUniformLocation(ColorMapShader.ID, "v_size");
            idUniformMat_View = GL.glGetUniformLocation(ColorMapShader.ID, "view");
            idUniformMat_Per = GL.glGetUniformLocation(ColorMapShader.ID, "perspective");
            idUniformMat_Tra = GL.glGetUniformLocation(ColorMapShader.ID, "trasform");

            GL.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE,dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

            LightMapShader = new Shader(dge.G2D.ShadersSourcesGL.ColorLightMapvs, dge.G2D.ShadersSourcesGL.ColorLightMapfs, false);

            idUniform_InColor = GL.glGetUniformLocation(LightMapShader.ID, "InColor");
            idUniform_p_size2 = GL.glGetUniformLocation(LightMapShader.ID, "p_size");
            idUniform_v_size2 = GL.glGetUniformLocation(LightMapShader.ID, "v_size");
            idUniformMat_View2 = GL.glGetUniformLocation(LightMapShader.ID, "view");
            idUniformMat_Per2 = GL.glGetUniformLocation(LightMapShader.ID, "perspective");
            idUniformMat_Tra2 = GL.glGetUniformLocation(LightMapShader.ID, "trasform");


            GL.glUniformMatrix(idUniformMat_View2, dgtk.OpenGL.Boolean.GL_FALSE,dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));
            GL.glUniform3f(idUniform_InColor, 1f, 0f, 0f);
        }

        private static void InitColorMapSurfaceGL()
        {
            TVertexColorMap[] verticesColor = new TVertexColorMap[14];

            verticesColor[0] = new TVertexColorMap(1, new Vector2(0, 0f));
            verticesColor[1] = new TVertexColorMap(2, new Vector2(0, 1/6f*1));
            verticesColor[2] = new TVertexColorMap(3, new Vector2(0, 1/6f*2));
            verticesColor[3] = new TVertexColorMap(4, new Vector2(0, 1/6f*3));
            verticesColor[4] = new TVertexColorMap(5, new Vector2(0, 1/6f*4));
            verticesColor[5] = new TVertexColorMap(6, new Vector2(0, 1/6f*5));
            verticesColor[6] = new TVertexColorMap(1, new Vector2(0, 1f));

            verticesColor[7] = new TVertexColorMap(1, new Vector2(1, 0f));
            verticesColor[8] = new TVertexColorMap(2, new Vector2(1, 1/6f*1));
            verticesColor[9] = new TVertexColorMap(3, new Vector2(1, 1/6f*2));
            verticesColor[10] = new TVertexColorMap(4, new Vector2(1, 1/6f*3));
            verticesColor[11] = new TVertexColorMap(5, new Vector2(1, 1/6f*4));
            verticesColor[12] = new TVertexColorMap(6, new Vector2(1, 1/6f*5));
            verticesColor[13] = new TVertexColorMap(1, new Vector2(1, 1f));

            uint[] indicesColor = new uint[]{7, 0, 8, 1, 9, 2, 10, 3, 11, 4, 12, 5, 13, 6};

            VAO_Color = GL.glGenVertexArray(); 
            VBO_Color = GL.glGenBuffer();
            EBO_Color = GL.glGenBuffer();

            GL.glBindVertexArray(VAO_Color);
            GL.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO_Color);
            GL.glBufferData<TVertexColorMap>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertexColorMap))*verticesColor.Length, verticesColor, BufferUsageARB.GL_STATIC_DRAW);

            GL.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO_Color);
            GL.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indicesColor.Length, indicesColor, BufferUsageARB.GL_STATIC_DRAW);
            
            GL.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(0)); // ID
            GL.glEnableVertexAttribArray(0);

            GL.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(sizeof(int))); // Vertex Position
            GL.glEnableVertexAttribArray(1);

            GL.glBindVertexArray(0);

            //------------------------------------------------------------------------

            VAO_Light = GL.glGenVertexArray(); 
            VBO_Light = GL.glGenBuffer();
            EBO_Light = GL.glGenBuffer();

            TVertexColorMap[] verticesLuz = new TVertexColorMap[1];
            verticesLuz[0] = new TVertexColorMap(1, new Vector2(0.5f, 0.5f));
            /*verticesLuz[0] = new TVertexColorMap(1, new Vector2(0, 0));            
            verticesLuz[1] = new TVertexColorMap(2, new Vector2(0, 1));
            verticesLuz[2] = new TVertexColorMap(3, new Vector2(1, 0));            
            verticesLuz[3] = new TVertexColorMap(4, new Vector2(1, 1));*/

            uint[] indicesLuz = new uint[]{0}; //{2, 1, 0, 2, 3, 1};


            VAO_Light = GL.glGenVertexArray(); 
            VBO_Light = GL.glGenBuffer();
            EBO_Light = GL.glGenBuffer();

            GL.glBindVertexArray(VAO_Light);
            GL.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO_Light);
            GL.glBufferData<TVertexColorMap>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertexColorMap))*verticesLuz.Length, verticesLuz, BufferUsageARB.GL_STATIC_DRAW);

            GL.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO_Light);
            GL.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indicesLuz.Length, indicesLuz, BufferUsageARB.GL_STATIC_DRAW);
            
            GL.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(0)); // ID
            GL.glEnableVertexAttribArray(0);

            GL.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(sizeof(int))); // Vertex Position
            GL.glEnableVertexAttribArray(1);

            GL.glBindVertexArray(0);
        }

        #endregion

        #region PRIVATE  GLES METHODS:

        private static void InitColorMapShaderGLES()
        {
            Console.WriteLine("ColorPicker Shader Compilation.");
            ColorMapShader = new Shader(dge.G2D.ShadersSourcesGLES.ColorMapvs, dge.G2D.ShadersSourcesGLES.ColorMapfs, true);

            idUniform_v_size = GLES.glGetUniformLocation(ColorMapShader.ID, "v_size");
            idUniformMat_View = GLES.glGetUniformLocation(ColorMapShader.ID, "view");
            idUniformMat_Per = GLES.glGetUniformLocation(ColorMapShader.ID, "perspective");
            idUniformMat_Tra = GLES.glGetUniformLocation(ColorMapShader.ID, "trasform");

            GLES.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE,dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));

            Console.WriteLine("LightMapPicker Shader Compilation.");
            LightMapShader = new Shader(dge.G2D.ShadersSourcesGLES.ColorLightMapvs, dge.G2D.ShadersSourcesGLES.ColorLightMapfs, true);

            idUniform_InColor = GLES.glGetUniformLocation(LightMapShader.ID, "InColor");
            idUniform_p_size2 = GLES.glGetUniformLocation(LightMapShader.ID, "p_size");
            idUniform_v_size2 = GLES.glGetUniformLocation(LightMapShader.ID, "v_size");
            idUniformMat_View2 = GLES.glGetUniformLocation(LightMapShader.ID, "view");
            idUniformMat_Per2 = GLES.glGetUniformLocation(LightMapShader.ID, "perspective");
            idUniformMat_Tra2 = GLES.glGetUniformLocation(LightMapShader.ID, "trasform");


            GLES.glUniformMatrix(idUniformMat_View2, dgtk.OpenGL.Boolean.GL_FALSE,dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f)));
            GLES.glUniform3f(idUniform_InColor, 1f, 0f, 0f);
        }

        private static void InitColorMapSurfaceGLES()
        {
            TVertexColorMap[] verticesColor = new TVertexColorMap[14];

            verticesColor[0] = new TVertexColorMap(1, new Vector2(0, 0f));
            verticesColor[1] = new TVertexColorMap(2, new Vector2(0, 1/6f*1));
            verticesColor[2] = new TVertexColorMap(3, new Vector2(0, 1/6f*2));
            verticesColor[3] = new TVertexColorMap(4, new Vector2(0, 1/6f*3));
            verticesColor[4] = new TVertexColorMap(5, new Vector2(0, 1/6f*4));
            verticesColor[5] = new TVertexColorMap(6, new Vector2(0, 1/6f*5));
            verticesColor[6] = new TVertexColorMap(1, new Vector2(0, 1f));

            verticesColor[7] = new TVertexColorMap(1, new Vector2(1, 0f));
            verticesColor[8] = new TVertexColorMap(2, new Vector2(1, 1/6f*1));
            verticesColor[9] = new TVertexColorMap(3, new Vector2(1, 1/6f*2));
            verticesColor[10] = new TVertexColorMap(4, new Vector2(1, 1/6f*3));
            verticesColor[11] = new TVertexColorMap(5, new Vector2(1, 1/6f*4));
            verticesColor[12] = new TVertexColorMap(6, new Vector2(1, 1/6f*5));
            verticesColor[13] = new TVertexColorMap(1, new Vector2(1, 1f));

            uint[] indicesColor = new uint[]{7, 0, 8, 1, 9, 2, 10, 3, 11, 4, 12, 5, 13, 6};

            VAO_Color = GLES.glGenVertexArray(); 
            VBO_Color = GLES.glGenBuffer();
            EBO_Color = GLES.glGenBuffer();

            GLES.glBindVertexArray(VAO_Color);
            GLES.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO_Color);
            GLES.glBufferData<TVertexColorMap>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertexColorMap))*verticesColor.Length, verticesColor, BufferUsageARB.GL_STATIC_DRAW);

            GLES.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO_Color);
            GLES.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indicesColor.Length, indicesColor, BufferUsageARB.GL_STATIC_DRAW);
            
            GLES.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(0)); // ID
            GLES.glEnableVertexAttribArray(0);

            GLES.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(sizeof(int))); // Vertex Position
            GLES.glEnableVertexAttribArray(1);

            GLES.glBindVertexArray(0);

            //------------------------------------------------------------------------

            VAO_Light = GLES.glGenVertexArray(); 
            VBO_Light = GLES.glGenBuffer();
            EBO_Light = GLES.glGenBuffer();

            TVertexColorMap[] verticesLuz = new TVertexColorMap[1];
            verticesLuz[0] = new TVertexColorMap(1, new Vector2(0.5f, 0.5f));
            /*verticesLuz[0] = new TVertexColorMap(1, new Vector2(0, 0));            
            verticesLuz[1] = new TVertexColorMap(2, new Vector2(0, 1));
            verticesLuz[2] = new TVertexColorMap(3, new Vector2(1, 0));            
            verticesLuz[3] = new TVertexColorMap(4, new Vector2(1, 1));*/

            uint[] indicesLuz = new uint[]{0}; //{2, 1, 0, 2, 3, 1};


            VAO_Light = GLES.glGenVertexArray(); 
            VBO_Light = GLES.glGenBuffer();
            EBO_Light = GLES.glGenBuffer();

            GLES.glBindVertexArray(VAO_Light);
            GLES.glBindBuffer(BufferTargetARB.GL_ARRAY_BUFFER, VBO_Light);
            GLES.glBufferData<TVertexColorMap>(BufferTargetARB.GL_ARRAY_BUFFER, Marshal.SizeOf(typeof(TVertexColorMap))*verticesLuz.Length, verticesLuz, BufferUsageARB.GL_STATIC_DRAW);

            GLES.glBindBuffer(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, EBO_Light);
            GLES.glBufferData<UInt32>(BufferTargetARB.GL_ELEMENT_ARRAY_BUFFER, sizeof(uint)*indicesLuz.Length, indicesLuz, BufferUsageARB.GL_STATIC_DRAW);
            
            GLES.glVertexAttribIPointer(0, 1, VertexAttribIType.GL_INT, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(0)); // ID
            GLES.glEnableVertexAttribArray(0);

            GLES.glVertexAttribPointer(1, 2, VertexAttribPointerType.GL_FLOAT, dgtk.OpenGL.Boolean.GL_FALSE, Marshal.SizeOf(typeof(TVertexColorMap)), new IntPtr(sizeof(int))); // Vertex Position
            GLES.glEnableVertexAttribArray(1);

            GLES.glBindVertexArray(0);
        }

        #endregion

        #region PRIVATE DRAW METHODS:
        private void DrawColorMapLauncher()
        {
            this.DrawColorMap(0, 0, this.gui.Drawer.m4P);
        }
        private void DrawLightMapLauncher()
        {
            this.DrawLigthMap(this.LightSaturation_Compensation_X, this.LightSaturation_Compensation_Y, this.gui.Drawer.m4P);
        }

        private void DrawColorMap(float x, float y, Mat4 Drawer2D_PerMap)
        {
            //dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D(0, new Vector2(0, 0));
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.

            ColorMapShader.Use();

            Mat4 m4P = Drawer2D_PerMap; //dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, this.InnerSize.Width, this.InnerSize.Width, y, -100f, 100f);
            GL.glUniformMatrix(idUniformMat_Per, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            GL.glUniformMatrix(idUniformMat_Tra, dgtk.OpenGL.Boolean.GL_FALSE, m4T/* * m4R*/); // Transmitimos al Shader la trasformación.
            GL.glUniformMatrix(idUniformMat_View, dgtk.OpenGL.Boolean.GL_FALSE,dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f))); //¿Solo necesario en el inicio?

            GL.glUniform2f(idUniform_v_size, 10/*this.InnerSize.Width*/ /* - Elementos */,this.InnerSize.Height /* - Elementos */);
            GL.glBindVertexArray(VAO_Color);
            GL.glDrawElements(PrimitiveType.GL_TRIANGLE_STRIP, 14, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }
        private void DrawLigthMap(float x, float y, Mat4 Drawer2D_PerMap)
        {
            //GL.glEnable(EnableCap.GL_PROGRAM_POINT_SIZE);
            GL.glEnable(EnableCap.GL_POINT_SMOOTH);
            GL.glPointSize(this.p_size);
            GL.glEnable((EnableCap)0x8861);
            //glTexEnvi(GL_POINT_SPRITE, GL_COORD_REPLACE, GL_TRUE);
            //GL.glDisable(EnableCap.GL_POLYGON_SMOOTH);
            dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, 0)); // Creamos la Matriz de traslación.

            LightMapShader.Use();
            GL.glUniform1f(idUniform_p_size2, (float)this.p_size);
            GL.glUniform3f(idUniform_InColor, this.c4_PreColorSelected.R, this.c4_PreColorSelected.G, this.c4_PreColorSelected.B);
            Mat4 m4P = Drawer2D_PerMap; //dgtk.Math.MatrixTools.MakeOrthoPerspectiveMatrix(x, this.InnerSize.Width, this.InnerSize.Width, y, -100f, 100f);
            GL.glUniformMatrix(idUniformMat_Per2, dgtk.OpenGL.Boolean.GL_FALSE, m4P);
            GL.glUniformMatrix(idUniformMat_Tra2, dgtk.OpenGL.Boolean.GL_FALSE, m4T/* * m4R*/); // Transmitimos al Shader la trasformación.
            GL.glUniformMatrix(idUniformMat_View2, dgtk.OpenGL.Boolean.GL_FALSE,dgtk.Math.MatrixTools.MakeTraslationMatrix(new dgtk.Math.Vector3(0f,0f,0f))); //¿Solo necesario en el inicio?

            GL.glUniform2f(idUniform_v_size2, this.LightSaturation_Width,this.LightSaturation_Height);
            GL.glBindVertexArray(VAO_Light);
            GL.glDrawElements(PrimitiveType.GL_POINTS, 1, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
            GL.glBindVertexArray(0);
        }

        #endregion

        #region PROPERTIES:

        public Color4 SelectedColor
        {
            get { return this.c4_FinalColorSelected; }
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TVertexColorMap
    {
        internal int i_id;
        internal Vector2 v2pos;
        internal TVertexColorMap(int id, Vector2 pos)
        {
            this.i_id = id;
            this.v2pos = pos;
        }
    }
}