using System;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class UIRobotPointer : MonoBehaviour
{
	// Token: 0x060026D0 RID: 9936 RVA: 0x00113F0A File Offset: 0x0011210A
	private void OnEnable()
	{
		base.GetComponent<BoxCollider2D>().enabled = (SteamManager.bKickstarterGold || SteamManager.bKickstarterPointer);
	}
}
