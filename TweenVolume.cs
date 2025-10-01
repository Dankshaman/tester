using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("NGUI/Tween/Tween Volume")]
public class TweenVolume : UITweener
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x00026808 File Offset: 0x00024A08
	public AudioSource audioSource
	{
		get
		{
			if (this.mSource == null)
			{
				this.mSource = base.GetComponent<AudioSource>();
				if (this.mSource == null)
				{
					this.mSource = base.GetComponent<AudioSource>();
					if (this.mSource == null)
					{
						Debug.LogError("TweenVolume needs an AudioSource to work with", this);
						base.enabled = false;
					}
				}
			}
			return this.mSource;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x0002686F File Offset: 0x00024A6F
	// (set) Token: 0x0600058B RID: 1419 RVA: 0x00026877 File Offset: 0x00024A77
	[Obsolete("Use 'value' instead")]
	public float volume
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

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x00026880 File Offset: 0x00024A80
	// (set) Token: 0x0600058D RID: 1421 RVA: 0x000268A1 File Offset: 0x00024AA1
	public float value
	{
		get
		{
			if (!(this.audioSource != null))
			{
				return 0f;
			}
			return this.mSource.volume;
		}
		set
		{
			if (this.audioSource != null)
			{
				this.mSource.volume = value;
			}
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x000268BD File Offset: 0x00024ABD
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		this.mSource.enabled = (this.mSource.volume > 0.01f);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x000268FC File Offset: 0x00024AFC
	public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
	{
		TweenVolume tweenVolume = UITweener.Begin<TweenVolume>(go, duration, 0f);
		tweenVolume.from = tweenVolume.value;
		tweenVolume.to = targetVolume;
		if (targetVolume > 0f)
		{
			AudioSource audioSource = tweenVolume.audioSource;
			audioSource.enabled = true;
			audioSource.Play();
		}
		return tweenVolume;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00026944 File Offset: 0x00024B44
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00026952 File Offset: 0x00024B52
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x040003CB RID: 971
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x040003CC RID: 972
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x040003CD RID: 973
	private AudioSource mSource;
}
