using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NewNet;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class CustomMusicPlayer : NetworkSingleton<CustomMusicPlayer>
{
	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0004DD5F File Offset: 0x0004BF5F
	public CustomMusicPlayer.MusicPlayerStatus PlayStatus
	{
		get
		{
			return this._playStatus;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0004DD67 File Offset: 0x0004BF67
	// (set) Token: 0x06000B1B RID: 2843 RVA: 0x0004DD97 File Offset: 0x0004BF97
	[TupleElementNames(new string[]
	{
		"AudioUrl",
		"AudioName"
	})]
	public List<ValueTuple<string, string>> AudioLibrary
	{
		[return: TupleElementNames(new string[]
		{
			"AudioUrl",
			"AudioName"
		})]
		get
		{
			if (!this.ShuffleAudio)
			{
				return this.SortedAudioLibrary;
			}
			if (this.CurrentAudioLibrary.Count == 0)
			{
				this.CurrentAudioLibrary = this.SortedAudioLibrary;
			}
			return this.CurrentAudioLibrary;
		}
		[param: TupleElementNames(new string[]
		{
			"AudioUrl",
			"AudioName"
		})]
		set
		{
			this.CurrentAudioLibrary = value;
			this.SortedAudioLibrary = value;
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0004DDA8 File Offset: 0x0004BFA8
	private void Start()
	{
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected += this.OnPlayerDisconnect;
		NetworkEvents.OnDisconnectedFromServer += this.OnDisconnect;
		this.AudioSource = base.gameObject.GetOrAddComponent<AudioSource>();
		this.AudioSource.volume = 0.15f * Singleton<ConfigSound>.Instance._settings.Mp3Volume;
		if (Network.isServer)
		{
			this.ResetPlayerReadyStates();
		}
		this._bInitilized = true;
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0004DE2D File Offset: 0x0004C02D
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected -= this.OnPlayerDisconnect;
		NetworkEvents.OnDisconnectedFromServer -= this.OnDisconnect;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0004DE64 File Offset: 0x0004C064
	private void Update()
	{
		if (!Network.isServer)
		{
			return;
		}
		if (this._playStatus == CustomMusicPlayer.MusicPlayerStatus.Ready)
		{
			this.RPCPlayPauseServer(false);
		}
		if (this._playStatus == CustomMusicPlayer.MusicPlayerStatus.Play && !this.AudioSource.isPlaying)
		{
			base.networkView.RPC<CustomMusicPlayer.MusicPlayerStatus>(RPCTarget.All, new Action<CustomMusicPlayer.MusicPlayerStatus>(this.RPCPlayPause), CustomMusicPlayer.MusicPlayerStatus.Play);
			if (this.PlaylistEntry != -1)
			{
				this.NextAudioLibraryEntry(false);
				this.ResetPlayerReadyStates();
				base.networkView.RPC<string, string, bool>(RPCTarget.All, new Action<string, string, bool>(this.RPCLoadAudio), this.AudioLibrary[this.PlaylistEntry].Item2, this.AudioLibrary[this.PlaylistEntry].Item1, true);
				return;
			}
			base.networkView.RPC<float>(RPCTarget.All, new Action<float>(this.RPCSkipTo), 0f);
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0004DF38 File Offset: 0x0004C138
	public void OnLoad()
	{
		if (Network.isServer)
		{
			this.ResetPlayerReadyStates();
			this.AudioSource.loop = this.RepeatSong;
			this.UiCustomMp3.RepeatHighlight.SetActive(this.RepeatSong);
			if (this.CurrentAudioName != "")
			{
				base.networkView.RPC<string, string, bool>(RPCTarget.All, new Action<string, string, bool>(this.RPCLoadAudio), this.CurrentAudioName, this.CurrentAudioUrl, false);
			}
			if (this.AudioLibrary.Count > 0)
			{
				this.SyncAudioLibrary();
			}
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0004DFC4 File Offset: 0x0004C1C4
	private void OnPlayerConnect(NetworkPlayer networkPlayer)
	{
		if (Network.isServer)
		{
			this._playerReadyStates.Add(networkPlayer.steamID.ToString(), false);
			if (this.CurrentAudioUrl != "")
			{
				float arg = 0f;
				bool isPlaying = this.AudioSource.isPlaying;
				if (this.AudioSource.clip != null)
				{
					arg = this.AudioSource.time;
				}
				base.networkView.RPC<string, string, float, bool>(networkPlayer, new Action<string, string, float, bool>(this.RPCSyncAudioPlayer), this.CurrentAudioName, this.CurrentAudioUrl, arg, isPlaying);
			}
			if (this.AudioLibrary.Count > 0)
			{
				this.SyncAudioLibrary();
			}
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0004E07C File Offset: 0x0004C27C
	private void OnPlayerDisconnect(NetworkPlayer networkPlayer, DisconnectInfo info)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (!this._playerReadyStates.ContainsKey(networkPlayer.steamID.ToString()))
		{
			return;
		}
		this._playerReadyStates.Remove(networkPlayer.steamID.ToString());
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0004E0D5 File Offset: 0x0004C2D5
	private void OnDisconnect(DisconnectInfo info)
	{
		this.Cleanup(false);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0004E0DE File Offset: 0x0004C2DE
	public void Cleanup(bool RPC)
	{
		if (RPC)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCCleanup));
			return;
		}
		this.RPCCleanup();
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0004E104 File Offset: 0x0004C304
	[Remote(Permission.Server)]
	private void RPCCleanup()
	{
		Singleton<CustomLoadingManager>.Instance.Audio.Cleanup(this.CurrentAudioUrl, new Action<CustomAudioContainer>(this.LoadingComplete), true);
		Singleton<CustomLoadingManager>.Instance.Audio.Cleanup(this.CurrentAudioUrl, new Action<CustomAudioContainer>(this.LoadingCompleteNoAutoplay), true);
		foreach (ValueTuple<string, string> valueTuple in this.AudioLibrary)
		{
			Singleton<CustomLoadingManager>.Instance.Audio.Cleanup(valueTuple.Item1, new Action<CustomAudioContainer>(this.LoadingComplete), true);
			Singleton<CustomLoadingManager>.Instance.Audio.Cleanup(valueTuple.Item1, new Action<CustomAudioContainer>(this.LoadingCompleteNoAutoplay), true);
		}
		this.CurrentAudioName = "";
		this.CurrentAudioUrl = "";
		this.AudioLibrary = new List<ValueTuple<string, string>>();
		this.SortedAudioLibrary = new List<ValueTuple<string, string>>();
		this._repeatSong = false;
		this._shuffleAudio = false;
		this._playlistEntry = -1;
		this._playerReadyStates = new Dictionary<string, bool>();
		this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Stop;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0004E22C File Offset: 0x0004C42C
	public void PlayPause()
	{
		if (!this.CheckPermission())
		{
			return;
		}
		if (this.CurrentAudioUrl == string.Empty)
		{
			return;
		}
		base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.RPCPlayPauseServer), true);
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0004E264 File Offset: 0x0004C464
	[Remote(Permission.Server)]
	public void RPCPlayPause(CustomMusicPlayer.MusicPlayerStatus sentStatus)
	{
		UIButton component = this.UiCustomMp3.Play.transform.parent.GetComponent<UIButton>();
		switch (sentStatus)
		{
		case CustomMusicPlayer.MusicPlayerStatus.Play:
			this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Pause;
			this.UiCustomMp3.Play.SetActive(true);
			this.UiCustomMp3.Pause.SetActive(false);
			component.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerGreen];
			component.ThemeNormalAs = UIPalette.UI.PlayerGreen;
			this.AudioSource.Pause();
			return;
		case CustomMusicPlayer.MusicPlayerStatus.Pause:
			this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Play;
			this.UiCustomMp3.Play.SetActive(false);
			this.UiCustomMp3.Pause.SetActive(true);
			component.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerRed];
			component.ThemeNormalAs = UIPalette.UI.PlayerRed;
			this.AudioSource.Play();
			return;
		case CustomMusicPlayer.MusicPlayerStatus.Loading:
			break;
		case CustomMusicPlayer.MusicPlayerStatus.Ready:
			this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Play;
			this.UiCustomMp3.Play.SetActive(false);
			this.UiCustomMp3.Pause.SetActive(true);
			component.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PlayerRed];
			component.ThemeNormalAs = UIPalette.UI.PlayerRed;
			this.AudioSource.time = 0f;
			this.AudioSource.Play();
			break;
		default:
			return;
		}
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0004E3BF File Offset: 0x0004C5BF
	[Remote("Permissions/Music")]
	public void RPCPlayPauseServer(bool logEnable)
	{
		if (this.ArePlayersReady(logEnable))
		{
			base.networkView.RPC<CustomMusicPlayer.MusicPlayerStatus>(RPCTarget.All, new Action<CustomMusicPlayer.MusicPlayerStatus>(this.RPCPlayPause), this._playStatus);
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0004E3E8 File Offset: 0x0004C5E8
	public void SkipStart()
	{
		if (!this.CheckPermission())
		{
			return;
		}
		base.networkView.RPC<bool>(RPCTarget.Server, new Func<bool>(this.RPCSkipStartServer));
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0004E40C File Offset: 0x0004C60C
	[Remote("Permissions/Music")]
	public bool RPCSkipStartServer()
	{
		if (this._playStatus != CustomMusicPlayer.MusicPlayerStatus.Play && this._playStatus != CustomMusicPlayer.MusicPlayerStatus.Pause)
		{
			return false;
		}
		if (this.AudioSource.time >= 3f)
		{
			base.networkView.RPC<float>(RPCTarget.All, new Action<float>(this.RPCSkipTo), 0f);
			return true;
		}
		this.NextAudioLibraryEntry(true);
		if (this.PlaylistEntry == -1)
		{
			return false;
		}
		base.networkView.RPC<string, string, bool>(RPCTarget.All, new Action<string, string, bool>(this.RPCLoadAudio), this.AudioLibrary[this.PlaylistEntry].Item2, this.AudioLibrary[this.PlaylistEntry].Item1, true);
		return true;
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0004E4B9 File Offset: 0x0004C6B9
	public void SkipEnd()
	{
		if (!this.CheckPermission())
		{
			return;
		}
		base.networkView.RPC<bool>(RPCTarget.Server, new Func<bool>(this.RPCSkipEndServer));
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0004E4DC File Offset: 0x0004C6DC
	[Remote("Permissions/Music")]
	public bool RPCSkipEndServer()
	{
		if (this._playStatus != CustomMusicPlayer.MusicPlayerStatus.Play && this._playStatus != CustomMusicPlayer.MusicPlayerStatus.Pause)
		{
			return false;
		}
		this.NextAudioLibraryEntry(false);
		if (this.PlaylistEntry == -1)
		{
			return false;
		}
		this.ResetPlayerReadyStates();
		base.networkView.RPC<string, string, bool>(RPCTarget.All, new Action<string, string, bool>(this.RPCLoadAudio), this.AudioLibrary[this.PlaylistEntry].Item2, this.AudioLibrary[this.PlaylistEntry].Item1, true);
		return true;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x0004E55B File Offset: 0x0004C75B
	public void SkipTo(float time)
	{
		base.networkView.RPC<float>(RPCTarget.Server, new Action<float>(this.RPCSkipToServer), time);
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0004E576 File Offset: 0x0004C776
	[Remote(Permission.Server)]
	public void RPCSkipTo(float time)
	{
		this.AudioSource.time = time;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0004E584 File Offset: 0x0004C784
	[Remote("Permissions/Music")]
	public void RPCSkipToServer(float time)
	{
		base.networkView.RPC<float>(RPCTarget.All, new Action<float>(this.RPCSkipTo), time);
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0004E59F File Offset: 0x0004C79F
	public void AddAudio(string audioName, string url)
	{
		if (!this.CheckPermission())
		{
			return;
		}
		base.networkView.RPC<string, string>(RPCTarget.Server, new Action<string, string>(this.RPCAddAudioServer), audioName, url);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0004E5C4 File Offset: 0x0004C7C4
	[Remote(Permission.Server)]
	public void RPCAddAudio(string audioName, string url)
	{
		this.AudioLibrary.Add(new ValueTuple<string, string>(url, audioName));
		if (this.UiCustomMp3.UiPlayList.activeSelf)
		{
			this.UiCustomMp3.InitializePlayList();
		}
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0004E5F5 File Offset: 0x0004C7F5
	[Remote("Permissions/Music")]
	public void RPCAddAudioServer(string audioName, string url)
	{
		base.networkView.RPC<string, string>(RPCTarget.All, new Action<string, string>(this.RPCAddAudio), audioName, url);
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0004E611 File Offset: 0x0004C811
	public void EditAudio(string audioName, string url)
	{
		if (!this.CheckPermission())
		{
			return;
		}
		base.networkView.RPC<string, string>(RPCTarget.Server, new Action<string, string>(this.RPCEditAudio), audioName, url);
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0004E636 File Offset: 0x0004C836
	[Remote("Permissions/Music")]
	public void RPCEditAudio(string audioName, string url)
	{
		if (Network.isServer)
		{
			this.ResetPlayerReadyStates();
			base.networkView.RPC<string, string, bool>(RPCTarget.All, new Action<string, string, bool>(this.RPCLoadAudio), audioName, url, true);
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0004E660 File Offset: 0x0004C860
	public void EditAudioPlayist(string audioName, string url, string oldEntry)
	{
		if (!this.CheckPermission())
		{
			return;
		}
		base.networkView.RPC<string, string, string>(RPCTarget.Server, new Action<string, string, string>(this.RPCEditAudioPlaylistServer), audioName, url, oldEntry);
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0004E688 File Offset: 0x0004C888
	[Remote(Permission.Server)]
	public void RPCEditAudioPlaylist(string audioName, string url, string oldEntry)
	{
		int audioLibraryEntry = this.GetAudioLibraryEntry(oldEntry);
		this.AudioLibrary.RemoveAt(audioLibraryEntry);
		if (audioLibraryEntry == this.PlaylistEntry)
		{
			this.PlaylistEntry = -1;
		}
		if (audioLibraryEntry < this.PlaylistEntry && this.AudioLibrary.Count > 1)
		{
			this.NextAudioLibraryEntry(true);
		}
		if (audioName != "")
		{
			this.AudioLibrary.Add(new ValueTuple<string, string>(url, audioName));
		}
		if (this.UiCustomMp3.UiPlayList.activeSelf)
		{
			this.UiCustomMp3.InitializePlayList();
		}
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0004E713 File Offset: 0x0004C913
	[Remote("Permissions/Music")]
	public void RPCEditAudioPlaylistServer(string audioName, string url, string oldEntry)
	{
		base.networkView.RPC<string, string, string>(RPCTarget.All, new Action<string, string, string>(this.RPCEditAudioPlaylist), audioName, url, oldEntry);
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0004E730 File Offset: 0x0004C930
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void RPCLoadAudio(string audioName, string url, bool autoplay = true)
	{
		Action <>9__2;
		Wait.Condition(delegate
		{
			Action action;
			if ((action = <>9__2) == null)
			{
				action = (<>9__2 = delegate()
				{
					Singleton<CustomLoadingManager>.Instance.Audio.Cleanup(this.CurrentAudioUrl, new Action<CustomAudioContainer>(this.LoadingComplete), true);
					Singleton<CustomLoadingManager>.Instance.Audio.Cleanup(this.CurrentAudioUrl, new Action<CustomAudioContainer>(this.LoadingCompleteNoAutoplay), true);
					this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Pause;
					this.RPCPlayPause(CustomMusicPlayer.MusicPlayerStatus.Play);
					this.AudioSource.clip = null;
					this.CurrentAudioName = audioName;
					this.CurrentAudioUrl = url;
					if (autoplay)
					{
						Singleton<CustomLoadingManager>.Instance.Audio.Load(url, new Action<CustomAudioContainer>(this.LoadingComplete), audioName, CustomLoadingManager.LoadType.Auto, true);
						return;
					}
					Singleton<CustomLoadingManager>.Instance.Audio.Load(url, new Action<CustomAudioContainer>(this.LoadingCompleteNoAutoplay), audioName, CustomLoadingManager.LoadType.Auto, true);
				});
			}
			Wait.Frames(action, 1);
		}, () => this._bInitilized, float.PositiveInfinity, null);
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0004E784 File Offset: 0x0004C984
	private void LoadingComplete(CustomAudioContainer container)
	{
		if (container.AudioClip == null)
		{
			return;
		}
		this.SetAudioDisplayName(this.CurrentAudioName, container.AudioClip);
		this.AudioSource.clip = container.AudioClip;
		this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Ready;
		this.UiCustomMp3.Play.SetActive(true);
		this.UiCustomMp3.Pause.SetActive(false);
		base.networkView.RPC<string, string>(RPCTarget.Server, new Action<string, string>(this.NotifyServerReady), NetworkSingleton<PlayerManager>.Instance.MyPlayerState().steamId, this.CurrentAudioUrl);
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0004E81C File Offset: 0x0004CA1C
	private void LoadingCompleteNoAutoplay(CustomAudioContainer container)
	{
		if (container.AudioClip == null)
		{
			return;
		}
		this.SetAudioDisplayName(this.CurrentAudioName, container.AudioClip);
		this.AudioSource.clip = container.AudioClip;
		this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Pause;
		this.UiCustomMp3.Play.SetActive(true);
		this.UiCustomMp3.Pause.SetActive(false);
		base.networkView.RPC<string, string>(RPCTarget.Server, new Action<string, string>(this.NotifyServerReady), NetworkSingleton<PlayerManager>.Instance.MyPlayerState().steamId, this.CurrentAudioUrl);
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0004E8B4 File Offset: 0x0004CAB4
	private void SetAudioDisplayName(string audioTitle, AudioClip audioClip)
	{
		if (audioClip.name == "")
		{
			audioClip.name = "Unknown Title";
		}
		if (audioTitle == "Unknown Title" && audioClip.name != audioTitle)
		{
			audioTitle = audioClip.name;
			this.CurrentAudioName = audioTitle;
			int i = 0;
			while (i < this.AudioLibrary.Count)
			{
				ValueTuple<string, string> valueTuple = this.AudioLibrary[i];
				string item = valueTuple.Item1;
				if (valueTuple.Item2 == "Unknown Title" && item == this.CurrentAudioUrl)
				{
					this.AudioLibrary[i] = new ValueTuple<string, string>(this.CurrentAudioUrl, audioTitle);
					this.SyncAudioLibrary();
					if (this.UiCustomMp3.UiPlayList.activeSelf)
					{
						this.UiCustomMp3.InitializePlayList();
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		this.UiCustomMp3.Title.text = audioTitle;
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x0004E9A7 File Offset: 0x0004CBA7
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void NotifyServerReady(string steamId, string url)
	{
		if (url != this.CurrentAudioUrl)
		{
			return;
		}
		this._playerReadyStates[steamId] = true;
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x0004E9C5 File Offset: 0x0004CBC5
	[Remote(Permission.Server)]
	public void RPCSyncAudioPlayer(string audioName, string audioUrl, float time, bool isPlaying)
	{
		base.StartCoroutine(this.CoroutineSyncAudioPlayer(audioName, audioUrl, time, isPlaying));
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x0004E9D9 File Offset: 0x0004CBD9
	private IEnumerator CoroutineSyncAudioPlayer(string audioName, string audioUrl, float time, bool isPlaying)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		this.RPCLoadAudio(audioName, audioUrl, false);
		while (this._playStatus != CustomMusicPlayer.MusicPlayerStatus.Ready && this._playStatus != CustomMusicPlayer.MusicPlayerStatus.Pause)
		{
			yield return null;
		}
		if (isPlaying)
		{
			stopwatch.Stop();
			time += (float)stopwatch.Elapsed.Seconds;
			if (this.AudioSource.clip.length > time)
			{
				this.AudioSource.time = time;
				this._playStatus = CustomMusicPlayer.MusicPlayerStatus.Play;
				this.UiCustomMp3.Play.SetActive(false);
				this.UiCustomMp3.Pause.SetActive(true);
				this.AudioSource.Play();
			}
		}
		else
		{
			stopwatch.Stop();
			time += (float)stopwatch.Elapsed.Seconds;
			if (this.AudioSource.clip.length > time)
			{
				this.AudioSource.time = time;
			}
		}
		yield break;
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x0004EA08 File Offset: 0x0004CC08
	public void SyncAudioLibrary()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (ValueTuple<string, string> valueTuple in this.AudioLibrary)
		{
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			list.Add(item2);
			list2.Add(item);
		}
		base.networkView.RPC<List<string>, List<string>>(RPCTarget.Others, new Action<List<string>, List<string>>(this.RPCSyncAudioLibrary), list, list2);
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0004EA98 File Offset: 0x0004CC98
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void RPCSyncAudioLibrary(List<string> titles, List<string> urls)
	{
		this.AudioLibrary = new List<ValueTuple<string, string>>();
		for (int i = 0; i < titles.Count; i++)
		{
			this.AudioLibrary.Add(new ValueTuple<string, string>(urls[i], titles[i]));
		}
		if (this.UiCustomMp3.UiPlayList.activeSelf)
		{
			this.UiCustomMp3.InitializePlayList();
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0004EAFC File Offset: 0x0004CCFC
	// (set) Token: 0x06000B41 RID: 2881 RVA: 0x0004EB04 File Offset: 0x0004CD04
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool RepeatSong
	{
		get
		{
			return this._repeatSong;
		}
		set
		{
			if (value == this._repeatSong)
			{
				return;
			}
			this._repeatSong = value;
			this.AudioSource.loop = value;
			this.UiCustomMp3.RepeatHighlight.SetActive(value);
			base.DirtySync("RepeatSong");
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0004EB3F File Offset: 0x0004CD3F
	// (set) Token: 0x06000B43 RID: 2883 RVA: 0x0004EB48 File Offset: 0x0004CD48
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public bool ShuffleAudio
	{
		get
		{
			return this._shuffleAudio;
		}
		set
		{
			if (value == this._shuffleAudio)
			{
				return;
			}
			this._shuffleAudio = value;
			this.UiCustomMp3.ShuffleHighlight.SetActive(this._shuffleAudio);
			if (this._shuffleAudio && Network.isServer && this.SortedAudioLibrary.Count > 0)
			{
				this.CurrentAudioLibrary = new List<ValueTuple<string, string>>(this.SortedAudioLibrary);
				this.CurrentAudioLibrary.Randomize<ValueTuple<string, string>>();
				if (this.CurrentAudioUrl != "")
				{
					this.PlaylistEntry = this.GetAudioLibraryEntry(this.CurrentAudioUrl);
				}
				this.SyncAudioLibrary();
			}
			if (!this._shuffleAudio && Network.isServer)
			{
				if (this.CurrentAudioLibrary.Count > this.SortedAudioLibrary.Count)
				{
					foreach (ValueTuple<string, string> valueTuple in this.CurrentAudioLibrary)
					{
						string item = valueTuple.Item1;
						string item2 = valueTuple.Item2;
						if (!this.SortedAudioLibrary.Contains(new ValueTuple<string, string>(item, item2)))
						{
							this.SortedAudioLibrary.Add(new ValueTuple<string, string>(item, item2));
						}
					}
				}
				if (this.CurrentAudioUrl != "")
				{
					this.PlaylistEntry = this.GetAudioLibraryEntry(this.CurrentAudioUrl);
				}
				this.SyncAudioLibrary();
			}
			base.DirtySync("ShuffleAudio");
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0004ECB4 File Offset: 0x0004CEB4
	// (set) Token: 0x06000B45 RID: 2885 RVA: 0x0004ECBC File Offset: 0x0004CEBC
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public int PlaylistEntry
	{
		get
		{
			return this._playlistEntry;
		}
		set
		{
			if (value == this._playlistEntry)
			{
				return;
			}
			this._playlistEntry = value;
			this.UiCustomMp3.HighlightEntry(value);
			base.DirtySync("PlaylistEntry");
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x0004ECE8 File Offset: 0x0004CEE8
	private void ResetPlayerReadyStates()
	{
		this._playerReadyStates = new Dictionary<string, bool>();
		foreach (PlayerState playerState in NetworkSingleton<PlayerManager>.Instance.PlayersList)
		{
			if (!this._playerReadyStates.ContainsKey(playerState.steamId))
			{
				this._playerReadyStates.Add(playerState.steamId, false);
			}
		}
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x0004ED68 File Offset: 0x0004CF68
	public bool ArePlayersReady(bool logEnable)
	{
		bool result = true;
		foreach (KeyValuePair<string, bool> keyValuePair in this._playerReadyStates)
		{
			if (!keyValuePair.Value)
			{
				if (logEnable)
				{
					Chat.Log("Not all players have loaded the Audioclip!", Colour.White, ChatMessageType.System, true);
				}
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0004EDD8 File Offset: 0x0004CFD8
	public int GetAudioLibraryEntry(string url)
	{
		int num = 0;
		using (List<ValueTuple<string, string>>.Enumerator enumerator = this.AudioLibrary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Item1 == url)
				{
					break;
				}
				num++;
			}
		}
		if (num > this.AudioLibrary.Count)
		{
			return -1;
		}
		return num;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x0004EE4C File Offset: 0x0004D04C
	private void NextAudioLibraryEntry(bool reverse = false)
	{
		if (this.PlaylistEntry == -1)
		{
			return;
		}
		if (this.AudioLibrary.Count == 0)
		{
			this.PlaylistEntry = -1;
			return;
		}
		if (!reverse)
		{
			if (this.PlaylistEntry + 1 == this.AudioLibrary.Count)
			{
				this.PlaylistEntry = 0;
				return;
			}
			int playlistEntry = this.PlaylistEntry;
			this.PlaylistEntry = playlistEntry + 1;
			return;
		}
		else
		{
			if (this.PlaylistEntry - 1 < 0)
			{
				this.PlaylistEntry = this.AudioLibrary.Count;
				return;
			}
			int playlistEntry = this.PlaylistEntry;
			this.PlaylistEntry = playlistEntry - 1;
			return;
		}
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x0004EED6 File Offset: 0x0004D0D6
	private bool CheckPermission()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Music, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Music Player (Music)");
			return false;
		}
		return true;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0004EEF7 File Offset: 0x0004D0F7
	public void SetVolume(float value)
	{
		this.Volume = value;
		this.AudioSource.volume = value * 0.3f * Singleton<ConfigSound>.Instance._settings.Mp3Volume;
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0004EF22 File Offset: 0x0004D122
	public bool HasSongsLoaded()
	{
		return this.AudioLibrary.Count > 0 || this.CurrentAudioUrl != "";
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0004EF44 File Offset: 0x0004D144
	[Remote(Permission.Admin, serializationMethod = SerializationMethod.Json)]
	public void InitFromState(MusicPlayerState musicPlayerState)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<MusicPlayerState>(RPCTarget.Server, new Action<MusicPlayerState>(this.InitFromState), musicPlayerState);
			return;
		}
		if (this.HasSongsLoaded())
		{
			this.Cleanup(true);
		}
		if (musicPlayerState != null)
		{
			this.RepeatSong = musicPlayerState.RepeatSong;
			this.PlaylistEntry = musicPlayerState.PlaylistEntry;
			this.CurrentAudioName = musicPlayerState.CurrentAudioTitle;
			this.CurrentAudioUrl = musicPlayerState.CurrentAudioURL;
			this.SortedAudioLibrary = musicPlayerState.AudioLibrary;
			if (this.CurrentAudioUrl != "")
			{
				this.OnLoad();
			}
		}
	}

	// Token: 0x040007D2 RID: 2002
	public UICustomMusicPlayer UiCustomMp3;

	// Token: 0x040007D3 RID: 2003
	public AudioSource AudioSource;

	// Token: 0x040007D4 RID: 2004
	public string CurrentAudioName = "";

	// Token: 0x040007D5 RID: 2005
	public string CurrentAudioUrl = "";

	// Token: 0x040007D6 RID: 2006
	[TupleElementNames(new string[]
	{
		"AudioUrl",
		"AudioName"
	})]
	public List<ValueTuple<string, string>> CurrentAudioLibrary = new List<ValueTuple<string, string>>();

	// Token: 0x040007D7 RID: 2007
	[TupleElementNames(new string[]
	{
		"AudioUrl",
		"AudioName"
	})]
	public List<ValueTuple<string, string>> SortedAudioLibrary = new List<ValueTuple<string, string>>();

	// Token: 0x040007D8 RID: 2008
	private const float MaxVolume = 0.3f;

	// Token: 0x040007D9 RID: 2009
	[HideInInspector]
	public float Volume;

	// Token: 0x040007DA RID: 2010
	private bool _bInitilized;

	// Token: 0x040007DB RID: 2011
	private bool _repeatSong;

	// Token: 0x040007DC RID: 2012
	private bool _shuffleAudio;

	// Token: 0x040007DD RID: 2013
	private int _playlistEntry = -1;

	// Token: 0x040007DE RID: 2014
	private Dictionary<string, bool> _playerReadyStates = new Dictionary<string, bool>();

	// Token: 0x040007DF RID: 2015
	private CustomMusicPlayer.MusicPlayerStatus _playStatus;

	// Token: 0x020005C9 RID: 1481
	public enum MusicPlayerStatus
	{
		// Token: 0x040026B3 RID: 9907
		Stop,
		// Token: 0x040026B4 RID: 9908
		Play,
		// Token: 0x040026B5 RID: 9909
		Pause,
		// Token: 0x040026B6 RID: 9910
		Loading,
		// Token: 0x040026B7 RID: 9911
		Ready
	}
}
