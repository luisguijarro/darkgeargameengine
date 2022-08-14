using System;
using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
using SkiaSharp;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dge.Tools;

using dgtk.OpenGL;

namespace dge.G2D
{
    interface I_ImageTools
    {
        unsafe TextureBufferObject p_LoadImageFromIntPTr(string name, int Width, int Height, IntPtr Scan0, string hash);

		bool SaveImage(TextureBufferObject tbo, string filepath);
        
        byte[] GetImageBytes(TextureBufferObject tbo);
        		
		bool SaveScreenShot( string filepath, dgWindow win);
    }
}