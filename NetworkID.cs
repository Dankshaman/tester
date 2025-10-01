using System;
using NewNet;

// Token: 0x020001BE RID: 446
public static class NetworkID
{
	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06001644 RID: 5700 RVA: 0x0009AC29 File Offset: 0x00098E29
	public static int ID
	{
		get
		{
			if (NetworkID.id == -1 && Network.peerType != NetworkPeerMode.Disconnected)
			{
				NetworkID.id = NetworkID.IDFromNetworkPlayer(Network.player);
			}
			return NetworkID.id;
		}
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x0009AC4E File Offset: 0x00098E4E
	public static void OverrideID(int newID)
	{
		NetworkID.id = newID;
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x0009AC56 File Offset: 0x00098E56
	public static void ResetID()
	{
		NetworkID.id = -1;
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x0009AC5E File Offset: 0x00098E5E
	public static int IDFromNetworkPlayer(NetworkPlayer NP)
	{
		return (int)NP.id;
	}

	// Token: 0x04000CA5 RID: 3237
	private static int id = -1;
}
