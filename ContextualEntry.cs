using System;
using System.Collections.Generic;

// Token: 0x020002A6 RID: 678
public class ContextualEntry
{
	// Token: 0x0600220B RID: 8715 RVA: 0x000F5079 File Offset: 0x000F3279
	public ContextualEntry(ContextualType Type, string Name, string SpriteName, List<ContextualEntry> SubContextualEntries)
	{
		this.Type = Type;
		this.Name = Name;
		this.SpriteName = SpriteName;
		this.SubContextualEntries = SubContextualEntries;
	}

	// Token: 0x04001576 RID: 5494
	public ContextualType Type;

	// Token: 0x04001577 RID: 5495
	public string Name;

	// Token: 0x04001578 RID: 5496
	public string SpriteName;

	// Token: 0x04001579 RID: 5497
	public List<ContextualEntry> SubContextualEntries;
}
