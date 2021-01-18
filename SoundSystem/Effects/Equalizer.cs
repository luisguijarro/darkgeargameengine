using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class Equalizer : I_SoundEffect
    {
        internal EffectSlot slot;
        internal uint ui_ID;

        public Equalizer()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_EQUALIZER);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~Equalizer()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public float Low_Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_LOW_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_LOW_GAIN); }
        }

        public float Low_Cutoff
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_LOW_CUTOFF, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_LOW_CUTOFF); }
        }

        public float Mid1_Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID1_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID1_GAIN); }
        }

        public float Mid1_Center
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID1_CENTER, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID1_CENTER); }
        }

        public float Mid1_Width
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID1_WIDTH, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID1_WIDTH); }
        }

        public float Mid2_Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID2_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID2_GAIN); }
        }

        public float Mid2_Center
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID2_CENTER, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID2_CENTER); }
        }

        public float Mid2_Width
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID2_WIDTH, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_MID2_WIDTH); }
        }

        public float High_Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_LOW_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_LOW_GAIN); }
        }

        public float High_Cutoff
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_HIGH_CUTOFF, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_EQUALIZER_HIGH_CUTOFF); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.Equalizer ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}