using System;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class UIHighlightTargets : MonoBehaviour
{
	// Token: 0x06002442 RID: 9282 RVA: 0x001004E0 File Offset: 0x000FE6E0
	public void Add(GameObject GO)
	{
		if (!GO)
		{
			return;
		}
		Highlighter component = GO.GetComponent<Highlighter>();
		if (component && !this.HighlightingObject.Contains(component))
		{
			this.HighlightingObject.Add(component);
		}
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x00100520 File Offset: 0x000FE720
	public void Remove(GameObject GO)
	{
		if (!GO)
		{
			return;
		}
		Highlighter component = GO.GetComponent<Highlighter>();
		if (component && this.HighlightingObject.Contains(component))
		{
			this.HighlightingObject.Remove(component);
		}
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x00100560 File Offset: 0x000FE760
	public void Reset()
	{
		this.HighlightingObject.Clear();
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x00100570 File Offset: 0x000FE770
	private void Update()
	{
		for (int i = 0; i < this.HighlightingObject.Count; i++)
		{
			if (this.HighlightingObject[i])
			{
				this.HighlightingObject[i].On(this.HighlightColor);
			}
		}
	}

	// Token: 0x04001747 RID: 5959
	private List<Highlighter> HighlightingObject = new List<Highlighter>();

	// Token: 0x04001748 RID: 5960
	public Color HighlightColor = Colour.Green;
}
