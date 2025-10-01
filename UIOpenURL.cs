using System;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class UIOpenURL : MonoBehaviour
{
	// Token: 0x060025F0 RID: 9712 RVA: 0x0010B986 File Offset: 0x00109B86
	private void OnClick()
	{
		TTSUtilities.OpenURL(this.URL);
	}

	// Token: 0x0400189B RID: 6299
	public string URL;
}
