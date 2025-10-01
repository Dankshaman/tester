using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour
{
	// Token: 0x06000120 RID: 288 RVA: 0x000076AC File Offset: 0x000058AC
	protected virtual void Start()
	{
		if (this.reparentTarget == null)
		{
			this.reparentTarget = base.transform;
		}
	}

	// Token: 0x040000E5 RID: 229
	public Transform reparentTarget;
}
