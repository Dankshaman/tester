using System;
using NewNet;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class Mp3PlayerScript : NetworkBehavior
{
	// Token: 0x06001609 RID: 5641 RVA: 0x000993B4 File Offset: 0x000975B4
	private void Start()
	{
		Debug.Log("theVolume: " + this.theVolume);
		if (Network.isServer)
		{
			int menu = 0;
			string menuTitle = "GENRES";
			string songTitle;
			string genre;
			float num;
			float time;
			bool isPlaying;
			if (!this.audio.clip)
			{
				songTitle = "";
				genre = "";
				num = 0.2f;
				time = 0f;
				isPlaying = false;
			}
			else
			{
				songTitle = this.audio.clip.name;
				genre = this.PlayingSong.GetComponent<Mp3PlayerSong>().genre;
				num = this.theVolume;
				time = this.audio.time;
				isPlaying = this.audio.isPlaying;
			}
			this.SetVolumeBars(num);
			MenuStruct activeMenu = this.GetActiveMenu();
			if (activeMenu.menu == this.GenreGO)
			{
				menu = 0;
				menuTitle = activeMenu.title;
			}
			else if (activeMenu.menu == this.SongListGO)
			{
				menu = 1;
				menuTitle = activeMenu.title;
			}
			else if (activeMenu.menu == this.PlayingSong)
			{
				menu = 2;
				menuTitle = activeMenu.title;
			}
			else if (activeMenu.menu == this.VolumeMenu)
			{
				menu = 3;
				menuTitle = activeMenu.title;
			}
			base.networkView.RPC<Mp3PlayerScript.State>(RPCTarget.Others, new Action<Mp3PlayerScript.State>(this.SyncPlayer), new Mp3PlayerScript.State(songTitle, genre, num, time, isPlaying, menu, menuTitle, this.loopOne));
		}
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x00099522 File Offset: 0x00097722
	public void Awake()
	{
		this.theVolume = 0.2f;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x00099540 File Offset: 0x00097740
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x00099553 File Offset: 0x00097753
	private void Update()
	{
		this.audio.volume = this.theVolume * SoundScript.GLOBAL_MP3_MULTI;
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x0009956C File Offset: 0x0009776C
	public void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			int menu = 0;
			string menuTitle = "GENRES";
			string songTitle;
			string genre;
			float volume;
			float time;
			bool isPlaying;
			if (!this.audio.clip)
			{
				songTitle = "";
				genre = "";
				volume = 0.2f;
				time = 0f;
				isPlaying = false;
			}
			else
			{
				songTitle = this.audio.clip.name;
				genre = this.PlayingSong.GetComponent<Mp3PlayerSong>().genre;
				volume = this.theVolume;
				time = this.audio.time;
				isPlaying = this.audio.isPlaying;
			}
			MenuStruct activeMenu = this.GetActiveMenu();
			if (activeMenu.menu == this.GenreGO)
			{
				menu = 0;
				menuTitle = activeMenu.title;
			}
			else if (activeMenu.menu == this.SongListGO)
			{
				menu = 1;
				menuTitle = activeMenu.title;
			}
			else if (activeMenu.menu == this.PlayingSong)
			{
				menu = 2;
				menuTitle = activeMenu.title;
			}
			else if (activeMenu.menu == this.VolumeMenu)
			{
				menu = 3;
				menuTitle = activeMenu.title;
			}
			base.networkView.RPC<Mp3PlayerScript.State>(player, new Action<Mp3PlayerScript.State>(this.SyncPlayer), new Mp3PlayerScript.State(songTitle, genre, volume, time, isPlaying, menu, menuTitle, this.loopOne));
		}
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x000996BC File Offset: 0x000978BC
	[Remote("Permissions/Digital")]
	public void SyncPlayer(Mp3PlayerScript.State state)
	{
		if (state.songTitle != "")
		{
			this.currentGenre = state.genre;
			this.currentSong = this.FindSongByTitleAndGenre(state.songTitle, state.genre);
			Mp3PlayerSong component = this.PlayingSong.GetComponent<Mp3PlayerSong>();
			component.songName = this.currentSong;
			component.genre = this.currentGenre;
			if (this.loopOne)
			{
				this.LoopType.text = "Song";
				this.LoopOneButton.defaultColor = new Color(0f, 0.92156863f, 1f);
				this.LoopOneButton.UpdateColor(true);
				this.LoopAllButton.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.High];
				this.LoopAllButton.UpdateColor(true);
			}
			component.isPlaying = state.isPlaying;
			state.volume = (float)Math.Round((double)state.volume, 1);
			this.theVolume = state.volume;
			this.SetVolumeBars(state.volume);
			this.audio.clip = (AudioClip)Resources.Load(this.currentSong);
			this.audio.volume = this.theVolume * SoundScript.GLOBAL_MP3_MULTI;
			this.audio.time = state.time;
			this.SongListScript = this.SongListGO.GetComponent<Mp3PlayerSongList>();
			string[] songsFromGenre = this.GetSongsFromGenre(state.genre);
			this.SongListScript.PopulateSongList(songsFromGenre, state.genre);
			if (state.isPlaying)
			{
				this.audio.Play();
			}
		}
		this.HideAllMenus();
		this.TitleLabel.text = state.menuTitle;
		switch (state.menu)
		{
		case 0:
			this.GenreGO.SetActive(true);
			this.ScrollBar.SetActive(false);
			return;
		case 1:
			this.SongListGO.SetActive(true);
			this.ScrollBar.SetActive(true);
			return;
		case 2:
			this.PlayingSong.SetActive(true);
			this.ScrollBar.SetActive(false);
			return;
		case 3:
			this.VolumeMenu.SetActive(true);
			this.ScrollBar.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x000998EC File Offset: 0x00097AEC
	public void PlayPause()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.PlayPauseServerRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.PlayPauseServerRPC));
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x00099949 File Offset: 0x00097B49
	[Remote("Permissions/Digital")]
	public void PlayPauseServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.PlayPauseRPC));
		}
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x0009996C File Offset: 0x00097B6C
	[Remote("Permissions/Digital")]
	public void PlayPauseRPC()
	{
		if (this.audio)
		{
			if (this.audio.isPlaying)
			{
				this.audio.Pause();
				if (this.PlayingSongScript != null)
				{
					this.PlayingSongScript.isPaused = true;
					return;
				}
			}
			else if (this.audio.clip)
			{
				this.audio.Play();
				if (this.PlayingSongScript != null)
				{
					this.PlayingSongScript.isPaused = false;
				}
			}
		}
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x000999F0 File Offset: 0x00097BF0
	public void Menu()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.MenuServerRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.MenuServerRPC));
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x00099A4D File Offset: 0x00097C4D
	[Remote("Permissions/Digital")]
	public void MenuServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.MenuRPC));
		}
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x00099A70 File Offset: 0x00097C70
	[Remote("Permissions/Digital")]
	public void MenuRPC()
	{
		if (this.GenreGO.activeSelf && this.audio.clip)
		{
			this.HideAllMenus();
			this.PlayingSong.SetActive(true);
			this.ScrollBar.SetActive(false);
			this.TitleLabel.text = this.audio.clip.name.Replace("MP3_", "");
			this.TitleLabel.text = this.TitleLabel.text.Replace("_", " ");
			return;
		}
		if (!this.GenreGO.activeSelf)
		{
			this.HideAllMenus();
			this.TitleLabel.text = "GENRES";
			this.GenreGO.SetActive(true);
			this.ScrollBar.SetActive(false);
		}
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x00099B48 File Offset: 0x00097D48
	public void Left()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.LeftServerRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.LeftServerRPC));
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x00099BA5 File Offset: 0x00097DA5
	[Remote("Permissions/Digital")]
	public void LeftServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.LeftRPC));
		}
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x00099BC8 File Offset: 0x00097DC8
	[Remote("Permissions/Digital")]
	public void LeftRPC()
	{
		if (this.VolumeMenu.activeSelf && this.theVolume > 0f)
		{
			this.theVolume -= 0.1f;
			this.SetVolumeBars(this.theVolume);
			return;
		}
		if (!this.VolumeMenu.activeSelf && this.SongListScript != null && this.audio.clip != null)
		{
			this.SongListScript.PlayPreviousSong();
		}
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x00099C48 File Offset: 0x00097E48
	public void Right()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.RightServerRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.RightServerRPC));
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x00099CA5 File Offset: 0x00097EA5
	[Remote("Permissions/Digital")]
	public void RightServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RightRPC));
		}
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00099CC8 File Offset: 0x00097EC8
	[Remote("Permissions/Digital")]
	public void RightRPC()
	{
		if (this.VolumeMenu.activeSelf && this.theVolume < 1f)
		{
			this.theVolume += 0.1f;
			this.SetVolumeBars(this.theVolume);
			return;
		}
		if (!this.VolumeMenu.activeSelf && this.SongListScript != null && this.audio.clip != null)
		{
			this.SongListScript.PlayNextSong();
		}
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x00099D48 File Offset: 0x00097F48
	public void SetVolumeBars(float v)
	{
		if (this.VolumeGrid != null)
		{
			v *= 10f;
			int num = (int)Mathf.Round(v);
			for (int i = 0; i < this.VolumeGrid.transform.childCount; i++)
			{
				if (i < num)
				{
					this.VolumeGrid.transform.GetChild(i).gameObject.SetActive(true);
				}
				else
				{
					this.VolumeGrid.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00099DD0 File Offset: 0x00097FD0
	public void Volume()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.VolumeServerRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.VolumeServerRPC));
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00099E2D File Offset: 0x0009802D
	[Remote("Permissions/Digital")]
	public void VolumeServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.VolumeRPC));
		}
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x00099E50 File Offset: 0x00098050
	[Remote("Permissions/Digital")]
	public void VolumeRPC()
	{
		if (this.VolumeMenu.activeSelf)
		{
			this.HideAllMenus();
			if (this.previousMenu == null)
			{
				this.GenreGO.SetActive(true);
				this.TitleLabel.text = "GENRES";
				this.ScrollBar.SetActive(false);
				return;
			}
			this.previousMenu.menu.SetActive(true);
			this.TitleLabel.text = this.previousMenu.title;
			if (this.previousMenu.menu != this.VolumeMenu && this.previousMenu.menu != this.PlayingSong && this.previousMenu.menu != this.GenreGO)
			{
				this.ScrollBar.SetActive(true);
				return;
			}
		}
		else
		{
			this.previousMenu = this.GetActiveMenu();
			this.HideAllMenus();
			this.VolumeMenu.SetActive(true);
			this.TitleLabel.text = "VOLUME";
			this.ScrollBar.SetActive(false);
		}
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x00099F64 File Offset: 0x00098164
	public void LoopOne()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.LoopOneRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.LoopOneServerRPC));
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x00099FC1 File Offset: 0x000981C1
	[Remote("Permissions/Digital")]
	public void LoopOneServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.LoopOneRPC));
		}
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x00099FE4 File Offset: 0x000981E4
	[Remote("Permissions/Digital")]
	public void LoopOneRPC()
	{
		this.loopOne = true;
		this.LoopType.text = "Song";
		this.LoopOneButton.defaultColor = new Color(0f, 0.92156863f, 1f);
		this.LoopOneButton.UpdateColor(true);
		this.LoopAllButton.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.High];
		this.LoopAllButton.UpdateColor(true);
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x0009A060 File Offset: 0x00098260
	public void LoopAll()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.LoopAllRPC();
			return;
		}
		base.networkView.RPC(RPCTarget.Server, new Action(this.LoopAllServerRPC));
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x0009A0BD File Offset: 0x000982BD
	[Remote("Permissions/Digital")]
	public void LoopAllServerRPC()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.LoopAllRPC));
		}
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x0009A0E0 File Offset: 0x000982E0
	[Remote("Permissions/Digital")]
	public void LoopAllRPC()
	{
		this.loopOne = false;
		this.LoopType.text = "Genre";
		this.LoopAllButton.defaultColor = new Color(0f, 0.92156863f, 1f);
		this.LoopAllButton.UpdateColor(true);
		this.LoopOneButton.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.High];
		this.LoopOneButton.UpdateColor(true);
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x0009A15C File Offset: 0x0009835C
	public void SetGenre(string genre)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.SetGenreServerRPC(genre);
			return;
		}
		base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.SetGenreServerRPC), genre);
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x0009A1BB File Offset: 0x000983BB
	[Remote("Permissions/Digital")]
	public void SetGenreServerRPC(string genre)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.SetGenreRPC), genre);
		}
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x0009A1E0 File Offset: 0x000983E0
	[Remote("Permissions/Digital")]
	public void SetGenreRPC(string genre)
	{
		this.TitleLabel.text = genre;
		this.HideAllMenus();
		this.SongListGO.SetActive(true);
		this.ScrollBar.SetActive(true);
		this.SongListScript = this.SongListGO.GetComponent<Mp3PlayerSongList>();
		string[] songsFromGenre = this.GetSongsFromGenre(genre);
		this.SongListScript.PopulateSongList(songsFromGenre, genre);
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x0009A240 File Offset: 0x00098440
	public string[] GetSongsFromGenre(string genre)
	{
		for (int i = 0; i < this.GenreGrid.transform.childCount; i++)
		{
			if (this.GenreGrid.transform.GetChild(i).GetComponent<UILabel>().text == genre)
			{
				return this.GenreGrid.transform.GetChild(i).GetComponent<Mp3PlayerGenre>().songNames;
			}
		}
		return null;
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x0009A2A8 File Offset: 0x000984A8
	public void HideAllMenus()
	{
		this.GenreGO.SetActive(false);
		this.SongListGO.SetActive(false);
		this.PlayingSong.SetActive(false);
		this.VolumeMenu.SetActive(false);
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x0009A2DC File Offset: 0x000984DC
	public MenuStruct GetActiveMenu()
	{
		MenuStruct menuStruct = new MenuStruct();
		if (this.GenreGO.activeSelf)
		{
			menuStruct.menu = this.GenreGO;
			menuStruct.title = this.TitleLabel.text;
			return menuStruct;
		}
		if (this.SongListGO.activeSelf)
		{
			menuStruct.menu = this.SongListGO;
			menuStruct.title = this.TitleLabel.text;
			return menuStruct;
		}
		if (this.PlayingSong.activeSelf)
		{
			menuStruct.menu = this.PlayingSong;
			menuStruct.title = this.TitleLabel.text;
			return menuStruct;
		}
		if (this.VolumeMenu.activeSelf)
		{
			menuStruct.menu = this.VolumeMenu;
			menuStruct.title = this.TitleLabel.text;
			return menuStruct;
		}
		return null;
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x0009A3A0 File Offset: 0x000985A0
	public void SelectSong(string songTitle, string genre)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the MP3 Player (Digital)");
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (Network.isServer)
		{
			this.SelectSongServerRPC(songTitle, genre);
			return;
		}
		base.networkView.RPC<string, string>(RPCTarget.Server, new Action<string, string>(this.SelectSongServerRPC), songTitle, genre);
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x0009A401 File Offset: 0x00098601
	[Remote("Permissions/Digital")]
	public void SelectSongServerRPC(string songTitle, string genre)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<string, string>(RPCTarget.All, new Action<string, string>(this.SelectSongRPC), songTitle, genre);
		}
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x0009A424 File Offset: 0x00098624
	[Remote("Permissions/Digital")]
	public void SelectSongRPC(string songTitle, string genre)
	{
		this.currentGenre = genre;
		this.currentSong = this.FindSongByTitleAndGenre(songTitle, genre);
		this.HideAllMenus();
		this.ScrollBar.SetActive(false);
		this.PlayingSong.SetActive(true);
		this.TitleLabel.text = this.currentSong.Replace("MP3_", "");
		this.TitleLabel.text = this.TitleLabel.text.Replace("_", " ");
		Mp3PlayerSong component = this.PlayingSong.GetComponent<Mp3PlayerSong>();
		component.songName = this.currentSong;
		component.genre = genre;
		component.Play();
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x0009A4CC File Offset: 0x000986CC
	public void PlayNextSong()
	{
		if (Network.isServer)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.PlayNextSongRPC));
		}
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x0009A4ED File Offset: 0x000986ED
	[Remote("Permissions/Digital")]
	public void PlayNextSongRPC()
	{
		if (this.SongListScript != null && this.audio.clip != null)
		{
			this.SongListScript.PlayNextSong();
		}
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x0009A51C File Offset: 0x0009871C
	public string FindSongByTitleAndGenre(string songTitle, string genre)
	{
		for (int i = 0; i < this.GenreGrid.transform.childCount; i++)
		{
			if (this.GenreGrid.transform.GetChild(i).GetComponent<UILabel>().text == genre)
			{
				Mp3PlayerGenre component = this.GenreGrid.transform.GetChild(i).GetComponent<Mp3PlayerGenre>();
				for (int j = 0; j < component.songNames.Length; j++)
				{
					if (component.songNames[j] == songTitle)
					{
						return component.songNames[j];
					}
				}
			}
		}
		return null;
	}

	// Token: 0x04000C7A RID: 3194
	public GameObject GenreGO;

	// Token: 0x04000C7B RID: 3195
	public GameObject SongListGO;

	// Token: 0x04000C7C RID: 3196
	public GameObject VolumeMenu;

	// Token: 0x04000C7D RID: 3197
	public GameObject PlayingSong;

	// Token: 0x04000C7E RID: 3198
	public Mp3PlayerSong PlayingSongScript;

	// Token: 0x04000C7F RID: 3199
	public GameObject ScrollBar;

	// Token: 0x04000C80 RID: 3200
	public UILabel TitleLabel;

	// Token: 0x04000C81 RID: 3201
	public GameObject GenreGrid;

	// Token: 0x04000C82 RID: 3202
	public GameObject SongListGrid;

	// Token: 0x04000C83 RID: 3203
	public GameObject VolumeGrid;

	// Token: 0x04000C84 RID: 3204
	public AudioSource audio;

	// Token: 0x04000C85 RID: 3205
	public string currentSong;

	// Token: 0x04000C86 RID: 3206
	public string currentGenre;

	// Token: 0x04000C87 RID: 3207
	public float theVolume = 0.2f;

	// Token: 0x04000C88 RID: 3208
	public bool loopOne;

	// Token: 0x04000C89 RID: 3209
	public UILabel LoopType;

	// Token: 0x04000C8A RID: 3210
	public UIButton LoopOneButton;

	// Token: 0x04000C8B RID: 3211
	public UIButton LoopAllButton;

	// Token: 0x04000C8C RID: 3212
	private Mp3PlayerSongList SongListScript;

	// Token: 0x04000C8D RID: 3213
	private MenuStruct previousMenu;

	// Token: 0x04000C8E RID: 3214
	private const float startVolume = 0.2f;

	// Token: 0x020006A3 RID: 1699
	public struct State
	{
		// Token: 0x06003C13 RID: 15379 RVA: 0x00178775 File Offset: 0x00176975
		public State(string songTitle, string genre, float volume, float time, bool isPlaying, int menu, string menuTitle, bool loopOne)
		{
			this.songTitle = songTitle;
			this.genre = genre;
			this.volume = volume;
			this.time = time;
			this.isPlaying = isPlaying;
			this.menu = menu;
			this.menuTitle = menuTitle;
			this.loopOne = loopOne;
		}

		// Token: 0x040028C8 RID: 10440
		public string songTitle;

		// Token: 0x040028C9 RID: 10441
		public string genre;

		// Token: 0x040028CA RID: 10442
		public float volume;

		// Token: 0x040028CB RID: 10443
		public float time;

		// Token: 0x040028CC RID: 10444
		public bool isPlaying;

		// Token: 0x040028CD RID: 10445
		public int menu;

		// Token: 0x040028CE RID: 10446
		public string menuTitle;

		// Token: 0x040028CF RID: 10447
		public bool loopOne;
	}
}
