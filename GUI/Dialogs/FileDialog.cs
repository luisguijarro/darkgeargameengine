using System;
using System.IO;
using System.Collections.Generic;

namespace dge.GUI
{
    public class FileDialog : Dialog
    {
        private readonly TextBox textBox1; //Ruta completa.
        private readonly TreeViewer ftv;
        private readonly ListViewer flv; // Lista de ficheros y carpetas
        private readonly Button b_ok; // Botón de OK
        private readonly Button b_cancel; // Botón de Cancelar
        private readonly TextBox textBox2; //Filename.
        private bool b_showHidden;
        private string s_Path;
        private string s_RootFolder;
        //private string s_FullName;
        private string s_filter;
        private List<string> CarpetasRevisadas;
        private bool b_InSaveMode;
        
        public FileDialog(GraphicsUserInterface gui) : base(gui)
        {
            this.Text = "FileDialog";
            this.s_Path = "./";
            this.s_filter = "*.*";
            this.s_RootFolder = "./";

            this.textBox1 = new TextBox(this.Width-10, 22, "/absolute path....");
            this.textBox1.X = 5;
            this.textBox1.Y = 5;
            this.textBox1.Editable = false;
            this.AddSurface(this.textBox1);

            this.ftv = new TreeViewer();
            this.ftv.X = 5;
            this.ftv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.ftv.Width = 150;
            this.ftv.Height = this.InnerSize.Height - (int)(this.textBox1.Y + this.textBox1.Height+15);
            this.ftv.AsociateObjectsType = typeof(DirectoryInfo);
            this.ftv.FieldToShow = "Name";
            this.ftv.ElementSelected += TreeElementSelected;
            this.AddSurface(this.ftv);

            this.flv = new ListViewer();
            this.flv.X = 10 + this.ftv.Width;
            this.flv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.flv.Width = this.InnerSize.Width - (15+this.ftv.Width);
            this.flv.DoubleClick += this.ListDoubleClick;
            this.flv.MouseDown += this.ListMouseDown;
            this.flv.ItemSelected += this.ListItemSelected;
            this.flv.SetHeaders(new string[]{"Name", "Name", "Last Change", "LastWriteTimeUtc"});
            this.flv.SetHeaderWidth("Name", 217); // No se aplica, INVESTIGAR CAUSA.
            this.flv.ObjectType = typeof(System.IO.FileSystemInfo);
            this.AddSurface(this.flv);
            this.flv.SetHeaderWidth("Name", 217);

            this.textBox2 = new TextBox(this.flv.Width, 22, "");
            this.textBox2.X = this.flv.X;
            this.textBox2.Y = this.flv.Y + this.flv.Height + 5;
            this.textBox2.Width = this.flv.Width;
            this.AddSurface(this.textBox2);

            this.b_ok = new Button(102, 22);

            this.b_ok.Text = "OK";
            this.b_ok.MouseUp += OkPulsed;
            this.AddSurface(this.b_ok);

            this.b_cancel = new Button(102, 22);
            this.b_cancel.Text = "CANCEL";
            this.b_cancel.MouseUp += CancelPulsed;
            this.AddSurface(this.b_cancel);

            this.Width = 507;
            this.Height = 284;
            this.OnResize();

            this.CarpetasRevisadas = new List<string>();
            this.b_showHidden = false;

            this.updateFolder();
            this.UpdateTreeViewer();
        }

        protected override void OnResize()
        {
            base.OnResize();

            this.textBox1.X = 5;
            this.textBox1.Y = 5;
            this.textBox1.Width = this.InnerSize.Width - 10;

            this.ftv.X = 5;
            this.ftv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.ftv.Width = 150;
            this.ftv.Height = this.InnerSize.Height - (int)(this.textBox1.Y + this.textBox1.Height+10);

            this.flv.X = 10 + this.ftv.Width;
            this.flv.Y = (int)(this.textBox1.Y + this.textBox1.Height+5);
            this.flv.Width = this.InnerSize.Width - (15+this.ftv.Width);
            this.flv.Height = this.InnerSize.Height - (40 + this.b_ok.Height + 30);

            this.textBox2.X = this.flv.X;
            this.textBox2.Y = this.flv.Y + this.flv.Height + 5;
            this.textBox2.Width = this.flv.Width;

            this.b_cancel.X = this.InnerSize.Width-(5 + this.b_cancel.Width);
            this.b_cancel.Y = this.InnerSize.Height - (this.b_cancel.Height + 5); // (int)(this.flv.Y + this.flv.Height) + 5;
            this.b_ok.X = this.InnerSize.Width-(this.b_cancel.Width + 10 + this.b_ok.Width);
            this.b_ok.Y = this.InnerSize.Height - (this.b_ok.Height + 5); //(int)(this.flv.Y + this.flv.Height) + 5;    

                    
        }

