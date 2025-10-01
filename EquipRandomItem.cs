using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000A RID: 10
[AddComponentMenu("NGUI/Examples/Equip Random Item")]
public class EquipRandomItem : MonoBehaviour
{
	// Token: 0x0600003F RID: 63 RVA: 0x000032A8 File Offset: 0x000014A8
	private void OnClick()
	{
		if (this.equipment == null)
		{
			return;
		}
		List<InvBaseItem> items = InvDatabase.list[0].items;
		if (items.Count == 0)
		{
			return;
		}
		int max = 12;
		int num = UnityEngine.Random.Range(0, items.Count);
		InvBaseItem invBaseItem = items[num];
		InvGameItem invGameItem = new InvGameItem(num, invBaseItem);
		invGameItem.quality = (InvGameItem.Quality)UnityEngine.Random.Range(0, max);
		invGameItem.itemLevel = NGUITools.RandomRange(invBaseItem.minItemLevel, invBaseItem.maxItemLevel);
		this.equipment.Equip(invGameItem);
	}

	// Token: 0x04000017 RID: 23
	public InvEquipment equipment;
}
