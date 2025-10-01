using System;
using NewNet;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class UIStarTurn : MonoBehaviour
{
	// Token: 0x0600279E RID: 10142 RVA: 0x0011964D File Offset: 0x0011784D
	private void Awake()
	{
		if (Network.isClient)
		{
			base.GetComponent<UITooltipScript>().Tooltip = "Turn";
		}
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x00119666 File Offset: 0x00117866
	private void OnClick()
	{
		if (Network.isServer)
		{
			NetworkSingleton<Turns>.Instance.GUIEndTurn();
		}
	}
}
