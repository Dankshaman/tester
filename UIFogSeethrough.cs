using System;
using NewNet;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class UIFogSeethrough : MonoBehaviour
{
	// Token: 0x06002330 RID: 9008 RVA: 0x000FA1E4 File Offset: 0x000F83E4
	private void OnEnable()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		GameObject infoHiddenZoneGO = PlayerScript.PointerScript.InfoHiddenZoneGO;
		if (infoHiddenZoneGO)
		{
			base.GetComponent<UIToggle>().value = infoHiddenZoneGO.GetComponent<HiddenZone>().isTranslucent;
		}
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000FA228 File Offset: 0x000F8428
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
			component.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(component.SetTranslucent), base.GetComponent<UIToggle>().value);
		}
	}
}
