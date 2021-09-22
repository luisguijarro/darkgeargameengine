using System;
using dgtk;
using dgtk.Graphics;

namespace dge.GUI
{    
    public class ScrollBar : BaseObjects.Control
    {
        private int lastPosX;
        private int lastPosY;
        //private bool b_SliderPulsed;
        private int i_value;
        private int i_MaxValue;
        private int i_MinValue;
        private int i_step;
        private Orientation o_Orientation;
        private readonly Button btn1;
        private readonly Button btn2;
        private readonly Button slider;
        public event EventHandler<IntValueChangedEventArgs> ValueChanged;
        public ScrollBar() : base(22, 150)
        {
            this.btn1 = new Button();
            this.btn1.Text = "";
            this.AddSubControl(this.btn1);
            this.btn2 = new Button();
            this.btn2.Text = "";
            this.AddSubControl(this.btn2);
            this.slider = new Button();
            this.slider.Text = "";
            this.AddSubControl(this.slider);
            this.o_Orientation = Orientation.Vertical;

            this.i_MinValue = 0;
            this.i_MaxValue = 100;
            this.i_value = 0;

            this.ValueChanged+=delegate{};
            this.btn1.MouseDown += Btn1Down;
            this.btn2.MouseDown += Btn2Down;
            this.slider.MouseDown += SliderDown;
            this.slider.MouseMove += SliderMove;

            this.UpdateOrientation(GuiTheme.DefaultGuiTheme);
            this.UpdateSizePos();
            this.UpdateSliderPos();
            this.UpdateStepSize();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.btn1.MouseDown -= Btn1Down;
                this.btn2.MouseDown -= Btn2Down;
                this.slider.MouseDown -= SliderDown;
                this.slider.MouseMove -= SliderMove;
                this.btn1.Dispose();
                this.btn2.Dispose();
                this.slider.Dispose();
            }
        }

        protected internal override void UpdateTheme()
        {
            this.btn1.Width = this.gui.gt_ActualGuiTheme.ScrollBar_BarWidth;
            this.btn1.Height = this.gui.gt_ActualGuiTheme.ScrollBar_BarWidth;
            this.btn2.Width = this.gui.gt_ActualGuiTheme.ScrollBar_BarWidth;
            this.btn2.Height = this.gui.gt_ActualGuiTheme.ScrollBar_BarWidth;
            this.slider.Width = this.gui.gt_ActualGuiTheme.ScrollBar_BarWidth;
            this.slider.Height = this.gui.gt_ActualGuiTheme.ScrollBar_BarWidth;
            this.UpdateOrientation(this.gui.gt_ActualGuiTheme);
        }

        #region SubEvents:

        private void Btn1Down(object sender, MouseButtonEventArgs e)
        {
            if (Core2D.SelectedID == this.btn1.ID)
            {
                this.Value -= this.i_step;
            }
        }

        private void Btn2Down(object sender, MouseButtonEventArgs e)
        {
            if (Core2D.SelectedID == this.btn2.ID)
            {
                this.Value += this.i_step;
            }
        }

