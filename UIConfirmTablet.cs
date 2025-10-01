using System;
using UnityEngine;

// Token: 0x020002A1 RID: 673
public class UIConfirmTablet : MonoBehaviour
{
	// Token: 0x060021F5 RID: 8693 RVA: 0x000F4C40 File Offset: 0x000F2E40
	private void OnEnable()
	{
		this.DontAskToggle.value = false;
		string str;
		if (this.PlayerID == -1)
		{
			str = "Tablet";
		}
		else if (this.PlayerID != 0)
		{
			str = NetworkSingleton<PlayerManager>.Instance.NameFromID(this.PlayerID);
		}
		else
		{
			str = "Host";
		}
		this.NameLabel.text = str + " wants to open URL:";
		this.URLLabel.text = this.URL;
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000F4CB8 File Offset: 0x000F2EB8
	public void OpenButton()
	{
		if (this.DontAskToggle.value)
		{
			TabletScript.WhiteListId.Add(this.PlayerID);
		}
		this.CurrentTablet.LoadURL(this.URL);
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000F4CE8 File Offset: 0x000F2EE8
	public void CancelButton()
	{
		if (this.DontAskToggle.value)
		{
			TabletScript.BlackListId.Add(this.PlayerID);
		}
	}

	// Token: 0x04001562 RID: 5474
	public TabletScript CurrentTablet;

	// Token: 0x04001563 RID: 5475
	public int PlayerID;

	// Token: 0x04001564 RID: 5476
	public string URL;

	// Token: 0x04001565 RID: 5477
	public UILabel NameLabel;

	// Token: 0x04001566 RID: 5478
	public UILabel URLLabel;

	// Token: 0x04001567 RID: 5479
	public UIToggle DontAskToggle;
}
