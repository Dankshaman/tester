using System;
using UnityEngine;

// Token: 0x020002A4 RID: 676
public class UIConnectToServer : MonoBehaviour
{
	// Token: 0x060021FE RID: 8702 RVA: 0x000F4DBC File Offset: 0x000F2FBC
	private void OnClick()
	{
		Transform transform = NetworkSingleton<NetworkUI>.Instance.GUIBrowserGrid.transform;
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (transform.GetChild(i).GetChild(0).gameObject.GetComponent<UIServerButton>().bSelected)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.GetComponent<UIServerBrowser>().Connect(-1);
				return;
			}
		}
	}
}
