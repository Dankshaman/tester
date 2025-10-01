using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewNet;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class ServerOptions : NetworkSingleton<ServerOptions>, INotifyPropertyChanged
{
	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x000BE990 File Offset: 0x000BCB90
	// (set) Token: 0x06001BA6 RID: 7078 RVA: 0x000BE997 File Offset: 0x000BCB97
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public int MaxConnections
	{
		get
		{
			return Network.maxConnections;
		}
		set
		{
			if (Network.maxConnections != value)
			{
				Network.maxConnections = value;
				NetworkSingleton<ServerOptions>.Instance.DirtySync("MaxConnections");
			}
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x000BE9B6 File Offset: 0x000BCBB6
	// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x000BE9BD File Offset: 0x000BCBBD
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public string ServerName
	{
		get
		{
			return Network.serverName;
		}
		set
		{
			if (Network.serverName != value)
			{
				Network.serverName = value;
				NetworkSingleton<ServerOptions>.Instance.DirtySync("ServerName");
			}
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x000BE9E1 File Offset: 0x000BCBE1
	// (set) Token: 0x06001BAA RID: 7082 RVA: 0x000BE9E8 File Offset: 0x000BCBE8
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public ServerType ServerType
	{
		get
		{
			return Network.serverType;
		}
		set
		{
			if (Network.serverType != value)
			{
				Network.serverType = value;
				NetworkSingleton<ServerOptions>.Instance.DirtySync("ServerType");
			}
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x000BEA07 File Offset: 0x000BCC07
	// (set) Token: 0x06001BAC RID: 7084 RVA: 0x000BEA0E File Offset: 0x000BCC0E
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public string Password
	{
		get
		{
			return Network.password;
		}
		set
		{
			if (Network.password != value)
			{
				Network.password = value;
				NetworkSingleton<ServerOptions>.Instance.DirtySync("Password");
			}
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06001BAD RID: 7085 RVA: 0x000BEA32 File Offset: 0x000BCC32
	// (set) Token: 0x06001BAE RID: 7086 RVA: 0x000BEA44 File Offset: 0x000BCC44
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public bool LookingForPlayers
	{
		get
		{
			return ServerOptions.GetNetworkComment(Network.comment).LookingForPlayers;
		}
		set
		{
			if (ServerOptions.GetNetworkComment(Network.comment).LookingForPlayers != value)
			{
				ServerOptions.NetworkComment networkComment = ServerOptions.GetNetworkComment(Network.comment);
				networkComment.LookingForPlayers = value;
				Network.comment = Json.GetJson(networkComment, true);
				NetworkSingleton<ServerOptions>.Instance.DirtySync("LookingForPlayers");
				this.OnPropertyChanged("LookingForPlayers");
			}
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06001BAF RID: 7087 RVA: 0x000BEA99 File Offset: 0x000BCC99
	// (set) Token: 0x06001BB0 RID: 7088 RVA: 0x000BEAA1 File Offset: 0x000BCCA1
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public ServerOptions.PhysicsMode Physics
	{
		get
		{
			return this._physics;
		}
		set
		{
			if (this._physics != value)
			{
				this._physics = value;
				NetworkSingleton<ServerOptions>.Instance.DirtySync("Physics");
				if (Network.sender == Network.player)
				{
					PlayerPrefs.SetInt("ServerOptions.Physics", (int)value);
				}
			}
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x000BEADE File Offset: 0x000BCCDE
	public static bool isPhysicsLock
	{
		get
		{
			return NetworkSingleton<ServerOptions>.Instance.Physics == ServerOptions.PhysicsMode.Lock;
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x000BEAED File Offset: 0x000BCCED
	public static bool isPhysicsSemi
	{
		get
		{
			return NetworkSingleton<ServerOptions>.Instance.Physics == ServerOptions.PhysicsMode.Semi;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x000BEAFC File Offset: 0x000BCCFC
	public static bool isPhysicsFull
	{
		get
		{
			return NetworkSingleton<ServerOptions>.Instance.Physics == ServerOptions.PhysicsMode.Full;
		}
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x000BEB0C File Offset: 0x000BCD0C
	public static ServerOptions.NetworkComment GetNetworkComment(string comment)
	{
		ServerOptions.NetworkComment result;
		if (string.IsNullOrEmpty(comment))
		{
			result = new ServerOptions.NetworkComment();
		}
		else
		{
			try
			{
				result = Json.Load<ServerOptions.NetworkComment>(comment);
			}
			catch (Exception)
			{
				result = new ServerOptions.NetworkComment();
			}
		}
		return result;
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x000BEB50 File Offset: 0x000BCD50
	private void Start()
	{
		if (PlayerPrefs.HasKey("ServerOptions.Physics"))
		{
			this._physics = (ServerOptions.PhysicsMode)PlayerPrefs.GetInt("ServerOptions.Physics");
		}
		NetworkEvents.OnSettingsChange += this.NetworkEvents_OnSettingsChange;
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x000BEB7F File Offset: 0x000BCD7F
	private void OnDestroy()
	{
		NetworkEvents.OnSettingsChange -= this.NetworkEvents_OnSettingsChange;
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x000BEB92 File Offset: 0x000BCD92
	private void NetworkEvents_OnSettingsChange()
	{
		NetworkSingleton<ServerOptions>.Instance.DirtySync(null);
	}

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06001BB8 RID: 7096 RVA: 0x000BEBA0 File Offset: 0x000BCDA0
	// (remove) Token: 0x06001BB9 RID: 7097 RVA: 0x000BEBD8 File Offset: 0x000BCDD8
	public event PropertyChangedEventHandler PropertyChanged;

	// Token: 0x06001BBA RID: 7098 RVA: 0x000BEC0D File Offset: 0x000BCE0D
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
		if (propertyChanged == null)
		{
			return;
		}
		propertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	// Token: 0x04001173 RID: 4467
	private ServerOptions.PhysicsMode _physics;

	// Token: 0x04001174 RID: 4468
	private const string PhysicsPlayerPref = "ServerOptions.Physics";

	// Token: 0x020006BC RID: 1724
	public enum PhysicsMode
	{
		// Token: 0x04002925 RID: 10533
		Full,
		// Token: 0x04002926 RID: 10534
		Semi,
		// Token: 0x04002927 RID: 10535
		Lock
	}

	// Token: 0x020006BD RID: 1725
	public class NetworkComment
	{
		// Token: 0x04002928 RID: 10536
		public bool LookingForPlayers = true;
	}
}
