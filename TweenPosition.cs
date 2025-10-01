using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600055F RID: 1375 RVA: 0x000260F7 File Offset: 0x000242F7
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

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x00026119 File Offset: 0x00024319
	// (set) Token: 0x06000561 RID: 1377 RVA: 0x00026121 File Offset: 0x00024321
	[Obsolete("Use 'value' instead")]
	public Vector3 position
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

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000562 RID: 1378 RVA: 0x0002612A File Offset: 0x0002432A
	// (set) Token: 0x06000563 RID: 1379 RVA: 0x0002614C File Offset: 0x0002434C
	public Vector3 value
	{
		get
		{
			if (!this.worldSpace)
			{
				return this.cachedTransform.localPosition;
			}
			return this.cachedTransform.position;
		}
		set
		{
			if (!(this.mRect == null) && this.mRect.isAnchored && !this.worldSpace)
			{
				value -= this.cachedTransform.localPosition;
				NGUIMath.MoveRect(this.mRect, value.x, value.y);
				return;
			}
			if (this.worldSpace)
			{
				this.cachedTransform.position = value;
				return;
			}
			this.cachedTransform.localPosition = value;
		}
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x000261C8 File Offset: 0x000243C8
	private void Awake()
	{
		this.mRect = base.GetComponent<UIRect>();
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x000261D6 File Offset: 0x000243D6
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00026204 File Offset: 0x00024404
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration, 0f);
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00026250 File Offset: 0x00024450
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration, 0f);
		tweenPosition.worldSpace = worldSpace;
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x000262A0 File Offset: 0x000244A0
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x000262AE File Offset: 0x000244AE
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x000262BC File Offset: 0x000244BC
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x000262CA File Offset: 0x000244CA
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x040003B6 RID: 950
	public Vector3 from;

	// Token: 0x040003B7 RID: 951
	public Vector3 to;

	// Token: 0x040003B8 RID: 952
	[HideInInspector]
	public bool worldSpace;

	// Token: 0x040003B9 RID: 953
	private Transform mTrans;

	// Token: 0x040003BA RID: 954
	private UIRect mRect;
}
