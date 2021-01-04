/*
using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    /// <summary>
	/// Description of EffectSlot.
	/// </summary>
	public class EffectSlot
	{
		private int i_id;
		private Efectos.Efecto ef;
		public EffectSlot()
		{
			this.i_id = EFX.alGenAuxiliaryEffectSlots();			
		}
		~EffectSlot()
		{
			EFX.alDeleteAuxiliaryEffectSlot(this.i_id);
		}
		public void EnlazarEfecto(Efectos.Efecto efecto)
		{
			this.ef = efecto;
			EFX.BindEffectToAuxiliarySlot(this.i_id, efecto.ID_Efecto);
			efecto.Actualizado += delegate(object sender, ENgine.SistemaSonido.Efectos.EfectoActualizadoEventArgs e) 
			{
				FuncionesSonido.EFX.BindEffectToAuxiliarySlot(this.i_id, e.efecto.ID_Efecto);
			};
			//FuncionesSonido.EFX.AuxiliaryEffectSlot(this.i_id, EfxAuxiliaryi.EffectslotEffect, efecto.ID_Efecto);
		}
		public float GananciaSalida
		{
			set { EFX.alAuxiliaryEffectSlot(this.i_id, EfxAuxiliaryf.EffectslotGain, value);}
			get { float ret; EFX.alGetAuxiliaryEffectSlot(this.i_id, EfxAuxiliaryf.EffectslotGain, out ret); return ret;}
		}
		public Efectos.Efecto EfectoEnlazado
		{
			get { return this.ef;}
		}
		public int ID
		{
			get { return this.i_id; }
		}
	}
}
*/