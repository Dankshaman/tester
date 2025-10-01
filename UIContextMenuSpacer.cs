using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A5 RID: 677
public class UIContextMenuSpacer : MonoBehaviour
{
	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06002200 RID: 8704 RVA: 0x000F4E20 File Offset: 0x000F3020
	// (set) Token: 0x06002201 RID: 8705 RVA: 0x000F4E31 File Offset: 0x000F3031
	public bool Open
	{
		get
		{
			return this.open || UIContextMenuSpacer.ALWAYS_OPEN;
		}
		set
		{
			if (this.open == value)
			{
				return;
			}
			this.open = value;
			PlayerPrefs.SetInt("ContextMenuSection" + base.transform.name, this.open ? 1 : 0);
			this.UpdateChildren(true);
		}
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000F4E71 File Offset: 0x000F3071
	private void Awake()
	{
		UIContextMenuSpacer.ContextualSpacers.Add(this);
		this.Open = (PlayerPrefs.GetInt("ContextMenuSection" + base.transform.name, 1) != 0);
		this.UpdateCollapsingEnabled();
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000F4EA8 File Offset: 0x000F30A8
	public void UpdateCollapsingEnabled()
	{
		UIButton component = base.GetComponent<UIButton>();
		if (UIContextMenuSpacer.ALWAYS_OPEN)
		{
			component.hoverSprite = "CMLineTRANS";
			component.enabled = false;
			base.GetComponent<UITooltipScript>().enabled = false;
			return;
		}
		component.hoverSprite = "CMLineTRANSExpand";
		component.enabled = true;
		base.GetComponent<UITooltipScript>().enabled = true;
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x000F4F00 File Offset: 0x000F3100
	private void OnDestroy()
	{
		UIContextMenuSpacer.ContextualSpacers.Remove(this);
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000F4F10 File Offset: 0x000F3110
	public void UpdateChildren(bool doReposition = true)
	{
		UISprite component = base.GetComponent<UISprite>();
		UIButton component2 = base.GetComponent<UIButton>();
		if (this.Open)
		{
			component.height = 10;
			component.spriteName = "CMLineTRANS";
			component2.normalSprite = "CMLineTRANS";
		}
		else
		{
			component.height = UIContextMenuSpacer.COLLAPSED_SPACER_HEIGHT;
			component.spriteName = "CMLineTRANSExpand";
			component2.normalSprite = "CMLineTRANSExpand";
		}
		for (int i = 0; i < this.ChildItems.Length; i++)
		{
			this.ChildItems[i].SetActive(this.Open && PlayerScript.PointerScript.ActiveItems.Contains(this.ChildItems[i]));
		}
		if (doReposition)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.GetComponent<UIContextual>().Reposition();
		}
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000F4FD0 File Offset: 0x000F31D0
	public static void UpdateContextualSpacerChildren()
	{
		for (int i = 0; i < UIContextMenuSpacer.ContextualSpacers.Count; i++)
		{
			UIContextMenuSpacer.ContextualSpacers[i].UpdateChildren(false);
		}
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000F5004 File Offset: 0x000F3204
	public static void UpdateEnabledDisabledBehaviour()
	{
		for (int i = 0; i < UIContextMenuSpacer.ContextualSpacers.Count; i++)
		{
			UIContextMenuSpacer.ContextualSpacers[i].UpdateCollapsingEnabled();
		}
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000F5036 File Offset: 0x000F3236
	public void Toggle()
	{
		this.Open = (UIContextMenuSpacer.ALWAYS_OPEN || !this.Open);
	}

	// Token: 0x0400156E RID: 5486
	public static bool ALWAYS_OPEN = true;

	// Token: 0x0400156F RID: 5487
	public static int COLLAPSED_SPACER_HEIGHT = 10;

	// Token: 0x04001570 RID: 5488
	private const string normalLine = "CMLineTRANS";

	// Token: 0x04001571 RID: 5489
	private const string collapsedLine = "CMLineTRANSExpand";

	// Token: 0x04001572 RID: 5490
	public static List<UIContextMenuSpacer> ContextualSpacers = new List<UIContextMenuSpacer>();

	// Token: 0x04001573 RID: 5491
	private const string PREF_PREFIX = "ContextMenuSection";

	// Token: 0x04001574 RID: 5492
	public GameObject[] ChildItems;

	// Token: 0x04001575 RID: 5493
	private bool open = true;
}
