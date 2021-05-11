using System;
using System.IO;

namespace dge.GUI
{
    public class FileDialog : Window
    {
        private ListViewer flv;
        private Button b_ok;
        private Button b_cancel;
        
        public FileDialog()
        {
            this.Visible = false;
            this.Title = "FileDialog";

            this.flv = new ListViewer();
            this.flv.X = 10;
            this.flv.Y = 10;
            this.AddControl(this.flv);

            this.b_ok = new Button();
            this.b_ok.Text = "OK";
            this.AddControl(this.b_ok);

            this.b_cancel = new Button();
            this.b_cancel.Text = "CANCEL";
            this.AddControl(this.b_cancel);

            this.OnResize();
        }

        public void ShowDialog(BaseObjects.Control launcher)
        {
            //¿Cómo coño implemento esto?
            this.gui = launcher.GUI;
            this.gui.AddWindow(this);
            this.Visible = true;
        }

        protected override void OnResize()
        {
            base.OnResize();
            this.flv.Width = (uint)(this.ui_width-(this.MarginsFromTheEdge[0]+this.MarginsFromTheEdge[2]+20));
            this.flv.Height = (uint)(this.ui_height-(this.MarginsFromTheEdge[1]+this.MarginsFromTheEdge[3] + 30 + this.b_ok.Height));
            this.b_cancel.X = (int)(this.ui_width-(10+this.MarginsFromTheEdge[1] + this.b_cancel.Width));
            this.b_cancel.Y = (int)(this.ui_height-(10+this.MarginsFromTheEdge[3] + this.b_cancel.Height));
            this.b_ok.X = (int)(this.ui_width-(10+this.MarginsFromTheEdge[1] + this.b_cancel.Width + 10 + this.b_ok.Width));
            this.b_ok.Y = (int)(this.ui_height-(10+this.MarginsFromTheEdge[3] + this.b_ok.Height));
        }
    }
}