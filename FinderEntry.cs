using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class FinderEntry
{
	// Token: 0x06000DE5 RID: 3557 RVA: 0x0005984B File Offset: 0x00057A4B
	public FinderEntry(Transform target, Transform window, FinderCategory category, string keyText)
	{
		this.targetItem = target;
		this.windowItem = window;
		this.keyText = keyText;
		this.category = category;
	}

	// Token: 0x04000906 RID: 2310
	public Transform targetItem;

	// Token: 0x04000907 RID: 2311
	public Transform windowItem;

	// Token: 0x04000908 RID: 2312
	public string keyText;

	// Token: 0x04000909 RID: 2313
	public string additionalText;

	// Token: 0x0400090A RID: 2314
	public FinderCategory category;
}
