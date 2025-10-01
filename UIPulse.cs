using System;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class UIPulse : MonoBehaviour
{
	// Token: 0x0600269B RID: 9883 RVA: 0x0011314C File Offset: 0x0011134C
	private void Awake()
	{
		this.StartScale = base.transform.localScale;
		this.hoverEnlarge = base.GetComponent<UIHoverEnlarge>();
	}

	// Token: 0x0600269C RID: 9884 RVA: 0x0011316C File Offset: 0x0011136C
	private void Update()
	{
		if (this.hoverEnlarge && (this.hoverEnlarge.isScaling() || UICamera.HoveredUIObject == base.gameObject))
		{
			return;
		}
		if (this.Mode == PulseMode.Fade)
		{
			float num = (Time.time - this.StartTime) / this.Duration;
			num *= num * num;
			base.transform.localScale = Vector3.Lerp(this.StartScale, Vector3.zero, num);
			return;
		}
		if (UIPulse.bEnlarge)
		{
			this.TargetLarge = this.StartScale * this.PulseSize;
			if (Mathf.Abs(base.transform.localScale.x - this.TargetLarge.x) < 0.01f)
			{
				UIPulse.bEnlarge = false;
			}
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.TargetLarge, this.ResizeSpeed * Time.deltaTime);
			return;
		}
		this.TargetSmall = this.StartScale / this.PulseSize;
		if (Mathf.Abs(base.transform.localScale.x - this.TargetSmall.x) < 0.01f)
		{
			UIPulse.bEnlarge = true;
		}
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.TargetSmall, this.ResizeSpeed * Time.deltaTime);
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x001132D2 File Offset: 0x001114D2
	private void OnDisable()
	{
		base.transform.localScale = this.StartScale;
	}

	// Token: 0x0400192B RID: 6443
	public float PulseSize = 1.05f;

	// Token: 0x0400192C RID: 6444
	public float ResizeSpeed = 2f;

	// Token: 0x0400192D RID: 6445
	public PulseMode Mode;

	// Token: 0x0400192E RID: 6446
	public float StartTime;

	// Token: 0x0400192F RID: 6447
	public float Duration;

	// Token: 0x04001930 RID: 6448
	private static bool bEnlarge = true;

	// Token: 0x04001931 RID: 6449
	private Vector3 StartScale;

	// Token: 0x04001932 RID: 6450
	private Vector3 TargetSmall;

	// Token: 0x04001933 RID: 6451
	private Vector3 TargetLarge;

	// Token: 0x04001934 RID: 6452
	private UIHoverEnlarge hoverEnlarge;
}
