using System;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class UILayoutZone : MonoBehaviour
{
	// Token: 0x060024EA RID: 9450 RVA: 0x00104689 File Offset: 0x00102889
	public void OnEditSettings()
	{
		NetworkSingleton<NetworkUI>.Instance.GUILayoutZone.SetActive(false);
		NetworkSingleton<NetworkUI>.Instance.GUILayoutZoneSettings.Activate(PlayerScript.PointerScript.InfoLayoutZoneGO.GetComponent<LayoutZone>());
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x001046B9 File Offset: 0x001028B9
	public void OnLayout()
	{
		PlayerScript.PointerScript.InfoLayoutZoneGO.GetComponent<LayoutZone>().ManualLayoutZone();
		NetworkSingleton<NetworkUI>.Instance.GUILayoutZone.SetActive(false);
	}
}
