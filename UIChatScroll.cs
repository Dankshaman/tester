using System;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class UIChatScroll : MonoBehaviour
{
	// Token: 0x06002170 RID: 8560 RVA: 0x000402F2 File Offset: 0x0003E4F2
	private void OnDrag()
	{
		NetworkSingleton<Chat>.Instance.RefreshChatLog();
	}
}
