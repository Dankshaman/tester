using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

// Token: 0x02000197 RID: 407
public class LuaMusicPlayer
{
	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06001402 RID: 5122 RVA: 0x000847D5 File Offset: 0x000829D5
	// (set) Token: 0x06001403 RID: 5123 RVA: 0x000847E2 File Offset: 0x000829E2
	public bool repeat_track
	{
		get
		{
			return this._musicPlayer.RepeatSong;
		}
		set
		{
			this._musicPlayer.RepeatSong = value;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06001404 RID: 5124 RVA: 0x000847F0 File Offset: 0x000829F0
	// (set) Token: 0x06001405 RID: 5125 RVA: 0x000847FD File Offset: 0x000829FD
	public bool shuffle
	{
		get
		{
			return this._musicPlayer.ShuffleAudio;
		}
		set
		{
			this._musicPlayer.ShuffleAudio = value;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001406 RID: 5126 RVA: 0x0008480B File Offset: 0x00082A0B
	// (set) Token: 0x06001407 RID: 5127 RVA: 0x00084818 File Offset: 0x00082A18
	public int playlist_index
	{
		get
		{
			return this._musicPlayer.PlaylistEntry;
		}
		set
		{
			if (value < -1 || value >= this._musicPlayer.AudioLibrary.Count)
			{
				Chat.LogError("Error in MusicPlayer.playlistIndex: Bad Index (value is less than -1 or higher than the audio library count).", true);
				return;
			}
			this._musicPlayer.PlaylistEntry = value;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001408 RID: 5128 RVA: 0x00084849 File Offset: 0x00082A49
	// (set) Token: 0x06001409 RID: 5129 RVA: 0x00084851 File Offset: 0x00082A51
	public int playlistIndex
	{
		get
		{
			return this.playlist_index;
		}
		set
		{
			this.playlist_index = value;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x0600140A RID: 5130 RVA: 0x0008485A File Offset: 0x00082A5A
	public bool loaded
	{
		get
		{
			return this._musicPlayer.ArePlayersReady(false);
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x0600140B RID: 5131 RVA: 0x00084868 File Offset: 0x00082A68
	public string player_status
	{
		get
		{
			return this._musicPlayer.PlayStatus.ToString();
		}
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x0008488E File Offset: 0x00082A8E
	public bool play()
	{
		if (this._musicPlayer.PlayStatus == CustomMusicPlayer.MusicPlayerStatus.Stop)
		{
			return false;
		}
		if (!this.loaded)
		{
			return false;
		}
		if (this._musicPlayer.PlayStatus == CustomMusicPlayer.MusicPlayerStatus.Play)
		{
			return true;
		}
		this._musicPlayer.RPCPlayPauseServer(false);
		return true;
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000848C6 File Offset: 0x00082AC6
	public bool pause()
	{
		if (this._musicPlayer.PlayStatus == CustomMusicPlayer.MusicPlayerStatus.Pause)
		{
			return true;
		}
		if (this._musicPlayer.PlayStatus != CustomMusicPlayer.MusicPlayerStatus.Play)
		{
			return false;
		}
		this._musicPlayer.RPCPlayPauseServer(false);
		return true;
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x000848F5 File Offset: 0x00082AF5
	public bool skipForward()
	{
		return this._musicPlayer.RPCSkipEndServer();
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x00084902 File Offset: 0x00082B02
	public bool skipBack()
	{
		return this._musicPlayer.RPCSkipStartServer();
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x0008490F File Offset: 0x00082B0F
	public Dictionary<string, string> getCurrentAudioclip()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["url"] = this._musicPlayer.CurrentAudioUrl;
		dictionary["title"] = this._musicPlayer.CurrentAudioName;
		return dictionary;
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x00084942 File Offset: 0x00082B42
	public bool setCurrentAudioclip(Dictionary<string, string> dictionary)
	{
		if (!this.ValidAudioDictionary(dictionary, "Error in MusicPlayer.loadAudioclip: "))
		{
			return false;
		}
		this._musicPlayer.RPCEditAudio(dictionary["title"], dictionary["url"]);
		return true;
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x00084978 File Offset: 0x00082B78
	public List<Dictionary<string, string>> getPlaylist()
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		foreach (ValueTuple<string, string> valueTuple in this._musicPlayer.AudioLibrary)
		{
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			List<Dictionary<string, string>> list2 = list;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["url"] = item;
			dictionary["title"] = item2;
			list2.Add(dictionary);
		}
		return list;
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x00084A00 File Offset: 0x00082C00
	public bool setPlaylist(List<Dictionary<string, string>> luaAudioLibrary)
	{
		List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
		foreach (Dictionary<string, string> dictionary in luaAudioLibrary)
		{
			if (this.ValidAudioDictionary(dictionary, "Error in MusicPlayer.setPlaylist: "))
			{
				list.Add(new ValueTuple<string, string>(dictionary["url"], dictionary["title"]));
			}
		}
		this._musicPlayer.AudioLibrary = list;
		this._musicPlayer.SyncAudioLibrary();
		return true;
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x00084A94 File Offset: 0x00082C94
	[MoonSharpHidden]
	private bool ValidAudioDictionary(Dictionary<string, string> dictionary, string ErrorMsg)
	{
		if (!dictionary.ContainsKey("url"))
		{
			Chat.LogError(ErrorMsg + "Table does not contain the key 'url'.", true);
			return false;
		}
		if (string.IsNullOrEmpty(dictionary["url"]))
		{
			Chat.LogError(ErrorMsg + "Table.url is empty.", true);
			return false;
		}
		if (!dictionary.ContainsKey("title"))
		{
			Chat.LogError(ErrorMsg + "Table does not contain the key 'title'.", true);
			return false;
		}
		if (string.IsNullOrEmpty(dictionary["title"]))
		{
			Chat.LogError(ErrorMsg + "Table.title is empty.", true);
			return false;
		}
		return true;
	}

	// Token: 0x04000BC4 RID: 3012
	[MoonSharpHidden]
	private readonly CustomMusicPlayer _musicPlayer = NetworkSingleton<CustomMusicPlayer>.Instance;
}
