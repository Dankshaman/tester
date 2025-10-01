using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class UICustomMusicPlayer : Singleton<UICustomMusicPlayer>
{
	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06002282 RID: 8834 RVA: 0x000F7025 File Offset: 0x000F5225
	public bool MuteAudio
	{
		get
		{
			return this._muteAudio;
		}
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000F702D File Offset: 0x000F522D
	private void Update()
	{
		this.UpdateProgress();
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000F7038 File Offset: 0x000F5238
	private void OnEnable()
	{
		this.UiPlayer.SetActive(true);
		this.UiPlayList.SetActive(false);
		UISlider targetSlider = this.TargetSlider;
		targetSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(targetSlider.onDragFinished, new UIProgressBar.OnDragFinished(this.OnAudioScrubbingFinished));
		UISlider targetSlider2 = this.TargetSlider;
		targetSlider2.onDragStart = (UIProgressBar.OnDragStart)Delegate.Combine(targetSlider2.onDragStart, new UIProgressBar.OnDragStart(this.OnAudioScrubbingStart));
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000F70AB File Offset: 0x000F52AB
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnUIThemeChange += this.OnThemeChanged;
		this.OnThemeChanged();
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000F70CC File Offset: 0x000F52CC
	private void OnDestroy()
	{
		EventManager.OnUIThemeChange -= this.OnThemeChanged;
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.Grid.transform.GetChild(i).gameObject);
			uieventListener.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onDragEnd, new UIEventListener.VoidDelegate(this.OnDragEndAudio));
		}
		UISlider targetSlider = this.TargetSlider;
		targetSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(targetSlider.onDragFinished, new UIProgressBar.OnDragFinished(this.OnAudioScrubbingFinished));
		UISlider targetSlider2 = this.TargetSlider;
		targetSlider2.onDragStart = (UIProgressBar.OnDragStart)Delegate.Remove(targetSlider2.onDragStart, new UIProgressBar.OnDragStart(this.OnAudioScrubbingStart));
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000F7190 File Offset: 0x000F5390
	private void OnThemeChanged()
	{
		Colour colour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.Low];
		for (int i = 0; i < this.ThemableTextures.Length; i++)
		{
			this.ThemableTextures[i].color = colour;
		}
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000F71D8 File Offset: 0x000F53D8
	private void UpdateProgress()
	{
		if (this.CustomMusicPlayer.AudioSource.clip == null)
		{
			this.TargetSlider.value = 0f;
			this.LabelLeft.text = "--:--";
			this.LabelRight.text = "--:--";
			return;
		}
		if (this.UpdateProgressbar)
		{
			float length = this.CustomMusicPlayer.AudioSource.clip.length;
			float time = this.CustomMusicPlayer.AudioSource.time;
			float value = time / length;
			this.TargetSlider.value = value;
			this.LabelLeft.text = this.ConvertSecondsToLabel(time);
			this.LabelRight.text = this.ConvertSecondsToLabel(length - time);
			return;
		}
		float length2 = this.CustomMusicPlayer.AudioSource.clip.length;
		float num = length2 * this.TargetSlider.value;
		this.LabelLeft.text = this.ConvertSecondsToLabel(num);
		this.LabelRight.text = this.ConvertSecondsToLabel(length2 - num);
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x000F72E1 File Offset: 0x000F54E1
	public void OnVolumeChange()
	{
		if (this._muteAudio)
		{
			return;
		}
		this.CustomMusicPlayer.SetVolume(this.VolumeSlider.value);
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000F7302 File Offset: 0x000F5502
	public void UpdateVolumeSlider()
	{
		this.VolumeSlider.Set(this.CustomMusicPlayer.Volume, false);
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000F731B File Offset: 0x000F551B
	public void ToggleMuteAudio()
	{
		this._muteAudio = !this._muteAudio;
		this.CustomMusicPlayer.SetVolume(this._muteAudio ? 0f : this.VolumeSlider.value);
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x000F7354 File Offset: 0x000F5554
	public void EditSong()
	{
		this.UiAudioImport.ImportAudio = UIAudioImport.AudioImportMode.EditSong;
		this.UiAudioImport.Name = this.CustomMusicPlayer.CurrentAudioName;
		this.UiAudioImport.URL = this.CustomMusicPlayer.CurrentAudioUrl;
		this.UiImport.SetActive(true);
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x000F73A8 File Offset: 0x000F55A8
	public void EditSongPlayList(UILabel key)
	{
		this.UiAudioImport.ImportAudio = UIAudioImport.AudioImportMode.EditSongPlaylist;
		this.UiAudioImport.Name = this.CustomMusicPlayer.AudioLibrary[this.CustomMusicPlayer.GetAudioLibraryEntry(key.text)].Item2;
		this.UiAudioImport.URL = key.text;
		this.UiImport.SetActive(true);
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000F740F File Offset: 0x000F560F
	public void AddSong()
	{
		this.UiAudioImport.ImportAudio = UIAudioImport.AudioImportMode.AddSongPlaylist;
		this.UiImport.SetActive(true);
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x000F7429 File Offset: 0x000F5629
	public void PlayPause()
	{
		this.CustomMusicPlayer.PlayPause();
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x000BCD9D File Offset: 0x000BAF9D
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x000F7436 File Offset: 0x000F5636
	public void RepeatAudio()
	{
		this.CustomMusicPlayer.RepeatSong = !this.CustomMusicPlayer.RepeatSong;
		this.RepeatHighlight.SetActive(this.CustomMusicPlayer.RepeatSong);
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x000F7467 File Offset: 0x000F5667
	public void ShuffleAudio()
	{
		this.CustomMusicPlayer.ShuffleAudio = !this.CustomMusicPlayer.ShuffleAudio;
		this.ShuffleHighlight.SetActive(this.CustomMusicPlayer.ShuffleAudio);
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000F7498 File Offset: 0x000F5698
	public void SkipStart()
	{
		this.CustomMusicPlayer.SkipStart();
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000F74A5 File Offset: 0x000F56A5
	public void SkipEnd()
	{
		this.CustomMusicPlayer.SkipEnd();
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000F74B4 File Offset: 0x000F56B4
	private void SwitchContext()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Music, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Music Player (Music)");
			return;
		}
		this.ScrollViewPanel.transform.localPosition = new Vector3(-20f, -90f, 0f);
		if (this.UiPlayer.activeSelf)
		{
			this.InitializePlayList();
			this.UiPlayer.SetActive(false);
			this.UiPlayList.SetActive(true);
			return;
		}
		this.UiPlayer.SetActive(true);
		this.UiPlayList.SetActive(false);
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000F7548 File Offset: 0x000F5748
	private string ConvertSecondsToLabel(float time)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
		return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000F7580 File Offset: 0x000F5780
	public void InitializePlayList()
	{
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			GameObject gameObject = this.Grid.transform.GetChild(i).gameObject;
			if (UIEventListener.Get(gameObject))
			{
				UIEventListener uieventListener = UIEventListener.Get(gameObject);
				uieventListener.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onDragEnd, new UIEventListener.VoidDelegate(this.OnDragEndAudio));
			}
		}
		TTSUtilities.DestroyChildren(this.Grid.transform);
		int num = 0;
		int num2 = 0;
		foreach (ValueTuple<string, string> valueTuple in this.CustomMusicPlayer.AudioLibrary)
		{
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			GameObject gameObject2 = this.Grid.AddChild(this.ButtonPrefab);
			gameObject2.name = item2;
			gameObject2.transform.localPosition = new Vector3(-200f, (float)num2, 0f);
			UILabel[] componentsInChildren = gameObject2.GetComponentsInChildren<UILabel>();
			componentsInChildren[0].text = item2;
			componentsInChildren[1].text = item;
			foreach (Transform transform in gameObject2.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.name == "Highlight")
				{
					transform.gameObject.SetActive(num == this.CustomMusicPlayer.PlaylistEntry);
					num++;
				}
			}
			UIEventListener uieventListener2 = UIEventListener.Get(gameObject2);
			uieventListener2.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onDragEnd, new UIEventListener.VoidDelegate(this.OnDragEndAudio));
			num2 -= 40;
		}
		this._addButton = this.Grid.AddChild(this.AddButton);
		this._addButton.transform.position = new Vector3(200f, (float)num2, 0f);
		this.Grid.GetComponent<UIGrid>().Reposition();
		this.Grid.GetComponent<UIGrid>().repositionNow = true;
		this.ScrollViewPanel.gameObject.transform.localPosition = new Vector3(-20f, -90f, 0f);
		this.ScrollViewPanel.clipOffset = new Vector2(0f, 0f);
		Wait.Frames(delegate
		{
			this.ScrollViewPanel.gameObject.SetActive(false);
		}, 1);
		Wait.Frames(delegate
		{
			this.ScrollViewPanel.gameObject.SetActive(true);
		}, 2);
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x000F7808 File Offset: 0x000F5A08
	public void PlaySong(UILabel audioName, UILabel url, GameObject highlight)
	{
		this.CustomMusicPlayer.EditAudio(audioName.text, url.text);
		highlight.SetActive(true);
		this.CustomMusicPlayer.PlaylistEntry = this.CustomMusicPlayer.GetAudioLibraryEntry(url.text);
		this.SwitchContext();
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x000F7858 File Offset: 0x000F5A58
	public void HighlightEntry(int id)
	{
		List<Transform> childList = this.Grid.GetComponent<UIGrid>().GetChildList();
		int num = 0;
		foreach (Transform transform in childList)
		{
			GameObject gameObject = transform.gameObject;
			if (!(gameObject == this._addButton))
			{
				foreach (Transform transform2 in gameObject.GetComponentsInChildren<Transform>(true))
				{
					if (transform2.gameObject.name == "Highlight")
					{
						transform2.gameObject.SetActive(num == id);
						num++;
					}
				}
			}
		}
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000F7910 File Offset: 0x000F5B10
	public void OnAudioScrubbingStart()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Music, -1))
		{
			PermissionsOptions.BroadcastPermissionWarning("interact with the Music Player (Music)");
			return;
		}
		if (this.CustomMusicPlayer.AudioSource.clip == null)
		{
			return;
		}
		this.UpdateProgressbar = false;
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000F7950 File Offset: 0x000F5B50
	public void OnAudioScrubbingFinished()
	{
		float num = this.TargetSlider.value * this.CustomMusicPlayer.AudioSource.clip.length;
		num = (float)Mathf.FloorToInt(num);
		this.CustomMusicPlayer.SkipTo(num);
		this.UpdateProgressbar = true;
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000F799C File Offset: 0x000F5B9C
	private void OnDragEndAudio(GameObject audioEntry)
	{
		if (audioEntry.transform.localPosition.y < this._addButton.transform.localPosition.y)
		{
			Vector3 localPosition = audioEntry.transform.localPosition;
			audioEntry.transform.localPosition = this._addButton.transform.localPosition;
			this._addButton.transform.localPosition = localPosition;
			this.Grid.GetComponent<UIGrid>().Reposition();
			this.Grid.GetComponent<UIGrid>().repositionNow = true;
		}
		string url = "";
		if (this.CustomMusicPlayer.PlaylistEntry != -1)
		{
			url = this.CustomMusicPlayer.AudioLibrary[this.CustomMusicPlayer.PlaylistEntry].Item1;
		}
		this.CustomMusicPlayer.AudioLibrary = new List<ValueTuple<string, string>>();
		List<Transform> childList = this.Grid.GetComponent<UIGrid>().GetChildList();
		for (int i = 0; i < childList.Count; i++)
		{
			GameObject gameObject = childList[i].gameObject;
			if (gameObject != this._addButton)
			{
				UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
				this.CustomMusicPlayer.AudioLibrary.Add(new ValueTuple<string, string>(componentsInChildren[1].text, componentsInChildren[0].text));
			}
		}
		this.CustomMusicPlayer.PlaylistEntry = this.CustomMusicPlayer.GetAudioLibraryEntry(url);
		this.CustomMusicPlayer.SyncAudioLibrary();
	}

	// Token: 0x040015D0 RID: 5584
	public CustomMusicPlayer CustomMusicPlayer;

	// Token: 0x040015D1 RID: 5585
	public UIAudioImport UiAudioImport;

	// Token: 0x040015D2 RID: 5586
	public GameObject UiImport;

	// Token: 0x040015D3 RID: 5587
	public GameObject UiPlayer;

	// Token: 0x040015D4 RID: 5588
	public GameObject UiPlayList;

	// Token: 0x040015D5 RID: 5589
	public UIPanel ScrollViewPanel;

	// Token: 0x040015D6 RID: 5590
	public UITexture[] ThemableTextures;

	// Token: 0x040015D7 RID: 5591
	public GameObject Play;

	// Token: 0x040015D8 RID: 5592
	public GameObject Pause;

	// Token: 0x040015D9 RID: 5593
	public UILabel Title;

	// Token: 0x040015DA RID: 5594
	public GameObject RepeatHighlight;

	// Token: 0x040015DB RID: 5595
	public GameObject ShuffleHighlight;

	// Token: 0x040015DC RID: 5596
	public UISlider VolumeSlider;

	// Token: 0x040015DD RID: 5597
	public UISlider TargetSlider;

	// Token: 0x040015DE RID: 5598
	public UILabel LabelLeft;

	// Token: 0x040015DF RID: 5599
	public UILabel LabelRight;

	// Token: 0x040015E0 RID: 5600
	public bool UpdateProgressbar = true;

	// Token: 0x040015E1 RID: 5601
	private bool _muteAudio;

	// Token: 0x040015E2 RID: 5602
	public GameObject Grid;

	// Token: 0x040015E3 RID: 5603
	public GameObject ButtonPrefab;

	// Token: 0x040015E4 RID: 5604
	public GameObject AddButton;

	// Token: 0x040015E5 RID: 5605
	private GameObject _addButton;
}
