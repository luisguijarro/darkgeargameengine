using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class ImagesGallery : BaseObjects.Control
    {
        private readonly ScrollBar sbVer;
        //private readonly ScrollBar sbHor;

        private dgFont font;
        private float f_FontSize;
        private Color4 c4_fontColor;
        private dgtk.Graphics.Color4 c4_textBorderColor;
        private Color4 c4_BackgroundColor;
        private int[] ImageBorderMargins;
        private float[] ImageBorderTexCoords;
        private Dictionary<uint, TextureBufferObject> d_images;
        private Dictionary<string, uint> d_indexNames;
        private Dictionary<uint, uint> d_InteractiveIDs; // internal ID, Interactive ID. 
        private uint i_ImagesMargin;
        private uint i_ImagesForRow;
        private int imageSize;
        private TextureBufferObject tbo_Selected;

        public event EventHandler<ImageSelectedEventArgs> SelectedImage;
        
        public ImagesGallery() : base()
        {
            this.sbVer = new ScrollBar();
            this.sbVer.MaxValue = 0; // Ocultas por defecto.
            this.sbVer.Visible = true; // Ocultas por defecto.
            /*this.sbHor = new ScrollBar()
            {
                Orientation = Orientation.Horizontal
            };

            this.sbHor.MaxValue = 0; // Ocultas por defecto.

            this.sbHor.Visible = false; // Ocultas por defecto.
            */
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.ImagesGallery_MarginsFromTheEdge; // n=4
            this.Texcoords = GuiTheme.DefaultGuiTheme.ImagesGallery_Texcoords; // n=8
            this.c4_BackgroundColor = GuiTheme.DefaultGuiTheme.ImagesGallery_DefaultBackgroundColor;
            this.ImageBorderMargins = GuiTheme.DefaultGuiTheme.ImagesGallery__image_MarginsFromTheEdge; // n=4
            this.ImageBorderTexCoords = GuiTheme.DefaultGuiTheme.ImagesGallery_image_Texcoords; // n=8
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.ImagesGallery_image_FrameOffset; // n=2

            this.d_images = new Dictionary<uint, TextureBufferObject>();
            this.d_indexNames = new Dictionary<string, uint>();
            this.d_InteractiveIDs = new Dictionary<uint, uint>();

            this.tbo_Selected = TextureBufferObject.Null;

            this.i_ImagesMargin = 3;
            this.i_ImagesForRow = 3;

            this.SelectedImage += delegate {};

            this.UpdateScrollBars();
        }

        #region PROTECTED METHODS:

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                //this.sbHor.Dispose();
                this.sbVer.Dispose();
            }
        }

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();            

            //this.sbHor.GUI = this.gui;
            this.sbVer.GUI = this.gui;
        }

        protected internal override void UpdateTheme()
        {
            base.UpdateTheme();

            this.MarginsFromTheEdge = this.gui.GuiTheme.ImagesGallery_MarginsFromTheEdge; // n=4
            this.Texcoords =  this.gui.GuiTheme.ImagesGallery_Texcoords; // n=8
            this.c4_BackgroundColor =  this.gui.GuiTheme.ImagesGallery_DefaultBackgroundColor;
            this.ImageBorderMargins =  this.gui.GuiTheme.ImagesGallery__image_MarginsFromTheEdge; // n=4
            this.ImageBorderTexCoords =  this.gui.GuiTheme.ImagesGallery_image_Texcoords; // n=8
            this.tcFrameOffset =  this.gui.GuiTheme.ImagesGallery_image_FrameOffset; // n=2

            if (this.c4_BackgroundColor == this.gui.gt_ActualGuiTheme.TreeViewer_DefaultBackgroundColor)
            {
                this.c4_BackgroundColor = this.gui.gt_ActualGuiTheme.TreeViewer_DefaultBackgroundColor;
            }

            // Si la fuente establecida es la del tema por defecto se cambia, sino, se deja la establecida por el usuario.
            if (this.font.Name == GuiTheme.DefaultGuiTheme.Default_Font.Name)
            {
                this.font = this.gui.GuiTheme.Default_Font;
            }
            if (this.f_FontSize == GuiTheme.DefaultGuiTheme.Default_FontSize)
            {
                this.f_FontSize = this.gui.GuiTheme.Default_FontSize;
            }
            if (this.c4_fontColor == GuiTheme.DefaultGuiTheme.Default_TextColor)
            {
                this.c4_fontColor = this.gui.GuiTheme.Default_TextColor;
            }
        }

        protected override void OnResize()
        {
            base.OnResize();
            
            this.sbVer.X = (int)(this.Width - this.sbVer.Width);
            this.sbVer.Y = this.Y;
            this.sbVer.Height = this.Height;
            /*
            this.sbHor.X = this.X;
            this.sbHor.Y = (int)(this.Height - this.sbHor.Height);
            this.sbHor.Width = this.Width-this.sbVer.Width;
            */

            this.UpdateScrollBars();  
        }
        
        protected override void OnReposition()
        {
            base.OnResize();
            
            this.sbVer.X = (int)(this.Width - this.sbVer.Width);
            this.sbVer.Y = this.Y;
            this.sbVer.Height = this.Height;
            /*
            this.sbHor.X = this.X;
            this.sbHor.Y = (int)(this.Height - this.sbHor.Height);
            this.sbHor.Width = this.Width-this.sbVer.Width;
            */

            this.UpdateScrollBars();  
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);
            //this.tbo_Selected = TextureBufferObject.Null;
            if (this.d_InteractiveIDs.ContainsValue(dge.Core2D.SelectedID))
            {
                foreach(uint key in this.d_InteractiveIDs.Keys)
                {
                    if (this.d_InteractiveIDs[key] == dge.Core2D.SelectedID)
                    {
                        this.tbo_Selected = this.d_images[key];
                        this.SelectedImage(this, new ImageSelectedEventArgs(this.d_images[key]));
                    }
                }
            }
        }

        protected override void OnMWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnMWheel(sender, e);
            if ((this.ui_id == e.ID) || (this.d_InteractiveIDs.ContainsValue(e.ID)))
            {
                this.sbVer.Value -= (int)((this.imageSize/2f) * e.Delta);
            }
        }

        protected override void pDraw()
        {
            if (this.b_ShowMe)
            {
                this.gui.Drawer.Draw(this.c4_BackgroundColor, this.i_x, this.i_y, this.i_width, this.i_height, 0f);
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, new float[]{0f, 0f}, 0);
            }
            this.DrawScrollBars();
        }

        protected override void pDrawID()
        {
            base.pDrawID();
            this.DrawScrollBarsIDs();
        }

        protected override void pDrawContent()
        {
            if (this.contentUpdate && d_images.Count>0) 
            {
                DrawIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContent);
            }
        }

        protected override void pDrawContentID()
        {
            if (this.contentUpdate && d_images.Count>0) 
            {
                DrawIdIn(this.ida_X, this.ida_Y, this.ida_Width, this.ida_Height, DrawContentIDs);
            }
        }

        protected override void DrawContentIDs()
        {
            //base.DrawContentIDs();
            int XCont = 0;
            int rows = 0;
            foreach (TextureBufferObject val in this.d_images.Values)
            {
                int xpos = (int)(this.i_ImagesMargin+((this.i_ImagesMargin+imageSize)*XCont));
                int ypos = (int)(this.i_ImagesMargin+((this.i_ImagesMargin+imageSize)*rows));
                ypos-=this.sbVer.Value;
                dge.G2D.IDsDrawer.DrawGL2D(new Color4(dge.Core2D.DeUIntAByte4(this.d_InteractiveIDs[val.ID])), xpos, ypos, this.imageSize, this.imageSize, 0f);
                XCont++;
                if (XCont >= this.i_ImagesForRow)
                {
                    rows++;
                    XCont=0;
                    xpos = 0;
                }
            }
        }
        protected override void DrawContent()
        {            
            //int xpos = 0;
            //int ypos = 0;
            int XCont = 0;
            int rows = 0;
            foreach (TextureBufferObject val in this.d_images.Values)
            {
                int xpos = (int)(this.i_ImagesMargin+((this.i_ImagesMargin+imageSize)*XCont));
                int ypos = (int)(this.i_ImagesMargin+((this.i_ImagesMargin+imageSize)*rows));
                int xpos2 = xpos;
                int ypos2 = ypos;


                int twidth = imageSize;
                int theight = imageSize;
                if (val.Width > val.Height)
                {
                    twidth = imageSize;
                    theight = (int)((val.Height/(float)val.Width)*(float)imageSize);
                    ypos += (imageSize-theight)/2;
                }
                else
                {
                    twidth = (int)((val.Width/(float)val.Height)*(float)imageSize);
                    theight = imageSize;
                    xpos += (imageSize-twidth)/2;
                }

                
                ypos-=this.sbVer.Value;
                ypos2-=this.sbVer.Value;
                this.gui.Drawer.Draw(val.ID, xpos, ypos, twidth, theight, 0f);
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, xpos2, ypos2, imageSize, imageSize, 0, this.ImageBorderMargins, this.ImageBorderTexCoords, (val.ID == this.SelectedTBO.ID) ? this.tcFrameOffset : new float[]{0f, 0f}, 0);
                XCont++;
                if (XCont >= this.i_ImagesForRow)
                {
                    rows++;
                    XCont=0;
                    xpos = 0;
                }
            }
        }
        
        private void DrawScrollBars()
        {
            if (this.sbVer.Visible) { this.sbVer.Draw(); }
            //if (this.sbHor.Visible) { this.sbHor.Draw(); }
        }

        private void DrawScrollBarsIDs()
        {
            if (this.sbVer.Visible) { this.sbVer.DrawID(); }
            //if (this.sbHor.Visible) { this.sbHor.DrawID(); }
        }

        protected override void InputSizeAlter(int width, int height)
        {
            base.InputSizeAlter(sbVer.Visible ? width-this.sbVer.Width : width, /*sbHor.Visible ? height - this.sbHor.Height : */height);
        }

        protected override int[] OutputSizeAlter(int width, int height)
        {
            return base.OutputSizeAlter(sbVer.Visible ? width+this.sbVer.Width : width, /*sbHor.Visible ? height + this.sbHor.Height : */height);
        }

        #endregion

        #region PRIVATE METHODS:

        private void ClearIDs()
        {
            foreach (uint key in this.d_InteractiveIDs.Keys)
            {
                dge.Core2D.ReleaseID(this.d_InteractiveIDs[key]);
            }
            this.d_InteractiveIDs.Clear();
        }

        private void UpdateScrollBars()
        {
            this.imageSize = (int)((this.InnerSize.Width-(this.i_ImagesMargin*this.i_ImagesForRow+1))/this.ImagesForRow);

            int rows = (int)(this.d_images.Count/this.i_ImagesForRow);
            if (this.d_images.Count%this.i_ImagesForRow != 0)
            {
                rows++;
            }
            int rowsHeight = (int)(this.i_ImagesMargin + (rows * (this.i_ImagesMargin + this.imageSize)));
            if (rowsHeight > this.InnerSize.Height)
            {
                this.sbVer.MaxValue = rowsHeight - this.InnerSize.Height;
                this.sbVer.Enable = true;
            }
            else
            {
                this.sbVer.MaxValue = 0;
                this.sbVer.Enable = false;
            }
        }

        #endregion

        #region  PUBLIC METHODS:

        public void AddImage(string filepath)
        {
            this.AddImage(dge.G2D.Tools.LoadImage(filepath));
        }

        public void AddImage(Stream filedata, string name)
        {
            this.AddImage(dge.G2D.Tools.LoadImage(filedata, name));
        }

        public  void AddImage(TextureBufferObject tbo)
        {
            if (!this.d_images.ContainsKey(tbo.ID))
            {
                this.d_images.Add(tbo.ID, tbo);
                this.d_indexNames.Add(tbo.s_name, tbo.ID);
                this.d_InteractiveIDs.Add(tbo.ID, dge.Core2D.GetID());
                this.UpdateScrollBars();
            }
        }

        public TextureBufferObject GetImage(string name)
        {
            if (this.d_indexNames.ContainsKey(name))
            {
                return this.d_images[this.d_indexNames[name]];
            }
            return TextureBufferObject.Null;
        }

        public TextureBufferObject GetImage(uint id)
        {
            if (this.d_images.ContainsKey(id))
            {
                return this.d_images[id];
            }
            return TextureBufferObject.Null;
        }

        public void RemoveImage(uint id)
        {
            if (this.d_images.ContainsKey(id))
            {
                this.d_images.Remove(id);
                foreach(uint key in this.d_InteractiveIDs.Keys)
                {
                    if (key == id)
                    {
                        this.d_InteractiveIDs.Remove(key);
                        break;
                    }
                }
                this.UpdateScrollBars();
            }
        }

        public void RemoveImage(string name)
        {
            if (this.d_indexNames.ContainsKey(name))
            {
                if (this.d_InteractiveIDs.ContainsKey(this.d_indexNames[name]))
                {
                    this.d_InteractiveIDs.Remove(this.d_indexNames[name]);
                }
                this.d_images.Remove(this.d_indexNames[name]);
                this.d_indexNames.Remove(name);
                this.UpdateScrollBars();
            }
        }

        public void Clear()
        {
            this.d_images.Clear();
            this.d_indexNames.Clear();
            this.ClearIDs();
            this.UpdateScrollBars();
        }

        #endregion

        #region PROPERTIES:

        public dgFont Font
        {
            set 
            { 
                this.font = value; 
                this.UpdateScrollBars();  
            }
            get { return this.font; }
        }

        public float FontSize
        {
            set
            {
                this.f_FontSize = value; 
                this.UpdateScrollBars();  
            }
            get { return this.f_FontSize; }
        }

        public Color4 TextColor
        {
            set { this.c4_fontColor = value; }
            get { return this.c4_fontColor; }
        }

        public Color4 TextBorderColor
        {
            set { this.c4_textBorderColor = value; }
            get { return this.c4_textBorderColor; }
        }

        public uint ImagesMargin
        {
            set { this.i_ImagesMargin = value;}
            get { return this.i_ImagesMargin; }
        }
        
        public uint ImagesForRow
        {
            set 
            { 
                if (value > 0)
                {
                    this.i_ImagesForRow = value; 
                }
            }
            get { return this.i_ImagesForRow; }
        }

        public TextureBufferObject SelectedTBO
        {
            get { return this.tbo_Selected; }
        }

        #endregion
    }
}