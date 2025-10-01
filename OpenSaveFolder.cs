using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class OpenSaveFolder : MonoBehaviour
{
	// Token: 0x06001859 RID: 6233 RVA: 0x000A59EF File Offset: 0x000A3BEF
	public void OpenFolder()
	{
		Application.OpenURL("file://" + DirectoryScript.modsFilePath);
	}

	// Token: 0x04000E94 RID: 3732
	public UIToggle ModLocDocumentsToggle;
}
