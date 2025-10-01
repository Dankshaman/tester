using System;
using Dissonance.Networking;
using Steamworks;

// Token: 0x02000006 RID: 6
public class SteamVoiceP2PServer : BaseServer<SteamVoiceP2PServer, SteamVoiceP2PClient, SteamVoiceP2PPeer>
{
	// Token: 0x0600002F RID: 47 RVA: 0x00002F83 File Offset: 0x00001183
	public SteamVoiceP2PServer(SteamVoiceP2PCommsNetwork network)
	{
		this._network = network;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002F92 File Offset: 0x00001192
	public override void Connect()
	{
		base.Connect();
		EventManager.OnPlayersAdd += this.EventManager_OnPlayersAdd;
		EventManager.OnPlayersRemove += this.EventManager_OnPlayersRemove;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002FBC File Offset: 0x000011BC
	public override void Disconnect()
	{
		base.Disconnect();
		EventManager.OnPlayersAdd -= this.EventManager_OnPlayersAdd;
		EventManager.OnPlayersRemove -= this.EventManager_OnPlayersRemove;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000025B8 File Offset: 0x000007B8
	private void EventManager_OnPlayersAdd(PlayerState playerState)
	{
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002FE6 File Offset: 0x000011E6
	private void EventManager_OnPlayersRemove(PlayerState playerState)
	{
		this.DisconnectClient(new SteamVoiceP2PPeer(playerState.cSteamId));
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002FF9 File Offset: 0x000011F9
	public void DisconnectClient(SteamVoiceP2PPeer peer)
	{
		base.ClientDisconnected(peer);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void ReadMessages()
	{
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003002 File Offset: 0x00001202
	protected override void SendReliable(SteamVoiceP2PPeer connection, ArraySegment<byte> packet)
	{
		Singleton<SteamP2PManager>.Instance.SendP2PPacket(connection.steamID, packet.Array, (uint)packet.Count, EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel.VoiceClient);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003025 File Offset: 0x00001225
	protected override void SendUnreliable(SteamVoiceP2PPeer connection, ArraySegment<byte> packet)
	{
		Singleton<SteamP2PManager>.Instance.SendP2PPacket(connection.steamID, packet.Array, (uint)packet.Count, EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.VoiceClient);
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003048 File Offset: 0x00001248
	public void ReceivePacket(SteamVoiceP2PPeer source, ArraySegment<byte> data)
	{
		base.NetworkReceivedPacket(source, data);
	}

	// Token: 0x0400000B RID: 11
	private readonly SteamVoiceP2PCommsNetwork _network;
}
