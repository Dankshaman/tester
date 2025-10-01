using System;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class UISnapDot : MonoBehaviour
{
	// Token: 0x06002772 RID: 10098 RVA: 0x001187E2 File Offset: 0x001169E2
	private void OnEnable()
	{
		this.SnapPoint = base.GetComponent<UIAttachToObject>().AttachTo.GetComponent<SnapPoint>();
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x001187FA File Offset: 0x001169FA
	private void OnClick()
	{
		if (!this.wasDragging)
		{
			NetworkSingleton<SnapPointManager>.Instance.RemoveSnapPoint(this.SnapPoint);
		}
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x00118814 File Offset: 0x00116A14
	private void OnAltClick()
	{
		if (!this.wasDragging)
		{
			NetworkSingleton<SnapPointManager>.Instance.EditSnapPointTags(this.SnapPoint);
		}
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x0011882E File Offset: 0x00116A2E
	private void OnPress(bool pressed)
	{
		if (!pressed && this.wasDragging)
		{
			this.wasDragging = false;
			NetworkSingleton<SnapPointManager>.Instance.UpdateSnapPoint(this.SnapPoint);
		}
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x00118852 File Offset: 0x00116A52
	private void OnDrag()
	{
		this.wasDragging = true;
		this.SnapPoint.transform.position = HoverScript.PointerPosition;
	}

	// Token: 0x040019E7 RID: 6631
	private SnapPoint SnapPoint;

	// Token: 0x040019E8 RID: 6632
	private bool wasDragging;
}
