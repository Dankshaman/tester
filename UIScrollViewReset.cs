using System;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class UIScrollViewReset : MonoBehaviour
{
	// Token: 0x060026DB RID: 9947 RVA: 0x00114388 File Offset: 0x00112588
	private void OnEnable()
	{
		UIScrollView component = base.GetComponent<UIScrollView>();
		if (component)
		{
			component.ResetPosition();
		}
	}
}
