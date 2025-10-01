using System;

// Token: 0x020000BD RID: 189
public struct TagLabel
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x000456E5 File Offset: 0x000438E5
	public TagLabel(string label)
	{
		this.displayed = label;
		this.normalized = ((label == "") ? "" : LibString.NormalizedTag(label));
	}

	// Token: 0x04000700 RID: 1792
	public string displayed;

	// Token: 0x04000701 RID: 1793
	public string normalized;
}
