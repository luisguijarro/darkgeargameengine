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
        private Button btn1;
        private Button btn2;
        private Button slider;
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

            this.UpdateOrientation();
            this.UpdateSizePos();
            this.UpdateSliderPos();
            this.UpdateStepSize();
        }

        #region SubEvents:

        private void Btn1Down(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (Core2D.SelectedID == this.btn1.ID)
            {
                this.Value -= this.i_step;
            }
        }

        private void Btn2Down(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (Core2D.SelectedID == this.btn2.ID)
            {
                this.Value += this.i_step;
            }
        }

        private void SliderDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (Core2D.SelectedID == this.slider.ID)
            {
                this.lastPosX = e.X;
                this.lastPosY = e.Y;
            }
        }
        private void SliderMove(object sender, dgtk_MouseMoveEventArgs e)
        {
            if ((this.slider.b_pulsed) && (Core2D.SelectedID == this.slider.ID))
            {
                if (this.o_Orientation == Orientation.Vertical)
                {
                    int i_diference = e.Y - this.lastPosY;
                    if ((this.slider.Y + i_diference) >= this.btn1.Height)
                    {
                        if ((this.slider.Y + i_diference) <= this.btn2.Y-this.slider.Height)
                        {
                            this.slider.Y += i_diference;
                            int pixelrange = (int)(this.Height-(this.btn1.Height+this.btn2.Height+this.slider.Height));
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
                            int pixelrange = (int)(this.Width-(this.btn1.Width+this.btn2.Width+this.slider.Width));
                            int valuerange = this.i_MaxValue-this.i_MinValue;
                            float mult = (float)valuerange / (float)pixelrange;
                            this.i_value = (int)((this.slider.X-this.btn1.Width) * mult);
                        }
                    }
                }
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value));
                this.lastPosX = e.X;
                this.lastPosY = e.Y;
            }
        }

        #endregion

        #region PRIVATE METHODS:

        private void UpdateOrientation()
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Track_MarginsFromTheEdge;
                this.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Track_Texcoords;
                this.tcFrameOffset = new float[] {0,0};

                this.btn1.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Btn1_MarginsFromTheEdge;
                this.btn1.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Btn1_Texcoords;
                this.btn1.tcFrameOffset = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Btn1_FrameOffset;

                this.slider.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Slider_MarginsFromTheEdge;
                this.slider.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Slider_Texcoords;
                this.slider.tcFrameOffset = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Slider_FrameOffset;

                this.btn2.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Btn2_MarginsFromTheEdge;
                this.btn2.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Btn2_Texcoords;
                this.btn2.tcFrameOffset = GuiTheme.DefaultGuiTheme.ScrollBar_Hor_Btn2_FrameOffset;
            }
            else
            {
                this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Track_MarginsFromTheEdge;
                this.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Track_Texcoords;
                this.tcFrameOffset = new float[] {0,0};

                this.btn1.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Btn1_MarginsFromTheEdge;
                this.btn1.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Btn1_Texcoords;
                this.btn1.tcFrameOffset = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Btn1_FrameOffset;

                this.slider.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Slider_MarginsFromTheEdge;
                this.slider.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Slider_Texcoords;
                this.slider.tcFrameOffset = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Slider_FrameOffset;

                this.btn2.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Btn2_MarginsFromTheEdge;
                this.btn2.Texcoords = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Btn2_Texcoords;
                this.btn2.tcFrameOffset = GuiTheme.DefaultGuiTheme.ScrollBar_Ver_Btn2_FrameOffset;
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
                pixelrange = (int)(this.Height-(this.btn1.Height+this.btn2.Height+this.slider.Height));
                this.i_step = (int)((float)valuerange / (float)pixelrange);
            }
            else
            {
                pixelrange = (int)(this.Width-(this.btn1.Width+this.btn2.Width+this.slider.Width));
                this.i_step = (int)((float)valuerange / (float)pixelrange);
            }
        }

        #endregion

        #region VIRTUAL/OVERRIDE

        protected override void OnResize()
        {
            base.OnResize();
            if (this.o_Orientation == Orientation.Horizontal)
            {
                base.Height = (uint)GuiTheme.DefaultGuiTheme.ScrollBar_BarWidth;
            }
            else
            {
                base.Width = (uint)GuiTheme.DefaultGuiTheme.ScrollBar_BarWidth;
            }
            this.UpdateSizePos();
            this.UpdateSliderPos();
            this.UpdateStepSize();
        }

        protected override void OnReposition()
        {
            base.OnReposition();
            this.UpdateSizePos();
            this.UpdateSliderPos();
        }

        internal override void Draw()
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x+(int)this.btn1.Height, this.i_y, this.ui_width-(uint)(this.btn1.Width+this.btn2.Width), this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }
            else
            {
                this.gui.GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y+(int)this.btn1.Height, this.ui_width, this.ui_height-(uint)(this.btn1.Height+this.btn2.Height), 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }

            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                DrawIn(this.i_x, this.i_y, (int)this.ui_width, (int)this.ui_height, DrawContent);
            }

        }

        internal override void DrawID()
        {
            if (this.contentUpdate && VisibleSurfaceOrder.Count>0) 
            {
                this.DrawIdIn(this.i_x, this.i_y, (int)this.ui_width, (int)this.ui_height, DrawContentIDs);
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
                    uint temp = this.ui_width;
                    this.ui_width = this.ui_height;
                    this.ui_height = temp;
                    this.o_Orientation = value; 
                    this.UpdateOrientation();
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
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value));
                this.UpdateSizePos();
                this.UpdateSliderPos();
            }
            get { return this.i_value; }
        }

        #endregion
    }
}