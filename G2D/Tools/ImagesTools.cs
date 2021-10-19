using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dge.Tools;

using dgtk.OpenGL;

namespace dge.G2D
{
    public static partial class Tools
    {
        internal static Dictionary<string, TextureBufferObject> d_imageshash = new Dictionary<string, TextureBufferObject>();

        public static TextureBufferObject LoadImage(string filepath)
        {
            string s_hash = FileTools.MD5FromFile(filepath);

            if (d_imageshash.ContainsKey(s_hash))
            {
                return d_imageshash[s_hash];
            }
            d_imageshash.Add(s_hash, p_LoadImage(filepath, s_hash));
            return d_imageshash[s_hash];
        }

        public static TextureBufferObject LoadImage(Stream stream, string name)
        {
            string s_hash = name.GetHashCode().ToString();

            if (d_imageshash.ContainsKey(s_hash))
            {
                return d_imageshash[s_hash];
            }
            d_imageshash.Add(s_hash, p_LoadImage(stream, s_hash, name));
            return d_imageshash[s_hash];
        }

        public static unsafe TextureBufferObject LoadImageFromIntPTr(string name, string hash, int Width, int Height, IntPtr Scan0)
		{
            if (d_imageshash.ContainsKey(hash))
            {
                return d_imageshash[hash];
            }
            d_imageshash.Add(hash, p_LoadImageFromIntPTr(name, Width, Height, Scan0, hash));
            return d_imageshash[hash];
        }

        private static TextureBufferObject p_LoadImage(string filepath, string s_hash)
        {
            TextureBufferObject tbo_ret = new TextureBufferObject();
            if (File.Exists(filepath))
			{
                Bitmap bp = new Bitmap(filepath);
                BitmapData bd = bp.LockBits(new Rectangle(0, 0, bp.Size.Width, bp.Size.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
							
				tbo_ret = p_LoadImageFromIntPTr(filepath, bd.Width, bd.Height, bd.Scan0, s_hash);
							
				bp.UnlockBits(bd);
				bp.Dispose();
				
				return tbo_ret;
            }
            #if DEBUG
            Console.WriteLine("Error: File "+filepath+"not exist.");
            #endif
            return tbo_ret;
        }

        private static TextureBufferObject p_LoadImage(Stream stream, string s_hash, string name)
        {
            TextureBufferObject tbo_ret = new TextureBufferObject();
            if (stream != null)
			{
                Bitmap bp = (Bitmap)Image.FromStream(stream, true, false);
                BitmapData bd = bp.LockBits(new Rectangle(0, 0, bp.Size.Width, bp.Size.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
							
				tbo_ret = p_LoadImageFromIntPTr(name, bd.Width, bd.Height, bd.Scan0, s_hash);
							
				bp.UnlockBits(bd);
				bp.Dispose();
				
				return tbo_ret;
            }
            #if DEBUG
            Console.WriteLine("Error: " + name + " Stream is Null.");
            #endif
            return tbo_ret;
        }

        private static unsafe TextureBufferObject p_LoadImageFromIntPTr(string name, int Width, int Height, IntPtr Scan0, string hash)
		{
            /*bool current = */dgtk.OpenGL.OGL_SharedContext.MakeCurrent();

            UInt32 idret = 0; //stackalloc uint[1];
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

            return new TextureBufferObject(name, Width, Height, idret, hash);
        }
        
		public static bool SaveImage(TextureBufferObject tbo, string filepath)
		{
            IntPtr ptr_data = Marshal.AllocHGlobal(tbo.Width*tbo.Height*4);
			GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, dgtk.OpenGL.PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, ptr_data);
            Bitmap bp = new Bitmap(tbo.Width, tbo.Height, 4*tbo.Width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr_data);
            bp.Save(filepath);
			return true;			
		}
        
        #nullable enable
        #nullable disable warnings
        public static byte[]? GetImageBytes(TextureBufferObject tbo)
        {
            return null;
        }
        
        public static string ImageToBase64String(TextureBufferObject tbo, ImageFormat FileImageFormat)
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
                Bitmap bp = new Bitmap(tbo.Width, tbo.Height, 4 * tbo.Width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr_data);
                
                bp.Save(ms, FileImageFormat);
                bytes = ms.ToArray();
                ms.Close();
                Marshal.FreeHGlobal(ptr_data);
                
                dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
            }
            return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        }
		
		public static bool SaveScreenShot( string filepath)
		{
			MemoryStream ms = new MemoryStream();
			return false;
			
		}
    }
}