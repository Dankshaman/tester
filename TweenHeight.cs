using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Height")]
public class TweenHeight : UITweener
{
	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x000258AB File Offset: 0x00023AAB
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

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x000258CD File Offset: 0x00023ACD
	// (set) Token: 0x06000540 RID: 1344 RVA: 0x000258D5 File Offset: 0x00023AD5
	[Obsolete("Use 'value' instead")]
	public int height
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

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000541 RID: 1345 RVA: 0x000258DE File Offset: 0x00023ADE
	// (set) Token: 0x06000542 RID: 1346 RVA: 0x000258EB File Offset: 0x00023AEB
	public int value
	{
		get
		{
			return this.cachedWidget.height;
		}
		set
		{
			this.cachedWidget.height = value;
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x000258FC File Offset: 0x00023AFC
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

	// Token: 0x06000544 RID: 1348 RVA: 0x00025978 File Offset: 0x00023B78
	public static TweenHeight Begin(UIWidget widget, float duration, int height)
	{
		TweenHeight tweenHeight = UITweener.Begin<TweenHeight>(widget.gameObject, duration, 0f);
		tweenHeight.from = widget.height;
		tweenHeight.to = height;
		if (duration <= 0f)
		{
			tweenHeight.Sample(1f, true);
			tweenHeight.enabled = false;
		}
		return tweenHeight;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x000259C6 File Offset: 0x00023BC6
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000259D4 File Offset: 0x00023BD4
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x000259E2 File Offset: 0x00023BE2
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x000259F0 File Offset: 0x00023BF0
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x040003A7 RID: 935
	public int from = 100;

	// Token: 0x040003A8 RID: 936
	public int to = 100;

	// Token: 0x040003A9 RID: 937
	public bool updateTable;

	// Token: 0x040003AA RID: 938
	private UIWidget mWidget;

	// Token: 0x040003AB RID: 939
	private UITable mTable;
}
