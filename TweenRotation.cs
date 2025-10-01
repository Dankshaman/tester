using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x000262E0 File Offset: 0x000244E0
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x00026302 File Offset: 0x00024502
	// (set) Token: 0x0600056F RID: 1391 RVA: 0x0002630A File Offset: 0x0002450A
	[Obsolete("Use 'value' instead")]
	public Quaternion rotation
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000570 RID: 1392 RVA: 0x00026313 File Offset: 0x00024513
	// (set) Token: 0x06000571 RID: 1393 RVA: 0x00026320 File Offset: 0x00024520
	public Quaternion value
	{
		get
		{
			return this.cachedTransform.localRotation;
		}
		set
		{
			this.cachedTransform.localRotation = value;
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00026330 File Offset: 0x00024530
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = (this.quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor) : Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor))));
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000263C8 File Offset: 0x000245C8
	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration, 0f);
		tweenRotation.from = tweenRotation.value.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		if (duration <= 0f)
		{
			tweenRotation.Sample(1f, true);
			tweenRotation.enabled = false;
		}
		return tweenRotation;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00026420 File Offset: 0x00024620
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value.eulerAngles;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00026444 File Offset: 0x00024644
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value.eulerAngles;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00026465 File Offset: 0x00024665
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = Quaternion.Euler(this.from);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00026478 File Offset: 0x00024678
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = Quaternion.Euler(this.to);
	}

	// Token: 0x040003BB RID: 955
	public Vector3 from;

	// Token: 0x040003BC RID: 956
	public Vector3 to;

	// Token: 0x040003BD RID: 957
	public bool quaternionLerp;

	// Token: 0x040003BE RID: 958
	private Transform mTrans;
}
