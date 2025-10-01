using System;
using System.Collections.Generic;
using Dissonance.Networking;
using Steamworks;

// Token: 0x02000003 RID: 3
public class SteamVoiceP2PClient : BaseClient<SteamVoiceP2PServer, SteamVoiceP2PClient, SteamVoiceP2PPeer>
{
	// Token: 0x06000005 RID: 5 RVA: 0x0000259C File Offset: 0x0000079C
	public SteamVoiceP2PClient(ICommsNetworkState network) : base(network)
	{
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000025B0 File Offset: 0x000007B0
	public override void Connect()
	{
		base.Connected();
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void ReadMessages()
	{
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000025BA File Offset: 0x000007BA
	protected override void SendReliable(ArraySegment<byte> packet)
	{
		Singleton<SteamP2PManager>.Instance.SendP2PPacketServer(packet.Array, (uint)packet.Count, EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel.VoiceServer);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000025D8 File Offset: 0x000007D8
	protected override void SendReliableP2P(List<ClientInfo<SteamVoiceP2PPeer?>> destinations, ArraySegment<byte> packet)
	{
		for (int i = destinations.Count - 1; i >= 0; i--)
		{
			ClientInfo<SteamVoiceP2PPeer?> clientInfo = destinations[i];
			if (clientInfo.Connection != null)
			{
				CSteamID steamID = clientInfo.Connection.Value.steamID;
				Singleton<SteamP2PManager>.Instance.SendP2PPacket(steamID, packet.Array, (uint)packet.Count, EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel.VoiceClient);
				destinations.RemoveAt(i);
			}
		}
		base.SendReliableP2P(destinations, packet);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002652 File Offset: 0x00000852
	protected override void SendUnreliable(ArraySegment<byte> packet)
	{
		Singleton<SteamP2PManager>.Instance.SendP2PPacketServer(packet.Array, (uint)packet.Count, EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.VoiceServer);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002670 File Offset: 0x00000870
	protected override void SendUnreliableP2P(List<ClientInfo<SteamVoiceP2PPeer?>> destinations, ArraySegment<byte> packet)
	{
		for (int i = destinations.Count - 1; i >= 0; i--)
		{
			ClientInfo<SteamVoiceP2PPeer?> clientInfo = destinations[i];
			if (clientInfo.Connection != null)
			{
				CSteamID steamID = clientInfo.Connection.Value.steamID;
				Singleton<SteamP2PManager>.Instance.SendP2PPacket(steamID, packet.Array, (uint)packet.Count, EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.VoiceClient);
				destinations.RemoveAt(i);
			}
		}
		base.SendUnreliableP2P(destinations, packet);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000026EC File Offset: 0x000008EC
	protected override void OnServerAssignedSessionId(uint session, ushort id)
	{
		base.OnServerAssignedSessionId(session, id);
		ArraySegment<byte> arraySegment = new ArraySegment<byte>(BaseClient<SteamVoiceP2PServer, SteamVoiceP2PClient, SteamVoiceP2PPeer>.WriteHandshakeP2P(session, id));
		Singleton<SteamP2PManager>.Instance.SendP2PPacketOthers(arraySegment.Array, (uint)arraySegment.Count, EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel.VoiceClient);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000272C File Offset: 0x0000092C
	public void ReceivePacket(SteamVoiceP2PPeer source, ArraySegment<byte> data)
	{
		ushort? num = base.NetworkReceivedPacket(data);
		if (num != null)
		{
			base.ReceiveHandshakeP2P(num.Value, source);
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002758 File Offset: 0x00000958
	protected override void OnMetClient(ClientInfo<SteamVoiceP2PPeer?> client)
	{
		base.OnMetClient(client);
		this.clients.Add(client);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002770 File Offset: 0x00000970
	public SteamVoiceP2PPeer? PlayerNameToPeer(string PlayerName)
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			ClientInfo<SteamVoiceP2PPeer?> clientInfo = this.clients[i];
			if (clientInfo.PlayerName == PlayerName)
			{
				return clientInfo.Connection;
			}
		}
		return null;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000027C0 File Offset: 0x000009C0
	public string PeerToPlayerName(SteamVoiceP2PPeer peer)
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			ClientInfo<SteamVoiceP2PPeer?> clientInfo = this.clients[i];
			if (clientInfo.Connection != null && clientInfo.Connection.Value.Equals(peer))
			{
				return clientInfo.PlayerName;
			}
		}
		return null;
	}

	// Token: 0x04000001 RID: 1
	private List<ClientInfo<SteamVoiceP2PPeer?>> clients = new List<ClientInfo<SteamVoiceP2PPeer?>>();
}
