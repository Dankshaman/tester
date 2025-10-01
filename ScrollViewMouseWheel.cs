using System;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class ScrollViewMouseWheel : MonoBehaviour
{
	// Token: 0x06002119 RID: 8473 RVA: 0x000EF7DE File Offset: 0x000ED9DE
	private void Awake()
	{
		if (this.scrollView == null)
		{
			this.scrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x000EF7FF File Offset: 0x000ED9FF
	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this) && this.bMouseDrag)
		{
			this.scrollView.Drag();
		}
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x000EF82C File Offset: 0x000EDA2C
	private void OnScroll(float delta)
	{
		if (zInput.GetButton("Alt", ControlType.All))
		{
			return;
		}
		if (this.scrollView == null)
		{
			this.scrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	// Token: 0x0400146F RID: 5231
	public UIScrollView scrollView;

	// Token: 0x04001470 RID: 5232
	public bool bMouseDrag;
}
