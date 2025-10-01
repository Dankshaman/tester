using System;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class UIProfilePotrait : MonoBehaviour
{
	// Token: 0x06002697 RID: 9879 RVA: 0x00113100 File Offset: 0x00111300
	private void OnClick()
	{
		string str = NetworkSingleton<PlayerManager>.Instance.SteamIDFromID(NetworkSingleton<PlayerManager>.Instance.IDFromName(base.transform.parent.GetComponentInChildren<UILabel>().text));
		TTSUtilities.OpenURL("http://steamcommunity.com/profiles/" + str + "/");
	}
}
