using System;
using System.Reflection;
using System.Collections.Generic;

using dge.SoundSystem;

using dge.G2D;
using dgtk.Graphics;

namespace dge.GUI
{
    public class AudioPlayer : BaseObjects.Control
    {
        private readonly InteractiveProgressBar ipb_ProgresBar;
        private int[] AudioPlayer_ButtonsSize;
        private float[] AudioPlayer_PlayButtonTexcoords;
        private float[] AudioPlayer_PlayButtonOffSet;
        private float[] AudioPlayer_PauseButtonTexcoords;
        private float[] AudioPlayer_PauseButtonOffSet;
        private float[] AudioPlayer_StopButtonTexcoords;
        private float[] AudioPlayer_StopButtonOffSet;
        private readonly uint id_play, id_pause, id_stop;
        private readonly Color4 c4_play, c4_pause, c4_stop;
        private bool b_play, b_pause, b_stop;
        private readonly SoundSource3D ss3D;
        private string SoundName;
        public AudioPlayer() : base(200, 55)
        {
            this.SoundName = "";
            this.MarginsFromTheEdge = GuiTheme.DefaultGuiTheme.AudioPlayer_MarginsFromTheEdge;            
            this.Texcoords = GuiTheme.DefaultGuiTheme.AudioPlayer_Texcoords;
            this.AudioPlayer_ButtonsSize = GuiTheme.DefaultGuiTheme.AudioPlayer_ButtonsSize;
            this.AudioPlayer_PlayButtonTexcoords = GuiTheme.DefaultGuiTheme.AudioPlayer_PlayButtonTexcoords;
            this.AudioPlayer_PlayButtonOffSet =  GuiTheme.DefaultGuiTheme.AudioPlayer_PlayButtonOffSet;
            this.AudioPlayer_PauseButtonTexcoords = GuiTheme.DefaultGuiTheme.AudioPlayer_PauseButtonTexcoords;
            this.AudioPlayer_PauseButtonOffSet =  GuiTheme.DefaultGuiTheme.AudioPlayer_PauseButtonOffSet;
            this.AudioPlayer_StopButtonTexcoords = GuiTheme.DefaultGuiTheme.AudioPlayer_StopButtonTexcoords;
            this.AudioPlayer_StopButtonOffSet =  GuiTheme.DefaultGuiTheme.AudioPlayer_StopButtonOffSet;
            this.tcFrameOffset = new float[] {0,0};

            this.id_play = dge.Core2D.GetID();
            this.id_pause = dge.Core2D.GetID();
            this.id_stop = dge.Core2D.GetID();

            this.c4_play = new Color4(Core2D.DeUIntAByte4(this.id_play));
            this.c4_pause = new Color4(Core2D.DeUIntAByte4(this.id_pause));
            this.c4_stop = new Color4(Core2D.DeUIntAByte4(this.id_stop));

            this.b_play = false;
            this.b_pause = false;
            this.b_stop = true;

            this.ss3D = new SoundSource3D();

            // Texto con el nombre de la canción.
            this.ipb_ProgresBar = new InteractiveProgressBar();
            this.ipb_ProgresBar.X = this.MarginLeft;
            this.ipb_ProgresBar.Y = 20;
            this.ipb_ProgresBar.Width = this.InnerSize.Width - (this.MarginLeft+this.MarginRight);
            this.ipb_ProgresBar.Height = 10;
            this.ipb_ProgresBar.ValueChanged += Ipb_ValueChange;
            this.AddSubControl(this.ipb_ProgresBar);


            this.UpdateState();
        }

