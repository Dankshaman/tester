using System;
using NewNet;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class NetworkSetup : MonoBehaviour
{
	// Token: 0x06001838 RID: 6200 RVA: 0x000A52C8 File Offset: 0x000A34C8
	private void Start()
	{
		NetworkEvents.OnServerInitializing += this.ServerInitializing;
		NetworkEvents.OnServerInitialized += this.ServerInitialized;
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x000A52EC File Offset: 0x000A34EC
	private void OnDestroy()
	{
		NetworkEvents.OnServerInitializing -= this.ServerInitializing;
		NetworkEvents.OnServerInitialized -= this.ServerInitialized;
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x000025B8 File Offset: 0x000007B8
	private void ServerInitializing()
	{
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x000025B8 File Offset: 0x000007B8
	private void ServerInitialized()
	{
	}
}
