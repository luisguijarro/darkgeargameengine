using System;
using dgtk;
using dgtk.Graphics;

namespace dge.GUI
{    
    public class TrackBar : BaseObjects.Control
    {
        int lastX, lastY;
        private bool b_pulsed;
        private bool b_InvertRange;
        private int i_Slide_XPos;
        private int i_Slide_YPos;
        private int i_value;
        private int i_lastValue;
        private int i_MaxValue;
        private int i_MinValue;
        private Orientation o_Orientation; // indica si se visualiza Horizontal o Verticalmente,
        private SliderShape ss_SliderShape; // indica la forma del deslizador, Flecha o duadrado.
        private SliderOrientation so_SliderOrientation; // Indica hacia donde apunta el deslizador si es Flecha. 
        private float[] Slider_Texcoords;
        private int [] i_TrackBar_Slider_Size;

        public event EventHandler<IntValueChangedEventArgs> ValueChanged;

        public TrackBar() : base(66, 22)
        {
            this.o_Orientation = Orientation.Horizontal;
            this.ss_SliderShape = SliderShape.Square;
            this.so_SliderOrientation = SliderOrientation.UpRigth;
            this.b_InvertRange = false;
            this.i_MinValue = 0;
            this.i_MaxValue = 100;
            this.i_value = 0;
            this.ValueChanged += delegate { };

            this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
            this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
        }

        protected internal override void UpdateTheme()
        {

        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);
            if (Core2D.SelectedID == this.ui_id)
            {
                this.b_pulsed = true;
                this.lastX = e.X;
                this.lastY = e.Y;
            }
        }