        private void Ipb_ValueChange(object sender, IntValueChangedEventArgs e)
        {
            if (!e.fromProperty)
            {
                if (this.ss3D.HaveSound())
                {
                    this.ss3D.TimeMilliSeconds = e.Value;
                    Console.WriteLine("Value: "+e.Value);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                dge.Core2D.ReleaseID(this.id_play);
                dge.Core2D.ReleaseID(this.id_pause);
                dge.Core2D.ReleaseID(this.id_stop);
            }
        }
        
        protected override void OnResize()
        {
            base.OnResize();

            this.ipb_ProgresBar.Width = this.InnerSize.Width - (this.MarginLeft+this.MarginRight);
            this.ipb_ProgresBar.Height = 10;
            this.ipb_ProgresBar.Y = this.Height - 35;
        }

        protected override void OnGuiUpdate()
        {
            base.OnGuiUpdate();

            this.MarginsFromTheEdge = this.gui.gt_ActualGuiTheme.AudioPlayer_MarginsFromTheEdge;            
            this.Texcoords = this.gui.gt_ActualGuiTheme.AudioPlayer_Texcoords;
            this.AudioPlayer_ButtonsSize = this.gui.gt_ActualGuiTheme.AudioPlayer_ButtonsSize;
            this.AudioPlayer_PlayButtonTexcoords = this.gui.gt_ActualGuiTheme.AudioPlayer_PlayButtonTexcoords;
            this.AudioPlayer_PlayButtonOffSet =  this.gui.gt_ActualGuiTheme.AudioPlayer_PlayButtonOffSet;
            this.AudioPlayer_PauseButtonTexcoords = this.gui.gt_ActualGuiTheme.AudioPlayer_PauseButtonTexcoords;
            this.AudioPlayer_PauseButtonOffSet =  this.gui.gt_ActualGuiTheme.AudioPlayer_PauseButtonOffSet;
            this.AudioPlayer_StopButtonTexcoords = this.gui.gt_ActualGuiTheme.AudioPlayer_StopButtonTexcoords;
            this.AudioPlayer_StopButtonOffSet =  this.gui.gt_ActualGuiTheme.AudioPlayer_StopButtonOffSet;
        }

        #region Protected Draw:

        protected override void DrawContent()
        {
            base.DrawContent();

            if (dge.G2D.Writer.Fonts.ContainsKey(this.gui.gt_ActualGuiTheme.Default_Font.Name))
            {
                this.gui.Writer.Write(this.gui.gt_ActualGuiTheme.Default_Font.Name, new Color4(0, 195, 255, 255), this.SoundName, 14, 5, 5, Color4.Blue);
            }

            this.gui.Drawer.Draw(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO, this.InnerSize.Width-(this.AudioPlayer_ButtonsSize[0]*3), this.InnerSize.Height-this.AudioPlayer_ButtonsSize[1], 0, this.AudioPlayer_ButtonsSize[0], this.AudioPlayer_ButtonsSize[1], 0f,
            this.AudioPlayer_PlayButtonTexcoords[0] + (this.b_play ? this.AudioPlayer_PlayButtonOffSet[0] : 0), 
            this.AudioPlayer_PlayButtonTexcoords[1] + (this.b_play ? this.AudioPlayer_PlayButtonOffSet[1] : 0),  
            this.AudioPlayer_PlayButtonTexcoords[2] + (this.b_play ? this.AudioPlayer_PlayButtonOffSet[0] : 0),  
            this.AudioPlayer_PlayButtonTexcoords[3] + (this.b_play ? this.AudioPlayer_PlayButtonOffSet[1] : 0) );

            this.gui.Drawer.Draw(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO, this.InnerSize.Width-(this.AudioPlayer_ButtonsSize[0]*2), this.InnerSize.Height-this.AudioPlayer_ButtonsSize[1], 0, this.AudioPlayer_ButtonsSize[0], this.AudioPlayer_ButtonsSize[1], 0f,
            this.AudioPlayer_PauseButtonTexcoords[0] + (this.b_pause ? this.AudioPlayer_PauseButtonOffSet[0] : 0),  
            this.AudioPlayer_PauseButtonTexcoords[1] + (this.b_pause ? this.AudioPlayer_PauseButtonOffSet[1] : 0),  
            this.AudioPlayer_PauseButtonTexcoords[2] + (this.b_pause ? this.AudioPlayer_PauseButtonOffSet[0] : 0),  
            this.AudioPlayer_PauseButtonTexcoords[3] + (this.b_pause ? this.AudioPlayer_PauseButtonOffSet[1] : 0) );

            this.gui.Drawer.Draw(this.gui.gt_ActualGuiTheme.tbo_ThemeTBO, this.InnerSize.Width-(this.AudioPlayer_ButtonsSize[0]*1), this.InnerSize.Height-this.AudioPlayer_ButtonsSize[1], 0, this.AudioPlayer_ButtonsSize[0], this.AudioPlayer_ButtonsSize[1], 0f,
            this.AudioPlayer_StopButtonTexcoords[0] + (this.b_stop ? this.AudioPlayer_StopButtonOffSet[0] : 0),  
            this.AudioPlayer_StopButtonTexcoords[1] + (this.b_stop ? this.AudioPlayer_StopButtonOffSet[1] : 0),  
            this.AudioPlayer_StopButtonTexcoords[2] + (this.b_stop ? this.AudioPlayer_StopButtonOffSet[0] : 0),  
            this.AudioPlayer_StopButtonTexcoords[3] + (this.b_stop ? this.AudioPlayer_StopButtonOffSet[1] : 0) );
        }

        protected override void DrawContentIDs()
        {
            base.DrawContentIDs();

            dge.G2D.IDsDrawer.DrawGL2D(this.gui.gt_ActualGuiTheme.tbo_ThemeSltTBO.ID, this.c4_play, this.InnerSize.Width-(this.AudioPlayer_ButtonsSize[0]*3), this.InnerSize.Height-this.AudioPlayer_ButtonsSize[1], this.AudioPlayer_ButtonsSize[0], this.AudioPlayer_ButtonsSize[1], 0f, 
            this.AudioPlayer_PlayButtonTexcoords[0], this.AudioPlayer_PlayButtonTexcoords[1], this.AudioPlayer_PlayButtonTexcoords[2], this.AudioPlayer_PlayButtonTexcoords[3], 1);

            dge.G2D.IDsDrawer.DrawGL2D(this.gui.gt_ActualGuiTheme.tbo_ThemeSltTBO.ID, this.c4_pause, this.InnerSize.Width-(this.AudioPlayer_ButtonsSize[0]*2), this.InnerSize.Height-this.AudioPlayer_ButtonsSize[1], this.AudioPlayer_ButtonsSize[0], this.AudioPlayer_ButtonsSize[1], 0f, 
            this.AudioPlayer_PauseButtonTexcoords[0], this.AudioPlayer_PauseButtonTexcoords[1], this.AudioPlayer_PauseButtonTexcoords[2], this.AudioPlayer_PauseButtonTexcoords[3], 1);

            dge.G2D.IDsDrawer.DrawGL2D(this.gui.gt_ActualGuiTheme.tbo_ThemeSltTBO.ID, this.c4_stop, this.InnerSize.Width-(this.AudioPlayer_ButtonsSize[0]*1), this.InnerSize.Height-this.AudioPlayer_ButtonsSize[1], this.AudioPlayer_ButtonsSize[0], this.AudioPlayer_ButtonsSize[1], 0f, 
            this.AudioPlayer_StopButtonTexcoords[0], this.AudioPlayer_StopButtonTexcoords[1], this.AudioPlayer_StopButtonTexcoords[2], this.AudioPlayer_StopButtonTexcoords[3], 1);
        }

        #endregion

        #region Protected Inputs:

        protected override void OnMDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMDown(sender, e);

            if (this.id_play == e.ID)
            {
                this.ss3D.Play();
                this.UpdateState();
                this.gui.ParentWindow.UpdateFrame += UpdateFrame;
            }

            if (this.id_pause == e.ID)
            {
                this.ss3D.Pause();
                this.UpdateState();
                this.gui.ParentWindow.UpdateFrame -= UpdateFrame;
            }

            if (this.id_stop == e.ID)
            {
                this.ss3D.Stop();
                this.UpdateState();
                this.gui.ParentWindow.UpdateFrame -= UpdateFrame;
            }
        }

