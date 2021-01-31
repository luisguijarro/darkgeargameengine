using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class PitchShifter : C_SoundEffect
    {
        // internal EffectSlot slot;
        // internal uint ui_ID;

        public PitchShifter() : base (AL_Effect_Type.AL_EFFECT_PITCH_SHIFTER)
        {
            // this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            // EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_PITCH_SHIFTER);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~PitchShifter()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public int Coarse_Tune
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_COARSE_TUNE, (int)value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_COARSE_TUNE); }
        }

        public int Fine_Tune
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_FINE_TUNE, (int)value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_PITCH_SHIFTER_FINE_TUNE); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.PitchShifter ;}
        }
/*
        public uint ID
        {
            get { return this.ui_ID;}
        }*/
    }
}