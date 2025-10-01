using System;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class UIChestActivate : MonoBehaviour
{
	// Token: 0x06002172 RID: 8562 RVA: 0x000F1223 File Offset: 0x000EF423
	private void Awake()
	{
		this.chestMenu = GameObject.Find("15 Chest");
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000F1238 File Offset: 0x000EF438
	private void OnClick()
	{
		if (this.target != null)
		{
			Resources.UnloadUnusedAssets();
			int childCount = this.chestMenu.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject = this.chestMenu.transform.GetChild(i).gameObject;
				if (gameObject.name != "01 Chest Main")
				{
					NGUITools.SetActive(gameObject, false);
				}
			}
			NGUITools.SetActive(this.target, true);
			NGUITools.SetActiveChildren(this.target, true);
			NGUITools.SetActive(this.target.transform.parent.gameObject, true);
			if (this.target.GetComponent<UIRect>())
			{
				this.target.GetComponent<UIRect>().UpdateAnchors();
			}
			if (this.target.GetComponent<UIWidget>())
			{
				this.target.GetComponent<UIWidget>().UpdateAnchors();
			}
		}
	}

	// Token: 0x040014C1 RID: 5313
	public GameObject chestMenu;

	// Token: 0x040014C2 RID: 5314
	public GameObject target;
}
