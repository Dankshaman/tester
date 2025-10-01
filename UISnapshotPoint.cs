using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Snapshot Point")]
public class UISnapshotPoint : MonoBehaviour
{
	// Token: 0x060004B9 RID: 1209 RVA: 0x00022FD8 File Offset: 0x000211D8
	private void Start()
	{
		if (base.tag != "EditorOnly")
		{
			base.tag = "EditorOnly";
		}
	}

	// Token: 0x04000356 RID: 854
	public bool isOrthographic = true;

	// Token: 0x04000357 RID: 855
	public float nearClip = -100f;

	// Token: 0x04000358 RID: 856
	public float farClip = 100f;

	// Token: 0x04000359 RID: 857
	[Range(10f, 80f)]
	public int fieldOfView = 35;

	// Token: 0x0400035A RID: 858
	public float orthoSize = 30f;

	// Token: 0x0400035B RID: 859
	public Texture2D thumbnail;
}
