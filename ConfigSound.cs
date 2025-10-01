using System;
using System.Linq;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class ConfigSound : Config<ConfigSound.ConfigSoundState, ConfigSound>
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x000458E2 File Offset: 0x00043AE2
	// (set) Token: 0x060009D1 RID: 2513 RVA: 0x000458EE File Offset: 0x00043AEE
	public static ConfigSound.ConfigSoundState Settings
	{
		get
		{
			return Singleton<ConfigSound>.Instance.settings;
		}
		set
		{
			Singleton<ConfigSound>.Instance.settings = value;
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x000458FB File Offset: 0x00043AFB
	protected override void Awake()
	{
		base.Awake();
		if (this._settings == null)
		{
			this._settings = new ConfigSound.ConfigSoundState();
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00045918 File Offset: 0x00043B18
	protected override void OnSettingsChanged()
	{
		base.OnSettingsChanged();
		SoundScript.SetSoundMulti(base.settings.GameVolume);
		SoundScript.SetMusicMulti(base.settings.MusicVolume);
		SoundScript.SetMp3Multi(base.settings.Mp3Volume);
		VoiceChat.Mode = base.settings.Mode;
		if (SteamVoiceP2PCommsNetwork.Instance)
		{
			SteamVoiceP2PCommsNetwork.Instance.InputVolume = ConfigSound.Settings.VoiceInput;
			SteamVoiceP2PCommsNetwork.Instance.OutputVolume = ConfigSound.Settings.VoiceOutput;
			if (base.settings.Mode == VoiceChat.VoiceMode.None)
			{
				SteamVoiceP2PCommsNetwork.Instance.OutputVolume = 0f;
			}
			if (Microphone.devices.ToList<string>().Contains(base.settings.Microphone))
			{
				SteamVoiceP2PCommsNetwork.Instance.Comms.MicrophoneName = base.settings.Microphone;
			}
		}
		Singleton<DiscordController>.Instance.RefreshPresence();
		EventManager.TriggerConfigSoundChange(base.settings);
	}

	// Token: 0x02000593 RID: 1427
	[Serializable]
	public class ConfigSoundState
	{
		// Token: 0x04002558 RID: 9560
		public float MasterVolume = 0.5f;

		// Token: 0x04002559 RID: 9561
		public float GameVolume = 0.5f;

		// Token: 0x0400255A RID: 9562
		public float MusicVolume = 0.5f;

		// Token: 0x0400255B RID: 9563
		public float Mp3Volume = 0.5f;

		// Token: 0x0400255C RID: 9564
		public float VoiceInput = 0.5f;

		// Token: 0x0400255D RID: 9565
		public float VoiceOutput = 0.5f;

		// Token: 0x0400255E RID: 9566
		public VoiceChat.VoiceMode Mode;

		// Token: 0x0400255F RID: 9567
		public string Microphone;
	}
}
