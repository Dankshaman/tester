using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class AttachTo : MonoBehaviour
{
	// Token: 0x06000837 RID: 2103 RVA: 0x0003A2B0 File Offset: 0x000384B0
	private void Update()
	{
		base.gameObject.transform.position = this.AttachedToObject.transform.position;
		base.gameObject.transform.rotation = this.AttachedToObject.transform.rotation;
	}

	// Token: 0x040005BD RID: 1469
	public GameObject AttachedToObject;
}
