using System;
using dgtk;

namespace dge.GUI
{
    public class Button : BaseObjects.Control
    {
        protected bool pulsed;
        public Button() : base(22,22)
        {
            this.pulsed = false;

            float multHor = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_width;
            float multVer = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_height;

            this.f_Texcoord0x = multHor*1f;
            this.f_Texcoord0y = multVer*1f;
            this.f_Texcoord1x = multHor*23f;
            this.f_Texcoord1x = multVer*23f;

            //Esto debe ir en el Evento principal.
            /*this.MouseDown += delegate 
            { 
                if (dge.Core2D.SelectedID == this.ui_id) 
                {
                    this.MouseDown(this, new bla_bla_bla_args e); = true; 
                }
            };*/
            this.MouseUp += delegate { this.pulsed = false; };
        }

        protected override void MDown(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.pulsed = true;
                base.MDown(sender, e);
            }
        }

        protected override void MUp(object sender, dgtk_MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                base.MUp(sender, e);
            }
            this.pulsed = false;
        }

        internal override void Draw(G2D.Drawer drawer)
        {
            float multHor = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_width;
            float multVer = 1f/(float)GraphicsUserInterface.DefaultThemeTBO.ui_height;
            if (pulsed)
            {
                base.Draw(drawer, multHor*25f, multVer*1f, multHor*47f, multVer*23f);
            }
            else
            {
                base.Draw(drawer, multHor*1f, multVer*1f, multHor*23f, multVer*23f);
            }
        }
    }
}