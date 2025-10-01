using System;

// Token: 0x02000298 RID: 664
public class UIConfigGame : UIReactiveMenu
{
	// Token: 0x060021CE RID: 8654 RVA: 0x000F37E8 File Offset: 0x000F19E8
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.LookSpeedSlider.onChange);
		this.ReactiveElements.Add(this.InvertVerticalToggle.onChange);
		this.ReactiveElements.Add(this.InvertHorizontalToggle.onChange);
		this.ReactiveElements.Add(this.MovementSpeedSlider.onChange);
		this.ReactiveElements.Add(this.FovSlider.onChange);
		this.ReactiveElements.Add(this.ModCachingToggle.onChange);
		this.ReactiveElements.Add(this.ModLocDocumentsToggle.onChange);
		this.ReactiveElements.Add(this.ModLocGameDataToggle.onChange);
		this.ReactiveElements.Add(this.ModThreadingToggle.onChange);
		this.ReactiveElements.Add(this.PortraitsToggle.onChange);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.OnClickReset));
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000F38F4 File Offset: 0x000F1AF4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.OnClickReset));
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x000F3919 File Offset: 0x000F1B19
	private void OnClickReset()
	{
		Singleton<ConfigGame>.Instance.settings = new ConfigGame.ConfigGameState();
		base.TriggerReloadUI();
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000F3930 File Offset: 0x000F1B30
	protected override void ReloadUI()
	{
		ConfigGame.ConfigGameState settings = Singleton<ConfigGame>.Instance.settings;
		this.LookSpeedSlider.value = settings.ConfigCamera.LookSpeed;
		this.InvertVerticalToggle.value = settings.ConfigCamera.InvertVertical;
		this.InvertHorizontalToggle.value = settings.ConfigCamera.InvertHorizontal;
		this.MovementSpeedSlider.value = settings.ConfigCamera.MovementSpeed;
		this.FovSlider.value = (float)(settings.ConfigCamera.FOV - 50) / 20f;
		this.ModCachingToggle.value = settings.ConfigMods.Caching;
		this.ModLocDocumentsToggle.value = (settings.ConfigMods.Location == ConfigGame.ModLocation.Documents);
		this.ModLocGameDataToggle.value = (settings.ConfigMods.Location == ConfigGame.ModLocation.GameData);
		this.ModThreadingToggle.value = settings.ConfigMods.Threading;
		this.PortraitsToggle.value = settings.Portraits;
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000F3A30 File Offset: 0x000F1C30
	protected override void UpdateSource()
	{
		ConfigGame.ConfigGameState configGameState = new ConfigGame.ConfigGameState();
		configGameState.ConfigCamera.LookSpeed = this.LookSpeedSlider.value;
		configGameState.ConfigCamera.InvertVertical = this.InvertVerticalToggle.value;
		configGameState.ConfigCamera.InvertHorizontal = this.InvertHorizontalToggle.value;
		configGameState.ConfigCamera.MovementSpeed = this.MovementSpeedSlider.value;
		configGameState.ConfigCamera.FOV = (int)(this.FovSlider.value * 20f + 50f);
		configGameState.ConfigMods.Caching = this.ModCachingToggle.value;
		configGameState.ConfigMods.Location = (this.ModLocDocumentsToggle.value ? ConfigGame.ModLocation.Documents : ConfigGame.ModLocation.GameData);
		configGameState.ConfigMods.Threading = this.ModThreadingToggle.value;
		configGameState.Portraits = this.PortraitsToggle.value;
		Singleton<ConfigGame>.Instance.settings = configGameState;
	}

	// Token: 0x04001521 RID: 5409
	public UISlider LookSpeedSlider;

	// Token: 0x04001522 RID: 5410
	public UIToggle InvertVerticalToggle;

	// Token: 0x04001523 RID: 5411
	public UIToggle InvertHorizontalToggle;

	// Token: 0x04001524 RID: 5412
	public UISlider MovementSpeedSlider;

	// Token: 0x04001525 RID: 5413
	public UISlider FovSlider;

	// Token: 0x04001526 RID: 5414
	public UIToggle ModCachingToggle;

	// Token: 0x04001527 RID: 5415
	public UIToggle ModLocDocumentsToggle;

	// Token: 0x04001528 RID: 5416
	public UIToggle ModLocGameDataToggle;

	// Token: 0x04001529 RID: 5417
	public UIToggle ModThreadingToggle;

	// Token: 0x0400152A RID: 5418
	public UIToggle PortraitsToggle;

	// Token: 0x0400152B RID: 5419
	public UIButton ResetButton;
}
