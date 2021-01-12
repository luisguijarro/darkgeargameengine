using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    /// <summary>
	/// Description of EffectSlot.
	/// </summary>
	public class FilterBandPass : I_Filter
	{
		internal uint ui_ID;

		public FilterBandPass()
		{
			this.ui_ID = EFX.alGenFilter();
			EFX.alFilteri(this.ui_ID, AL_FilterParam.AL_FILTER_TYPE, (int)AL_Filter_Type.AL_FILTER_BANDPASS);
		}

		#region Parameters:

        public float Gain
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_BANDPASS_GAIN, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_BANDPASS_GAIN); }
        }
        
        public float GainLF
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_BANDPASS_GAINLF, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_BANDPASS_GAINLF); }
        }        

        public float GainHF
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_BANDPASS_GAINHF, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_BANDPASS_GAINHF); }
        }

		#endregion

        public FilterType Filter
        {
            get { return FilterType.BandPass ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}