using System;
using System.Collections.Generic;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    /// <summary>
	/// Description of EffectSlot.
	/// </summary>
	public class EffectSlot
	{
		private uint ui_ID;
		private C_SoundEffect se_effect;
		private Dictionary<uint, SoundSource3D> SourcesLinked;
		private Dictionary<uint, I_Filter> FiltersLinked;
		public EffectSlot()
		{
			this.ui_ID = EFX.alGenAuxiliaryEffectSlot();			
		}
		~EffectSlot()
		{
			EFX.alDeleteAuxiliaryEffectSlot(this.ui_ID);
		}

		#region Public Methods

		public void LinkSourceToInput(SoundSource3D source)
		{
			if (!this.SourcesLinked.ContainsKey(source.ID))
			{
				this.SourcesLinked.Add(source.ID, source);
				//OpenAL Code;
			}
		}

		public void UnLinkSourceToInput(SoundSource3D source)
		{
			if (this.SourcesLinked.ContainsKey(source.ID))
			{
				this.SourcesLinked.Remove(source.ID);
				//OpenAL Code;
			}
		}

		public void AttachEffect(C_SoundEffect effect)
		{
			this.se_effect = effect;
			Type efxtype = effect.GetType();
			//OpenAL Code;
			//effect.slot = this;
			EFX.alAuxiliaryEffectSloti(this.ui_ID, AL_AuxiliaryEffectSlot.AL_EFFECTSLOT_EFFECT, (int)effect.ID);
		}

		#endregion
		
		#region PROPERTIES:

		public float Gain
		{
			set { EFX.alAuxiliaryEffectSlotf(this.ui_ID, AL_AuxiliaryEffectSlot.AL_EFFECTSLOT_GAIN, value); }
			get { return EFX.alGetAuxiliaryEffectSlotf(this.ui_ID, AL_AuxiliaryEffectSlot.AL_EFFECTSLOT_GAIN); }
		}

		public bool SendAuto
		{
			set { EFX.alAuxiliaryEffectSlotf(this.ui_ID, AL_AuxiliaryEffectSlot.AL_EFFECTSLOT_AUXILIARY_SEND_AUTO, value? 1 : 0); }
			get { return EFX.alGetAuxiliaryEffectSlotf(this.ui_ID, AL_AuxiliaryEffectSlot.AL_EFFECTSLOT_AUXILIARY_SEND_AUTO) == 1; }
		}

		public C_SoundEffect EffectAttached
		{
			get { return this.se_effect; }
		}

		#endregion

		public uint ID
		{
			get { return this.ui_ID; }
		}
	}
}