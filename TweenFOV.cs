using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Field of View")]
public class TweenFOV : UITweener
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x0600052A RID: 1322 RVA: 0x00025693 File Offset: 0x00023893
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

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600052B RID: 1323 RVA: 0x000256B5 File Offset: 0x000238B5
	// (set) Token: 0x0600052C RID: 1324 RVA: 0x000256BD File Offset: 0x000238BD
	[Obsolete("Use 'value' instead")]
	public float fov
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

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600052D RID: 1325 RVA: 0x000256C6 File Offset: 0x000238C6
	// (set) Token: 0x0600052E RID: 1326 RVA: 0x000256D3 File Offset: 0x000238D3
	public float value
	{
		get
		{
			return this.cachedCamera.fieldOfView;
		}
		set
		{
			this.cachedCamera.fieldOfView = value;
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x000256E1 File Offset: 0x000238E1
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00025700 File Offset: 0x00023900
	public static TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration, 0f);
		tweenFOV.from = tweenFOV.value;
		tweenFOV.to = to;
		if (duration <= 0f)
		{
			tweenFOV.Sample(1f, true);
			tweenFOV.enabled = false;
		}
		return tweenFOV;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00025749 File Offset: 0x00023949
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00025757 File Offset: 0x00023957
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00025765 File Offset: 0x00023965
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00025773 File Offset: 0x00023973
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x040003A0 RID: 928
	public float from = 45f;

	// Token: 0x040003A1 RID: 929
	public float to = 45f;

	// Token: 0x040003A2 RID: 930
	private Camera mCam;
}
