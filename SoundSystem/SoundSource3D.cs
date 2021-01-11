using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    /// <summary>
	/// Description of Emisor3D.
	/// </summary>
	public class SoundSource3D
	{
		private uint id;
		private float x, y, z;
		private Sound snd;
		//private Dictionary<int, AuxEfectSlot> Slots;
		//public event EventHandler<SonidoEventArgs> Situacion;
		internal SoundSource3D()
		{
			AL.alGenSource(out this.id);
			this.Loop = false;
			//this.Slots = new Dictionary<int, AuxEfectSlot>();
			//this.Situacion += delegate { };
			//FuncionesSonido.UpdateSnd += FuncionesSonido_UpdateSnd;
		}
		~SoundSource3D()
		{
			AL.alDeleteSource(ref this.id);
			//FuncionesSonido.UpdateSnd -= FuncionesSonido_UpdateSnd;
		}

		void UpdateSnd(object sender, EventArgs e)
		{
			//try{this.Situacion(this, new SonidoEventArgs(this.Duracion, this.Tiempo, this.MenosTiempo, this.Estado));}
			//catch{}
		}
		
		#region METODOS PUBLICOS:
		public void Play()
		{
			AL.alSourcePlay(this.id);
		}
		public void Pause()
		{
			AL.alSourcePause(this.id);
		}
		public void Stop()
		{
			AL.alSourceStop(this.id);
		}
		public void AssignSound(Sound sound)
		{
			this.snd = sound; AL.alSourcei(this.id, AL_SourceiParam.AL_BUFFER, (int)sound.IDBuffer);
		}
		public unsafe bool HaveSound()
		{
            int[] val = new int[1];
            fixed(int* ptr_val = val)
            {
                AL.alGetSourcei(this.id, AL_GetSourceiParam.AL_SOURCE_TYPE, ptr_val);
            }
            
			if ((AL_SourceType)val[0] == AL_SourceType.AL_UNDETERMINED) { return false;}else{return true;}
		}
        /*
		public int CrearSlot()
		{
			AuxEfectSlot aes = new AuxEfectSlot();
			this.Slots.Add(aes.ID, aes);
			AL.Source(this.id, ALSourcei.EfxDirectFilter, aes.ID);
			FuncionesSonido.EFX.BindSourceToAuxiliarySlot(this.id, (uint)aes.ID, 1, 0);
			return aes.ID;
		}
		public AuxEfectSlot GetSlot(int id)
		{
			if (this.Slots.ContainsKey(id))
			{
				return this.Slots[id];
			}
			throw new Exception("Emisor "+this.id+": No existe Slot con ID "+id+" en este emisor.");
		}
		public bool EliminarSlot(int id)
		{
			if (this.Slots.ContainsKey(id))
			{
				this.Slots.Remove(id);
				return true;
			}
			return false;
		}
		public void EnlazarEfectoSlot(Efectos.Reverb efecto, AuxEfectSlot slot)
		{
			slot.EnlazarEfecto(efecto);
		}
		public void EnlazarEfectoSlot(Efectos.Reverb efecto, int id_slot)
		{
			if (this.Slots.ContainsKey(id_slot))
			{
				this.Slots[id_slot].EnlazarEfecto(efecto);
			}
		}
        */
		#endregion
		
		#region METODOS PRIVADOS:
		private void UpdatePos()
		{
			AL.alSource3f(this.id, AL_Source3Param.AL_POSITION, x, y, z);
			AL.alSource3f(this.id, AL_Source3Param.AL_VELOCITY, 0, 0, 0);
		}
		#endregion
		
		#region PROPIEDADES:
		public float PositionX
		{
			set { this.x = value; this.UpdatePos();}
			get { return this.x;}
		}
		public float PositionY
		{
			set { this.y = value; this.UpdatePos();}
			get { return this.y;}
		}
		public float PositionZ
		{
			set { this.z = value; this.UpdatePos();}
			get { return this.z;}
		}
		public float ReferenceDistance
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_REFERENCE_DISTANCE, value); }
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_REFERENCE_DISTANCE); }
		}
		public float MaxDistance
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_MAX_DISTANCE, value); }
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_MAX_DISTANCE); }
		}
		public bool Loop
		{
			set { AL.alSourceb(this.id, AL_SourcebParam.AL_LOOPING, value);}
			get { return AL.alGetSourceb(this.id, AL_SourcebParam.AL_LOOPING); }
		}
		public AL_SourceState State
		{
			get 
			{ 
				return AL.alGetSourceState(this.id);				
			}
		}
		public float pitch
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_PITCH, value);}
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_PITCH); }
		}
		public float Gain
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_GAIN, value);}
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_GAIN); }
		}
		public float MaxGain
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_MAX_GAIN, value);}
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_MAX_GAIN); }
		}
		public float MinGain
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_MIN_GAIN, value);}
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_MIN_GAIN); }
		}
		public float RolloffFactor
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_ROLLOFF_FACTOR, value); }
			get { return AL.alGetSourcef(this.id, AL_SourcefParam.AL_ROLLOFF_FACTOR);}
		}
		public DateTime Time
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_SEC_OFFSET, value.Ticks); }
			get { float ret = AL.alGetSourcef(this.id, AL_SourcefParam.AL_SEC_OFFSET); return new DateTime((TimeSpan.FromSeconds(ret)).Ticks);}
		}
		public long TimeTicks
		{
			set { AL.alSourcef(this.id, AL_SourcefParam.AL_SEC_OFFSET, (float)TimeSpan.FromTicks(value).TotalSeconds);}
			get { float ret = AL.alGetSourcef(this.id, AL_SourcefParam.AL_SEC_OFFSET); return (TimeSpan.FromSeconds(ret)).Ticks;}
		}
		public DateTime Duration
		{
			get { TimeSpan dur = TimeSpan.FromSeconds(this.snd.Duration); return new DateTime(dur.Ticks);}
		}
		public DateTime TimeRemaining
		{
			get { return new DateTime((this.Duration.Ticks - this.Time.Ticks));}
		}
		#endregion
	}
}