        public virtual void ShowDialog(bool save)
        {            
            if (this.gui.ActiveDialog==null)
            {
                this.updateFolder();
                this.textBox2.Editable = save;
                this.b_InSaveMode = save;
            }
            base.ShowDialog();
        }
        
        private void UpdateTreeViewer()
        {
            DirectoryInfo rootDI = new DirectoryInfo(this.s_RootFolder);
            TreeViewerElement tve = new TreeViewerElement(rootDI);
            this.UpdtateTreeViewer(tve);
            this.ftv.Clear();
            this.ftv.AddElement(tve);
        }

        private void UpdtateTreeViewer(TreeViewerElement tve_parent)
        {
            if (!this.b_showHidden)
            {
                DirectoryInfo AO = (DirectoryInfo)tve_parent.AsociatedObject;
                if ((AO.Name.Substring(0,1) == ".") && (AO.Name != "./") && (AO.Name != "../"))
                {
                    return;
                }
            }
            DirectoryInfo[] dis;
            try
            {
                dis = ((DirectoryInfo)tve_parent.AsociatedObject).GetDirectories();
            }
            catch
            {
                return;
            }
            for (int i=0;i<dis.Length;i++)
            {
                TreeViewerElement tve = new TreeViewerElement(dis[i]);
                tve_parent.AddSubElement(tve);
                if (!CarpetasRevisadas.Contains(dis[i].Name))
                {
                    Console.WriteLine(dis[i].Name);
                    this.CarpetasRevisadas.Add(dis[i].Name);
                    this.UpdtateTreeViewer(tve);
                }                
            }
        }

        private void updateFolder()
        {
            this.textBox1.Text = this.s_Path;
            int selectedindex = 0;
            //this.s_FullName = this.s_Path;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.b_ok.MouseUp -= OkPulsed;
            }
            base.Dispose(disposing);
        }

        public void OkPulsed(object sender, MouseButtonEventArgs e)
        {
            if (this.FullPath != "")
            {
                string[] line = this.textBox2.Text.Split('.');
                if (line.Length>1)
                {
                    if (line[line.Length-1] == this.s_filter.Split('.')[1])
                    {
                        //this.s_FullName = this.s_Path+this.textBox2.Text;
                        this.SetResult(DialogResult.OK);
                    }
                }                
            }
        }

        public void CancelPulsed(object sender, MouseButtonEventArgs e)
        {
            this.SetResult(DialogResult.Cancel);
        }

        private void ListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if(dge.Core2D.SelectedID == ((ListViewer2)sender).ID)
            //{
                if (this.flv.SelectedObject != null)
                {
                    if ((this.flv.SelectedObject.GetType() == typeof(DirectoryInfo)) ||  (this.flv.SelectedObject.GetType() == typeof(Dialogs.ParentDirectoryInfo)))
                    {
                        this.updateFolder();
                    }
                }
            //}
        }

        private void TreeElementSelected(object sender, dge.GUI.ElementSelectedEventArgs e)
        {
            this.s_Path = ((DirectoryInfo)e.ElementSelected.AsociatedObject).FullName;
            this.updateFolder();
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

        private void ListItemSelected(object sender, ListItemSelectedEventArgs e)
        {
            if (e.ObjectSelected.GetType() == typeof(Dialogs.ParentDirectoryInfo))
            {
                this.s_Path = ((Dialogs.ParentDirectoryInfo)e.ObjectSelected).FullName;
            }
            else if (e.ObjectSelected.GetType() == typeof(DirectoryInfo))
            {
                this.s_Path = ((DirectoryInfo)e.ObjectSelected).FullName;
            }
            else
            {
                this.s_Path = ((FileInfo)e.ObjectSelected).DirectoryName;//.FullName;
                this.textBox2.Text = ((FileInfo)e.ObjectSelected).Name;
            }
        }

        public string Filter 
        {
            set { this.s_filter = value; this.updateFolder(); }
            get { return this.s_filter; }
        }

        public string FullPath
        {
            get { return this.s_Path+"/"+this.textBox2.Text; }
        }

        public string FileName
        {
            get { return this.textBox2.Text; }
        }

        public string RootFolder 
        {
            set { this.s_RootFolder = value; UpdateTreeViewer(); }
            get { return this.s_RootFolder; }
        }

        public bool IsInSaveMode
        {
            get { return this.b_InSaveMode; }
        }
    }
}