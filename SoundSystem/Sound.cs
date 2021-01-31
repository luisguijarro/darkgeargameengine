using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    public class Sound
	{
		private uint ui_ID;
		private byte i_Channels;
		//private int i_Bits;
		private int i_Rate;
		private long l_Duration;
		//internal string HASH;
		private string s_FileName;

		public Sound (string filename, byte channels, short[] data, int samplerate)
		{
			this.ui_ID = AL.alGenBuffer();
			this.s_FileName = filename;
			this.i_Channels = channels;
			AL_FORMAT alf = channels > 1? AL_FORMAT.AL_FORMAT_STEREO16 : AL_FORMAT.AL_FORMAT_MONO16;
			AL.alBufferData(this.ui_ID, alf, data, data.Length*sizeof(short), samplerate);

			System.IO.FileInfo fi = new System.IO.FileInfo(filename);
			this.l_Duration = (long)((data.Length*sizeof(short)) / (samplerate * channels * (16/8f)));
			//duration = filesize in bytes / (samplerate * #of channels * (bitspersample/eight))
		}

		public Sound (string filename, byte channels, float[] data, int samplerate)
		{
			this.ui_ID = AL.alGenBuffer();
			this.s_FileName = filename;
			this.i_Channels = channels;
			this.i_Rate = samplerate;
			AL_FORMAT alf = channels > 1? AL_FORMAT.AL_FORMAT_STEREO16 : AL_FORMAT.AL_FORMAT_MONO16;
			AL.alBufferData(this.ui_ID, alf, data, data.Length*sizeof(float), samplerate);

			System.IO.FileInfo fi = new System.IO.FileInfo(filename);
			this.l_Duration = (long)((data.Length*sizeof(float)) / (samplerate * channels * (16/8f)));
		}

		public override string ToString()
		{
			return string.Format("[Sound FileName={0}]", FileName);
		}

		public uint ID
		{
			get { return this.ui_ID; }
		}

		public long Duration
		{
			get { return this.l_Duration;}
		}

		public string FileName
		{
			get { return this.s_FileName;}
		}
		
		public int SampleRate
		{
			get { return this.i_Rate; }
		}
	}
}