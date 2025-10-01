using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
	// Token: 0x060000B5 RID: 181 RVA: 0x000052EC File Offset: 0x000034EC
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mLastPos = this.mTrans.position;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000530C File Offset: 0x0000350C
	private void Update()
	{
		Vector3 vector = this.mTrans.position - this.mLastPos;
		this.mLastPos = this.mTrans.position;
		this.mAngle += vector.x * this.degrees;
		this.mAngle = NGUIMath.SpringLerp(this.mAngle, 0f, 20f, Time.deltaTime);
		this.mTrans.localRotation = Quaternion.Euler(0f, 0f, -this.mAngle);
	}

	// Token: 0x04000081 RID: 129
	public int updateOrder;

	// Token: 0x04000082 RID: 130
	public float degrees = 30f;

	// Token: 0x04000083 RID: 131
	private Vector3 mLastPos;

	// Token: 0x04000084 RID: 132
	private Transform mTrans;

	// Token: 0x04000085 RID: 133
	private float mAngle;
}
