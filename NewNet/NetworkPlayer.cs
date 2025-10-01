using System;
using Steamworks;

namespace NewNet
{
	// Token: 0x020003AC RID: 940
	public struct NetworkPlayer : IEquatable<NetworkPlayer>
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x00136D8B File Offset: 0x00134F8B
		// (set) Token: 0x06002C51 RID: 11345 RVA: 0x00136D93 File Offset: 0x00134F93
		public ushort id { get; private set; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x00136D9C File Offset: 0x00134F9C
		public bool isServer
		{
			get
			{
				return NetworkPlayer.IsServer(this);
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x00136DA9 File Offset: 0x00134FA9
		public bool isAdmin
		{
			get
			{
				return NetworkPlayer.IsAdmin(this);
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06002C54 RID: 11348 RVA: 0x00136DB6 File Offset: 0x00134FB6
		public bool isClient
		{
			get
			{
				return NetworkPlayer.IsClient(this);
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06002C55 RID: 11349 RVA: 0x00136DC3 File Offset: 0x00134FC3
		public bool isMe
		{
			get
			{
				return NetworkPlayer.IsMe(this);
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x00136DD0 File Offset: 0x00134FD0
		public CSteamID steamID
		{
			get
			{
				CSteamID result;
				Network.steamPlayers.TryGet(this, out result);
				return result;
			}
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00136DF1 File Offset: 0x00134FF1
		public NetworkPlayer(ushort id)
		{
			this.id = id;
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x00136DFA File Offset: 0x00134FFA
		public bool IsValid()
		{
			return this.id > 0;
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x00136E05 File Offset: 0x00135005
		public static NetworkPlayer GetServerPlayer()
		{
			return new NetworkPlayer(1);
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00136E0D File Offset: 0x0013500D
		public static NetworkPlayer GetMe()
		{
			return Network.player;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x00136E14 File Offset: 0x00135014
		private static bool IsServer(NetworkPlayer networkPlayer)
		{
			return networkPlayer.id == 1;
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x00136E20 File Offset: 0x00135020
		private static bool IsAdmin(NetworkPlayer networkPlayer)
		{
			return NetworkPlayer.IsServer(networkPlayer) || Network.admins.Contains(networkPlayer);
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x00136E37 File Offset: 0x00135037
		private static bool IsClient(NetworkPlayer networkPlayer)
		{
			return !NetworkPlayer.IsServer(networkPlayer);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x00136E42 File Offset: 0x00135042
		private static bool IsMe(NetworkPlayer networkPlayer)
		{
			return networkPlayer == NetworkPlayer.GetMe();
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x00136E4F File Offset: 0x0013504F
		public bool Equals(NetworkPlayer obj)
		{
			return this == obj;
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x00136E60 File Offset: 0x00135060
		public override bool Equals(object obj)
		{
			if (obj is NetworkPlayer)
			{
				NetworkPlayer y = (NetworkPlayer)obj;
				return this == y;
			}
			return false;
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x00136E8C File Offset: 0x0013508C
		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x00136EA7 File Offset: 0x001350A7
		public static bool operator ==(NetworkPlayer x, NetworkPlayer y)
		{
			return x.id == y.id;
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x00136EB9 File Offset: 0x001350B9
		public static bool operator !=(NetworkPlayer x, NetworkPlayer y)
		{
			return x.id != y.id;
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x00136ED0 File Offset: 0x001350D0
		public override string ToString()
		{
			return "Network Player: " + this.id.ToString();
		}

		// Token: 0x04001DFA RID: 7674
		public const ushort SERVER_ID = 1;
	}
}
