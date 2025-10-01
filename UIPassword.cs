using System;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class UIPassword : MonoBehaviour
{
	// Token: 0x06002659 RID: 9817 RVA: 0x00111AD1 File Offset: 0x0010FCD1
	private void OnEnable()
	{
		base.GetComponentInChildren<UIInput>().value = "";
		this.SelectText();
		base.Invoke("SelectText", 0.2f);
		base.Invoke("SelectText", 0.5f);
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x00111B09 File Offset: 0x0010FD09
	private void SelectText()
	{
		base.GetComponentInChildren<UIInput>().isSelected = true;
	}
}
