using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Width")]
public class TweenWidth : UITweener
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000593 RID: 1427 RVA: 0x0002697E File Offset: 0x00024B7E
	public UIWidget cachedWidget
	{
		get
		{
			if (this.mWidget == null)
			{
				this.mWidget = base.GetComponent<UIWidget>();
			}
			return this.mWidget;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000594 RID: 1428 RVA: 0x000269A0 File Offset: 0x00024BA0
	// (set) Token: 0x06000595 RID: 1429 RVA: 0x000269A8 File Offset: 0x00024BA8
	[Obsolete("Use 'value' instead")]
	public int width
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

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000596 RID: 1430 RVA: 0x000269B1 File Offset: 0x00024BB1
	// (set) Token: 0x06000597 RID: 1431 RVA: 0x000269BE File Offset: 0x00024BBE
	public int value
	{
		get
		{
			return this.cachedWidget.width;
		}
		set
		{
			this.cachedWidget.width = value;
		}
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x000269CC File Offset: 0x00024BCC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.RoundToInt((float)this.from * (1f - factor) + (float)this.to * factor);
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

	// Token: 0x06000599 RID: 1433 RVA: 0x00026A48 File Offset: 0x00024C48
	public static TweenWidth Begin(UIWidget widget, float duration, int width)
	{
		TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration, 0f);
		tweenWidth.from = widget.width;
		tweenWidth.to = width;
		if (duration <= 0f)
		{
			tweenWidth.Sample(1f, true);
			tweenWidth.enabled = false;
		}
		return tweenWidth;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00026A96 File Offset: 0x00024C96
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00026AA4 File Offset: 0x00024CA4
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00026AB2 File Offset: 0x00024CB2
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00026AC0 File Offset: 0x00024CC0
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x040003CE RID: 974
	public int from = 100;

	// Token: 0x040003CF RID: 975
	public int to = 100;

	// Token: 0x040003D0 RID: 976
	public bool updateTable;

	// Token: 0x040003D1 RID: 977
	private UIWidget mWidget;

	// Token: 0x040003D2 RID: 978
	private UITable mTable;
}
