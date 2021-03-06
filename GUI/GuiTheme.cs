using System;
using dgtk.Math;

using dgtk.Graphics;
using dge.G2D;

namespace dge.GUI
{
    public class GuiTheme
    {
        internal dge.G2D.TextureBufferObject tbo_ThemeTBO; // TBO Visual
        internal dge.G2D.TextureBufferObject tbo_ThemeSltTBO; // TBO de siluetas para ID.

        public float[] FileIcon_TexCoords; // n=4
        public float[] FileImageIcon_TexCoords; // n=4
        public float[] FileSoundIcon_TexCoords; // n=4
        public float[] FolderIcon_TexCoords; // n=4
        public float[] TransparentBackground_TexCoords; // n=4

        public dgFont Default_Font;
        public float Default_FontSize;

        public Color4 Default_MenuBackgroundColor;
        public Color4 Default_BackgroundColor;
        public Color4 Default_ControlsBackgroundColor;
        public Color4 Default_TextColor;
        public Color4 Default_TextBorderColor;
        public Color4 Default_DisableTextColor;

        #region Ventanas:
        public int[] Window_MarginsFromTheEdge; // n=4
        public float[] Window_Texcoords; // n=8
        public float[] Window_FrameOffset; //n=2
        #endregion

        #region Dialogs:
        public int[] Dialog_Head_MarginsFromTheEdge; // n=4
        public float[] Dialog_Head_Texcoords; // n=8
        public int[] Dialog_Body_MarginsFromTheEdge; // n=4
        public float[] Dialog_Body_Texcoords; // n=8
        #endregion


        #region Button:
        public int[] Button_MarginsFromTheEdge; // n=4
        public float[] Button_Texcoords; // n=8
        public float[] Button_FrameOffset; // n=2
        #endregion

        #region TrackBar:
        public int[] TrackBar_Hor_MarginsFromTheEdge; // n=4
        public int[] TrackBar_Ver_MarginsFromTheEdge; // n=4
        public float[] TrackBar_Hor_Texcoords; // n=8
        public float[] TrackBar_Ver_Texcoords; // n=8
        public float[] TrackBar_Square_Hor_Slider_Texcoords; // n=4
        public float[] TrackBar_Square_Ver_Slider_Texcoords; // n=4
        public float[] TrackBar_Arrow_Up_Slider_Texcoords; // n=4
        public float[] TrackBar_Arrow_Down_Slider_Texcoords; // n=4
        public float[] TrackBar_Arrow_Left_Slider_Texcoords; // n=4
        public float[] TrackBar_Arrow_Right_Slider_Texcoords; // n=4
        public float[] TrackBar_FrameOffset; // n=2
        public int[] TrackBar_Slider_Size; // (Ancho, Alto)Hor, (Ancho, Alto)Ver
        public int TrackBar_Slider_PosMargin;
        public int TrackBar_Hor_MaxHeight;
        public int TrackBar_Ver_MaxWidth;
        #endregion

        #region TextBox:
        public int[] TextBox_MarginsFromTheEdge; // n=4
        public float[] TextBox_Texcoords; // n=8
        //public float[] TextBox_FrameOffset; // n=2
        public char TextBox_CursorChar;
        public Color4 TextBox_Default_BackgroundColor;
        #endregion

        #region ProgressBar

        public int[] ProgressBar_Hor_MarginsFromTheEdge; // n=4
        public int[] ProgressBar_Ver_MarginsFromTheEdge; // n=4
        public float[] ProgressBar_Hor_Texcoords; // n=8
        public float[] ProgressBar_Ver_Texcoords; // n=8
        public float[] ProgressBar_Hor_Filling_Texcoords; // n=4
        public float[] ProgressBar_Ver_Filling_Texcoords; // n=4

        #endregion

        #region InteractiveProgressBar

        public int[] InteractiveProgressBar_Hor_MarginsFromTheEdge; // n=4
        public int[] InteractiveProgressBar_Ver_MarginsFromTheEdge; // n=4
        public float[] InteractiveProgressBar_Hor_Texcoords; // n=8
        public float[] InteractiveProgressBar_Ver_Texcoords; // n=8
        public float[] InteractiveProgressBar_Hor_Filling_Texcoords; // n=4
        public float[] InteractiveProgressBar_Ver_Filling_Texcoords; // n=4

