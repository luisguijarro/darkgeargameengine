using System;

namespace dge.SoundSystem
{
    public class Sound
	{
		public uint IDBuffer;
		public int Channels;
		public int Bits;
		public int Rate;
		public float Duration;
		public string HASH;
		public string FileName;
		public override string ToString()
		{
			return string.Format("[Sound FileName={0}]", FileName);
		}
	}
}