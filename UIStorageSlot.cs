using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000059 RID: 89 RVA: 0x00003BA9 File Offset: 0x00001DA9
	protected override InvGameItem observedItem
	{
		get
		{
			if (!(this.storage != null))
			{
				return null;
			}
			return this.storage.GetItem(this.slot);
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003BCC File Offset: 0x00001DCC
	protected override InvGameItem Replace(InvGameItem item)
	{
		if (!(this.storage != null))
		{
			return item;
		}
		return this.storage.Replace(this.slot, item);
	}

	// Token: 0x04000031 RID: 49
	public UIItemStorage storage;

	// Token: 0x04000032 RID: 50
	public int slot;
}
