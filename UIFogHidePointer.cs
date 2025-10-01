using System;
using NewNet;
using UnityEngine;

// Token: 0x020002CD RID: 717
public class UIFogHidePointer : MonoBehaviour
{
	// Token: 0x06002322 RID: 8994 RVA: 0x000F9F70 File Offset: 0x000F8170
	private void OnEnable()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		GameObject infoHiddenZoneGO = PlayerScript.PointerScript.InfoHiddenZoneGO;
		if (infoHiddenZoneGO)
		{
			base.GetComponent<UIToggle>().value = infoHiddenZoneGO.GetComponent<HiddenZone>().pointersAreHidden;
		}
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000F9FB4 File Offset: 0x000F81B4
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
			component.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(component.SetPointersAreHidden), base.GetComponent<UIToggle>().value);
		}
	}
}
