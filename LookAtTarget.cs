using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x06000097 RID: 151 RVA: 0x00004A70 File Offset: 0x00002C70
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004A80 File Offset: 0x00002C80
	private void LateUpdate()
	{
		if (this.target != null)
		{
			Vector3 forward = this.target.position - this.mTrans.position;
			if (forward.magnitude > 0.001f)
			{
				Quaternion b = Quaternion.LookRotation(forward);
				this.mTrans.rotation = Quaternion.Slerp(this.mTrans.rotation, b, Mathf.Clamp01(this.speed * Time.deltaTime));
			}
		}
	}

	// Token: 0x04000063 RID: 99
	public int level;

	// Token: 0x04000064 RID: 100
	public Transform target;

	// Token: 0x04000065 RID: 101
	public float speed = 8f;

	// Token: 0x04000066 RID: 102
	private Transform mTrans;
}
