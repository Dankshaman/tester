using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/Orthographic Camera")]
public class UIOrthoCamera : MonoBehaviour
{
	// Token: 0x0600072E RID: 1838 RVA: 0x0003284D File Offset: 0x00030A4D
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x00032874 File Offset: 0x00030A74
	private void Update()
	{
		float num = this.mCam.rect.yMin * (float)Screen.height;
		float num2 = (this.mCam.rect.yMax * (float)Screen.height - num) * 0.5f * this.mTrans.lossyScale.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, num2))
		{
			this.mCam.orthographicSize = num2;
		}
	}

	// Token: 0x04000503 RID: 1283
	private Camera mCam;

	// Token: 0x04000504 RID: 1284
	private Transform mTrans;
}
