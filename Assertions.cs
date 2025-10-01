using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class Assertions : MonoBehaviour
{
	// Token: 0x06000831 RID: 2097 RVA: 0x0003A21D File Offset: 0x0003841D
	private void Awake()
	{
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0003A228 File Offset: 0x00038428
	private void checkLayers()
	{
		for (int i = 0; i < 32; i++)
		{
			LayerMask.LayerToName(i);
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0003A249 File Offset: 0x00038449
	private bool layerMatchesName(int layer, string name)
	{
		this.LayerNames[name] = layer;
		return layer == LayerMask.NameToLayer(name);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0003A261 File Offset: 0x00038461
	private void checkPlayerActions()
	{
		this.playerActionIndex = -1;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0003A26C File Offset: 0x0003846C
	private string nextPlayerActionName()
	{
		this.playerActionIndex++;
		PlayerAction playerAction = (PlayerAction)this.playerActionIndex;
		return playerAction.ToString();
	}

	// Token: 0x040005BB RID: 1467
	private Dictionary<string, int> LayerNames = new Dictionary<string, int>();

	// Token: 0x040005BC RID: 1468
	private int playerActionIndex;
}
