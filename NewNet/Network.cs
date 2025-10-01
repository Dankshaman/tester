using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace NewNet
{
	// Token: 0x0200039E RID: 926
	public static class Network
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x0013411D File Offset: 0x0013231D
		// (set) Token: 0x06002BB5 RID: 11189 RVA: 0x00134124 File Offset: 0x00132324
		public static NetworkPlayer player { get; private set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x0013412C File Offset: 0x0013232C
		// (set) Token: 0x06002BB7 RID: 11191 RVA: 0x00134133 File Offset: 0x00132333
		public static NetworkPlayer sender { get; internal set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x0013413B File Offset: 0x0013233B
		// (set) Token: 0x06002BB9 RID: 11193 RVA: 0x00134142 File Offset: 0x00132342
		public static NetworkPeerMode peerType { get; private set; } = NetworkPeerMode.Disconnected;

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002BBA RID: 11194 RVA: 0x0013414C File Offset: 0x0013234C
		public static bool isServer
		{
			get
			{
				return Network.player.isServer;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x00134168 File Offset: 0x00132368
		public static bool isAdmin
		{
			get
			{
				return Network.player.isAdmin;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x00134184 File Offset: 0x00132384
		public static bool isClient
		{
			get
			{
				return Network.player.isClient;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06002BBD RID: 11197 RVA: 0x0013419E File Offset: 0x0013239E
		public static bool isSenderOther
		{
			get
			{
				return Network.sender != Network.player;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06002BBE RID: 11198 RVA: 0x001341AF File Offset: 0x001323AF
		// (set) Token: 0x06002BBF RID: 11199 RVA: 0x001341B6 File Offset: 0x001323B6
		public static int maxConnections
		{
			get
			{
				return Network._maxConnections;
			}
			set
			{
				if (Network._maxConnections != value)
				{
					Network._maxConnections = value;
					NetworkEvents.TriggerSettingsChange();
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x001341CB File Offset: 0x001323CB
		// (set) Token: 0x06002BC1 RID: 11201 RVA: 0x001341D2 File Offset: 0x001323D2
		public static ServerType serverType
		{
			get
			{
				return Network._serverType;
			}
			set
			{
				if (Network._serverType != value)
				{
					Network._serverType = value;
					NetworkEvents.TriggerSettingsChange();
					Singleton<SteamLobbyManager>.Instance.SetLobbyType(value);
				}
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x001341F2 File Offset: 0x001323F2
		// (set) Token: 0x06002BC3 RID: 11203 RVA: 0x001341F9 File Offset: 0x001323F9
		public static string password
		{
			get
			{
				return Network._password;
			}
			set
			{
				if (value != null && value.Length > 40)
				{
					value = value.Substring(0, 40);
				}
				if (Network._password != value)
				{
					Network._password = value;
					NetworkEvents.TriggerSettingsChange();
				}
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x0013422B File Offset: 0x0013242B
		// (set) Token: 0x06002BC5 RID: 11205 RVA: 0x00134232 File Offset: 0x00132432
		public static string version
		{
			get
			{
				return Network._version;
			}
			set
			{
				if (Network._version != value)
				{
					Network._version = value;
					NetworkEvents.TriggerSettingsChange();
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x0013424C File Offset: 0x0013244C
		// (set) Token: 0x06002BC7 RID: 11207 RVA: 0x00134253 File Offset: 0x00132453
		public static string serverName
		{
			get
			{
				return Network._serverName;
			}
			set
			{
				if (Network._serverName != value)
				{
					Network._serverName = value;
					NetworkEvents.TriggerSettingsChange();
				}
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x0013426D File Offset: 0x0013246D
		// (set) Token: 0x06002BC9 RID: 11209 RVA: 0x00134274 File Offset: 0x00132474
		public static string gameName
		{
			get
			{
				return Network._gameName;
			}
			set
			{
				if (Network._gameName != value)
				{
					Network._gameName = value;
					NetworkEvents.TriggerSettingsChange();
				}
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06002BCA RID: 11210 RVA: 0x0013428E File Offset: 0x0013248E
		// (set) Token: 0x06002BCB RID: 11211 RVA: 0x00134295 File Offset: 0x00132495
		public static string comment
		{
			get
			{
				return Network._comment;
			}
			set
			{
				if (Network._comment != value)
				{
					Network._comment = value;
					NetworkEvents.TriggerSettingsChange();
				}
			}
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x001342B0 File Offset: 0x001324B0
		public static NetworkPlayer IdToNetworkPlayer(int id)
		{
			for (int i = 0; i < Network.connections.Count; i++)
			{
				NetworkPlayer result = Network.connections[i];
				if ((int)result.id == id)
				{
					return result;
				}
			}
			return default(NetworkPlayer);
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x001342F4 File Offset: 0x001324F4
		public static ushort GetNextPlayerId()
		{
			ushort num = 1;
			for (int i = 0; i < Network.connections.Count; i++)
			{
				if (Network.connections[i].id >= num)
				{
					num = Network.connections[i].id;
					num += 1;
				}
			}
			return num;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00134348 File Offset: 0x00132548
		public static int GetAveragePing(NetworkPlayer player)
		{
			SteamP2PManager.PlayerInfo playerInfo;
			if (Singleton<SteamP2PManager>.Instance.players.TryGetValue(player.steamID, out playerInfo))
			{
				return playerInfo.smoothPing;
			}
			return -1;
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00134378 File Offset: 0x00132578
		private static void Cleanup()
		{
			Network.peerType = NetworkPeerMode.Disconnected;
			Network.connections.Clear();
			Network.steamPlayers.Clear();
			Network.admins.Clear();
			Network.serverName = null;
			Network.serverType = ServerType.Public;
			Network.password = null;
			Network.version = null;
			Network.gameName = null;
			Network.serverName = null;
			Network.maxConnections = 10;
			Network.sender = default(NetworkPlayer);
			Network.player = default(NetworkPlayer);
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x001343F0 File Offset: 0x001325F0
		public static void InitializeServer()
		{
			if (!SteamManager.Initialized)
			{
				Chat.LogError("Not connected to Steam.", true);
				return;
			}
			NetworkEvents.TriggerServerInitializing();
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x0013440A File Offset: 0x0013260A
		internal static void ServerInitialized()
		{
			Network.InternalAddPlayer(NetworkPlayer.GetServerPlayer(), SteamUser.GetSteamID());
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x0013441B File Offset: 0x0013261B
		public static void Disconnect()
		{
			Network.ReceiveDisconnect(DisconnectInfo.Successful);
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x00134423 File Offset: 0x00132623
		internal static void ReceiveDisconnect(DisconnectInfo info)
		{
			Network.Cleanup();
			NetworkEvents.TriggerDisconnectedFromServer(info);
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x00134430 File Offset: 0x00132630
		public static void CloseConnectionTo(NetworkPlayer player)
		{
			if (Network.connections.Contains(player))
			{
				Network.RemovePlayer(player, DisconnectInfo.Kick);
			}
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x00134446 File Offset: 0x00132646
		internal static void Connect(CSteamID steamID, string password = null)
		{
			Network.password = password;
			Network.InternalAddPlayer(NetworkPlayer.GetServerPlayer(), steamID);
			NetworkEvents.TriggerConnectingToServer();
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x0013445E File Offset: 0x0013265E
		private static int GetNetworkViewCount(GameObject go)
		{
			go.GetComponentsInChildren<NetworkView>(true, Network.networkViewsForCount);
			int count = Network.networkViewsForCount.Count;
			Network.networkViewsForCount.Clear();
			return count;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x00134480 File Offset: 0x00132680
		private static void CallLifeCycles(GameObject go, bool creation)
		{
			if (!Network.usingLifeCycles)
			{
				Network.usingLifeCycles = true;
				go.GetComponentsInChildren<INetworkLifeCycle>(true, Network.lifeCycles);
				for (int i = 0; i < Network.lifeCycles.Count; i++)
				{
					if (creation)
					{
						Network.lifeCycles[i].Creation();
					}
					else
					{
						Network.lifeCycles[i].Destruction();
					}
				}
				Network.lifeCycles.Clear();
				Network.usingLifeCycles = false;
				return;
			}
			INetworkLifeCycle[] componentsInChildren = go.GetComponentsInChildren<INetworkLifeCycle>(true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (creation)
				{
					componentsInChildren[j].Creation();
				}
				else
				{
					componentsInChildren[j].Destruction();
				}
			}
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x0013451C File Offset: 0x0013271C
		public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, NetworkPlayer owner = default(NetworkPlayer))
		{
			return Network.Instantiate(prefab, position, rotation.eulerAngles, owner);
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x0013452D File Offset: 0x0013272D
		public static T Instantiate<T>(GameObject prefab, Vector3 position = default(Vector3), Vector3 rotation = default(Vector3), NetworkPlayer owner = default(NetworkPlayer))
		{
			return Network.Instantiate(prefab, position, rotation, owner).GetComponent<T>();
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00134540 File Offset: 0x00132740
		public static GameObject Instantiate(GameObject prefab, Vector3 position = default(Vector3), Vector3 rotation = default(Vector3), NetworkPlayer owner = default(NetworkPlayer))
		{
			if (Network.peerType == NetworkPeerMode.Disconnected)
			{
				Debug.LogError("Can't Network Instantiate if not connected.");
				return null;
			}
			if (Network.isClient)
			{
				Debug.LogError("Client cannot Network Instantiate.");
				return null;
			}
			if (!owner.IsValid())
			{
				owner = NetworkPlayer.GetServerPlayer();
			}
			string text = Utilities.RemoveCloneFromName(prefab.name);
			prefab = Resources.Load<GameObject>(text);
			if (!prefab)
			{
				Debug.LogError("Could not find prefab in resources: " + text);
				return null;
			}
			int networkViewCount = Network.GetNetworkViewCount(prefab);
			if (networkViewCount == 0)
			{
				Debug.LogError("Can't Network Instantiate object with no NetworkView.");
				return null;
			}
			NetworkView.GetNextIds(Network.ids, networkViewCount);
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(2);
			stream.WriteUshort(owner.id);
			stream.WriteString(text);
			for (int i = 0; i < networkViewCount; i++)
			{
				stream.WriteUshort(Network.ids[i]);
			}
			stream.WriteVector3(position);
			stream.WriteVector3(rotation);
			Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.Others, stream, SendType.ReliableBuffered, -1);
			NetworkManager.ReturnStream(stream);
			return Network.InternalInstantiate(owner, prefab, Network.ids, networkViewCount, position, rotation);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00134644 File Offset: 0x00132844
		internal static void ReceiveInstantiate(BitStream stream, NetworkPlayer networkPlayer)
		{
			if (networkPlayer.isClient)
			{
				Debug.LogError("Client cannot Network Instantiate.");
				return;
			}
			NetworkPlayer owner = new NetworkPlayer(stream.ReadUshort());
			string text = stream.ReadString();
			GameObject gameObject = Resources.Load<GameObject>(text);
			if (!gameObject)
			{
				Debug.LogError("Could not find prefab in resources: " + text);
				return;
			}
			int networkViewCount = Network.GetNetworkViewCount(gameObject);
			for (int i = 0; i < networkViewCount; i++)
			{
				Network.ids[i] = stream.ReadUshort();
			}
			Vector3 position = stream.ReadVector3();
			Vector3 rotation = stream.ReadVector3();
			Network.InternalInstantiate(owner, gameObject, Network.ids, networkViewCount, position, rotation);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x001346E0 File Offset: 0x001328E0
		private static GameObject InternalInstantiate(NetworkPlayer owner, GameObject prefab, ushort[] ids, int idCount, Vector3 position, Vector3 rotation)
		{
			NetworkView.SetNextIds(ids, idCount, owner);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, Quaternion.Euler(rotation));
			Network.CallLifeCycles(gameObject, true);
			return gameObject;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00134700 File Offset: 0x00132900
		public static void Destroy(GameObject gameObject)
		{
			if (gameObject)
			{
				Network.Destroy(gameObject.GetComponent<NetworkView>());
			}
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x00134718 File Offset: 0x00132918
		public static void Destroy(NetworkView networkView)
		{
			if (!networkView)
			{
				return;
			}
			if (Network.isClient)
			{
				Debug.LogError("Client cannot Network Destroy.");
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(3);
			stream.WriteUshort(networkView.id);
			Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.All, stream, SendType.ReliableBuffered, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00134774 File Offset: 0x00132974
		internal static void ReceiveDestroy(BitStream stream, NetworkPlayer networkPlayer)
		{
			if (networkPlayer.isClient)
			{
				Debug.LogError("Client cannot Network Destroy.");
				return;
			}
			ushort num = stream.ReadUshort();
			NetworkView networkView;
			if (!NetworkView.IdView.TryGetValue(num, out networkView))
			{
				Debug.LogError("Unable to find object to Network Destroy: " + num);
				return;
			}
			Network.InternalDestroy(networkView);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x001347C7 File Offset: 0x001329C7
		private static void InternalDestroy(NetworkView networkView)
		{
			GameObject gameObject = networkView.gameObject;
			UnityEngine.Object.Destroy(gameObject);
			Network.CallLifeCycles(gameObject, false);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x001347DC File Offset: 0x001329DC
		internal static void AddPlayer(NetworkPlayer player, CSteamID steamID)
		{
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(4);
			stream.WriteUshort(player.id);
			stream.WriteUlong(steamID.m_SteamID);
			Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.All, stream, SendType.ReliableBuffered, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x0013482C File Offset: 0x00132A2C
		internal static void ReceiveAddPlayer(BitStream stream, NetworkPlayer sender)
		{
			if (sender.isClient)
			{
				Debug.LogError("Client cannot add player.");
				return;
			}
			ushort id = stream.ReadUshort();
			ulong ulSteamID = stream.ReadUlong();
			NetworkPlayer player = new NetworkPlayer(id);
			CSteamID steamID = new CSteamID(ulSteamID);
			Network.InternalAddPlayer(player, steamID);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00134870 File Offset: 0x00132A70
		private static void InternalAddPlayer(NetworkPlayer player, CSteamID steamID)
		{
			Network.steamPlayers.Add(player, steamID);
			Network.connections.Add(player);
			Debug.Log(string.Concat(new object[]
			{
				"Add Player: ",
				player.id,
				" : ",
				steamID
			}));
			if (!(steamID == SteamUser.GetSteamID()))
			{
				NetworkEvents.TriggerPlayerConnected(player);
				return;
			}
			Network.player = player;
			if (Network.isServer)
			{
				Network.peerType = NetworkPeerMode.Server;
				NetworkEvents.TriggerServerInitialized();
				return;
			}
			Network.peerType = NetworkPeerMode.Client;
			NetworkEvents.TriggerConnectedToServer();
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x00134904 File Offset: 0x00132B04
		internal static void RemovePlayer(NetworkPlayer player, DisconnectInfo info)
		{
			if (!Network.connections.Contains(player))
			{
				Debug.LogError("Cannot remove player that isn't connected.");
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(5);
			stream.WriteUshort(player.id);
			stream.WriteByte((byte)info);
			Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.All, stream, SendType.ReliableBuffered, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x00134968 File Offset: 0x00132B68
		internal static void ReceiveRemovePlayer(BitStream stream, NetworkPlayer sender)
		{
			if (sender.isClient)
			{
				Debug.LogError("Client cannot Player Disconnect.");
				return;
			}
			ushort id = stream.ReadUshort();
			DisconnectInfo info = (DisconnectInfo)stream.ReadByte();
			Network.InternalRemovePlayer(new NetworkPlayer(id), info);
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x001349A4 File Offset: 0x00132BA4
		private static void InternalRemovePlayer(NetworkPlayer player, DisconnectInfo info)
		{
			CSteamID csteamID = Network.steamPlayers.Forward[player];
			Debug.Log(string.Concat(new object[]
			{
				"Remove Player: ",
				player.id,
				" : ",
				csteamID,
				" : ",
				info
			}));
			Network.connections.Remove(player);
			if (Network.admins.Contains(player))
			{
				Network.admins.Remove(player);
			}
			if (csteamID == SteamUser.GetSteamID())
			{
				Network.ReceiveDisconnect(info);
			}
			else
			{
				NetworkEvents.TriggerPlayerDisconnect(player, info);
			}
			if (Network.isServer)
			{
				Network.DestroyPlayerObjects(player);
			}
			Network.steamPlayers.Remove(player);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x00134A64 File Offset: 0x00132C64
		public static void DestroyPlayerObjects(NetworkPlayer player)
		{
			Listionary<ushort, NetworkView> idView = NetworkView.IdView;
			for (int i = 0; i < idView.Count; i++)
			{
				NetworkView networkView = idView.List[i];
				if (networkView.owner == player)
				{
					Network.Destroy(networkView);
				}
			}
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00134AAC File Offset: 0x00132CAC
		public static void AddAdmin(NetworkPlayer player)
		{
			if (Network.isClient)
			{
				Debug.LogError("Client cannot Add Admin.");
				return;
			}
			if (!Network.connections.Contains(player) || Network.admins.Contains(player))
			{
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(6);
			stream.WriteUshort(player.id);
			Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.All, stream, SendType.ReliableBuffered, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00134B1C File Offset: 0x00132D1C
		internal static void ReceiveAddAdmin(BitStream stream, NetworkPlayer sender)
		{
			if (sender.isClient)
			{
				Debug.LogError("Client cannot Add Admin.");
				return;
			}
			ushort id = stream.ReadUshort();
			NetworkPlayer item = new NetworkPlayer(id);
			if (!Network.admins.Contains(item))
			{
				Network.admins.Add(item);
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00134B64 File Offset: 0x00132D64
		public static void RemoveAdmin(NetworkPlayer player)
		{
			if (Network.isClient)
			{
				Debug.LogError("Client cannot Remove Admin.");
				return;
			}
			if (!Network.connections.Contains(player) || !Network.admins.Contains(player))
			{
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(7);
			stream.WriteUshort(player.id);
			Singleton<NetworkManager>.Instance.SendPacket(RPCTarget.All, stream, SendType.ReliableBuffered, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x00134BD4 File Offset: 0x00132DD4
		internal static void ReceiveRemoveAdmin(BitStream stream, NetworkPlayer sender)
		{
			if (sender.isClient)
			{
				Debug.LogError("Client cannot Remove Admin.");
				return;
			}
			ushort id = stream.ReadUshort();
			NetworkPlayer item = new NetworkPlayer(id);
			if (Network.admins.Contains(item))
			{
				Network.admins.Remove(item);
			}
		}

		// Token: 0x04001D81 RID: 7553
		private static int _maxConnections = 10;

		// Token: 0x04001D82 RID: 7554
		private static ServerType _serverType = ServerType.Public;

		// Token: 0x04001D83 RID: 7555
		private static string _password;

		// Token: 0x04001D84 RID: 7556
		public const int MAX_PASSWORD_LENGTH = 40;

		// Token: 0x04001D85 RID: 7557
		private static string _version;

		// Token: 0x04001D86 RID: 7558
		private static string _serverName;

		// Token: 0x04001D87 RID: 7559
		private static string _gameName;

		// Token: 0x04001D88 RID: 7560
		private static string _comment;

		// Token: 0x04001D89 RID: 7561
		public static readonly List<NetworkPlayer> connections = new List<NetworkPlayer>();

		// Token: 0x04001D8A RID: 7562
		public static readonly Map<NetworkPlayer, CSteamID> steamPlayers = new Map<NetworkPlayer, CSteamID>();

		// Token: 0x04001D8B RID: 7563
		public static readonly List<NetworkPlayer> admins = new List<NetworkPlayer>();

		// Token: 0x04001D8C RID: 7564
		private static readonly List<NetworkView> networkViewsForCount = new List<NetworkView>(1);

		// Token: 0x04001D8D RID: 7565
		private static bool usingLifeCycles = false;

		// Token: 0x04001D8E RID: 7566
		private static readonly List<INetworkLifeCycle> lifeCycles = new List<INetworkLifeCycle>(1);

		// Token: 0x04001D8F RID: 7567
		private static readonly ushort[] ids = new ushort[256];
	}
}
