using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class RingModulator : I_SoundEffect
    {
        internal uint ui_ID;

        public RingModulator()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_RING_MODULATOR);
        }
        ~RingModulator()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:



        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.RingModulator ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}