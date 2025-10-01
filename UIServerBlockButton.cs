using System;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class UIServerBlockButton : MonoBehaviour
{
	// Token: 0x060026E8 RID: 9960 RVA: 0x001148AE File Offset: 0x00112AAE
	private void OnClick()
	{
		UIDialog.Show(Language.Translate("Block {0}?", this.PlayerName), "Yes", "No", new Action(this.Confirm), null);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060026E9 RID: 9961 RVA: 0x001148E8 File Offset: 0x00112AE8
	private void Confirm()
	{
		Singleton<BlockList>.Instance.AddBlock(this.PlayerName, this.PlayerSteamID);
		Singleton<UIServerBrowser>.Instance.StartRefresh();
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x0011490A File Offset: 0x00112B0A
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && UICamera.HoveredUIObject != base.gameObject)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04001963 RID: 6499
	public string PlayerName;

	// Token: 0x04001964 RID: 6500
	public string PlayerSteamID;
}
