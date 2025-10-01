using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using Steamworks;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class SteamLobbyManager : Singleton<SteamLobbyManager>
{
	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06001C70 RID: 7280 RVA: 0x000C3F4D File Offset: 0x000C214D
	// (set) Token: 0x06001C71 RID: 7281 RVA: 0x000C3F55 File Offset: 0x000C2155
	public bool isInLobby { get; private set; }

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06001C72 RID: 7282 RVA: 0x000C3F5E File Offset: 0x000C215E
	// (set) Token: 0x06001C73 RID: 7283 RVA: 0x000C3F66 File Offset: 0x000C2166
	public CSteamID CurrentSteamIDLobby
	{
		get
		{
			return this._CurrentSteamIDLobby;
		}
		private set
		{
			this._CurrentSteamIDLobby = value;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06001C74 RID: 7284 RVA: 0x000C3F6F File Offset: 0x000C216F
	// (set) Token: 0x06001C75 RID: 7285 RVA: 0x000C3F77 File Offset: 0x000C2177
	public CSteamID CurrentSteamIDOwner
	{
		get
		{
			return this._CurrentSteamIDOwner;
		}
		private set
		{
			this._CurrentSteamIDOwner = value;
		}
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000C3F80 File Offset: 0x000C2180
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		this.OnLobbyCreated = CallResult<LobbyCreated_t>.Create(new CallResult<LobbyCreated_t>.APIDispatchDelegate(this.CallResultCreateLobby));
		this.m_LobbydDataUpdate = Callback<LobbyDataUpdate_t>.Create(new Callback<LobbyDataUpdate_t>.DispatchDelegate(this.CallbackUpdateLobby));
		this.OnLobbyEnter = CallResult<LobbyEnter_t>.Create(new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.CallResultEnterLobby));
		this.OnLobbyMatchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(this.CallResultMatchlistLobby));
		this.m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(this.CallbackChatLobby));
		this.m_LobbyInvite = Callback<LobbyInvite_t>.Create(new Callback<LobbyInvite_t>.DispatchDelegate(this.CallbackInviteToLobby));
		this.m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.CallbackJoinRequestLobby));
		this.OnLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(new Callback<LobbyChatMsg_t>.DispatchDelegate(this.CallbackLobbyChatMsg));
		NetworkEvents.OnServerInitializing += this.ServerInitializing;
		NetworkEvents.OnDisconnectedFromServer += this.DisconnectedFromServer;
		NetworkEvents.OnPlayerConnected += this.PlayerConnected;
		NetworkEvents.OnPlayerDisconnected += this.PlayerDisconnected;
		NetworkEvents.OnSettingsChange += this.SettingsChange;
		string b = "+connect_lobby";
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i] == b)
			{
				try
				{
					this.JoinLobby(SteamManager.StringToSteamID(commandLineArgs[i + 1]));
				}
				catch (Exception)
				{
				}
			}
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000C40EC File Offset: 0x000C22EC
	private void OnDestroy()
	{
		NetworkEvents.OnServerInitializing -= this.ServerInitializing;
		NetworkEvents.OnDisconnectedFromServer -= this.DisconnectedFromServer;
		NetworkEvents.OnPlayerConnected -= this.PlayerConnected;
		NetworkEvents.OnPlayerDisconnected -= this.PlayerDisconnected;
		NetworkEvents.OnSettingsChange -= this.SettingsChange;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000C414E File Offset: 0x000C234E
	private void ServerInitializing()
	{
		if (Network.maxConnections > 0)
		{
			this.CreateLobby();
			return;
		}
		Network.ServerInitialized();
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x000C4164 File Offset: 0x000C2364
	private void PlayerConnected(NetworkPlayer player)
	{
		this.SetLobbyInfo();
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000C416C File Offset: 0x000C236C
	private void PlayerDisconnected(NetworkPlayer player, DisconnectInfo info)
	{
		this.SetLobbyInfo();
		this.RemoveFromLobby(player.steamID, SteamLobbyManager.LobbyMessageType.Disconnect, (int)info);
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000C4183 File Offset: 0x000C2383
	private void DisconnectedFromServer(DisconnectInfo info)
	{
		this.LeaveLobby();
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x000C4164 File Offset: 0x000C2364
	private void SettingsChange()
	{
		this.SetLobbyInfo();
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000C418C File Offset: 0x000C238C
	public void RemoveFromLobby(CSteamID steamID, SteamLobbyManager.LobbyMessageType type, int messageIndex)
	{
		if (this.inLobby(steamID))
		{
			if (!this.removedPlayers.Contains(steamID))
			{
				this.removedPlayers.Add(steamID);
			}
			if (this.CurrentSteamIDOwner == SteamUser.GetSteamID())
			{
				byte[] bson = Json.GetBson(new SteamLobbyManager.LobbyMessage(type, steamID.m_SteamID, messageIndex));
				if (!SteamMatchmaking.SendLobbyChatMsg(this.CurrentSteamIDLobby, bson, bson.Length))
				{
					Debug.LogError("Failed to send lobby chat message.");
				}
			}
		}
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000C41FC File Offset: 0x000C23FC
	private void CallbackLobbyChatMsg(LobbyChatMsg_t pCallback)
	{
		Debug.Log(string.Concat(new object[]
		{
			"CallbackLobbyChatMsg ",
			pCallback.m_eChatEntryType,
			" : ",
			pCallback.m_iChatID,
			" : ",
			pCallback.m_ulSteamIDLobby
		}));
		CSteamID y;
		EChatEntryType echatEntryType;
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(new CSteamID(pCallback.m_ulSteamIDLobby), (int)pCallback.m_iChatID, out y, this.chatMsgData, this.chatMsgData.Length, out echatEntryType);
		SteamLobbyManager.LobbyMessage lobbyMessage = Json.Load<SteamLobbyManager.LobbyMessage>(this.chatMsgData, lobbyChatEntry, false);
		if (lobbyMessage == null)
		{
			return;
		}
		if (this.CurrentSteamIDOwner == y)
		{
			CSteamID csteamID = new CSteamID(lobbyMessage.targetSteamID);
			SteamLobbyManager.LobbyMessageType type = lobbyMessage.type;
			if (type != SteamLobbyManager.LobbyMessageType.ConnectFailed)
			{
				if (type != SteamLobbyManager.LobbyMessageType.Disconnect)
				{
					return;
				}
				if (csteamID == SteamUser.GetSteamID())
				{
					Network.ReceiveDisconnect((DisconnectInfo)lobbyMessage.messageIndex);
					return;
				}
				if (!this.removedPlayers.Contains(csteamID))
				{
					this.removedPlayers.Add(csteamID);
				}
				Singleton<SteamP2PManager>.Instance.CloseP2PSession(csteamID);
			}
			else
			{
				if (csteamID == SteamUser.GetSteamID())
				{
					NetworkEvents.TriggerFailedToConnect((ConnectFailedInfo)lobbyMessage.messageIndex);
					Network.ReceiveDisconnect(DisconnectInfo.Failed);
					return;
				}
				if (!this.removedPlayers.Contains(csteamID))
				{
					this.removedPlayers.Add(csteamID);
				}
				Singleton<SteamP2PManager>.Instance.CloseP2PSession(csteamID);
				return;
			}
		}
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x000C4351 File Offset: 0x000C2551
	private ELobbyType GetLobbyType(ServerType serverType)
	{
		switch (serverType)
		{
		case ServerType.Public:
			return ELobbyType.k_ELobbyTypePublic;
		case ServerType.Friends:
			return ELobbyType.k_ELobbyTypeFriendsOnly;
		case ServerType.Invite:
			return ELobbyType.k_ELobbyTypePrivate;
		default:
			return ELobbyType.k_ELobbyTypePublic;
		}
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x000C4370 File Offset: 0x000C2570
	public void SetLobbyType(ServerType serverType)
	{
		if (this.isInLobby && this.CurrentSteamIDOwner == SteamUser.GetSteamID() && !SteamMatchmaking.SetLobbyType(this.CurrentSteamIDLobby, this.GetLobbyType(serverType)))
		{
			Chat.LogError("Error changing server type: " + serverType, true);
		}
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000C43C4 File Offset: 0x000C25C4
	private void CreateLobby()
	{
		SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby(this.GetLobbyType(Network.serverType), 25);
		this.OnLobbyCreated.Set(hAPICall, new CallResult<LobbyCreated_t>.APIDispatchDelegate(this.CallResultCreateLobby));
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x000C43FC File Offset: 0x000C25FC
	private void CallResultCreateLobby(LobbyCreated_t pCallback, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("create lobby", pCallback.m_eResult, Failure, null, null))
		{
			return;
		}
		this.isInLobby = true;
		this.CurrentSteamIDLobby = new CSteamID(pCallback.m_ulSteamIDLobby);
		this.CurrentSteamIDOwner = SteamUser.GetSteamID();
		Debug.Log("CallResultCreateLobby: " + this.CurrentSteamIDLobby);
		this.SetLobbyInfo();
		Network.ServerInitialized();
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000C4468 File Offset: 0x000C2668
	private void SetLobbyInfo()
	{
		if (this.isInLobby && this.CurrentSteamIDOwner == SteamUser.GetSteamID())
		{
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "serverName", Network.serverName);
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "gameName", Network.gameName);
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "hostName", SteamFriends.GetPersonaName());
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "version", Network.version);
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "comment", Network.comment);
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "country", SteamUtils.GetIPCountry());
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "passworded", (!string.IsNullOrEmpty(Network.password)).ToString());
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "currentPlayers", Mathf.Max(1, Network.connections.Count).ToString());
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "maxPlayers", Network.maxConnections.ToString());
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "ownerID", SteamUser.GetSteamID().m_SteamID.ToString());
			SteamMatchmaking.SetLobbyData(this.CurrentSteamIDLobby, "lobbyPage", UnityEngine.Random.Range(0, SteamLobbyManager.LOBBY_PAGES).ToString());
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000C45D0 File Offset: 0x000C27D0
	private LobbyInfo GetLobbyInfo(CSteamID steamIDLobby)
	{
		LobbyInfo result;
		try
		{
			result = new LobbyInfo
			{
				Server = SteamMatchmaking.GetLobbyData(steamIDLobby, "serverName"),
				Game = SteamMatchmaking.GetLobbyData(steamIDLobby, "gameName"),
				Host = SteamMatchmaking.GetLobbyData(steamIDLobby, "hostName"),
				Version = SteamMatchmaking.GetLobbyData(steamIDLobby, "version"),
				Comment = SteamMatchmaking.GetLobbyData(steamIDLobby, "comment"),
				Country = SteamMatchmaking.GetLobbyData(steamIDLobby, "country"),
				Passworded = bool.Parse(SteamMatchmaking.GetLobbyData(steamIDLobby, "passworded")),
				CurrentPlayers = int.Parse(SteamMatchmaking.GetLobbyData(steamIDLobby, "currentPlayers")),
				MaxPlayers = int.Parse(SteamMatchmaking.GetLobbyData(steamIDLobby, "maxPlayers")),
				OwnerID = new CSteamID(ulong.Parse(SteamMatchmaking.GetLobbyData(steamIDLobby, "ownerID"))),
				LobbyID = steamIDLobby
			};
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			result = new LobbyInfo();
		}
		return result;
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000C46D0 File Offset: 0x000C28D0
	private void CallResultEnterLobby(LobbyEnter_t pCallback, bool Failure)
	{
		if (Failure)
		{
			NetworkEvents.TriggerFailedToConnect(ConnectFailedInfo.Steam);
			Network.ReceiveDisconnect(DisconnectInfo.Failed);
			return;
		}
		CSteamID csteamID = new CSteamID(pCallback.m_ulSteamIDLobby);
		if (this.CurrentSteamIDLobby == csteamID)
		{
			Debug.Log("Already in this lobby: " + csteamID);
			return;
		}
		if (Network.peerType != NetworkPeerMode.Disconnected)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Entering a new lobby while in another lobby: ",
				csteamID,
				" old lobby: ",
				this.CurrentSteamIDLobby
			}));
			Network.ReceiveDisconnect(DisconnectInfo.Successful);
		}
		base.StartCoroutine(this.WaitFrameConnect(csteamID));
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x000C4770 File Offset: 0x000C2970
	private IEnumerator WaitFrameConnect(CSteamID steamIDLobby)
	{
		yield return null;
		this.isInLobby = true;
		this.CurrentSteamIDLobby = steamIDLobby;
		Debug.Log("CallbackEnterLobby: " + this.CurrentSteamIDLobby);
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			LobbyInfo lobbyInfo = this.GetLobbyInfo(this.CurrentSteamIDLobby);
			this.CurrentSteamIDOwner = SteamMatchmaking.GetLobbyOwner(steamIDLobby);
			if (lobbyInfo.Passworded)
			{
				if (NetworkSingleton<HostMigrationManager>.Instance.IsMigrating(this.CurrentSteamIDOwner))
				{
					this.Connect(HostMigrationManager.migrationInfo.Password);
					NetworkSingleton<HostMigrationManager>.Instance.ResetMigrating();
				}
				else if (!string.IsNullOrEmpty(Singleton<DiscordController>.Instance.DiscordJoinPassword))
				{
					this.Connect(Singleton<DiscordController>.Instance.DiscordJoinPassword);
					Singleton<DiscordController>.Instance.ResetDiscordJoin();
				}
				else
				{
					UIDialog.ShowInput("Enter Server Password", "Ok", "Cancel", new Action<string>(this.Connect), delegate(string x)
					{
						this.LeaveLobby();
					}, "", "Password");
				}
			}
			else
			{
				this.Connect(null);
			}
			if (!SteamMatchmaking.RequestLobbyData(steamIDLobby))
			{
				Debug.Log("Failed to request lobby data: " + steamIDLobby);
			}
		}
		yield break;
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000C4788 File Offset: 0x000C2988
	private void CallResultMatchlistLobby(LobbyMatchList_t pCallback, bool Failure)
	{
		if (Failure)
		{
			this.StartCoroutineCallbackRequestLobbyInfo();
			Chat.LogError("Failed to request Steam lobbies. (Steam might be down)", true);
			return;
		}
		int nLobbiesMatching = (int)pCallback.m_nLobbiesMatching;
		this.currentNumberLobbies += nLobbiesMatching;
		Debug.Log(string.Concat(new object[]
		{
			"CallResultMatchlistLobby: ",
			this.currentLobbyPage,
			" Count: ",
			nLobbiesMatching
		}));
		if (this.IsLastLobbyPage())
		{
			Debug.Log("Number of Server Lobbies: " + this.currentNumberLobbies);
			this.lastLobbyPage = true;
			if (nLobbiesMatching == 0)
			{
				this.StartCoroutineCallbackRequestLobbyInfo();
			}
		}
		for (int i = 0; i < nLobbiesMatching; i++)
		{
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
			if (!SteamMatchmaking.RequestLobbyData(lobbyByIndex))
			{
				Debug.Log("Failed to request lobby data: " + lobbyByIndex);
			}
		}
		this.RequestLobby();
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x000C4860 File Offset: 0x000C2A60
	private void CallbackUpdateLobby(LobbyDataUpdate_t pCallback)
	{
		if (pCallback.m_bSuccess != 1)
		{
			return;
		}
		CSteamID csteamID = new CSteamID(pCallback.m_ulSteamIDLobby);
		LobbyInfo lobbyInfo = this.GetLobbyInfo(csteamID);
		if (this.CurrentSteamIDLobby == csteamID)
		{
			this.CurrentSteamIDOwner = SteamMatchmaking.GetLobbyOwner(csteamID);
			return;
		}
		if (lobbyInfo.Version != Network.version)
		{
			return;
		}
		if (this.addedLobbyOwners.Contains(lobbyInfo.OwnerID))
		{
			return;
		}
		this.addedLobbyOwners.Add(lobbyInfo.OwnerID);
		this.requestLobbyInfos.Add(lobbyInfo);
		this.StartCoroutineCallbackRequestLobbyInfo();
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x000C48F1 File Offset: 0x000C2AF1
	private void StartCoroutineCallbackRequestLobbyInfo()
	{
		if (this.coroutineCallbackLobby != null)
		{
			base.StopCoroutine(this.coroutineCallbackLobby);
		}
		this.coroutineCallbackLobby = base.StartCoroutine(this.CallbackRequestLobbyInfo());
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x000C4919 File Offset: 0x000C2B19
	private IEnumerator CallbackRequestLobbyInfo()
	{
		yield return null;
		if (this.requestLobbyCallback != null)
		{
			this.requestLobbyCallback(this.requestLobbyInfos, this.lastLobbyPage);
			this.requestLobbyInfos.Clear();
		}
		yield break;
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x000C4928 File Offset: 0x000C2B28
	private void Connect(string password = null)
	{
		if (Network.peerType == NetworkPeerMode.Disconnected && this.isInLobby)
		{
			Network.Connect(this.CurrentSteamIDOwner, password);
		}
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000C4948 File Offset: 0x000C2B48
	private void CallbackChatLobby(LobbyChatUpdate_t pCallback)
	{
		CSteamID csteamID = new CSteamID(pCallback.m_ulSteamIDLobby);
		CSteamID csteamID2 = new CSteamID(pCallback.m_ulSteamIDMakingChange);
		CSteamID csteamID3 = new CSteamID(pCallback.m_ulSteamIDUserChanged);
		Debug.Log(string.Concat(new object[]
		{
			"CallbackChatLobby: ",
			csteamID,
			" Making Change: ",
			csteamID2,
			" UserChanged: ",
			csteamID3,
			" Bit: ",
			pCallback.m_rgfChatMemberStateChange
		}));
		uint rgfChatMemberStateChange = pCallback.m_rgfChatMemberStateChange;
		switch (rgfChatMemberStateChange)
		{
		case 1U:
			if (this.removedPlayers.Contains(csteamID3))
			{
				this.removedPlayers.Remove(csteamID3);
				return;
			}
			return;
		case 2U:
		case 4U:
			break;
		case 3U:
			return;
		default:
			if (rgfChatMemberStateChange != 8U && rgfChatMemberStateChange != 10U)
			{
				return;
			}
			break;
		}
		NetworkPlayer player;
		if (Network.steamPlayers.TryGet(csteamID3, out player))
		{
			if (Network.isServer)
			{
				Network.RemovePlayer(player, DisconnectInfo.DisconnectLobbyServer);
			}
			else if (player.isServer)
			{
				Network.ReceiveDisconnect(DisconnectInfo.DisconnectLobbyClient);
			}
		}
		else
		{
			Singleton<SteamP2PManager>.Instance.CloseP2PSession(csteamID3);
		}
		if (this.removedPlayers.Contains(csteamID3))
		{
			this.removedPlayers.Remove(csteamID3);
		}
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000C4A74 File Offset: 0x000C2C74
	private void CallbackInviteToLobby(LobbyInvite_t pCallback)
	{
		CSteamID csteamID = new CSteamID(pCallback.m_ulSteamIDUser);
		CSteamID lobbyID = new CSteamID(pCallback.m_ulSteamIDLobby);
		if (NetworkSingleton<HostMigrationManager>.Instance.IsMigrating(csteamID))
		{
			UIDialog.Show("Join new Server Host?", "Yes", "No", delegate()
			{
				this.JoinLobby(lobbyID);
			}, null);
			return;
		}
		string friendPersonaName = SteamFriends.GetFriendPersonaName(csteamID);
		Chat.Log("You have been invited to a game by " + friendPersonaName + " (Check Steam Overlay).", Colour.Orange, ChatMessageType.All, true);
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x000C4B00 File Offset: 0x000C2D00
	private void CallbackJoinRequestLobby(GameLobbyJoinRequested_t pCallback)
	{
		SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(pCallback.m_steamIDLobby);
		this.OnLobbyEnter.Set(hAPICall, new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.CallResultEnterLobby));
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x000C4B31 File Offset: 0x000C2D31
	private bool IsLastLobbyPage()
	{
		return this.currentLobbyPage == SteamLobbyManager.LOBBY_PAGES;
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x000C4B40 File Offset: 0x000C2D40
	public void StopRequestLobbyList()
	{
		this.currentLobbyPage = SteamLobbyManager.LOBBY_PAGES;
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x000C4B50 File Offset: 0x000C2D50
	public void RequestLobbyList(Action<List<LobbyInfo>, bool> callback)
	{
		if (!SteamManager.Initialized)
		{
			Chat.LogError("Not connected to Steam.", true);
			return;
		}
		this.requestLobbyInfos.Clear();
		this.addedLobbyOwners.Clear();
		this.requestLobbyCallback = callback;
		this.currentLobbyPage = 0;
		this.currentNumberLobbies = 0;
		this.lastLobbyPage = false;
		foreach (CSteamID csteamID in Singleton<SteamManager>.Instance.GetFriendLobbies())
		{
			if (!SteamMatchmaking.RequestLobbyData(csteamID))
			{
				Debug.Log("Failed to request lobby data: " + csteamID);
			}
		}
		this.RequestLobby();
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x000C4C08 File Offset: 0x000C2E08
	private void RequestLobby()
	{
		if (this.currentLobbyPage < SteamLobbyManager.LOBBY_PAGES)
		{
			SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
			SteamMatchmaking.AddRequestLobbyListStringFilter("version", Network.version, ELobbyComparison.k_ELobbyComparisonEqual);
			SteamMatchmaking.AddRequestLobbyListNumericalFilter("lobbyPage", this.currentLobbyPage, ELobbyComparison.k_ELobbyComparisonEqual);
			SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
			this.OnLobbyMatchList.Set(hAPICall, new CallResult<LobbyMatchList_t>.APIDispatchDelegate(this.CallResultMatchlistLobby));
			this.currentLobbyPage++;
		}
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x000C4C75 File Offset: 0x000C2E75
	public void InviteToLobby(CSteamID steamID)
	{
		if (!this.isInLobby)
		{
			return;
		}
		if (!SteamMatchmaking.InviteUserToLobby(this.CurrentSteamIDLobby, steamID))
		{
			Chat.LogError("Failed to invite player to Steam lobby.", true);
		}
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x000C4C9C File Offset: 0x000C2E9C
	public void JoinLobby(CSteamID steamIDLobby)
	{
		if (this.CurrentSteamIDLobby != steamIDLobby)
		{
			SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(steamIDLobby);
			this.OnLobbyEnter.Set(hAPICall, new CallResult<LobbyEnter_t>.APIDispatchDelegate(this.CallResultEnterLobby));
		}
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x000C4CD6 File Offset: 0x000C2ED6
	public void OpenInviteOverlay()
	{
		if (this.isInLobby)
		{
			SteamFriends.ActivateGameOverlayInviteDialog(this.CurrentSteamIDLobby);
		}
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x000C4CEB File Offset: 0x000C2EEB
	public bool InCurrentLobby(CSteamID steamID)
	{
		return !this.removedPlayers.Contains(steamID) && this.inLobby(steamID);
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x000C4D04 File Offset: 0x000C2F04
	private bool inLobby(CSteamID steamID)
	{
		if (!this.isInLobby)
		{
			return false;
		}
		int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(this.CurrentSteamIDLobby);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			if (SteamMatchmaking.GetLobbyMemberByIndex(this.CurrentSteamIDLobby, i) == steamID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x000C4D4C File Offset: 0x000C2F4C
	private void LeaveLobby()
	{
		if (this.isInLobby)
		{
			Debug.Log("Leave lobby");
			SteamMatchmaking.LeaveLobby(this.CurrentSteamIDLobby);
			this.CurrentSteamIDLobby = default(CSteamID);
			this.CurrentSteamIDOwner = default(CSteamID);
			this.isInLobby = false;
			this.removedPlayers.Clear();
		}
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x000C4183 File Offset: 0x000C2383
	private void OnApplicationQuit()
	{
		this.LeaveLobby();
	}

	// Token: 0x04001232 RID: 4658
	private CallResult<LobbyCreated_t> OnLobbyCreated;

	// Token: 0x04001233 RID: 4659
	private Callback<LobbyDataUpdate_t> m_LobbydDataUpdate;

	// Token: 0x04001234 RID: 4660
	private CallResult<LobbyEnter_t> OnLobbyEnter;

	// Token: 0x04001235 RID: 4661
	private CallResult<LobbyMatchList_t> OnLobbyMatchList;

	// Token: 0x04001236 RID: 4662
	private Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;

	// Token: 0x04001237 RID: 4663
	private Callback<LobbyInvite_t> m_LobbyInvite;

	// Token: 0x04001238 RID: 4664
	private Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;

	// Token: 0x04001239 RID: 4665
	private Callback<LobbyChatMsg_t> OnLobbyChatMsg;

	// Token: 0x0400123B RID: 4667
	private CSteamID _CurrentSteamIDLobby;

	// Token: 0x0400123C RID: 4668
	private CSteamID _CurrentSteamIDOwner;

	// Token: 0x0400123D RID: 4669
	private List<CSteamID> removedPlayers = new List<CSteamID>();

	// Token: 0x0400123E RID: 4670
	private byte[] chatMsgData = new byte[4000];

	// Token: 0x0400123F RID: 4671
	private Coroutine coroutineCallbackLobby;

	// Token: 0x04001240 RID: 4672
	public static int LOBBY_PAGES = 30;

	// Token: 0x04001241 RID: 4673
	private int currentLobbyPage;

	// Token: 0x04001242 RID: 4674
	private int currentNumberLobbies;

	// Token: 0x04001243 RID: 4675
	private bool lastLobbyPage;

	// Token: 0x04001244 RID: 4676
	private Action<List<LobbyInfo>, bool> requestLobbyCallback;

	// Token: 0x04001245 RID: 4677
	private List<LobbyInfo> requestLobbyInfos = new List<LobbyInfo>();

	// Token: 0x04001246 RID: 4678
	private List<CSteamID> addedLobbyOwners = new List<CSteamID>();

	// Token: 0x020006C5 RID: 1733
	public enum LobbyMessageType
	{
		// Token: 0x04002940 RID: 10560
		ConnectFailed,
		// Token: 0x04002941 RID: 10561
		Disconnect
	}

	// Token: 0x020006C6 RID: 1734
	private class LobbyMessage
	{
		// Token: 0x06003C82 RID: 15490 RVA: 0x00179687 File Offset: 0x00177887
		public LobbyMessage(SteamLobbyManager.LobbyMessageType type, ulong targetSteamID, int messageIndex)
		{
			this.type = type;
			this.targetSteamID = targetSteamID;
			this.messageIndex = messageIndex;
		}

		// Token: 0x04002942 RID: 10562
		public SteamLobbyManager.LobbyMessageType type;

		// Token: 0x04002943 RID: 10563
		public ulong targetSteamID;

		// Token: 0x04002944 RID: 10564
		public int messageIndex;
	}
}
