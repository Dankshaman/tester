using System;
using UnityEngine;

// Token: 0x0200029E RID: 670
public class UIConfirmDeleteSave : MonoBehaviour
{
	// Token: 0x060021EE RID: 8686 RVA: 0x000F4960 File Offset: 0x000F2B60
	public void Yes()
	{
		if (NetworkSingleton<NetworkUI>.Instance.GUIDeleteDirectory(this.Directory))
		{
			if (this.WorkshopSteamID != "" && SteamManager.bSteam)
			{
				Singleton<SteamManager>.Instance.UnsubscribeFromId(ulong.Parse(this.WorkshopSteamID));
			}
			this.DirectoryButton.transform.parent.parent.GetComponent<UIGrid>().repositionNow = true;
			UnityEngine.Object.Destroy(this.DirectoryButton.transform.parent.gameObject);
		}
	}

	// Token: 0x0400155A RID: 5466
	public string Directory = "";

	// Token: 0x0400155B RID: 5467
	public string WorkshopSteamID = "";

	// Token: 0x0400155C RID: 5468
	public GameObject DirectoryButton;
}