        private void UpdateFrame(object sender, dgtk.dgtk_OnUpdateEventArgs e)
        {
            this.UpdatePlaystate();
        }

        private dgtk.OpenAL.AL_SourceState UpdateState()
        {
            switch(ss3D.State)
            {
                case dgtk.OpenAL.AL_SourceState.AL_PLAYING:
                    this.b_play = true;
                    this.b_pause = false;
                    this.b_stop = false;
                    break;
                
                case dgtk.OpenAL.AL_SourceState.AL_PAUSED:
                    this.b_play = false;
                    this.b_pause = true;
                    this.b_stop = false;
                    break;
                
                case dgtk.OpenAL.AL_SourceState.AL_STOPPED:
                    this.b_play = false;
                    this.b_pause = false;
                    this.b_stop = true;
                    this.ipb_ProgresBar.Value = 0;
                    break;
                
                default:
                    this.b_play = false;
                    this.b_pause = false;
                    this.b_stop = true;
                    break;
            }
            return ss3D.State;
        }

        private void UpdatePlaystate()
        {
            this.ipb_ProgresBar.Value = (int)this.ss3D.TimeMilliSeconds;            
            
            if (UpdateState() == dgtk.OpenAL.AL_SourceState.AL_STOPPED)
            {
                this.gui.ParentWindow.UpdateFrame -= UpdateFrame;
            }
        }

        #endregion

        public bool AssignSound(Sound sound)
        {
            bool ret = this.ss3D.AssignSound(sound);
            if (ret)
            {
                this.SoundName = sound.FileName;
                this.ipb_ProgresBar.MaxValue = (int)this.ss3D.AssignedSound.DurationMilliseconds; 
            }            
            return ret;
        }

        public Sound AssignedSound
        {
            get { return this.ss3D.AssignedSound; }
        }
    }
}