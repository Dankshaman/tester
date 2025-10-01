using System;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class UICalculatorButton : MonoBehaviour
{
	// Token: 0x06002158 RID: 8536 RVA: 0x000F081A File Offset: 0x000EEA1A
	private void OnClick()
	{
		base.transform.parent.parent.parent.GetComponent<CalculatorScript>().ButtonPress(base.gameObject.name);
	}
}
