using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class UIHoverEnlarge : MonoBehaviour
{
	// Token: 0x06002450 RID: 9296 RVA: 0x00100804 File Offset: 0x000FEA04
	private void Awake()
	{
		this.StartScale = base.transform.localScale;
		this.HoverObjects.Add(base.gameObject);
		UICamera.onHover = (UICamera.BoolDelegate)Delegate.Combine(UICamera.onHover, new UICamera.BoolDelegate(this.OnAnyHover));
	}

	// Token: 0x06002451 RID: 9297 RVA: 0x000025B8 File Offset: 0x000007B8
	private void OnEnable()
	{
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x00100853 File Offset: 0x000FEA53
	private void OnDestroy()
	{
		UICamera.onHover = (UICamera.BoolDelegate)Delegate.Remove(UICamera.onHover, new UICamera.BoolDelegate(this.OnAnyHover));
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x00100875 File Offset: 0x000FEA75
	private void OnAnyHover(GameObject GO, bool hover)
	{
		if (!base.gameObject.activeInHierarchy || !base.enabled)
		{
			return;
		}
		if (this.HoverObjects.Contains(GO))
		{
			this.Hover(hover);
		}
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x001008A4 File Offset: 0x000FEAA4
	private void Hover(bool hover)
	{
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
		this.coroutine = base.StartCoroutine(this.SmoothScale());
		if (this.InterceptTypedNumbers)
		{
			if (hover)
			{
				Pointer.TypedNumberIntercept = base.transform;
				return;
			}
			if (Pointer.TypedNumberIntercept == base.transform)
			{
				Pointer.TypedNumberIntercept = null;
			}
		}
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x00100906 File Offset: 0x000FEB06
	private IEnumerator SmoothScale()
	{
		Vector3 scale = this.StartScale;
		this.scaling = true;
		do
		{
			scale = (this.HoverObjects.Contains(UICamera.HoveredUIObject) ? (this.StartScale * this.HoverSize) : this.StartScale);
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, scale, this.ResizeSpeed * Time.deltaTime);
			yield return null;
		}
		while (Vector3.Distance(base.transform.localScale, scale) > 0.001f);
		base.transform.localScale = scale;
		this.scaling = false;
		yield break;
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x00100915 File Offset: 0x000FEB15
	public bool isScaling()
	{
		return this.scaling;
	}

	// Token: 0x06002457 RID: 9303 RVA: 0x0010091D File Offset: 0x000FEB1D
	private void OnDisable()
	{
		base.transform.localScale = this.StartScale;
		this.scaling = false;
		if (Pointer.TypedNumberIntercept == base.transform)
		{
			Pointer.TypedNumberIntercept = null;
		}
	}

	// Token: 0x0400174E RID: 5966
	public float HoverSize = 1.2f;

	// Token: 0x0400174F RID: 5967
	public float ResizeSpeed = 10f;

	// Token: 0x04001750 RID: 5968
	public bool InterceptTypedNumbers;

	// Token: 0x04001751 RID: 5969
	public List<GameObject> HoverObjects = new List<GameObject>();

	// Token: 0x04001752 RID: 5970
	private Vector3 StartScale;

	// Token: 0x04001753 RID: 5971
	private Coroutine coroutine;

	// Token: 0x04001754 RID: 5972
	private bool scaling;
}
