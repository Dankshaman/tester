using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using yIRC;

// Token: 0x02000265 RID: 613
public class ChatIRC : Singleton<ChatIRC>, IrcClient.IListConsumer<ChannelInfo>
{
	// Token: 0x0600204A RID: 8266 RVA: 0x000E964C File Offset: 0x000E784C
	protected override void Awake()
	{
		base.Awake();
		if (ChatIRC.bHasSpawned)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ChatIRC.bHasSpawned = true;
		EventManager.OnAllowMessagesChanged += this.AllowMessages;
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		this.allowMessages = (PlayerPrefs.GetInt("GlobalallowMessages", 1) == 1);
		if ((double)PlayerPrefs.GetInt("LastDownloadOfChatFilter", 0) < LibTime.DaysSinceTTSEpoch)
		{
			Wait.Frames(delegate
			{
				this.RefreshWordlist(true);
			}, 10);
			return;
		}
		Wait.Frames(delegate
		{
			this.RefreshWordlist(false);
		}, 10);
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000E96E9 File Offset: 0x000E78E9
	private void Start()
	{
		this.userName = Regex.Replace(NetworkSingleton<NetworkUI>.Instance.GetPlayerNickIRC(), "[^0-9a-zA-Z\\[\\]\\^_`{|}]+", "");
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x000025B8 File Offset: 0x000007B8
	public void Init()
	{
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000E970A File Offset: 0x000E790A
	private void PingNames()
	{
		if (this.isConnected())
		{
			this.irc.Names(this.channel);
			base.Invoke("PingNames", 60f);
		}
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x000E9738 File Offset: 0x000E7938
	private void OnDestroy()
	{
		if (this.irc != null)
		{
			this.irc.OnError -= this.irc_OnError;
			this.irc.OnRawMessage -= this.irc_OnRawMessage;
			this.irc.OnQueryMessage -= this.irc_OnQueryMessage;
			this.irc.UnregisterChannelListConsumer(this);
		}
		EventManager.OnAllowMessagesChanged -= this.AllowMessages;
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000025B8 File Offset: 0x000007B8
	public void Add(ChannelInfo entry)
	{
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000025B8 File Offset: 0x000007B8
	public void Clear()
	{
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000025B8 File Offset: 0x000007B8
	public void Finished()
	{
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000E97AF File Offset: 0x000E79AF
	private void Connecting(object sender, EventArgs e)
	{
		this.connecting = true;
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000E97B8 File Offset: 0x000E79B8
	private void Connected(object sender, EventArgs e)
	{
		this.connecting = false;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000E97B8 File Offset: 0x000E79B8
	private void ConnectedError(object sender, EventArgs e)
	{
		this.connecting = false;
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000E97C1 File Offset: 0x000E79C1
	private void CheckConnected()
	{
		if (!this.isConnected())
		{
			this.Connect();
		}
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x000E97D4 File Offset: 0x000E79D4
	public void Connect()
	{
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000E97E1 File Offset: 0x000E79E1
	private void Update()
	{
		if (this.isConnected())
		{
			this.irc.ListenOnce(false);
		}
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000E97F8 File Offset: 0x000E79F8
	private void irc_OnChannelMessage(object sender, ActionEventArgs e)
	{
		if (e.Data.Type == ReceiveType.Join && this.irc.Nickname.Equals(e.Data.Nick))
		{
			Chat.LogSystem(string.Format("Connected to {0} {1}.", this.serverName, this.channel), Colour.Green, ChatMessageType.Global, false);
			Chat.Log(string.Format("Press <Enter> then /help for chat commands.", this.serverName, this.channel), Colour.Green, ChatMessageType.Global, false);
		}
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x000E987C File Offset: 0x000E7A7C
	private void irc_OnRawMessage(object sender, IrcEventArgs e)
	{
		string text = NGUIText.StripSymbols(e.Data.Nick);
		string text2 = NGUIText.StripSymbols(e.Data.Message);
		if (text != null)
		{
			TextCode.LocalizeUIText(ref text);
		}
		if (text2 != null)
		{
			TextCode.LocalizeUIText(ref text2);
		}
		if (e.Data.Type == ReceiveType.Join)
		{
			if (this.irc.Nickname.Equals(e.Data.Nick))
			{
				Chat.LogSystem(string.Format("Connected to {0} {1}.", this.serverName, this.channel), Colour.Green, ChatMessageType.Global, false);
				Chat.Log("Press <Enter> then /help for chat commands.", Colour.Purple, ChatMessageType.Global, false);
				return;
			}
		}
		else if (e.Data.Type != ReceiveType.Part && e.Data.Type != ReceiveType.Quit)
		{
			if (e.Data.Type == ReceiveType.Kick)
			{
				Chat.LogSystem(string.Format("{0} has been kicked from the channel.", e.Data.RawMessageArray[3]), Colour.Red, ChatMessageType.Global, false);
				return;
			}
			if (e.Data.Type == ReceiveType.NickChange)
			{
				Chat.LogSystem(string.Format("{0} changed nick to {1}", text, text2), Colour.Green, ChatMessageType.Global, false);
				return;
			}
			if (e.Data.Type == ReceiveType.ChannelMessage)
			{
				ChannelUser channelUser = this.irc.GetChannelUser(this.channel, e.Data.Nick);
				if (channelUser == null || !channelUser.IsOp)
				{
					this.LogIRCMessage(text, text2, ChatIRC.IRCMessageType.Normal);
					return;
				}
				bool flag = Developer.HasName(text);
				if (!this.Bots.Contains(text) && !flag)
				{
					this.LogIRCMessage(text, text2, ChatIRC.IRCMessageType.Moderator);
					return;
				}
				if (text2.ToUpper().StartsWith("[BROADCAST]"))
				{
					this.LogIRCMessage(text, text2, ChatIRC.IRCMessageType.Broadcast);
					return;
				}
				if (flag)
				{
					this.LogIRCMessage(text, text2, ChatIRC.IRCMessageType.Developer);
					return;
				}
				this.LogIRCMessage(text, text2, ChatIRC.IRCMessageType.Bot);
				return;
			}
			else
			{
				if (e.Data.Type == ReceiveType.ErrorMessage && !text2.Equals("You have not registered", StringComparison.CurrentCultureIgnoreCase))
				{
					Chat.LogError(string.Format("Error {0}", text2), ChatMessageType.Global, true);
					return;
				}
				if (e.Data.Type == ReceiveType.Name && ChatIRC.showNames)
				{
					if (!e.Data.Channel.Equals(this.channel, StringComparison.CurrentCultureIgnoreCase))
					{
						return;
					}
					string text3 = string.Empty;
					for (int i = 0; i < e.Data.MessageArray.Length; i++)
					{
						if (!string.IsNullOrEmpty(e.Data.MessageArray[i]))
						{
							if (i == 0)
							{
								text3 += e.Data.MessageArray[i];
							}
							else
							{
								text3 = text3 + ", " + e.Data.MessageArray[i];
							}
						}
					}
					Chat.LogSystem(text3, Colour.Green, ChatMessageType.Global, false);
				}
			}
		}
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000E9B30 File Offset: 0x000E7D30
	public void LogIRCMessage(string Nick, string Message, ChatIRC.IRCMessageType Type)
	{
		if (Type != ChatIRC.IRCMessageType.Moderator && Type != ChatIRC.IRCMessageType.Developer && Type != ChatIRC.IRCMessageType.Bot && Message.Trim().Length == 1)
		{
			if ((DateTime.Now - this.lastSingleCharMessage).TotalSeconds < 30.0)
			{
				return;
			}
			this.lastSingleCharMessage = DateTime.Now;
		}
		string text;
		switch (Type)
		{
		case ChatIRC.IRCMessageType.Moderator:
			text = "[F4641D][MOD] ";
			goto IL_CF;
		case ChatIRC.IRCMessageType.Developer:
			text = "[DA1918][DEV] ";
			goto IL_CF;
		case ChatIRC.IRCMessageType.Bot:
			text = "[31B32B][BOT] ";
			goto IL_CF;
		case ChatIRC.IRCMessageType.Broadcast:
			text = "[F570CE][BROADCAST] ";
			UIBroadcast.Log(Message, Colour.Pink, 8f, 0f);
			Message = Message.Replace("[BROADCAST] ", "");
			Message = Message.Replace("[BROADCAST]", "");
			goto IL_CF;
		}
		text = "[1F87FF]";
		IL_CF:
		string text2 = Message.Contains(this.userName) ? "[A020F0]" : "[FFFFFF]";
		string message = string.Format("{0}<{1}> {2}{3}{4}", new object[]
		{
			text,
			Nick,
			text2,
			Message,
			"[FFFFFF]"
		});
		if (Type != ChatIRC.IRCMessageType.Moderator && Type != ChatIRC.IRCMessageType.Developer && Type != ChatIRC.IRCMessageType.Bot)
		{
			bool flag;
			message = this.FilterChatMessage(message, out flag);
			if (flag)
			{
				return;
			}
		}
		Chat.LogNoColour(message, ChatMessageType.Global);
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x000E9C74 File Offset: 0x000E7E74
	public void irc_OnQueryMessage(object sender, IrcEventArgs e)
	{
		string arg = NGUIText.StripSymbols(e.Data.Nick);
		string arg2 = NGUIText.StripSymbols(e.Data.Message);
		Chat.Log(string.Format("From <{0}> {1}", arg, arg2), Colour.Pink, ChatMessageType.Global, false);
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000E9CBB File Offset: 0x000E7EBB
	private void NonConnectedErrorAndReconnect()
	{
		Chat.LogError("Not connected to IRC.", ChatMessageType.Global, true);
		this.Connect();
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000E9CD0 File Offset: 0x000E7ED0
	public void SendIRCMessage(string message)
	{
		Achievements.Set("ACH_JOIN_LOBBY");
		if (this.isConnected())
		{
			if (message.Trim().Length == 1)
			{
				this.lastSingleCharMessage = DateTime.Now;
			}
			if (ChatIRC.ENABLE_IRC_SEND)
			{
				this.irc.SendMessage(SendType.Message, this.channel, message);
			}
			string message2 = string.Format("<{0}> {1}", this.irc.Nickname, message);
			if (Singleton<ChatSettings>.Instance.FilterChatMessages)
			{
				bool flag;
				message2 = this.FilterChatMessage(message2, out flag);
			}
			Chat.Log(message2, ChatMessageType.Global);
			return;
		}
		this.NonConnectedErrorAndReconnect();
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000E9D5D File Offset: 0x000E7F5D
	public bool NewNickname(string newNickname)
	{
		if (this.isConnected())
		{
			this.irc.Nick(newNickname);
			return true;
		}
		this.NonConnectedErrorAndReconnect();
		return false;
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000E9D7C File Offset: 0x000E7F7C
	public void Emote(string message)
	{
		if (this.isConnected())
		{
			this.irc.SendMessage(SendType.Action, this.channel, message);
			return;
		}
		this.NonConnectedErrorAndReconnect();
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000E9DA0 File Offset: 0x000E7FA0
	public void NamesList()
	{
		if (this.isConnected())
		{
			ChatIRC.showNames = true;
			this.irc.Names(this.channel);
			return;
		}
		this.NonConnectedErrorAndReconnect();
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000E9DC8 File Offset: 0x000E7FC8
	public void Kick(string user)
	{
		if (this.irc.GetIrcUser(this.userName).IsIrcOp)
		{
			this.irc.Kick(this.channel, user);
			return;
		}
		Chat.LogError("Error you are not an Admin.", ChatMessageType.Global, true);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000E9E01 File Offset: 0x000E8001
	public void Ban(string user)
	{
		if (this.irc.GetIrcUser(this.userName).IsIrcOp)
		{
			this.irc.Ban(this.channel, user);
			return;
		}
		Chat.LogError("Error you are not an Admin.", ChatMessageType.Global, true);
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x000E9E3A File Offset: 0x000E803A
	public void PrivateMessage(string user, string message)
	{
		if (this.isConnected())
		{
			this.irc.Privmsg(user, message, Priority.Critical);
			Chat.Log(string.Format("To <{0}> {1}", user, message), Colour.Pink, ChatMessageType.Global, false);
			return;
		}
		this.NonConnectedErrorAndReconnect();
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000E9E71 File Offset: 0x000E8071
	private void irc_OnError(object sender, yIRC.ErrorEventArgs e)
	{
		Chat.LogError(e.ErrorMessage, ChatMessageType.Global, true);
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x000E9E80 File Offset: 0x000E8080
	private void Disconnected(object sender, EventArgs e)
	{
		Chat.LogSystem("Disconnected from IRC.", Color.red, ChatMessageType.Global, false);
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x000E9E93 File Offset: 0x000E8093
	public bool isConnected()
	{
		return this.irc != null && this.irc.IsConnected;
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x000E9EAA File Offset: 0x000E80AA
	private void OnApplicationQuit()
	{
		this.StopIRC();
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000E9EB4 File Offset: 0x000E80B4
	private void StopIRC()
	{
		if (this.irc != null)
		{
			try
			{
				this.irc.Quit("Application close");
				this.irc.Disconnect();
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000E9EFC File Offset: 0x000E80FC
	public void Disconnect()
	{
		this.connecting = false;
		if (this.isConnected())
		{
			this.irc.Disconnect();
		}
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x000E9F18 File Offset: 0x000E8118
	public void AllowMessages(bool allow, ChatMessageType type)
	{
		if (type != ChatMessageType.Global)
		{
			return;
		}
		this.allowMessages = allow;
		if (!allow && this.isConnected())
		{
			this.irc.Disconnect();
			return;
		}
		if (allow && !this.isConnected())
		{
			this.Init();
		}
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x000E9F4D File Offset: 0x000E814D
	public void RefreshWordlist(bool force = false)
	{
		Singleton<CustomLoadingManager>.Instance.Text.Load("https://raw.githubusercontent.com/Berserk-Games/Tabletop-Simulator-Bad-Word-List/master/wordlist.sh", new Action<CustomTextContainer>(this.ProcessWordlist), force, CustomLoadingManager.LoadType.Auto, true);
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x000E9F74 File Offset: 0x000E8174
	private void ProcessWordlist(CustomTextContainer text)
	{
		PlayerPrefs.SetInt("LastDownloadOfChatFilter", (int)LibTime.DaysSinceTTSEpoch);
		Dictionary<ChatIRC.ChatRuleCategory, List<string>> dictionary = new Dictionary<ChatIRC.ChatRuleCategory, List<string>>();
		ChatIRC.ChatRuleCategory chatRuleCategory = new ChatIRC.ChatRuleCategory("spam", "mask");
		ChatIRC.ChatRuleCategory chatRuleCategory2 = chatRuleCategory;
		dictionary[chatRuleCategory2] = new List<string>();
		List<string> list = dictionary[chatRuleCategory2];
		using (StringReader stringReader = new StringReader(text.Text))
		{
			string text2;
			while ((text2 = stringReader.ReadLine()) != null)
			{
				text2 = text2.Trim();
				if (text2.StartsWith("/") && text2.EndsWith("/"))
				{
					if (text2.Length > 2)
					{
						text2 = text2.Substring(1, text2.Length - 2);
						try
						{
							Regex.Match("", text2);
						}
						catch (ArgumentException)
						{
							text2 = Regex.Escape(text2);
						}
						list.Add(text2);
					}
				}
				else
				{
					text2 = text2.Replace("##", "commentsymbol").Replace(",,", "splitsymbol").Replace("::", "exclusionsymbol").ToLower();
					int num = text2.IndexOf("#");
					if (num != -1)
					{
						text2 = text2.Substring(0, num).Trim();
						if (text2 == "")
						{
							continue;
						}
					}
					if (text2.StartsWith(":"))
					{
						string text3 = text2.Substring(1).Trim();
						num = text3.IndexOf("[");
						string text4;
						if (num == -1)
						{
							text4 = "mask";
						}
						else
						{
							text4 = text3.Substring(num + 1);
							text3 = text3.Substring(0, num).Trim();
							num = text4.IndexOf("]");
							if (num == -1)
							{
								text4 = text4.Trim();
							}
							else
							{
								text4 = text4.Substring(0, num).Trim();
							}
						}
						chatRuleCategory2 = new ChatIRC.ChatRuleCategory(text3, text4);
						if (!dictionary.ContainsKey(chatRuleCategory2))
						{
							dictionary[chatRuleCategory2] = new List<string>();
							this.chatRuleCategories.Add(chatRuleCategory2);
						}
						list = dictionary[chatRuleCategory2];
					}
					else
					{
						text2 = text2.Replace("commentsymbol", "#").Replace("exclusionsymbol", ":");
						for (string text5 = LibString.bite(ref text2, false, ',', true, true, '\0'); text5 != null; text5 = LibString.bite(ref text2, false, ',', true, true, '\0'))
						{
							text5 = Regex.Escape(text5.Replace("splitsymbol", ","));
							list.Add(text5);
						}
					}
				}
			}
		}
		if (dictionary[chatRuleCategory].Count > 0)
		{
			this.chatRuleCategories.Add(chatRuleCategory);
		}
		else
		{
			dictionary.Remove(chatRuleCategory);
		}
		foreach (KeyValuePair<ChatIRC.ChatRuleCategory, List<string>> keyValuePair in dictionary)
		{
			ChatIRC.ChatRuleCategory key = keyValuePair.Key;
			List<string> value = keyValuePair.Value;
			StringBuilder stringBuilder = new StringBuilder("");
			for (int i = 0; i < value.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("|");
				}
				stringBuilder.Append(value[i]);
			}
			stringBuilder.Append("");
			this.chatRulePatterns[key] = new Regex(stringBuilder.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled, new TimeSpan(0, 0, 1));
		}
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x000EA314 File Offset: 0x000E8514
	public string FilterChatMessage(string message, out bool shouldHide)
	{
		MatchEvaluator evaluator = new MatchEvaluator(ChatIRC.GrawlixIt);
		char c = '\0';
		int num = 0;
		for (int i = 0; i < message.Length; i++)
		{
			if (message[i] != ' ')
			{
				if (c == '\0')
				{
					c = message[i];
				}
				if (message[i] != c)
				{
					break;
				}
				num++;
				if (num >= 4)
				{
					shouldHide = true;
					return message;
				}
			}
		}
		shouldHide = false;
		for (int j = 0; j < this.chatRuleCategories.Count; j++)
		{
			ChatIRC.ChatRuleCategory chatRuleCategory = this.chatRuleCategories[j];
			try
			{
				if (this.chatRulePatterns[chatRuleCategory].IsMatch(message))
				{
					if (chatRuleCategory.action == "hide")
					{
						shouldHide = false;
					}
					else if (Singleton<ChatSettings>.Instance.FilterChatMessages)
					{
						message = this.chatRulePatterns[chatRuleCategory].Replace(message, evaluator);
					}
				}
			}
			catch (RegexMatchTimeoutException)
			{
				shouldHide = true;
				return message;
			}
		}
		return message;
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x000EA40C File Offset: 0x000E860C
	private static string GrawlixIt(Match match)
	{
		List<string> list = new List<string>
		{
			"@",
			"#",
			"$",
			"%",
			"&"
		};
		StringBuilder stringBuilder = new StringBuilder();
		int hashCode = match.ToString().GetHashCode();
		for (int i = 0; i < match.Length; i++)
		{
			int num = i % list.Count;
			if (num == 0)
			{
				Utilities.Shuffle<string>(list, new int?(hashCode + i));
			}
			stringBuilder.Append(list[num]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x040013C8 RID: 5064
	public static bool ENABLE_IRC_SEND = true;

	// Token: 0x040013C9 RID: 5065
	private string serverName = "irc.geekshed.net";

	// Token: 0x040013CA RID: 5066
	private string userName = "table";

	// Token: 0x040013CB RID: 5067
	private string channel = "#tabletopsimulator";

	// Token: 0x040013CC RID: 5068
	private int port = 6667;

	// Token: 0x040013CD RID: 5069
	private const string wordlistURL = "https://raw.githubusercontent.com/Berserk-Games/Tabletop-Simulator-Bad-Word-List/master/wordlist.sh";

	// Token: 0x040013CE RID: 5070
	private const string wordlistPlayerPref = "LastDownloadOfChatFilter";

	// Token: 0x040013CF RID: 5071
	private Dictionary<ChatIRC.ChatRuleCategory, Regex> chatRulePatterns = new Dictionary<ChatIRC.ChatRuleCategory, Regex>();

	// Token: 0x040013D0 RID: 5072
	private List<ChatIRC.ChatRuleCategory> chatRuleCategories = new List<ChatIRC.ChatRuleCategory>();

	// Token: 0x040013D1 RID: 5073
	private ChatIRC.ChatRuleCategory FloodCategory = new ChatIRC.ChatRuleCategory("flooding", "hide");

	// Token: 0x040013D2 RID: 5074
	private DateTime lastSingleCharMessage = new DateTime(0L);

	// Token: 0x040013D3 RID: 5075
	public IrcClient irc;

	// Token: 0x040013D4 RID: 5076
	private static bool bHasSpawned = false;

	// Token: 0x040013D5 RID: 5077
	private static bool showNames;

	// Token: 0x040013D6 RID: 5078
	public bool allowMessages;

	// Token: 0x040013D7 RID: 5079
	private bool connecting;

	// Token: 0x040013D8 RID: 5080
	private readonly List<string> Bots = new List<string>
	{
		"Jolt",
		"Twitter",
		"Berserk",
		"Berserk`"
	};

	// Token: 0x02000703 RID: 1795
	private struct ChatRuleCategory
	{
		// Token: 0x06003D5F RID: 15711 RVA: 0x0017B91A File Offset: 0x00179B1A
		public ChatRuleCategory(string reason, string action)
		{
			this.reason = reason;
			this.action = action;
		}

		// Token: 0x04002A70 RID: 10864
		public string reason;

		// Token: 0x04002A71 RID: 10865
		public string action;
	}

	// Token: 0x02000704 RID: 1796
	public enum IRCMessageType
	{
		// Token: 0x04002A73 RID: 10867
		Normal,
		// Token: 0x04002A74 RID: 10868
		Moderator,
		// Token: 0x04002A75 RID: 10869
		Developer,
		// Token: 0x04002A76 RID: 10870
		Bot,
		// Token: 0x04002A77 RID: 10871
		Broadcast
	}
}
