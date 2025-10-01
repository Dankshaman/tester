using System;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class UILabelClickURL : MonoBehaviour
{
	// Token: 0x060024C5 RID: 9413 RVA: 0x00103980 File Offset: 0x00101B80
	private void OnClick()
	{
		string urlAtPosition = base.GetComponent<UILabel>().GetUrlAtPosition(UICamera.lastWorldPosition);
		if (!string.IsNullOrEmpty(urlAtPosition))
		{
			Debug.Log("UILabelClickURL: " + urlAtPosition);
		}
	}
}
