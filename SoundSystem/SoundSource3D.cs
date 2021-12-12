using System;
using System.Collections.Generic;
using dgtk.OpenAL;


namespace dge.SoundSystem
{
    /// <summary>
	/// Description of Emisor3D.
	/// </summary>
	public class SoundSource3D
	{
		private uint ui_ID;
		private float x, y, z;
		private Sound snd;

		private Dictionary<uint,I_Filter> FiltersLinked;
		private I_Filter LinkedDirecFilter; // Solo se puede enlazar un filtro en modo directo.
		public SoundSource3D()
		{
			this.ui_ID = AL.alGenSource();
			this.FiltersLinked = new Dictionary<uint, I_Filter>();
			this.Loop = false;
		}
		~SoundSource3D()
		{
			AL.alDeleteSource(ref this.ui_ID);
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
			AL.alSourcePlay(this.ui_ID);
		}
		public void Pause()
		{
			AL.alSourcePause(this.ui_ID);
		}
		public void Stop()
		{
			AL.alSourceStop(this.ui_ID);
		}
		public bool AssignSound(Sound sound)
		{
			if (sound != null)
			{
				this.snd = sound; AL.alSourcei(this.ui_ID, AL_SourceiParam.AL_BUFFER, (int)sound.ID);
				return true;
			}
			return false;
		}
		public unsafe bool HaveSound()
		{
            int[] val = new int[1];
            fixed(int* ptr_val = val)
            {
                AL.alGetSourcei(this.ui_ID, AL_GetSourceiParam.AL_SOURCE_TYPE, ptr_val);
            }
            
			if ((AL_SourceType)val[0] == AL_SourceType.AL_UNDETERMINED) { return false;}else{return true;}
		}

		public void LinkFilterToSlot(I_Filter filter, EffectSlot slot)
		{
			if (this.FiltersLinked.ContainsKey(filter.ID))
			{
				//UnLinkFilter(filter);
				// Actualizar Filtro. No hace falta código.
			}
			else
			{
				this.FiltersLinked.Add(filter.ID, filter);
			}

			//OpenAL Code;
			AL.alSource3i(this.ui_ID, AL_Source3Param.AL_AUXILIARY_SEND_FILTER, (int)slot.ID, 0, (int)filter.ID);
		}
		
		public void UnLinkFilterToSlot(I_Filter filter, EffectSlot slot)
		{
			if (this.FiltersLinked.ContainsKey(filter.ID))
			{
				//OpenAL Code;
				AL.alSource3i(this.ui_ID, AL_Source3Param.AL_AUXILIARY_SEND_FILTER, (int)dgtk.OpenAL.ALEnum.AL_EFFECTSLOT_NULL, 0, (int)dgtk.OpenAL.ALEnum.AL_FILTER_NULL);
			}
		}		

		public void LinkDirectFilter (I_Filter filter)
		{
			this.LinkedDirecFilter = filter;
			AL.alSourcei(this.ui_ID, AL_SourceiParam.AL_DIRECT_FILTER, (int)filter.ID);
		}

		public void UnlinkDirectFilter ()
		{
			this.LinkedDirecFilter = null;
			AL.alSourcei(this.ui_ID, AL_SourceiParam.AL_DIRECT_FILTER, (int)dgtk.OpenAL.ALEnum.AL_FILTER_NULL);
		}

		#endregion
		
		#region METODOS PRIVADOS:
		private void UpdatePos()
		{
			AL.alSource3f(this.ui_ID, AL_Source3Param.AL_POSITION, x, y, z);
			AL.alSource3f(this.ui_ID, AL_Source3Param.AL_VELOCITY, 0, 0, 0);
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
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_REFERENCE_DISTANCE, value); }
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_REFERENCE_DISTANCE); }
		}
		public float MaxDistance
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_MAX_DISTANCE, value); }
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_MAX_DISTANCE); }
		}
		public bool Loop
		{
			set { AL.alSourceb(this.ui_ID, AL_SourcebParam.AL_LOOPING, value);}
			get { return AL.alGetSourceb(this.ui_ID, AL_SourcebParam.AL_LOOPING); }
		}
		public AL_SourceState State
		{
			get 
			{ 
				return AL.alGetSourceState(this.ui_ID);				
			}
		}
		public float Pitch
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_PITCH, value);}
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_PITCH); }
		}
		public float Gain
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_GAIN, value);}
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_GAIN); }
		}
		public float MaxGain
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_MAX_GAIN, value);}
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_MAX_GAIN); }
		}
		public float MinGain
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_MIN_GAIN, value);}
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_MIN_GAIN); }
		}
		public float RolloffFactor
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_ROLLOFF_FACTOR, value); }
			get { return AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_ROLLOFF_FACTOR);}
		}
		public DateTime Time
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_SEC_OFFSET, value.Ticks); }
			get { float ret = AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_SEC_OFFSET); return new DateTime((TimeSpan.FromSeconds(ret)).Ticks);}
		}
		public long TimeSeconds
		{
			set { AL.alSourcef(this.ui_ID, AL_SourcefParam.AL_SEC_OFFSET, value); } //(float)TimeSpan.FromTicks(value).TotalSeconds);}
			get { float ret = AL.alGetSourcef(this.ui_ID, AL_SourcefParam.AL_SEC_OFFSET); return (long)ret; } //(TimeSpan.FromSeconds(ret)).Ticks;}
		}
		public TimeSpan Duration
		{
			get { TimeSpan dur = TimeSpan.FromSeconds(this.snd.Duration); return dur; } //new DateTime(dur.Ticks);}
		}
		public DateTime TimeRemaining
		{
			get { return new DateTime((this.Duration.Ticks - this.Time.Ticks));}
		}
		#endregion

		public uint ID
		{
			get { return this.ui_ID; }
		}
	}
}