        #endregion

        #region ContentViewer

        public int[] ContentViewer_MarginsFromTheEdge; // n=4
        public float[] ContentViewer_Texcoords; // n=8

        #endregion

        #region ScrollBar

        public int[] ScrollBar_Hor_Btn1_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Hor_Btn1_Texcoords; // n=8
        public float[] ScrollBar_Hor_Btn1_FrameOffset; // n=2
        public int[] ScrollBar_Ver_Btn1_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Ver_Btn1_Texcoords; // n=8
        public float[] ScrollBar_Ver_Btn1_FrameOffset; // n=2
        public int[] ScrollBar_Hor_Btn2_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Hor_Btn2_Texcoords; // n=8
        public float[] ScrollBar_Hor_Btn2_FrameOffset; // n=2
        public int[] ScrollBar_Ver_Btn2_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Ver_Btn2_Texcoords; // n=8
        public float[] ScrollBar_Ver_Btn2_FrameOffset; // n=2
        public int[] ScrollBar_Hor_Slider_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Hor_Slider_Texcoords; // n=8
        public float[] ScrollBar_Hor_Slider_FrameOffset; // n=2
        public int[] ScrollBar_Ver_Slider_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Ver_Slider_Texcoords; // n=8
        public float[] ScrollBar_Ver_Slider_FrameOffset; // n=2
        public int[] ScrollBar_Hor_Track_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Hor_Track_Texcoords; // n=8
        public int[] ScrollBar_Ver_Track_MarginsFromTheEdge; // n=4
        public float[] ScrollBar_Ver_Track_Texcoords; // n=8
        public int ScrollBar_BarWidth;

        #endregion

        #region ListViewer

        public int[] ListViewer_MarginsFromTheEdge; // n=4
        public float[] ListViewer_Texcoords; // n=8
        public int[] ListViewer_Selection_MarginsFromTheEdge; // n=4
        public float[] ListViewer_Selection_Texcoords; // n=8
        public int[] ListViewer_Header_MarginsFromTheEdge; // n=4
        public float[] ListViewer_Header_Texcoords; // n=8
        public float[] ListViewer_Header_FrameOffset; // n=2
        public float[] ListViewer_Dibider_Texcoords; // n=4
        public float[] ListViewer_ArrowUp_Texcoords; // n=4
        public float[] ListViewer_ArrowDown_Texcoords; // n=4
        public int[] ListViewer_Arrow_Size; // n=2
        public int ListViewer_Dibider_Width;
        public Color4 ListViewer_Default_BackgroundColor;

        #endregion


        #region CheckBox:
        public int[] CheckBox_MarginsFromTheEdge; // n=4
        public float[] CheckBox_Texcoords; // n=8
        public float[] CheckBox_FrameOffset; // n=2
        #endregion

        #region Menu:
        public int[] Menu_MarginsFromTheEdge; // n=4
        public float[] Menu_Texcoords; // n=8
        public int[] Menu_BordersWidths; // n=4
        public float[] Menu_Border_Texcoords; // n=8
        public float[] Menu_FrameOffset; // n=2
        public float[] Menu_Opened_icon_Texcoords; // n=4
        public float[] Menu_Closed_icon_Texcoords; // n=4
        #endregion

        #region TabPage
        public int[] TabPage_MarginsFromTheEdge_Hor; // n=4
        public int[] TabPage_MarginsFromTheEdge_Ver; // n=4
        public float[] TabPage_Texcoords_Hor; // n=8
        public float[] TabPage_Texcoords_Ver; // n=8
        public float[] TabPage_FrameOffset_Hor; // n=2
        public float[] TabPage_FrameOffset_Ver; // n=2
        public float[] TabPage_X_Texcoords; // n=4
        public float[] TabPage_X_FrameOffset; // n=2
        public int[] TabPage_X_Size; // n=2
        public Color4 TabPage_X_MouseOnColor;
        public float[] TabPage_Plus_Texcoords; // n=4
        public float[] TabPage_Plus_FrameOffset; // n=2
        public int[] TabPage_Plus_Size; // n=2
        public int[] TabPage_Surface_MarginsFromTheEdge; // n=4
        public float[] TabPage_Surface_Texcoords; // n=8
        #endregion

