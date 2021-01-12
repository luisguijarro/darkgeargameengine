using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class PitchShifter : I_SoundEffect
    {
        internal uint ui_ID;

        public PitchShifter()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_PITCH_SHIFTER);
        }
        ~PitchShifter()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public int Coarse_Tune
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_COARSE_TUNE, (int)value); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_COARSE_TUNE); }
        }

        public int Fine_Tune
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_FINE_TUNE, (int)value); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_FINE_TUNE); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.PitchShifter ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}