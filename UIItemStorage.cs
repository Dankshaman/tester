using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000E RID: 14
[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000054 RID: 84 RVA: 0x00003996 File Offset: 0x00001B96
	public List<InvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000039BF File Offset: 0x00001BBF
	public InvGameItem GetItem(int slot)
	{
		if (slot >= this.items.Count)
		{
			return null;
		}
		return this.mItems[slot];
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000039DD File Offset: 0x00001BDD
	public InvGameItem Replace(int slot, InvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			InvGameItem result = this.items[slot];
			this.mItems[slot] = item;
			return result;
		}
		return item;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003A04 File Offset: 0x00001C04
	private void Start()
	{
		if (this.template != null)
		{
			int num = 0;
			Bounds bounds = default(Bounds);
			for (int i = 0; i < this.maxRows; i++)
			{
				for (int j = 0; j < this.maxColumns; j++)
				{
					GameObject gameObject = base.gameObject.AddChild(this.template);
					gameObject.transform.localPosition = new Vector3((float)this.padding + ((float)j + 0.5f) * (float)this.spacing, (float)(-(float)this.padding) - ((float)i + 0.5f) * (float)this.spacing, 0f);
					UIStorageSlot component = gameObject.GetComponent<UIStorageSlot>();
					if (component != null)
					{
						component.storage = this;
						component.slot = num;
					}
					bounds.Encapsulate(new Vector3((float)this.padding * 2f + (float)((j + 1) * this.spacing), (float)(-(float)this.padding) * 2f - (float)((i + 1) * this.spacing), 0f));
					if (++num >= this.maxItemCount)
					{
						if (this.background != null)
						{
							this.background.transform.localScale = bounds.size;
						}
						return;
					}
				}
			}
			if (this.background != null)
			{
				this.background.transform.localScale = bounds.size;
			}
		}
	}

	// Token: 0x04000029 RID: 41
	public int maxItemCount = 8;

	// Token: 0x0400002A RID: 42
	public int maxRows = 4;

	// Token: 0x0400002B RID: 43
	public int maxColumns = 4;

	// Token: 0x0400002C RID: 44
	public GameObject template;

	// Token: 0x0400002D RID: 45
	public UIWidget background;

	// Token: 0x0400002E RID: 46
	public int spacing = 128;

	// Token: 0x0400002F RID: 47
	public int padding = 10;

	// Token: 0x04000030 RID: 48
	private List<InvGameItem> mItems = new List<InvGameItem>();
}
