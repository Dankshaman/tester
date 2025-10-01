using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class MinMaxRangeAttribute : PropertyAttribute
{
	// Token: 0x0600033A RID: 826 RVA: 0x00014F28 File Offset: 0x00013128
	public MinMaxRangeAttribute(float minLimit, float maxLimit)
	{
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}

	// Token: 0x040002A6 RID: 678
	public float minLimit;

	// Token: 0x040002A7 RID: 679
	public float maxLimit;
}
