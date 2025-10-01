using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : MonoBehaviour
{
	// Token: 0x0600009C RID: 156 RVA: 0x00004B43 File Offset: 0x00002D43
	private void Start()
	{
		this.mTrans = base.transform;
		this.mStart = this.mTrans.localRotation;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00004B64 File Offset: 0x00002D64
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		Vector3 vector = UICamera.lastEventPosition;
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		if (this.range < 0.1f)
		{
			this.range = 0.1f;
		}
		float x = Mathf.Clamp((vector.x - num) / num / this.range, -1f, 1f);
		float y = Mathf.Clamp((vector.y - num2) / num2 / this.range, -1f, 1f);
		this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), deltaTime * 5f);
		this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.degrees.y, this.mRot.x * this.degrees.x, 0f);
	}

	// Token: 0x04000067 RID: 103
	public Vector2 degrees = new Vector2(5f, 3f);

	// Token: 0x04000068 RID: 104
	public float range = 1f;

	// Token: 0x04000069 RID: 105
	private Transform mTrans;

	// Token: 0x0400006A RID: 106
	private Quaternion mStart;

	// Token: 0x0400006B RID: 107
	private Vector2 mRot = Vector2.zero;
}
