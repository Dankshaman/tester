using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	// Token: 0x0600051E RID: 1310 RVA: 0x00025404 File Offset: 0x00023604
	private void Cache()
	{
		this.mCached = true;
		this.mWidget = base.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			return;
		}
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mSr != null)
		{
			return;
		}
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			this.mMat = component.material;
			return;
		}
		this.mLight = base.GetComponent<Light>();
		if (this.mLight == null)
		{
			this.mWidget = base.GetComponentInChildren<UIWidget>();
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x0600051F RID: 1311 RVA: 0x00025491 File Offset: 0x00023691
	// (set) Token: 0x06000520 RID: 1312 RVA: 0x00025499 File Offset: 0x00023699
	[Obsolete("Use 'value' instead")]
	public Color color
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

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000521 RID: 1313 RVA: 0x000254A4 File Offset: 0x000236A4
	// (set) Token: 0x06000522 RID: 1314 RVA: 0x0002552C File Offset: 0x0002372C
	public Color value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.color;
			}
			if (this.mSr != null)
			{
				return this.mSr.color;
			}
			if (this.mLight != null)
			{
				return this.mLight.color;
			}
			return Color.black;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
				return;
			}
			if (this.mMat != null)
			{
				this.mMat.color = value;
				return;
			}
			if (this.mSr != null)
			{
				this.mSr.color = value;
				return;
			}
			if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = (value.r + value.g + value.b > 0.01f);
			}
		}
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x000255D8 File Offset: 0x000237D8
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Color.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x000255F4 File Offset: 0x000237F4
	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration, 0f);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0002563D File Offset: 0x0002383D
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0002564B File Offset: 0x0002384B
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00025659 File Offset: 0x00023859
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00025667 File Offset: 0x00023867
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04000399 RID: 921
	public Color from = Color.white;

	// Token: 0x0400039A RID: 922
	public Color to = Color.white;

	// Token: 0x0400039B RID: 923
	private bool mCached;

	// Token: 0x0400039C RID: 924
	private UIWidget mWidget;

	// Token: 0x0400039D RID: 925
	private Material mMat;

	// Token: 0x0400039E RID: 926
	private Light mLight;

	// Token: 0x0400039F RID: 927
	private SpriteRenderer mSr;
}
