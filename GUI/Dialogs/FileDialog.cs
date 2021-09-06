using System;
using System.IO;

namespace dge.GUI
{
    public class FileDialog : Window
    {
        private readonly TextBox textBox1; //Ruta completa.
        private readonly ListViewer flv; // Lista de ficheros y carpetas
        private readonly Button b_ok; // Botón de OK
        private readonly Button b_cancel; // Botón de Cancelar
        
        public FileDialog()
        {
            this.Visible = false;
            this.Title = "FileDialog";

            this.textBox1 = new TextBox(this.Width-10, 22, "/absolute path....");
            this.textBox1.X = 5;
            this.textBox1.Y = 5;
            this.textBox1.Editable = false;
            this.AddControl(this.textBox1);

            this.flv = new ListViewer();
            this.flv.X = 5;
            this.flv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.AddControl(this.flv);

            this.b_ok = new Button(102, 22);

            this.b_ok.Text = "OK";
            this.AddControl(this.b_ok);

            this.b_cancel = new Button(102, 22);
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

            this.textBox1.X = 5;
            this.textBox1.Y = 5;
            this.textBox1.Width = (int)(this.Width - (10 + this.MarginLeft + this.MarginRight));

            this.flv.X = 5;
            this.flv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.flv.Width = (int)(this.i_width - (this.MarginLeft + this.MarginRight + 10));
            this.flv.Height = (int)(this.i_height - (this.MarginTop+this.MarginBottom + 40 + this.b_ok.Height));

            this.b_cancel.X = (int)(this.i_width-(10+this.MarginsFromTheEdge[1] + this.b_cancel.Width));
            this.b_cancel.Y = (int)(this.flv.Y + this.flv.Height) + 5;
            this.b_ok.X = (int)(this.i_width-(10+this.MarginsFromTheEdge[1] + this.b_cancel.Width + 10 + this.b_ok.Width));
            this.b_ok.Y = (int)(this.flv.Y + this.flv.Height) + 5;
            
        }
    }
}