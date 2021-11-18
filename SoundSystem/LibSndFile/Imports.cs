using System;
using System.Runtime.InteropServices;

namespace dge.SoundSystem
{
    
    internal static class ImportsW
    {

        [DllImport("sndfile.dll", EntryPoint="sf_open")]
		internal static extern IntPtr sf_open(string path, OpenMode Mode, ref SF_INFO sfInfo);

        [DllImport("sndfile.dll", EntryPoint="sf_open_virtual")]
        internal static extern IntPtr sf_open_virtual (ref SF_VIRTUAL_IO sfvirtual, OpenMode mode, ref SF_INFO sfinfo, ref VIO_DATA user_data);

        [DllImport("sndfile.dll", EntryPoint="sf_open_virtual")]
        internal static extern IntPtr sf_open_virtual (ref SF_VIRTUAL_IO sfvirtual, OpenMode mode, ref SF_INFO sfinfo, IntPtr user_data);

        [DllImport("sndfile.dll", EntryPoint="sf_open_virtual")]
        internal static extern IntPtr sf_open_virtual (ref IntPtr sfvirtual, OpenMode mode, ref SF_INFO sfinfo, IntPtr user_data);
		
		[DllImport("sndfile.dll", EntryPoint="sf_close")]
		internal static extern int  sf_close (IntPtr sndfile);
		
		[DllImport("sndfile.dll", EntryPoint="sf_readf_short")]
		internal static unsafe extern long sf_readf_short (IntPtr sndfile, short* data, long frames);

		internal static unsafe long sf_readf_short (IntPtr sndfile, ref short[] data, long frames)
		{
			fixed(short* ptr = data)
			{
				return sf_readf_short (sndfile, ptr, frames);
			}
		}

        [DllImport("sndfile.dll", EntryPoint = "sf_read_short")]
        internal static unsafe extern long sf_read_short(IntPtr sndfile, short* data, long items);

        internal static unsafe long sf_read_short(IntPtr sndfile, ref short[] data, long items)
        {
            fixed (short* ptr = data)
            {
                return sf_read_short(sndfile, ptr, items);
            }
        }

        [DllImport("sndfile.dll", EntryPoint="sf_readf_int")]
		internal static unsafe extern long sf_readf_int (IntPtr sndfile, int* data, long frames);

		internal static unsafe long sf_readf_int (IntPtr sndfile, ref int[] data, long frames)
		{
			fixed(int* ptr = data)
			{
				return sf_readf_int (sndfile, ptr, frames);
			}
		}

        [DllImport("sndfile.dll", EntryPoint = "sf_read_int")]
        internal static unsafe extern long sf_read_int(IntPtr sndfile, int* data, long items);

        internal static unsafe long sf_read_int(IntPtr sndfile, ref int[] data, long items)
        {
            fixed (int* ptr = data)
            {
                return sf_read_int(sndfile, ptr, items);
            }
        }

        [DllImport("sndfile.dll", EntryPoint="sf_readf_float")]
		internal static unsafe extern long sf_readf_float (IntPtr sndfile, float* data, long frames);

		internal static unsafe long sf_readf_float (IntPtr sndfile, ref float[] data, long frames)
		{
			fixed(float* ptr = data)
			{
				return sf_readf_float (sndfile, ptr, frames);
			}
		}

        [DllImport("sndfile.dll", EntryPoint = "sf_read_float")]
        internal static unsafe extern long sf_read_float(IntPtr sndfile, float* data, long items);

        internal static unsafe long sf_read_float(IntPtr sndfile, ref float[] data, long items)
        {
            fixed (float* ptr = data)
            {
                return sf_read_float(sndfile, ptr, items);
            }
        }

        [DllImport("sndfile.dll", EntryPoint="sf_readf_double")]
		internal static unsafe extern long sf_readf_double (IntPtr sndfile, double* data, long frames);

		internal static unsafe long sf_readf_double (IntPtr sndfile, ref double[] data, long frames)
		{
			fixed(double* ptr = data)
			{
				return sf_readf_double (sndfile, ptr, frames);
			}
		}

        [DllImport("sndfile.dll", EntryPoint = "sf_read_double")]
        internal static unsafe extern long sf_read_double(IntPtr sndfile, double* data, long items);

        internal static unsafe long sf_read_double(IntPtr sndfile, ref double[] data, long items)
        {
            fixed (double* ptr = data)
            {
                return sf_read_double(sndfile, ptr, items);
            }
        }

        [DllImport("sndfile.dll", EntryPoint="sf_error")]
		internal static extern Errores sf_error(IntPtr sndfile);
		
		[DllImport("sndfile.dll", EntryPoint="sf_error_number")]
		internal static extern string sf_error_number(int error);
    }

