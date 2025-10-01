using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000514 RID: 1300 RVA: 0x00025173 File Offset: 0x00023373
	// (set) Token: 0x06000515 RID: 1301 RVA: 0x0002517B File Offset: 0x0002337B
	[Obsolete("Use 'value' instead")]
	public float alpha
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

	// Token: 0x06000516 RID: 1302 RVA: 0x00025184 File Offset: 0x00023384
	private void Cache()
	{
		this.mCached = true;
		this.mRect = base.GetComponent<UIRect>();
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mRect == null && this.mSr == null)
		{
			this.mLight = base.GetComponent<Light>();
			if (this.mLight == null)
			{
				Renderer component = base.GetComponent<Renderer>();
				if (component != null)
				{
					this.mMat = component.material;
				}
				if (this.mMat == null)
				{
					this.mRect = base.GetComponentInChildren<UIRect>();
					return;
				}
			}
			else
			{
				this.mBaseIntensity = this.mLight.intensity;
			}
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000517 RID: 1303 RVA: 0x00025230 File Offset: 0x00023430
	// (set) Token: 0x06000518 RID: 1304 RVA: 0x000252A8 File Offset: 0x000234A8
	public float value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				return this.mRect.alpha;
			}
			if (this.mSr != null)
			{
				return this.mSr.color.a;
			}
			if (!(this.mMat != null))
			{
				return 1f;
			}
			return this.mMat.color.a;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				this.mRect.alpha = value;
				return;
			}
			if (this.mSr != null)
			{
				Color color = this.mSr.color;
				color.a = value;
				this.mSr.color = color;
				return;
			}
			if (this.mMat != null)
			{
				Color color2 = this.mMat.color;
				color2.a = value;
				this.mMat.color = color2;
				return;
			}
			if (this.mLight != null)
			{
				this.mLight.intensity = this.mBaseIntensity * value;
			}
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0002535D File Offset: 0x0002355D
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00025378 File Offset: 0x00023578
	public static TweenAlpha Begin(GameObject go, float duration, float alpha, float delay = 0f)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration, delay);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x000253BD File Offset: 0x000235BD
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x000253CB File Offset: 0x000235CB
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04000391 RID: 913
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04000392 RID: 914
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04000393 RID: 915
	private bool mCached;

	// Token: 0x04000394 RID: 916
	private UIRect mRect;

	// Token: 0x04000395 RID: 917
	private Material mMat;

	// Token: 0x04000396 RID: 918
	private Light mLight;

	// Token: 0x04000397 RID: 919
	private SpriteRenderer mSr;

	// Token: 0x04000398 RID: 920
	private float mBaseIntensity = 1f;
}
