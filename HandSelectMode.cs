using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class HandSelectMode : Singleton<HandSelectMode>
{
	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x00068413 File Offset: 0x00066613
	public bool IsActive
	{
		get
		{
			return this.ActiveHandZone != null;
		}
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00068421 File Offset: 0x00066621
	public bool IsSelected(NetworkPhysicsObject npo)
	{
		return this.selectedNPOs.Contains(npo);
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00068430 File Offset: 0x00066630
	public new void Awake()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.SoloConfirmButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.onConfirm));
		UIEventListener uieventListener2 = UIEventListener.Get(this.DuoConfirmButton);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.onConfirm));
		UIEventListener uieventListener3 = UIEventListener.Get(this.DuoCancelButton);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.onCancel));
		EventManager.OnLoadingComplete += this.onLoad;
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x000684E4 File Offset: 0x000666E4
	public void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.SoloConfirmButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.onConfirm));
		UIEventListener uieventListener2 = UIEventListener.Get(this.DuoConfirmButton);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.onConfirm));
		UIEventListener uieventListener3 = UIEventListener.Get(this.DuoCancelButton);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.onCancel));
		EventManager.OnLoadingComplete -= this.onLoad;
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x00068598 File Offset: 0x00066798
	public void Refresh()
	{
		HandZone handZone = HandZone.GetHandZone(NetworkSingleton<PlayerManager>.Instance.MyPlayerState().stringColor, 0, true);
		if (handZone && handZone.HasQueuedHandSelectMode)
		{
			this.StartHandSelectMode(handZone);
			return;
		}
		this.EndHandSelectMode(false, false);
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x000685DC File Offset: 0x000667DC
	private void OnPlayerChangeColor(PlayerState player)
	{
		if (player.IsMe())
		{
			this.Refresh();
		}
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x000685EC File Offset: 0x000667EC
	private void onLoad()
	{
		this.Refresh();
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x000685F4 File Offset: 0x000667F4
	private void onConfirm(GameObject go)
	{
		this.EndHandSelectMode(true, true);
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x000685FE File Offset: 0x000667FE
	private void onCancel(GameObject go)
	{
		this.EndHandSelectMode(false, true);
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00068608 File Offset: 0x00066808
	public void StartHandSelectMode(HandZone handZone)
	{
		PlayerScript.PointerScript.ResetHighlight();
		this.ActiveHandZone = handZone;
		HandSelectModeSettings handSelectModeSettings = handZone.HandSelectModeSettingsQueue[0];
		this.preset = handSelectModeSettings.preset;
		this.label = handSelectModeSettings.label;
		this.minSelectedCount = handSelectModeSettings.minCount;
		this.maxSelectedCount = handSelectModeSettings.maxCount;
		if (handSelectModeSettings.showCancel)
		{
			this.SoloConfirmButton.SetActive(false);
			this.DuoConfirmButton.SetActive(true);
			this.DuoCancelButton.SetActive(true);
			this.confirmButton = this.DuoConfirmButton.GetComponent<UIButton>();
		}
		else
		{
			this.SoloConfirmButton.SetActive(true);
			this.DuoConfirmButton.SetActive(false);
			this.DuoCancelButton.SetActive(false);
			this.confirmButton = this.SoloConfirmButton.GetComponent<UIButton>();
		}
		this.updateConfirmButton();
		string prompt = handSelectModeSettings.prompt;
		if (!string.IsNullOrEmpty(prompt))
		{
			TextCode.LocalizeUIText(ref prompt);
		}
		this.HandSelectLabel.text = prompt;
		this.HandSelectLabel.gameObject.SetActive(prompt != "");
		this.selectedNPOs.Clear();
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00068728 File Offset: 0x00066928
	public void EndHandSelectMode(bool confirmed, bool doCallback = true)
	{
		this.HandSelectLabel.gameObject.SetActive(false);
		this.SoloConfirmButton.SetActive(false);
		this.DuoConfirmButton.SetActive(false);
		this.DuoCancelButton.SetActive(false);
		if (!this.ActiveHandZone)
		{
			return;
		}
		if (doCallback)
		{
			List<LuaGameObjectScript> chosenObjects = confirmed ? this.selectedNPOs.ToLGOS((NetworkPhysicsObject npo) => this.ActiveHandZone.ContainedNPOs.Contains(npo)) : new List<LuaGameObjectScript>();
			if (this.preset == null)
			{
				EventManager.TriggerHandSelectModeEnd(this.ActiveHandZone.TriggerLabel, this.label, chosenObjects, confirmed);
			}
			this.ActiveHandZone.EndHandSelectMode();
		}
		this.selectedNPOs.Clear();
		this.ActiveHandZone = null;
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000687DC File Offset: 0x000669DC
	public void ToggleNPOSelection(NetworkPhysicsObject npo)
	{
		if (!this.ActiveHandZone || !this.ActiveHandZone.ContainedNPOs.Contains(npo))
		{
			return;
		}
		if (!this.selectedNPOs.Remove(npo))
		{
			this.selectedNPOs.Add(npo);
			if (this.maxSelectedCount > 0 && this.selectedNPOs.Count > this.maxSelectedCount)
			{
				this.selectedNPOs.RemoveAt(0);
			}
		}
		this.updateConfirmButton();
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x00068852 File Offset: 0x00066A52
	public bool Unselect(NetworkPhysicsObject npo)
	{
		bool result = this.selectedNPOs.Remove(npo);
		this.updateConfirmButton();
		return result;
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x00068866 File Offset: 0x00066A66
	private void updateConfirmButton()
	{
		this.confirmButton.isEnabled = (this.selectedNPOs.Count >= this.minSelectedCount);
	}

	// Token: 0x04000984 RID: 2436
	public GameObject SoloConfirmButton;

	// Token: 0x04000985 RID: 2437
	public GameObject DuoConfirmButton;

	// Token: 0x04000986 RID: 2438
	public GameObject DuoCancelButton;

	// Token: 0x04000987 RID: 2439
	public UILabel HandSelectLabel;

	// Token: 0x04000988 RID: 2440
	[NonSerialized]
	public HandZone ActiveHandZone;

	// Token: 0x04000989 RID: 2441
	private List<NetworkPhysicsObject> selectedNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x0400098A RID: 2442
	private HandSelectModePreset preset;

	// Token: 0x0400098B RID: 2443
	private string label;

	// Token: 0x0400098C RID: 2444
	private int minSelectedCount;

	// Token: 0x0400098D RID: 2445
	private int maxSelectedCount;

	// Token: 0x0400098E RID: 2446
	private UIButton confirmButton;
}
