using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class OpenURLOnClick : MonoBehaviour
{
	// Token: 0x0600009A RID: 154 RVA: 0x00004B0C File Offset: 0x00002D0C
	private void OnClick()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component != null)
		{
			string urlAtPosition = component.GetUrlAtPosition(UICamera.lastWorldPosition);
			if (!string.IsNullOrEmpty(urlAtPosition))
			{
				Application.OpenURL(urlAtPosition);
			}
		}
	}
}
