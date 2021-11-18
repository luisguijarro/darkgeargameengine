using System;
using System.IO;
using System.Runtime.InteropServices;

namespace dge.SoundSystem
{
    [StructLayout(LayoutKind.Sequential)]
	internal struct SF_INFO
	{
		public long	frames ;
		public int samplerate ;
		public int channels ;
		public FileFormat format ;
		public int sections ;
		public int seekable ;
		internal bool IsSet
        {
            get { return format != 0 && channels > 0 && samplerate > 0; }
        }
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct SF_VIRTUAL_IO
    {    
		public sf_vio_get_filelen get_filelen;
        public sf_vio_seek seek;
        public sf_vio_read read;
        public sf_vio_write write;
        public sf_vio_tell tell;
    }

	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct VIO_DATA
	{	
		public long offset;
		public long Length;
		//[MarshalAs(UnmanagedType.ByValArray, SizeConst = 524288)]
		public byte*/*[]*/ data;
	} 

	internal delegate long sf_vio_get_filelen (IntPtr user_data);
	internal delegate long sf_vio_seek (long offset, Whence whence, IntPtr user_data);
	internal delegate long sf_vio_read (IntPtr ptr, long count, IntPtr user_data);
	internal delegate long sf_vio_write (IntPtr ptr, long count, IntPtr user_data);
	internal delegate long sf_vio_tell (IntPtr user_data);

}