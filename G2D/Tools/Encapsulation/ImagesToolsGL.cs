using System;
using System.IO;

using SkiaSharp;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dge.Tools;

using dgtk.OpenGL;

namespace dge.G2D
{
    public static partial class Tools
    {   
        private class ImageToolsGL : I_ImageTools
        {
            public unsafe TextureBufferObject p_LoadImageFromIntPTr(string name, int Width, int Height, IntPtr Scan0, string hash)
            {
                /*bool current = */
                UInt32 idret = 0; //stackalloc uint[1];

                lock(dge.Core.LockObject)
                {
                    dgtk.OpenGL.OGL_Context PrevContext = dgtk.Core.ActualOpenGLContext;
                    dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
                    
                    GL.glEnable(EnableCap.GL_TEXTURE_2D);

                    idret = GL.glGenTexture();
                    GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, idret);
                    GL.glPixelStoref(PixelStoreParameter.GL_UNPACK_ALIGNMENT, 1);

                    GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MIN_FILTER, (int)TextureMinFilter.GL_LINEAR);
                    GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MAG_FILTER, (int)TextureMagFilter.GL_NEAREST);
                    GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_WRAP_S, (int)TextureWrapMode.GL_REPEAT);
                    GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_WRAP_T, (int)TextureWrapMode.GL_REPEAT);

                    GL.glTexEnvi(TextureEnvTarget.GL_TEXTURE_ENV, TextureEnvParameter.GL_TEXTURE_ENV_MODE, (int)TextureEnvMode.GL_REPLACE_EXT);

                    GL.glTexImage2D(TextureTarget.GL_TEXTURE_2D, 0, InternalFormat.GL_RGBA, Width, Height, 0, dgtk.OpenGL.PixelFormat.GL_BGRA, PixelType.GL_UNSIGNED_BYTE, Scan0);

                    GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);

                    GL.glFlush();

                    dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
                    PrevContext.MakeCurrent();
                }

                return new TextureBufferObject(name, Width, Height, idret, hash);
            }

            public bool SaveImage(TextureBufferObject tbo, string filepath)
            {
                lock(dge.Core.LockObject)
                {
                    IntPtr ptr_data = Marshal.AllocHGlobal(tbo.Width*tbo.Height*4);
                    
                    dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
                    GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, dgtk.OpenGL.PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, ptr_data);
                    dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();

                    
                    SKImage img = SKImage.FromPixels(new SKImageInfo(tbo.Width, tbo.Height), ptr_data);
                    Stream file = File.Create(filepath);

                    img.Encode().SaveTo(file);
                    file.Close();
                    //Bitmap bp = new Bitmap(tbo.Width, tbo.Height, 4*tbo.Width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr_data);
                    //bp.Save(filepath);
                }
                return true;			
            }
            
            public byte[] GetImageBytes(TextureBufferObject tbo)
            {
                byte[] bytes;
                lock(dge.Core.LockObject)
                {
                    MemoryStream ms = new MemoryStream();
                    if (!dgtk.OpenGL.OGL_SharedContext.MakeCurrent())
                    {
                        Console.WriteLine("MAKECURRENT() FAIL IN ImageToBase64String METHOD.");
                    }
                    IntPtr ptr_data = Marshal.AllocHGlobal(tbo.Width * tbo.Height * 4);
                    GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tbo.ui_ID);
                    GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, dgtk.OpenGL.PixelFormat.GL_BGRA, PixelType.GL_UNSIGNED_BYTE, ptr_data);
                    //Bitmap bp = new Bitmap(tbo.Width, tbo.Height, 4 * tbo.Width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr_data);
                    
                    SKImage img = SKImage.FromPixels(new SKImageInfo(tbo.Width, tbo.Height), ptr_data);

                    img.Encode().SaveTo(ms);

                    //bp.Save(ms, FileImageFormat);
                    bytes = ms.ToArray();
                    ms.Close();
                    Marshal.FreeHGlobal(ptr_data);
                    
                    dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
                }
                return bytes;
            }

            public bool SaveScreenShot( string filepath, dgWindow win)
            {
                lock(win.LockObject)
                {
                    win.MakeCurrent();
                    GL.glReadBuffer(ReadBufferMode.GL_FRONT);

                    IntPtr ptr_data = Marshal.AllocHGlobal(4*win.Width*win.Height);
                    GL.glReadPixels(0, 0, win.Width, win.Height, dgtk.OpenGL.PixelFormat.GL_BGRA, PixelType.GL_UNSIGNED_BYTE, ptr_data);
                    win.UnMakeCurrent();

                    SKImage img = SKImage.FromPixels(new SKImageInfo(win.Width, win.Height, SKColorType.Bgra8888, SKAlphaType.Opaque), ptr_data, 4*win.Width);
                    SKBitmap bmp = SKBitmap.FromImage(img);
                    SKBitmap fliped = new SKBitmap(bmp.Width, bmp.Height, true);
                    using (SKCanvas canvas = new SKCanvas(fliped))
                    {
                        canvas.Scale(1, -1, 0, bmp.Height/2f);
                        canvas.DrawBitmap(bmp, 0, 0);
                    }
                    bmp = fliped;               

                    if (filepath.Length>4)
                    {
                        if (filepath.Substring(filepath.Length-4, 4)!=".png")
                        {
                            filepath += ".png";
                        }
                    }

                    Stream fl = File.Create(filepath);

                    bmp.Encode(fl, SKEncodedImageFormat.Png, 100);
                    
                    fl.Close();

                    img.Dispose();
                    bmp.Dispose();
                    Marshal.FreeHGlobal(ptr_data);
                }

                return true;			
            }
        }
    }
}