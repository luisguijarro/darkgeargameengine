using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{    
    public class ProgressBar : BaseObjects.Control
    {
        private Orientation o_Orientation;
        private int i_value;
        private int i_MaxValue;
        private int i_MinValue;
        private float[] FillingTexCoords;
        private int ProgressWidthHeight;
        public event EventHandler<IntValueChangedEventArgs> ValueChanged;
        public ProgressBar() : this (100,22)
        {

        }
        public ProgressBar(int width, int height) : base(width, height)
        {
            this.o_Orientation = Orientation.Horizontal;
            this.Texcoords = GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Texcoords;
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ProgressBar_Hor_MarginsFromTheEdge;
            this.FillingTexCoords = GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords;
            this.tcFrameOffset = new float[] {0,0};
            this.ProgressWidthHeight = 0;

            this.i_MinValue = 0;
            this.i_MaxValue = 100;
            this.i_value = 0;
            this.ValueChanged += delegate { };
        }

        protected internal override void UpdateTheme()
        {
            this.Texcoords = this.gui.gt_ActualGuiTheme.ProgressBar_Hor_Texcoords;
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.ProgressBar_Hor_MarginsFromTheEdge;
            this.FillingTexCoords = this.gui.gt_ActualGuiTheme.ProgressBar_Hor_Filling_Texcoords;
        }

        private void UpdateProgres()
        {
            int valuerange = this.MaxValue - this.MinValue;
            if (this.o_Orientation == Orientation.Horizontal)
            {
                int pixelrange = this.Width;
                float mult = (float)pixelrange/(float)valuerange;
                this.ProgressWidthHeight = (int)(mult*this.i_value);
                this.Texcoords = GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Texcoords;
                float ValueRangeOfTexCoords = GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords[2] - GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords[0];
                this.FillingTexCoords = new float[]
                {
                    GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords[0],
                    GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords[1],
                    GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords[0] + (ValueRangeOfTexCoords/this.MaxValue) * this.i_value,
                    GuiTheme.DefaultGuiTheme.ProgressBar_Hor_Filling_Texcoords[3]
                };
            }
            else
            {
                int pixelrange = this.Height;
                float mult = (float)pixelrange/(float)valuerange;
                this.ProgressWidthHeight = (int)(mult*this.i_value);
                this.Texcoords = GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Texcoords;
                float ValueRangeOfTexCoords = GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Filling_Texcoords[3] - GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Filling_Texcoords[1];
                this.FillingTexCoords = new float[]
                {
                    GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Filling_Texcoords[0],
                    GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Filling_Texcoords[1] + (ValueRangeOfTexCoords/this.MaxValue) * (this.MaxValue-this.i_value),
                    GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Filling_Texcoords[2],
                    GuiTheme.DefaultGuiTheme.ProgressBar_Ver_Filling_Texcoords[3]
                };
            }
        }

        protected override void pDraw()
        {
            if (this.o_Orientation == Orientation.Horizontal)
            {
                this.gui.Drawer.Draw(GuiTheme.DefaultGuiTheme.ThemeTBO.ui_ID, this.X, this.Y, this.ProgressWidthHeight, this.i_height, 0f, this.FillingTexCoords[0], this.FillingTexCoords[1], this.FillingTexCoords[2], this.FillingTexCoords[3]);
            }
            else
            {
                this.gui.Drawer.Draw(GuiTheme.DefaultGuiTheme.ThemeTBO.ui_ID, this.X, this.Y + (this.i_height-this.ProgressWidthHeight), this.i_width, this.ProgressWidthHeight, 0f, this.FillingTexCoords[0], this.FillingTexCoords[1], this.FillingTexCoords[2], this.FillingTexCoords[3]);
            }
            base.pDraw();
        }

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
                this.UpdateProgres();
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
                this.UpdateProgres();
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
                this.UpdateProgres();
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
                this.UpdateProgres();
                this.ValueChanged(this, new IntValueChangedEventArgs(this.i_value, true));
            }
            get { return this.i_value; }
        }

        #endregion
    }
}