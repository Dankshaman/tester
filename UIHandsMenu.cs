using System;

// Token: 0x020002E3 RID: 739
public class UIHandsMenu : UIReactiveMenu
{
	// Token: 0x06002433 RID: 9267 RVA: 0x001002C0 File Offset: 0x000FE4C0
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.EnableToggle.onChange);
		this.ReactiveElements.Add(this.DisableUnusedToggle.onChange);
		this.ReactiveElements.Add(this.HidingDefaultToggle.onChange);
		this.ReactiveElements.Add(this.HidingReverseToggle.onChange);
		this.ReactiveElements.Add(this.HidingDisableToggle.onChange);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x0010035E File Offset: 0x000FE55E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x00100383 File Offset: 0x000FE583
	private void ResetOnClick()
	{
		NetworkSingleton<Hands>.Instance.Reset();
		base.TriggerReloadUI();
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x00100398 File Offset: 0x000FE598
	protected override void ReloadUI()
	{
		HandsState handsState = NetworkSingleton<Hands>.Instance.handsState;
		this.EnableToggle.value = handsState.Enable;
		this.DisableUnusedToggle.value = handsState.DisableUnused;
		this.HidingDefaultToggle.value = (handsState.Hiding == HidingType.Default);
		this.HidingReverseToggle.value = (handsState.Hiding == HidingType.Reverse);
		this.HidingDisableToggle.value = (handsState.Hiding == HidingType.Disable);
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x00100410 File Offset: 0x000FE610
	protected override void UpdateSource()
	{
		HandsState handsState = new HandsState();
		handsState.Enable = this.EnableToggle.value;
		handsState.DisableUnused = this.DisableUnusedToggle.value;
		if (this.HidingDefaultToggle.value)
		{
			handsState.Hiding = HidingType.Default;
		}
		else if (this.HidingReverseToggle.value)
		{
			handsState.Hiding = HidingType.Reverse;
		}
		else
		{
			handsState.Hiding = HidingType.Disable;
		}
		NetworkSingleton<Hands>.Instance.handsState = handsState;
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x04001740 RID: 5952
	public UIToggle EnableToggle;

	// Token: 0x04001741 RID: 5953
	public UIToggle DisableUnusedToggle;

	// Token: 0x04001742 RID: 5954
	public UIToggle HidingDefaultToggle;

	// Token: 0x04001743 RID: 5955
	public UIToggle HidingReverseToggle;

	// Token: 0x04001744 RID: 5956
	public UIToggle HidingDisableToggle;

	// Token: 0x04001745 RID: 5957
	public UIButton ResetButton;
}
