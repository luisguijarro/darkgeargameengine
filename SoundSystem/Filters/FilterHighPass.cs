using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    /// <summary>
	/// Description of EffectSlot.
	/// </summary>
	public class FilterHighPass : I_Filter
	{
		internal uint ui_ID;

		public FilterHighPass()
		{
			this.ui_ID = EFX.alGenFilter();
			EFX.alFilteri(this.ui_ID, AL_FilterParam.AL_FILTER_TYPE, (int)AL_Filter_Type.AL_FILTER_HIGHPASS);
		}

		#region Parameters:

        public float Gain
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_HIGHPASS_GAIN, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_HIGHPASS_GAIN); }
        }
        
        public float GainLF
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_HIGHPASS_GAINLF, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_HIGHPASS_GAINLF); }
        }        

		#endregion

        public FilterType Filter
        {
            get { return FilterType.HighPass ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}