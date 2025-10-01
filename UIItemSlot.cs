using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000D RID: 13
public abstract class UIItemSlot : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600004B RID: 75
	protected abstract InvGameItem observedItem { get; }

	// Token: 0x0600004C RID: 76
	protected abstract InvGameItem Replace(InvGameItem item);

	// Token: 0x0600004D RID: 77 RVA: 0x000035E0 File Offset: 0x000017E0
	private void OnTooltip(bool show)
	{
		InvGameItem invGameItem = show ? this.mItem : null;
		if (invGameItem != null)
		{
			InvBaseItem baseItem = invGameItem.baseItem;
			if (baseItem != null)
			{
				string text = string.Concat(new string[]
				{
					"[",
					NGUIText.EncodeColor(invGameItem.color),
					"]",
					invGameItem.name,
					"[-]\n"
				});
				text = string.Concat(new object[]
				{
					text,
					"[AFAFAF]Level ",
					invGameItem.itemLevel,
					" ",
					baseItem.slot
				});
				List<InvStat> list = invGameItem.CalculateStats();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					InvStat invStat = list[i];
					if (invStat.amount != 0)
					{
						if (invStat.amount < 0)
						{
							text = text + "\n[FF0000]" + invStat.amount;
						}
						else
						{
							text = text + "\n[00FF00]+" + invStat.amount;
						}
						if (invStat.modifier == InvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + invStat.id;
						text += "[-]";
					}
					i++;
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.Show(text);
				return;
			}
		}
		UITooltip.Hide();
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0000375E File Offset: 0x0000195E
	private void OnClick()
	{
		if (UIItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
			return;
		}
		if (this.mItem != null)
		{
			UIItemSlot.mDraggedItem = this.Replace(null);
			if (UIItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000379C File Offset: 0x0000199C
	private void OnDrag(Vector2 delta)
	{
		if (UIItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000037D8 File Offset: 0x000019D8
	private void OnDrop(GameObject go)
	{
		InvGameItem invGameItem = this.Replace(UIItemSlot.mDraggedItem);
		if (UIItemSlot.mDraggedItem == invGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (invGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIItemSlot.mDraggedItem = invGameItem;
		this.UpdateCursor();
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003830 File Offset: 0x00001A30
	private void UpdateCursor()
	{
		if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
		{
			UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
			return;
		}
		UICursor.Clear();
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00003870 File Offset: 0x00001A70
	private void Update()
	{
		InvGameItem observedItem = this.observedItem;
		if (this.mItem != observedItem)
		{
			this.mItem = observedItem;
			InvBaseItem invBaseItem = (observedItem != null) ? observedItem.baseItem : null;
			if (this.label != null)
			{
				string text = (observedItem != null) ? observedItem.name : null;
				if (string.IsNullOrEmpty(this.mText))
				{
					this.mText = this.label.text;
				}
				this.label.text = ((text != null) ? text : this.mText);
			}
			if (this.icon != null)
			{
				if (invBaseItem == null || invBaseItem.iconAtlas == null)
				{
					this.icon.enabled = false;
				}
				else
				{
					this.icon.atlas = invBaseItem.iconAtlas;
					this.icon.spriteName = invBaseItem.iconName;
					this.icon.enabled = true;
					this.icon.MakePixelPerfect();
				}
			}
			if (this.background != null)
			{
				this.background.color = ((observedItem != null) ? observedItem.color : Color.white);
			}
		}
	}

	// Token: 0x04000020 RID: 32
	public UISprite icon;

	// Token: 0x04000021 RID: 33
	public UIWidget background;

	// Token: 0x04000022 RID: 34
	public UILabel label;

	// Token: 0x04000023 RID: 35
	public AudioClip grabSound;

	// Token: 0x04000024 RID: 36
	public AudioClip placeSound;

	// Token: 0x04000025 RID: 37
	public AudioClip errorSound;

	// Token: 0x04000026 RID: 38
	private InvGameItem mItem;

	// Token: 0x04000027 RID: 39
	private string mText = "";

	// Token: 0x04000028 RID: 40
	private static InvGameItem mDraggedItem;
}
