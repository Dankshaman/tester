using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	// Token: 0x06000136 RID: 310 RVA: 0x00007F48 File Offset: 0x00006148
	private void OnEnable()
	{
		UIDragDropRoot.prevroot = UIDragDropRoot.root;
		UIDragDropRoot.root = base.transform;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00007F5F File Offset: 0x0000615F
	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			if (UIDragDropRoot.root == UIDragDropRoot.prevroot)
			{
				UIDragDropRoot.root = null;
				return;
			}
			UIDragDropRoot.root = UIDragDropRoot.prevroot;
		}
	}

	// Token: 0x040000F8 RID: 248
	public static Transform root;

	// Token: 0x040000F9 RID: 249
	public static Transform prevroot;
}
