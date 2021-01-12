using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    /// <summary>
	/// Description of EffectSlot.
	/// </summary>
	public class FilterLowPass : I_Filter
	{
		internal uint ui_ID;

		public FilterLowPass()
		{
			this.ui_ID = EFX.alGenFilter();
			EFX.alFilteri(this.ui_ID, AL_FilterParam.AL_FILTER_TYPE, (int)AL_Filter_Type.AL_FILTER_LOWPASS);
		}

		#region Parameters:

        public float Gain
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_LOWPASS_GAIN, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_LOWPASS_GAIN); }
        }
        
        public float GainHF
        {
            set { EFX.alFilterf(this.ui_ID, AL_FilterPassParam.AL_LOWPASS_GAINHF, value); }
            get { return EFX.alGetFilterf(this.ui_ID, AL_FilterPassParam.AL_LOWPASS_GAINHF); }
        }        

		#endregion

        public FilterType Filter
        {
            get { return FilterType.LowPass ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
	
}