        #region Panel

        public int[] Panel_MarginsFromTheEdge; // n=4
        public float[] Panel_Texcoords; // n=8
        public float[] Panel_BorderInFrameOffset; // n=2
        public float[] Panel_BorderOutFrameOffset; // n=2

        #endregion

        #region NumberBox

        public int[] NumberBox_MarginsFromTheEdge; // n=4
        public float[] NumberBox_Texcoords; // n=8
        public float[] NumberBox_ButtonUpTexcoords; // n=8
        public int[] NumberBox_ButtonUpMarginsFromTheEdge; // n=4
        public float[] NumberBox_ButtonDownTexcoords; // n=8
        public int[] NumberBox_ButtonDownMarginsFromTheEdge; // n=4
        public float[] NumberBox_ButtonUpFrameOffset; // n=2
        public float[] NumberBox_ButtonDownFrameOffset; // n=2
        public int[] NumberBox_ButtonsSize;

        #endregion

        #region TreeViewer

        public int[] TreeViewer_MarginsFromTheEdge; // n=4
        public float[] TreeViewer_Texcoords; // n=8
        public Color4 TreeViewer_DefaultBackgroundColor;
        public float[] TreeViewer_ExpandCollapseButton_Texcoords; // n=4
        public float[] TreeViewer_ExpandCollapseButton_FrameOffset; // n=4
        public int[] TreeViewer_Selection_MarginsFromTheEdge; // n=4
        public float[] TreeViewer_Selection_Texcoords; // n=8

        #endregion

        #region ImagesGallery

        public int[] ImagesGallery_MarginsFromTheEdge; // n=4
        public float[] ImagesGallery_Texcoords; // n=8
        public Color4 ImagesGallery_DefaultBackgroundColor;
        public int[] ImagesGallery__image_MarginsFromTheEdge; // n=4
        public float[] ImagesGallery_image_Texcoords; // n=8
        public float[] ImagesGallery_image_FrameOffset; // n=2
        #endregion

        #region ColorPicker

        public int[] ColorPicker_MarginsFromTheEdge; // n=4
        public float[] ColorPicker_Texcoords; // n=8
        public float[] ColorPicker_CromSelector_Texcoords; // n=4
        public float[] ColorPicker_Picker_Texcoords; // n=4
        public int[] ColorPicker_CromSelectorMargins; // n=2
        public int[] ColorPicker_PickerMargins; // n=2
        public int[] ColorPicker_CromSelectorSize; // n=2
        public int[] ColorPicker_PickerSize; // n=2


        #endregion

        #region MiniAudioPlayer
        
        public int[] AudioPlayer_MarginsFromTheEdge; // n=4
        public float[] AudioPlayer_Texcoords; // n=8
        public int[] AudioPlayer_ButtonsMarginsFromTheEdge; // n=4
        public int[] AudioPlayer_ButtonsSize; // n=2
        public float[] AudioPlayer_PlayButtonTexcoords; // n=4
        public float[] AudioPlayer_PlayButtonOffSet;
        public float[] AudioPlayer_PauseButtonTexcoords; // n=4
        public float[] AudioPlayer_PauseButtonOffSet;
        public float[] AudioPlayer_StopButtonTexcoords; // n=4
        public float[] AudioPlayer_StopButtonOffSet;
        public Color4 AudioPlayer_DefaultTextColor;

        #endregion

        public GuiTheme()
        {

        }

        public dge.G2D.TextureBufferObject ThemeTBO // TBO Visual
        {
            get { return this.tbo_ThemeTBO; }
        }

        public dge.G2D.TextureBufferObject ThemeSltTBO // TBO de siluetas para ID.
        {
            get { return this.tbo_ThemeSltTBO; }
        }

        #region STATIC for DefaultTheme:

        private static GuiTheme p_DefaultGuiTheme;
        public static GuiTheme DefaultGuiTheme
        {
            get
            {
                if (p_DefaultGuiTheme == null)
                {
                    p_DefaultGuiTheme = InitDefaultGuiTheme();
                }
                return p_DefaultGuiTheme;
            }
        }

