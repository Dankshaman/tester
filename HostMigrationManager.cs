using System;
using System.Collections.Generic;
using NewNet;
using Steamworks;

// Token: 0x02000129 RID: 297
public class HostMigrationManager : NetworkSingleton<HostMigrationManager>
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0006AD5B File Offset: 0x00068F5B
	// (set) Token: 0x06000FAA RID: 4010 RVA: 0x0006AD62 File Offset: 0x00068F62
	public static HostMigrationManager.HostMigrationInfo migrationInfo { get; private set; }

	// Token: 0x06000FAB RID: 4011 RVA: 0x0006AD6C File Offset: 0x00068F6C
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnServerInitialized += this.NetworkEvents_OnServerInitialized;
		NetworkEvents.OnConnectedToServer += this.NetworkEvents_OnConnectedToServer;
		NetworkEvents.OnDisconnectedFromServer += this.NetworkEvents_OnDisconnectedFromServer;
		NetworkEvents.OnPlayerDisconnected += this.NetworkEvents_OnPlayerDisconnected;
		EventManager.OnPlayersAdd += this.EventManager_OnPlayersAdd;
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0006ADD4 File Offset: 0x00068FD4
	private void OnDestroy()
	{
		NetworkEvents.OnServerInitialized -= this.NetworkEvents_OnServerInitialized;
		NetworkEvents.OnConnectedToServer -= this.NetworkEvents_OnConnectedToServer;
		NetworkEvents.OnDisconnectedFromServer -= this.NetworkEvents_OnDisconnectedFromServer;
		NetworkEvents.OnPlayerDisconnected -= this.NetworkEvents_OnPlayerDisconnected;
		EventManager.OnPlayersAdd -= this.EventManager_OnPlayersAdd;
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0006AE36 File Offset: 0x00069036
	private void Start()
	{
		if (this.IsMigrating())
		{
			this.StartMigration();
		}
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0006AE46 File Offset: 0x00069046
	public bool IsMigrating()
	{
		HostMigrationManager.HostMigrationInfo migrationInfo = HostMigrationManager.migrationInfo;
		return ((migrationInfo != null) ? migrationInfo.GetHostData() : null) != null;
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0006AE5C File Offset: 0x0006905C
	public bool IsMigrating(CSteamID host)
	{
		return this.IsMigrating() && host.ToString() == HostMigrationManager.migrationInfo.GetHostData().steamId;
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0006AE89 File Offset: 0x00069089
	public void ResetMigrating()
	{
		HostMigrationManager.migrationInfo = null;
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0006AE94 File Offset: 0x00069094
	private void StartMigration()
	{
		if (this.IsMigrating(SteamManager.UserSteamID))
		{
			Chat.Log("Starting Server Host Migration...", Colour.Green, ChatMessageType.All, true);
			Network.serverName = HostMigrationManager.migrationInfo.ServerName;
			Network.password = HostMigrationManager.migrationInfo.Password;
			Network.serverType = HostMigrationManager.migrationInfo.ServerType;
			Network.maxConnections = HostMigrationManager.migrationInfo.MaxConnections;
			Network.InitializeServer();
			return;
		}
		string name = HostMigrationManager.migrationInfo.GetHostData().name;
		Chat.Log(string.Format("Player {0} has accepted to be the Server's Host. Waiting to join...", name), Colour.Green, ChatMessageType.All, true);
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0006AF28 File Offset: 0x00069128
	private void NetworkEvents_OnServerInitialized()
	{
		if (this.IsMigrating(SteamManager.UserSteamID) && Network.maxConnections > 0)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIGames.SetActive(false);
			using (List<PlayerManager.PlayerData>.Enumerator enumerator = HostMigrationManager.migrationInfo.players.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerManager.PlayerData player = enumerator.Current;
					Action <>9__2;
					Wait.Condition(delegate
					{
						Action action;
						if ((action = <>9__2) == null)
						{
							action = (<>9__2 = delegate()
							{
								PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromSteamID(player.steamId);
								NetworkSingleton<NetworkUI>.Instance.CheckColor(player.stringColor, playerState.id);
								if (player.promoted || player.networkPlayer.isServer)
								{
									NetworkSingleton<PlayerManager>.Instance.PromoteThisPlayer(playerState.name);
								}
								if (player.blind)
								{
									NetworkSingleton<PlayerManager>.Instance.ChangeBlindfold(playerState.id, true);
								}
								if (player.team != Team.None)
								{
									NetworkSingleton<PlayerManager>.Instance.ChangeTeam(playerState.id, player.team);
								}
							});
						}
						Wait.Frames(action, 1);
					}, () => NetworkSingleton<PlayerManager>.Instance.PlayerStateFromSteamID(player.steamId) != null, 120f, null);
					if (player.steamId != SteamManager.StringSteamID)
					{
						Singleton<SteamLobbyManager>.Instance.InviteToLobby(SteamManager.StringToSteamID(player.steamId));
					}
				}
			}
			NetworkSingleton<ManagerPhysicsObject>.Instance.LoadSaveState(HostMigrationManager.migrationInfo.saveState, false, true);
		}
		this.ResetMigrating();
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0006B020 File Offset: 0x00069220
	private void NetworkEvents_OnConnectedToServer()
	{
		this.ResetMigrating();
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x000025B8 File Offset: 0x000007B8
	private void EventManager_OnPlayersAdd(PlayerState playerState)
	{
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x000025B8 File Offset: 0x000007B8
	private void NetworkEvents_OnDisconnectedFromServer(DisconnectInfo info)
	{
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x000025B8 File Offset: 0x000007B8
	private void NetworkEvents_OnPlayerDisconnected(NetworkPlayer player, DisconnectInfo info)
	{
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0006B028 File Offset: 0x00069228
	public void AskToMigrate(NetworkPlayer player)
	{
		if (Network.isClient || player.isServer)
		{
			return;
		}
		if (!this.askedPlayers.Contains(player))
		{
			this.askedPlayers.Add(player);
		}
		base.networkView.RPC(player, new Action(this.RPCAskToMigrate));
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x0006B078 File Offset: 0x00069278
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCAskToMigrate()
	{
		UIDialog.Show("Do you accept to be the Server's Host?", "Yes", "No", delegate()
		{
			this.HostMigrationResponse(true);
		}, delegate()
		{
			this.HostMigrationResponse(false);
		});
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x0006B0A6 File Offset: 0x000692A6
	private void HostMigrationResponse(bool accept)
	{
		base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.RPCHostMigrationResponse), accept);
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x0006B0C4 File Offset: 0x000692C4
	[Remote(SendType.ReliableNoDelay)]
	private void RPCHostMigrationResponse(bool accept)
	{
		if (!this.askedPlayers.Contains(Network.sender))
		{
			return;
		}
		string arg = NetworkSingleton<PlayerManager>.Instance.NameFromID((int)Network.sender.id);
		if (accept)
		{
			this.MigrateHostTo(Network.sender);
			this.askedPlayers.Clear();
			return;
		}
		Chat.Log(string.Format("Player {0} has declined to be the Server's Host.", arg), Colour.Red, ChatMessageType.All, true);
		this.askedPlayers.Remove(Network.sender);
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x0006B140 File Offset: 0x00069340
	private void MigrateHostTo(NetworkPlayer newHost)
	{
		try
		{
			base.networkView.RPC<SaveState>(newHost, new Action<SaveState>(this.RPCMigrateHost), NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentState());
		}
		catch (Exception)
		{
			Chat.LogError("Save is too large to transfer for Host Migration.", true);
			return;
		}
		List<NetworkPlayer> playersToMonitor = new List<NetworkPlayer>();
		foreach (NetworkPlayer networkPlayer in Network.connections)
		{
			if (networkPlayer != newHost)
			{
				base.networkView.RPC<NetworkPlayer>(networkPlayer, new Action<NetworkPlayer>(this.RPCMigrateClient), newHost);
			}
			if (networkPlayer != Network.player)
			{
				playersToMonitor.Add(networkPlayer);
			}
		}
		Wait.Condition(new Action(NetworkSingleton<NetworkUI>.Instance.GUIDisconnect), delegate
		{
			foreach (NetworkPlayer item in playersToMonitor)
			{
				if (Network.connections.Contains(item))
				{
					return false;
				}
			}
			return true;
		}, 10f, new Action(NetworkSingleton<NetworkUI>.Instance.GUIDisconnect));
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x0006B250 File Offset: 0x00069450
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCMigrateClient(NetworkPlayer newHost)
	{
		HostMigrationManager.migrationInfo = this.GetHostInfo(null);
		HostMigrationManager.migrationInfo.Host = newHost;
		if (Network.isClient)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIDisconnect();
		}
		Chat.Log("Migrating Server Hosts...", ChatMessageType.All);
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x0006B285 File Offset: 0x00069485
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default, serializationMethod = SerializationMethod.Json)]
	private void RPCMigrateHost(SaveState saveState)
	{
		HostMigrationManager.migrationInfo = this.GetHostInfo(saveState);
		HostMigrationManager.migrationInfo.Host = Network.player;
		NetworkSingleton<NetworkUI>.Instance.GUIDisconnect();
		Chat.Log("Migrating Server Hosts...", ChatMessageType.All);
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x0006B2B8 File Offset: 0x000694B8
	private HostMigrationManager.HostMigrationInfo GetHostInfo(SaveState saveState)
	{
		List<PlayerManager.PlayerData> list = new List<PlayerManager.PlayerData>();
		foreach (PlayerState playerState in NetworkSingleton<PlayerManager>.Instance.PlayersList)
		{
			list.Add(new PlayerManager.PlayerData(playerState));
		}
		return new HostMigrationManager.HostMigrationInfo
		{
			ServerName = NetworkSingleton<ServerOptions>.Instance.ServerName,
			Password = NetworkSingleton<ServerOptions>.Instance.Password,
			ServerType = NetworkSingleton<ServerOptions>.Instance.ServerType,
			MaxConnections = NetworkSingleton<ServerOptions>.Instance.MaxConnections,
			LookingForPlayers = NetworkSingleton<ServerOptions>.Instance.LookingForPlayers,
			players = list,
			saveState = saveState
		};
	}

	// Token: 0x040009BF RID: 2495
	private List<NetworkPlayer> askedPlayers = new List<NetworkPlayer>();

	// Token: 0x0200063B RID: 1595
	public class HostMigrationInfo
	{
		// Token: 0x06003AFA RID: 15098 RVA: 0x00175ED0 File Offset: 0x001740D0
		public PlayerManager.PlayerData GetHostData()
		{
			foreach (PlayerManager.PlayerData playerData in this.players)
			{
				if (playerData.networkPlayer == this.Host)
				{
					return playerData;
				}
			}
			return null;
		}

		// Token: 0x04002752 RID: 10066
		public bool migrated;

		// Token: 0x04002753 RID: 10067
		public NetworkPlayer Host;

		// Token: 0x04002754 RID: 10068
		public string ServerName;

		// Token: 0x04002755 RID: 10069
		public string Password;

		// Token: 0x04002756 RID: 10070
		public ServerType ServerType;

		// Token: 0x04002757 RID: 10071
		public int MaxConnections;

		// Token: 0x04002758 RID: 10072
		public bool LookingForPlayers;

		// Token: 0x04002759 RID: 10073
		public List<PlayerManager.PlayerData> players;

		// Token: 0x0400275A RID: 10074
		public SaveState saveState;
	}
}
