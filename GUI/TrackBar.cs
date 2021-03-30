using System;
using dgtk;
using dgtk.Graphics;

namespace dge.GUI
{    
    public class TrackBar : BaseObjects.Control
    {
        int lastX, lastY;
        private bool b_pulsed;
        private int i_Slide_XPos;
        private int i_Slide_YPos;
        private int i_value;
        private int i_MaxValue;
        private int i_MinValue;
        private Orientation o_Orientation; // indica si se visualiza Horizontal o Verticalmente,
        private SliderShape ss_SliderShape; // indica la forma del deslizador, Flecha o duadrado.
        private SliderOrientation so_SliderOrientation; // Indica hacia donde apunta el deslizador si es Flecha. 
        private float[] Slider_Texcoords;
        private int [] TrackBar_Slider_Size;

        public event EventHandler<IntValueChangedEventArgs> ValueChanged;

        public TrackBar() : base(66, 22)
        {
            this.o_Orientation = Orientation.Horizontal;
            this.ss_SliderShape = SliderShape.Square;
            this.so_SliderOrientation = SliderOrientation.UpRigth;
            this.i_MinValue = 0;
            this.i_MaxValue = 100;
            this.i_value = 0;
            this.ValueChanged += delegate { };

            this.UpdateShape();
            this.UpdateShapePos();
        }

        protected override void MDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            base.MDown(sender, e);
            if (Core2D.SelectedID == this.ui_id)
            {
                this.b_pulsed = true;
                this.lastX = e.X;
                this.lastY = e.Y;
            }
        }

        protected override void MUp(object sender, dgtk_MouseButtonEventArgs e)
        {
            base.MUp(sender, e);
            this.b_pulsed = false;
        }

