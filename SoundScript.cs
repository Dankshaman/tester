using System;
using NewNet;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class SoundScript : NetworkBehavior
{
	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x000C00FE File Offset: 0x000BE2FE
	// (set) Token: 0x06001BFA RID: 7162 RVA: 0x000C0106 File Offset: 0x000BE306
	public bool UseAltSounds
	{
		get
		{
			return this._UseAltSounds;
		}
		set
		{
			this.soundMaterial.soundMaterialType = (value ? this.altSoundMaterialType : this.soundMaterialType);
			this._UseAltSounds = value;
		}
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x000C012B File Offset: 0x000BE32B
	public ObjectSounds GetSounds()
	{
		if (!this.UseAltSounds)
		{
			return this.sounds;
		}
		return this.soundsAlt;
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x000C0144 File Offset: 0x000BE344
	private void Awake()
	{
		this.audio = base.GetComponent<AudioSource>();
		this.soundMaterial = base.GetComponent<SoundMaterial>();
		if (this.soundMaterial)
		{
			this.soundMaterialType = this.soundMaterial.soundMaterialType;
		}
		if (SoundScript.NoSound)
		{
			this.audio.enabled = false;
			UnityEngine.Object.Destroy(this.audio);
		}
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x000C01A8 File Offset: 0x000BE3A8
	private void Start()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		if (this.audio)
		{
			if (this.audio.maxDistance < 150f)
			{
				this.audio.maxDistance = 150f;
			}
			this.audio.dopplerLevel = 0f;
		}
		if (this.NPO)
		{
			this.NPO.collisionEvents.OnCollisionEnterEvent += this.ManagedOnCollisionEnter;
		}
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x000C0229 File Offset: 0x000BE429
	private void OnDestroy()
	{
		if (this.NPO)
		{
			this.NPO.collisionEvents.OnCollisionEnterEvent -= this.ManagedOnCollisionEnter;
		}
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x000C0254 File Offset: 0x000BE454
	public void PlayRandomSound(AudioClip[] clips, float volume)
	{
		if (clips != null && clips.Length != 0 && this.audio)
		{
			this.audio.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
			this.audio.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)], volume * SoundScript.GLOBAL_SOUND_MULTI);
		}
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x000C02AC File Offset: 0x000BE4AC
	public void PlaySound(AudioClip AC, float volume)
	{
		if (this.audio)
		{
			this.audio.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
			this.audio.PlayOneShot(AC, volume * SoundScript.GLOBAL_SOUND_MULTI);
		}
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x000C02E8 File Offset: 0x000BE4E8
	public void PlayGUISound(AudioClip sfx, float volume, float pitch = 1f)
	{
		if (this.audio)
		{
			this.audio.pitch = pitch;
			this.audio.PlayOneShot(sfx, volume * SoundScript.GLOBAL_SOUND_MULTI);
		}
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x000C0318 File Offset: 0x000BE518
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void TableFlipSound()
	{
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.TableFlipSound));
			}
			this.PlaySound(Singleton<SoundManager>.Instance.FlipTable, 1.25f);
			this.last_sound_time = Time.time;
			Singleton<CameraController>.Instance.Shake();
		}
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x000C0384 File Offset: 0x000BE584
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void TeleportSound()
	{
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.TeleportSound));
			}
			this.PlaySound(Singleton<SoundManager>.Instance.TeleportObject, 1f);
			this.last_sound_time = Time.time;
		}
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x000C03E4 File Offset: 0x000BE5E4
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void CopyPasteSound()
	{
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.CopyPasteSound));
			}
			this.PlaySound(Singleton<SoundManager>.Instance.CopyPaste, 1f);
			this.last_sound_time = Time.time;
		}
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000C0444 File Offset: 0x000BE644
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void BeepSound()
	{
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.BeepSound));
			}
			this.PlaySound(Singleton<SoundManager>.Instance.BeepSound, 2f);
			this.last_sound_time = Time.time;
		}
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x000C04A4 File Offset: 0x000BE6A4
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void TimerSound()
	{
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.TimerSound));
			}
			this.PlaySound(Singleton<SoundManager>.Instance.TimerSound, 2f);
			this.last_sound_time = Time.time;
		}
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x000C0504 File Offset: 0x000BE704
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void ShakeSound()
	{
		if (Time.time > this.last_shake_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.ShakeSound));
			}
			if (base.GetComponent<DeckScript>())
			{
				base.GetComponent<DeckScript>().SpawnFacadeCard();
			}
			this.PlayRandomSound(this.GetSounds().Shake, 1f);
			this.last_shake_sound_time = Time.time;
		}
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x000C057C File Offset: 0x000BE77C
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void PickUpSound()
	{
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.PickUpSound));
			}
			this.PlayRandomSound(this.GetSounds().Pickup, 0.35f);
			this.last_sound_time = Time.time;
		}
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x000C05DC File Offset: 0x000BE7DC
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void DropSound()
	{
		AudioClip[] drop = this.GetSounds().Drop;
		if (drop == null || drop.Length == 0)
		{
			return;
		}
		if (Time.time > this.last_sound_time + 0.1f)
		{
			if (Network.isServer)
			{
				base.networkView.RPC(RPCTarget.Others, new Action(this.DropSound));
			}
			this.PlayRandomSound(this.GetSounds().Drop, 0.35f);
			this.last_sound_time = Time.time;
		}
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x000C0650 File Offset: 0x000BE850
	[Remote(Permission.Server, SendType.Unreliable, null, SerializationMethod.Default)]
	public void PlayImpactSound(SoundMaterialType type, float volume)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<SoundMaterialType, float>(RPCTarget.Others, new Action<SoundMaterialType, float>(this.PlayImpactSound), type, volume);
		}
		AudioClip[] array = this.GetSounds().SoundsFromMaterial(type);
		if (array != null)
		{
			this.PlayRandomSound(array, volume);
		}
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x000C0698 File Offset: 0x000BE898
	public void ManagedOnCollisionEnter(Collision info)
	{
		if (Network.isClient)
		{
			return;
		}
		if (this.NPO.IsLocked)
		{
			return;
		}
		float magnitude = info.relativeVelocity.magnitude;
		if (magnitude > 1f && Time.time > this.last_sound_time + 0.1f)
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(info.collider);
			if (networkPhysicsObject.IsDestroyed)
			{
				return;
			}
			if (!NetworkSingleton<ManagerPhysicsObject>.Instance.CanPlayImpactSound())
			{
				return;
			}
			this.last_sound_time = Time.time;
			float num = magnitude * 0.1f * 1f;
			num = Mathf.Min(10f, num);
			SoundMaterial componentInParent = info.collider.gameObject.GetComponentInParent<SoundMaterial>();
			if (componentInParent)
			{
				SoundMaterialType type = (networkPhysicsObject && networkPhysicsObject.IsLocked) ? SoundMaterial.GetSurfaceVersion(componentInParent.soundMaterialType) : componentInParent.soundMaterialType;
				this.PlayImpactSound(type, num);
			}
		}
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x000C0782 File Offset: 0x000BE982
	public static void SetSoundMulti(float Multi)
	{
		if (Multi != 0f)
		{
			SoundScript.GLOBAL_SOUND_MULTI = Multi / 2.5f;
			return;
		}
		SoundScript.GLOBAL_SOUND_MULTI = 0f;
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x000C07A3 File Offset: 0x000BE9A3
	public static void SetMusicMulti(float Multi)
	{
		if (Multi != 0f)
		{
			SoundScript.GLOBAL_MUSIC_MULTI = Multi / 2.5f;
			return;
		}
		SoundScript.GLOBAL_MUSIC_MULTI = 0f;
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000C07C4 File Offset: 0x000BE9C4
	public static void SetMp3Multi(float Multi)
	{
		if (Multi != 0f)
		{
			SoundScript.GLOBAL_MP3_MULTI = Multi / 2.5f;
			return;
		}
		SoundScript.GLOBAL_MP3_MULTI = 0f;
	}

	// Token: 0x0400119D RID: 4509
	private bool _UseAltSounds;

	// Token: 0x0400119E RID: 4510
	private SoundMaterialType soundMaterialType;

	// Token: 0x0400119F RID: 4511
	[SerializeField]
	private SoundMaterialType altSoundMaterialType;

	// Token: 0x040011A0 RID: 4512
	private SoundMaterial soundMaterial;

	// Token: 0x040011A1 RID: 4513
	public ObjectSounds sounds;

	// Token: 0x040011A2 RID: 4514
	public ObjectSounds soundsAlt;

	// Token: 0x040011A3 RID: 4515
	private float last_sound_time;

	// Token: 0x040011A4 RID: 4516
	public static float GLOBAL_SOUND_MULTI = 0.5f;

	// Token: 0x040011A5 RID: 4517
	public static float GLOBAL_MUSIC_MULTI = 0.5f;

	// Token: 0x040011A6 RID: 4518
	public static float GLOBAL_MP3_MULTI = 0.5f;

	// Token: 0x040011A7 RID: 4519
	private const float IMPACT_INTERVAL = 0.1f;

	// Token: 0x040011A8 RID: 4520
	private const float IMPACT_SOUND_MULTI = 1f;

	// Token: 0x040011A9 RID: 4521
	private const float IMPACT_MIN_SPEED = 1f;

	// Token: 0x040011AA RID: 4522
	private NetworkPhysicsObject NPO;

	// Token: 0x040011AB RID: 4523
	private AudioSource audio;

	// Token: 0x040011AC RID: 4524
	private static readonly bool NoSound = Utilities.IsLaunchOption("-nosound");

	// Token: 0x040011AD RID: 4525
	private float last_shake_sound_time;
}
