using System;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class UIInitializeScript : MonoBehaviour
{
	// Token: 0x06002484 RID: 9348 RVA: 0x0010165E File Offset: 0x000FF85E
	private void OnEnable()
	{
		this.target.Set(this.source.Get());
	}

	// Token: 0x0400177F RID: 6015
	public PropertyReference source;

	// Token: 0x04001780 RID: 6016
	public PropertyReference target;
}
