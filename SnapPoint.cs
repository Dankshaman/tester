using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000230 RID: 560
public class SnapPoint : MonoBehaviour
{
	// Token: 0x06001BC7 RID: 7111 RVA: 0x000BEEAF File Offset: 0x000BD0AF
	public bool TagsAllowActingUpon(NetworkPhysicsObject npo)
	{
		return !ComponentTags.HasAnyFlag(this.tags) || ComponentTags.HaveMatchingFlag(this.tags, npo.tags);
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000BEED1 File Offset: 0x000BD0D1
	public void SetTag(int index, bool isEnabled)
	{
		ComponentTags.SetFlag(ref this.tags, index, isEnabled);
		NetworkSingleton<SnapPointManager>.Instance.UpdateSnapPoint(this);
	}

	// Token: 0x0400117F RID: 4479
	public bool bRotate;

	// Token: 0x04001180 RID: 4480
	public List<ulong> tags = new List<ulong>();
}
