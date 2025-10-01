using System;
using UnityEngine;

// Token: 0x020002DE RID: 734
public class UIGridMenuSpotlights : UIGridMenu
{
	// Token: 0x06002416 RID: 9238 RVA: 0x000FFAC2 File Offset: 0x000FDCC2
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnUIThemeChange += this.OnThemeChange;
		this.OnThemeChange();
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000FFAE1 File Offset: 0x000FDCE1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventManager.OnUIThemeChange -= this.OnThemeChange;
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000FFAFA File Offset: 0x000FDCFA
	private void OnThemeChange()
	{
		base.UpdateButtons();
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000FFB04 File Offset: 0x000FDD04
	private void Update()
	{
		if (this.prevPage != base.currentPage)
		{
			this.timeHolder = Time.time;
			this.prevPage = base.currentPage;
			return;
		}
		if (Time.time < this.timeHolder + 10f)
		{
			return;
		}
		this.CyclePages();
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000FFB51 File Offset: 0x000FDD51
	public void CyclePages()
	{
		if (this.numberPages <= 1)
		{
			return;
		}
		if (this.numberPages > base.currentPage)
		{
			base.PageRight();
			return;
		}
		base.SetPage(1);
	}

	// Token: 0x04001722 RID: 5922
	private const float pageTurnSpeed = 10f;

	// Token: 0x04001723 RID: 5923
	private int prevPage = -1;

	// Token: 0x04001724 RID: 5924
	private float timeHolder;
}