    internal static class ImportsL
    {

        [DllImport("libsndfile.so.1", EntryPoint="sf_open")]
		internal static extern IntPtr sf_open(string path, OpenMode Mode, ref SF_INFO sfInfo);

        [DllImport("libsndfile.so.1", EntryPoint="sf_open_virtual")]
        internal static extern IntPtr sf_open_virtual (ref SF_VIRTUAL_IO sfvirtual, OpenMode mode, ref SF_INFO sfinfo, ref VIO_DATA user_data);

        [DllImport("libsndfile.so.1", EntryPoint="sf_open_virtual")]
        internal static extern IntPtr sf_open_virtual (ref SF_VIRTUAL_IO sfvirtual, OpenMode mode, ref SF_INFO sfinfo, IntPtr user_data);
		
        [DllImport("libsndfile.so.1", EntryPoint="sf_open_virtual")]
        internal static extern IntPtr sf_open_virtual (ref IntPtr sfvirtual, OpenMode mode, ref SF_INFO sfinfo, IntPtr user_data);
		
		[DllImport("libsndfile.so.1", EntryPoint="sf_close")]
		internal static extern int  sf_close (IntPtr sndfile);
		
		[DllImport("libsndfile.so.1", EntryPoint="sf_readf_short")]
		internal static unsafe extern long sf_readf_short (IntPtr sndfile, short* data, long frames);

		internal static unsafe long sf_readf_short (IntPtr sndfile, ref short[] data, long frames)
		{
			fixed(short* ptr = data)
			{
				return sf_readf_short (sndfile, ptr, frames);
			}
		}

        [DllImport("libsndfile.so.1", EntryPoint = "sf_read_short")]
        internal static unsafe extern long sf_read_short(IntPtr sndfile, short* data, long items);

        internal static unsafe long sf_read_short(IntPtr sndfile, ref short[] data, long items)
        {
            fixed (short* ptr = data)
            {
                return sf_read_short(sndfile, ptr, items);
            }
        }

        [DllImport("libsndfile.so.1", EntryPoint="sf_readf_int")]
		internal static unsafe extern long sf_readf_int (IntPtr sndfile, int* data, long frames);

		internal static unsafe long sf_readf_int (IntPtr sndfile, ref int[] data, long frames)
		{
			fixed(int* ptr = data)
			{
				return sf_readf_int (sndfile, ptr, frames);
			}
		}

        [DllImport("libsndfile.so.1", EntryPoint = "sf_read_int")]
        internal static unsafe extern long sf_read_int(IntPtr sndfile, int* data, long items);

        internal static unsafe long sf_read_int(IntPtr sndfile, ref int[] data, long items)
        {
            fixed (int* ptr = data)
            {
                return sf_read_int(sndfile, ptr, items);
            }
        }

        [DllImport("libsndfile.so.1", EntryPoint="sf_readf_float")]
		internal static unsafe extern long sf_readf_float (IntPtr sndfile, float* data, long frames);

		internal static unsafe long sf_readf_float (IntPtr sndfile, ref float[] data, long frames)
		{
			fixed(float* ptr = data)
			{
				return sf_readf_float (sndfile, ptr, frames);
			}
		}

        [DllImport("libsndfile.so.1", EntryPoint = "sf_read_float")]
        internal static unsafe extern long sf_read_float(IntPtr sndfile, float* data, long items);

        internal static unsafe long sf_read_float(IntPtr sndfile, ref float[] data, long items)
        {
            fixed (float* ptr = data)
            {
                return sf_read_float(sndfile, ptr, items);
            }
        }

        [DllImport("libsndfile.so.1", EntryPoint="sf_readf_double")]
		internal static unsafe extern long sf_readf_double (IntPtr sndfile, double* data, long frames);

		internal static unsafe long sf_readf_double (IntPtr sndfile, ref double[] data, long frames)
		{
			fixed(double* ptr = data)
			{
				return sf_readf_double (sndfile, ptr, frames);
			}
		}

        [DllImport("libsndfile.so.1", EntryPoint = "sf_read_double")]
        internal static unsafe extern long sf_read_double(IntPtr sndfile, double* data, long items);

        internal static unsafe long sf_read_double(IntPtr sndfile, ref double[] data, long items)
        {
            fixed (double* ptr = data)
            {
                return sf_read_double(sndfile, ptr, items);
            }
        }

        [DllImport("libsndfile.so.1", EntryPoint="sf_error")]
		internal static extern Errores sf_error(IntPtr sndfile);
		
		[DllImport("libsndfile.so.1", EntryPoint="sf_error_number")]
		internal static extern string sf_error_number(int error);
    }

}