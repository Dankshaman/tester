using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	// Token: 0x060000A9 RID: 169 RVA: 0x00004FD5 File Offset: 0x000031D5
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00004FE4 File Offset: 0x000031E4
	private void OnDrag(Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		if (this.target != null)
		{
			this.target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.target.localRotation;
			return;
		}
		this.mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.mTrans.localRotation;
	}

	// Token: 0x04000076 RID: 118
	public Transform target;

	// Token: 0x04000077 RID: 119
	public float speed = 1f;

	// Token: 0x04000078 RID: 120
	private Transform mTrans;
}
