using System;
using UnityEngine;

// Token: 0x0200029D RID: 669
public class UIConfigSound : UIReactiveMenu
{
	// Token: 0x060021E7 RID: 8679 RVA: 0x000F465C File Offset: 0x000F285C
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.MasterVolumeSlider.onChange);
		this.ReactiveElements.Add(this.GameVolumeSlider.onChange);
		this.ReactiveElements.Add(this.MusicVolumeSlider.onChange);
		this.ReactiveElements.Add(this.Mp3VolumeSlider.onChange);
		this.ReactiveElements.Add(this.VoiceInputSlider.onChange);
		this.ReactiveElements.Add(this.VoiceOutputSlider.onChange);
		this.ReactiveElements.Add(this.VoiceChatPopup.onChange);
		this.ReactiveElements.Add(this.MicrophonePopup.onChange);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.OnClickReset));
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000F473C File Offset: 0x000F293C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.OnClickReset));
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000F4761 File Offset: 0x000F2961
	private void OnClickReset()
	{
		ConfigSound.Settings = new ConfigSound.ConfigSoundState();
		base.TriggerReloadUI();
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000F4774 File Offset: 0x000F2974
	protected override void ReloadUI()
	{
		ConfigSound.ConfigSoundState settings = ConfigSound.Settings;
		this.MasterVolumeSlider.value = settings.MasterVolume;
		this.GameVolumeSlider.value = settings.GameVolume;
		this.MusicVolumeSlider.value = settings.MusicVolume;
		this.Mp3VolumeSlider.value = settings.Mp3Volume;
		this.VoiceInputSlider.value = settings.VoiceInput;
		this.VoiceOutputSlider.value = settings.VoiceOutput;
		this.VoiceChatPopup.value = this.VoiceChatPopup.items[(int)settings.Mode];
		this.MicrophonePopup.items.Clear();
		this.MicrophonePopup.items.Add("<Default>");
		string[] devices = Microphone.devices;
		for (int i = 0; i < devices.Length; i++)
		{
			this.MicrophonePopup.items.Add(devices[i]);
			if (settings.Microphone == devices[i])
			{
				this.MicrophonePopup.value = devices[i];
			}
		}
		if (string.IsNullOrEmpty(settings.Microphone))
		{
			this.MicrophonePopup.value = "<Default>";
		}
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000F4898 File Offset: 0x000F2A98
	protected override void UpdateSource()
	{
		ConfigSound.ConfigSoundState configSoundState = new ConfigSound.ConfigSoundState();
		configSoundState.MasterVolume = this.MasterVolumeSlider.value;
		configSoundState.GameVolume = this.GameVolumeSlider.value;
		configSoundState.MusicVolume = this.MusicVolumeSlider.value;
		configSoundState.Mp3Volume = this.Mp3VolumeSlider.value;
		configSoundState.VoiceInput = this.VoiceInputSlider.value;
		configSoundState.VoiceOutput = this.VoiceOutputSlider.value;
		configSoundState.Mode = (VoiceChat.VoiceMode)this.VoiceChatPopup.items.IndexOf(this.VoiceChatPopup.value);
		if (this.MicrophonePopup.value != "<Default>")
		{
			configSoundState.Microphone = this.MicrophonePopup.value;
		}
		ConfigSound.Settings = configSoundState;
	}

	// Token: 0x04001551 RID: 5457
	public UISlider MasterVolumeSlider;

	// Token: 0x04001552 RID: 5458
	public UISlider GameVolumeSlider;

	// Token: 0x04001553 RID: 5459
	public UISlider MusicVolumeSlider;

	// Token: 0x04001554 RID: 5460
	public UISlider Mp3VolumeSlider;

	// Token: 0x04001555 RID: 5461
	public UISlider VoiceInputSlider;

	// Token: 0x04001556 RID: 5462
	public UISlider VoiceOutputSlider;

	// Token: 0x04001557 RID: 5463
	public UIPopupList VoiceChatPopup;

	// Token: 0x04001558 RID: 5464
	public UIPopupList MicrophonePopup;

	// Token: 0x04001559 RID: 5465
	public UIButton ResetButton;
}
