using System;
using UnityEngine;

// Token: 0x0200034F RID: 847
public class UIThemeEditorSplashButton : MonoBehaviour
{
	// Token: 0x0600282B RID: 10283 RVA: 0x0011BDB4 File Offset: 0x00119FB4
	private void Start()
	{
		UISprite component = base.GetComponent<UISprite>();
		if (NetworkSingleton<NetworkUI>.Instance.VersionHotfix == "")
		{
			component.SetAnchor(this.Version);
			return;
		}
		component.SetAnchor(this.Hotfix);
	}

	// Token: 0x04001A68 RID: 6760
	public GameObject Hotfix;

	// Token: 0x04001A69 RID: 6761
	public GameObject Version;
}
