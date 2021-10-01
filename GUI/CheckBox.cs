using System;
using dgtk;
using dgtk.Math;
using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class CheckBox : BaseObjects.Control
    {
        protected bool b_Checked;
        protected bool b_ButtonMode;
        private TextureBufferObject tbo_image;
        public event EventHandler<CheckedStateChanged> CheckedStateChanged;
        public CheckBox() : base(22, 22)
        {
            this.b_Checked = false;

            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.CheckBox_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.CheckBox_Texcoords;
            this.tcFrameOffset = GuiTheme.DefaultGuiTheme.CheckBox_FrameOffset;

            this.CheckedStateChanged += delegate{};

            this.tbo_image = TextureBufferObject.Null;
        }

        protected internal override void UpdateTheme()
        {
            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.CheckBox_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.CheckBox_Texcoords;
            this.tcFrameOffset = this.gui.gt_ActualGuiTheme.CheckBox_FrameOffset;
            this.SetGraphicMode(this.b_ButtonMode);
        }

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();
            if (this.b_ButtonMode)
            {
                this.MarginsFromTheEdge = this.gui.GuiTheme.Button_MarginsFromTheEdge;            
                this.Texcoords = this.gui.GuiTheme.Button_Texcoords;
                this.tcFrameOffset = this.gui.GuiTheme.Button_FrameOffset;
            }
            else
            {
                this.MarginsFromTheEdge = this.gui.GuiTheme.CheckBox_MarginsFromTheEdge;            
                this.Texcoords = this.gui.GuiTheme.CheckBox_Texcoords;
                this.tcFrameOffset = this.gui.GuiTheme.CheckBox_FrameOffset;
            }
        }

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            if (dge.Core2D.SelectedID == this.ui_id)
            {
                this.b_Checked = !this.b_Checked;
                this.CheckedStateChanged(this, new CheckedStateChanged(this.b_Checked));
                base.OnMDown(sender, e);
            }
        }

        protected override void pDraw()
        {
            if (this.gui != null)
            {
                //base.Draw();
                this.gui.gd_GuiDrawer.DrawGL(this.gui.GuiTheme.ThemeTBO.ID, Color4.White, this.i_x, this.i_y, this.i_width, this.i_height, 0, this.MarginsFromTheEdge, Texcoords, this.b_Checked ? this.tcFrameOffset : new float[]{0,0}, 0);
                if (this.b_ButtonMode)
                {
                    if (tbo_image.ID>0)
                    {
                        this.gui.Drawer.Draw(this.tbo_image.ui_ID, this.X+this.MarginLeft, this.Y+this.MarginTop, this.i_width-(this.MarginLeft+this.MarginRight), this.i_height-(this.MarginTop+this.MarginBottom), 0f, 0f, 0f, 1f, 1f);
                    }
                }
            }
        }

        public void SetGraphicMode(bool Is_Like_Button)
        {
            if (this.gui != null)
            {
                if (Is_Like_Button)
                {
                    this.MarginsFromTheEdge = this.gui.GuiTheme.Button_MarginsFromTheEdge;            
                    this.Texcoords = this.gui.GuiTheme.Button_Texcoords;
                    this.tcFrameOffset = this.gui.GuiTheme.Button_FrameOffset;
                }
                else
                {
                    this.MarginsFromTheEdge = this.gui.GuiTheme.CheckBox_MarginsFromTheEdge;            
                    this.Texcoords = this.gui.GuiTheme.CheckBox_Texcoords;
                    this.tcFrameOffset = this.gui.GuiTheme.CheckBox_FrameOffset;
                }
            }
            else
            {
                if (Is_Like_Button)
                {
                    this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.Button_MarginsFromTheEdge;            
                    this.Texcoords = GuiTheme.DefaultGuiTheme.Button_Texcoords;
                    this.tcFrameOffset = GuiTheme.DefaultGuiTheme.Button_FrameOffset;
                }
                else
                {
                    this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.CheckBox_MarginsFromTheEdge;            
                    this.Texcoords = GuiTheme.DefaultGuiTheme.CheckBox_Texcoords;
                    this.tcFrameOffset = GuiTheme.DefaultGuiTheme.CheckBox_FrameOffset;
                }
            }
            this.b_ButtonMode = Is_Like_Button;
        }

        public bool GraphicalButtonMode
        {
            set
            {
                this.SetGraphicMode(value);
            }
            get { return this.b_ButtonMode; }
        }

        public TextureBufferObject Image
        {
            set { this.tbo_image = value; }
            get { return this.tbo_image; }
        }
        public bool Checked
        {
            set { this.b_Checked = value; this.CheckedStateChanged(this, new CheckedStateChanged(this.b_Checked)); }
            get { return this.b_Checked; }
        }
    }
}