using System;
using System.Collections.Generic;

namespace dge
{
    public class Scene2D : Scene
    {
        protected int i_width;
        protected int i_height;
        protected G2D.Drawer drawer2D;
        protected G2D.Writer writer2D;
        protected int ViewPortX;
        protected int ViewPortY;
        protected int ViewPortWidth;
        protected int ViewPortHeight;

        protected bool b_invert_Yaxis;
        
        private Scene2dScaleMode SSM;
        private bool mustUpdatePerspective;

        public Scene2D(int width, int height, string name) : base(name)
        {
            this.i_width = width;
            this.i_height = height;
            c4_BackGroundColor = dgtk.Graphics.Color4.Gray;
            this.SceneScaleMode = Scene2dScaleMode.ScaleProportionally;
            this.mustUpdatePerspective = true;
        }
        internal override void SetParentWindow(dgWindow win)
        {
            base.SetParentWindow(win);
            this.drawer2D = this.parentWin.Drawer2D;
            this.writer2D = this.parentWin.Writer2D;
            parentWin.WindowSizeChange += ParentWinResize;
            this.Rescale();
        }

        internal override void RemParentWindow()
        {
            parentWin.WindowSizeChange -= ParentWinResize;
            base.RemParentWindow();
        }
        
        internal override void InternalDraw()
        {
            //this.UpDatePerspective();
            this.Call_glViewPort();
            base.InternalDraw();
        }
        
        protected void Rescale()
        {
            if (this.parentWin != null)
            {
                switch(this.SSM)
                {
                    case Scene2dScaleMode.NotScale:
                        this.ViewPortX = 0;
                        this.ViewPortY = 0;
                        this.ViewPortWidth = this.parentWin.Width;
                        this.ViewPortHeight = this.parentWin.Height;
                        break;
                    case Scene2dScaleMode.ScaleToWindow:
                        this.ViewPortX = 0;
                        this.ViewPortY = 0;
                        this.ViewPortWidth = this.parentWin.Width;
                        this.ViewPortHeight = this.parentWin.Height;
                        break;
                    case Scene2dScaleMode.ScaleProportionally:
                        if (((float)this.parentWin.Width/(float)this.parentWin.Height) > ((float)this.i_width/(float)this.i_height))
                        {
                            this.ViewPortHeight = this.parentWin.Height;
                            this.ViewPortWidth = (int)(this.parentWin.Height * ((float)this.i_width/(float)this.i_height));
                            this.ViewPortX = (int)((this.parentWin.Width-this.ViewPortWidth)/2f);
                            this.ViewPortY = 0;
                        }
                        else if (((float)this.parentWin.Width/(float)this.parentWin.Height) < ((float)this.i_width/(float)this.i_height))
                        {
                            this.ViewPortWidth = this.parentWin.Width;
                            this.ViewPortHeight = (int)(this.parentWin.Width * ((float)this.i_height/(float)this.i_width));
                            this.ViewPortX = 0;
                            this.ViewPortY = (int)((this.parentWin.Height-this.ViewPortHeight)/2f);
                        }
                        else
                        {
                            this.ViewPortX = 0;
                            this.ViewPortY = 0;
                            this.ViewPortWidth = this.parentWin.Width;
                            this.ViewPortHeight = this.parentWin.Height;
                        }
                        break;
                }
                this.mustUpdatePerspective = true;
            }              
        }

        protected void UpDatePerspective()
        {
            //if (this.mustUpdatePerspective)
            //{
                if (this.SSM == Scene2dScaleMode.NotScale)
                {
                    this.parentWin.Drawer2D.DefinePerspectiveMatrix(0, 0, this.ViewPortWidth, this.ViewPortHeight, b_invert_Yaxis);
                    this.parentWin.Writer2D.DefinePerspectiveMatrix(0, 0, this.ViewPortWidth, this.ViewPortHeight, b_invert_Yaxis);
                }
                else
                {
                    this.parentWin.Drawer2D.DefinePerspectiveMatrix(0, 0, this.i_width, this.i_height, b_invert_Yaxis);
                    this.parentWin.Writer2D.DefinePerspectiveMatrix(0, 0, this.i_width, this.i_height, b_invert_Yaxis);
                }
                //this.Call_glViewPort();
                this.mustUpdatePerspective = false;
            //}
            //this.Call_glViewPort();
        }

        protected void Call_glViewPort()
        {
            dgtk.OpenGL.GL.glViewport(this.ViewPortX, this.ViewPortY, this.ViewPortWidth, this.ViewPortHeight);
        }

        public Scene2dScaleMode SceneScaleMode
        {
            set { this.SSM = value; this.Rescale(); }
            get { return this.SSM; }
        }

        private void ParentWinResize(object sender, dgtk.dgtk_ResizeEventArgs e)
        {
            this.Rescale();
        }
    }

    public enum Scene2dScaleMode
    {
        NotScale = 0, ScaleToWindow = 1, ScaleProportionally = 2
    }
}