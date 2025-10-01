using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class LagPosition : MonoBehaviour
{
	// Token: 0x06000088 RID: 136 RVA: 0x000047F6 File Offset: 0x000029F6
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00004804 File Offset: 0x00002A04
	private void Interpolate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			Vector3 vector = parent.position + parent.rotation * this.mRelative;
			this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector.x, Mathf.Clamp01(delta * this.speed.x));
			this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector.y, Mathf.Clamp01(delta * this.speed.y));
			this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector.z, Mathf.Clamp01(delta * this.speed.z));
			this.mTrans.position = this.mAbsolute;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x000048F0 File Offset: 0x00002AF0
	private void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x000048FE File Offset: 0x00002AFE
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.ResetPosition();
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0000490E File Offset: 0x00002B0E
	private void Start()
	{
		this.mStarted = true;
		this.ResetPosition();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0000491D File Offset: 0x00002B1D
	public void ResetPosition()
	{
		this.mAbsolute = this.mTrans.position;
		this.mRelative = this.mTrans.localPosition;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00004941 File Offset: 0x00002B41
	private void Update()
	{
		this.Interpolate(this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
	}

	// Token: 0x04000057 RID: 87
	public Vector3 speed = new Vector3(10f, 10f, 10f);

	// Token: 0x04000058 RID: 88
	public bool ignoreTimeScale;

	// Token: 0x04000059 RID: 89
	private Transform mTrans;

	// Token: 0x0400005A RID: 90
	private Vector3 mRelative;

	// Token: 0x0400005B RID: 91
	private Vector3 mAbsolute;

	// Token: 0x0400005C RID: 92
	private bool mStarted;
}
