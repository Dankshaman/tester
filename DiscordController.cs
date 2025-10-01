using System;
using System.Collections.Generic;
using Discord;
using NewNet;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class DiscordController : Singleton<DiscordController>
{
	// Token: 0x06000C90 RID: 3216 RVA: 0x00055C18 File Offset: 0x00053E18
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnSettingsChange += this.GeneralSettingUpdated;
		NetworkEvents.OnConnectedToServer += this.GeneralSettingUpdated;
		NetworkEvents.OnServerInitialized += this.GeneralSettingUpdated;
		NetworkEvents.OnPlayerConnected += this.PlayerConnected;
		NetworkEvents.OnPlayerDisconnected += this.PlayerDisconnected;
		EventManager.OnLoadingComplete += this.GeneralSettingUpdated;
		EventManager.OnChangePlayerColor += this.PlayerSeatUpdated;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00055CA4 File Offset: 0x00053EA4
	public void OnDestroy()
	{
		NetworkEvents.OnSettingsChange -= this.GeneralSettingUpdated;
		NetworkEvents.OnConnectedToServer -= this.GeneralSettingUpdated;
		NetworkEvents.OnServerInitialized -= this.GeneralSettingUpdated;
		NetworkEvents.OnPlayerConnected -= this.PlayerConnected;
		NetworkEvents.OnPlayerDisconnected -= this.PlayerDisconnected;
		EventManager.OnLoadingComplete -= this.GeneralSettingUpdated;
		EventManager.OnChangePlayerColor -= this.PlayerSeatUpdated;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x00055D28 File Offset: 0x00053F28
	public void Start()
	{
		this.playerStates = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		this.GameName = "";
		this.Connections = 0;
		this.MaxConnections = 0;
		this.Seated = 0;
		this.Seats = 0;
		this.MyColor = Colour.MyColorLabel();
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x00055D78 File Offset: 0x00053F78
	private void OnEnable()
	{
		try
		{
			this.DiscordActivity.Assets.LargeImage = "default-large";
			this.DiscordActivity.Assets.LargeText = "Tabletop Simulator by Berserk Games";
			this.DiscordActivity.Assets.SmallImage = "";
			this.DiscordActivity.Assets.SmallText = "";
			this.DiscordActivity.Party.Id = string.Format("{0}{1}", Singleton<SteamLobbyManager>.Instance.CurrentSteamIDLobby, DateTime.UtcNow).GetHashCode().ToString();
			this.discord = new Discord(402572971681644545L, 1UL);
			this.activityManager = this.discord.GetActivityManager();
			this.activityManager.RegisterSteam(286160U);
			this.activityManager.OnActivityJoin += this.OnActivityJoin;
			this.activityManager.OnActivityJoinRequest += this.OnActivityJoinRequest;
		}
		catch (Exception)
		{
			this.UseDiscord = false;
		}
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00055E9C File Offset: 0x0005409C
	private void OnDisable()
	{
		if (!this.UseDiscord)
		{
			return;
		}
		this.DiscordActivity.Details = "-";
		this.DiscordActivity.State = "";
		this.DiscordActivity.Party.Size.CurrentSize = 0;
		this.DiscordActivity.Party.Size.MaxSize = 0;
		this.DiscordActivity.Assets.SmallImage = "";
		this.DiscordActivity.Assets.SmallText = "";
		this.DiscordActivity.Timestamps.Start = DiscordController.EpochTime();
		this.activityManager.UpdateActivity(this.DiscordActivity, delegate(Discord.Result _)
		{
		});
		this.activityManager.OnActivityJoin -= this.OnActivityJoin;
		this.activityManager.OnActivityJoinRequest -= this.OnActivityJoinRequest;
		this.discord.Dispose();
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00055FA6 File Offset: 0x000541A6
	private void Update()
	{
		if (this.UseDiscord)
		{
			this.discord.RunCallbacks();
		}
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x00055FBC File Offset: 0x000541BC
	public void PlayerSeatUpdated(Color newColor, int id)
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < this.playerStates.Count; i++)
		{
			if (this.playerStates[i].CanHaveHand())
			{
				num++;
			}
		}
		if (num != this.Seated)
		{
			this.Seated = num;
			flag = true;
		}
		if (NetworkID.ID == id)
		{
			this.MyColor = Colour.MyColorLabel();
			flag = true;
		}
		if (flag)
		{
			this.RefreshPresence();
		}
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x0005602A File Offset: 0x0005422A
	public void PlayerConnected(NetworkPlayer player)
	{
		this.GeneralSettingUpdated();
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x0005602A File Offset: 0x0005422A
	public void PlayerDisconnected(NetworkPlayer player, DisconnectInfo info)
	{
		this.GeneralSettingUpdated();
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00056034 File Offset: 0x00054234
	public void GeneralSettingUpdated()
	{
		bool flag = false;
		if (this.GameName != Network.gameName)
		{
			this.GameName = Network.gameName;
			if (Network.peerType == NetworkPeerMode.Disconnected)
			{
				this.DiscordActivity.Timestamps.Start = 0L;
			}
			else
			{
				this.DiscordActivity.Timestamps.Start = DiscordController.EpochTime();
			}
			flag = true;
		}
		if (Network.connections.Count != this.Connections)
		{
			this.Connections = Network.connections.Count;
			flag = true;
		}
		if (Network.maxConnections != this.Connections)
		{
			this.MaxConnections = Network.maxConnections;
			flag = true;
		}
		if (Network.peerType != this.PeerType)
		{
			this.PeerType = Network.peerType;
			flag = true;
		}
		int uniqueHandCount = HandZone.GetUniqueHandCount();
		if (uniqueHandCount != this.Seats)
		{
			this.Seats = uniqueHandCount;
			flag = true;
		}
		if (flag)
		{
			this.RefreshPresence();
		}
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0005610C File Offset: 0x0005430C
	public void RefreshPresence()
	{
		if (Network.peerType == NetworkPeerMode.Connecting)
		{
			this.DiscordActivity.Details = "Connecting...";
			this.DiscordActivity.Party.Size.CurrentSize = 0;
			this.DiscordActivity.Party.Size.MaxSize = 0;
			this.DiscordActivity.Assets.SmallImage = "";
			this.DiscordActivity.Assets.SmallText = "";
			this.DiscordActivity.Secrets.Join = "";
		}
		else if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			if (Singleton<ConfigSound>.Instance && (Singleton<ConfigSound>.Instance.settings.MusicVolume == 0f || Singleton<ConfigSound>.Instance.settings.MasterVolume == 0f))
			{
				this.DiscordActivity.Details = "Chilling in the menus";
			}
			else
			{
				this.DiscordActivity.Details = "Grooving in the menus";
			}
			this.DiscordActivity.Party.Size.CurrentSize = 0;
			this.DiscordActivity.Party.Size.MaxSize = 0;
			this.DiscordActivity.Assets.SmallImage = "";
			this.DiscordActivity.Assets.SmallText = "";
			this.DiscordActivity.Secrets.Join = "";
		}
		else
		{
			this.DiscordActivity.Assets.SmallImage = this.MyColor.ToLower();
			this.DiscordActivity.Assets.SmallText = "Playing as " + this.MyColor;
			if (this.MaxConnections == 0)
			{
				if (this.GameName == "None")
				{
					this.DiscordActivity.Details = "No game loaded";
				}
				else if (VRHMD.isVR)
				{
					this.DiscordActivity.Details = LibString.StripBBCode(this.GameName) + " in VR";
				}
				else
				{
					this.DiscordActivity.Details = (LibString.StripBBCode(this.GameName) ?? "");
				}
				this.DiscordActivity.Party.Size.CurrentSize = 0;
				this.DiscordActivity.Party.Size.MaxSize = 0;
				this.DiscordActivity.Secrets.Join = "";
			}
			else
			{
				this.DiscordActivity.State = "Multiplayer";
				this.DiscordActivity.Party.Size.CurrentSize = this.Connections;
				this.DiscordActivity.Party.Size.MaxSize = this.MaxConnections;
				this.DiscordActivity.Secrets.Join = new JoinLobbyDetails(Singleton<SteamLobbyManager>.Instance.CurrentSteamIDLobby, Network.password).Encode();
				int num = this.Seats;
				if (this.MaxConnections < this.Seats)
				{
					num = this.MaxConnections;
				}
				string text = "";
				if (num > 0)
				{
					if (this.Seated < this.Connections)
					{
						text = string.Format(" (seating\u00a0{0}/{1}\u00a0+{2})", this.Seated, num, this.Connections - this.Seated);
					}
					else
					{
						text = string.Format(" (seating\u00a0{0}/{1})", this.Seated, num);
					}
				}
				if (this.GameName == "None")
				{
					this.DiscordActivity.Details = "No game loaded" + text;
				}
				else if (VRHMD.isVR)
				{
					this.DiscordActivity.Details = LibString.StripBBCode(this.GameName) + " in VR" + text;
				}
				else
				{
					this.DiscordActivity.Details = LibString.StripBBCode(this.GameName) + text;
				}
			}
		}
		this.DiscordActivityLastRefresh = Time.time;
		if (this.UseDiscord)
		{
			this.activityManager.UpdateActivity(this.DiscordActivity, delegate(Discord.Result _)
			{
			});
		}
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00056518 File Offset: 0x00054718
	private static long EpochTime()
	{
		return (long)(DateTime.UtcNow - DiscordController.epochStart).TotalSeconds;
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0005653D File Offset: 0x0005473D
	public void ResetDiscordJoin()
	{
		this.DiscordJoinPassword = "";
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0005654C File Offset: 0x0005474C
	private void OnActivityJoin(string secret)
	{
		Debug.Log("OnJoin " + secret);
		JoinLobbyDetails joinLobbyDetails;
		try
		{
			joinLobbyDetails = new JoinLobbyDetails(secret);
		}
		catch
		{
			Chat.LogError("Failed to parse Discord join secret.", true);
			return;
		}
		this.DiscordJoinPassword = joinLobbyDetails.password;
		Singleton<SteamLobbyManager>.Instance.JoinLobby(joinLobbyDetails.lobbyID);
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x000565AC File Offset: 0x000547AC
	private void OnActivityJoinRequest(ref User user)
	{
		Debug.Log(string.Format("Join Request from: {0}", user.Id));
		Singleton<UIToast>.Instance.AddJoinRequest(user);
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x000565D8 File Offset: 0x000547D8
	public void AcceptJoinRequest(User user)
	{
		this.activityManager.SendRequestReply(user.Id, ActivityJoinRequestReply.Yes, delegate(Discord.Result res)
		{
			if (res != Discord.Result.Ok)
			{
				Debug.LogError("Discord Error: AcceptJoinRequest->SendRequestReply did not succeed. [" + res + "]");
			}
		});
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0005660B File Offset: 0x0005480B
	public void RejectJoinRequest(User user)
	{
		this.activityManager.SendRequestReply(user.Id, ActivityJoinRequestReply.No, delegate(Discord.Result res)
		{
			if (res != Discord.Result.Ok)
			{
				Debug.LogError("Discord Error: RejectJoinRequest->SendRequestReply did not succeed. [" + res + "]");
			}
		});
	}

	// Token: 0x04000880 RID: 2176
	private Discord discord;

	// Token: 0x04000881 RID: 2177
	private ActivityManager activityManager;

	// Token: 0x04000882 RID: 2178
	[NonSerialized]
	public Activity DiscordActivity;

	// Token: 0x04000883 RID: 2179
	[NonSerialized]
	public float DiscordActivityLastRefresh;

	// Token: 0x04000884 RID: 2180
	[NonSerialized]
	public string GameName;

	// Token: 0x04000885 RID: 2181
	[NonSerialized]
	public int Connections;

	// Token: 0x04000886 RID: 2182
	[NonSerialized]
	public int MaxConnections;

	// Token: 0x04000887 RID: 2183
	[NonSerialized]
	public int Seated;

	// Token: 0x04000888 RID: 2184
	[NonSerialized]
	public int Seats;

	// Token: 0x04000889 RID: 2185
	[NonSerialized]
	public string MyColor;

	// Token: 0x0400088A RID: 2186
	[NonSerialized]
	public NetworkPeerMode PeerType;

	// Token: 0x0400088B RID: 2187
	[NonSerialized]
	public string DiscordJoinPassword = "";

	// Token: 0x0400088C RID: 2188
	private List<PlayerState> playerStates;

	// Token: 0x0400088D RID: 2189
	private static readonly DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	// Token: 0x0400088E RID: 2190
	private int partyID;

	// Token: 0x0400088F RID: 2191
	public const string STATUS_CONNECTING = "Connecting...";

	// Token: 0x04000890 RID: 2192
	public const string STATUS_CHILLING = "Chilling in the menus";

	// Token: 0x04000891 RID: 2193
	public const string STATUS_GROOVING = "Grooving in the menus";

	// Token: 0x04000892 RID: 2194
	public const string STATUS_HACKING = "-";

	// Token: 0x04000893 RID: 2195
	public const string STATUS_NO_GAME = "No game loaded";

	// Token: 0x04000894 RID: 2196
	private bool UseDiscord = true;
}
