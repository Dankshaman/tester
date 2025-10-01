using System;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class UIConfigBlocked : MonoBehaviour
{
	// Token: 0x060021B8 RID: 8632 RVA: 0x000F3211 File Offset: 0x000F1411
	private void Start()
	{
		EventDelegate.Add(this.BlockButton.onClick, new EventDelegate.Callback(this.BlockOnClick));
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x000F3230 File Offset: 0x000F1430
	private void OnDestroy()
	{
		EventDelegate.Remove(this.BlockButton.onClick, new EventDelegate.Callback(this.BlockOnClick));
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000F3250 File Offset: 0x000F1450
	private void BlockOnClick()
	{
		if (string.IsNullOrEmpty(this.NameInput.value))
		{
			Chat.LogError("No Name provided!", true);
			return;
		}
		if (string.IsNullOrEmpty(this.SteamIdInput.value))
		{
			Chat.LogError("No Steam ID provided!", true);
			return;
		}
		try
		{
			SteamManager.StringToSteamID(this.SteamIdInput.value);
		}
		catch (Exception)
		{
			Chat.LogError("Invalid Steam ID!", true);
			return;
		}
		UIDialog.Show(Language.Translate("Block {0}?", this.NameInput.value), "Block", "Cancel", new Action(this.BlockInput), new Action(this.CleanupBlock));
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x000F3308 File Offset: 0x000F1508
	private void BlockInput()
	{
		Singleton<BlockList>.Instance.AddBlock(this.NameInput.value, this.SteamIdInput.value);
		NetworkSingleton<NetworkUI>.Instance.GUIStartBlocked();
		this.CleanupBlock();
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x000F333A File Offset: 0x000F153A
	private void CleanupBlock()
	{
		this.NameInput.value = "";
		this.SteamIdInput.value = "";
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x000F335C File Offset: 0x000F155C
	private void OnEnable()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIStartBlocked();
	}

	// Token: 0x04001515 RID: 5397
	public UIInput NameInput;

	// Token: 0x04001516 RID: 5398
	public UIInput SteamIdInput;

	// Token: 0x04001517 RID: 5399
	public UIButton BlockButton;

	// Token: 0x04001518 RID: 5400
	public UIGrid Grid;
}
