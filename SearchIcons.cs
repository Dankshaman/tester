using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000277 RID: 631
public class SearchIcons : MonoBehaviour
{
	// Token: 0x0600211D RID: 8477 RVA: 0x000EF888 File Offset: 0x000EDA88
	public void Init()
	{
		for (int i = 0; i < this.searchIcons.Length; i++)
		{
			this.searchIcons[i].gameObject.SetActive(false);
		}
		this.grid.repositionNow = true;
		this.grid.Reposition();
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x000EF8D2 File Offset: 0x000EDAD2
	public void RemoveIcon(int id)
	{
		this.usedColors[id].gameObject.SetActive(false);
		this.usedColors.Remove(id);
		this.grid.repositionNow = true;
		this.grid.Reposition();
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x000EF910 File Offset: 0x000EDB10
	public void AddIcon(Color playerColor, int id)
	{
		UISprite uisprite = this.searchIcons[this.usedColors.Count];
		uisprite.gameObject.SetActive(true);
		uisprite.color = playerColor;
		this.usedColors.Add(id, uisprite);
		this.grid.repositionNow = true;
		this.grid.Reposition();
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x000EF968 File Offset: 0x000EDB68
	private void Update()
	{
		if (!VRHMD.isVR)
		{
			base.transform.rotation = Camera.main.transform.rotation;
			return;
		}
		base.transform.eulerAngles = new Vector3(45f, Singleton<VRHMD>.Instance.transform.eulerAngles.y, 0f);
	}

	// Token: 0x04001471 RID: 5233
	public UISprite[] searchIcons;

	// Token: 0x04001472 RID: 5234
	public Dictionary<int, UISprite> usedColors = new Dictionary<int, UISprite>();

	// Token: 0x04001473 RID: 5235
	public UIGrid grid;
}
