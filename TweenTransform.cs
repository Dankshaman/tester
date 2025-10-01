using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
	// Token: 0x06000585 RID: 1413 RVA: 0x000265FC File Offset: 0x000247FC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
				this.mPos = this.mTrans.position;
				this.mRot = this.mTrans.rotation;
				this.mScale = this.mTrans.localScale;
			}
			if (this.from != null)
			{
				this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
			}
			else
			{
				this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
			}
			if (this.parentWhenFinished && isFinished)
			{
				this.mTrans.parent = this.to;
			}
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x000267B7 File Offset: 0x000249B7
	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x000267C4 File Offset: 0x000249C4
	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration, 0f);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.enabled = false;
		}
		return tweenTransform;
	}

	// Token: 0x040003C4 RID: 964
	public Transform from;

	// Token: 0x040003C5 RID: 965
	public Transform to;

	// Token: 0x040003C6 RID: 966
	public bool parentWhenFinished;

	// Token: 0x040003C7 RID: 967
	private Transform mTrans;

	// Token: 0x040003C8 RID: 968
	private Vector3 mPos;

	// Token: 0x040003C9 RID: 969
	private Quaternion mRot;

	// Token: 0x040003CA RID: 970
	private Vector3 mScale;
}
