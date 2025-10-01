using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class ChatSettings : Singleton<ChatSettings>
{
	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06002073 RID: 8307 RVA: 0x000EA57A File Offset: 0x000E877A
	// (set) Token: 0x06002074 RID: 8308 RVA: 0x000EA582 File Offset: 0x000E8782
	public int AllowMessages
	{
		get
		{
			return this.allowMessages;
		}
		set
		{
			this.allowMessages = value;
			EventManager.TriggerAllowMessages(this.allowMessages > 0, this.tabType);
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06002075 RID: 8309 RVA: 0x000EA5A3 File Offset: 0x000E87A3
	// (set) Token: 0x06002076 RID: 8310 RVA: 0x000EA5AB File Offset: 0x000E87AB
	public int AutoHideChat
	{
		get
		{
			return this.autoHideChat;
		}
		set
		{
			this.autoHideChat = value;
			EventManager.TriggerAutoHideChat(this.autoHideChat > 0);
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06002077 RID: 8311 RVA: 0x000EA5C6 File Offset: 0x000E87C6
	// (set) Token: 0x06002078 RID: 8312 RVA: 0x000EA5CE File Offset: 0x000E87CE
	public bool FilterChatMessages
	{
		get
		{
			return this.filterChatMessages;
		}
		set
		{
			this.filterChatMessages = value;
			this.filterChatToggle.value = value;
		}
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000EA5E4 File Offset: 0x000E87E4
	private void Start()
	{
		EventDelegate.Add(this.CloseButton.onClick, new EventDelegate.Callback(this.OnCloseSettings));
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.OnReset));
		EventDelegate.Add(this.fontSizeSlider.onChange, new EventDelegate.Callback(this.FontSizeSliderChanged));
		base.StartCoroutine(this.Init());
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000EA658 File Offset: 0x000E8858
	private void OnDestroy()
	{
		EventDelegate.Remove(this.CloseButton.onClick, new EventDelegate.Callback(this.OnCloseSettings));
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.OnReset));
		this.opacitySlider != null;
		if (this.fontSizeSlider != null)
		{
			EventDelegate.Remove(this.fontSizeSlider.onChange, new EventDelegate.Callback(this.FontSizeSliderChanged));
		}
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x000EA6D7 File Offset: 0x000E88D7
	public IEnumerator Init()
	{
		this.AllowMessages = PlayerPrefs.GetInt(this.tabType.ToString() + "allowMessages", 1);
		this.allowMessagesToggle.value = (this.AllowMessages > 0);
		this.AutoHideChat = PlayerPrefs.GetInt("autoHideChat", 1);
		this.autoHideChatToggle.value = (this.AutoHideChat > 0);
		this.FilterChatMessages = (PlayerPrefs.GetInt("filterChatMessages", 1) != 0);
		this.chatDisplayTimestamp[ChatMessageType.System] = false;
		this.chatDisplayTimestamp[ChatMessageType.Global] = false;
		this.chatDisplayTimestamp[ChatMessageType.Game] = false;
		this.chatDisplayTimestamp[ChatMessageType.Team] = false;
		this.chatDisplayTimestamp[ChatMessageType.All] = (PlayerPrefs.GetInt("chatDisplayTimestampAll", 0) == 1);
		this.displayTimestampsToggle.value = this.chatDisplayTimestamp[ChatMessageType.All];
		this.chatFontSize = PlayerPrefs.GetInt("chatFontSize", 16);
		this.fontSizeSlider.value = Mathf.InverseLerp(10f, 30f, (float)this.chatFontSize);
		this.chatLabel.fontSize = this.chatFontSize;
		yield return null;
		if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System)
		{
			this.SetConsoleColors(1f);
		}
		else
		{
			this.SetChatColors(1f);
		}
		int @int;
		int int2;
		if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System)
		{
			@int = PlayerPrefs.GetInt("ChatConsoleWidth", 498);
			int2 = PlayerPrefs.GetInt("ChatConsoleHeight", 522);
		}
		else
		{
			@int = PlayerPrefs.GetInt("ChatWidth", 428);
			int2 = PlayerPrefs.GetInt("ChatHeight", 261);
		}
		this.chatWindow.rightAnchor.absolute = @int;
		this.chatWindow.topAnchor.absolute = int2;
		yield break;
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000EA6E6 File Offset: 0x000E88E6
	public void SetConsoleColors(float progress = 1f)
	{
		this.SetChatColors(1f - progress);
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x000EA6F8 File Offset: 0x000E88F8
	public void SetChatColors(float progress = 1f)
	{
		Dictionary<UIPalette.UI, Colour> currentThemeColours = Singleton<UIPalette>.Instance.CurrentThemeColours;
		Color color = Colour.Lerp(currentThemeColours[UIPalette.UI.ConsoleOutputControls], currentThemeColours[UIPalette.UI.ChatOutputControls], progress);
		Color color2 = Colour.Lerp(currentThemeColours[UIPalette.UI.ConsoleOutputBackground], currentThemeColours[UIPalette.UI.ChatOutputBackground], progress);
		Color color3 = Colour.Lerp(currentThemeColours[UIPalette.UI.ConsoleInputControls], currentThemeColours[UIPalette.UI.ChatInputControls], progress);
		Color activeTextColor = Colour.Lerp(currentThemeColours[UIPalette.UI.ConsoleInputText], currentThemeColours[UIPalette.UI.ChatInputText], progress);
		Color color4 = Colour.Lerp(currentThemeColours[UIPalette.UI.ConsoleInputBackground], currentThemeColours[UIPalette.UI.ChatInputBackground], progress);
		UISprite[] componentsInChildren = this.chatWindow.GetComponentsInChildren<UISprite>(true);
		UISprite[] componentsInChildren2 = this.chatInput.GetComponentsInChildren<UISprite>(true);
		foreach (UISprite uisprite in componentsInChildren)
		{
			if (!(uisprite.gameObject.name == "Highlight"))
			{
				Color color5;
				UIPalette.UI ui;
				if (uisprite.gameObject.name == "Vignette")
				{
					color5 = color2;
					ui = ((progress >= 0.5f) ? UIPalette.UI.ChatOutputBackground : UIPalette.UI.ConsoleOutputBackground);
				}
				else
				{
					color5 = color;
					ui = ((progress >= 0.5f) ? UIPalette.UI.ChatOutputControls : UIPalette.UI.ConsoleOutputControls);
				}
				uisprite.color = color5;
				uisprite.ThemeAs = ui;
				UIButton component = uisprite.GetComponent<UIButton>();
				if (component)
				{
					component.defaultColor = color5;
					component.ThemeNormalAs = ui;
				}
			}
		}
		foreach (UISprite uisprite2 in componentsInChildren2)
		{
			if (!(uisprite2.gameObject.name == "Highlight"))
			{
				Color color6;
				UIPalette.UI ui2;
				if (uisprite2.gameObject.name == "Vignette")
				{
					color6 = color4;
					ui2 = ((progress >= 0.5f) ? UIPalette.UI.ChatInputBackground : UIPalette.UI.ConsoleInputBackground);
				}
				else
				{
					color6 = color3;
					ui2 = ((progress >= 0.5f) ? UIPalette.UI.ChatInputControls : UIPalette.UI.ConsoleInputControls);
				}
				uisprite2.color = color6;
				uisprite2.ThemeAs = ui2;
				UIButton component2 = uisprite2.GetComponent<UIButton>();
				if (component2)
				{
					component2.defaultColor = color6;
					component2.ThemeNormalAs = ui2;
				}
				UIInput component3 = uisprite2.GetComponent<UIInput>();
				if (component3)
				{
					component3.activeTextColor = activeTextColor;
					component3.ThemeActiveAs = ((progress >= 0.5f) ? UIPalette.UI.ChatInputText : UIPalette.UI.ConsoleInputText);
				}
			}
		}
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x000EA953 File Offset: 0x000E8B53
	public void SetOpacity(float opacity)
	{
		this.chatOpacity = opacity;
		this.opacitySlider.value = this.chatOpacity;
		this.chatSprite.alpha = this.chatOpacity;
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x000EA980 File Offset: 0x000E8B80
	public void SaveSettings()
	{
		PlayerPrefs.SetInt(this.tabType.ToString() + "allowMessages", this.allowMessages);
		PlayerPrefs.SetInt("autoHideChat", this.autoHideChat);
		PlayerPrefs.SetInt("filterChatMessages", this.FilterChatMessages ? 1 : 0);
		PlayerPrefs.SetInt("chatFontSize", this.chatFontSize);
		PlayerPrefs.SetInt("chatDisplayTimestampAll", this.chatDisplayTimestamp[ChatMessageType.All] ? 1 : 0);
		if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System)
		{
			PlayerPrefs.SetFloat("chatConsoleOpacity2", this.chatOpacity);
			Colour.SetPlayerPrefColour("chatConsoleColor", this.colorInput.GetColor());
			return;
		}
		PlayerPrefs.SetFloat("chatOpacity2", this.chatOpacity);
		Colour.SetPlayerPrefColour("chatColor", this.colorInput.GetColor());
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x000EAA6C File Offset: 0x000E8C6C
	private void OnCloseSettings()
	{
		if (this.allowMessagesToggle.value)
		{
			this.AllowMessages = 1;
		}
		else
		{
			this.AllowMessages = 0;
		}
		if (this.autoHideChatToggle.value)
		{
			this.AutoHideChat = 1;
		}
		else
		{
			this.AutoHideChat = 0;
		}
		this.FilterChatMessages = this.filterChatToggle.value;
		this.chatDisplayTimestamp[ChatMessageType.All] = this.displayTimestampsToggle.value;
		this.chatSprite.alpha = this.chatOpacity;
		this.chatLabel.fontSize = this.chatFontSize;
		this.SaveSettings();
		Singleton<ChatIRC>.Instance.allowMessages = (this.AllowMessages == 1);
		if (this.AllowMessages == 1)
		{
			Singleton<ChatIRC>.Instance.Init();
		}
		else
		{
			Singleton<ChatIRC>.Instance.Disconnect();
		}
		this.background.SetActive(false);
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000EAB44 File Offset: 0x000E8D44
	private void OnReset()
	{
		PlayerPrefs.DeleteKey(this.tabType.ToString() + "allowMessages");
		PlayerPrefs.DeleteKey("autoHideChat");
		PlayerPrefs.DeleteKey("filterChatMessages");
		PlayerPrefs.DeleteKey("chatDisplayTimestampAll");
		PlayerPrefs.DeleteKey("chatFontSize");
		PlayerPrefs.DeleteKey("chatOpacity2");
		PlayerPrefs.DeleteKey("chatColor");
		PlayerPrefs.DeleteKey("chatConsoleOpacity2");
		PlayerPrefs.DeleteKey("chatConsoleColor");
		PlayerPrefs.DeleteKey("ChatWidth");
		PlayerPrefs.DeleteKey("ChatHeight");
		PlayerPrefs.DeleteKey("ChatConsoleWidth");
		PlayerPrefs.DeleteKey("ChatConsoleHeight");
		base.StartCoroutine(this.Init());
	}

	// Token: 0x06002082 RID: 8322 RVA: 0x000EABF6 File Offset: 0x000E8DF6
	private void OpacitySliderChanged()
	{
		this.chatOpacity = this.opacitySlider.value;
		this.chatSprite.alpha = this.chatOpacity;
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x000EAC1A File Offset: 0x000E8E1A
	private void FontSizeSliderChanged()
	{
		this.chatFontSize = (int)Mathf.Lerp(10f, 30f, this.fontSizeSlider.value);
		this.chatLabel.fontSize = this.chatFontSize;
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000EAC50 File Offset: 0x000E8E50
	public void OnChatResized()
	{
		if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System)
		{
			PlayerPrefs.SetInt("ChatConsoleWidth", this.chatWindow.rightAnchor.absolute);
			PlayerPrefs.SetInt("ChatConsoleHeight", this.chatWindow.topAnchor.absolute);
			return;
		}
		PlayerPrefs.SetInt("ChatWidth", this.chatWindow.rightAnchor.absolute);
		PlayerPrefs.SetInt("ChatHeight", this.chatWindow.topAnchor.absolute);
	}

	// Token: 0x040013D9 RID: 5081
	public const int DEFAULT_ALLOW_MESSAGES = 1;

	// Token: 0x040013DA RID: 5082
	public const int DEFAULT_AUTO_HIDE_CHAT = 1;

	// Token: 0x040013DB RID: 5083
	public const int DEFAULT_FONT_SIZE = 16;

	// Token: 0x040013DC RID: 5084
	public const int DEFAULT_WIDTH = 428;

	// Token: 0x040013DD RID: 5085
	public const int DEFAULT_HEIGHT = 261;

	// Token: 0x040013DE RID: 5086
	public const int DEFAULT_CONSOLE_WIDTH = 498;

	// Token: 0x040013DF RID: 5087
	public const int DEFAULT_CONSOLE_HEIGHT = 522;

	// Token: 0x040013E0 RID: 5088
	public ChatMessageType tabType;

	// Token: 0x040013E1 RID: 5089
	public int fontSize = 1;

	// Token: 0x040013E2 RID: 5090
	private int allowMessages = 1;

	// Token: 0x040013E3 RID: 5091
	private int autoHideChat = 1;

	// Token: 0x040013E4 RID: 5092
	private bool filterChatMessages = true;

	// Token: 0x040013E5 RID: 5093
	public UIButton CloseButton;

	// Token: 0x040013E6 RID: 5094
	public UIButton ResetButton;

	// Token: 0x040013E7 RID: 5095
	public UIToggle allowMessagesToggle;

	// Token: 0x040013E8 RID: 5096
	public UIToggle autoHideChatToggle;

	// Token: 0x040013E9 RID: 5097
	public UIToggle filterChatToggle;

	// Token: 0x040013EA RID: 5098
	public UIToggle displayTimestampsToggle;

	// Token: 0x040013EB RID: 5099
	public GameObject background;

	// Token: 0x040013EC RID: 5100
	public UILabel chatLabel;

	// Token: 0x040013ED RID: 5101
	public UISprite chatSprite;

	// Token: 0x040013EE RID: 5102
	public UISlider fontSizeSlider;

	// Token: 0x040013EF RID: 5103
	public UISlider opacitySlider;

	// Token: 0x040013F0 RID: 5104
	public float chatOpacity;

	// Token: 0x040013F1 RID: 5105
	private const int minFontSize = 10;

	// Token: 0x040013F2 RID: 5106
	private const int maxFontSize = 30;

	// Token: 0x040013F3 RID: 5107
	private int chatFontSize = 16;

	// Token: 0x040013F4 RID: 5108
	public UIWidget chatWindow;

	// Token: 0x040013F5 RID: 5109
	public UIWidget chatInput;

	// Token: 0x040013F6 RID: 5110
	public UIColorPickerInput colorInput;

	// Token: 0x040013F7 RID: 5111
	public Dictionary<ChatMessageType, bool> chatDisplayTimestamp = new Dictionary<ChatMessageType, bool>();

	// Token: 0x02000705 RID: 1797
	// (Invoke) Token: 0x06003D61 RID: 15713
	public delegate void FontSizeChanged(int fontsize);
}
