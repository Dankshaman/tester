using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
	// Token: 0x060000A4 RID: 164 RVA: 0x00004EE5 File Offset: 0x000030E5
	private void Start()
	{
		this.mTrans = base.transform;
		this.mRb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00004EFF File Offset: 0x000030FF
	private void Update()
	{
		if (this.mRb == null)
		{
			this.ApplyDelta(this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00004F29 File Offset: 0x00003129
	private void FixedUpdate()
	{
		if (this.mRb != null)
		{
			this.ApplyDelta(Time.deltaTime);
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00004F44 File Offset: 0x00003144
	public void ApplyDelta(float delta)
	{
		delta *= 360f;
		Quaternion rhs = Quaternion.Euler(this.rotationsPerSecond * delta);
		if (this.mRb == null)
		{
			this.mTrans.rotation = this.mTrans.rotation * rhs;
			return;
		}
		this.mRb.MoveRotation(this.mRb.rotation * rhs);
	}

	// Token: 0x04000072 RID: 114
	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04000073 RID: 115
	public bool ignoreTimeScale;

	// Token: 0x04000074 RID: 116
	private Rigidbody mRb;

	// Token: 0x04000075 RID: 117
	private Transform mTrans;
}
