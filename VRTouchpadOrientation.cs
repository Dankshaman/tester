using System;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class VRTouchpadOrientation : MonoBehaviour
{
	// Token: 0x06002995 RID: 10645 RVA: 0x001238CC File Offset: 0x00121ACC
	private void Update()
	{
		if (this.trackpad)
		{
			base.transform.rotation = this.trackpad.rotation * this.offset;
			return;
		}
		this.trackpad = base.transform.parent.parent.parent.Find("Model/trackpad");
		this.offset = Quaternion.Inverse(this.trackpad.rotation) * base.transform.rotation;
	}

	// Token: 0x04001BA6 RID: 7078
	private Transform trackpad;

	// Token: 0x04001BA7 RID: 7079
	private Quaternion offset;
}
