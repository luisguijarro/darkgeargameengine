using System;
using dgtk.Math;

using dge.G2D;

namespace dge.GUI
{
    public class GuiTheme
    {
        internal dge.G2D.TextureBufferObject ThemeTBO; // TBO Visual
        internal dge.G2D.TextureBufferObject ThemeSltTBO; // TBO de siluetas para ID.

        internal dgFont DefaultFont;

        #region Ventanas:
        internal int[] Window_MarginsFromTheEdge; // n=4
        internal float[] Window_Texcoords; // n=8
        internal float[] Window_FrameOffset; //n=2
        #endregion

        #region Botones:
        internal int[] Button_MarginsFromTheEdge; // n=4
        internal float[] Button_Texcoords; // n=8
        internal float[] Button_FrameOffset; // n=2
        #endregion

        #region TrackBar:
        internal int[] TrackBar_Hor_MarginsFromTheEdge; // n=4
        internal int[] TrackBar_Ver_MarginsFromTheEdge; // n=4
        internal float[] TrackBar_Hor_Texcoords; // n=8
        internal float[] TrackBar_Ver_Texcoords; // n=8
        internal float[] TrackBar_Square_Hor_Slider_Texcoords; // n=4
        internal float[] TrackBar_Square_Ver_Slider_Texcoords; // n=4
        internal float[] TrackBar_Arrow_Up_Slider_Texcoords; // n=4
        internal float[] TrackBar_Arrow_Down_Slider_Texcoords; // n=4
        internal float[] TrackBar_Arrow_Left_Slider_Texcoords; // n=4
        internal float[] TrackBar_Arrow_Right_Slider_Texcoords; // n=4
        internal float[] TrackBar_FrameOffset; // n=2
        internal int[] TrackBar_Slider_Size; // (Ancho, Alto)Hor, (Ancho, Alto)Ver
        internal int TrackBar_Slider_PosMargin;
        internal int TrackBar_Hor_MaxHeight;
        internal int TrackBar_Ver_MaxWidth;
        #endregion

        #region TextBox:
        internal int[] TextBox_MarginsFromTheEdge; // n=4
        internal float[] TextBox_Texcoords; // n=8
        //internal float[] TextBox_FrameOffset; // n=2
        internal char TextBox_CursorChar;
        #endregion

        #region ProgressBar

        internal int[] ProgressBar_Hor_MarginsFromTheEdge; // n=4
        internal int[] ProgressBar_Ver_MarginsFromTheEdge; // n=4
        internal float[] ProgressBar_Hor_Texcoords; // n=8
        internal float[] ProgressBar_Ver_Texcoords; // n=8
        internal float[] ProgressBar_Hor_Filling_Texcoords; // n=4
        internal float[] ProgressBar_Ver_Filling_Texcoords; // n=4

        #endregion

        #region ContentViewer

        internal int[] ContentViewer_MarginsFromTheEdge; // n=4
        internal float[] ContentViewer_Texcoords; // n=8

        #endregion

        #region ScrollBar

        internal int[] ScrollBar_Hor_Btn1_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Hor_Btn1_Texcoords; // n=8
        internal float[] ScrollBar_Hor_Btn1_FrameOffset; // n=2
        internal int[] ScrollBar_Ver_Btn1_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Ver_Btn1_Texcoords; // n=8
        internal float[] ScrollBar_Ver_Btn1_FrameOffset; // n=2
        internal int[] ScrollBar_Hor_Btn2_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Hor_Btn2_Texcoords; // n=8
        internal float[] ScrollBar_Hor_Btn2_FrameOffset; // n=2
        internal int[] ScrollBar_Ver_Btn2_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Ver_Btn2_Texcoords; // n=8
        internal float[] ScrollBar_Ver_Btn2_FrameOffset; // n=2
        internal int[] ScrollBar_Hor_Slider_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Hor_Slider_Texcoords; // n=8
        internal float[] ScrollBar_Hor_Slider_FrameOffset; // n=2
        internal int[] ScrollBar_Ver_Slider_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Ver_Slider_Texcoords; // n=8
        internal float[] ScrollBar_Ver_Slider_FrameOffset; // n=2
        internal int[] ScrollBar_Hor_Track_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Hor_Track_Texcoords; // n=8
        internal int[] ScrollBar_Ver_Track_MarginsFromTheEdge; // n=4
        internal float[] ScrollBar_Ver_Track_Texcoords; // n=8
        internal int ScrollBar_BarWidth;

        #endregion

        #region ListViewer

        internal int[] ListViewer_MarginsFromTheEdge; // n=4
        internal float[] ListViewer_Texcoords; // n=8
        internal int[] ListViewer_Header_MarginsFromTheEdge; // n=4
        internal float[] ListViewer_Header_Texcoords; // n=8
        internal float[] ListViewer_Header_FrameOffset; // n=2
        internal float[] ListViewer_Dibider_Texcoords; // n=4
        internal int ListViewer_Dibider_Width;

        #endregion

        public GuiTheme()
        {

        }

        #region STATIC for DefaultTheme:

        private static GuiTheme p_DefaultGuiTheme;
        internal static GuiTheme DefaultGuiTheme
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

            ret.ThemeTBO  = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultTheme.png"), "GuiDefaultTheme");
            ret.ThemeSltTBO = dge.G2D.Tools.LoadImage(Core.LoadEmbeddedResource("dge.images.GuiDefaultThemeSlt.png"), "GuiDefaultThemeSlt");
            ret.DefaultFont = dge.G2D.Tools.LoadDGFont(Core.LoadEmbeddedResource("dge.dgFonts.LinuxLibertine.dgf"), "dge.dgFonts.LinuxLibertine.dgf");

            float multHor = 1f/(float)ret.ThemeTBO.ui_width;
            float multVer = 1f/(float)ret.ThemeTBO.ui_height;

            // Window:__________________________________________________________________________
            ret.Window_MarginsFromTheEdge = new int[]{2, 22, 2, 2};
            ret.Window_Texcoords = new float[]
            {
                multHor*2f, multHor*3f, multHor*21f, multHor*23f, 
                multVer*121f, multVer*143f, multVer*163f, multVer*165f
            };
            ret.Window_FrameOffset = new float[]{0f,0f};

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
                multHor*54f, multHor*66f,
                multVer*60f, multVer*42f
            };
            ret.TrackBar_Arrow_Down_Slider_Texcoords = new float[]
            {
                multHor*78f, multHor*90f,
                multVer*60f, multVer*42f
            };
            ret.TrackBar_Arrow_Left_Slider_Texcoords = new float[]
            {
                multHor*53f, multHor*66f,
                multVer*54f, multVer*66f
            };
            ret.TrackBar_Arrow_Right_Slider_Texcoords = new float[]
            {
                multHor*77f, multHor*90f,
                multVer*54f, multVer*66f
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
            ret.ScrollBar_BarWidth = 22;


            // ListViewer:_______________________________________________________________________
            ret.ListViewer_MarginsFromTheEdge = new int[]{2, 2, 2, 2};
            ret.ListViewer_Texcoords = new float[]
            {
                multHor*25f, multHor*27f, multHor*45f, multHor*47f, 
                multVer*145f, multVer*147f, multVer*165f, multVer*167f
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
                multHor*99f, multVer*147f
            };
            ret.ListViewer_Header_MarginsFromTheEdge = new int[] {2, 2, 2, 2};



            return ret;
        }

        #endregion
    }
}