        protected override void OnMUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMUp(sender, e);
            this.b_pulsed = false;
        }

        protected override void OnMMove(object sender, MouseMoveEventArgs e)
        {
            base.OnMMove(sender, e);
            if ((this.b_pulsed)) // && (Core2D.SelectedID == this.ui_id))
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (((this.i_Slide_XPos + e.X-lastX) >= 0) && ((this.i_Slide_XPos + e.X-lastX) <= this.i_width-this.i_TrackBar_Slider_Size[0]))
                    {
                        this.i_Slide_XPos += e.X-lastX;
                        int pixelrange = (int)this.i_width-this.i_TrackBar_Slider_Size[0];
                        int valuerange = this.i_MaxValue-this.i_MinValue;
                        float mult = (float)valuerange / (float)pixelrange;
                        if (!this.b_InvertRange)
                        {
                            this.i_value = (int)(this.i_Slide_XPos * mult);  
                        }
                        else
                        {
                            this.i_value = (int)((pixelrange-this.i_Slide_XPos) * mult);
                        }
                        Console.WriteLine("Valor Hor: "+this.i_value);
                    }
                }
                else
                {
                    if (((this.i_Slide_YPos + e.Y-lastY) >= 0) && ((this.i_Slide_YPos + e.Y-lastY) <= this.i_height-this.i_TrackBar_Slider_Size[1]))
                    {
                        this.i_Slide_YPos += e.Y-lastY;
                        int pixelrange = this.i_height-this.i_TrackBar_Slider_Size[1];
                        int valuerange = this.i_MaxValue-this.i_MinValue;
                        float mult = (float)valuerange / (float)pixelrange;
                        if (!this.b_InvertRange)
                        {
                            this.i_value = (int)((pixelrange-this.i_Slide_YPos) * mult);
                        }
                        else
                        {
                            this.i_value = (int)((this.i_Slide_YPos) * mult);
                        }
                        Console.WriteLine("Valor Ver: "+this.i_value);
                    }
                }
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, this.i_lastValue, false));
                this.i_lastValue = this.i_value;
                this.lastX = e.X;
                this.lastY = e.Y;
            }
        }

        #region PRIVATE METHODS:
        private void UpdateShapePos(GuiTheme theme)
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                int pixelrange = this.i_width-this.i_TrackBar_Slider_Size[0];
                int valuerange = this.i_MaxValue-this.i_MinValue;
                float mult = (float)pixelrange / (float)valuerange;
                if (!this.b_InvertRange)
                {
                    this.i_Slide_XPos = (int)(mult*this.i_value);
                }
                else
                {
                    this.i_Slide_XPos = pixelrange-((int)(mult*this.i_value));
                }
                this.i_Slide_YPos = theme.TrackBar_Slider_PosMargin; 
            }
            else
            {
                int pixelrange = this.i_height-this.i_TrackBar_Slider_Size[1];
                int valuerange = this.i_MaxValue-this.i_MinValue;
                float mult = (float)pixelrange / (float)valuerange;
                if (!this.b_InvertRange)
                {
                    this.i_Slide_YPos = pixelrange-(int)(mult*this.i_value);
                }
                else
                {
                    this.i_Slide_YPos = (int)(mult*this.i_value);
                }
                this.i_Slide_XPos = theme.TrackBar_Slider_PosMargin;
            }
        }

        private void UpdateShape(GuiTheme theme)
        {
            this.tcFrameOffset = theme.TrackBar_FrameOffset;
            if (this.Orientation == Orientation.Horizontal)
            {
                this.i_TrackBar_Slider_Size = new int[]{ theme.TrackBar_Slider_Size[0], theme.TrackBar_Slider_Size[1] };
                this.Texcoords = theme.TrackBar_Hor_Texcoords;
                this.MarginsFromTheEdge = theme.TrackBar_Hor_MarginsFromTheEdge;
                if (SliderShape == SliderShape.Square)
                {
                    this.Slider_Texcoords = theme.TrackBar_Square_Hor_Slider_Texcoords;
                }
                else
                {
                    if (SliderOrientation == SliderOrientation.UpRigth)
                    {
                        this.Slider_Texcoords = theme.TrackBar_Arrow_Up_Slider_Texcoords;
                    }
                    else
                    {
                        this.Slider_Texcoords = theme.TrackBar_Arrow_Down_Slider_Texcoords;
                    }
                }
            }
            else
            {
                this.i_TrackBar_Slider_Size = new int[]{ theme.TrackBar_Slider_Size[2], theme.TrackBar_Slider_Size[3] };
                this.Texcoords = theme.TrackBar_Ver_Texcoords;
                this.MarginsFromTheEdge = theme.TrackBar_Ver_MarginsFromTheEdge;
                if (SliderShape == SliderShape.Square)
                {
                    this.Slider_Texcoords = theme.TrackBar_Square_Ver_Slider_Texcoords;
                }
                else
                {
                    if (SliderOrientation == SliderOrientation.UpRigth)
                    {
                        this.Slider_Texcoords = theme.TrackBar_Arrow_Right_Slider_Texcoords;
                    }
                    else
                    {
                        this.Slider_Texcoords = theme.TrackBar_Arrow_Left_Slider_Texcoords;
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
                this.i_width = GuiTheme.DefaultGuiTheme.TrackBar_Ver_MaxWidth;
            }
            else
            {
                this.i_height = GuiTheme.DefaultGuiTheme.TrackBar_Hor_MaxHeight;
            }
            this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
            this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
        }

        protected override void pDraw()
        {
            base.pDraw();
            base.DrawIn(this.i_x, this.i_y, this.i_width, this.i_height, DrawSliderShape);
        }

        protected override void pDrawID()
        {
            dge.G2D.IDsDrawer.DrawGuiGL(this.gui.GuiTheme.ThemeSltTBO.ID, new Color4(0f,0f,0f,0f), this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 1); // Pintamos sin ID de la superficie.
        }


        protected override void pDrawContentID()
        {
            /*
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                this.DrawIdIn(this.i_x-this.MarginsFromTheEdge[0], this.i_y+this.MarginsFromTheEdge[1], this.i_width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]), this.i_height-this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3], DrawContentIDs);
            }
            */
            base.DrawIdIn(this.i_x, this.i_y, this.i_width, this.i_height, DrawSliderIDShape);
        }


        private void DrawSliderShape()
        {
            this.gui.Drawer.Draw(GuiTheme.DefaultGuiTheme.ThemeTBO.ID, this.i_Slide_XPos, this.i_Slide_YPos, 0, this.i_TrackBar_Slider_Size[0], this.i_TrackBar_Slider_Size[1], 0f, this.Slider_Texcoords[0], this.Slider_Texcoords[2], this.Slider_Texcoords[1], this.Slider_Texcoords[3]);
        }

        private void DrawSliderIDShape()
        {
            dge.G2D.IDsDrawer.DrawGL2D(GuiTheme.DefaultGuiTheme.ThemeSltTBO.ID, this.idColor,  this.i_Slide_XPos, this.i_Slide_YPos, this.i_TrackBar_Slider_Size[0], this.i_TrackBar_Slider_Size[1], 0f, this.Slider_Texcoords[0], this.Slider_Texcoords[2], this.Slider_Texcoords[1], this.Slider_Texcoords[3], 1);
        }

        #endregion

        #region PROPERTIES:
        public Orientation Orientation
        {
            set 
            { 
                if (this.o_Orientation != value)
                {
                    int temp = this.i_width;
                    this.i_width = this.i_height;
                    this.i_height = temp;
                }
                this.o_Orientation = value; 
                this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
                this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme); 
            }
            get { return this.o_Orientation; }
        }

        public SliderShape SliderShape
        {
            set { this.ss_SliderShape = value; this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme); this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);}
            get { return this.ss_SliderShape; }
        }

        public SliderOrientation SliderOrientation
        {
            set { this.so_SliderOrientation = value; this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme); this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);}
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
                this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
                this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
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
                this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
                this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
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
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, this.i_lastValue, true));
                this.i_lastValue = this.i_value;
                this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
                this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
            }
            get { return this.i_value; }
        }

        public int TrackBar_Slider_width
        {
            get { return this.i_TrackBar_Slider_Size[0]; }
        }

        public int TrackBar_Slider_height
        {
            get { return this.i_TrackBar_Slider_Size[1]; }
        }

        public bool InvertRange
        {
            set 
            { 
                this.b_InvertRange = value;
                this.UpdateShapePos(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
                this.UpdateShape(this.gui != null? this.gui.gt_ActualGuiTheme : GuiTheme.DefaultGuiTheme);
            }
            get { return this.b_InvertRange; }
        }

        #endregion
    }

}