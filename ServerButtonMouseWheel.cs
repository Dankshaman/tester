using System;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class ServerButtonMouseWheel : MonoBehaviour
{
	// Token: 0x06002126 RID: 8486 RVA: 0x000EFACA File Offset: 0x000EDCCA
	private void Awake()
	{
		if (this.browser == null)
		{
			this.browser = NGUITools.FindInParents<UIServerBrowser>(base.gameObject);
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000EFAEB File Offset: 0x000EDCEB
	private void OnScroll(float delta)
	{
		if (zInput.GetButton("Alt", ControlType.All))
		{
			return;
		}
		if (this.browser && NGUITools.GetActive(this))
		{
			this.browser.DoScroll(delta * UIServerBrowser.SCROLL_WHEEL_FACTOR);
		}
	}

	// Token: 0x04001475 RID: 5237
	public UIServerBrowser browser;

	// Token: 0x04001476 RID: 5238
	public bool bMouseDrag;
}
