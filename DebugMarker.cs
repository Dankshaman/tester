using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class DebugMarker : NetworkSingleton<DebugMarker>
{
	// Token: 0x06000BF8 RID: 3064 RVA: 0x00051C1D File Offset: 0x0004FE1D
	private void Update()
	{
		if (this.hideAt > Time.time)
		{
			base.GetComponent<Renderer>().enabled = false;
			base.GetComponent<Light>().enabled = false;
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00051C44 File Offset: 0x0004FE44
	public void Show(Vector3 position, float duration = 3f)
	{
		base.transform.position = position;
		base.GetComponent<Renderer>().enabled = true;
		base.GetComponent<Light>().enabled = true;
		this.hideAt = Time.time + duration;
	}

	// Token: 0x0400083E RID: 2110
	private float hideAt;
}
