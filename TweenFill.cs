using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
[RequireComponent(typeof(UIBasicSprite))]
[AddComponentMenu("NGUI/Tween/Tween Fill")]
public class TweenFill : UITweener
{
	// Token: 0x06000536 RID: 1334 RVA: 0x0002579F File Offset: 0x0002399F
	private void Cache()
	{
		this.mCached = true;
		this.mSprite = base.GetComponent<UISprite>();
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000537 RID: 1335 RVA: 0x000257B4 File Offset: 0x000239B4
	// (set) Token: 0x06000538 RID: 1336 RVA: 0x000257E3 File Offset: 0x000239E3
	public float value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mSprite != null)
			{
				return this.mSprite.fillAmount;
			}
			return 0f;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mSprite != null)
			{
				this.mSprite.fillAmount = value;
			}
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0002580D File Offset: 0x00023A0D
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00025828 File Offset: 0x00023A28
	public static TweenFill Begin(GameObject go, float duration, float fill)
	{
		TweenFill tweenFill = UITweener.Begin<TweenFill>(go, duration, 0f);
		tweenFill.from = tweenFill.value;
		tweenFill.to = fill;
		if (duration <= 0f)
		{
			tweenFill.Sample(1f, true);
			tweenFill.enabled = false;
		}
		return tweenFill;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00025871 File Offset: 0x00023A71
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0002587F File Offset: 0x00023A7F
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x040003A3 RID: 931
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x040003A4 RID: 932
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x040003A5 RID: 933
	private bool mCached;

	// Token: 0x040003A6 RID: 934
	private UIBasicSprite mSprite;
}
