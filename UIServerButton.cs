using System;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class UIServerButton : MonoBehaviour
{
	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002706 RID: 9990 RVA: 0x00115877 File Offset: 0x00113A77
	public string Players
	{
		get
		{
			return this.CurrentPlayers + "/" + this.MaxPlayers;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002707 RID: 9991 RVA: 0x00115899 File Offset: 0x00113A99
	// (set) Token: 0x06002708 RID: 9992 RVA: 0x001158A4 File Offset: 0x00113AA4
	public bool bSelected
	{
		get
		{
			return this.bselected;
		}
		set
		{
			if (value != this.bselected)
			{
				this.bselected = value;
				if (this.bselected)
				{
					UIButton[] componentsInChildren = base.GetComponentsInChildren<UIButton>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].duration = 0f;
						componentsInChildren[i].defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHighlightB];
						componentsInChildren[i].hover = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHighlightB];
					}
					return;
				}
				UIButton[] componentsInChildren2 = base.GetComponentsInChildren<UIButton>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].duration = 0f;
					componentsInChildren2[j].defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
					componentsInChildren2[j].hover = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHover];
				}
			}
		}
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x00115988 File Offset: 0x00113B88
	private void UpdateServerLabel(UILabel label, string text, bool applyChatFilter)
	{
		if (applyChatFilter)
		{
			bool flag;
			text = Singleton<ChatIRC>.Instance.FilterChatMessage(text, out flag);
		}
		label.text = text;
		UITooltipScript component = label.GetComponent<UITooltipScript>();
		if (label == this.regionNameLabel)
		{
			if (component)
			{
				component.Tooltip = CountryCodesScript.ISOCodeToName(this.Region);
				component.ScrollHandler = new UITooltipScript.ScrollHandlerFunction(this.OnScroll);
			}
			label.GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			if (component)
			{
				component.Tooltip = text;
				component.ScrollHandler = new UITooltipScript.ScrollHandlerFunction(this.OnScroll);
			}
			label.GetComponent<BoxCollider2D>().enabled = NGUIHelper.ClampAndAddDots(label);
		}
		label.color = (this.bIsFriend ? Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerGreen] : Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.Label]);
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x00115A68 File Offset: 0x00113C68
	private void Awake()
	{
		this.serverNameLabel.GetComponent<UITooltipScript>().DeleteIfEmpty = false;
		this.gameNameLabel.GetComponent<UITooltipScript>().DeleteIfEmpty = false;
		this.hostNameLabel.GetComponent<UITooltipScript>().DeleteIfEmpty = false;
		this.playersNameLabel.GetComponent<UITooltipScript>().DeleteIfEmpty = false;
		this.regionNameLabel.GetComponent<UITooltipScript>().DeleteIfEmpty = false;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x00115ACC File Offset: 0x00113CCC
	private void Start()
	{
		this.browser = NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.GetComponent<UIServerBrowser>();
		base.GetComponent<BoxCollider2D>().enabled = true;
		this.bselected = false;
		UIEventListener uieventListener = UIEventListener.Get(this.gameNameLabel.gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.hostNameLabel.gameObject);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
		UIEventListener uieventListener3 = UIEventListener.Get(this.playersNameLabel.gameObject);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
		UIEventListener uieventListener4 = UIEventListener.Get(this.serverNameLabel.gameObject);
		uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
		UIEventListener uieventListener5 = UIEventListener.Get(this.regionNameLabel.gameObject);
		uieventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener5.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
		UIEventListener uieventListener6 = UIEventListener.Get(this.gameNameLabel.gameObject);
		uieventListener6.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener6.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		UIEventListener uieventListener7 = UIEventListener.Get(this.hostNameLabel.gameObject);
		uieventListener7.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener7.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		UIEventListener uieventListener8 = UIEventListener.Get(this.playersNameLabel.gameObject);
		uieventListener8.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener8.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		UIEventListener uieventListener9 = UIEventListener.Get(this.serverNameLabel.gameObject);
		uieventListener9.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener9.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		UIEventListener uieventListener10 = UIEventListener.Get(this.regionNameLabel.gameObject);
		uieventListener10.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener10.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		this.view = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		this.UpdateButton();
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x00115D04 File Offset: 0x00113F04
	public void UpdateButton()
	{
		this.lockIcon.gameObject.SetActive(this.bPassworded);
		if (this.bLookingForPlayers)
		{
			this.lookingForPlayersIcon.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.Motif];
			this.lookingForPlayersIcon.ThemeAs = UIPalette.UI.Motif;
			this.lookingForPlayersIcon.DoNotInitTheme();
			this.lookingForPlayersIcon.GetComponent<UITooltipScript>().Tooltip = "Looking for more players.";
		}
		else
		{
			this.lookingForPlayersIcon.color = new Colour(127, 127, 127, 1);
			this.lookingForPlayersIcon.ThemeAs = UIPalette.UI.DoNotTheme;
			this.lookingForPlayersIcon.DoNotInitTheme();
			this.lookingForPlayersIcon.GetComponent<UITooltipScript>().Tooltip = "Not looking for more players.";
		}
		this.lookingForPlayersIcon.GetComponent<BoxCollider2D>().enabled = (this.lookingForPlayersIcon.GetComponent<UITooltipScript>().Tooltip != "");
		if (!UIServerButton.BlockButton)
		{
			UIServerButton.BlockButton = NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.transform.Find("Block Player").GetChild(0).gameObject;
		}
		this.UpdateLabels(Singleton<ChatSettings>.Instance.FilterChatMessages);
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x00115E38 File Offset: 0x00114038
	private void UpdateLabels(bool filterLabels)
	{
		this.UpdateServerLabel(this.serverNameLabel, this.ServerName, filterLabels);
		this.UpdateServerLabel(this.gameNameLabel, this.Game, filterLabels);
		this.UpdateServerLabel(this.hostNameLabel, this.Host, filterLabels);
		this.UpdateServerLabel(this.playersNameLabel, this.Players, false);
		this.UpdateServerLabel(this.regionNameLabel, this.Region, false);
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x00115EA4 File Offset: 0x001140A4
	private void OnScroll(float delta)
	{
		if (zInput.GetButton("Alt", ControlType.All))
		{
			return;
		}
		if (this.view && NGUITools.GetActive(this))
		{
			this.view.transform.parent.parent.GetComponent<UIServerBrowser>().DoScroll(delta * UIServerBrowser.SCROLL_WHEEL_FACTOR);
		}
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x00115EFC File Offset: 0x001140FC
	private void OnDestroy()
	{
		if (this.gameNameLabel != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.gameNameLabel.gameObject);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
			UIEventListener uieventListener2 = UIEventListener.Get(this.gameNameLabel.gameObject);
			uieventListener2.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		}
		if (this.hostNameLabel != null)
		{
			UIEventListener uieventListener3 = UIEventListener.Get(this.hostNameLabel.gameObject);
			uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
			UIEventListener uieventListener4 = UIEventListener.Get(this.hostNameLabel.gameObject);
			uieventListener4.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener4.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		}
		if (this.playersNameLabel != null)
		{
			UIEventListener uieventListener5 = UIEventListener.Get(this.playersNameLabel.gameObject);
			uieventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener5.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
			UIEventListener uieventListener6 = UIEventListener.Get(this.playersNameLabel.gameObject);
			uieventListener6.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener6.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		}
		if (this.serverNameLabel != null)
		{
			UIEventListener uieventListener7 = UIEventListener.Get(this.serverNameLabel.gameObject);
			uieventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener7.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
			UIEventListener uieventListener8 = UIEventListener.Get(this.serverNameLabel.gameObject);
			uieventListener8.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener8.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		}
		if (this.regionNameLabel != null)
		{
			UIEventListener uieventListener9 = UIEventListener.Get(this.regionNameLabel.gameObject);
			uieventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener9.onClick, new UIEventListener.VoidDelegate(this.OnLabelClicked));
			UIEventListener uieventListener10 = UIEventListener.Get(this.regionNameLabel.gameObject);
			uieventListener10.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener10.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClickLabel));
		}
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x00116139 File Offset: 0x00114339
	private void OnDoubleClick()
	{
		this.Connect();
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x00116141 File Offset: 0x00114341
	private void OnDoubleClickLabel(GameObject go)
	{
		this.OnDoubleClick();
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x00116149 File Offset: 0x00114349
	public void Connect()
	{
		this.browser.Connect(this.LobbyIndex);
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x0011615C File Offset: 0x0011435C
	private void OnClick()
	{
		this.browser.Select(this.LobbyIndex);
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x0011616F File Offset: 0x0011436F
	private void OnLabelClicked(GameObject go)
	{
		this.OnClick();
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x00116178 File Offset: 0x00114378
	private void Update()
	{
		if ((UICamera.HoveredUIObject == base.gameObject || UICamera.HoveredUIObject == this.serverNameLabel.gameObject || UICamera.HoveredUIObject == this.gameNameLabel.gameObject || UICamera.HoveredUIObject == this.hostNameLabel.gameObject || UICamera.HoveredUIObject == this.playersNameLabel.gameObject || UICamera.HoveredUIObject == this.regionNameLabel.gameObject || UICamera.HoveredUIObject == this.lockIcon.gameObject) && Input.GetMouseButtonDown(1))
		{
			UIServerButton.BlockButton.SetActive(true);
			UIServerButton.BlockButton.transform.position = UICamera.lastWorldPosition;
			NGUITools.GetChildLabel(UIServerButton.BlockButton).text = "Block " + this.Host + "?";
			UIServerButton.BlockButton.GetComponent<UIServerBlockButton>().PlayerName = this.Host;
			UIServerButton.BlockButton.GetComponent<UIServerBlockButton>().PlayerSteamID = this.SteamID;
		}
	}

	// Token: 0x04001982 RID: 6530
	public bool bPassworded;

	// Token: 0x04001983 RID: 6531
	public bool bLookingForPlayers;

	// Token: 0x04001984 RID: 6532
	public string ServerName;

	// Token: 0x04001985 RID: 6533
	public string Game;

	// Token: 0x04001986 RID: 6534
	public string Host;

	// Token: 0x04001987 RID: 6535
	public int CurrentPlayers;

	// Token: 0x04001988 RID: 6536
	public int MaxPlayers;

	// Token: 0x04001989 RID: 6537
	public string Region;

	// Token: 0x0400198A RID: 6538
	public bool bFull;

	// Token: 0x0400198B RID: 6539
	public bool bIsFriend;

	// Token: 0x0400198C RID: 6540
	public string SteamID;

	// Token: 0x0400198D RID: 6541
	public string LobbyID;

	// Token: 0x0400198E RID: 6542
	public int LobbyIndex;

	// Token: 0x0400198F RID: 6543
	private bool bselected;

	// Token: 0x04001990 RID: 6544
	public UIScrollView view;

	// Token: 0x04001991 RID: 6545
	private static GameObject BlockButton;

	// Token: 0x04001992 RID: 6546
	public UILabel serverNameLabel;

	// Token: 0x04001993 RID: 6547
	public UILabel gameNameLabel;

	// Token: 0x04001994 RID: 6548
	public UILabel hostNameLabel;

	// Token: 0x04001995 RID: 6549
	public UILabel playersNameLabel;

	// Token: 0x04001996 RID: 6550
	public UILabel regionNameLabel;

	// Token: 0x04001997 RID: 6551
	public UISprite lockIcon;

	// Token: 0x04001998 RID: 6552
	public UISprite lookingForPlayersIcon;

	// Token: 0x04001999 RID: 6553
	private UIServerBrowser browser;
}
