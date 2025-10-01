using System;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class PlayerScript : MonoBehaviour
{
	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x060018F0 RID: 6384 RVA: 0x000A7E60 File Offset: 0x000A6060
	public static int Id
	{
		get
		{
			return NetworkID.ID;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x060018F1 RID: 6385 RVA: 0x000A7E67 File Offset: 0x000A6067
	// (set) Token: 0x060018F2 RID: 6386 RVA: 0x000A7E96 File Offset: 0x000A6096
	public static int PointerInt
	{
		get
		{
			if (PlayerScript.bFirstPointerInt && PlayerPrefs.HasKey("PointerInt3"))
			{
				PlayerScript.pointint = PlayerPrefs.GetInt("PointerInt3");
			}
			PlayerScript.bFirstPointerInt = false;
			return PlayerScript.pointint;
		}
		set
		{
			if (value == 1 && !SteamManager.bKickstarterGold && !SteamManager.bKickstarterPointer)
			{
				return;
			}
			PlayerPrefs.SetInt("PointerInt3", value);
			PlayerScript.pointint = value;
		}
	}

	// Token: 0x04000EC4 RID: 3780
	public static Pointer PointerScript;

	// Token: 0x04000EC5 RID: 3781
	public static GameObject Pointer;

	// Token: 0x04000EC6 RID: 3782
	private static bool bFirstPointerInt = true;

	// Token: 0x04000EC7 RID: 3783
	private static int pointint = -1;
}
