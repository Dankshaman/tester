using System;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class UIGridFast : MonoBehaviour
{
	// Token: 0x06002340 RID: 9024 RVA: 0x000FA420 File Offset: 0x000F8620
	private void Start()
	{
		if (this.Panel == null)
		{
			this.Panel = base.GetComponentInParent<UIPanel>();
		}
		if (this.Grid == null)
		{
			this.Grid = base.GetComponent<UIGrid>();
		}
		if (this.ScrollView == null)
		{
			this.ScrollView = base.GetComponentInParent<UIScrollView>();
		}
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000FA47C File Offset: 0x000F867C
	private void Update()
	{
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			Transform child = this.Grid.transform.GetChild(i);
			bool active = this.Panel.IsVisible(child.position);
			child.GetChild(0).gameObject.SetActive(active);
		}
	}

	// Token: 0x0400165B RID: 5723
	public UIPanel Panel;

	// Token: 0x0400165C RID: 5724
	public UIGrid Grid;

	// Token: 0x0400165D RID: 5725
	public UIScrollView ScrollView;
}
