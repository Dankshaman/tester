using System;
using System.Collections.Generic;
using NewNet;
using Steamworks;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class PlayerManager : NetworkSingleton<PlayerManager>
{
	// Token: 0x0600189A RID: 6298 RVA: 0x000A67AC File Offset: 0x000A49AC
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnLoadingComplete += this.UpdateOrder;
		EventManager.OnLoadingChange += this.OnLoadingChange;
		EventManager.OnUIThemeChange += this.OnUIThemeChange;
		EventManager.OnChatTyping += this.OnChatTyping;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x000A6804 File Offset: 0x000A4A04
	private void OnDestroy()
	{
		EventManager.OnLoadingComplete -= this.UpdateOrder;
		EventManager.OnLoadingChange -= this.OnLoadingChange;
		EventManager.OnUIThemeChange -= this.OnUIThemeChange;
		EventManager.OnChatTyping -= this.OnChatTyping;
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x000A6858 File Offset: 0x000A4A58
	private void OnUIThemeChange()
	{
		PlayerState playerState = this.MyPlayerState();
		if (playerState != null)
		{
			playerState.UpdateUIColour();
		}
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x000A6878 File Offset: 0x000A4A78
	private void Update()
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			int id = this.PlayersList[i].id;
			this.PlayersList[i].ui.bTurn = (NetworkSingleton<Turns>.Instance.turnsState.TurnColor == this.PlayersList[i].stringColor);
		}
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000A68E8 File Offset: 0x000A4AE8
	private void Add(PlayerState playerState)
	{
		this.PlayersDictionary.Add(playerState.id, playerState);
		this.PlayersList.Add(playerState);
		this.PlayersUpdate();
		EventManager.TriggerPlayersAdd(playerState);
		string steamId = playerState.steamId;
		if (SteamManager.bSteam && steamId != SteamManager.StringSteamID)
		{
			Singleton<SteamManager>.Instance.SetPlayedWith(steamId);
			if (Singleton<SteamManager>.Instance.IsFriend(steamId))
			{
				Achievements.Set("ACH_PLAY_WITH_STEAM_FRIEND");
			}
			if (Singleton<BlockList>.Instance.Contains(steamId))
			{
				Chat.LogError(playerState.name + " is on your block list. Blocked players can be found in the configuration menu.", true);
			}
		}
		if (this.PlayersList.Count >= 8)
		{
			Achievements.Set("ACH_FULL_GAME_OF_8_PLAYERS");
		}
		Singleton<SteamManager>.Instance.GetAvatarFromSteamID(playerState.cSteamId, new Action<CSteamID, Texture2D>(this.SpawnPlayerPotrait));
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000A69B4 File Offset: 0x000A4BB4
	private void Remove(PlayerState playerState)
	{
		EventManager.TriggerPlayersRemove(playerState);
		this.PlayersDictionary.Remove(playerState.id);
		this.PlayersList.Remove(playerState);
		UnityEngine.Object.Destroy(playerState.ui.gameObject);
		Transform transform = this.PlayerUIGrid.transform.Find("Drop-down List");
		if (transform)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		this.PlayersUpdate();
		UnityEngine.Object.Destroy(playerState.portrait);
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000A6A30 File Offset: 0x000A4C30
	public void PlayersUpdate()
	{
		this.UpdateOrder();
		EventManager.TriggerPlayersUpdate();
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000A6A40 File Offset: 0x000A4C40
	public void UpdateOrder()
	{
		List<string> turnOrder = NetworkSingleton<Turns>.Instance.GetTurnOrder();
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			string stringColor = this.PlayersList[i].stringColor;
			Colour colour = Colour.ColourFromLabel(stringColor);
			int num = turnOrder.IndexOf(stringColor);
			if (num == -1)
			{
				num = Colour.IDFromColour(colour);
				if (Colour.Grey == colour)
				{
					num = 99;
				}
			}
			this.PlayersList[i].ui.name = num.ToString("00");
		}
		this.PlayerUIGrid.repositionNow = true;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000A6ADC File Offset: 0x000A4CDC
	public void AddPlayer(NetworkPlayer targetPlayer, PlayerManager.PlayerData playerData)
	{
		base.networkView.RPC<PlayerManager.PlayerData>(targetPlayer, new Action<PlayerManager.PlayerData>(this.RPCAddPlayer), playerData);
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000A6AF7 File Offset: 0x000A4CF7
	public void AddPlayer(PlayerManager.PlayerData playerData)
	{
		base.networkView.RPC<PlayerManager.PlayerData>(RPCTarget.All, new Action<PlayerManager.PlayerData>(this.RPCAddPlayer), playerData);
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000A6B14 File Offset: 0x000A4D14
	[Remote(Permission.Server)]
	private void RPCAddPlayer(PlayerManager.PlayerData playerData)
	{
		PlayerState playerState = new PlayerState(playerData.name, playerData.networkPlayer, playerData.steamId, playerData.isVR);
		GameObject gameObject = this.PlayerUIGrid.gameObject.AddChild(this.PlayerButtonPrefab);
		UINameButton component = gameObject.GetComponent<UINameButton>();
		component.bHost = (playerState.id == 1 || NetworkSingleton<NetworkUI>.Instance.bHotseat);
		component.ServerOptionsGameObject = this.ServerOption;
		gameObject.GetComponentInChildren<UILabel>().text = playerData.name;
		playerState.ui = gameObject.GetComponent<UINameButton>();
		playerState.ui.SetPlayer(playerState.id);
		playerState.stringColor = playerData.stringColor;
		playerState.promoted = playerData.promoted;
		playerState.team = playerData.team;
		playerState.blind = playerData.blind;
		this.Add(playerState);
		this.UpdatePermissionUiNameButtons();
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x000A6BEE File Offset: 0x000A4DEE
	public void ChangeColor(int id, string stringColor)
	{
		base.networkView.RPC<int, string>(RPCTarget.All, new Action<int, string>(this.RPCChangeColor), id, stringColor);
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x000A6C0A File Offset: 0x000A4E0A
	[Remote(Permission.Admin)]
	private void RPCChangeColor(int id, string stringColor)
	{
		PlayerState playerState = this.PlayersDictionary[id];
		playerState.stringColor = stringColor;
		this.PlayersUpdate();
		EventManager.TriggerPlayerChangeColor(playerState);
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x000A6C2A File Offset: 0x000A4E2A
	public bool IsBlinded()
	{
		return this.MyPlayerState() != null && this.MyPlayerState().blind;
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000A6C41 File Offset: 0x000A4E41
	public void ToggleBlindfold()
	{
		this.ChangeBlindfold(NetworkID.ID, !this.MyPlayerState().blind);
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x000A6C5C File Offset: 0x000A4E5C
	public void SetVR(int id, bool isVR)
	{
		base.networkView.RPC<int, bool>(RPCTarget.All, new Action<int, bool>(this.RPCSetVR), id, isVR);
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x000A6C78 File Offset: 0x000A4E78
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCSetVR(int id, bool isVR)
	{
		this.PlayerStateFromID(id).isVR = isVR;
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x000A6C87 File Offset: 0x000A4E87
	public void ChangeBlindfold(int id, bool Blind)
	{
		base.networkView.RPC<int, bool>(RPCTarget.All, new Action<int, bool>(this.RPCBlind), id, Blind);
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000A6CA4 File Offset: 0x000A4EA4
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCBlind(int id, bool bBlind)
	{
		PlayerState playerState = this.PlayerStateFromID(id);
		playerState.blind = bBlind;
		if (NetworkID.ID == id)
		{
			if (this.IsAdmin(Network.sender) || !Network.isSenderOther)
			{
				NetworkUI.bHideGUI = false;
				if (bBlind)
				{
					Chat.Log("You have put on a blindfold and can no longer see blindfold messages for other players.", Colour.Red, ChatMessageType.Game, false);
				}
				else
				{
					Chat.Log("You have taken off a blindfold and can now see other players' blindfold messages.", ChatMessageType.Game);
				}
			}
		}
		else if (!this.PlayerStateFromID(NetworkID.ID).blind)
		{
			string name = playerState.name;
			if (bBlind)
			{
				Chat.Log(name + " has put on a blindfold and cannot see anything.", playerState.color, ChatMessageType.Game, false);
			}
			else
			{
				Chat.Log(name + " has taken off their blindfold and can see everything.", playerState.color, ChatMessageType.Game, false);
			}
		}
		EventManager.TriggerBlindfold(bBlind, id);
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x000A6D67 File Offset: 0x000A4F67
	public void StartHandSelectMode(string colourLabel, HandSelectModeSettings settings)
	{
		base.networkView.RPC<string, HandSelectModeSettings>(RPCTarget.All, new Action<string, HandSelectModeSettings>(this.RPCHandSelectMode), colourLabel, settings);
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x000A6D84 File Offset: 0x000A4F84
	[Remote(Permission.Server)]
	private void RPCHandSelectMode(string colourLabel, HandSelectModeSettings settings)
	{
		HandZone handZone = HandZone.GetHandZone(colourLabel, 0, true);
		if (!handZone)
		{
			return;
		}
		handZone.PushHandSelectMode(settings);
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x000A6DAA File Offset: 0x000A4FAA
	public void ClearHandSelectMode(string colourLabel)
	{
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.RPCClearHandSelectMode), colourLabel);
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000A6DC8 File Offset: 0x000A4FC8
	[Remote(Permission.Server)]
	private void RPCClearHandSelectMode(string colourLabel)
	{
		HandZone handZone = HandZone.GetHandZone(colourLabel, 0, true);
		if (!handZone)
		{
			return;
		}
		handZone.ClearHandSelectModes();
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x000A6DED File Offset: 0x000A4FED
	public void ChangeTeam(int id, Team team)
	{
		base.networkView.RPC<int, Team>(RPCTarget.All, new Action<int, Team>(this.RPCChangeTeam), id, team);
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x000A6E0C File Offset: 0x000A500C
	[Remote("Permissions/ChangeTeam")]
	private void RPCChangeTeam(int id, Team team)
	{
		PlayerState playerState = this.PlayerStateFromID(id);
		playerState.team = team;
		Chat.Log(playerState.name + " has joined team " + team.ToString() + ".", playerState.color, ChatMessageType.Game, false);
		EventManager.TriggerChangePlayerTeam(team != Team.None, id);
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000A6E69 File Offset: 0x000A5069
	public void RemovePlayer(int id)
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCRemovePlayer), id);
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000A6E84 File Offset: 0x000A5084
	[Remote(Permission.Server)]
	private void RPCRemovePlayer(int id)
	{
		this.Remove(this.PlayersDictionary[id]);
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x000A6E98 File Offset: 0x000A5098
	private void SpawnPlayerPotrait(CSteamID steamID, Texture2D Text)
	{
		if (!(Text != null))
		{
			Debug.Log("Steam failed to download avatar id: " + steamID);
			return;
		}
		int num = this.IDFromSteamID(steamID);
		if (num != -1)
		{
			PlayerState playerState = this.PlayerStateFromID(num);
			GameObject prefab = NetworkSingleton<GameMode>.Instance.GetPrefab("SteamPortrait");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, Vector3.zero, prefab.transform.rotation);
			gameObject.name = "Portrait " + playerState.name;
			gameObject.GetComponent<Renderer>().material.mainTexture = Text;
			gameObject.GetComponent<PlayerPortrait>().id = playerState.id;
			playerState.portrait = Text;
			return;
		}
		UnityEngine.Object.Destroy(Text);
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000A6F48 File Offset: 0x000A5148
	[Remote(Permission.Server)]
	private void RPCPromote(int id, bool promoted)
	{
		NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id).promoted = promoted;
		Debug.Log(id + " : " + NetworkID.ID);
		if (NetworkID.ID == id)
		{
			if (promoted)
			{
				NetworkSingleton<NetworkUI>.Instance.bCanFlipTable = true;
				Chat.Log("You have been promoted.", ChatMessageType.Game);
			}
			else
			{
				Chat.Log("You have been demoted.", ChatMessageType.Game);
			}
			this.UpdatePermissionUiNameButtons();
		}
		else
		{
			string str = NetworkSingleton<PlayerManager>.Instance.NameFromID(id);
			if (promoted)
			{
				Chat.Log(str + " has been promoted.", ChatMessageType.Game);
			}
			else
			{
				Chat.Log(str + " has been demoted.", ChatMessageType.Game);
			}
		}
		EventManager.TriggerPlayerPromote(promoted, id);
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x000A6FF5 File Offset: 0x000A51F5
	private void UpdatePermissionUiNameButtons()
	{
		this.PlayersDictionary[1].ui.bLookingForPlayer = (NetworkSingleton<ServerOptions>.Instance.LookingForPlayers && Network.isAdmin);
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000A7024 File Offset: 0x000A5224
	[Remote(Permission.Admin)]
	private void RPCMute(int id, bool muted)
	{
		NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id).muted = muted;
		if (NetworkID.ID == id)
		{
			if (muted)
			{
				Chat.Log("You have been muted.", ChatMessageType.Game);
			}
			else
			{
				Chat.Log("You have been unmuted.", ChatMessageType.Game);
			}
		}
		else
		{
			string str = NetworkSingleton<PlayerManager>.Instance.NameFromID(id);
			if (muted)
			{
				Chat.Log(str + " has been muted.", ChatMessageType.Game);
			}
			else
			{
				Chat.Log(str + " has been unmuted.", ChatMessageType.Game);
			}
		}
		EventManager.TriggerPlayerMute(muted, id);
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x000A70A1 File Offset: 0x000A52A1
	private void OnLoadingChange(int NumDownloads, int NumComplete)
	{
		if (NumDownloads == 0 && NumComplete == 0)
		{
			this.SetLoadingPercent(100);
			return;
		}
		this.SetLoadingPercent((int)Mathf.Round(100f * (float)NumComplete / (float)NumDownloads));
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x000A70CC File Offset: 0x000A52CC
	private void SetLoadingPercent(int percent)
	{
		PlayerState playerState = this.MyPlayerState();
		if (playerState != null && playerState.loadingPercent != percent)
		{
			playerState.loadingPercent = percent;
			base.networkView.RPC<byte>(RPCTarget.Others, new Action<byte>(this.RPCSetLoadingPercent), (byte)percent);
		}
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x000A7110 File Offset: 0x000A5310
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCSetLoadingPercent(byte percent)
	{
		PlayerState playerState = this.PlayerStateFromID((int)Network.sender.id);
		if (playerState != null)
		{
			playerState.loadingPercent = (int)percent;
		}
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x000A713C File Offset: 0x000A533C
	private void OnChatTyping(ChatMessageType type, bool typing)
	{
		if (type != ChatMessageType.Game && type != ChatMessageType.Team)
		{
			return;
		}
		if (typing)
		{
			if (this.typingWaitIdentifier == default(Wait.Identifier))
			{
				if (type == ChatMessageType.Team)
				{
					foreach (PlayerState playerState in this.PlayersList)
					{
						if (this.SameTeam(playerState.id, -1) && Network.player != playerState.networkPlayer)
						{
							base.networkView.RPC<bool>(playerState.networkPlayer, new Action<bool>(this.RPCTyping), true);
						}
					}
					this.RPCTyping(true);
				}
				else
				{
					base.networkView.RPC<bool>(RPCTarget.Others, new Action<bool>(this.RPCTyping), true);
					this.RPCTyping(true);
				}
			}
			else
			{
				Wait.Stop(this.typingWaitIdentifier);
			}
			this.typingWaitIdentifier = Wait.Time(delegate
			{
				this.typingWaitIdentifier = default(Wait.Identifier);
				base.networkView.RPC<bool>(RPCTarget.Others, new Action<bool>(this.RPCTyping), false);
				this.RPCTyping(false);
			}, 5f, 1);
			return;
		}
		if (this.typingWaitIdentifier != default(Wait.Identifier))
		{
			Wait.Stop(this.typingWaitIdentifier);
			this.typingWaitIdentifier = default(Wait.Identifier);
			base.networkView.RPC<bool>(RPCTarget.Others, new Action<bool>(this.RPCTyping), false);
			this.RPCTyping(false);
		}
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x000A7298 File Offset: 0x000A5498
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCTyping(bool typing)
	{
		PlayerState playerState = this.PlayerStateFromID((int)Network.sender.id);
		if (playerState != null)
		{
			playerState.typing = typing;
		}
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x000A72C4 File Offset: 0x000A54C4
	public void PromoteThisPlayer(string name)
	{
		if (Network.isServer)
		{
			int num = NetworkSingleton<PlayerManager>.Instance.IDFromName(name);
			if (num == NetworkID.ID)
			{
				Chat.Log("You cannot promote yourself.", Colour.Red, ChatMessageType.Game, false);
				return;
			}
			bool flag = !NetworkSingleton<PlayerManager>.Instance.IsPromoted(num);
			NetworkPlayer player = new NetworkPlayer((ushort)num);
			if (flag)
			{
				Network.AddAdmin(player);
			}
			else
			{
				Network.RemoveAdmin(player);
			}
			base.networkView.RPC<int, bool>(RPCTarget.All, new Action<int, bool>(this.RPCPromote), num, flag);
		}
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x000A7344 File Offset: 0x000A5544
	public void MuteThisPlayer(string name)
	{
		int num = NetworkSingleton<PlayerManager>.Instance.IDFromName(name);
		if (num == NetworkID.ID)
		{
			Chat.Log("You cannot mute yourself.", Colour.Red, ChatMessageType.Game, false);
			return;
		}
		if (NetworkSingleton<PlayerManager>.Instance.IsHost(-1))
		{
			base.networkView.RPC<int, bool>(RPCTarget.All, new Action<int, bool>(this.RPCMute), num, !NetworkSingleton<PlayerManager>.Instance.IsMuted(num));
			return;
		}
		NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(num).muted = !NetworkSingleton<PlayerManager>.Instance.IsMuted(num);
		EventManager.TriggerPlayerMute(NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(num).muted, num);
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x000A73E0 File Offset: 0x000A55E0
	public void KickThisPlayer(string name)
	{
		if (Network.isServer)
		{
			int num = NetworkSingleton<PlayerManager>.Instance.IDFromName(name);
			if (num == NetworkID.ID)
			{
				Chat.Log("Why are you trying to kick yourself, silly?", Colour.Red, ChatMessageType.Game, false);
				return;
			}
			Chat.SendChat(name + " is kicked.", Color.yellow);
			NetworkSingleton<NetworkUI>.Instance.KickPlayer(new NetworkPlayer((ushort)num), "You have been kicked.");
		}
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x000A7448 File Offset: 0x000A5648
	public void BanThisPlayer(string name)
	{
		if (Network.isServer)
		{
			int num = NetworkSingleton<PlayerManager>.Instance.IDFromName(name);
			if (num == NetworkID.ID)
			{
				Chat.Log("Why are you trying to ban yourself, silly?", Colour.Red, ChatMessageType.Game, false);
				return;
			}
			Chat.SendChat(name + " is banned.", Color.red);
			Singleton<BlockList>.Instance.AddBlock(NetworkSingleton<PlayerManager>.Instance.NameFromID(num), NetworkSingleton<PlayerManager>.Instance.SteamIDFromID(num));
			NetworkSingleton<NetworkUI>.Instance.KickPlayer(new NetworkPlayer((ushort)num), "You have been banned.");
		}
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x000A74CD File Offset: 0x000A56CD
	public bool IsPromoted(int ID = -1)
	{
		if (ID == -1)
		{
			ID = NetworkID.ID;
		}
		return this.PlayersDictionary.ContainsKey(ID) && this.PlayersDictionary[ID].promoted;
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x000A74FB File Offset: 0x000A56FB
	public bool IsMuted(int ID = -1)
	{
		if (ID == -1)
		{
			ID = NetworkID.ID;
		}
		return this.PlayersDictionary.ContainsKey(ID) && this.PlayersDictionary[ID].muted;
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x000A7529 File Offset: 0x000A5729
	public bool IsHost(NetworkPlayer NP)
	{
		return NP.isServer;
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x000A7532 File Offset: 0x000A5732
	public bool IsHost(int ID = -1)
	{
		if (ID == -1)
		{
			ID = NetworkID.ID;
		}
		return this.IsHost(new NetworkPlayer((ushort)ID));
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x000A754C File Offset: 0x000A574C
	public bool IsAdmin(NetworkPlayer NP)
	{
		return NP.isAdmin;
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x000A7555 File Offset: 0x000A5755
	public bool IsAdmin(int ID = -1)
	{
		if (ID == -1)
		{
			ID = NetworkID.ID;
		}
		return NetworkSingleton<NetworkUI>.Instance.bHotseat || this.IsAdmin(new NetworkPlayer((ushort)ID));
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x000A757D File Offset: 0x000A577D
	public bool SameTeam(Color color)
	{
		return (PlayerScript.Pointer != null && color == Colour.ColourFromLabel(PlayerScript.PointerScript.PointerColorLabel)) || this.SameTeam(Colour.LabelFromColour(color), -1);
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x000A75B7 File Offset: 0x000A57B7
	public bool SameTeam(string color, int sourceId = -1)
	{
		return this.SameTeam(this.IDFromColour(color), -1);
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x000A75C8 File Offset: 0x000A57C8
	public bool SameTeam(int ID, int sourceId = -1)
	{
		if (sourceId == -1)
		{
			sourceId = NetworkID.ID;
		}
		return this.PlayersDictionary.ContainsKey(ID) && this.PlayersDictionary.ContainsKey(sourceId) && (this.PlayersDictionary[ID].team != Team.None && this.PlayersDictionary[sourceId].team != Team.None) && this.PlayersDictionary[ID].team == this.PlayersDictionary[sourceId].team;
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x000A764C File Offset: 0x000A584C
	public bool SameTeam(string colourA, string colourB)
	{
		return this.SameTeam(this.IDFromColour(colourA), this.IDFromColour(colourB));
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000A7662 File Offset: 0x000A5862
	public bool SameTeam(Colour colourA, Colour colourB)
	{
		return this.SameTeam(Colour.LabelFromColour(colourA), Colour.LabelFromColour(colourB));
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000A7678 File Offset: 0x000A5878
	public bool NameInUse(string name)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].name.ToUpper() == name.ToUpper())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000A76C1 File Offset: 0x000A58C1
	public bool ColourInUse(Colour colour)
	{
		return this.ColourInUse(Colour.LabelFromColour(colour));
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x000A76D0 File Offset: 0x000A58D0
	public bool ColourInUse(string label)
	{
		if (label == "Grey")
		{
			return false;
		}
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].stringColor == label)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x000A771E File Offset: 0x000A591E
	public string NameFromColour(Colour colour)
	{
		return this.NameFromColour(Colour.LabelFromColour(colour));
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000A772C File Offset: 0x000A592C
	public string NameFromColour(string label)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].stringColor == label)
			{
				return this.PlayersList[i].name;
			}
		}
		return null;
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000A777B File Offset: 0x000A597B
	public int IDFromColour(Colour colour)
	{
		return this.IDFromColour(Colour.LabelFromColour(colour));
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x000A778C File Offset: 0x000A598C
	public int IDFromColour(string label)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].stringColor == label)
			{
				return this.PlayersList[i].id;
			}
		}
		return -1;
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x000A77DC File Offset: 0x000A59DC
	public Colour ColourFromName(string name)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].name == name)
			{
				return this.PlayersList[i].color;
			}
		}
		return Color.clear;
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000A783C File Offset: 0x000A5A3C
	public int IDFromName(string name)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].name == name)
			{
				return this.PlayersList[i].id;
			}
		}
		return -1;
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000A788C File Offset: 0x000A5A8C
	public string SteamIDFromName(string name)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].name == name)
			{
				return this.PlayersList[i].steamId;
			}
		}
		return null;
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000A78DB File Offset: 0x000A5ADB
	public string ColourLabelFromID(int id)
	{
		return this.PlayersDictionary[id].stringColor;
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000A78EE File Offset: 0x000A5AEE
	public Color ColourFromID(int id)
	{
		return this.PlayersDictionary[id].color;
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x000A7901 File Offset: 0x000A5B01
	public string NameFromID(int id)
	{
		return this.PlayersDictionary[id].name;
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000A7914 File Offset: 0x000A5B14
	public string SteamIDFromID(int id)
	{
		return this.PlayersDictionary[id].steamId;
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x000A7927 File Offset: 0x000A5B27
	public GameObject ObjectFromID(int id)
	{
		return this.PlayersDictionary[id].ui.gameObject;
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x000A793F File Offset: 0x000A5B3F
	public CSteamID CSteamIDFromID(int id)
	{
		return this.PlayersDictionary[id].cSteamId;
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000A7954 File Offset: 0x000A5B54
	public NetworkPlayer NetworkPlayerFromSteamID(CSteamID id)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].cSteamId == id)
			{
				return this.PlayersList[i].networkPlayer;
			}
		}
		return new NetworkPlayer(0);
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x000A79A8 File Offset: 0x000A5BA8
	public string NameFromSteamID(string id)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].steamId == id)
			{
				return this.PlayersList[i].name;
			}
		}
		return null;
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x000A79F8 File Offset: 0x000A5BF8
	public string NameFromSteamID(CSteamID id)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].cSteamId == id)
			{
				return this.PlayersList[i].name;
			}
		}
		return null;
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x000A7A48 File Offset: 0x000A5C48
	public int IDFromSteamID(CSteamID id)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].cSteamId == id)
			{
				return this.PlayersList[i].id;
			}
		}
		return -1;
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x000A7A98 File Offset: 0x000A5C98
	public PlayerState PlayerStateFromColour(Colour colour)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].color == colour)
			{
				return this.PlayersList[i];
			}
		}
		return null;
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x000A7AE4 File Offset: 0x000A5CE4
	public PlayerState PlayerStateFromSteamID(string id)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].steamId == id)
			{
				return this.PlayersList[i];
			}
		}
		return null;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x000A7B30 File Offset: 0x000A5D30
	public PlayerState PlayerStateFromSteamID(CSteamID id)
	{
		for (int i = 0; i < this.PlayersList.Count; i++)
		{
			if (this.PlayersList[i].cSteamId == id)
			{
				return this.PlayersList[i];
			}
		}
		return null;
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x000A7B7C File Offset: 0x000A5D7C
	public PlayerState PlayerStateFromID(int id)
	{
		PlayerState result;
		this.PlayersDictionary.TryGetValue(id, out result);
		return result;
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x000A7B99 File Offset: 0x000A5D99
	public PlayerState MyPlayerState()
	{
		return this.PlayerStateFromID(NetworkID.ID);
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x000A7BA6 File Offset: 0x000A5DA6
	public bool ContainsID(int id = -1)
	{
		if (id == -1)
		{
			id = NetworkID.ID;
		}
		return this.PlayersDictionary.ContainsKey(id);
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x000A7BBF File Offset: 0x000A5DBF
	public bool ContainsSteamId(CSteamID id)
	{
		return this.IDFromSteamID(id) != -1;
	}

	// Token: 0x04000EB8 RID: 3768
	public UIGrid PlayerUIGrid;

	// Token: 0x04000EB9 RID: 3769
	public GameObject PlayerButtonPrefab;

	// Token: 0x04000EBA RID: 3770
	public GameObject ServerOption;

	// Token: 0x04000EBB RID: 3771
	public Dictionary<int, PlayerState> PlayersDictionary = new Dictionary<int, PlayerState>();

	// Token: 0x04000EBC RID: 3772
	public List<PlayerState> PlayersList = new List<PlayerState>();

	// Token: 0x04000EBD RID: 3773
	private Wait.Identifier typingWaitIdentifier;

	// Token: 0x020006B5 RID: 1717
	public class PlayerData
	{
		// Token: 0x06003C4C RID: 15436 RVA: 0x00002594 File Offset: 0x00000794
		public PlayerData()
		{
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x00178C88 File Offset: 0x00176E88
		public PlayerData(string name, NetworkPlayer networkPlayer, string steamId, bool isVR, string stringColor = "Grey", bool promoted = false, Team team = Team.None, bool blind = false)
		{
			this.name = name;
			this.networkPlayer = networkPlayer;
			this.steamId = steamId;
			this.stringColor = stringColor;
			this.promoted = promoted;
			this.team = team;
			this.blind = blind;
			this.isVR = isVR;
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00178CD8 File Offset: 0x00176ED8
		public PlayerData(PlayerState playerState)
		{
			this.name = playerState.name;
			this.networkPlayer = playerState.networkPlayer;
			this.steamId = playerState.steamId;
			this.stringColor = playerState.stringColor;
			this.promoted = playerState.promoted;
			this.team = playerState.team;
			this.blind = playerState.blind;
			this.isVR = playerState.isVR;
		}

		// Token: 0x04002909 RID: 10505
		public string name;

		// Token: 0x0400290A RID: 10506
		public NetworkPlayer networkPlayer;

		// Token: 0x0400290B RID: 10507
		public string steamId;

		// Token: 0x0400290C RID: 10508
		public string stringColor;

		// Token: 0x0400290D RID: 10509
		public bool promoted;

		// Token: 0x0400290E RID: 10510
		public Team team;

		// Token: 0x0400290F RID: 10511
		public bool blind;

		// Token: 0x04002910 RID: 10512
		public bool isVR;
	}
}
