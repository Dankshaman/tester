using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace NewNet
{
	// Token: 0x020003AB RID: 939
	public class NetworkManager : Singleton<NetworkManager>
	{
		// Token: 0x06002C33 RID: 11315 RVA: 0x00135CC4 File Offset: 0x00133EC4
		protected override void Awake()
		{
			base.Awake();
			NetworkView.ResetId();
			NetworkID.ResetID();
			SteamP2PManager.OnSteamPlayerConnected += this.OnSteamPlayerConnect;
			Debug.Log(string.Format("Tick Rate: {0} Unreliable Packets Per Tick: {1}", NetworkManager.TickRate, NetworkManager.UnreliablePacketsPerTick));
			Debug.Log(string.Format("Unreliable -> Per Object: {0} Max Objects: {1}", 103, 92));
			Debug.Log(string.Format("Reliable -> Per Object: {0} Max Objects: {1}", 208, 384615));
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x00135D55 File Offset: 0x00133F55
		private void Start()
		{
			NetworkView.InitDisable();
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x00135D5C File Offset: 0x00133F5C
		private void OnDestroy()
		{
			SteamP2PManager.OnSteamPlayerConnected -= this.OnSteamPlayerConnect;
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x00135D70 File Offset: 0x00133F70
		private void OnSteamPlayerConnect(CSteamID connectingSteamID)
		{
			if (Network.isClient)
			{
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			NetworkPlayer y = new NetworkPlayer(Network.GetNextPlayerId());
			stream.Rewind();
			stream.WriteByte(4);
			stream.WriteUshort(y.id);
			stream.WriteUlong(connectingSteamID.m_SteamID);
			for (int i = 0; i < Network.connections.Count; i++)
			{
				NetworkPlayer networkPlayer = Network.connections[i];
				if (networkPlayer.isClient)
				{
					stream.WriteUshort(networkPlayer.id);
					stream.WriteUlong(networkPlayer.steamID.m_SteamID);
				}
			}
			this.SendPacket(connectingSteamID, stream, SendType.ReliableBuffered, -1);
			stream.Rewind();
			stream.WriteByte(6);
			int num = 0;
			for (int j = 0; j < Network.admins.Count; j++)
			{
				stream.WriteUshort(Network.admins[j].id);
				num++;
			}
			if (num > 0)
			{
				this.SendPacket(connectingSteamID, stream, SendType.ReliableBuffered, -1);
			}
			stream.Rewind();
			stream.WriteByte(2);
			int num2 = 0;
			for (int k = 0; k < NetworkView.IdView.Count; k++)
			{
				NetworkView networkView = NetworkView.IdView.List[k];
				if (networkView.isNetworkInstantiated)
				{
					stream.WriteUshort(networkView.owner.id);
					stream.WriteString(networkView.InternalName);
					ushort[] ids = networkView.GetIds();
					for (int l = 0; l < ids.Length; l++)
					{
						stream.WriteUshort(ids[l]);
					}
					stream.WriteVector3(networkView.transform.position);
					stream.WriteVector3(networkView.transform.eulerAngles);
					num2++;
				}
			}
			if (num2 > 0)
			{
				this.SendPacket(connectingSteamID, stream, SendType.ReliableBuffered, -1);
			}
			stream.Rewind();
			stream.WriteByte(1);
			int num3 = 0;
			ListReadOnly<NetworkView> list = NetworkView.IdView.List;
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].CheckConnectSync(stream))
				{
					num3++;
				}
			}
			if (num3 > 0)
			{
				this.SendPacket(connectingSteamID, stream, SendType.ReliableBuffered, -1);
			}
			stream.Rewind();
			stream.WriteByte(4);
			stream.WriteUshort(y.id);
			stream.WriteUlong(connectingSteamID.m_SteamID);
			for (int n = 0; n < Network.connections.Count; n++)
			{
				NetworkPlayer x = Network.connections[n];
				if (x != y)
				{
					this.SendPacket(x.steamID, stream, SendType.ReliableBuffered, -1);
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00135FFC File Offset: 0x001341FC
		private void LateUpdate()
		{
			if (Network.peerType == NetworkPeerMode.Disconnected)
			{
				return;
			}
			Singleton<SteamP2PManager>.Instance.writePacketStopWatch.Start();
			this.tickTime += Time.deltaTime;
			if (this.tickTime >= NetworkManager.TickRate)
			{
				this.tickTime = 0f;
				this.PacketId += 1U;
				this.SendPointerPacket();
				this.SendTrackingPackets();
				if (Network.isServer)
				{
					this.SendTickPackets();
				}
				this.SendVarSyncPackets();
			}
			this.pingTime += Time.deltaTime;
			if (this.pingTime >= NetworkManager.PingRate)
			{
				this.pingTime = 0f;
				this.SendPingPacket();
			}
			Singleton<SteamP2PManager>.Instance.writePacketStopWatch.Stop();
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x001360B8 File Offset: 0x001342B8
		private void SendPointerPacket()
		{
			if (PlayerScript.Pointer && this.prevPointerPosition != HoverScript.PointerPosition)
			{
				BitStream stream = NetworkManager.GetStream();
				this.prevPointerPosition = HoverScript.PointerPosition;
				stream.Rewind();
				stream.WritePositionLossy(HoverScript.PointerPosition);
				stream.WriteRotationLossyCoordinate(HoverScript.MainCamera.transform.eulerAngles.y);
				Singleton<SteamP2PManager>.Instance.SendP2PPacketOthers(stream.Buffer, (uint)stream.GetWrittenSizeInBytes(), EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.UnreliablePointer);
				NetworkManager.ReturnStream(stream);
			}
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x00136140 File Offset: 0x00134340
		private void SendTrackingPackets()
		{
			if (PlayerScript.Pointer)
			{
				BitStream stream = NetworkManager.GetStream();
				int num = 0;
				stream.Rewind();
				foreach (NetworkTouch networkTouch in PlayerScript.PointerScript.pointerSyncs.NetworkTouches)
				{
					if (networkTouch.gameObject.activeSelf)
					{
						stream.WriteUshort(networkTouch.GetComponent<NetworkView>().id);
						stream.WritePositionLossy(networkTouch.transform.position);
						num++;
					}
				}
				if (num > 0)
				{
					Singleton<SteamP2PManager>.Instance.SendP2PPacketOthers(stream.Buffer, (uint)stream.GetWrittenSizeInBytes(), EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.UnreliableTouch);
				}
				int num2 = 0;
				stream.Rewind();
				foreach (NetworkTracked networkTracked in PlayerScript.PointerScript.pointerSyncs.NetworkTrackeds)
				{
					if (networkTracked.gameObject.activeSelf)
					{
						stream.WriteUshort(networkTracked.GetComponent<NetworkView>().id);
						stream.WritePosition(networkTracked.transform.position);
						stream.WriteRotation(networkTracked.transform.eulerAngles);
						num2++;
					}
				}
				if (num2 > 0)
				{
					Singleton<SteamP2PManager>.Instance.SendP2PPacketOthers(stream.Buffer, (uint)stream.GetWrittenSizeInBytes(), EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.UnreliableTracked);
				}
				NetworkManager.ReturnStream(stream);
			}
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x00136280 File Offset: 0x00134480
		private void SendTickPackets()
		{
			List<NetworkPhysicsObject> allNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.AllNPOs;
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteUint(this.PacketId, 32);
			int num = 0;
			for (int i = 0; i < allNPOs.Count; i++)
			{
				NetworkPhysicsObject networkPhysicsObject = allNPOs[i];
				if (networkPhysicsObject && networkPhysicsObject.JustFellAsleep)
				{
					networkPhysicsObject.JustFellAsleep = false;
					if (networkPhysicsObject.isSleeping())
					{
						stream.WriteUshort(networkPhysicsObject.networkView.id);
						stream.WritePosition(networkPhysicsObject.transform.position);
						stream.WriteRotation(networkPhysicsObject.transform.eulerAngles);
						num++;
					}
				}
				if (num >= 384615)
				{
					break;
				}
			}
			if (num > 0)
			{
				Singleton<SteamP2PManager>.Instance.SendP2PPacketOthers(stream.Buffer, (uint)stream.GetWrittenSizeInBytes(), EP2PSend.k_EP2PSendReliable, SteamP2PManager.P2PChannel.ReliableTick);
			}
			int num2 = -1;
			for (int j = 0; j < NetworkManager.UnreliablePacketsPerTick; j++)
			{
				stream.Rewind();
				stream.WriteUint(this.PacketId, 32);
				num = 0;
				bool flag = false;
				if (this.startIndex > allNPOs.Count)
				{
					this.startIndex = 0;
				}
				for (int k = this.startIndex; k < allNPOs.Count; k++)
				{
					if (k == num2)
					{
						this.startIndex = 0;
						break;
					}
					NetworkPhysicsObject networkPhysicsObject2 = allNPOs[k];
					if (networkPhysicsObject2 && !networkPhysicsObject2.isSleeping())
					{
						stream.WriteUshort(networkPhysicsObject2.networkView.id);
						stream.WritePositionLossy(networkPhysicsObject2.transform.position);
						stream.WriteRotationLossy(networkPhysicsObject2.transform.eulerAngles);
						num++;
					}
					if (num >= 92)
					{
						if (num2 == -1)
						{
							num2 = this.startIndex;
						}
						flag = true;
						this.startIndex = k + 1;
						break;
					}
				}
				if (!flag && this.startIndex != 0)
				{
					for (int l = 0; l < this.startIndex; l++)
					{
						if (l == num2)
						{
							this.startIndex = 0;
							break;
						}
						NetworkPhysicsObject networkPhysicsObject3 = allNPOs[l];
						if (networkPhysicsObject3 && !networkPhysicsObject3.isSleeping())
						{
							stream.WriteUshort(networkPhysicsObject3.networkView.id);
							stream.WritePositionLossy(networkPhysicsObject3.transform.position);
							stream.WriteRotationLossy(networkPhysicsObject3.transform.eulerAngles);
							num++;
						}
						if (num >= 92)
						{
							if (num2 == -1)
							{
								num2 = this.startIndex;
							}
							flag = true;
							this.startIndex = l + 1;
							break;
						}
					}
				}
				if (num > 0)
				{
					Singleton<SteamP2PManager>.Instance.SendP2PPacketOthers(stream.Buffer, (uint)stream.GetWrittenSizeInBytes(), EP2PSend.k_EP2PSendUnreliableNoDelay, SteamP2PManager.P2PChannel.UnreliableTick);
				}
				if (!flag)
				{
					this.startIndex = 0;
					break;
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x00136528 File Offset: 0x00134728
		private void SendVarSyncPackets()
		{
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(1);
			int num = 0;
			ListReadOnly<NetworkView> list = NetworkView.IdView.List;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].CheckSync(stream))
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.SendPacket(RPCTarget.Others, stream, SendType.ReliableNoDelay, -1);
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00136590 File Offset: 0x00134790
		private void SendPingPacket()
		{
			this.lastPingPacketId = this.PacketId;
			this.lastPingTime = Time.time;
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(11);
			stream.WriteUint(this.lastPingPacketId);
			this.SendPacket(RPCTarget.Others, stream, SendType.ReliableNoDelay, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x001365E4 File Offset: 0x001347E4
		private void ReceivePing(BitStream s, NetworkPlayer sender)
		{
			uint value = s.ReadUint();
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteByte(12);
			stream.WriteUint(value);
			this.SendPacket(sender, stream, SendType.ReliableNoDelay, -1);
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00136624 File Offset: 0x00134824
		private void ReceivePong(BitStream s, NetworkPlayer sender)
		{
			uint num = s.ReadUint();
			int num2 = 999;
			if (this.lastPingPacketId == num)
			{
				num2 = (int)(1000f * (Time.time - this.lastPingTime));
			}
			num2 = Mathf.Clamp(num2, 0, 999);
			SteamP2PManager.PlayerInfo playerInfo;
			if (Singleton<SteamP2PManager>.Instance.players.TryGetValue(sender.steamID, out playerInfo))
			{
				playerInfo.ping = num2;
			}
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x0013668C File Offset: 0x0013488C
		private NetworkManager.PacketSend GetPacketSend(SendType sendType)
		{
			NetworkManager.PacketSend result = default(NetworkManager.PacketSend);
			switch (sendType)
			{
			case SendType.ReliableNoDelay:
				result.send = EP2PSend.k_EP2PSendReliable;
				result.channel = SteamP2PManager.P2PChannel.Reliable;
				break;
			case SendType.ReliableBuffered:
				result.send = (NetworkManager.Buffering ? EP2PSend.k_EP2PSendReliableWithBuffering : EP2PSend.k_EP2PSendReliable);
				result.channel = SteamP2PManager.P2PChannel.Reliable;
				break;
			case SendType.Unreliable:
				result.send = EP2PSend.k_EP2PSendUnreliable;
				result.channel = SteamP2PManager.P2PChannel.Unreliable;
				break;
			}
			return result;
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x001366F4 File Offset: 0x001348F4
		public void SendPacket(RPCTarget target, BitStream stream, SendType sendType = SendType.ReliableBuffered, int size = -1)
		{
			NetworkManager.PacketSend packetSend = this.GetPacketSend(sendType);
			Singleton<SteamP2PManager>.Instance.SendP2PPacket(target, stream.Buffer, (uint)((size == -1) ? stream.GetWrittenSizeInBytes() : size), packetSend.send, packetSend.channel);
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x00136738 File Offset: 0x00134938
		public void SendPacket(NetworkPlayer receiver, BitStream stream, SendType sendType = SendType.ReliableBuffered, int size = -1)
		{
			if (!receiver.IsValid())
			{
				if (!NetworkSingleton<NetworkUI>.Instance.bHotseat)
				{
					Debug.LogError("Invalid network player for send packet.");
				}
				return;
			}
			NetworkManager.PacketSend packetSend = this.GetPacketSend(sendType);
			Singleton<SteamP2PManager>.Instance.SendP2PPacket(receiver.steamID, stream.Buffer, (uint)((size == -1) ? stream.GetWrittenSizeInBytes() : size), packetSend.send, packetSend.channel);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x001367A0 File Offset: 0x001349A0
		public void SendPacket(CSteamID steamID, BitStream stream, SendType sendType = SendType.ReliableBuffered, int size = -1)
		{
			NetworkManager.PacketSend packetSend = this.GetPacketSend(sendType);
			Singleton<SteamP2PManager>.Instance.SendP2PPacket(steamID, stream.Buffer, (uint)((size == -1) ? stream.GetWrittenSizeInBytes() : size), packetSend.send, packetSend.channel);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x001367E4 File Offset: 0x001349E4
		public void ReceiveUnreliableTouch(NetworkPlayer sender, byte[] data, uint size)
		{
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteBytes(data, (int)size);
			stream.Rewind();
			while ((long)stream.GetWrittenSizeInBytes() < (long)((ulong)size))
			{
				ushort key = stream.ReadUshort();
				Vector3 position = stream.ReadPositionLossy();
				NetworkView networkView;
				if (NetworkView.IdView.TryGetValue(key, out networkView))
				{
					networkView.transform.position = position;
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00136848 File Offset: 0x00134A48
		public void ReceiveUnreliableTracked(NetworkPlayer sender, byte[] data, uint size)
		{
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteBytes(data, (int)size);
			stream.Rewind();
			while ((long)stream.GetWrittenSizeInBytes() < (long)((ulong)size))
			{
				ushort key = stream.ReadUshort();
				Vector3 position = stream.ReadPosition();
				Vector3 euler = stream.ReadRotation();
				NetworkView networkView;
				if (NetworkView.IdView.TryGetValue(key, out networkView))
				{
					networkView.transform.SetPositionAndRotation(position, Quaternion.Euler(euler));
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x001368BC File Offset: 0x00134ABC
		public void ReceiveUnreliableTickPacket(NetworkPlayer sender, byte[] data, uint size)
		{
			if (sender.isClient)
			{
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteBytes(data, (int)size);
			stream.Rewind();
			uint packetId = stream.ReadUint(32);
			int num = (int)((size * 8U - 32U) / 103U);
			for (int i = 0; i < num; i++)
			{
				ushort key = stream.ReadUshort();
				Vector3 position = stream.ReadPositionLossy();
				Vector3 euler = stream.ReadRotationLossy();
				NetworkView networkView;
				if (NetworkView.IdView.TryGetValue(key, out networkView))
				{
					networkView.GetComponent<NetworkInterpolate>().Interpolate(position, Quaternion.Euler(euler), packetId);
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x00136950 File Offset: 0x00134B50
		public void ReceiveReliableTickPacket(NetworkPlayer sender, byte[] data, uint size)
		{
			if (sender.isClient)
			{
				return;
			}
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteBytes(data, (int)size);
			stream.Rewind();
			uint packetId = stream.ReadUint(32);
			int num = (int)((size * 8U - 32U) / 208U);
			for (int i = 0; i < num; i++)
			{
				ushort key = stream.ReadUshort();
				Vector3 position = stream.ReadPosition();
				Vector3 euler = stream.ReadRotation();
				NetworkView networkView;
				if (NetworkView.IdView.TryGetValue(key, out networkView))
				{
					networkView.GetComponent<NetworkInterpolate>().Interpolate(position, Quaternion.Euler(euler), packetId);
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x001369E8 File Offset: 0x00134BE8
		public void ReceivePointerPacket(NetworkPlayer sender, byte[] data, uint size)
		{
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteBytes(data, (int)size);
			stream.Rewind();
			Vector3 position = stream.ReadPositionLossy();
			float num = stream.ReadRotationLossyCoordinate();
			Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID((int)sender.id);
			if (pointer)
			{
				pointer.rigidbody.position = position;
				pointer.rigidbody.rotation = Quaternion.Euler(new Vector3(0f, num - 180f, 0f));
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x00136A70 File Offset: 0x00134C70
		public void ReceivePacket(NetworkPlayer sender, byte[] data, uint size, bool reliable)
		{
			BitStream stream = NetworkManager.GetStream();
			stream.Rewind();
			stream.WriteBytes(data, (int)size);
			stream.Rewind();
			NetworkHeader networkHeader = (NetworkHeader)stream.ReadByte();
			if (networkHeader == NetworkHeader.RPC)
			{
				ushort num = stream.Peek<ushort>(stream.FuncReadUshort);
				if (Network.isServer)
				{
					stream.WriteUshort(sender.id);
					if (num == 0)
					{
						for (int i = 0; i < Network.connections.Count; i++)
						{
							NetworkPlayer networkPlayer = Network.connections[i];
							if (networkPlayer != sender && networkPlayer != Network.player)
							{
								this.SendPacket(networkPlayer, stream, reliable ? SendType.ReliableBuffered : SendType.Unreliable, (int)size);
							}
						}
					}
					else if (num == 65535)
					{
						for (int j = 0; j < Network.connections.Count; j++)
						{
							NetworkPlayer networkPlayer2 = Network.connections[j];
							if (networkPlayer2 != Network.player)
							{
								this.SendPacket(networkPlayer2, stream, reliable ? SendType.ReliableBuffered : SendType.Unreliable, (int)size);
							}
						}
					}
					else if (num != 1)
					{
						NetworkPlayer networkPlayer3 = new NetworkPlayer(num);
						if (Network.connections.Contains(networkPlayer3))
						{
							this.SendPacket(networkPlayer3, stream, reliable ? SendType.ReliableBuffered : SendType.Unreliable, (int)size);
						}
						else
						{
							Debug.LogError(string.Concat(new object[]
							{
								"Player is not connected for redirect: ",
								num,
								" sender: ",
								sender
							}));
						}
						NetworkManager.ReturnStream(stream);
						return;
					}
				}
				else
				{
					if (sender.isClient)
					{
						Debug.LogError("Clients are not allowed to directly communicate with RPCs.");
						NetworkManager.ReturnStream(stream);
						return;
					}
					stream.ReadUshort();
					sender = new NetworkPlayer(num);
					Network.sender = sender;
				}
			}
			while ((long)stream.GetWrittenSizeInBytes() < (long)((ulong)size))
			{
				bool flag = false;
				switch (networkHeader)
				{
				case NetworkHeader.RPC:
					if (!NetworkView.ReceiveRPC(stream, sender, reliable))
					{
						flag = true;
					}
					break;
				case NetworkHeader.VarSync:
					if (!NetworkView.ReceiveSync(stream, sender))
					{
						flag = true;
					}
					break;
				case NetworkHeader.Instantiate:
					Network.ReceiveInstantiate(stream, sender);
					break;
				case NetworkHeader.Destroy:
					Network.ReceiveDestroy(stream, sender);
					break;
				case NetworkHeader.AddPlayer:
					Network.ReceiveAddPlayer(stream, sender);
					break;
				case NetworkHeader.RemovePlayer:
					Network.ReceiveRemovePlayer(stream, sender);
					break;
				case NetworkHeader.AddAdmin:
					Network.ReceiveAddAdmin(stream, sender);
					break;
				case NetworkHeader.RemoveAdmin:
					Network.ReceiveRemoveAdmin(stream, sender);
					break;
				case NetworkHeader.Ping:
					this.ReceivePing(stream, sender);
					break;
				case NetworkHeader.Pong:
					this.ReceivePong(stream, sender);
					break;
				}
				if (flag)
				{
					break;
				}
			}
			NetworkManager.ReturnStream(stream);
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x0013441B File Offset: 0x0013261B
		private void OnApplicationQuit()
		{
			Network.ReceiveDisconnect(DisconnectInfo.Successful);
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00136CD2 File Offset: 0x00134ED2
		public static BitStream GetStream()
		{
			return BitStream.GetPooled(10000000);
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00136CDE File Offset: 0x00134EDE
		public static void ReturnStream(BitStream bitStream)
		{
			BitStream.ReturnPooled(bitStream);
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x00136CE6 File Offset: 0x00134EE6
		// (set) Token: 0x06002C4D RID: 11341 RVA: 0x00136CF0 File Offset: 0x00134EF0
		public static NetworkManager.NetworkQuality Quality
		{
			get
			{
				return NetworkManager._Quality;
			}
			set
			{
				if (value != NetworkManager._Quality)
				{
					NetworkManager._Quality = value;
					switch (value)
					{
					case NetworkManager.NetworkQuality.High:
						NetworkManager.TickRate = 0.016f;
						NetworkManager.UnreliablePacketsPerTick = 2;
						return;
					case NetworkManager.NetworkQuality.Medium:
						NetworkManager.TickRate = 0.033f;
						NetworkManager.UnreliablePacketsPerTick = 2;
						return;
					case NetworkManager.NetworkQuality.Low:
						NetworkManager.TickRate = 0.033f;
						NetworkManager.UnreliablePacketsPerTick = 1;
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x04001DDE RID: 7646
		public static float TickRate = 0.016f;

		// Token: 0x04001DDF RID: 7647
		public static float PingRate = 1f;

		// Token: 0x04001DE0 RID: 7648
		public static int UnreliablePacketsPerTick = 2;

		// Token: 0x04001DE1 RID: 7649
		public static bool Buffering = true;

		// Token: 0x04001DE2 RID: 7650
		private const EP2PSend ReliableSend = EP2PSend.k_EP2PSendReliable;

		// Token: 0x04001DE3 RID: 7651
		private const EP2PSend UnreliableSend = EP2PSend.k_EP2PSendUnreliableNoDelay;

		// Token: 0x04001DE4 RID: 7652
		public const int MAX_PACKET_SIZE = 10000000;

		// Token: 0x04001DE5 RID: 7653
		public const int MAX_RPC_PACKET_SIZE = 9999993;

		// Token: 0x04001DE6 RID: 7654
		private const int HEADER_SIZE_BIT = 32;

		// Token: 0x04001DE7 RID: 7655
		private const int ID_SIZE_BIT = 16;

		// Token: 0x04001DE8 RID: 7656
		private const int RELIABLE_PACKET_SIZE_BYTE = 10000000;

		// Token: 0x04001DE9 RID: 7657
		private const int UNRELIABLE_PACKET_SIZE_BYTE = 1200;

		// Token: 0x04001DEA RID: 7658
		private const int RELIABLE_PACKET_SIZE_BIT = 80000000;

		// Token: 0x04001DEB RID: 7659
		private const int UNRELIABLE_PACKET_SIZE_BIT = 9600;

		// Token: 0x04001DEC RID: 7660
		private const int RELIABLE_AVAILABLE_SIZE_BIT = 79999968;

		// Token: 0x04001DED RID: 7661
		private const int UNRELIABLE_AVAILABLE_SIZE_BIT = 9568;

		// Token: 0x04001DEE RID: 7662
		private const int UNRELIABLE_PER_SIZE_BIT = 103;

		// Token: 0x04001DEF RID: 7663
		private const int RELIABLE_PER_SIZE_BIT = 208;

		// Token: 0x04001DF0 RID: 7664
		private const int MAX_COUNT_UNRELIABLE_PACKET = 92;

		// Token: 0x04001DF1 RID: 7665
		private const int MAX_COUNT_RELIABLE_PACKET = 384615;

		// Token: 0x04001DF2 RID: 7666
		private float tickTime;

		// Token: 0x04001DF3 RID: 7667
		private int startIndex;

		// Token: 0x04001DF4 RID: 7668
		private uint PacketId;

		// Token: 0x04001DF5 RID: 7669
		private float pingTime;

		// Token: 0x04001DF6 RID: 7670
		private uint lastPingPacketId;

		// Token: 0x04001DF7 RID: 7671
		private float lastPingTime;

		// Token: 0x04001DF8 RID: 7672
		private Vector3 prevPointerPosition = Vector3.zero;

		// Token: 0x04001DF9 RID: 7673
		private static NetworkManager.NetworkQuality _Quality = NetworkManager.NetworkQuality.High;

		// Token: 0x020007DC RID: 2012
		private struct PacketSend
		{
			// Token: 0x04002D7D RID: 11645
			public EP2PSend send;

			// Token: 0x04002D7E RID: 11646
			public SteamP2PManager.P2PChannel channel;
		}

		// Token: 0x020007DD RID: 2013
		public enum NetworkQuality
		{
			// Token: 0x04002D80 RID: 11648
			High,
			// Token: 0x04002D81 RID: 11649
			Medium,
			// Token: 0x04002D82 RID: 11650
			Low
		}
	}
}
