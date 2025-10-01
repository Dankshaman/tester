using System;
using UnityEngine;

// Token: 0x0200038C RID: 908
public class WebsiteJsonGenerator : MonoBehaviour
{
	// Token: 0x06002A9E RID: 10910 RVA: 0x0012F5E7 File Offset: 0x0012D7E7
	public void CopyJsonToClipboard()
	{
		GUIUtility.systemCopyBuffer = Json.GetJson(this.websiteInfo, true);
		Debug.Log("Website Json copied to clipboard");
	}

	// Token: 0x04001D0C RID: 7436
	public WebsiteWWW.WebsiteInfo websiteInfo;
}
