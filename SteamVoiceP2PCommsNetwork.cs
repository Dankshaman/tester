using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dissonance;
using Dissonance.Networking;
using NewNet;
using Steamworks;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class SteamVoiceP2PCommsNetwork : BaseCommsNetwork<SteamVoiceP2PServer, SteamVoiceP2PClient, SteamVoiceP2PPeer, Unit, Unit>
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000015 RID: 21 RVA: 0x0000284F File Offset: 0x00000A4F
	// (set) Token: 0x06000016 RID: 22 RVA: 0x00002856 File Offset: 0x00000A56
	public static SteamVoiceP2PCommsNetwork Instance { get; private set; }

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000017 RID: 23 RVA: 0x0000285E File Offset: 0x00000A5E
	// (set) Token: 0x06000018 RID: 24 RVA: 0x00002866 File Offset: 0x00000A66
	public DissonanceComms Comms { get; private set; }

	// Token: 0x06000019 RID: 25 RVA: 0x0000286F File Offset: 0x00000A6F
	protected override SteamVoiceP2PServer CreateServer(Unit connectionParameters)
	{
		return new SteamVoiceP2PServer(this);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002877 File Offset: 0x00000A77
	protected override SteamVoiceP2PClient CreateClient(Unit connectionParameters)
	{
		return new SteamVoiceP2PClient(this);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002880 File Offset: 0x00000A80
	public void ReceivePacketServer(CSteamID cSteamID, ArraySegment<byte> data)
	{
		SteamVoiceP2PPeer source = new SteamVoiceP2PPeer(cSteamID);
		SteamVoiceP2PServer server = base.Server;
		if (server == null)
		{
			return;
		}
		server.ReceivePacket(source, data);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000028A8 File Offset: 0x00000AA8
	public void ReceivePacketClient(CSteamID cSteamID, ArraySegment<byte> data)
	{
		SteamVoiceP2PPeer source = new SteamVoiceP2PPeer(cSteamID);
		SteamVoiceP2PClient client = base.Client;
		if (client == null)
		{
			return;
		}
		client.ReceivePacket(source, data);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000028D0 File Offset: 0x00000AD0
	private void Awake()
	{
		if (Utilities.IsLaunchOption("-novoicechat"))
		{
			Chat.Log("Voice chat is disabled by -novoicechat launch option.", Colour.Blue, ChatMessageType.All, true);
			base.gameObject.SetActive(false);
			return;
		}
		SteamVoiceP2PCommsNetwork.Instance = this;
		this.Comms = base.GetComponent<DissonanceComms>();
		this.Comms.OnPlayerStartedSpeaking += this.Comms_OnPlayerStartedSpeaking;
		this.Comms.OnPlayerStoppedSpeaking += this.Comms_OnPlayerStoppedSpeaking;
		this.Comms.OnPlayerJoinedSession += this.Comms_OnPlayerJoinedSession;
		EventManager.OnChangePlayerTeam += this.OnChangePlayerTeam;
		EventManager.OnVoiceTalk += this.OnVoiceTalk;
		EventManager.OnPlayerMute += this.OnPlayerMute;
		base.StartCoroutine(this.CleanupZombieConnections());
		base.StartCoroutine(this.RequestMicPermission());
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000029AC File Offset: 0x00000BAC
	private void OnDestroy()
	{
		this.Comms.OnPlayerStartedSpeaking -= this.Comms_OnPlayerStartedSpeaking;
		this.Comms.OnPlayerStoppedSpeaking -= this.Comms_OnPlayerStoppedSpeaking;
		this.Comms.OnPlayerJoinedSession -= this.Comms_OnPlayerJoinedSession;
		EventManager.OnChangePlayerTeam -= this.OnChangePlayerTeam;
		EventManager.OnVoiceTalk -= this.OnVoiceTalk;
		EventManager.OnPlayerMute -= this.OnPlayerMute;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002A31 File Offset: 0x00000C31
	private IEnumerator RequestMicPermission()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		if (Application.HasUserAuthorization(UserAuthorization.Microphone))
		{
			Debug.Log("Mic permission was granted.");
		}
		else
		{
			Chat.LogError("Mic permission was not granted talking in voice chat will not work.", true);
			Debug.LogError("Mic permission was not granted talking in voice chat will not work.");
		}
		yield break;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002A39 File Offset: 0x00000C39
	private IEnumerator CleanupZombieConnections()
	{
		for (;;)
		{
			yield return this.waitZombie;
			if (Network.isServer && base.Server != null && base.Client != null)
			{
				List<CSteamID> list = new List<CSteamID>();
				ReadOnlyCollection<VoicePlayerState> players = this.Comms.Players;
				for (int i = 0; i < players.Count; i++)
				{
					VoicePlayerState voicePlayerState = players[i];
					SteamVoiceP2PPeer? steamVoiceP2PPeer = base.Client.PlayerNameToPeer(voicePlayerState.Name);
					if (steamVoiceP2PPeer != null)
					{
						CSteamID steamID = steamVoiceP2PPeer.Value.steamID;
						if (!list.Contains(steamID))
						{
							list.Add(steamID);
							if (!NetworkSingleton<PlayerManager>.Instance.ContainsSteamId(steamID))
							{
								Chat.LogError("Cleanup zombie player voice chat: " + steamID, true);
								this.ServerDisconnectClient(steamVoiceP2PPeer.Value);
							}
						}
						else
						{
							Chat.LogError("Cleanup duplicate player voice chat: " + steamID, true);
							this.ServerDisconnectClient(steamVoiceP2PPeer.Value);
						}
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002A48 File Offset: 0x00000C48
	public void ServerDisconnectClient(SteamVoiceP2PPeer peer)
	{
		SteamVoiceP2PServer server = base.Server;
		if (server == null)
		{
			return;
		}
		server.DisconnectClient(peer);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002A5B File Offset: 0x00000C5B
	private void Comms_OnPlayerJoinedSession(VoicePlayerState voicePlayerState)
	{
		voicePlayerState.Volume = this.OutputVolume;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Comms_OnPlayerStartedSpeaking(VoicePlayerState voicePlayerState)
	{
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Comms_OnPlayerStoppedSpeaking(VoicePlayerState voicePlayerState)
	{
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002A6C File Offset: 0x00000C6C
	private void OnVoiceTalk(VoiceTalking talking)
	{
		for (int i = 0; i < this.VoiceRooms.Count; i++)
		{
			VoiceBroadcastTrigger component = this.VoiceRooms[i].GetComponent<VoiceBroadcastTrigger>();
			if (component.name == "Game")
			{
				component.Mode = ((talking == VoiceTalking.Game) ? CommActivationMode.VoiceActivation : CommActivationMode.None);
			}
			else
			{
				component.Mode = ((talking == VoiceTalking.Team) ? CommActivationMode.VoiceActivation : CommActivationMode.None);
			}
		}
		NetworkSingleton<PlayerManager>.Instance.MyPlayerState().ui.bTalk = (talking == VoiceTalking.Game);
		NetworkSingleton<PlayerManager>.Instance.MyPlayerState().ui.bTalkTeam = (talking == VoiceTalking.Team);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002B04 File Offset: 0x00000D04
	private void OnPlayerMute(bool muted, int id)
	{
		if (base.Client != null)
		{
			string text = base.Client.PeerToPlayerName(new SteamVoiceP2PPeer(NetworkSingleton<PlayerManager>.Instance.CSteamIDFromID(id)));
			if (!string.IsNullOrEmpty(text))
			{
				VoicePlayerState voicePlayerState = this.Comms.FindPlayer(text);
				if (voicePlayerState != null)
				{
					voicePlayerState.IsLocallyMuted = muted;
				}
			}
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002B54 File Offset: 0x00000D54
	private void OnChangePlayerTeam(bool join, int id)
	{
		if (NetworkID.ID != id)
		{
			return;
		}
		foreach (string text in this.Comms.Tokens)
		{
			if (text != "Game")
			{
				this.Comms.RemoveToken(text);
				break;
			}
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
		if (playerState.team != Team.None)
		{
			this.Comms.AddToken(playerState.team.ToString());
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000028 RID: 40 RVA: 0x00002BF8 File Offset: 0x00000DF8
	// (set) Token: 0x06000029 RID: 41 RVA: 0x00002C00 File Offset: 0x00000E00
	public float InputVolume
	{
		get
		{
			return this._InputVolume;
		}
		set
		{
			if (value == this._InputVolume)
			{
				return;
			}
			this._InputVolume = value;
			for (int i = 0; i < this.VoiceRooms.Count; i++)
			{
				this.VoiceRooms[i].GetComponent<VoiceBroadcastTrigger>().ActivationFader.Volume = this._InputVolume;
			}
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600002A RID: 42 RVA: 0x00002C55 File Offset: 0x00000E55
	// (set) Token: 0x0600002B RID: 43 RVA: 0x00002C60 File Offset: 0x00000E60
	public float OutputVolume
	{
		get
		{
			return this._OutputVolume;
		}
		set
		{
			if (value == this._OutputVolume)
			{
				return;
			}
			this._OutputVolume = value;
			ReadOnlyCollection<VoicePlayerState> players = this.Comms.Players;
			for (int i = 0; i < players.Count; i++)
			{
				VoicePlayerState voicePlayerState = this.Comms.Players[i];
				if (voicePlayerState.Name != this.Comms.LocalPlayerName)
				{
					voicePlayerState.Volume = this._OutputVolume;
				}
			}
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002CD4 File Offset: 0x00000ED4
	protected override void Update()
	{
		if (base.IsInitialized)
		{
			bool flag = false;
			if (Network.peerType != NetworkPeerMode.Disconnected && NetworkSingleton<PlayerManager>.Instance.ContainsID(-1))
			{
				if (base.Mode == NetworkMode.None)
				{
					flag = Singleton<SteamP2PManager>.Instance.IsP2PConnectedToAll();
					if (!flag && Singleton<SteamP2PManager>.Instance.IsP2PConnectedToServer())
					{
						if (this.TimeOut == 0f)
						{
							this.TimeOut = Time.time;
						}
						else if (Time.time > this.TimeOut + 21f)
						{
							Chat.LogError("Voice timeout connecting to other peers, but connected to host.", true);
							flag = true;
							this.TimeOut = 0f;
						}
					}
				}
				else
				{
					flag = Singleton<SteamP2PManager>.Instance.IsP2PConnectedToServer();
				}
			}
			if (flag)
			{
				bool isServer = Network.isServer;
				bool flag2 = Network.isServer || Network.isClient;
				if (base.Mode.IsServerEnabled() != isServer || base.Mode.IsClientEnabled() != flag2)
				{
					if (isServer && flag2)
					{
						base.RunAsHost(Unit.None, Unit.None);
					}
					else if (isServer)
					{
						base.RunAsDedicatedServer(Unit.None);
					}
					else if (flag2)
					{
						base.RunAsClient(Unit.None);
					}
				}
			}
			else if (base.Mode != NetworkMode.None)
			{
				base.Stop();
			}
		}
		this.UpdateVoiceIcons();
		base.Update();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002E00 File Offset: 0x00001000
	private void UpdateVoiceIcons()
	{
		if (base.Client == null)
		{
			return;
		}
		ReadOnlyCollection<VoicePlayerState> players = this.Comms.Players;
		for (int i = 0; i < players.Count; i++)
		{
			VoicePlayerState voicePlayerState = players[i];
			SteamVoiceP2PPeer? steamVoiceP2PPeer = base.Client.PlayerNameToPeer(voicePlayerState.Name);
			if (steamVoiceP2PPeer != null)
			{
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromSteamID(steamVoiceP2PPeer.Value.steamID);
				if (playerState != null && playerState.ui != null)
				{
					if (voicePlayerState.IsSpeaking)
					{
						bool flag = false;
						voicePlayerState.GetSpeakingChannels(this.speakingChannels);
						for (int j = 0; j < this.speakingChannels.Count; j++)
						{
							if (this.speakingChannels[j].TargetName != "Game")
							{
								flag = true;
								break;
							}
						}
						this.speakingChannels.Clear();
						playerState.ui.bTalkTeam = flag;
						playerState.ui.bTalk = !flag;
					}
					else
					{
						playerState.ui.bTalkTeam = false;
						playerState.ui.bTalk = false;
					}
				}
			}
		}
	}

	// Token: 0x04000005 RID: 5
	public List<GameObject> VoiceRooms = new List<GameObject>();

	// Token: 0x04000006 RID: 6
	private WaitForSeconds waitZombie = new WaitForSeconds(5f);

	// Token: 0x04000007 RID: 7
	private float _InputVolume = 1f;

	// Token: 0x04000008 RID: 8
	private float _OutputVolume = 1f;

	// Token: 0x04000009 RID: 9
	private float TimeOut;

	// Token: 0x0400000A RID: 10
	private List<RemoteChannel> speakingChannels = new List<RemoteChannel>();
}
