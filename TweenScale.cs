using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000579 RID: 1401 RVA: 0x0002648B File Offset: 0x0002468B
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

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600057A RID: 1402 RVA: 0x000264AD File Offset: 0x000246AD
	// (set) Token: 0x0600057B RID: 1403 RVA: 0x000264BA File Offset: 0x000246BA
	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x0600057C RID: 1404 RVA: 0x000264C8 File Offset: 0x000246C8
	// (set) Token: 0x0600057D RID: 1405 RVA: 0x000264D0 File Offset: 0x000246D0
	[Obsolete("Use 'value' instead")]
	public Vector3 scale
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

	// Token: 0x0600057E RID: 1406 RVA: 0x000264DC File Offset: 0x000246DC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0002655C File Offset: 0x0002475C
	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration, 0f);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x000265A5 File Offset: 0x000247A5
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x000265B3 File Offset: 0x000247B3
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x000265C1 File Offset: 0x000247C1
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x000265CF File Offset: 0x000247CF
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x040003BF RID: 959
	public Vector3 from = Vector3.one;

	// Token: 0x040003C0 RID: 960
	public Vector3 to = Vector3.one;

	// Token: 0x040003C1 RID: 961
	public bool updateTable;

	// Token: 0x040003C2 RID: 962
	private Transform mTrans;

	// Token: 0x040003C3 RID: 963
	private UITable mTable;
}
