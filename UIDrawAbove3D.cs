using System;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class UIDrawAbove3D : MonoBehaviour
{
	// Token: 0x060022F1 RID: 8945 RVA: 0x000F94EF File Offset: 0x000F76EF
	private void OnEnable()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -1000f);
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000F9526 File Offset: 0x000F7726
	private void OnDisable()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
	}
}
