using System;
using NewNet;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class MainMenuObjects : MonoBehaviour
{
	// Token: 0x0600153A RID: 5434 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Start()
	{
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x0008A1E8 File Offset: 0x000883E8
	private void Update()
	{
		if (Network.peerType != NetworkPeerMode.Disconnected)
		{
			int childCount = base.gameObject.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(base.gameObject.transform.GetChild(i));
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
