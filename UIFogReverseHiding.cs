using System;
using NewNet;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class UIFogReverseHiding : MonoBehaviour
{
	// Token: 0x0600232D RID: 9005 RVA: 0x000FA148 File Offset: 0x000F8348
	private void OnEnable()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		GameObject infoHiddenZoneGO = PlayerScript.PointerScript.InfoHiddenZoneGO;
		if (infoHiddenZoneGO)
		{
			base.GetComponent<UIToggle>().value = infoHiddenZoneGO.GetComponent<HiddenZone>().isReversed;
		}
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000FA18C File Offset: 0x000F838C
	private void OnClick()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		GameObject infoHiddenZoneGO = PlayerScript.PointerScript.InfoHiddenZoneGO;
		if (infoHiddenZoneGO)
		{
			HiddenZone component = infoHiddenZoneGO.GetComponent<HiddenZone>();
			component.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(component.SetHidingIsReversed), base.GetComponent<UIToggle>().value);
		}
	}
}
