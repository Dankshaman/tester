using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class LuaTime
{
	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001418 RID: 5144 RVA: 0x00084B61 File Offset: 0x00082D61
	public float time
	{
		get
		{
			return Time.time;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001419 RID: 5145 RVA: 0x00084B68 File Offset: 0x00082D68
	public float delta_time
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x0600141A RID: 5146 RVA: 0x00084B6F File Offset: 0x00082D6F
	public float fixed_time
	{
		get
		{
			return Time.fixedTime;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x0600141B RID: 5147 RVA: 0x00084B76 File Offset: 0x00082D76
	public float fixed_delta_time
	{
		get
		{
			return Time.fixedDeltaTime;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x0600141C RID: 5148 RVA: 0x00084B7D File Offset: 0x00082D7D
	public int frame_count
	{
		get
		{
			return Time.frameCount;
		}
	}
}
