using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BD RID: 445
[Serializable]
public class NGUIManagedGrid : MonoBehaviour
{
	// Token: 0x0600163B RID: 5691 RVA: 0x0009AA0A File Offset: 0x00098C0A
	public void Init()
	{
		if (this.children == null)
		{
			this.children = new List<UISprite>();
			EventDelegate.Add(this.scrollbar.onChange, new EventDelegate.Callback(this.OnScrolling));
		}
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x0009AA3C File Offset: 0x00098C3C
	private void OnDestroy()
	{
		EventDelegate.Remove(this.scrollbar.onChange, new EventDelegate.Callback(this.OnScrolling));
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x000025B8 File Offset: 0x000007B8
	private void OnScrolling()
	{
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x0009AA5C File Offset: 0x00098C5C
	private void Update()
	{
		if (this.children == null || this.children.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.panel.IsVisible(this.children[i]))
			{
				this.children[i].transform.GetChild(0).gameObject.SetActive(true);
				if (this.disableChildren)
				{
					this.children[i].enabled = false;
				}
			}
			else
			{
				this.children[i].transform.GetChild(0).gameObject.SetActive(false);
				if (this.disableChildren)
				{
					this.children[i].enabled = true;
				}
			}
		}
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x0009AB30 File Offset: 0x00098D30
	public void UpdateVisible()
	{
		if (!base.enabled)
		{
			return;
		}
		this.panel.Update();
		if (this.children == null)
		{
			return;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.panel.IsVisible(this.children[i]))
			{
				this.children[i].transform.GetChild(0).gameObject.SetActive(true);
			}
			else
			{
				this.children[i].transform.GetChild(0).gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x0009ABCF File Offset: 0x00098DCF
	public void AddChild(GameObject go)
	{
		this.children.Add(go.GetComponent<UISprite>());
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x0009ABE4 File Offset: 0x00098DE4
	public void RemoveChild(GameObject go)
	{
		UISprite component = go.GetComponent<UISprite>();
		this.children.Remove(component);
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0009AC05 File Offset: 0x00098E05
	public void ClearList()
	{
		if (this.children != null)
		{
			this.children.Clear();
		}
	}

	// Token: 0x04000CA0 RID: 3232
	public UIScrollBar scrollbar;

	// Token: 0x04000CA1 RID: 3233
	public UIScrollView scrollView;

	// Token: 0x04000CA2 RID: 3234
	public UIPanel panel;

	// Token: 0x04000CA3 RID: 3235
	private List<UISprite> children;

	// Token: 0x04000CA4 RID: 3236
	public bool disableChildren = true;
}
