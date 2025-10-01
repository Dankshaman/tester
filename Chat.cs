using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NewNet;
using UnityEngine;
using Valve.VR;

// Token: 0x020000B5 RID: 181
public class Chat : NetworkSingleton<Chat>
{
	// Token: 0x060008D6 RID: 2262 RVA: 0x0003FA04 File Offset: 0x0003DC04
	protected override void SingletonInit()
	{
		this.selectedTab = this.chatTabs[3];
		for (int i = 0; i < this.chatTabs.Length; i++)
		{
			if (this.chatTabs[i].chatType != ChatMessageType.Global)
			{
				this.chatTabs[i].gameObject.SetActive(false);
			}
			this.tabsDict.Add(this.chatTabs[i].chatType, this.chatTabs[i]);
		}
		this.tabsDict[ChatMessageType.Global].textList = new List<string>(Chat.GlobalChatHistory);
		this.tabsDict[ChatMessageType.System].textList = new List<string>(Chat.SystemChatHistory);
		this.ChatList.Clear();
		this.ChatList.Add(this.tabsDict[ChatMessageType.Global].textList);
		EventManager.OnChangePlayerTeam += this.PlayerChangedTeam;
		EventManager.OnAutoHideChat += this.OnAutoHideChatChanged;
		this.chatWindow = this.ChatWindow.GetComponent<UIWidget>();
		this.AddChat("System Console", "[E7E52C]", ChatMessageType.System, false);
		this.AddChat("", "[FFFFFF]", ChatMessageType.System, false);
		this.AddChat("To autocomplete commands hit [88FFFF]<tab>[-].", "[FFFFFF]", ChatMessageType.System, false);
		this.AddChat("Type [88FFFF]help[-] for help.", "[FFFFFF]", ChatMessageType.System, false);
		this.AddChat("", "[FFFFFF]", ChatMessageType.System, false);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003FB60 File Offset: 0x0003DD60
	private void Start()
	{
		for (int i = 0; i < this.chatTabs.Length; i++)
		{
			this.chatTabs[i].TabClicked += this.OnChatTabClicked;
		}
		this.currentChatWidth = this.chatWindow.rightAnchor.absolute;
		this.currentChatHeight = this.chatWindow.topAnchor.absolute;
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0003FBC8 File Offset: 0x0003DDC8
	private void OnDestroy()
	{
		for (int i = 0; i < this.chatTabs.Length; i++)
		{
			if (this.chatTabs[i] != null)
			{
				this.chatTabs[i].TabClicked -= this.OnChatTabClicked;
			}
		}
		EventManager.OnChangePlayerTeam -= this.PlayerChangedTeam;
		EventManager.OnAutoHideChat -= this.OnAutoHideChatChanged;
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003FC34 File Offset: 0x0003DE34
	public void SaveChatSize()
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

	// Token: 0x060008DA RID: 2266 RVA: 0x0003FCBC File Offset: 0x0003DEBC
	private void Update()
	{
		if (this.autoHideChat && this.ChatWindow.activeInHierarchy && Time.time > this.last_chat_time + 9f)
		{
			this.ChatWindow.SetActive(false);
		}
		if (this.isConsoleSwitching)
		{
			float num = (Time.time - this.consoleSwitchStart) / this.consoleSwitchDuration;
			if (num >= 1f)
			{
				this.chatWindow.rightAnchor.absolute = this.targetChatWidth;
				this.chatWindow.topAnchor.absolute = this.targetChatHeight;
				if (this.switchingToConsole)
				{
					this.chatSettings.SetConsoleColors(1f);
				}
				else
				{
					this.chatSettings.SetChatColors(1f);
				}
				this.isConsoleSwitching = false;
				return;
			}
			this.chatWindow.rightAnchor.absolute = (int)Mathf.Lerp((float)this.currentChatWidth, (float)this.targetChatWidth, num);
			this.chatWindow.topAnchor.absolute = (int)Mathf.Lerp((float)this.currentChatHeight, (float)this.targetChatHeight, num);
			if (this.switchingToConsole)
			{
				this.chatSettings.SetConsoleColors(num);
				return;
			}
			this.chatSettings.SetChatColors(num);
		}
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003FDED File Offset: 0x0003DFED
	public void SinglePlayerGameStarted()
	{
		this.tabsDict[ChatMessageType.Game].gameObject.SetActive(true);
		this.OnChatTabClicked(this.tabsDict[ChatMessageType.Game]);
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003FDED File Offset: 0x0003DFED
	public void MultiplayerGameStarted()
	{
		this.tabsDict[ChatMessageType.Game].gameObject.SetActive(true);
		this.OnChatTabClicked(this.tabsDict[ChatMessageType.Game]);
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0003FE18 File Offset: 0x0003E018
	public void PlayerChangedTeam(bool join, int id)
	{
		if (id == NetworkID.ID)
		{
			this.tabsDict[ChatMessageType.Team].gameObject.SetActive(join);
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003FE3C File Offset: 0x0003E03C
	private void AddChat(string newchat, string hexcolor, ChatMessageType type, bool canCollate = false)
	{
		EventManager.TriggerChatMessageType(type);
		if (type != ChatMessageType.Global)
		{
			this.RefreshChatLog();
		}
		else if (this.selectedTab.chatType == ChatMessageType.Global)
		{
			this.RefreshChatLog();
		}
		if (!string.IsNullOrEmpty(hexcolor))
		{
			newchat = newchat.Insert(0, hexcolor);
		}
		bool flag;
		this.chatSettings.chatDisplayTimestamp.TryGetValue(ChatMessageType.All, out flag);
		if (!flag)
		{
			this.chatSettings.chatDisplayTimestamp.TryGetValue(type, out flag);
		}
		if (flag)
		{
			newchat = newchat.Insert(0, Chat.getTimestamp());
		}
		this.ChatList.paragraphHistory = 400;
		int num = this.tabsDict[type].textList.Count - 1;
		if (canCollate && num >= 0 && this.tabsDict[type].textList[num].StartsWith(newchat))
		{
			int num2;
			int.TryParse(this.tabsDict[type].textList[num].Substring(newchat.Length).Replace("<", "").Replace(">", ""), out num2);
			if (num2 == 0)
			{
				num2++;
			}
			num2++;
			newchat = string.Concat(new object[]
			{
				newchat,
				" <",
				num2,
				">"
			});
			this.tabsDict[type].textList[num] = newchat;
			if (this.selectedTab.chatType == type)
			{
				this.ChatList.Edit(-1, newchat);
			}
		}
		else
		{
			this.tabsDict[type].AddText(newchat);
			if (this.selectedTab.chatType == type)
			{
				this.ChatList.Add(newchat);
			}
		}
		Chat.GlobalChatHistory = this.tabsDict[ChatMessageType.Global].textList;
		Chat.SystemChatHistory = this.tabsDict[ChatMessageType.System].textList;
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0004001E File Offset: 0x0003E21E
	public void SetChatFontSize(int size)
	{
		this.ChatOutputText.GetComponent<UILabel>().fontSize = size;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00040031 File Offset: 0x0003E231
	public int GetChatFontSize()
	{
		return this.ChatOutputText.GetComponent<UILabel>().fontSize;
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00040044 File Offset: 0x0003E244
	private static string getTimestamp()
	{
		return DateTime.Now.ToString(Chat.TIMESTAMP_FORMAT) + " ";
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0004006D File Offset: 0x0003E26D
	// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0004007A File Offset: 0x0003E27A
	public bool ChatActive
	{
		get
		{
			return this.ChatWindow.activeInHierarchy;
		}
		set
		{
			this.ChatWindow.SetActive(value);
			Chat.RefreshChat();
		}
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0004008D File Offset: 0x0003E28D
	public void ClearTab(ChatMessageType type)
	{
		if (this.selectedTab.chatType == type)
		{
			this.ChatList.Clear();
		}
		this.tabsDict[type].ClearText();
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000400BC File Offset: 0x0003E2BC
	public void ChangeSelectedTab(ChatTab tab)
	{
		if (this.selectedTab == tab)
		{
			return;
		}
		if (!this.isConsoleSwitching && (this.selectedTab.chatType == ChatMessageType.System || tab.chatType == ChatMessageType.System))
		{
			this.consoleSwitchStart = Time.time;
			this.isConsoleSwitching = true;
			this.SaveChatSize();
			this.currentChatWidth = this.chatWindow.rightAnchor.absolute;
			this.currentChatHeight = this.chatWindow.topAnchor.absolute;
			if (this.selectedTab.chatType == ChatMessageType.System)
			{
				PlayerPrefs.SetInt("ChatConsoleWidth", this.currentChatWidth);
				PlayerPrefs.SetInt("ChatConsoleHeight", this.currentChatHeight);
				this.targetChatWidth = PlayerPrefs.GetInt("ChatWidth", 428);
				this.targetChatHeight = PlayerPrefs.GetInt("ChatHeight", 261);
				this.switchingToConsole = false;
			}
			else
			{
				PlayerPrefs.SetInt("ChatWidth", this.currentChatWidth);
				PlayerPrefs.SetInt("ChatHeight", this.currentChatHeight);
				this.targetChatWidth = PlayerPrefs.GetInt("ChatConsoleWidth", 498);
				this.targetChatHeight = PlayerPrefs.GetInt("ChatConsoleHeight", 522);
				this.switchingToConsole = true;
			}
		}
		this.previousTab = this.selectedTab;
		this.selectedTab = tab;
		this.selectedTab.OnTabClicked(null);
		this.ChatList.Clear();
		this.ChatList.Add(tab.textList);
		if (this.selectedTab.chatType == ChatMessageType.System)
		{
			this.ChatOutputText.GetComponent<UILabel>().bitmapFont = this.ConsoleChatFont;
			this.ChatInputText.GetComponent<UILabel>().bitmapFont = this.ConsoleChatFont;
			this.ChatOutputText.GetComponent<UILabel>().effectStyle = UILabel.Effect.None;
			this.ChatInputText.GetComponent<UILabel>().effectStyle = UILabel.Effect.None;
			return;
		}
		this.ChatOutputText.GetComponent<UILabel>().bitmapFont = this.DefaultChatFont;
		this.ChatInputText.GetComponent<UILabel>().bitmapFont = this.DefaultChatFont;
		this.ChatOutputText.GetComponent<UILabel>().effectStyle = UILabel.Effect.Outline8;
		this.ChatInputText.GetComponent<UILabel>().effectStyle = UILabel.Effect.Outline8;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000402D9 File Offset: 0x0003E4D9
	public void RefreshChatLog()
	{
		this.ChatWindow.SetActive(true);
		this.last_chat_time = Time.time;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x000402F2 File Offset: 0x0003E4F2
	public static void RefreshChat()
	{
		NetworkSingleton<Chat>.Instance.RefreshChatLog();
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00040300 File Offset: 0x0003E500
	public static async void LogNoColour(string message, ChatMessageType type = ChatMessageType.Game)
	{
		await new WaitForUpdate();
		if (!Regex.IsMatch(message, "^\\[[0-9a-fA-F]{6}([0-9a-fA-F]{2})?\\]"))
		{
			message = "[FFFFFF]" + message;
		}
		if (type == ChatMessageType.All)
		{
			for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
			{
				if (NetworkSingleton<Chat>.Instance.chatTabs[i].chatType != ChatMessageType.System)
				{
					NetworkSingleton<Chat>.Instance.AddChat(message, null, NetworkSingleton<Chat>.Instance.chatTabs[i].chatType, false);
				}
			}
		}
		else
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, null, type, false);
		}
		if (type != ChatMessageType.System && (Singleton<SystemConsole>.Instance.mirrorTab[type] || Singleton<SystemConsole>.Instance.mirrorTab[ChatMessageType.All]))
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, null, ChatMessageType.System, false);
		}
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00040344 File Offset: 0x0003E544
	public static async void Log(string message, Colour colour, ChatMessageType type = ChatMessageType.Game, bool broadcast = false)
	{
		await new WaitForUpdate();
		if (type == ChatMessageType.All)
		{
			for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
			{
				if (NetworkSingleton<Chat>.Instance.chatTabs[i].chatType != ChatMessageType.System)
				{
					NetworkSingleton<Chat>.Instance.AddChat(message, colour.RGBHex, NetworkSingleton<Chat>.Instance.chatTabs[i].chatType, false);
				}
			}
		}
		else
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, colour.RGBHex, type, false);
		}
		if (type != ChatMessageType.System && (Singleton<SystemConsole>.Instance.mirrorTab[type] || Singleton<SystemConsole>.Instance.mirrorTab[ChatMessageType.All]))
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, colour.RGBHex, ChatMessageType.System, false);
		}
		if (broadcast)
		{
			UIBroadcast.Log(message, colour, 2f, 0f);
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00040395 File Offset: 0x0003E595
	public static void Log(string message, ChatMessageType type = ChatMessageType.Game)
	{
		Chat.Log(message, Colour.White, type, false);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x000403A4 File Offset: 0x0003E5A4
	public static void Log(string message, string label, ChatMessageType type = ChatMessageType.Game)
	{
		Chat.Log(message, Colour.ColourFromLabel(label), type, false);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000403B4 File Offset: 0x0003E5B4
	public static async void LogSystem(string message, Color color, ChatMessageType alsoLogTo, bool important = false)
	{
		await new WaitForUpdate();
		string hexcolor = Colour.RGBHexFromColour(color);
		if (important || alsoLogTo == ChatMessageType.All)
		{
			for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
			{
				NetworkSingleton<Chat>.Instance.AddChat(message, hexcolor, NetworkSingleton<Chat>.Instance.chatTabs[i].chatType, true);
			}
		}
		else
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, hexcolor, ChatMessageType.System, true);
			if (alsoLogTo != ChatMessageType.System)
			{
				NetworkSingleton<Chat>.Instance.AddChat(message, hexcolor, alsoLogTo, true);
			}
		}
		if (important)
		{
			UIBroadcast.Log(message, color, 2f, 0f);
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00040405 File Offset: 0x0003E605
	public static void LogSystem(string message, ChatMessageType alsoLogTo, bool important = false)
	{
		Chat.LogSystem(message, SystemConsole.OutputColour, alsoLogTo, important);
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00040419 File Offset: 0x0003E619
	public static void LogSystem(string message, Color color, bool important = false)
	{
		Chat.LogSystem(message, color, ChatMessageType.System, important);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00040424 File Offset: 0x0003E624
	public static void LogSystem(string message, bool important = false)
	{
		Chat.LogSystem(message, SystemConsole.OutputColour, ChatMessageType.System, important);
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00040438 File Offset: 0x0003E638
	public static string EscapeFormatting(string message)
	{
		return message.Replace("[", "[​");
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0004044C File Offset: 0x0003E64C
	public static async void LogWarning(string message, ChatMessageType alsoLogTo, bool important = false)
	{
		await new WaitForUpdate();
		message = Chat.EscapeFormatting(message);
		if (important || alsoLogTo == ChatMessageType.All)
		{
			for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
			{
				if (!SystemConsole.RestrictErrorsToConsole || NetworkSingleton<Chat>.Instance.chatTabs[i].chatType == ChatMessageType.System)
				{
					NetworkSingleton<Chat>.Instance.AddChat(message, "[E7E52C]", NetworkSingleton<Chat>.Instance.chatTabs[i].chatType, true);
				}
			}
		}
		else
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, "[E7E52C]", ChatMessageType.System, true);
			if (alsoLogTo != ChatMessageType.System)
			{
				NetworkSingleton<Chat>.Instance.AddChat(message, "[E7E52C]", alsoLogTo, true);
			}
		}
		if (important && !SystemConsole.DisableErrorBroadcast)
		{
			UIBroadcast.Log(message, Colour.Yellow, 2f, 0f);
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00040495 File Offset: 0x0003E695
	public static void LogWarning(string message, bool important = true)
	{
		Chat.LogWarning(message, ChatMessageType.System, important);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x000404A0 File Offset: 0x0003E6A0
	public static async void LogError(string message, ChatMessageType alsoLogTo, bool important = true)
	{
		await new WaitForUpdate();
		message = Chat.EscapeFormatting(message);
		if (important || alsoLogTo == ChatMessageType.All)
		{
			for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
			{
				if (!SystemConsole.RestrictErrorsToConsole || NetworkSingleton<Chat>.Instance.chatTabs[i].chatType == ChatMessageType.System)
				{
					NetworkSingleton<Chat>.Instance.AddChat(message, "[DA1918]", NetworkSingleton<Chat>.Instance.chatTabs[i].chatType, true);
				}
			}
		}
		else
		{
			NetworkSingleton<Chat>.Instance.AddChat(message, "[DA1918]", ChatMessageType.System, true);
			if (alsoLogTo != ChatMessageType.System)
			{
				NetworkSingleton<Chat>.Instance.AddChat(message, "[DA1918]", alsoLogTo, true);
			}
		}
		if (important && !SystemConsole.DisableErrorBroadcast)
		{
			UIBroadcast.Log(message, Colour.Red, 2f, 0f);
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x000404E9 File Offset: 0x0003E6E9
	public static void LogError(string message, bool important = true)
	{
		Chat.LogError(message, ChatMessageType.System, important);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x000404F4 File Offset: 0x0003E6F4
	public static async void LogException(string exceptionAction, Exception e, bool important = true, bool stackTrace = false)
	{
		await new WaitForUpdate();
		Debug.LogException(e);
		string text = string.Format("Error {0}: {1}", exceptionAction, e.Message);
		if (stackTrace)
		{
			text = text + " trace: " + e.StackTrace;
		}
		text = Chat.EscapeFormatting(text);
		if (important)
		{
			for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
			{
				NetworkSingleton<Chat>.Instance.AddChat(text, "[DA1918]", NetworkSingleton<Chat>.Instance.chatTabs[i].chatType, true);
			}
		}
		else
		{
			NetworkSingleton<Chat>.Instance.AddChat(text, "[DA1918]", ChatMessageType.System, true);
		}
		if (important)
		{
			UIBroadcast.Log(text, Colour.Red, 2f, 0f);
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00040548 File Offset: 0x0003E748
	public static async void LogObject(NetworkPhysicsObject npo)
	{
		await new WaitForUpdate();
		Chat.LogSystem("-", false);
		Singleton<SystemConsole>.Instance.LogVariable("Name", npo.name, 37);
		Singleton<SystemConsole>.Instance.LogVariable("GUID", npo.GUID, 37);
		Singleton<SystemConsole>.Instance.LogVariable("Type", npo.tag, 37);
		Singleton<SystemConsole>.Instance.LogVariable("Rotation", npo.transform.eulerAngles.ToString(), 37);
		JigsawPiece component = npo.GetComponent<JigsawPiece>();
		if (component)
		{
			Singleton<SystemConsole>.Instance.LogVariable("Position", string.Format("{0},{1}", component.desiredPosition.x, component.desiredPosition.z), 37);
			Singleton<SystemConsole>.Instance.LogVariable("Mesh", component.GetComponent<MeshSyncScript>().Meshes[0].name, 37);
		}
		Chat.LogSystem("*", false);
		if (npo.InvisibleHiders.Count > 0)
		{
			Chat.LogSystem("Invisible", false);
			foreach (KeyValuePair<string, uint> keyValuePair in npo.InvisibleHiders)
			{
				Singleton<SystemConsole>.Instance.LogVariable(keyValuePair.Key, string.Concat(keyValuePair.Value), 37);
			}
			Singleton<SystemConsole>.Instance.Log("Normal flags:", false);
			Colour.LogFlags(npo.NormalInvisibleFlags);
			Singleton<SystemConsole>.Instance.Log("Reverse flags:", false);
			Colour.LogFlags(npo.ReverseInvisibleFlags);
		}
		Chat.LogSystem("-", false);
		if (npo.ObscuredHiders.Count > 0)
		{
			Chat.LogSystem("Obscured", false);
			foreach (KeyValuePair<string, uint> keyValuePair2 in npo.ObscuredHiders)
			{
				Singleton<SystemConsole>.Instance.LogVariable(keyValuePair2.Key, string.Concat(keyValuePair2.Value), 37);
			}
			Singleton<SystemConsole>.Instance.Log("Normal flags:", false);
			Colour.LogFlags(npo.NormalObscuredFlags);
			Singleton<SystemConsole>.Instance.Log("Facedown flags:", false);
			Colour.LogFlags(npo.FacedownObscuredFlags);
			Singleton<SystemConsole>.Instance.Log("Reverse flags:", false);
			Colour.LogFlags(npo.ReverseObscuredFlags);
		}
		Chat.LogSystem("*", false);
		LayoutZone layoutZone;
		int num;
		if (LayoutZone.TryNPOInLayoutZone(npo, out layoutZone, out num, LayoutZone.PotentialZoneCheck.None))
		{
			Singleton<SystemConsole>.Instance.LogVariable("Layout Zone", num.ToString(), 37);
		}
		else
		{
			Singleton<SystemConsole>.Instance.LogVariable("Layout Zone", "-", 37);
		}
		for (int i = 0; i < npo.LayoutZonesContaining.Count; i++)
		{
			Singleton<SystemConsole>.Instance.LogVariable("Zone " + i, LayoutZone.LayoutZones.IndexOf(npo.LayoutZonesContaining[i]).ToString(), 37);
		}
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00040584 File Offset: 0x0003E784
	private void OnChatTabClicked(ChatTab tab)
	{
		this.RefreshChatLog();
		for (int i = 0; i < this.chatTabs.Length; i++)
		{
			if (this.chatTabs[i] == tab)
			{
				this.ChangeSelectedTab(tab);
			}
			else
			{
				this.chatTabs[i].Deselect();
			}
		}
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x000405D0 File Offset: 0x0003E7D0
	private void OnAutoHideChatChanged(bool allow)
	{
		this.autoHideChat = allow;
		if (!allow)
		{
			this.ChatWindow.SetActive(true);
			return;
		}
		this.last_chat_time = 0f;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x000405F4 File Offset: 0x0003E7F4
	public static void NotPromotedErrorMessage(string Action)
	{
		Chat.Log(string.Format("Cannot {0} because you are not promoted.", Action), Colour.Red, ChatMessageType.All, false);
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00040610 File Offset: 0x0003E810
	public static void InputChat(string msg, bool bTeamChat = false, string WhisperColor = "", ChatMessageType overrideChatType = ChatMessageType.None)
	{
		ChatMessageType chatMessageType;
		if (overrideChatType == ChatMessageType.None)
		{
			chatMessageType = NetworkSingleton<Chat>.Instance.selectedTab.chatType;
		}
		else
		{
			chatMessageType = overrideChatType;
		}
		if (overrideChatType == ChatMessageType.Team)
		{
			bTeamChat = true;
		}
		if (chatMessageType == ChatMessageType.System)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand(msg, false);
			return;
		}
		if (msg.Substring(0, 1) == "/")
		{
			Chat.ChatCMD(msg, chatMessageType);
			return;
		}
		if (bTeamChat)
		{
			Chat.SendChatTeamMessage(msg);
			return;
		}
		if (!string.IsNullOrEmpty(WhisperColor))
		{
			Chat.SendChatWhisperMessage(msg, NetworkSingleton<PlayerManager>.Instance.IDFromColour(WhisperColor));
			return;
		}
		if (chatMessageType == ChatMessageType.Global)
		{
			Singleton<ChatIRC>.Instance.SendIRCMessage(msg);
			return;
		}
		Chat.SendChatMessage(msg);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000406A2 File Offset: 0x0003E8A2
	public static void SendChat(string message)
	{
		NetworkSingleton<Chat>.Instance.networkView.RPC<string>(RPCTarget.All, new Action<string>(NetworkSingleton<Chat>.Instance.RPC_Chat), message);
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x000406C5 File Offset: 0x0003E8C5
	public static void SendChat(string message, string stringColor)
	{
		Chat.SendChat(message, Colour.ColourFromLabel(stringColor));
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x000406D8 File Offset: 0x0003E8D8
	public static void SendChat(string message, Color color)
	{
		message = Colour.RGBHexFromColour(color) + message;
		NetworkSingleton<Chat>.Instance.networkView.RPC<string>(RPCTarget.All, new Action<string>(NetworkSingleton<Chat>.Instance.RPC_Chat), message);
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0004070E File Offset: 0x0003E90E
	public static void SendChatMessage(string message)
	{
		if (Network.isServer)
		{
			NetworkSingleton<Chat>.Instance.RPC_ChatMessage(NetworkID.ID, message);
			return;
		}
		NetworkSingleton<Chat>.Instance.networkView.RPC<int, string>(RPCTarget.Server, new Action<int, string>(NetworkSingleton<Chat>.Instance.RPC_ChatMessage), NetworkID.ID, message);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00040750 File Offset: 0x0003E950
	public static void SendChatWhisperMessage(string message, int targetId)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(targetId);
		if (playerState.id != NetworkID.ID)
		{
			NetworkSingleton<Chat>.Instance.networkView.RPC<int, string, string>(playerState.networkPlayer, new Action<int, string, string>(NetworkSingleton<Chat>.Instance.RPC_ChatWhisperMessage), NetworkID.ID, message, playerState.stringColor);
		}
		NetworkSingleton<Chat>.Instance.RPC_ChatWhisperMessage(NetworkID.ID, message, playerState.stringColor);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000407C0 File Offset: 0x0003E9C0
	public static void SendChatTeamMessage(string message)
	{
		if (NetworkSingleton<PlayerManager>.Instance.MyPlayerState().team == Team.None)
		{
			Chat.LogError("You are not on a Team.", true);
			return;
		}
		List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		for (int i = 0; i < playersList.Count; i++)
		{
			PlayerState playerState = playersList[i];
			if (NetworkSingleton<PlayerManager>.Instance.SameTeam(playerState.stringColor, -1) && NetworkID.ID != playerState.id)
			{
				NetworkSingleton<Chat>.Instance.networkView.RPC<int, string>(playerState.networkPlayer, new Action<int, string>(NetworkSingleton<Chat>.Instance.RPC_ChatTeamMessage), NetworkID.ID, message);
			}
		}
		NetworkSingleton<Chat>.Instance.RPC_ChatTeamMessage(NetworkID.ID, message);
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0004086A File Offset: 0x0003EA6A
	[Remote(SendType.ReliableNoDelay)]
	private void RPC_Chat(string message)
	{
		this.LogWithSound(message, Singleton<ChatSettings>.Instance.FilterChatMessages);
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00040880 File Offset: 0x0003EA80
	[Remote(SendType.ReliableNoDelay)]
	private void RPC_ChatMessage(int senderId, string message)
	{
		this.LuaBlockChatMessage = false;
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(senderId);
		EventManager.TriggerChat(message, playerState.id);
		if (this.LuaBlockChatMessage)
		{
			this.LuaBlockChatMessage = false;
			return;
		}
		if (Network.isServer)
		{
			NetworkSingleton<Chat>.Instance.networkView.RPC<int, string>(RPCTarget.Others, new Action<int, string>(this.RPC_ChatMessage), senderId, message);
		}
		message = NGUIText.StripSymbols(message);
		TextCode.LocalizeUIText(ref message);
		string msg = Colour.HexFromColour(playerState.color) + playerState.name + ": [FFFFFF]" + message;
		this.LogWithSound(msg, Singleton<ChatSettings>.Instance.FilterChatMessages);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00040924 File Offset: 0x0003EB24
	[Remote(SendType.ReliableNoDelay)]
	private void RPC_ChatWhisperMessage(int senderId, string message, string whisperColor)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(senderId);
		string msg = string.Concat(new string[]
		{
			Colour.HexFromLabel(whisperColor),
			"<",
			whisperColor,
			"> ",
			Colour.HexFromColour(playerState.color),
			playerState.name,
			": [FFFFFF]",
			message
		});
		this.LogWithSound(msg, Singleton<ChatSettings>.Instance.FilterChatMessages);
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000409A0 File Offset: 0x0003EBA0
	[Remote(SendType.ReliableNoDelay)]
	private void RPC_ChatTeamMessage(int senderId, string message)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(senderId);
		string msg = string.Concat(new string[]
		{
			"<TEAM> ",
			Colour.HexFromColour(playerState.color),
			playerState.name,
			": [FFFFFF]",
			message
		});
		this.LogWithSound(msg, ChatMessageType.Team, Singleton<ChatSettings>.Instance.FilterChatMessages);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00040A08 File Offset: 0x0003EC08
	public void LogWithSound(string msg, ChatMessageType type = ChatMessageType.Game, bool filterMessage = false)
	{
		if (filterMessage)
		{
			bool flag;
			msg = Singleton<ChatIRC>.Instance.FilterChatMessage(msg, out flag);
		}
		Chat.LogNoColour(msg, type);
		base.GetComponent<SoundScript>().PlayGUISound(NetworkSingleton<NetworkUI>.Instance.ChatSound, 0.3f, 1f);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00040A4D File Offset: 0x0003EC4D
	public void LogWithSound(string msg, bool filterMessage)
	{
		this.LogWithSound(msg, ChatMessageType.Game, filterMessage);
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00040A58 File Offset: 0x0003EC58
	public void ShowDeveloperConsole(bool desiredVisibility = true)
	{
		for (int i = 0; i < this.chatTabs.Length; i++)
		{
			if (this.chatTabs[i].chatType == ChatMessageType.System)
			{
				this.chatTabs[i].gameObject.SetActive(desiredVisibility);
				return;
			}
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00040A9C File Offset: 0x0003EC9C
	public static void NotifyFromNetworkSender(string action)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID((int)Network.sender.id);
		Chat.Log(playerState.name + " " + action + ".", playerState.color, ChatMessageType.Game, false);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00040AEC File Offset: 0x0003ECEC
	private static void ChatCMD(string message, ChatMessageType type)
	{
		string text2;
		if (type != ChatMessageType.Global)
		{
			if (type - ChatMessageType.Game <= 1)
			{
				int i = 0;
				while (i < Colour.HandPlayerLabels.Length)
				{
					string text = Colour.HandPlayerLabels[i];
					if (Chat.MessageEqualCmd(message, "/" + text + " ", out text2))
					{
						string msg = text2;
						if (NetworkSingleton<PlayerManager>.Instance.ColourInUse(Colour.ColourFromLabel(text)))
						{
							Chat.InputChat(msg, false, text, ChatMessageType.None);
							return;
						}
						Chat.Log("No one is playing as " + text + ".", Colour.Red, type, false);
						return;
					}
					else
					{
						i++;
					}
				}
				if (Chat.MessageEqualCmd(message, "/team ", out text2))
				{
					Chat.InputChat(text2, true, "", ChatMessageType.None);
					return;
				}
				if (Chat.MessageEqualCmd(message, "/help"))
				{
					Chat.Log("Game Console Help, do not type <>, ex. /kick Batman", Colour.Purple, ChatMessageType.Game, false);
					if (Network.isServer)
					{
						Chat.Log("[31B32B]/kick <player name>[FFFFFF] [Ejects player from the game]", type);
						Chat.Log("[31B32B]/ban <player name>[FFFFFF] [Kicks player and adds them to block list]", type);
						Chat.Log("[31B32B]/promote <player name>[FFFFFF] [Promotes or Demotes player as admin]", type);
					}
					Chat.Log("[31B32B]/mute <player name>[FFFFFF] [Mutes or Unmutes player's voice chat]", type);
					Chat.Log("[31B32B]/<color> <message>[FFFFFF] [Whispers the player on this color]", type);
					Chat.Log("[31B32B]/team <message>[FFFFFF] [Message everyone on your team]", type);
					Chat.Log("[31B32B]/resetallsaved[FFFFFF] [Resets all saved data (General, Controls, UI, etc)]", type);
					Chat.Log("[31B32B]/filter /nofilter[FFFFFF] [Enable or disable chat filter]", type);
					Chat.Log("[31B32B]/clear[FFFFFF] [Deletes all text from this tab]", type);
					return;
				}
				if (Network.isServer)
				{
					if (Chat.MessageEqualCmd(message, "/kick ", out text2))
					{
						string text3 = text2;
						List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
						for (int j = 0; j < playersList.Count; j++)
						{
							if (playersList[j].name.ToUpper() == text3.ToUpper())
							{
								NetworkSingleton<PlayerManager>.Instance.KickThisPlayer(playersList[j].name);
								return;
							}
						}
						Chat.Log(text3 + " does not exist.", Colour.Red, type, false);
						return;
					}
					if (Chat.MessageEqualCmd(message, "/ban ", out text2))
					{
						string text4 = text2;
						List<PlayerState> playersList2 = NetworkSingleton<PlayerManager>.Instance.PlayersList;
						for (int k = 0; k < playersList2.Count; k++)
						{
							if (playersList2[k].name.ToUpper() == text4.ToUpper())
							{
								NetworkSingleton<PlayerManager>.Instance.BanThisPlayer(playersList2[k].name);
								return;
							}
						}
						Chat.Log(text4 + " does not exist.", Colour.Red, type, false);
						return;
					}
					if (Chat.MessageEqualCmd(message, "/promote ", out text2))
					{
						string text5 = text2;
						List<PlayerState> playersList3 = NetworkSingleton<PlayerManager>.Instance.PlayersList;
						for (int l = 0; l < playersList3.Count; l++)
						{
							if (playersList3[l].name.ToUpper() == text5.ToUpper())
							{
								NetworkSingleton<PlayerManager>.Instance.PromoteThisPlayer(playersList3[l].name);
								return;
							}
						}
						Chat.Log(text5 + " does not exist.", Colour.Red, type, false);
						return;
					}
					if (Chat.MessageEqualCmd(message, "/execute ", out text2))
					{
						string script = text2;
						LuaGlobalScriptManager.Instance.ExecuteScript(script, false);
						return;
					}
				}
				if (Chat.MessageEqualCmd(message, "/filter"))
				{
					Singleton<SystemConsole>.Instance.ProcessCommand("+chat_filter", false, SystemConsole.CommandEcho.Silent);
					Chat.Log("Chat filter: ON (use /nofilter to disable)", Colour.Green, type, false);
					return;
				}
				if (Chat.MessageEqualCmd(message, "/nofilter"))
				{
					Singleton<SystemConsole>.Instance.ProcessCommand("-chat_filter", false, SystemConsole.CommandEcho.Silent);
					Chat.Log("Chat filter: OFF (use /filter to enable)", Colour.Green, type, false);
					return;
				}
				if (Chat.MessageEqualCmd(message, "/mute ", out text2))
				{
					string text6 = text2;
					List<PlayerState> playersList4 = NetworkSingleton<PlayerManager>.Instance.PlayersList;
					for (int m = 0; m < playersList4.Count; m++)
					{
						if (playersList4[m].name.ToUpper() == text6.ToUpper())
						{
							NetworkSingleton<PlayerManager>.Instance.MuteThisPlayer(playersList4[m].name);
							return;
						}
					}
					Chat.Log(text6 + " does not exist.", Colour.Red, type, false);
					return;
				}
				if (Chat.MessageEqualCmd(message, "/recompilesave"))
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.Recompile(true);
					return;
				}
				if (Chat.MessageEqualCmd(message, "/recompile"))
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.Recompile(false);
					return;
				}
			}
		}
		else
		{
			if (Chat.MessageEqualCmd(message, "/help"))
			{
				Chat.Log("Global Console Help, do not type <>, ex. /nick Batman", Colour.Purple, ChatMessageType.Global, false);
				Chat.Log("[31B32B]/nick <player name>[FFFFFF] [Changes your nickname]", ChatMessageType.Global);
				Chat.Log("[31B32B]/msg <player name> <message>[FFFFFF] [Private message a player]", ChatMessageType.Global);
				Chat.Log("[31B32B]/rules[FFFFFF] [Global Chat Rules]", ChatMessageType.Global);
				Chat.Log("[31B32B]/resetallsaved[FFFFFF] [Resets all saved data (General, Controls, UI, etc)]", ChatMessageType.Global);
				Chat.Log("[31B32B]/filter /nofilter[FFFFFF] [Enable or disable chat filter]", ChatMessageType.Global);
				Chat.Log("[31B32B]/clear[FFFFFF] [Deletes all text from this tab]", ChatMessageType.Global);
				return;
			}
			if (Chat.MessageEqualCmd(message, "/filter"))
			{
				Singleton<SystemConsole>.Instance.ProcessCommand("+chat_filter", false, SystemConsole.CommandEcho.Silent);
				Chat.Log("Chat filter: ON (use /nofilter to disable)", Colour.Green, ChatMessageType.Global, false);
				return;
			}
			if (Chat.MessageEqualCmd(message, "/nofilter"))
			{
				Singleton<SystemConsole>.Instance.ProcessCommand("-chat_filter", false, SystemConsole.CommandEcho.Silent);
				Chat.Log("Chat filter: OFF (use /filter to enable)", Colour.Green, ChatMessageType.Global, false);
				return;
			}
			if (Chat.MessageEqualCmd(message, "/nick ", out text2))
			{
				string text7 = text2;
				if (!string.IsNullOrEmpty(text7))
				{
					if (Singleton<ChatIRC>.Instance.NewNickname(text7))
					{
						PlayerPrefs.SetString("NickIRC", text7);
						return;
					}
				}
				else
				{
					Chat.Log("Nickname is empty.", Colour.Red, ChatMessageType.Global, false);
				}
				return;
			}
			if (Chat.MessageEqualCmd(message, "/msg ", out text2))
			{
				string text8 = text2;
				if (string.IsNullOrEmpty(text8))
				{
					Chat.Log("Private message arguments are empty.", Colour.Red, ChatMessageType.Global, false);
					return;
				}
				string[] array = text8.Split(new char[]
				{
					' '
				});
				if (array.Length > 1)
				{
					string user = array[0];
					string text9 = string.Empty;
					for (int n = 1; n < array.Length; n++)
					{
						text9 = text9 + array[n] + " ";
					}
					Singleton<ChatIRC>.Instance.PrivateMessage(user, text9);
				}
				return;
			}
			else if (Chat.MessageEqualCmd(message, "/rules"))
			{
				TTSUtilities.OpenURL("https://kb.tabletopsimulator.com/getting-started/chat-rules/");
				return;
			}
		}
		if (Chat.MessageEqualCmd(message, "/resetallsaved"))
		{
			NetworkSingleton<NetworkUI>.Instance.ResetAllSavedData();
			return;
		}
		if (Chat.MessageEqualCmd(message, "/clear"))
		{
			NetworkSingleton<Chat>.Instance.ClearTab(type);
			return;
		}
		if (Chat.MessageEqualCmd(message, "/vrresscale ", out text2))
		{
			float num;
			if (float.TryParse(text2, out num))
			{
				SteamVR_Camera.sceneResolutionScale = num;
				Chat.Log("VR Resolution Scale = " + num, type);
			}
			return;
		}
		if (Chat.MessageEqualCmd(message, "/threading"))
		{
			Singleton<ConfigGame>.Instance.settings.ConfigMods.Threading = !Singleton<ConfigGame>.Instance.settings.ConfigMods.Threading;
			Chat.Log("Threading mod loading = " + Singleton<ConfigGame>.Instance.settings.ConfigMods.Threading.ToString(), Colour.Green, type, false);
			return;
		}
		if (Chat.MessageEqualCmd(message, "/debug ", out text2))
		{
			bool bDebug;
			if (bool.TryParse(text2, out bDebug))
			{
				Debugging.bDebug = bDebug;
				Chat.Log("Debug mode = " + Debugging.bDebug.ToString(), type);
			}
			return;
		}
		if (Chat.MessageEqualCmd(message, "/debug"))
		{
			Debugging.bDebug = !Debugging.bDebug;
			Chat.Log("Debug mode = " + Debugging.bDebug.ToString(), type);
			return;
		}
		if (Chat.MessageEqualCmd(message, "/log ", out text2))
		{
			bool bLog;
			if (bool.TryParse(text2, out bLog))
			{
				Debugging.bLog = bLog;
				Chat.Log("Log mode = " + Debugging.bLog.ToString(), type);
			}
			return;
		}
		if (Chat.MessageEqualCmd(message, "/log"))
		{
			Debugging.bLog = !Debugging.bLog;
			Chat.Log("Log mode = " + Debugging.bLog.ToString(), type);
			return;
		}
		if (Chat.MessageEqualCmd(message, "/mics"))
		{
			string[] devices = Microphone.devices;
			for (int num2 = 0; num2 < devices.Length; num2++)
			{
				Chat.Log(num2 + 1 + ": " + devices[num2], type);
			}
			return;
		}
		if (Chat.MessageEqualCmd(message, "/setmic ", out text2))
		{
			int num3 = -1;
			if (int.TryParse(text2, out num3))
			{
				num3--;
			}
			string[] devices2 = Microphone.devices;
			if (num3 >= 0 && num3 < devices2.Length)
			{
				if (SteamVoiceP2PCommsNetwork.Instance)
				{
					SteamVoiceP2PCommsNetwork.Instance.Comms.MicrophoneName = devices2[num3];
				}
				Chat.Log("Microphone = " + devices2[num3], type);
				return;
			}
			Chat.LogError("Bad microphone number.", type, true);
			return;
		}
		else if (Chat.MessageEqualCmd(message, "/networktickrate ", out text2))
		{
			float num4;
			if (float.TryParse(text2, out num4))
			{
				num4 = Mathf.Clamp(num4, 5f, 120f);
				NetworkManager.TickRate = 1f / num4;
				Chat.Log(string.Concat(new object[]
				{
					"Network tick rate = ",
					num4,
					" : ",
					NetworkManager.TickRate
				}), type);
				return;
			}
			Chat.LogError("Bad network tick rate.", type, true);
			return;
		}
		else
		{
			if (Chat.MessageEqualCmd(message, "/networktickrate"))
			{
				Chat.Log(string.Concat(new object[]
				{
					"Network tick rate = ",
					1f / NetworkManager.TickRate,
					" : ",
					NetworkManager.TickRate
				}), type);
				return;
			}
			if (Chat.MessageEqualCmd(message, "/networkpackets ", out text2))
			{
				int num5;
				if (int.TryParse(text2, out num5))
				{
					num5 = Mathf.Clamp(num5, 1, 10);
					NetworkManager.UnreliablePacketsPerTick = num5;
					Chat.Log("Network packets = " + num5, type);
					return;
				}
				Chat.LogError("Bad network packets.", type, true);
				return;
			}
			else
			{
				if (Chat.MessageEqualCmd(message, "/networkpackets"))
				{
					Chat.Log("Network packets = " + NetworkManager.UnreliablePacketsPerTick, type);
					return;
				}
				if (Chat.MessageEqualCmd(message, "/networkinterpolate ", out text2))
				{
					float num6;
					if (float.TryParse(text2, out num6))
					{
						num6 = Mathf.Clamp(num6, 0f, 5f);
						NetworkInterpolate.InterpolateMulti = num6;
						Chat.Log("Network intepolate = " + NetworkInterpolate.InterpolateMulti, type);
						return;
					}
					Chat.LogError("Bad network interpolate.", type, true);
					return;
				}
				else
				{
					if (Chat.MessageEqualCmd(message, "/networkinterpolate"))
					{
						Chat.Log("Network intepolate = " + NetworkInterpolate.InterpolateMulti, type);
						return;
					}
					if (Chat.MessageEqualCmd(message, "/networkquality ", out text2))
					{
						int quality;
						if (int.TryParse(text2, out quality))
						{
							NetworkManager.Quality = (NetworkManager.NetworkQuality)quality;
							Chat.Log("Network quality = " + NetworkManager.Quality, type);
							return;
						}
						Chat.LogError("Bad network quality.", type, true);
						return;
					}
					else
					{
						if (Chat.MessageEqualCmd(message, "/networkquality"))
						{
							Chat.Log("Network quality = " + NetworkManager.Quality, type);
							return;
						}
						if (Chat.MessageEqualCmd(message, "/networkbuffering"))
						{
							NetworkManager.Buffering = !NetworkManager.Buffering;
							Chat.Log("Network buffering = " + NetworkManager.Buffering.ToString(), type);
							return;
						}
						if (Chat.MessageEqualCmd(message, "/dev"))
						{
							NetworkSingleton<Chat>.Instance.ShowDeveloperConsole(true);
						}
						Chat.Log("Invalid command. Type /help", Colour.Red, type, false);
						return;
					}
				}
			}
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x000415BE File Offset: 0x0003F7BE
	private static bool MessageEqualCmd(string message, string command)
	{
		return message.ToLower().StartsWith(command.ToLower());
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x000415D4 File Offset: 0x0003F7D4
	private static bool MessageEqualCmd(string message, string command, out string secondaryCommand)
	{
		secondaryCommand = null;
		bool flag = Chat.MessageEqualCmd(message, command);
		if (flag && message.Length > command.Length)
		{
			secondaryCommand = message.Substring(command.Length, message.Length - command.Length);
		}
		return flag && !string.IsNullOrEmpty(secondaryCommand);
	}

	// Token: 0x04000664 RID: 1636
	public const float CHAT_AUTO_HIDE_TIME = 9f;

	// Token: 0x04000665 RID: 1637
	public const int MAX_CHAT_HISTORY = 400;

	// Token: 0x04000666 RID: 1638
	public static string TIMESTAMP_FORMAT = "[555555][hh:mm:ss][-] ";

	// Token: 0x04000667 RID: 1639
	[HideInInspector]
	public ChatTab selectedTab;

	// Token: 0x04000668 RID: 1640
	[HideInInspector]
	public ChatTab previousTab;

	// Token: 0x04000669 RID: 1641
	public ChatTab[] chatTabs;

	// Token: 0x0400066A RID: 1642
	private Dictionary<ChatMessageType, ChatTab> tabsDict = new Dictionary<ChatMessageType, ChatTab>();

	// Token: 0x0400066B RID: 1643
	public GameObject ChatWindow;

	// Token: 0x0400066C RID: 1644
	private UIWidget chatWindow;

	// Token: 0x0400066D RID: 1645
	public UITextList ChatList;

	// Token: 0x0400066E RID: 1646
	public ChatSettings chatSettings;

	// Token: 0x0400066F RID: 1647
	public GameObject ChatOutputText;

	// Token: 0x04000670 RID: 1648
	public GameObject ChatInputText;

	// Token: 0x04000671 RID: 1649
	public UIDragResize ChatResizer;

	// Token: 0x04000672 RID: 1650
	public UIFont DefaultChatFont;

	// Token: 0x04000673 RID: 1651
	public UIFont ConsoleChatFont;

	// Token: 0x04000674 RID: 1652
	private float last_chat_time;

	// Token: 0x04000675 RID: 1653
	private bool autoHideChat = true;

	// Token: 0x04000676 RID: 1654
	public float consoleSwitchDuration = 0.1f;

	// Token: 0x04000677 RID: 1655
	[HideInInspector]
	public bool isConsoleSwitching;

	// Token: 0x04000678 RID: 1656
	private bool switchingToConsole;

	// Token: 0x04000679 RID: 1657
	private float consoleSwitchStart;

	// Token: 0x0400067A RID: 1658
	private int currentChatWidth;

	// Token: 0x0400067B RID: 1659
	private int currentChatHeight;

	// Token: 0x0400067C RID: 1660
	private int targetChatWidth;

	// Token: 0x0400067D RID: 1661
	private int targetChatHeight;

	// Token: 0x0400067E RID: 1662
	private static List<string> GlobalChatHistory = new List<string>();

	// Token: 0x0400067F RID: 1663
	private static List<string> SystemChatHistory = new List<string>();

	// Token: 0x04000680 RID: 1664
	public bool LuaBlockChatMessage;
}
