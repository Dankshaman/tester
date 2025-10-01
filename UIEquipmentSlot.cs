using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003590 File Offset: 0x00001790
	protected override InvGameItem observedItem
	{
		get
		{
			if (!(this.equipment != null))
			{
				return null;
			}
			return this.equipment.GetItem(this.slot);
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000035B3 File Offset: 0x000017B3
	protected override InvGameItem Replace(InvGameItem item)
	{
		if (!(this.equipment != null))
		{
			return item;
		}
		return this.equipment.Replace(this.slot, item);
	}

	// Token: 0x0400001E RID: 30
	public InvEquipment equipment;

	// Token: 0x0400001F RID: 31
	public InvBaseItem.Slot slot;
}
