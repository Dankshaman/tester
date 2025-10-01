using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : MonoBehaviour
{
	// Token: 0x060000B1 RID: 177 RVA: 0x0000522E File Offset: 0x0000342E
	private void OnDisable()
	{
		this.mTrans.localRotation = Quaternion.identity;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005240 File Offset: 0x00003440
	private void OnEnable()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mTrans = base.transform;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00005274 File Offset: 0x00003474
	private void Update()
	{
		if (this.uiCamera != null)
		{
			Vector3 vector = this.uiCamera.WorldToViewportPoint(this.mTrans.position);
			this.mTrans.localRotation = Quaternion.Euler(0f, (vector.x * 2f - 1f) * this.yawAmount, 0f);
		}
	}

	// Token: 0x0400007D RID: 125
	public int updateOrder;

	// Token: 0x0400007E RID: 126
	public Camera uiCamera;

	// Token: 0x0400007F RID: 127
	public float yawAmount = 20f;

	// Token: 0x04000080 RID: 128
	private Transform mTrans;
}
