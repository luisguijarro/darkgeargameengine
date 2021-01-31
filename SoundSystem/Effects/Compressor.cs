using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class Compressor : C_SoundEffect
    {
        // internal EffectSlot slot;
        // internal uint ui_ID;

        public Compressor() : base(AL_Effect_Type.AL_EFFECT_COMPRESSOR)
        {
            // this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            // EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_COMPRESSOR);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~Compressor()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public bool OnOff
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_COMPRESSOR_ONOFF, value? 1 : 0); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_COMPRESSOR_ONOFF) == 1; }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.Compressor ;}
        }
/*
        public uint ID
        {
            get { return this.ui_ID;}
        }*/
    }
}