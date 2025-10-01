using System;
using UnityEngine;

// Token: 0x020002A2 RID: 674
public class UIConfirmUnsubscribe : MonoBehaviour
{
	// Token: 0x060021F9 RID: 8697 RVA: 0x000F4D07 File Offset: 0x000F2F07
	private void OnEnable()
	{
		this.Title.text = this.WorkshopSteamTitle;
		this.ID.text = this.WorkshopSteamID.ToString();
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000F4D30 File Offset: 0x000F2F30
	public void Yes()
	{
		if (this.WorkshopSteamID != 0UL && SteamManager.bSteam)
		{
			Debug.Log(string.Format("UIConfirmUnsubscribe: Usubscribing From {0}", this.WorkshopSteamTitle));
			Singleton<SteamManager>.Instance.UnsubscribeFromId(this.WorkshopSteamID);
		}
		base.gameObject.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000F4D8C File Offset: 0x000F2F8C
	public void No()
	{
		base.gameObject.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x04001568 RID: 5480
	public string WorkshopSteamTitle = "";

	// Token: 0x04001569 RID: 5481
	public ulong WorkshopSteamID;

	// Token: 0x0400156A RID: 5482
	public UILabel Title;

	// Token: 0x0400156B RID: 5483
	public UILabel ID;
}
