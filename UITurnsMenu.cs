using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class UITurnsMenu : UIReactiveMenu
{
	// Token: 0x06002882 RID: 10370 RVA: 0x0011E204 File Offset: 0x0011C404
	protected override void Awake()
	{
		base.Awake();
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			this.TurnsToggle.value = true;
			this.TurnsToggle.GetComponent<BoxCollider2D>().enabled = false;
		}
		for (int i = 0; i < this.TurnOrderGrid.transform.childCount; i++)
		{
			GameObject gameObject = this.TurnOrderGrid.transform.GetChild(i).gameObject;
			this.TurnColorObjects.Add(gameObject);
			UIEventListener uieventListener = UIEventListener.Get(gameObject);
			uieventListener.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onDragEnd, new UIEventListener.VoidDelegate(this.onDragEndColor));
		}
		this.ReactiveElements.Add(this.TurnsToggle.onChange);
		this.ReactiveElements.Add(this.AutomaticToggle.onChange);
		this.ReactiveElements.Add(this.CustomToggle.onChange);
		this.ReactiveElements.Add(this.ReverseToggle.onChange);
		this.ReactiveElements.Add(this.SkipToggle.onChange);
		this.ReactiveElements.Add(this.DisableInteractionToggle.onChange);
		this.ReactiveElements.Add(this.PassTurnToggle.onChange);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
		EventManager.OnLoadingComplete += this.EventManager_OnLoadingComplete;
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x0011E370 File Offset: 0x0011C570
	protected override void OnDestroy()
	{
		base.OnDestroy();
		for (int i = 0; i < this.TurnOrderGrid.transform.childCount; i++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.TurnOrderGrid.transform.GetChild(i).gameObject);
			uieventListener.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onDragEnd, new UIEventListener.VoidDelegate(this.onDragEndColor));
		}
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
		EventManager.OnLoadingComplete -= this.EventManager_OnLoadingComplete;
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000FFF61 File Offset: 0x000FE161
	private void EventManager_OnLoadingComplete()
	{
		if (base.gameObject.activeSelf)
		{
			base.TriggerReloadUI();
		}
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x0011E408 File Offset: 0x0011C608
	private void ResetOnClick()
	{
		NetworkSingleton<Turns>.Instance.Reset();
		base.TriggerReloadUI();
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x0011E41A File Offset: 0x0011C61A
	private void onDragEndColor(GameObject go)
	{
		base.ElementsOnChange();
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x0011E424 File Offset: 0x0011C624
	protected override void ReloadUI()
	{
		TurnsState turnsState = NetworkSingleton<Turns>.Instance.turnsState;
		this.TurnsToggle.value = (turnsState.Enable || NetworkSingleton<NetworkUI>.Instance.bHotseat);
		this.TurnsToggle.GetComponent<BoxCollider2D>().enabled = !NetworkSingleton<NetworkUI>.Instance.bHotseat;
		this.AutomaticToggle.value = (turnsState.Type == TurnType.Auto);
		this.CustomToggle.value = (turnsState.Type == TurnType.Custom);
		this.ReverseToggle.value = turnsState.Reverse;
		this.SkipToggle.value = turnsState.SkipEmpty;
		this.DisableInteractionToggle.value = turnsState.DisableInteractions;
		this.PassTurnToggle.value = turnsState.PassTurns;
		this.ReloadTurnOrderUI();
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x0011E4EC File Offset: 0x0011C6EC
	private void ReloadTurnOrderUI()
	{
		TurnsState turnsState = NetworkSingleton<Turns>.Instance.turnsState;
		List<string> turnOrder = NetworkSingleton<Turns>.Instance.GetTurnOrder();
		for (int i = 0; i < this.TurnColorObjects.Count; i++)
		{
			GameObject gameObject = this.TurnColorObjects[i];
			gameObject.SetActive(false);
			gameObject.GetComponent<UIDragDropItem>().enabled = (turnsState.Type == TurnType.Custom);
			int num = turnOrder.IndexOf(gameObject.name);
			if (num != -1)
			{
				gameObject.SetActive(true);
				gameObject.transform.position = new Vector3(this.TurnOrderGrid.transform.position.x + 0.01f * (float)num, this.TurnOrderGrid.transform.position.y, this.TurnOrderGrid.transform.position.z);
			}
		}
		this.TurnOrderGrid.Reposition();
		this.TurnOrderGrid.repositionNow = true;
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x0011E5DD File Offset: 0x0011C7DD
	protected override void UpdateSource()
	{
		NetworkSingleton<Turns>.Instance.turnsState = this.GetTurnState();
		this.ReloadTurnOrderUI();
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x0011E5F8 File Offset: 0x0011C7F8
	private TurnsState GetTurnState()
	{
		TurnsState turnsState = new TurnsState();
		turnsState.TurnColor = NetworkSingleton<Turns>.Instance.turnsState.TurnColor;
		turnsState.Enable = this.TurnsToggle.value;
		turnsState.Type = (this.AutomaticToggle.value ? TurnType.Auto : TurnType.Custom);
		turnsState.Reverse = this.ReverseToggle.value;
		turnsState.SkipEmpty = this.SkipToggle.value;
		turnsState.DisableInteractions = this.DisableInteractionToggle.value;
		turnsState.PassTurns = this.PassTurnToggle.value;
		if (turnsState.Type == TurnType.Custom)
		{
			List<string> list = new List<string>();
			List<Transform> childList = this.TurnOrderGrid.GetChildList();
			for (int i = 0; i < childList.Count; i++)
			{
				list.Add(childList[i].name);
			}
			turnsState.TurnOrder = list;
		}
		return turnsState;
	}

	// Token: 0x04001AA8 RID: 6824
	public UIToggle TurnsToggle;

	// Token: 0x04001AA9 RID: 6825
	public UIToggle AutomaticToggle;

	// Token: 0x04001AAA RID: 6826
	public UIToggle CustomToggle;

	// Token: 0x04001AAB RID: 6827
	public UIToggle ReverseToggle;

	// Token: 0x04001AAC RID: 6828
	public UIToggle SkipToggle;

	// Token: 0x04001AAD RID: 6829
	public UIToggle DisableInteractionToggle;

	// Token: 0x04001AAE RID: 6830
	public UIToggle PassTurnToggle;

	// Token: 0x04001AAF RID: 6831
	public UIGrid TurnOrderGrid;

	// Token: 0x04001AB0 RID: 6832
	public UIButton ResetButton;

	// Token: 0x04001AB1 RID: 6833
	private List<GameObject> TurnColorObjects = new List<GameObject>();
}