        private static GuiTheme InitDefaultGuiTheme()
        {
            GuiTheme ret = new GuiTheme();

            ret.tbo_ThemeTBO  = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultTheme.png"), "GuiDefaultTheme");
            ret.tbo_ThemeSltTBO = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultThemeSlt.png"), "GuiDefaultThemeSlt");

            float multHor = 1f/(float)ret.tbo_ThemeTBO.i_width;
            float multVer = 1f/(float)ret.tbo_ThemeTBO.i_height;

            ret.FolderIcon_TexCoords = new float[]
            {
                multHor * 179, multVer * 217,
                multHor * 201, multVer * 239
            };

            ret.FileIcon_TexCoords = new float[]
            {
                multHor * 203, multVer * 217,
                multHor * 225, multVer * 239
            };

            ret.FileImageIcon_TexCoords = new float[]
            {
                multHor * 225, multVer * 193,
                multHor * 246, multVer * 215
            };

            ret.FileSoundIcon_TexCoords = new float[]
            {
                multHor * 225, multVer * 217,
                multHor * 246, multVer * 239
            };

            ret.TransparentBackground_TexCoords = new float[]
            {
                multHor * 193, multVer * 193,
                multHor * 205, multVer * 205
            };

            ret.Default_Font = dge.G2D.Tools.LoadDGFont(Core.LoadEmbeddedResource("dge.dgFonts.LinuxLibertine.dgf"), "dge.dgFonts.LinuxLibertine.dgf");
            ret.Default_FontSize = 12;
            ret.Default_BackgroundColor = new Color4(83,83,83,255);
            ret.Default_ControlsBackgroundColor = new Color4((byte)185, 185, 185, 255);
            ret.Default_TextColor = Color4.Black;
            ret.Default_TextBorderColor = Color4.Black;
            ret.Default_DisableTextColor = Color4.Gray;

            // Window:__________________________________________________________________________
            ret.Window_MarginsFromTheEdge = new int[]{2, 22, 2, 2};
            ret.Window_Texcoords = new float[]
            {
                multHor*2f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*121f, multVer*143f, multVer*163f, multVer*165f
            };
            ret.Window_FrameOffset = new float[]{0f,0f};

            // Dialog:__________________________________________________________________________
            ret.Dialog_Head_MarginsFromTheEdge = new int[]{2, 2, 2, 1};
            ret.Dialog_Head_Texcoords = new float[]
            {
                multHor*205f, multHor*207f, multHor*224f, multHor*227f, 
                multVer*97f, multVer*99f, multVer*118f, multVer*119f
            };
            ret.Dialog_Body_MarginsFromTheEdge = new int[]{2, 1, 2, 2};
            ret.Dialog_Body_Texcoords = new float[]
            {
                multHor*205f, multHor*207f, multHor*224f, multHor*227f, 
                multVer*121f, multVer*122f, multVer*140f, multVer*143f
            };

            // Button:__________________________________________________________________________
            ret.Button_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.Button_Texcoords = new float[]
            {
                multHor*2f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*2f, multVer*3f, multVer*21f, multVer*23f
            };
            ret.Button_FrameOffset = new float[]{multHor*24f, 0f};


            // TrackBar:_________________________________________________________________________
            ret.TrackBar_Hor_MarginsFromTheEdge = new int[]{2, 9, 2, 9};
            ret.TrackBar_Ver_MarginsFromTheEdge = new int[]{9, 2, 9, 2};
            ret.TrackBar_Hor_Texcoords = new float[]
            {
                multHor*58f, multHor*61f, multHor*83f, multHor*86f,
                multVer*2f, multVer*9f, multVer*14f, multVer*23f
            };
            ret.TrackBar_Ver_Texcoords = new float[]
            {
                multHor*121f, multHor*128f, multHor*133f, multHor*143f,
                multVer*22f, multVer*25f, multVer*46f, multVer*49f
            };
            ret.TrackBar_Square_Hor_Slider_Texcoords = new float[]
            {
                multHor*104f, multHor*111f,
                multVer*29f, multVer*43f
            };
            ret.TrackBar_Square_Ver_Slider_Texcoords = new float[]
            {
                multHor*101f, multHor*115f,
                multVer*57f, multVer*64f
            };
            ret.TrackBar_Arrow_Up_Slider_Texcoords = new float[]
            {
                multHor*57f, multHor*64f,
                multVer*29f, multVer*43f
            };
            ret.TrackBar_Arrow_Down_Slider_Texcoords = new float[]
            {
                multHor*81f, multHor*88f,
                multVer*29f, multVer*43f
            };
            ret.TrackBar_Arrow_Left_Slider_Texcoords = new float[]
            {
                multHor*53f, multHor*67f,
                multVer*57f, multVer*64f
            };
            ret.TrackBar_Arrow_Right_Slider_Texcoords = new float[]
            {
                multHor*77f, multHor*90f,
                multVer*57f, multVer*64f
            };
            ret.TrackBar_Slider_Size = new int[]{ 7, 14, 14, 7 };
            ret.TrackBar_Slider_PosMargin = 4;
            ret.TrackBar_Hor_MaxHeight = 22;
            ret.TrackBar_Ver_MaxWidth  = 22;
            ret.TrackBar_FrameOffset = new float[]{0,0};


            // TextBox:__________________________________________________________________________
            ret.TextBox_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.TextBox_Texcoords = new float[]
            {
                multHor*25f, multHor*27f, multHor*45f, multHor*47f, 
                multVer*73f, multVer*75f, multVer*93f, multVer*95f
            };
            ret.TextBox_CursorChar = '|';
            ret.TextBox_Default_BackgroundColor = Color4.White;
            //ret.TextBox_FrameOffset = new float[]{0f,0f};
            

            // ProgressBar:______________________________________________________________________
            ret.ProgressBar_Hor_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ProgressBar_Ver_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ProgressBar_Hor_Texcoords = new float[]
            {
                multHor*1f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*97f, multVer*99f, multVer*117f, multVer*119f
            };
            ret.ProgressBar_Ver_Texcoords = new float[]
            {
                multHor*1f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*97f, multVer*99f, multVer*117f, multVer*119f
            };
            ret.ProgressBar_Hor_Filling_Texcoords = new float[]
            {
                multHor*25f, multVer*97f, multHor*179f, multVer*119f
            };
            ret.ProgressBar_Ver_Filling_Texcoords = new float[]
            {
                multHor*181f, multVer*1f, multHor*203f, multVer*155f
            };

            // InteractiveProgressBar:___________________________________________________________
            ret.InteractiveProgressBar_Hor_MarginsFromTheEdge = new int[]{1, 1, 1, 1};
            ret.InteractiveProgressBar_Ver_MarginsFromTheEdge = new int[]{1, 1, 1, 1};
            ret.InteractiveProgressBar_Hor_Texcoords = new float[]
            {
                multHor*207f, multHor*208f, multHor*215f, multHor*216f, 
                multVer*56f, multVer*57f, multVer*64f, multVer*65f
            };
            ret.InteractiveProgressBar_Ver_Texcoords = new float[]
            {
                multHor*207f, multHor*208f, multHor*215f, multHor*216f, 
                multVer*56f, multVer*57f, multVer*64f, multVer*65f
            };
            ret.InteractiveProgressBar_Hor_Filling_Texcoords = new float[]
            {
                multHor*218f, multVer*57f, multHor*225f, multVer*64f
            };
            ret.InteractiveProgressBar_Ver_Filling_Texcoords = new float[]
            {
                multHor*218f, multVer*57f, multHor*225f, multVer*64f
            };

            // ContentViewer:____________________________________________________________________
            ret.ContentViewer_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ContentViewer_Texcoords = new float[]
            {
                multHor*25f, multHor*27f, multHor*45f, multHor*47f, 
                multVer*121f, multVer*123f, multVer*141f, multVer*143f
            };


            // ScrollBar Hor:____________________________________________________________________
            ret.ScrollBar_Hor_Btn1_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Hor_Btn1_Texcoords = new float[]
            {
                multHor*49f, multHor*51f, multHor*69f, multHor*71f, 
                multVer*73f, multVer*75f, multVer*93f, multVer*95f
            };
            ret.ScrollBar_Hor_Btn1_FrameOffset = new float[] {0, multVer*48};

            ret.ScrollBar_Hor_Btn2_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Hor_Btn2_Texcoords = new float[]
            {
                multHor*121f, multHor*123f, multHor*141f, multHor*143f, 
                multVer*73f, multVer*75f, multVer*93f, multVer*95f
            };
            ret.ScrollBar_Hor_Btn2_FrameOffset = new float[] {0, multVer*48};

            ret.ScrollBar_Hor_Slider_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Hor_Slider_Texcoords = new float[]
            {
                multHor*73f, multHor*75f, multHor*93f, multHor*95f, 
                multVer*73f, multVer*75f, multVer*93f, multVer*95f
            };
            ret.ScrollBar_Hor_Slider_FrameOffset = new float[] {0, multVer*48};

            ret.ScrollBar_Hor_Track_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Hor_Track_Texcoords = new float[]
            {
                multHor*97f, multHor*99f, multHor*117f, multHor*119f, 
                multVer*73f, multVer*75f, multVer*93f, multVer*95f
            };


            // ScrollBar Ver:____________________________________________________________________
            ret.ScrollBar_Ver_Btn1_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Ver_Btn1_Texcoords = new float[]
            {
                multHor*145f, multHor*147f, multHor*165f, multHor*167f, 
                multVer*1f, multVer*3f, multVer*21f, multVer*23f
            };
            ret.ScrollBar_Ver_Btn1_FrameOffset = new float[] {multHor*60, 0};

            ret.ScrollBar_Ver_Btn2_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Ver_Btn2_Texcoords = new float[]
            {
                multHor*145f, multHor*147f, multHor*165f, multHor*167f, 
                multVer*73f, multVer*75f, multVer*93f, multVer*95f
            };
            ret.ScrollBar_Ver_Btn2_FrameOffset = new float[] {multHor*60, 0};

            ret.ScrollBar_Ver_Slider_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Ver_Slider_Texcoords = new float[]
            {
                multHor*145f, multHor*147f, multHor*165f, multHor*167f, 
                multVer*25f, multVer*27f, multVer*45f, multVer*47f
            };
            ret.ScrollBar_Ver_Slider_FrameOffset = new float[] {multHor*60, 0};

            ret.ScrollBar_Ver_Track_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ScrollBar_Ver_Track_Texcoords = new float[]
            {
                multHor*145f, multHor*147f, multHor*165f, multHor*167f, 
                multVer*49f, multVer*51f, multVer*69f, multVer*71f
            };
            ret.ScrollBar_BarWidth = 15;


            // ListViewer:_______________________________________________________________________
            ret.ListViewer_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ListViewer_Texcoords = new float[]
            {
                multHor*25f, multHor*27f, multHor*45f, multHor*47f, 
                multVer*145f, multVer*147f, multVer*165f, multVer*167f
            };
            ret.ListViewer_Selection_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ListViewer_Selection_Texcoords = new float[]
            {
                multHor*101f, multHor*103f, multHor*107f, multHor*110f, 
                multVer*145f, multVer*147f, multVer*151f, multVer*154f
            };
            ret.ListViewer_Header_Texcoords = new float[]
            {
                multHor*49f, multHor*51f, multHor*69f, multHor*71f, 
                multVer*145f, multVer*147f, multVer*165f, multVer*167f
            };
            ret.ListViewer_Header_FrameOffset = new float[]{multHor*24f,0f};
            ret.ListViewer_Dibider_Width = 2;
            ret.ListViewer_Dibider_Texcoords = new float[]
            {
                multHor*97f, multVer*145f,
                multHor*99f, multVer*167f
            };
            ret.ListViewer_ArrowUp_Texcoords = new float[]
            {
                multHor*111f, multVer*158f,
                multHor*119f, multVer*147f
            };
            ret.ListViewer_ArrowDown_Texcoords = new float[]
            {
                multHor*101f, multVer*158f,
                multHor*109f, multVer*147f
            };
            ret.ListViewer_Arrow_Size = new int[]
            {
                8, 9
            };
            ret.ListViewer_Header_MarginsFromTheEdge = new int[] {2, 2, 2, 2};
            ret.ListViewer_Default_BackgroundColor = new Color4(0.95f, 0.95f, 0.95f, 1f);


            // CheckBox:__________________________________________________________________________
            ret.CheckBox_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.CheckBox_Texcoords = new float[]
            {
                multHor*1f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*49f, multVer*51f, multVer*69f, multVer*71f
            };
            ret.CheckBox_FrameOffset = new float[]{multHor*24f, 0f};


            // Menu:__________________________________________________________________________
            ret.Menu_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.Menu_Texcoords = new float[]
            {
                multHor*1f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*169f, multVer*171f, multVer*189f, multVer*191f
            };
            ret.Menu_BordersWidths = new int[]{2, 2, 2, 2};
            ret.Menu_Border_Texcoords = new float[]
            {
                multHor*73f, multHor*75f, multHor*93f, multHor*95f, 
                multVer*217f, multVer*219f, multVer*237f, multVer*239f
            };
            ret.Menu_FrameOffset = new float[]{multHor*24f, 0f};
            ret.Menu_Opened_icon_Texcoords = new float[] {multHor*49f, multVer*217, multHor*71f, multVer*239};
            ret.Menu_Closed_icon_Texcoords = new float[] {multHor*25f, multVer*217f, multHor*47f, multVer*239f};

            // TabPage:__________________________________________________________________________
            ret.TabPage_MarginsFromTheEdge_Hor = new int[]{4, 4, 6, 2};
            ret.TabPage_MarginsFromTheEdge_Ver = new int[]{4, 4, 2, 6};
            ret.TabPage_Texcoords_Hor = new float[]
            {
                multHor*49f, multHor*53f, multHor*64f, multHor*71f, 
                multVer*169f, multVer*173f, multVer*189f, multVer*191f
            };
            ret.TabPage_Texcoords_Ver = new float[]
            {
                multHor*49f, multHor*53f, multHor*66f, multHor*71f, 
                multVer*193f, multVer*197f, multVer*210f, multVer*215f
            };
            ret.TabPage_FrameOffset_Hor = new float[]{multHor*24f, 0f};
            ret.TabPage_FrameOffset_Ver = new float[]{multHor*24f, 0f};
            ret.TabPage_X_Texcoords = new float[] {multHor*97f, multVer*173f, multHor*106f, multVer*182f};
            ret.TabPage_X_FrameOffset = new float[]{multHor*11f, 0f};
            ret.TabPage_X_Size = new int[]{11, 11};
            ret.TabPage_X_MouseOnColor = new Color4(255,180,180,255);
            ret.TabPage_Plus_Texcoords = new float[] {multHor*97f, multVer*197f, multHor*106f, multVer*206f};
            ret.TabPage_Plus_FrameOffset = new float[]{multHor*11f, 0f};
            ret.TabPage_X_Size = new int[]{11, 11};
            ret.TabPage_Surface_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.TabPage_Surface_Texcoords = new float[]
            {
                multHor*97f, multHor*99f, multHor*117f, multHor*119f, 
                multVer*217f, multVer*219f, multVer*237f, multVer*239f
            };

            // Panel:__________________________________________________________________________
            ret.Panel_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.Panel_Texcoords = new float[]
            {
                multHor*1f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*193f, multVer*195f, multVer*213f, multVer*215f
            };
            ret.Panel_BorderInFrameOffset = new float[]{multHor*24f, 0f};
            ret.Panel_BorderOutFrameOffset = new float[]{0f, multVer*24f};

            // NumberBox:__________________________________________________________________________
            ret.NumberBox_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.NumberBox_Texcoords = new float[]
            {
                multHor*121f, multHor*123f, multHor*141f, multHor*143f, 
                multVer*217f, multVer*219f, multVer*237f, multVer*239f
            };

            ret.NumberBox_ButtonUpMarginsFromTheEdge = new int[]{1, 1, 1, 1};
            ret.NumberBox_ButtonUpTexcoords = new float[]
            {
                multHor*145f, multHor*146f, multHor*159f, multHor*160f, 
                multVer*217f, multVer*218f, multVer*226f, multVer*227f
            };
            ret.NumberBox_ButtonUpFrameOffset = new float[]{multHor*17f, 0f};

            ret.NumberBox_ButtonDownMarginsFromTheEdge = new int[]{1, 1, 1, 1};
            ret.NumberBox_ButtonDownTexcoords = new float[]
            {
                multHor*145f, multHor*146f, multHor*159f, multHor*160f, 
                multVer*229f, multVer*230f, multVer*238f, multVer*239f
            };
            ret.NumberBox_ButtonDownFrameOffset = new float[]{multHor*17f, 0f};

            ret.NumberBox_ButtonsSize = new int[] {15, 10};

            // TreeViewer:_________________________________________________________________________
            ret.TreeViewer_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.TreeViewer_Texcoords = new float[]
            {
                multHor*145f, multHor*147f, multHor*165f, multHor*167f, 
                multVer*193f, multVer*195f, multVer*213f, multVer*215f
            };
            ret.TreeViewer_DefaultBackgroundColor = new Color4(0.95f, 0.95f, 0.95f, 1f);
            ret.TreeViewer_ExpandCollapseButton_Texcoords = new float[]
            {
                multHor*169f, multVer*193f, multHor*180f, multVer*204f
            };
            ret.TreeViewer_ExpandCollapseButton_FrameOffset = new float[]
            {
                0f, multVer*11f
            };
            ret.TreeViewer_Selection_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.TreeViewer_Selection_Texcoords = new float[]
            {
                multHor*182f, multHor*184f, multHor*189f, multHor*191f, 
                multVer*193f, multVer*195f, multVer*200f, multVer*202f
            };

            // ImagesGallery:______________________________________________________________________

            ret.ImagesGallery_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ImagesGallery_Texcoords = new float[]
            {
                multHor*121f, multHor*123f, multHor*141f, multHor*143f, 
                multVer*169f, multVer*171f, multVer*189f, multVer*191f
            };
            ret.ImagesGallery_DefaultBackgroundColor = new Color4(0.95f, 0.95f, 0.95f, 1f);
            ret.ImagesGallery__image_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ImagesGallery_image_Texcoords = new float[]
            {
                multHor*145f, multHor*147f, multHor*165f, multHor*167f, 
                multVer*169f, multVer*171f, multVer*189f, multVer*191f
            };
            ret.ImagesGallery_image_FrameOffset = new float[]{multHor*24f, 0f};

            // ColorPicker:______________________________________________________________________

            ret.ColorPicker_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ColorPicker_Texcoords = new float[]
            {
                multHor*193f, multHor*195f, multHor*213f, multHor*215f, 
                multVer*169f, multVer*171f, multVer*189f, multVer*191f
            };
            ret.ColorPicker_CromSelector_Texcoords = new float[]
            {
                multHor*217f, multVer*169f, multHor*233f, multVer*178f
            };
            ret.ColorPicker_Picker_Texcoords = new float[]
            {
                multHor*217f, multVer*180f, multHor*226f, multVer*189f
            };
            ret.ColorPicker_CromSelectorMargins = new int[]{4, 4};
            ret.ColorPicker_PickerMargins = new int[]{5, 5};
            ret.ColorPicker_CromSelectorSize = new int[]{16, 9};
            ret.ColorPicker_PickerSize = new int[]{11, 11};

            // AudioPlayer:______________________________________________________________________

            ret.AudioPlayer_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.AudioPlayer_Texcoords = new float[]
            {
                multHor*229f, multHor*231f, multHor*249f, multHor*251f, 
                multVer*145f, multVer*147f, multVer*165f, multVer*167f
            };

            ret.AudioPlayer_ButtonsSize = new int[]{22, 22};

            ret.AudioPlayer_PlayButtonTexcoords = new float[]
            {
                multHor*229f, multVer*1, multHor*251f, multVer*23
            };
            ret.AudioPlayer_PlayButtonOffSet = new float[]{0f, multVer*24f};
            ret.AudioPlayer_PauseButtonTexcoords = new float[]
            {
                multHor*229f, multVer*48, multHor*251f, multVer*71
            };
            ret.AudioPlayer_PauseButtonOffSet = new float[]{0f, multVer*24f};
            ret.AudioPlayer_StopButtonTexcoords = new float[]
            {
                multHor*229f, multVer*97, multHor*251f, multVer*119
            };
            ret.AudioPlayer_StopButtonOffSet = new float[]{0f, multVer*24f};

            return ret;
        }

        #endregion
    }
}