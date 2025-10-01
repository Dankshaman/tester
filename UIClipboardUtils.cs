using System;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class UIClipboardUtils : MonoBehaviour
{
	// Token: 0x0600217B RID: 8571 RVA: 0x000F1568 File Offset: 0x000EF768
	public void PasteClipboardToInput()
	{
		UIInput component = base.GetComponent<UIInput>();
		if (component)
		{
			component.value = GUIUtility.systemCopyBuffer;
		}
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000F1590 File Offset: 0x000EF790
	public void CopyInputToClipboard()
	{
		UIInput component = base.GetComponent<UIInput>();
		if (component)
		{
			GUIUtility.systemCopyBuffer = component.value;
		}
	}
}
