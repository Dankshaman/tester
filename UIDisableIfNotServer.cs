using System;
using NewNet;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class UIDisableIfNotServer : MonoBehaviour
{
	// Token: 0x060022EF RID: 8943 RVA: 0x000F94DD File Offset: 0x000F76DD
	private void OnEnable()
	{
		base.gameObject.SetActive(Network.isServer);
	}
}
