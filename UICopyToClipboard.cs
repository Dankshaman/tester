using System;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class UICopyToClipboard : MonoBehaviour
{
	// Token: 0x06002220 RID: 8736 RVA: 0x000F5903 File Offset: 0x000F3B03
	private void OnClick()
	{
		TTSUtilities.CopyToClipboard(this.CopyText);
		if (PlayerScript.Pointer)
		{
			PlayerScript.PointerScript.ResetInfoObject();
		}
	}

	// Token: 0x04001592 RID: 5522
	public string CopyText = "";
}
