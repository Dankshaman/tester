using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
	// Token: 0x06000090 RID: 144 RVA: 0x0000497F File Offset: 0x00002B7F
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0000498C File Offset: 0x00002B8C
	private void Interpolate(float delta)
	{
		if (this.mTrans != null)
		{
			Transform parent = this.mTrans.parent;
			if (parent != null)
			{
				this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, delta * this.speed);
				this.mTrans.rotation = this.mAbsolute;
			}
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000049F7 File Offset: 0x00002BF7
	private void Start()
	{
		this.mTrans = base.transform;
		this.mRelative = this.mTrans.localRotation;
		this.mAbsolute = this.mTrans.rotation;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00004A27 File Offset: 0x00002C27
	private void Update()
	{
		this.Interpolate(this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
	}

	// Token: 0x0400005D RID: 93
	public float speed = 10f;

	// Token: 0x0400005E RID: 94
	public bool ignoreTimeScale;

	// Token: 0x0400005F RID: 95
	private Transform mTrans;

	// Token: 0x04000060 RID: 96
	private Quaternion mRelative;

	// Token: 0x04000061 RID: 97
	private Quaternion mAbsolute;
}
