using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class BoundsVisual : MonoBehaviour
{
	// Token: 0x0600084E RID: 2126 RVA: 0x0003A978 File Offset: 0x00038B78
	private void Awake()
	{
		this.mesh = base.GetComponent<MeshRenderer>();
		Material sharedMaterial = this.mesh.sharedMaterial;
		this.OffColor = new Colour(sharedMaterial.GetColor("_Color"));
		this.OffColor.a = 0f;
		this.OnColor = new Colour(sharedMaterial.GetColor("_Color"));
		this.FadeInDuration = 1f;
		this.FadeOutDuration = 2f;
		this.fadeState = BoundsVisual.FadeState.FadingOut;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0003A9F8 File Offset: 0x00038BF8
	private void Update()
	{
		if (!this.slider.activeInHierarchy && this.fadeState == BoundsVisual.FadeState.Out)
		{
			return;
		}
		this.Touching = false;
		if (UICamera.hoveredObject)
		{
			Transform[] componentsInChildren = this.slider.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject == UICamera.hoveredObject)
				{
					this.Touching = true;
					break;
				}
			}
		}
		if (this.touchUntil != 0f)
		{
			if (this.touchUntil > Time.time)
			{
				this.Touching = true;
			}
			else
			{
				this.touchUntil = 0f;
				this.Touching = false;
			}
		}
		switch (this.fadeState)
		{
		case BoundsVisual.FadeState.Out:
			if (this.Touching)
			{
				this.fadeState = BoundsVisual.FadeState.FadingIn;
				return;
			}
			break;
		case BoundsVisual.FadeState.FadingIn:
			if (!this.Touching)
			{
				this.fadeState = BoundsVisual.FadeState.FadingOut;
				return;
			}
			this.progress += Time.deltaTime / this.FadeInDuration;
			if (this.progress >= 1f)
			{
				this.progress = 1f;
				this.fadeState = BoundsVisual.FadeState.In;
			}
			this.UpdateOpacity();
			return;
		case BoundsVisual.FadeState.FadingOut:
			if (this.Touching)
			{
				this.fadeState = BoundsVisual.FadeState.FadingIn;
				return;
			}
			this.progress -= Time.deltaTime / this.FadeOutDuration;
			if (this.progress <= 0f)
			{
				this.progress = 0f;
				this.fadeState = BoundsVisual.FadeState.Out;
			}
			this.UpdateOpacity();
			break;
		case BoundsVisual.FadeState.In:
			if (!this.Touching)
			{
				this.fadeState = BoundsVisual.FadeState.FadingOut;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0003AB7C File Offset: 0x00038D7C
	private void UpdateOpacity()
	{
		this.OnColor.a = (VRHMD.isVR ? 0.33f : 0.99f);
		this.UpdateScale();
		if (this.progress == 0f)
		{
			this.mesh.enabled = false;
			return;
		}
		float num = 1f - this.progress;
		num = 1f - num * num;
		this.mesh.enabled = true;
		this.mesh.material.color = Colour.Lerp(this.OffColor, this.OnColor, num);
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0003AC14 File Offset: 0x00038E14
	public void UpdateScale()
	{
		float num = Mathf.Lerp(6f, 36f, NetworkSingleton<GameOptions>.Instance.PlayArea);
		Vector2 mainTextureScale = new Vector2(num, num);
		this.mesh.material.mainTextureScale = mainTextureScale;
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003AC55 File Offset: 0x00038E55
	public void FadeIn()
	{
		this.FadeInFor(1f);
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0003AC62 File Offset: 0x00038E62
	public void FadeInFor(float duration)
	{
		this.UpdateScale();
		this.touchUntil = Time.time + duration;
	}

	// Token: 0x040005D0 RID: 1488
	public GameObject slider;

	// Token: 0x040005D1 RID: 1489
	public float sliderValue = 0.5f;

	// Token: 0x040005D2 RID: 1490
	private const float FADE_IN_DURATION = 1f;

	// Token: 0x040005D3 RID: 1491
	private const float FADE_OUT_DURATION = 2f;

	// Token: 0x040005D4 RID: 1492
	private const float MIN_OPACITY = 0f;

	// Token: 0x040005D5 RID: 1493
	private const float MAX_OPACITY = 0.99f;

	// Token: 0x040005D6 RID: 1494
	private const float MAX_OPACITY_VR = 0.33f;

	// Token: 0x040005D7 RID: 1495
	private Colour OffColor;

	// Token: 0x040005D8 RID: 1496
	private Colour OnColor;

	// Token: 0x040005D9 RID: 1497
	private float FadeInDuration;

	// Token: 0x040005DA RID: 1498
	private float FadeOutDuration;

	// Token: 0x040005DB RID: 1499
	public BoundsVisual.FadeState fadeState;

	// Token: 0x040005DC RID: 1500
	private float progress;

	// Token: 0x040005DD RID: 1501
	public bool Touching;

	// Token: 0x040005DE RID: 1502
	private float touchUntil;

	// Token: 0x040005DF RID: 1503
	private MeshRenderer mesh;

	// Token: 0x0200057E RID: 1406
	public enum FadeState
	{
		// Token: 0x04002502 RID: 9474
		Out,
		// Token: 0x04002503 RID: 9475
		FadingIn,
		// Token: 0x04002504 RID: 9476
		FadingOut,
		// Token: 0x04002505 RID: 9477
		In
	}
}