        private void SliderDown(object sender, MouseButtonEventArgs e)
        {
            if (Core2D.SelectedID == this.slider.ID)
            {
                this.lastPosX = e.X;
                this.lastPosY = e.Y;
            }
        }
        private void SliderMove(object sender, MouseMoveEventArgs e)
        {
            if (this.slider.b_pulsed) // && (Core2D.SelectedID == this.slider.ID))
            {
                if (this.o_Orientation == Orientation.Vertical)
                {
                    int i_diference = e.Y - this.lastPosY;
                    if ((this.slider.Y + i_diference) >= this.btn1.Height)
                    {
                        if ((this.slider.Y + i_diference) <= this.btn2.Y-this.slider.Height)
                        {
                            this.slider.Y += i_diference;
                            int pixelrange = this.Height-(this.btn1.Height+this.btn2.Height+this.slider.Height);
                            int valuerange = this.i_MaxValue-this.i_MinValue;
                            float mult = (float)valuerange / (float)pixelrange;
                            this.i_value = (int)((this.slider.Y-this.btn1.Height) * mult);
                        }
                    }                    
                }
                else
                {
                    int i_diference = e.X - this.lastPosX;
                    if ((this.slider.X + i_diference) >= this.btn1.Width)
                    {
                        if ((this.slider.X + i_diference) <= this.btn2.X-this.slider.Width)
                        {
                            this.slider.X += i_diference;
                            int pixelrange = this.Width-(this.btn1.Width+this.btn2.Width+this.slider.Width);
                            int valuerange = this.i_MaxValue-this.i_MinValue;
                            float mult = (float)valuerange / (float)pixelrange;
                            this.i_value = (int)((this.slider.X-this.btn1.Width) * mult);
                        }
                    }
                }
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, false));
                this.lastPosX = e.X;
                this.lastPosY = e.Y;
            }
        }

        #endregion

        #region PRIVATE METHODS:

        private void UpdateOrientation(GuiTheme theme)
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                this.MarginsFromTheEdge = theme.ScrollBar_Hor_Track_MarginsFromTheEdge;
                this.Texcoords = theme.ScrollBar_Hor_Track_Texcoords;
                this.tcFrameOffset = new float[] {0,0};

                this.btn1.MarginsFromTheEdge = theme.ScrollBar_Hor_Btn1_MarginsFromTheEdge;
                this.btn1.Texcoords = theme.ScrollBar_Hor_Btn1_Texcoords;
                this.btn1.tcFrameOffset = theme.ScrollBar_Hor_Btn1_FrameOffset;

                this.slider.MarginsFromTheEdge = theme.ScrollBar_Hor_Slider_MarginsFromTheEdge;
                this.slider.Texcoords = theme.ScrollBar_Hor_Slider_Texcoords;
                this.slider.tcFrameOffset = theme.ScrollBar_Hor_Slider_FrameOffset;

                this.btn2.MarginsFromTheEdge = theme.ScrollBar_Hor_Btn2_MarginsFromTheEdge;
                this.btn2.Texcoords = theme.ScrollBar_Hor_Btn2_Texcoords;
                this.btn2.tcFrameOffset = theme.ScrollBar_Hor_Btn2_FrameOffset;
            }
            else
            {
                this.MarginsFromTheEdge = theme.ScrollBar_Ver_Track_MarginsFromTheEdge;
                this.Texcoords = theme.ScrollBar_Ver_Track_Texcoords;
                this.tcFrameOffset = new float[] {0,0};

                this.btn1.MarginsFromTheEdge = theme.ScrollBar_Ver_Btn1_MarginsFromTheEdge;
                this.btn1.Texcoords = theme.ScrollBar_Ver_Btn1_Texcoords;
                this.btn1.tcFrameOffset = theme.ScrollBar_Ver_Btn1_FrameOffset;

                this.slider.MarginsFromTheEdge = theme.ScrollBar_Ver_Slider_MarginsFromTheEdge;
                this.slider.Texcoords = theme.ScrollBar_Ver_Slider_Texcoords;
                this.slider.tcFrameOffset = theme.ScrollBar_Ver_Slider_FrameOffset;

                this.btn2.MarginsFromTheEdge = theme.ScrollBar_Ver_Btn2_MarginsFromTheEdge;
                this.btn2.Texcoords = theme.ScrollBar_Ver_Btn2_Texcoords;
                this.btn2.tcFrameOffset = theme.ScrollBar_Ver_Btn2_FrameOffset;
            }
        }

        private void UpdateSizePos()
        {
            if (this.o_Orientation == Orientation.Vertical)
            {
                this.btn1.X = 0;
                this.btn1.Y = 0;
                this.btn2.X = 0;
                this.btn2.Y = (int)(this.Height-this.btn2.Height);
            }
            else
            {
                this.btn1.X = 0;
                this.btn1.Y = 0;
                this.btn2.X = (int)(this.Width - this.btn2.Width);
                this.btn2.Y = 0;
            }
        }
    
        private void UpdateSliderPos()
        {
            int pixelrange = 0;
            int valuerange = this.i_MaxValue - this.i_MinValue;
            if (this.o_Orientation == Orientation.Vertical)
            {
                this.slider.X = 0;
                pixelrange = (int)(this.Height-(this.btn1.Height+this.btn2.Height+this.slider.Height));
                float mult = (float)pixelrange / (float)valuerange;
                this.slider.Y = (int)this.btn1.Height + (int)(mult * this.i_value);
            }
            else
            {
                this.slider.Y = 0;
                pixelrange = (int)(this.Width-(this.btn1.Width+this.btn2.Width+this.slider.Width));
                float mult = (float)pixelrange / (float)valuerange;
                this.slider.X = (int)this.btn1.Width + (int)(mult * this.i_value);
            }
        }

        private void UpdateStepSize()
        {
            int pixelrange = 0;
            int valuerange = this.i_MaxValue - this.i_MinValue;
            if (this.o_Orientation == Orientation.Vertical)
            {
                pixelrange = this.Height-(this.btn1.Height+this.btn2.Height+this.slider.Height);
                this.i_step = (int)((float)valuerange / (float)pixelrange);
            }
            else
            {
                pixelrange = this.Width-(this.btn1.Width+this.btn2.Width+this.slider.Width);
                this.i_step = (int)((float)valuerange / (float)pixelrange);
            }
            if (this.i_step < 1) { this.i_step = 1; }
        }

        #endregion

        #region VIRTUAL/OVERRIDE

        protected override void OnResize()
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                this.i_height = GuiTheme.DefaultGuiTheme.ScrollBar_BarWidth;
            }
            else
            {
                this.i_width = GuiTheme.DefaultGuiTheme.ScrollBar_BarWidth;
            }
            this.SetInternalDrawArea(this.i_x, this.i_y, this.i_width, this.i_height);
            this.UpdateSizePos();
            this.UpdateSliderPos();
            this.UpdateStepSize();
        }

        protected override void OnReposition()
        {
            this.SetInternalDrawArea(this.i_x, this.i_y, this.i_width, this.i_height);
            this.UpdateSizePos();
            this.UpdateSliderPos();
        }

        protected override void pDraw()
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x+this.btn1.Height, this.i_y, this.i_width-(this.btn1.Width+this.btn2.Width), this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }
            else
            {
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y+this.btn1.Height, this.i_width, this.i_height-(int)(this.btn1.Height+this.btn2.Height), 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }

            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                DrawIn(this.i_x, this.i_y, this.i_width, this.i_height, DrawContent);
            }
        }

        protected override void pDrawID()
        {
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                this.DrawIdIn(this.i_x, this.i_y, this.i_width, this.i_height, DrawContentIDs);
            }
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
                    this.o_Orientation = value; 
                    this.UpdateOrientation(this.gui != null ? this.gui.gt_ActualGuiTheme : dge.GUI.GuiTheme.DefaultGuiTheme);
                    this.UpdateSizePos();
                    this.UpdateSliderPos();
                    this.UpdateStepSize();
                }
            }
            get { return this.o_Orientation; }
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
                this.UpdateSizePos();
                this.UpdateSliderPos();
                this.UpdateStepSize();
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
                this.UpdateSizePos();
                this.UpdateSliderPos();
                this.UpdateStepSize();
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
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, true));
                this.UpdateSizePos();
                this.UpdateSliderPos();
            }
            get { return this.i_value; }
        }

        public int StepSize
        {
            get { return this.i_step; }
        }

        #endregion
    }
}