using System;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class UIBlockIDHolder : MonoBehaviour
{
	// Token: 0x06002146 RID: 8518 RVA: 0x000F04BC File Offset: 0x000EE6BC
	private void OnClick()
	{
		UIDialog.Show(Language.Translate("Remove block on {0}?", this.BlockedName), "Yes", "No", new Action(this.confirmDialog), null);
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x000F04EA File Offset: 0x000EE6EA
	private void confirmDialog()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIRemoveBlocked(this.BlockedName, this.IDHolder);
	}

	// Token: 0x0400149D RID: 5277
	public string BlockedName;

	// Token: 0x0400149E RID: 5278
	public string IDHolder;
}
