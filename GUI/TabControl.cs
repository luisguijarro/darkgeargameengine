using System;
using System.Collections.Generic;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class TabControl : BaseObjects.Control
    {
        private Orientation barOrientation;
        private int i_TabBarWidth;
        private readonly Dictionary<string,uint> d_Name_Id;
        private dgFont font;
        private float f_fontsize;

        
        public TabControl() : this(300, 100)
        {

        }
        public TabControl(uint width, uint height) : base(width, height)
        {
            this.barOrientation = Orientation.Horizontal;
            this.d_Name_Id = new Dictionary<string, uint>();
            this.f_fontsize = 12f;

            this.font = GuiTheme.DefaultGuiTheme.DefaultFont;

        }

        protected internal override void UpdateTheme()
        {
            base.UpdateTheme();
            this.font = this.gui.gt_ActualGuiTheme.DefaultFont;
        }

        protected virtual void pDraw()
        {
            if (this.b_ShowMe)
            {
                //this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.ui_width, this.ui_height, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
            }
        }

        protected override void DrawContent()
        {
            // Show Tabs Bar
            int actualPosx = 0;
            uint tabsHeigth = (uint)(Math.Ceiling(this.font.MaxCharacterHeight+(this.f_fontsize/this.font.MaxFontSize)));
            for (int i=0;i<this.VisibleSurfaceOrder.Count;i++)
            {
                if (this.barOrientation == Orientation.Horizontal)
                {
                    TabPage tp = (TabPage)this.d_guiSurfaces[this.VisibleSurfaceOrder[i]];
                    uint tmp_width = (uint)(Math.Ceiling(dge.G2D.Writer.MeasureString(this.font, tp.Name, this.f_fontsize)[0]));
                    this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, actualPosx, 0, tmp_width, tabsHeigth, 0, this.MarginsFromTheEdge, Texcoords, this.tcFrameOffset, 0);
                    actualPosx += (int)tmp_width;
                }
                else
                {

                }
            }
            //base.DrawContent();
        }

        public void AddTabPage(string TabName)
        {
            TabPage tp = new TabPage(TabName);
            this.d_Name_Id.Add(TabName, tp.ID);
            base.AddSurface((BaseObjects.BaseGuiSurface)tp);
        }
        public void AddTabPage(TabPage page, string TabName)
        {
            this.d_Name_Id.Add(TabName, page.ID);
            base.AddSurface((BaseObjects.BaseGuiSurface)page);
        }
        public void RemoveTabPage(string TabName)
        {
            base.RemoveSurface(this.d_Name_Id[TabName]);
        }
        #region Ocultar Metodos

        new public void AddSubControl(BaseObjects.Control control)
        {
            //base.AddSurface((BaseObjects.BaseGuiSurface) control);
        }

        new public void RemoveSubControl(uint id)
        {
            //base.RemoveSurface(id);
        }

        #endregion

        public Orientation TabBarOrientation
        {
            set { this.barOrientation = value; }
            get { return this.barOrientation; }
        }
    }
}