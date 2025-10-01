using System;
using UnityEngine;

// Token: 0x020002AE RID: 686
public class UICreditsActivate : MonoBehaviour
{
	// Token: 0x06002222 RID: 8738 RVA: 0x000F593C File Offset: 0x000F3B3C
	private void OnClick()
	{
		if (this.target != null)
		{
			Transform parent = this.target.transform.parent;
			int childCount = parent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				NGUITools.SetActive(parent.GetChild(i).gameObject, false);
			}
			NGUITools.SetActive(this.target, !this.target.activeSelf);
			if (this.target.GetComponent<UIRect>())
			{
				this.target.GetComponent<UIRect>().UpdateAnchors();
			}
			if (this.target.GetComponent<UIWidget>())
			{
				this.target.GetComponent<UIWidget>().UpdateAnchors();
			}
		}
	}

	// Token: 0x04001593 RID: 5523
	public GameObject target;
}
