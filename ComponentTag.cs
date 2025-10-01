using System;

// Token: 0x020000BE RID: 190
public struct ComponentTag
{
	// Token: 0x060009C2 RID: 2498 RVA: 0x0004570E File Offset: 0x0004390E
	public ComponentTag(int index, TagLabel label)
	{
		this.index = index;
		this.label = label;
	}

	// Token: 0x04000702 RID: 1794
	public int index;

	// Token: 0x04000703 RID: 1795
	public TagLabel label;
}
