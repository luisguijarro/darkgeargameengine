using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class FrequencyShifter : C_SoundEffect
    {
        // internal EffectSlot slot;
        // internal uint ui_ID;

        public FrequencyShifter() : base (AL_Effect_Type.AL_EFFECT_FREQUENCY_SHIFTER)
        {
            // this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            // EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_FREQUENCY_SHIFTER);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~FrequencyShifter()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public float Frequency
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_FREQUENCY_SHIFTER_FREQUENCY, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_FREQUENCY_SHIFTER_FREQUENCY); }
        }

        public int Left_Direction
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_FREQUENCY_SHIFTER_LEFT_DIRECTION, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_CHORUS_WAVEFORM); }
        }

        public int Right_Direction
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_FREQUENCY_SHIFTER_RIGHT_DIRECTION, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_FREQUENCY_SHIFTER_RIGHT_DIRECTION); }
        }


        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.FrequencyShifter ;}
        }
/*
        public uint ID
        {
            get { return this.ui_ID;}
        }*/
    }
}