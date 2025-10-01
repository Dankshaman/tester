using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using NewNet;
using Steamworks;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class SteamP2PManager : Singleton<SteamP2PManager>
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x000C7521 File Offset: 0x000C5721
	public static bool IsReliableChannel(SteamP2PManager.P2PChannel p2pChannel)
	{
		return p2pChannel == SteamP2PManager.P2PChannel.Reliable || p2pChannel == SteamP2PManager.P2PChannel.ReliableTick;
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x000C752C File Offset: 0x000C572C
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		base.StartCoroutine(this.P2PHeartBeat());
		this.m_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.CallbackP2PSessionRequest));
		this.m_P2PSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.CallbackP2PSessionConnectFail));
		NetworkEvents.OnPlayerConnected += this.PlayerConnected;
		NetworkEvents.OnPlayerDisconnected += this.PlayerDisconnected;
		NetworkEvents.OnDisconnectedFromServer += this.DisconnectedFromServer;
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x000C75AF File Offset: 0x000C57AF
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.PlayerConnected;
		NetworkEvents.OnPlayerDisconnected -= this.PlayerDisconnected;
		NetworkEvents.OnDisconnectedFromServer -= this.DisconnectedFromServer;
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000C75E4 File Offset: 0x000C57E4
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		this.ReadAvailablePackets();
		this.OnScreenDebug();
		this.UpdateNetworkIcons();
		this.CheckForTimeouts();
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000C7608 File Offset: 0x000C5808
	private void CheckForTimeouts()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			SteamP2PManager.PlayerInfo playerInfo = this.players.List[i];
			if (Time.time > playerInfo.lastPacketTime + 21f)
			{
				NetworkPlayer player;
				if (Network.steamPlayers.TryGet(playerInfo.steamID, out player))
				{
					if (Network.isServer)
					{
						Network.RemovePlayer(player, DisconnectInfo.TimeoutServer);
					}
					else if (player.isServer)
					{
						Network.ReceiveDisconnect(DisconnectInfo.TimeoutClient);
					}
				}
				else
				{
					this.CloseP2PSession(playerInfo.steamID);
				}
			}
		}
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x000C7691 File Offset: 0x000C5891
	private void PlayerConnected(NetworkPlayer player)
	{
		this.OpenP2PSession(player.steamID);
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000C76A0 File Offset: 0x000C58A0
	private void PlayerDisconnected(NetworkPlayer player, DisconnectInfo info)
	{
		this.CloseP2PSession(player.steamID);
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x000C76AF File Offset: 0x000C58AF
	private void DisconnectedFromServer(DisconnectInfo info)
	{
		this.CloseAllP2PSessions();
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000C76B8 File Offset: 0x000C58B8
	public bool IsP2PConnectedToServer()
	{
		return this.IsP2PConnected(NetworkPlayer.GetServerPlayer().steamID);
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000C76D8 File Offset: 0x000C58D8
	public bool IsP2PConnectedToAll()
	{
		List<NetworkPlayer> connections = Network.connections;
		for (int i = 0; i < connections.Count; i++)
		{
			CSteamID steamID = connections[i].steamID;
			if (!this.IsP2PConnected(steamID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000C7718 File Offset: 0x000C5918
	public bool IsP2PConnected(CSteamID steamID)
	{
		SteamP2PManager.PlayerInfo playerInfo;
		return steamID == SteamUser.GetSteamID() || (this.players.TryGetValue(steamID, out playerInfo) && playerInfo.state > SteamP2PManager.PlayerInfo.State.NotConnected);
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000C7750 File Offset: 0x000C5950
	public void SendP2PPacketServer(byte[] data, uint length, EP2PSend sendType = EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel channel = SteamP2PManager.P2PChannel.Reliable)
	{
		this.SendP2PPacket(NetworkPlayer.GetServerPlayer().steamID, data, length, sendType, channel);
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000C7778 File Offset: 0x000C5978
	public void SendP2PPacketOthers(byte[] data, uint length, EP2PSend sendType = EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel channel = SteamP2PManager.P2PChannel.Reliable)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			SteamP2PManager.PlayerInfo playerInfo = this.players.List[i];
			if (playerInfo.state == SteamP2PManager.PlayerInfo.State.Connected)
			{
				this.SendP2PPacket(playerInfo.steamID, data, length, sendType, channel);
			}
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000C77C7 File Offset: 0x000C59C7
	public void SendP2PPacketAll(byte[] data, uint length, EP2PSend sendType = EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel channel = SteamP2PManager.P2PChannel.Reliable)
	{
		this.ProcessPacket(data, length, SteamUser.GetSteamID(), channel);
		this.SendP2PPacketOthers(data, length, sendType, channel);
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000C77E3 File Offset: 0x000C59E3
	public void SendP2PPacket(CSteamID receiver, byte[] data, EP2PSend sendType)
	{
		this.SendP2PPacket(receiver, data, (uint)data.Length, sendType, SteamP2PManager.P2PChannel.Reliable);
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000C77F4 File Offset: 0x000C59F4
	public void SendP2PPacket(CSteamID receiver, byte[] data, uint length, EP2PSend sendType = EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel channel = SteamP2PManager.P2PChannel.Reliable)
	{
		if (receiver == SteamUser.GetSteamID())
		{
			this.ProcessPacket(data, length, SteamUser.GetSteamID(), channel);
			return;
		}
		this.sendBytesCount += (int)length;
		this.sendPacketsCount++;
		if (SteamP2PManager.IsReliableChannel(channel) && length >= 1000000U)
		{
			if (!SteamNetworking.SendP2PPacket(receiver, data, 1000000U, sendType, (int)channel))
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Error sending Steam P2P Packet! Length: ",
					length,
					" Receiver: ",
					receiver
				}));
				return;
			}
			int num = (int)(length / 1000000U);
			for (int i = 0; i < num; i++)
			{
				int num2 = i + 1;
				int num3 = 1000000 * num2;
				int num4 = (int)((num2 < num) ? 1000000U : (length - (uint)num3));
				Array.Copy(data, num3, SteamP2PManager.P2Pbytes, 0, num4);
				UnityEngine.Debug.Log(num3 + " : " + num4);
				if (!SteamNetworking.SendP2PPacket(receiver, SteamP2PManager.P2Pbytes, (uint)num4, sendType, (int)channel))
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Error sending Steam P2P Packet! Length: ",
						length,
						" Receiver: ",
						receiver
					}));
					return;
				}
			}
			return;
		}
		else
		{
			if (!SteamNetworking.SendP2PPacket(receiver, data, length, sendType, (int)channel))
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Error sending Steam P2P Packet! Length: ",
					length,
					" Receiver: ",
					receiver
				}));
				return;
			}
			return;
		}
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000C797F File Offset: 0x000C5B7F
	public void SendP2PPacket(RPCTarget target, byte[] data, uint length, EP2PSend sendType, SteamP2PManager.P2PChannel channel = SteamP2PManager.P2PChannel.Reliable)
	{
		switch (target)
		{
		case RPCTarget.All:
			this.SendP2PPacketAll(data, length, sendType, channel);
			return;
		case RPCTarget.Others:
			this.SendP2PPacketOthers(data, length, sendType, channel);
			return;
		case RPCTarget.Server:
			this.SendP2PPacketServer(data, length, sendType, channel);
			return;
		default:
			return;
		}
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000C79BA File Offset: 0x000C5BBA
	private IEnumerator P2PHeartBeat()
	{
		for (;;)
		{
			yield return this.heartBeatWait;
			for (int i = 0; i < this.players.Count; i++)
			{
				this.SendP2PPacket(this.players.List[i].steamID, this.heartBeatPacket, 1U, EP2PSend.k_EP2PSendUnreliable, SteamP2PManager.P2PChannel.Unreliable);
			}
		}
		yield break;
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000C79CC File Offset: 0x000C5BCC
	private void ReadAvailablePackets()
	{
		this.countFrame++;
		this.readPacketStopWatch.Start();
		try
		{
			for (int i = 0; i < SteamP2PManager.P2PChannels; i++)
			{
				int num = 0;
				uint num2;
				while (SteamNetworking.IsP2PPacketAvailable(out num2, i))
				{
					SteamP2PManager.P2PChannel p2PChannel = (SteamP2PManager.P2PChannel)i;
					uint num3;
					CSteamID csteamID;
					bool flag = SteamNetworking.ReadP2PPacket(SteamP2PManager.P2Pbytes, num2, out num3, out csteamID, i);
					this.recieveBytesCount += (int)num3;
					this.recievePacketsCount++;
					if (!csteamID.IsValid() || !flag)
					{
						UnityEngine.Debug.LogWarning("RecievedMsg is broken - sender " + (csteamID.IsValid() ? "valid" : "invalid") + ", hasData: " + flag.ToString());
					}
					else
					{
						SteamP2PManager.PlayerInfo playerInfo;
						if (!this.players.TryGetValue(csteamID, out playerInfo))
						{
							if (!Network.isServer || !Singleton<SteamLobbyManager>.Instance.InCurrentLobby(csteamID))
							{
								UnityEngine.Debug.Log("Receiver packets from non accepted player.");
								this.CloseP2PSession(csteamID);
								continue;
							}
							UnityEngine.Debug.Log("Added non request player.");
							this.OpenP2PSession(csteamID);
							playerInfo = this.players.Dictionary[csteamID];
						}
						playerInfo.lastPacketTime = Time.time;
						if (playerInfo.state == SteamP2PManager.PlayerInfo.State.NotConnected)
						{
							UnityEngine.Debug.Log("Received packet peer: " + csteamID);
							playerInfo.state = SteamP2PManager.PlayerInfo.State.Received;
						}
						if (Network.isServer && !playerInfo.authenticated)
						{
							if (num3 <= 1U)
							{
								continue;
							}
							try
							{
								SteamP2PManager.ConnectionAuthentication connectionAuthentication = Json.Load<SteamP2PManager.ConnectionAuthentication>(SteamP2PManager.P2Pbytes, (int)num2, false);
								string text = Network.password;
								if (text != null)
								{
									text = text.Trim();
								}
								string text2 = connectionAuthentication.password;
								if (text2 != null)
								{
									text2 = text2.Trim();
								}
								bool flag2 = string.IsNullOrEmpty(text) || text2 == text;
								bool flag3 = connectionAuthentication.version == Network.version;
								bool flag4 = Network.connections.Count < Network.maxConnections;
								if (flag3 && flag4 && flag2)
								{
									UnityEngine.Debug.Log("Authenticated peer: " + csteamID);
									playerInfo.authenticated = true;
								}
								else
								{
									UnityEngine.Debug.Log(string.Concat(new object[]
									{
										"Didn't authenticate peer: ",
										csteamID,
										" password: ",
										flag2.ToString(),
										" version: ",
										flag3.ToString(),
										" room: ",
										flag4.ToString()
									}));
									this.CloseP2PSession(csteamID);
									if (!flag3)
									{
										Singleton<SteamLobbyManager>.Instance.RemoveFromLobby(csteamID, SteamLobbyManager.LobbyMessageType.ConnectFailed, 5);
									}
									else if (!flag4)
									{
										Singleton<SteamLobbyManager>.Instance.RemoveFromLobby(csteamID, SteamLobbyManager.LobbyMessageType.ConnectFailed, 4);
									}
									else if (!flag2)
									{
										Singleton<SteamLobbyManager>.Instance.RemoveFromLobby(csteamID, SteamLobbyManager.LobbyMessageType.ConnectFailed, 3);
									}
								}
								continue;
							}
							catch (Exception)
							{
								UnityEngine.Debug.LogError("Couldn't deserialize ConnectionAuthenticate.");
								this.CloseP2PSession(csteamID);
								Singleton<SteamLobbyManager>.Instance.RemoveFromLobby(csteamID, SteamLobbyManager.LobbyMessageType.ConnectFailed, 1);
								continue;
							}
						}
						if (SteamP2PManager.IsReliableChannel(p2PChannel))
						{
							if (num3 == 1000000U)
							{
								if (!playerInfo.splitPackets.ContainsKey(p2PChannel))
								{
									UnityEngine.Debug.Log("Creat stream split packet");
									playerInfo.splitPackets[p2PChannel] = NetworkManager.GetStream();
									playerInfo.splitPackets[p2PChannel].Rewind();
								}
								UnityEngine.Debug.Log("Max Size, wait for next packet");
								playerInfo.splitPackets[p2PChannel].WriteBytes(SteamP2PManager.P2Pbytes, (int)num3);
								continue;
							}
							if (playerInfo.splitPackets.ContainsKey(p2PChannel))
							{
								UnityEngine.Debug.Log("Write not max: " + num3);
								BitStream bitStream = playerInfo.splitPackets[p2PChannel];
								bitStream.WriteBytes(SteamP2PManager.P2Pbytes, (int)num3);
								UnityEngine.Debug.Log("Final packet size: " + bitStream.GetWrittenSizeInBytes());
								this.ProcessPacket(bitStream.Buffer, (uint)bitStream.GetWrittenSizeInBytes(), csteamID, p2PChannel);
								NetworkManager.ReturnStream(bitStream);
								playerInfo.splitPackets.Remove(p2PChannel);
								continue;
							}
						}
						if (num3 == 1U)
						{
							byte b = SteamP2PManager.P2Pbytes[0];
							if (b == 1)
							{
								if (playerInfo.state != SteamP2PManager.PlayerInfo.State.Connected)
								{
									UnityEngine.Debug.Log("Ping to peer: " + csteamID);
									this.SendP2PPacket(csteamID, this.pingPacket, EP2PSend.k_EP2PSendReliable);
									continue;
								}
								continue;
							}
							else
							{
								if (b == 2)
								{
									UnityEngine.Debug.Log("Pong to peer: " + csteamID);
									this.SendP2PPacket(csteamID, this.pongPacket, EP2PSend.k_EP2PSendReliable);
									continue;
								}
								if (b == 3)
								{
									if (playerInfo.state == SteamP2PManager.PlayerInfo.State.Connected)
									{
										UnityEngine.Debug.Log("Already connected to this peer: " + csteamID);
										continue;
									}
									UnityEngine.Debug.Log("Connected to peer: " + csteamID);
									playerInfo.state = SteamP2PManager.PlayerInfo.State.Connected;
									SteamP2PManager.TriggerPlayerConnected(csteamID);
									P2PSessionState_t p2PSessionState_t = default(P2PSessionState_t);
									IPAddress ipaddress;
									if (SteamNetworking.GetP2PSessionState(csteamID, out p2PSessionState_t) && IPAddress.TryParse(p2PSessionState_t.m_nRemoteIP.ToString(), out ipaddress))
									{
										UnityEngine.Debug.Log(string.Concat(new object[]
										{
											"Begin DNS Id: ",
											csteamID,
											" Uint Ip: ",
											p2PSessionState_t.m_nRemoteIP,
											" Ip: ",
											ipaddress
										}));
										Dns.BeginGetHostEntry(ipaddress, new AsyncCallback(this.ProcessDNS), csteamID);
										continue;
									}
									continue;
								}
							}
						}
						this.ProcessPacket(SteamP2PManager.P2Pbytes, num3, csteamID, p2PChannel);
						num++;
						if (num > 100000)
						{
							Chat.LogError("Had to break out infinite loop reading Steam P2P packets.", true);
							break;
						}
					}
				}
			}
		}
		catch (Exception e)
		{
			Chat.LogException("reading Steam P2P packets", e, true, true);
		}
		this.readPacketStopWatch.Stop();
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000C7F88 File Offset: 0x000C6188
	private void ProcessPacket(byte[] data, uint msgSize, CSteamID sender, SteamP2PManager.P2PChannel channel)
	{
		try
		{
			NetworkPlayer sender2;
			if (!Network.steamPlayers.TryGet(sender, out sender2))
			{
				UnityEngine.Debug.Log("Receiving packets from player not connected: " + sender);
			}
			else if (channel == SteamP2PManager.P2PChannel.VoiceClient || channel == SteamP2PManager.P2PChannel.VoiceServer)
			{
				ArraySegment<byte> data2 = new ArraySegment<byte>(data, 0, (int)msgSize);
				if (channel != SteamP2PManager.P2PChannel.VoiceClient)
				{
					if (channel == SteamP2PManager.P2PChannel.VoiceServer)
					{
						if (SteamVoiceP2PCommsNetwork.Instance)
						{
							SteamVoiceP2PCommsNetwork.Instance.ReceivePacketServer(sender, data2);
						}
					}
				}
				else if (SteamVoiceP2PCommsNetwork.Instance)
				{
					SteamVoiceP2PCommsNetwork.Instance.ReceivePacketClient(sender, data2);
				}
			}
			else
			{
				Network.sender = sender2;
				switch (channel)
				{
				case SteamP2PManager.P2PChannel.Reliable:
				case SteamP2PManager.P2PChannel.Unreliable:
					Singleton<NetworkManager>.Instance.ReceivePacket(sender2, data, msgSize, channel == SteamP2PManager.P2PChannel.Reliable);
					break;
				case SteamP2PManager.P2PChannel.ReliableTick:
					Singleton<NetworkManager>.Instance.ReceiveReliableTickPacket(sender2, data, msgSize);
					break;
				case SteamP2PManager.P2PChannel.UnreliableTick:
					Singleton<NetworkManager>.Instance.ReceiveUnreliableTickPacket(sender2, data, msgSize);
					break;
				case SteamP2PManager.P2PChannel.UnreliablePointer:
					Singleton<NetworkManager>.Instance.ReceivePointerPacket(sender2, data, msgSize);
					break;
				case SteamP2PManager.P2PChannel.UnreliableTouch:
					Singleton<NetworkManager>.Instance.ReceiveUnreliableTouch(sender2, data, msgSize);
					break;
				case SteamP2PManager.P2PChannel.UnreliableTracked:
					Singleton<NetworkManager>.Instance.ReceiveUnreliableTracked(sender2, data, msgSize);
					break;
				}
				Network.sender = Network.player;
			}
		}
		catch (Exception e)
		{
			Chat.LogException("process Steam P2P packet", e, true, true);
		}
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000C80EC File Offset: 0x000C62EC
	private void OnScreenDebug()
	{
		this.debugTime += Time.deltaTime;
		if (this.debugTime >= this.debugInterval)
		{
			if (Debugging.bDebug)
			{
				if (this.debugInterval != 1f)
				{
					this.sendBytesCount = (int)((float)this.sendBytesCount / this.debugInterval);
					this.sendPacketsCount = (int)((float)this.sendPacketsCount / this.debugInterval);
					this.recieveBytesCount = (int)((float)this.recieveBytesCount / this.debugInterval);
					this.recievePacketsCount = (int)((float)this.recievePacketsCount / this.debugInterval);
				}
				string text = "";
				for (int i = 0; i < this.players.Count; i++)
				{
					CSteamID steamID = this.players.List[i].steamID;
					string friendPersonaName = SteamFriends.GetFriendPersonaName(steamID);
					P2PSessionState_t p2PSessionState_t = default(P2PSessionState_t);
					if (SteamNetworking.GetP2PSessionState(steamID, out p2PSessionState_t))
					{
						text = text + "\n[" + friendPersonaName + "]";
						if (p2PSessionState_t.m_bUsingRelay == 1)
						{
							text += " relay";
						}
						if (p2PSessionState_t.m_bConnecting == 1)
						{
							text += " connecting...";
						}
						if (p2PSessionState_t.m_bConnectionActive == 1)
						{
							text += " connected";
						}
						if (p2PSessionState_t.m_eP2PSessionError > 0)
						{
							text = string.Concat(new object[]
							{
								text,
								" error (",
								p2PSessionState_t.m_eP2PSessionError,
								")"
							});
						}
						if (this.IsConnectedRelay(steamID))
						{
							text += " dns relay";
						}
						if (p2PSessionState_t.m_nBytesQueuedForSend > 0)
						{
							text = text + " bytes queued: " + p2PSessionState_t.m_nBytesQueuedForSend;
						}
					}
					else
					{
						text = text + "\n" + friendPersonaName + " not connected";
					}
				}
				string text2 = string.Concat(new string[]
				{
					this.sendBytesCount.ToString(),
					" out bytes/s\n",
					this.sendPacketsCount.ToString(),
					" out packets/s\n",
					this.recieveBytesCount.ToString(),
					" in bytes/s\n",
					this.recievePacketsCount.ToString(),
					" in packets/s\n",
					((float)this.writePacketStopWatch.ElapsedMilliseconds / (float)this.countFrame).ToString("N3"),
					" (ms) write\n",
					((float)this.readPacketStopWatch.ElapsedMilliseconds / (float)this.countFrame).ToString("N3"),
					" (ms) read",
					text
				});
				Singleton<UIDebug>.Instance.Label.text = text2;
			}
			else
			{
				Singleton<UIDebug>.Instance.Label.text = "";
			}
			this.sendBytesCount = 0;
			this.sendPacketsCount = 0;
			this.recieveBytesCount = 0;
			this.recievePacketsCount = 0;
			this.debugTime = 0f;
			this.writePacketStopWatch.Reset();
			this.writePacketStopWatch.Stop();
			this.readPacketStopWatch.Reset();
			this.readPacketStopWatch.Stop();
			this.countFrame = 0;
		}
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000C8400 File Offset: 0x000C6600
	private void UpdateNetworkIcons()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			CSteamID steamID = this.players.List[i].steamID;
			this.UpdateConnectionUI(steamID);
		}
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000C8444 File Offset: 0x000C6644
	private void UpdateConnectionUI(CSteamID steamID)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromSteamID(steamID);
		if (playerState != null && playerState.ui != null)
		{
			SteamP2PManager.PlayerInfo playerInfo;
			if (this.players.TryGetValue(steamID, out playerInfo))
			{
				playerState.ui.Ping = playerInfo.smoothPing;
			}
			else
			{
				playerState.ui.Ping = -1;
			}
			P2PSessionState_t p2PSessionState_t = default(P2PSessionState_t);
			if (SteamNetworking.GetP2PSessionState(steamID, out p2PSessionState_t))
			{
				if (p2PSessionState_t.m_bConnectionActive == 1 && (p2PSessionState_t.m_bUsingRelay == 1 || this.IsConnectedRelay(steamID)))
				{
					playerState.ui.connection = UINameButton.Connection.BadConnection;
					return;
				}
				if (p2PSessionState_t.m_bConnectionActive == 1)
				{
					playerState.ui.connection = UINameButton.Connection.Connected;
					return;
				}
				if (p2PSessionState_t.m_bConnecting == 1)
				{
					playerState.ui.connection = UINameButton.Connection.Connecting;
					return;
				}
				playerState.ui.connection = UINameButton.Connection.Disconnected;
				return;
			}
			else
			{
				playerState.ui.connection = UINameButton.Connection.Disconnected;
			}
		}
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x000C8522 File Offset: 0x000C6722
	private bool IsConnectedRelay(CSteamID steamID)
	{
		return this.dnsPeers.ContainsKey(steamID) && this.dnsPeers[steamID].HostName.Contains("valve");
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x000C8554 File Offset: 0x000C6754
	private void ProcessDNS(IAsyncResult result)
	{
		CSteamID csteamID = (CSteamID)result.AsyncState;
		IPHostEntry iphostEntry = Dns.EndGetHostEntry(result);
		if (this.dnsPeers.ContainsKey(csteamID))
		{
			this.dnsPeers.Remove(csteamID);
		}
		this.dnsPeers.Add(csteamID, iphostEntry);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"End DNS Id: ",
			csteamID,
			" Host: ",
			iphostEntry.HostName,
			" Relay: ",
			this.IsConnectedRelay(csteamID).ToString()
		}));
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x000C85E8 File Offset: 0x000C67E8
	private void OpenP2PSession(CSteamID steamID)
	{
		if (!this.players.ContainsKey(steamID) && steamID != SteamUser.GetSteamID())
		{
			UnityEngine.Debug.Log("OpenP2PSession: " + steamID);
			this.players.Add(steamID, new SteamP2PManager.PlayerInfo(steamID));
			this.SendP2PPacket(steamID, this.heartBeatPacket, EP2PSend.k_EP2PSendReliable);
			if (Network.isClient && Singleton<SteamLobbyManager>.Instance.CurrentSteamIDOwner == steamID)
			{
				SteamP2PManager.ConnectionAuthentication obj = new SteamP2PManager.ConnectionAuthentication(Network.password, Network.version);
				this.SendP2PPacket(steamID, Json.GetBson(obj), EP2PSend.k_EP2PSendReliable);
			}
		}
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x000C867C File Offset: 0x000C687C
	public void CloseP2PSession(CSteamID steamID)
	{
		UnityEngine.Debug.Log("CloseP2PSession: " + steamID);
		if (this.players.ContainsKey(steamID))
		{
			this.players.Remove(steamID);
		}
		SteamNetworking.CloseP2PSessionWithUser(steamID);
		if (this.dnsPeers.ContainsKey(steamID))
		{
			this.dnsPeers.Remove(steamID);
		}
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000C86DC File Offset: 0x000C68DC
	private void CloseAllP2PSessions()
	{
		UnityEngine.Debug.Log("CloseAllP2PSessions");
		for (int i = 0; i < this.players.Count; i++)
		{
			SteamNetworking.CloseP2PSessionWithUser(this.players.List[i].steamID);
		}
		this.players.Clear();
		this.dnsPeers.Clear();
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000C873C File Offset: 0x000C693C
	protected string GetCurrentTime()
	{
		return string.Concat(new object[]
		{
			DateTime.Now.Hour,
			":",
			DateTime.Now.Minute,
			":",
			DateTime.Now.Second,
			",",
			DateTime.Now.Millisecond
		});
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000C87C0 File Offset: 0x000C69C0
	private void CallbackP2PSessionRequest(P2PSessionRequest_t request)
	{
		CSteamID steamIDRemote = request.m_steamIDRemote;
		UnityEngine.Debug.Log("CallbackP2PSessionRequest: " + steamIDRemote);
		if (GlobalBanList.BannedSteamIds.Contains(steamIDRemote.ToString()))
		{
			UnityEngine.Debug.Log("CallbackP2PSessionRequest banned: " + steamIDRemote);
			return;
		}
		if (!Singleton<SteamLobbyManager>.Instance.InCurrentLobby(steamIDRemote))
		{
			UnityEngine.Debug.Log("Unexpected CallbackP2PSessionRequest from: " + steamIDRemote);
			return;
		}
		if (SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote))
		{
			this.OpenP2PSession(steamIDRemote);
			return;
		}
		Chat.LogError("Failed to accept Steam P2P session with: " + steamIDRemote, true);
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x000C8864 File Offset: 0x000C6A64
	private void CallbackP2PSessionConnectFail(P2PSessionConnectFail_t fail)
	{
		CSteamID steamIDRemote = fail.m_steamIDRemote;
		string friendPersonaName = SteamFriends.GetFriendPersonaName(steamIDRemote);
		Chat.LogError("Steam P2P session failed with " + friendPersonaName + ".", true);
		UnityEngine.Debug.Log("Steam P2P session failed: " + steamIDRemote);
		NetworkPlayer player;
		if (Network.isServer && Network.steamPlayers.TryGet(steamIDRemote, out player))
		{
			Network.RemovePlayer(player, DisconnectInfo.TimeoutSteamServer);
		}
		if (steamIDRemote == Singleton<SteamLobbyManager>.Instance.CurrentSteamIDOwner)
		{
			if (Network.peerType == NetworkPeerMode.Disconnected)
			{
				NetworkEvents.TriggerFailedToConnect(ConnectFailedInfo.Failed);
			}
			Network.ReceiveDisconnect(DisconnectInfo.TimeoutSteamClient);
		}
		SteamP2PManager.PlayerInfo playerInfo;
		if (this.players.TryGetValue(steamIDRemote, out playerInfo))
		{
			playerInfo.state = SteamP2PManager.PlayerInfo.State.NotConnected;
		}
		if (this.dnsPeers.ContainsKey(steamIDRemote))
		{
			this.dnsPeers.Remove(steamIDRemote);
		}
	}

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06001D27 RID: 7463 RVA: 0x000C8920 File Offset: 0x000C6B20
	// (remove) Token: 0x06001D28 RID: 7464 RVA: 0x000C8954 File Offset: 0x000C6B54
	public static event SteamP2PManager.SteamPlayerConnected OnSteamPlayerConnected;

	// Token: 0x06001D29 RID: 7465 RVA: 0x000C8987 File Offset: 0x000C6B87
	public static void TriggerPlayerConnected(CSteamID steamID)
	{
		SteamP2PManager.SteamPlayerConnected onSteamPlayerConnected = SteamP2PManager.OnSteamPlayerConnected;
		if (onSteamPlayerConnected == null)
		{
			return;
		}
		onSteamPlayerConnected(steamID);
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000C76AF File Offset: 0x000C58AF
	private void OnApplicationQuit()
	{
		this.CloseAllP2PSessions();
	}

	// Token: 0x0400128B RID: 4747
	private Callback<P2PSessionRequest_t> m_P2PSessionRequest;

	// Token: 0x0400128C RID: 4748
	private Callback<P2PSessionConnectFail_t> m_P2PSessionConnectFail;

	// Token: 0x0400128D RID: 4749
	private const int SecondsToTimeout = 21;

	// Token: 0x0400128E RID: 4750
	public Listionary<CSteamID, SteamP2PManager.PlayerInfo> players = new Listionary<CSteamID, SteamP2PManager.PlayerInfo>();

	// Token: 0x0400128F RID: 4751
	private readonly Dictionary<CSteamID, IPHostEntry> dnsPeers = new Dictionary<CSteamID, IPHostEntry>();

	// Token: 0x04001290 RID: 4752
	public const EP2PSend DefaultSendType = EP2PSend.k_EP2PSendReliable;

	// Token: 0x04001291 RID: 4753
	private readonly WaitForSeconds heartBeatWait = new WaitForSeconds(1f);

	// Token: 0x04001292 RID: 4754
	private const byte heartBeatByte = 1;

	// Token: 0x04001293 RID: 4755
	private const byte pingByte = 2;

	// Token: 0x04001294 RID: 4756
	private const byte pongByte = 3;

	// Token: 0x04001295 RID: 4757
	private readonly byte[] heartBeatPacket = new byte[]
	{
		1
	};

	// Token: 0x04001296 RID: 4758
	private readonly byte[] pingPacket = new byte[]
	{
		2
	};

	// Token: 0x04001297 RID: 4759
	private readonly byte[] pongPacket = new byte[]
	{
		3
	};

	// Token: 0x04001298 RID: 4760
	private const int STEAM_RELIABLE_MAX_PACKET_SIZE = 1000000;

	// Token: 0x04001299 RID: 4761
	private static readonly byte[] P2Pbytes = new byte[10000000];

	// Token: 0x0400129A RID: 4762
	private static readonly int P2PChannels = Enum.GetNames(typeof(SteamP2PManager.P2PChannel)).Length;

	// Token: 0x0400129B RID: 4763
	private int recieveBytesCount;

	// Token: 0x0400129C RID: 4764
	private int recievePacketsCount;

	// Token: 0x0400129D RID: 4765
	private int sendBytesCount;

	// Token: 0x0400129E RID: 4766
	private int sendPacketsCount;

	// Token: 0x0400129F RID: 4767
	private float debugTime;

	// Token: 0x040012A0 RID: 4768
	private readonly float debugInterval = 1f;

	// Token: 0x040012A1 RID: 4769
	private int countFrame;

	// Token: 0x040012A2 RID: 4770
	public Stopwatch writePacketStopWatch = new Stopwatch();

	// Token: 0x040012A3 RID: 4771
	private readonly Stopwatch readPacketStopWatch = new Stopwatch();

	// Token: 0x020006CE RID: 1742
	public enum P2PChannel
	{
		// Token: 0x04002962 RID: 10594
		Reliable,
		// Token: 0x04002963 RID: 10595
		Unreliable,
		// Token: 0x04002964 RID: 10596
		ReliableTick,
		// Token: 0x04002965 RID: 10597
		UnreliableTick,
		// Token: 0x04002966 RID: 10598
		UnreliablePointer,
		// Token: 0x04002967 RID: 10599
		VoiceClient,
		// Token: 0x04002968 RID: 10600
		VoiceServer,
		// Token: 0x04002969 RID: 10601
		UnreliableTouch,
		// Token: 0x0400296A RID: 10602
		UnreliableTracked
	}

	// Token: 0x020006CF RID: 1743
	public class PlayerInfo
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x00179B12 File Offset: 0x00177D12
		// (set) Token: 0x06003CA5 RID: 15525 RVA: 0x00179B1A File Offset: 0x00177D1A
		public int ping
		{
			get
			{
				return this._ping;
			}
			set
			{
				this.prevPing = this._ping;
				this._ping = value;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x00179B2F File Offset: 0x00177D2F
		// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x00179B37 File Offset: 0x00177D37
		public int prevPing
		{
			get
			{
				return this._prevPing;
			}
			private set
			{
				this._prevPing = value;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x00179B40 File Offset: 0x00177D40
		public int smoothPing
		{
			get
			{
				if (this._prevPing == -1)
				{
					return this.ping;
				}
				return (this.ping + this.prevPing) / 2;
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00179B61 File Offset: 0x00177D61
		public PlayerInfo(CSteamID steamID)
		{
			this.steamID = steamID;
		}

		// Token: 0x0400296B RID: 10603
		public CSteamID steamID;

		// Token: 0x0400296C RID: 10604
		public SteamP2PManager.PlayerInfo.State state;

		// Token: 0x0400296D RID: 10605
		public float lastPacketTime = Time.time;

		// Token: 0x0400296E RID: 10606
		private int _ping = -1;

		// Token: 0x0400296F RID: 10607
		private int _prevPing = -1;

		// Token: 0x04002970 RID: 10608
		public bool authenticated;

		// Token: 0x04002971 RID: 10609
		public Dictionary<SteamP2PManager.P2PChannel, BitStream> splitPackets = new Dictionary<SteamP2PManager.P2PChannel, BitStream>();

		// Token: 0x020008A9 RID: 2217
		public enum State
		{
			// Token: 0x04002F7D RID: 12157
			NotConnected,
			// Token: 0x04002F7E RID: 12158
			Received,
			// Token: 0x04002F7F RID: 12159
			Connected
		}
	}

	// Token: 0x020006D0 RID: 1744
	public class ConnectionAuthentication
	{
		// Token: 0x06003CAA RID: 15530 RVA: 0x00179B94 File Offset: 0x00177D94
		public ConnectionAuthentication(string password, string version)
		{
			this.password = password;
			this.version = version;
		}

		// Token: 0x04002972 RID: 10610
		public string password;

		// Token: 0x04002973 RID: 10611
		public string version;
	}

	// Token: 0x020006D1 RID: 1745
	// (Invoke) Token: 0x06003CAC RID: 15532
	public delegate void SteamPlayerConnected(CSteamID steamID);
}
