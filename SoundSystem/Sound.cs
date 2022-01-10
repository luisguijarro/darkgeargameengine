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
		private long l_DurationM;
		//internal string HASH;
		private string s_FileName;

		private int i_bitrate;

		public Sound (string filename, byte channels, short[] data, int samplerate)
		{
			this.ui_ID = AL.alGenBuffer();
			this.s_FileName = filename;
			this.i_Channels = channels;
			this.i_Rate = samplerate;
			AL_FORMAT alf = channels > 1? AL_FORMAT.AL_FORMAT_STEREO16 : AL_FORMAT.AL_FORMAT_MONO16;
			AL.alBufferData(this.ui_ID, alf, data, data.Length*sizeof(short), samplerate);

			int bits;
			AL.alGetBufferi(this.ui_ID, AL_BufferParam.AL_BITS, out bits);

			this.i_bitrate = samplerate * bits * channels;

			//System.IO.FileInfo fi = new System.IO.FileInfo(filename);
			this.l_DurationM = (long)((float)(((sizeof(short)*data.Length) * 8f ) / (float)this.i_bitrate) * 1000f);
			this.l_Duration = (long)(float)(((sizeof(short)*data.Length) * 8f ) / (float)this.i_bitrate);
		}

		public Sound (string filename, byte channels, float[] data, int samplerate)
		{
			this.ui_ID = AL.alGenBuffer();
			this.s_FileName = filename;
			this.i_Channels = channels;
			this.i_Rate = samplerate;
			AL_FORMAT alf = channels > 1? AL_FORMAT.AL_FORMAT_STEREO_FLOAT32 : AL_FORMAT.AL_FORMAT_MONO_FLOAT32;
			AL.alBufferData(this.ui_ID, alf, data, data.Length*sizeof(float), samplerate);

			int bits;
			AL.alGetBufferi(this.ui_ID, AL_BufferParam.AL_BITS, out bits);

			this.i_bitrate = samplerate * bits * channels;

			//System.IO.FileInfo fi = new System.IO.FileInfo(filename);
			this.l_DurationM = (long)((float)(((sizeof(float)*data.Length) * 8f ) / (float)this.i_bitrate) * 1000);
			this.l_Duration = (long)(float)(((sizeof(float)*data.Length) * 8f ) / (float)this.i_bitrate);
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

		public long DurationMilliseconds
		{
			get { return this.l_DurationM;}
		}

		public string FileName
		{
			get { return this.s_FileName;}
		}
		
		public int SampleRate
		{
			get { return this.i_Rate; }
		}

		public int BitRate
		{
			get { return this.i_bitrate; }
		}
	}
}