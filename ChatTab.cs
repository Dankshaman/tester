using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class ChatTab : MonoBehaviour
{
	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06002086 RID: 8326 RVA: 0x000EAD10 File Offset: 0x000E8F10
	// (remove) Token: 0x06002087 RID: 8327 RVA: 0x000EAD48 File Offset: 0x000E8F48
	public event ChatTab.ChatTabClicked TabClicked;

	// Token: 0x06002088 RID: 8328 RVA: 0x000EAD80 File Offset: 0x000E8F80
	private void Start()
	{
		if (this.chatType == NetworkSingleton<Chat>.Instance.selectedTab.chatType)
		{
			this.OnTabClicked(null);
		}
		EventManager.OnChatMessage += this.ChatSent;
		UIEventListener uieventListener = UIEventListener.Get(base.gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.gearSprite);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnSettingsClicked));
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x000EAE14 File Offset: 0x000E9014
	private void OnDestroy()
	{
		EventManager.OnChatMessage -= this.ChatSent;
		UIEventListener uieventListener = UIEventListener.Get(base.gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.gearSprite);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnSettingsClicked));
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000EAE8A File Offset: 0x000E908A
	public void Deselect()
	{
		if (this.selected)
		{
			this.highlightSprite.alpha = 0f;
		}
		this.selected = false;
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x000EAEAC File Offset: 0x000E90AC
	public void OnTabClicked(GameObject go)
	{
		this.selected = true;
		this.highlightSprite.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ChatTabBackground];
		this.highlightSprite.ThemeAs = UIPalette.UI.ChatTabBackground;
		if (this.TabClicked != null)
		{
			this.TabClicked(this);
		}
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x000EAF04 File Offset: 0x000E9104
	private void ChatSent(ChatMessageType chatMessageType)
	{
		if (!this.selected && this.chatType == chatMessageType)
		{
			this.highlightSprite.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ChatTabHighlight];
			this.highlightSprite.ThemeAs = UIPalette.UI.ChatTabHighlight;
		}
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x000EAF50 File Offset: 0x000E9150
	public void AddText(string text)
	{
		if (this.textList.Count > 400)
		{
			this.textList.RemoveAt(0);
		}
		this.textList.Add(text);
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x000EAF7C File Offset: 0x000E917C
	public void ClearText()
	{
		this.textList.Clear();
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000EAF8C File Offset: 0x000E918C
	public void OnSettingsClicked(GameObject go)
	{
		if (this.chatSettings != null)
		{
			bool active = !this.chatSettings.background.activeSelf;
			this.chatSettings.background.SetActive(active);
		}
	}

	// Token: 0x040013FF RID: 5119
	private const float selectedAlpha = 0.5f;

	// Token: 0x04001400 RID: 5120
	public ChatMessageType chatType;

	// Token: 0x04001401 RID: 5121
	public UISprite highlightSprite;

	// Token: 0x04001403 RID: 5123
	[NonSerialized]
	public List<string> textList = new List<string>();

	// Token: 0x04001404 RID: 5124
	public ChatSettings chatSettings;

	// Token: 0x04001405 RID: 5125
	public GameObject gearSprite;

	// Token: 0x04001406 RID: 5126
	public bool selected;

	// Token: 0x02000707 RID: 1799
	// (Invoke) Token: 0x06003D6B RID: 15723
	public delegate void ChatTabClicked(ChatTab tab);
}
