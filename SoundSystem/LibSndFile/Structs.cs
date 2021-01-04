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
}