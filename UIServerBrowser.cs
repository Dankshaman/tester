using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class UIServerBrowser : Singleton<UIServerBrowser>
{
	// Token: 0x060026EC RID: 9964 RVA: 0x00114934 File Offset: 0x00112B34
	protected override void Awake()
	{
		base.Awake();
		this.browserGrid = NetworkSingleton<NetworkUI>.Instance.GUIBrowserGrid.transform;
		this.tweenRotation = this.RefreshIcon.GetComponent<TweenRotation>();
		this.scrollbar = NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.transform.Find("Background/Scroll Bar").GetComponent<UIScrollBar>();
		this.scrollview = NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.transform.Find("Background/Scroll View").GetComponent<UIScrollView>();
		this.searchInput = this.SearchScript.GetComponent<UIInput>();
		this.SearchScript.enabled = false;
		base.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("ServerBrowserX", base.transform.localPosition.x), PlayerPrefs.GetFloat("ServerBrowserY", base.transform.localPosition.y), base.transform.localPosition.z);
		base.transform.GetChild(0).GetComponent<UISprite>().width = PlayerPrefs.GetInt("ServerBrowserWidth", 824);
		base.transform.GetChild(0).GetComponent<UISprite>().height = PlayerPrefs.GetInt("ServerBrowserHeight", 616);
		this.AddButton();
		this.buttonHeight = this.browserGrid.transform.GetChild(0).GetComponent<UISprite>().height;
		this.visibleButtonCount = 1;
		this.scrollview.verticalScrollBar = null;
		EventManager.OnLanguageChange += this.UpdatePlayerCountLabel;
		EventDelegate.Add(this.searchInput.onChange, new EventDelegate.Callback(this.OnChange));
		EventDelegate.Add(this.scrollbar.onChange, new EventDelegate.Callback(this.OnScroll));
		EventDelegate.Add(this.HideLocked.onChange, new EventDelegate.Callback(this.OnToggleChange));
		EventDelegate.Add(this.FriendServers.onChange, new EventDelegate.Callback(this.OnToggleChange));
	}

	// Token: 0x060026ED RID: 9965 RVA: 0x00114B30 File Offset: 0x00112D30
	private void OnDestroy()
	{
		EventManager.OnLanguageChange -= this.UpdatePlayerCountLabel;
		EventDelegate.Remove(this.searchInput.onChange, new EventDelegate.Callback(this.OnChange));
		EventDelegate.Remove(this.scrollbar.onChange, new EventDelegate.Callback(this.OnScroll));
		EventDelegate.Remove(this.HideLocked.onChange, new EventDelegate.Callback(this.OnToggleChange));
		EventDelegate.Remove(this.FriendServers.onChange, new EventDelegate.Callback(this.OnToggleChange));
	}

	// Token: 0x060026EE RID: 9966 RVA: 0x00114BC2 File Offset: 0x00112DC2
	private void OnEnable()
	{
		this.ChatFilter.value = Singleton<ChatSettings>.Instance.FilterChatMessages;
		this.searchInput.value = PlayerPrefs.GetString("ServerBrowserSearch", "");
		this.StartRefresh();
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x00114BFC File Offset: 0x00112DFC
	private void OnDisable()
	{
		this.ServerSort = UIServerBrowser.ServerSortOptions.None;
		PlayerPrefs.SetInt("ServerBrowserWidth", base.transform.GetChild(0).GetComponent<UISprite>().width);
		PlayerPrefs.SetInt("ServerBrowserHeight", base.transform.GetChild(0).GetComponent<UISprite>().height);
		PlayerPrefs.SetFloat("ServerBrowserX", base.transform.localPosition.x);
		PlayerPrefs.SetFloat("ServerBrowserY", base.transform.localPosition.y);
		this.lobbyHelper = new UIServerBrowser.LobbyHelper();
		this.StopRefresh();
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x00114C96 File Offset: 0x00112E96
	private void OnChange()
	{
		PlayerPrefs.SetString("ServerBrowserSearch", this.searchInput.value);
		this.CalculateFoundServers();
		this.UpdateServerButtons();
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x00114CB9 File Offset: 0x00112EB9
	private void OnScroll()
	{
		this.currentFirstRow = (int)(this.scrollbar.value * (float)Mathf.Max(0, 1 + this.foundServers.Count - this.visibleButtonCount));
		this.UpdateServerButtons();
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x00114CEF File Offset: 0x00112EEF
	public void DoScroll(float delta)
	{
		this.scrollbar.value += delta * this.scrollbar.barSize;
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x00114D10 File Offset: 0x00112F10
	private void Update()
	{
		int num = (int)this.browserGrid.transform.parent.GetComponent<UIPanel>().width;
		int num2 = (int)this.browserGrid.transform.parent.GetComponent<UIPanel>().height;
		if (num != this.lastWidth)
		{
			this.lastWidth = num;
			for (int i = 0; i < this.browserGrid.childCount; i++)
			{
				this.browserGrid.GetChild(i).GetComponent<UISprite>().width = num;
			}
		}
		if (num2 != this.lastHeight)
		{
			this.lastHeight = num2;
			int childCount = this.browserGrid.transform.childCount;
			this.visibleButtonCount = num2 / this.buttonHeight + 1;
			for (int j = childCount; j < this.visibleButtonCount; j++)
			{
				this.AddButton();
			}
			this.CalculateFoundServers();
			this.UpdateServerButtons();
		}
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x00114DE4 File Offset: 0x00112FE4
	private void UpdateServerButtons()
	{
		for (int i = 0; i < this.browserGrid.transform.childCount; i++)
		{
			int num = i + this.currentFirstRow;
			if (num < this.foundServers.Count && i < this.visibleButtonCount)
			{
				this.SetButton(i, this.foundServers[num]);
			}
			else
			{
				this.DisableButton(i);
			}
		}
		this.scrollview.ResetPosition();
		this.browserGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x00114E64 File Offset: 0x00113064
	public void ToggleRefresh()
	{
		if (this.refreshDone)
		{
			this.StartRefresh();
			return;
		}
		this.StopRefresh();
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x00114E7C File Offset: 0x0011307C
	public void StartRefresh()
	{
		UIServerSort.ReverseSort = false;
		base.StartCoroutine(this.UpdatePlayerCount());
		this.refreshDone = false;
		this.lobbyHelper = new UIServerBrowser.LobbyHelper();
		this.tweenRotation.enabled = true;
		Singleton<SteamLobbyManager>.Instance.RequestLobbyList(new Action<List<LobbyInfo>, bool>(this.RequestLobbyCallback));
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x00114ED0 File Offset: 0x001130D0
	public void StopRefresh()
	{
		Singleton<SteamLobbyManager>.Instance.StopRequestLobbyList();
		this.SortServerList();
		this.CalculateFoundServers();
		this.UpdateServerButtons();
		this.tweenRotation.enabled = false;
		this.RefreshIcon.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		this.refreshDone = true;
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x00114F30 File Offset: 0x00113130
	private IEnumerator UpdatePlayerCount()
	{
		DataRequest dataRequest = DataRequest.Get("http://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v0001/?format=json&appid=286160", null);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		if (!dataRequest.isError)
		{
			int valueFromJson = Json.GetValueFromJson<int>(Encoding.UTF8.GetString(dataRequest.data), "player_count", Json.SearchType.Exact);
			this.PlayerCount.text = valueFromJson + " " + Language.Translate("In-Game");
		}
		dataRequest.Dispose();
		yield break;
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x00114F3F File Offset: 0x0011313F
	private void UpdatePlayerCountLabel(string oldCode, string newCode)
	{
		this.PlayerCount.text = this.playerCount + " " + Language.Translate("In-Game");
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x00114F6C File Offset: 0x0011316C
	private void RequestLobbyCallback(List<LobbyInfo> lobbyInfos, bool refreshDone)
	{
		if (!GlobalBanList.IsBanned(SteamManager.SteamName, SteamManager.StringSteamID, SystemInfo.deviceUniqueIdentifier))
		{
			int count = this.lobbyHelper.LobbyInfoList.Count;
			for (int i = 0; i < lobbyInfos.Count; i++)
			{
				LobbyInfo lobbyInfo = lobbyInfos[i];
				if (lobbyInfo.MaxPlayers != 0)
				{
					if (Singleton<BlockList>.Instance.Contains(lobbyInfo.OwnerID.ToString()))
					{
						this.lobbyHelper.BlockedCount++;
					}
					else
					{
						UIServerBrowser.TTSLobbyInfo ttslobbyInfo = new UIServerBrowser.TTSLobbyInfo(lobbyInfo, this.lobbyHelper.LobbyInfoList.Count, this.lobbyHelper.FriendLobbies.Contains(lobbyInfo.LobbyID), ServerOptions.GetNetworkComment(lobbyInfo.Comment).LookingForPlayers);
						this.lobbyHelper.LobbyInfoList.Add(ttslobbyInfo);
						this.lobbyHelper.PlayerCount += ttslobbyInfo.CurrentPlayers;
						this.lobbyHelper.ClientCount += ttslobbyInfo.CurrentPlayers - 1;
						if (ttslobbyInfo.Passworded)
						{
							this.lobbyHelper.LockedCount++;
						}
					}
				}
			}
			this.SortServerList();
			this.CalculateFoundServers();
			this.UpdateServerButtons();
		}
		if (refreshDone)
		{
			if (!this.refreshDone && this.lobbyHelper.LobbyInfoList.Count == 0)
			{
				Chat.LogError("Failed to find any servers. (Steam might be down)", true);
			}
			this.tweenRotation.enabled = false;
			this.RefreshIcon.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			this.refreshDone = true;
			if (this.lobbyHelper.BlockedCount > 0)
			{
				Chat.Log(this.lobbyHelper.BlockedCount + " blocked server(s). Blocked players can be found in the configuration menu.", ChatMessageType.All);
			}
		}
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x0011513F File Offset: 0x0011333F
	public void SetSort(UIServerBrowser.ServerSortOptions sortType, bool reversed)
	{
		UIServerSort.ReverseSort = reversed;
		this.ServerSort = sortType;
		this.SortServerList();
		this.CalculateFoundServers();
		this.UpdateServerButtons();
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x00115160 File Offset: 0x00113360
	private void SortServerList()
	{
		switch (this.ServerSort)
		{
		case UIServerBrowser.ServerSortOptions.Lock:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareLocked), UIServerSort.ReverseSort);
			return;
		case UIServerBrowser.ServerSortOptions.Server:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareServerName), UIServerSort.ReverseSort);
			return;
		case UIServerBrowser.ServerSortOptions.Game:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareGameName), UIServerSort.ReverseSort);
			return;
		case UIServerBrowser.ServerSortOptions.Host:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareHostName), UIServerSort.ReverseSort);
			return;
		case UIServerBrowser.ServerSortOptions.Player:
			if (UIServerSort.ReverseSort)
			{
				Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.ComparePlayerCountForwards), false);
				return;
			}
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.ComparePlayerCountBackwards), false);
			return;
		case UIServerBrowser.ServerSortOptions.Region:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareRegion), UIServerSort.ReverseSort);
			return;
		case UIServerBrowser.ServerSortOptions.Looking:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareLookingForPlayers), UIServerSort.ReverseSort);
			return;
		default:
			Utilities.StableSort<UIServerBrowser.TTSLobbyInfo>(this.lobbyHelper.LobbyInfoList, new Comparison<UIServerBrowser.TTSLobbyInfo>(UIServerSort.CompareDefault), UIServerSort.ReverseSort);
			return;
		}
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x001152CC File Offset: 0x001134CC
	private void CalculateFoundServers()
	{
		if (this.lobbyHelper == null)
		{
			return;
		}
		this.foundServers.Clear();
		bool flag = false;
		bool flag2 = false;
		string text = "";
		if (this.searchInput.value != "")
		{
			text = LibString.NormalizedString(this.searchInput.value, true);
			flag = (text != "");
			flag2 = text.Contains("[");
			if (text != this.lastSearch)
			{
				this.lastSearch = text;
				this.searchWordLists.Clear();
				for (string text2 = LibString.bite(ref text, ','); text2 != null; text2 = LibString.bite(ref text, ','))
				{
					List<string> list = new List<string>();
					for (string item = LibString.bite(ref text2, false, ' ', false, false, '\0'); item != null; item = LibString.bite(ref text2, false, ' ', false, false, '\0'))
					{
						list.Add(item);
					}
					if (list.Count > 0)
					{
						this.searchWordLists.Add(list);
					}
				}
			}
		}
		for (int i = 0; i < this.lobbyHelper.LobbyInfoList.Count; i++)
		{
			UIServerBrowser.TTSLobbyInfo ttslobbyInfo = this.lobbyHelper.LobbyInfoList[i];
			if ((!this.HideFull.value || ttslobbyInfo.CurrentPlayers < ttslobbyInfo.MaxPlayers) && (!this.HideLocked.value || !ttslobbyInfo.Passworded) && (!this.FriendServers.value || ttslobbyInfo.IsFriend))
			{
				bool flag3 = true;
				if (flag)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(ttslobbyInfo.Server);
					stringBuilder.AppendLine(ttslobbyInfo.Game);
					stringBuilder.AppendLine(ttslobbyInfo.Host);
					if (flag2)
					{
						stringBuilder.Append("[");
						stringBuilder.Append(ttslobbyInfo.Country);
						stringBuilder.AppendLine("]");
					}
					string text3 = LibString.NormalizedString(stringBuilder.ToString(), true);
					flag3 = false;
					for (int j = 0; j < this.searchWordLists.Count; j++)
					{
						int num = 0;
						bool flag4 = true;
						bool flag5 = false;
						for (int k = 0; k < this.searchWordLists[j].Count; k++)
						{
							string text4 = this.searchWordLists[j][k];
							if (text4.StartsWith("-"))
							{
								num++;
								text4 = text4.Substring(1);
								if (text4 != "" && text3.Contains(text4))
								{
									flag5 = true;
									flag4 = false;
								}
							}
							else if (!text3.Contains(text4))
							{
								flag4 = false;
							}
						}
						if (num == this.searchWordLists[j].Count)
						{
							if (flag5)
							{
								flag3 = false;
								break;
							}
						}
						else if (flag4)
						{
							flag3 = true;
						}
					}
				}
				if (flag3)
				{
					this.foundServers.Add(i);
				}
			}
		}
		this.ServerCount.text = string.Format("{0} / {1}", this.foundServers.Count, this.lobbyHelper.LobbyInfoList.Count);
		this.scrollbar.barSize = 1f / (float)Mathf.Max(1, 1 + this.foundServers.Count / this.visibleButtonCount);
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x0011560F File Offset: 0x0011380F
	public void OnToggleChange()
	{
		this.CalculateFoundServers();
		this.UpdateServerButtons();
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x00115620 File Offset: 0x00113820
	public void Connect(int lobbyIndex = -1)
	{
		if (lobbyIndex < 0)
		{
			lobbyIndex = this.selectedLobby;
		}
		if (lobbyIndex < 0 || lobbyIndex >= this.lobbyHelper.LobbyInfoList.Count)
		{
			return;
		}
		UIServerBrowser.TTSLobbyInfo ttslobbyInfo = this.lobbyHelper.LobbyInfoList[lobbyIndex];
		Singleton<SteamLobbyManager>.Instance.JoinLobby(ttslobbyInfo.LobbyID);
		NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.SetActive(false);
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x00115683 File Offset: 0x00113883
	public void Select(int lobbyIndex)
	{
		this.selectedLobby = lobbyIndex;
		this.UpdateServerButtons();
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x00115694 File Offset: 0x00113894
	private void AddButton()
	{
		GameObject gameObject = this.browserGrid.gameObject.AddChild(NetworkSingleton<NetworkUI>.Instance.GUIButtonServer);
		gameObject.transform.name = string.Format("{0:D6}", this.browserGrid.transform.childCount);
		gameObject.GetComponent<UISprite>().width = this.lastWidth;
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x001156F5 File Offset: 0x001138F5
	private void DisableButton(int id)
	{
		this.browserGrid.transform.GetChild(id).gameObject.SetActive(false);
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x00115714 File Offset: 0x00113914
	private void SetButton(int id, int lobbyIndex)
	{
		UIServerBrowser.TTSLobbyInfo ttslobbyInfo = this.lobbyHelper.LobbyInfoList[lobbyIndex];
		Transform child = this.browserGrid.transform.GetChild(id);
		child.gameObject.SetActive(true);
		UIServerButton component = child.GetChild(0).GetComponent<UIServerButton>();
		component.LobbyIndex = lobbyIndex;
		component.bPassworded = ttslobbyInfo.Passworded;
		component.ServerName = ttslobbyInfo.Server;
		component.Game = ttslobbyInfo.Game;
		component.Host = ttslobbyInfo.Host;
		component.Region = ttslobbyInfo.Country;
		component.SteamID = ttslobbyInfo.OwnerID.ToString();
		component.LobbyID = ttslobbyInfo.LobbyID.ToString();
		component.CurrentPlayers = ttslobbyInfo.CurrentPlayers;
		component.MaxPlayers = ttslobbyInfo.MaxPlayers;
		component.bFull = (ttslobbyInfo.CurrentPlayers >= ttslobbyInfo.MaxPlayers);
		component.bIsFriend = ttslobbyInfo.IsFriend;
		component.bLookingForPlayers = ttslobbyInfo.LookingForPlayers;
		component.bSelected = (component.LobbyIndex == this.selectedLobby);
		component.UpdateButton();
	}

	// Token: 0x04001965 RID: 6501
	public static float SCROLL_WHEEL_FACTOR = -1f;

	// Token: 0x04001966 RID: 6502
	public UIToggle HideLocked;

	// Token: 0x04001967 RID: 6503
	public UIToggle HideFull;

	// Token: 0x04001968 RID: 6504
	public UIToggle AllServers;

	// Token: 0x04001969 RID: 6505
	public UIToggle FriendServers;

	// Token: 0x0400196A RID: 6506
	public UIToggle ChatFilter;

	// Token: 0x0400196B RID: 6507
	public UISearch SearchScript;

	// Token: 0x0400196C RID: 6508
	public GameObject RefreshIcon;

	// Token: 0x0400196D RID: 6509
	public UILabel ServerCount;

	// Token: 0x0400196E RID: 6510
	public UILabel PlayerCount;

	// Token: 0x0400196F RID: 6511
	private UIScrollBar scrollbar;

	// Token: 0x04001970 RID: 6512
	private UIScrollView scrollview;

	// Token: 0x04001971 RID: 6513
	private UIInput searchInput;

	// Token: 0x04001972 RID: 6514
	private Transform browserGrid;

	// Token: 0x04001973 RID: 6515
	private TweenRotation tweenRotation;

	// Token: 0x04001974 RID: 6516
	private UIServerBrowser.LobbyHelper lobbyHelper;

	// Token: 0x04001975 RID: 6517
	private bool refreshDone = true;

	// Token: 0x04001976 RID: 6518
	private int buttonHeight;

	// Token: 0x04001977 RID: 6519
	private int visibleButtonCount;

	// Token: 0x04001978 RID: 6520
	private int currentFirstRow;

	// Token: 0x04001979 RID: 6521
	private int lastWidth;

	// Token: 0x0400197A RID: 6522
	private int lastHeight;

	// Token: 0x0400197B RID: 6523
	private List<int> foundServers = new List<int>();

	// Token: 0x0400197C RID: 6524
	private int selectedLobby = -1;

	// Token: 0x0400197D RID: 6525
	private int playerCount;

	// Token: 0x0400197E RID: 6526
	public UIServerBrowser.ServerSortOptions ServerSort = UIServerBrowser.ServerSortOptions.None;

	// Token: 0x0400197F RID: 6527
	private const string urlPlayerCount = "http://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v0001/?format=json&appid=286160";

	// Token: 0x04001980 RID: 6528
	private List<List<string>> searchWordLists = new List<List<string>>();

	// Token: 0x04001981 RID: 6529
	private string lastSearch = "";

	// Token: 0x02000784 RID: 1924
	public enum ServerSortOptions
	{
		// Token: 0x04002C7C RID: 11388
		Lock,
		// Token: 0x04002C7D RID: 11389
		Server,
		// Token: 0x04002C7E RID: 11390
		Game,
		// Token: 0x04002C7F RID: 11391
		Host,
		// Token: 0x04002C80 RID: 11392
		Player,
		// Token: 0x04002C81 RID: 11393
		Region,
		// Token: 0x04002C82 RID: 11394
		Looking,
		// Token: 0x04002C83 RID: 11395
		None = 99
	}

	// Token: 0x02000785 RID: 1925
	private class LobbyHelper
	{
		// Token: 0x04002C84 RID: 11396
		public List<CSteamID> FriendLobbies = Singleton<SteamManager>.Instance.GetFriendLobbies();

		// Token: 0x04002C85 RID: 11397
		public List<UIServerBrowser.TTSLobbyInfo> LobbyInfoList = new List<UIServerBrowser.TTSLobbyInfo>();

		// Token: 0x04002C86 RID: 11398
		public int BlockedCount;

		// Token: 0x04002C87 RID: 11399
		public int PlayerCount;

		// Token: 0x04002C88 RID: 11400
		public int ClientCount;

		// Token: 0x04002C89 RID: 11401
		public int LockedCount;
	}

	// Token: 0x02000786 RID: 1926
	public class TTSLobbyInfo : LobbyInfo
	{
		// Token: 0x06003F25 RID: 16165 RVA: 0x00180ED0 File Offset: 0x0017F0D0
		public TTSLobbyInfo(LobbyInfo lobbyInfo, int order, bool isFriend, bool lookingForPlayers)
		{
			this.Server = lobbyInfo.Server;
			this.Game = lobbyInfo.Game;
			this.Host = lobbyInfo.Host;
			this.Version = lobbyInfo.Version;
			this.Comment = lobbyInfo.Comment;
			this.Country = lobbyInfo.Country;
			this.Passworded = lobbyInfo.Passworded;
			this.CurrentPlayers = lobbyInfo.CurrentPlayers;
			this.MaxPlayers = lobbyInfo.MaxPlayers;
			this.LobbyID = lobbyInfo.LobbyID;
			this.OwnerID = lobbyInfo.OwnerID;
			this.Order = order;
			this.IsFriend = isFriend;
			this.LookingForPlayers = lookingForPlayers;
		}

		// Token: 0x04002C8A RID: 11402
		public int Order;

		// Token: 0x04002C8B RID: 11403
		public bool LookingForPlayers;

		// Token: 0x04002C8C RID: 11404
		public bool IsFriend;
	}
}