        protected override void MMove(object sender, dgtk_MouseMoveEventArgs e)
        {
            base.MMove(sender, e);
            if ((this.b_pulsed) && (Core2D.SelectedID == this.ui_id))
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (((this.i_Slide_XPos + e.X-lastX) >= 0) && ((this.i_Slide_XPos + e.X-lastX) <= this.ui_width-this.TrackBar_Slider_Size[0]))
                    {
                        this.i_Slide_XPos += e.X-lastX;
                        int pixelrange = (int)this.ui_width-(TrackBar_Slider_Size[0]);
                        int valuerange = this.i_MaxValue-this.i_MinValue;
                        float mult = (float)valuerange / (float)pixelrange;
                        this.i_value = (int)(this.i_Slide_XPos * mult);  
                        Console.WriteLine("Valor Hor: "+this.i_value);
                    }
                }
                else
                {
                    if (((this.i_Slide_YPos + e.Y-lastY) >= 0) && ((this.i_Slide_YPos + e.Y-lastY) <= this.ui_height-this.TrackBar_Slider_Size[1]))
                    {
                        this.i_Slide_YPos += e.Y-lastY;
                        int pixelrange = (int)this.ui_height-(TrackBar_Slider_Size[1]);
                        int valuerange = this.i_MaxValue-this.i_MinValue;
                        float mult = (float)valuerange / (float)pixelrange;
                        this.i_value = (int)((pixelrange-this.i_Slide_YPos) * mult);
                        Console.WriteLine("Valor Ver: "+this.i_value);
                    }
                }
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value));
                this.lastX = e.X;
                this.lastY = e.Y;
            }
        }

        #region PRIVATE METHODS:
        private void UpdateShapePos()
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                int pixelrange = (int)this.ui_width-(this.TrackBar_Slider_Size[0]);
                int valuerange = this.i_MaxValue-this.i_MinValue;
                float mult = (float)pixelrange / (float)valuerange;
                this.i_Slide_XPos = (int)(mult*this.i_value);
                this.i_Slide_YPos = GuiTheme.DefaultGuiTheme.TrackBar_Slider_PosMargin; 
            }
            else
            {
                int pixelrange = (int)this.ui_height-(this.TrackBar_Slider_Size[1]);
                int valuerange = this.i_MaxValue-this.i_MinValue;
                float mult = (float)pixelrange / (float)valuerange;
                this.i_Slide_YPos = pixelrange-(int)(mult*(this.i_value));
                this.i_Slide_XPos = GuiTheme.DefaultGuiTheme.TrackBar_Slider_PosMargin;
            }
        }

        private void UpdateShape()
        {
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.TrackBar_FrameOffset;
            if (this.Orientation == Orientation.Horizontal)
            {
                this.TrackBar_Slider_Size = new int[]{ GuiTheme.DefaultGuiTheme.TrackBar_Slider_Size[0], GuiTheme.DefaultGuiTheme.TrackBar_Slider_Size[1] };
                this.Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Hor_Texcoords;
                this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TrackBar_Hor_MarginsFromTheEdge;
                if (SliderShape == SliderShape.Square)
                {
                    this.Slider_Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Square_Hor_Slider_Texcoords;
                }
                else
                {
                    if (SliderOrientation == SliderOrientation.UpRigth)
                    {
                        this.Slider_Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Arrow_Up_Slider_Texcoords;
                    }
                    else
                    {
                        this.Slider_Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Arrow_Down_Slider_Texcoords;
                    }
                }
            }
            else
            {
                this.TrackBar_Slider_Size = new int[]{ GuiTheme.DefaultGuiTheme.TrackBar_Slider_Size[2], GuiTheme.DefaultGuiTheme.TrackBar_Slider_Size[3] };
                this.Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Ver_Texcoords;
                this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.TrackBar_Ver_MarginsFromTheEdge;
                if (SliderShape == SliderShape.Square)
                {
                    this.Slider_Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Square_Ver_Slider_Texcoords;
                }
                else
                {
                    if (SliderOrientation == SliderOrientation.UpRigth)
                    {
                        this.Slider_Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Arrow_Right_Slider_Texcoords;
                    }
                    else
                    {
                        this.Slider_Texcoords = GuiTheme.DefaultGuiTheme.TrackBar_Arrow_Left_Slider_Texcoords;
                    }
                }
            }
        }
        #endregion

        #region VIRTUAL/OVERRIDE METHODS:

        protected override void OnResize()
        {
            base.OnResize();
            if (this.o_Orientation == Orientation.Vertical)
            {
                /*base.Width*/ this.ui_width = (uint)GuiTheme.DefaultGuiTheme.TrackBar_Ver_MaxWidth;
            }
            else
            {
                /*base.Height*/ this.ui_height = (uint)GuiTheme.DefaultGuiTheme.TrackBar_Hor_MaxHeight;
            }
            this.UpdateShapePos();
            this.UpdateShape();
        }

        internal override void Draw()
        {
            base.Draw();
            base.DrawIn(this.i_x, this.i_y, (int)this.ui_width, (int)this.ui_height, DrawSliderShape);
            //DrawSliderShape();
        }
        internal override void DrawID()
        {
            //base.DrawID();
            dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, new Color4(0f,0f,0f,0f), this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 1); // Pintamos sin ID de la superficie.
           
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                this.DrawIdIn(this.i_x-(int)this.MarginsFromTheEdge[0], this.i_y+(int)this.MarginsFromTheEdge[1], (int)this.ui_width-(int)(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), (int)this.ui_height-(int)(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3]), DrawContentIDs);
            }

            base.DrawIdIn(this.i_x, this.i_y, (int)this.ui_width, (int)this.ui_height, DrawSliderIDShape);
        }

        private void DrawSliderShape()
        {
            this.gui.Drawer.Draw(GuiTheme.DefaultGuiTheme.ThemeTBO.ID, this.i_Slide_XPos, this.i_Slide_YPos, (uint)this.TrackBar_Slider_Size[0], (uint)this.TrackBar_Slider_Size[1], 0f, this.Slider_Texcoords[0], this.Slider_Texcoords[2], this.Slider_Texcoords[1], this.Slider_Texcoords[3]);
            //this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, dgtk.Graphics.Color4.White, this.i_Slide_XPos, this.i_Slide_YPos, (uint)this.TrackBar_Slider_Size[0], (uint)this.TrackBar_Slider_Size[1], 0f, this.MarginsFromTheEdge, this.Slider_Texcoords, this.tcFrameOffset, 0);
        }

        private void DrawSliderIDShape()
        {
            dge.G2D.IDsDrawer.DrawGL2D(GuiTheme.DefaultGuiTheme.ThemeSltTBO.ID, this.idColor,  this.i_Slide_XPos, this.i_Slide_YPos, (uint)this.TrackBar_Slider_Size[0], (uint)this.TrackBar_Slider_Size[1], 0f, this.Slider_Texcoords[0], this.Slider_Texcoords[2], this.Slider_Texcoords[1], this.Slider_Texcoords[3], 1);
            //dge.G2D.IDsDrawer.DrawGuiGL(GuiTheme.DefaultGuiTheme.ThemeSltTBO.ID, this.idColor,  this.i_Slide_XPos, this.i_Slide_YPos, (uint)this.TrackBar_Slider_Size[0], (uint)this.TrackBar_Slider_Size[1], 0f, this.MarginsFromTheEdge, this.Slider_Texcoords, new float[]{0f, 0f}, 1);
            //this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, dgtk.Graphics.Color4.White, this.i_Slide_XPos, this.i_Slide_YPos, (uint)this.TrackBar_Slider_Size[0], (uint)this.TrackBar_Slider_Size[1], 0f, this.MarginsFromTheEdge, this.Slider_Texcoords, this.tcFrameOffset, 0);
        }

        #endregion

        #region PROPERTIES:
        public Orientation Orientation
        {
            set 
            { 
                if (this.o_Orientation != value)
                {
                    uint temp = this.ui_width;
                    this.ui_width = this.ui_height;
                    this.ui_height = temp;
                }
                this.o_Orientation = value; 
                this.UpdateShapePos();
                this.UpdateShape(); 
            }
            get { return this.o_Orientation; }
        }

        public SliderShape SliderShape
        {
            set { this.ss_SliderShape = value; this.UpdateShapePos(); this.UpdateShape();}
            get { return this.ss_SliderShape; }
        }

        public SliderOrientation SliderOrientation
        {
            set { this.so_SliderOrientation = value; this.UpdateShapePos(); this.UpdateShape();}
            get { return this.so_SliderOrientation; } 
        }

        public int MaxValue
        {
            set 
            { 
                if (value <= this.i_MinValue) // Si el nuevo MaxValue es menor que el MinValue.
                {
                    this.i_MaxValue = this.i_MinValue+1; // Establecemos el MaxValue por encima del MinValue,
                    this.i_value = this.i_MinValue; // Establecemos el Valor igual al MinValue.
                }
                else
                {
                    if (value <= this.i_value) // Si max value es menor que Value...
                    {                        
                        this.i_value = value; // Degradamos Value a MaxValue
                    }
                    this.i_MaxValue = value; // Establecemos el valor de MaxValue
                }
                this.UpdateShapePos();
                this.UpdateShape();
            }
            get { return this.i_MaxValue; }
        }

        public int MinValue
        {
            set 
            { 
                if (value >= this.i_MaxValue) // Si el nuevo MinValue es mayor que el MaxValue.
                {
                    this.i_MinValue = this.i_MaxValue-1; // Establecemos el MinValue por debajo del MaxValue,
                    this.i_value = this.i_MinValue; // Establecemos el Valor igual al MinValue.
                }
                else
                {
                    if (value >= this.i_value) // Si MinValue es mayor que Value...
                    {
                        this.i_value = value; // Incrementamos Value a MinValue.
                    }
                    this.i_MinValue = value; // Establecemos el valor de MinValue
                }
                this.UpdateShapePos();
                this.UpdateShape();
            }
            get { return this.i_MinValue; }
        }

        public int Value
        {
            set 
            { 
                if (this.i_MinValue > value)
                {
                    this.i_value = this.i_MinValue; // Mantenemos el valor siempre dentro del rango.
                }
                else if (value > this.i_MaxValue)
                {
                    this.i_value = this.i_MaxValue; // Mantenemos el valor siempre dentro del rango.
                }
                else
                {
                    this.i_value = value;
                }
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value));
                this.UpdateShapePos();
                this.UpdateShape();
            }
            get { return this.i_value; }
        }
        #endregion
    }

}