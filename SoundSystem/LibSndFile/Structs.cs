using System;
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
		internal sf_vio_get_filelen get_filelen ;
        internal sf_vio_seek seek ;
        internal sf_vio_read read ;
        internal sf_vio_write write ;
        internal sf_vio_tell tell ;
    }

	delegate long sf_vio_get_filelen (IntPtr user_data) ;
	delegate long  sf_vio_seek (long offset, int whence, IntPtr user_data) ;
	delegate long  sf_vio_read (IntPtr ptr, long count, IntPtr user_data) ;
	delegate long  sf_vio_write (IntPtr ptr, long count, IntPtr user_data) ;
	delegate long  sf_vio_tell (IntPtr user_data) ;

}