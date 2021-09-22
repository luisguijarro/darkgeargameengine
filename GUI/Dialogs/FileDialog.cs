using System;
using System.IO;
using System.Collections.Generic;

namespace dge.GUI
{
    public class FileDialog : Dialog
    {
        private readonly TextBox textBox1; //Ruta completa.
        private readonly ListViewer flv; // Lista de ficheros y carpetas
        private readonly Button b_ok; // Botón de OK
        private readonly Button b_cancel; // Botón de Cancelar
        private string s_Path;
        private string s_FullName;
        private string s_filter;
        
        public FileDialog(GraphicsUserInterface gui) : base(gui)
        {
            this.Text = "FileDialog";
            this.s_Path = "./";
            this.s_filter = "*.*";

            this.textBox1 = new TextBox(this.Width-10, 22, "/absolute path....");
            this.textBox1.X = 5;
            this.textBox1.Y = 5;
            this.textBox1.Editable = false;
            this.AddSurface(this.textBox1);

            this.flv = new ListViewer();
            this.flv.X = 5;
            this.flv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.flv.DoubleClick += this.ListDoubleClick;
            this.flv.MouseDown += this.ListMouseDown;
            this.flv.ItemSelected += this.ListItemSelected;
            this.AddSurface(this.flv);

            this.b_ok = new Button(102, 22);

            this.b_ok.Text = "OK";
            this.b_ok.MouseUp += OkPulsed;
            this.AddSurface(this.b_ok);

            this.b_cancel = new Button(102, 22);
            this.b_cancel.Text = "CANCEL";
            this.b_cancel.MouseUp += CancelPulsed;
            this.AddSurface(this.b_cancel);

            this.Width = 450;
            this.Height = 254;
            this.OnResize();

            this.updateFolder();
        }

        private void updateFolder()
        {
            int selectedindex = 0;
            this.s_FullName = this.s_Path;
            DirectoryInfo di = new DirectoryInfo(this.s_Path);
            List<DirectoryInfo> folders = new List<DirectoryInfo>(di.GetDirectories());
            // folders.Sort();
            this.flv.ClearList();
            if (di.Parent != null)
            {
                Dialogs.ParentDirectoryInfo pdi = new Dialogs.ParentDirectoryInfo(di.Parent);
                this.flv.AddObject(pdi);
                selectedindex++;
            }
            
            this.flv.AddObjects(folders.ToArray());
            List<FileInfo> files = new List<FileInfo>(di.GetFiles(this.s_filter));
            //files.Sort();
            this.flv.AddObjects(files.ToArray());
            this.flv.SelectItem(selectedindex);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.b_ok.MouseUp -= OkPulsed;
            }
            base.Dispose(disposing);
        }

        public void OkPulsed(object sender, MouseButtonEventArgs e)
        {
            this.s_FullName = this.s_Path;
            this.SetResult(DialogResult.OK);
        }

        public void CancelPulsed(object sender, MouseButtonEventArgs e)
        {
            this.SetResult(DialogResult.Cancel);
        }

        private void ListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dge.Core2D.SelectedID == ((ListViewer)sender).ID)
            {
                if (this.flv.SelectedItem != null)
                {
                    if ((this.flv.SelectedItem.GetType() == typeof(DirectoryInfo)) ||  (this.flv.SelectedItem.GetType() == typeof(Dialogs.ParentDirectoryInfo)))
                    {
                        this.updateFolder();
                    }
                }
            }
        }

        private void ListMouseDown(object sender, MouseButtonEventArgs e)
        {
            /*if(dge.Core2D.SelectedID == ((ListViewer)sender).ID)
            {
                if (((ListViewer)sender).SelectedItem != null)
                {
                    if (((ListViewer)sender).SelectedItem.GetType() == typeof(Dialogs.ParentDirectoryInfo))
                    {
                        this.s_Path = ((Dialogs.ParentDirectoryInfo)((ListViewer)sender).SelectedItem).FullName;
                    }
                    else if (((ListViewer)sender).SelectedItem.GetType() == typeof(DirectoryInfo))
                    {
                        this.s_Path = ((DirectoryInfo)((ListViewer)sender).SelectedItem).FullName;
                    }
                    else
                    {
                        this.s_FullName = ((FileInfo)((ListViewer)sender).SelectedItem).FullName;
                    }
                }
            }*/
        }

        private void ListItemSelected(object sender, EventArgs e)
        {
            if (((ListViewer)sender).SelectedItem.GetType() == typeof(Dialogs.ParentDirectoryInfo))
            {
                this.s_Path = ((Dialogs.ParentDirectoryInfo)((ListViewer)sender).SelectedItem).FullName;
            }
            else if (((ListViewer)sender).SelectedItem.GetType() == typeof(DirectoryInfo))
            {
                this.s_Path = ((DirectoryInfo)((ListViewer)sender).SelectedItem).FullName;
            }
            else
            {
                this.s_Path = ((FileInfo)((ListViewer)sender).SelectedItem).FullName;
            }
        }

        protected override void OnResize()
        {
            base.OnResize();

            this.textBox1.X = 5;
            this.textBox1.Y = 5;
            this.textBox1.Width = this.InnerSize.Width - 10;

            this.flv.X = 5;
            this.flv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.flv.Width = this.InnerSize.Width - 10;
            this.flv.Height = this.InnerSize.Height - (40 + this.b_ok.Height);

            this.b_cancel.X = this.InnerSize.Width-(27 + this.b_cancel.Width);
            this.b_cancel.Y = (int)(this.flv.Y + this.flv.Height) + 5;
            this.b_ok.X = this.InnerSize.Width-(27 + this.b_cancel.Width + 10 + this.b_ok.Width);
            this.b_ok.Y = (int)(this.flv.Y + this.flv.Height) + 5;            
        }

        public string Filter 
        {
            set { this.s_filter = value; this.updateFolder(); }
            get { return this.s_filter; }
        }

        public string FullPath
        {
            get { return this.s_FullName; }
        }
    }
}