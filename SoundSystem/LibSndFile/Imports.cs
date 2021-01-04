using System;
using System.Runtime.InteropServices;

namespace dge.SoundSystem
{
    internal static class Imports
    {

        [DllImport("libsndfile-1.dll", EntryPoint="sf_open")]
		internal static extern IntPtr sf_open(string path, OpenMode Mode, ref SF_INFO sfInfo);
		
		[DllImport("libsndfile-1.dll", EntryPoint="sf_close")]
		internal static extern int  sf_close (IntPtr sndfile);
		
		[DllImport("libsndfile-1.dll", EntryPoint="sf_readf_short")]
		internal static unsafe extern long sf_readf_short (IntPtr sndfile, short* data, long frames);

		internal static unsafe long sf_readf_short (IntPtr sndfile, ref short[] data, long frames)
		{
			fixed(short* ptr = data)
			{
				return sf_readf_short (sndfile, ptr, frames);
			}
		}
		
		[DllImport("libsndfile-1.dll", EntryPoint="sf_readf_int")]
		internal static unsafe extern long sf_readf_int (IntPtr sndfile, int* data, long frames);

		internal static unsafe long sf_readf_int (IntPtr sndfile, int[] data, long frames)
		{
			fixed(int* ptr = data)
			{
				return sf_readf_int (sndfile, ptr, frames);
			}
		}

		[DllImport("libsndfile-1.dll", EntryPoint="sf_readf_float")]
		internal static unsafe extern long sf_readf_float (IntPtr sndfile, float* data, long frames);

		internal static unsafe long sf_readf_float (IntPtr sndfile, float[] data, long frames)
		{
			fixed(float* ptr = data)
			{
				return sf_readf_float (sndfile, ptr, frames);
			}
		}

		[DllImport("libsndfile-1.dll", EntryPoint="sf_readf_double")]
		internal static unsafe extern long sf_readf_double (IntPtr sndfile, double* data, long frames);

		internal static unsafe long sf_readf_double (IntPtr sndfile, double[] data, long frames)
		{
			fixed(double* ptr = data)
			{
				return sf_readf_double (sndfile, ptr, frames);
			}
		}

		[DllImport("libsndfile-1.dll", EntryPoint="sf_error")]
		internal static extern Errores sf_error(IntPtr sndfile);
		
		[DllImport("libsndfile-1.dll", EntryPoint="sf_error_number")]
		internal static extern string sf_error_number(int error);
    }
}