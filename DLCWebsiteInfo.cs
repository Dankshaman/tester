using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000ED RID: 237
[Serializable]
public class DLCWebsiteInfo : DLCWebsiteBaseInfo
{
	// Token: 0x04000822 RID: 2082
	public string AssetBundleURL;

	// Token: 0x04000823 RID: 2083
	public string ThumbnailURL;

	// Token: 0x04000824 RID: 2084
	public bool New;

	// Token: 0x04000825 RID: 2085
	public bool HideIfNotOwned;

	// Token: 0x04000826 RID: 2086
	public List<DLCWebsiteBaseInfo> Expansions;

	// Token: 0x04000827 RID: 2087
	[HideInInspector]
	[NonSerialized]
	public int DiscountPercent;
}
