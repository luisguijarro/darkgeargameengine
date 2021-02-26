using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class Button : BaseObjects.Control
    {
        protected bool b_pulsed;
        public Button() : this(22,22)
        {
            
        }
        public Button(uint width, uint height) : base(width,height)
        {
            this.setPulsed(false);

            float multHor = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_width;
            float multVer = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_height;

            float f_hor = this.ui_width;
            float f_ver = this.ui_width;

            this.MarginsFromTheEdge = new Vector4(2,2,2,2);
            
            this.Texcoords = new float[]
            {
                multHor*2f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*2f, multVer*3f, multVer*21f, multVer*23f
            };      

            this.MouseUp += delegate { this.setPulsed(false); };
        }

        private void setPulsed(bool pulsed)
        {
            this.b_pulsed = pulsed;
            if (this.b_pulsed)
            {
                float multHor = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_width;
                this.tcDisplacement = new Vector2(multHor*24f, 0f);
            }
            else
            {
                this.tcDisplacement = new Vector2(0,0);
            }
        }

        protected override void MDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.setPulsed(true);
                base.MDown(sender, e);
            }
        }

        protected override void MUp(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MUp(sender, e);
            }
            this.setPulsed(false);
        }
        /*
        internal override void Draw(GuiDrawer drawer)
        {
            base.Draw(drawer);
        }
        */
    }
}