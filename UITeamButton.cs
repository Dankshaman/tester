using System;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class UITeamButton : MonoBehaviour
{
	// Token: 0x060027F8 RID: 10232 RVA: 0x0011A96C File Offset: 0x00118B6C
	private void OnEnable()
	{
		UITeamButton.OverrideId = -1;
		base.GetComponent<BoxCollider2D>().enabled = PermissionsOptions.CheckAllow(PermissionsOptions.options.ChangeTeam, -1);
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x0011A990 File Offset: 0x00118B90
	private void OnClick()
	{
		if (PermissionsOptions.CheckAllow(PermissionsOptions.options.ChangeTeam, -1))
		{
			if (UITeamButton.OverrideId == -1)
			{
				NetworkSingleton<PlayerManager>.Instance.ChangeTeam(NetworkID.ID, (Team)this.TeamInt);
				return;
			}
			NetworkSingleton<PlayerManager>.Instance.ChangeTeam(UITeamButton.OverrideId, (Team)this.TeamInt);
		}
	}

	// Token: 0x04001A3D RID: 6717
	public int TeamInt;

	// Token: 0x04001A3E RID: 6718
	public static int OverrideId = -1;
}
