using System;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class UIOnSubmit : MonoBehaviour
{
	// Token: 0x060025ED RID: 9709 RVA: 0x0010B927 File Offset: 0x00109B27
	private void Update()
	{
		if (UICamera.selectedObject == base.gameObject && (TTSInput.GetKeyDown(KeyCode.Return) || TTSInput.GetKeyDown(KeyCode.KeypadEnter) || UIOnScreenKeyboard.EnterPressed))
		{
			this.OnSubmit();
		}
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x0010B95D File Offset: 0x00109B5D
	private void OnSubmit()
	{
		Debug.Log("OnSubmit!");
		if (this.TargetOnClick)
		{
			this.TargetOnClick.SendMessage("OnClick");
		}
	}

	// Token: 0x0400189A RID: 6298
	public GameObject TargetOnClick;
}
