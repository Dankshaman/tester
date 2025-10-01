using System;
using UnityEngine;

// Token: 0x02000342 RID: 834
public class UIStaticObject : MonoBehaviour
{
	// Token: 0x060027A5 RID: 10149 RVA: 0x00119757 File Offset: 0x00117957
	private void Awake()
	{
		this.StartPosition = base.transform.position;
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x0011976A File Offset: 0x0011796A
	private void Update()
	{
		base.transform.position = this.StartPosition;
	}

	// Token: 0x04001A08 RID: 6664
	private Vector3 StartPosition;
}
