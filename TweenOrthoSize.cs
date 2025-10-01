using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Orthographic Size")]
public class TweenOrthoSize : UITweener
{
	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000555 RID: 1365 RVA: 0x00026006 File Offset: 0x00024206
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000556 RID: 1366 RVA: 0x00026028 File Offset: 0x00024228
	// (set) Token: 0x06000557 RID: 1367 RVA: 0x00026030 File Offset: 0x00024230
	[Obsolete("Use 'value' instead")]
	public float orthoSize
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

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000558 RID: 1368 RVA: 0x00026039 File Offset: 0x00024239
	// (set) Token: 0x06000559 RID: 1369 RVA: 0x00026046 File Offset: 0x00024246
	public float value
	{
		get
		{
			return this.cachedCamera.orthographicSize;
		}
		set
		{
			this.cachedCamera.orthographicSize = value;
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x00026054 File Offset: 0x00024254
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00026074 File Offset: 0x00024274
	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration, 0f);
		tweenOrthoSize.from = tweenOrthoSize.value;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000260BD File Offset: 0x000242BD
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000260CB File Offset: 0x000242CB
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x040003B3 RID: 947
	public float from = 1f;

	// Token: 0x040003B4 RID: 948
	public float to = 1f;

	// Token: 0x040003B5 RID: 949
	private Camera mCam;
}
