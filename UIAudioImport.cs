using System;

// Token: 0x0200027E RID: 638
public class UIAudioImport : Singleton<UIAudioImport>
{
	// Token: 0x06002139 RID: 8505 RVA: 0x000EFFE8 File Offset: 0x000EE1E8
	private void OnEnable()
	{
		switch (this.ImportAudio)
		{
		case UIAudioImport.AudioImportMode.EditSong:
			this.ImportLabel.text = "IMPORT";
			this.TitleLabel.text = "IMPORT AUDIO";
			break;
		case UIAudioImport.AudioImportMode.EditSongPlaylist:
			this.ImportLabel.text = "EDIT";
			this.TitleLabel.text = "EDIT AUDIO";
			this.DeleteButton.gameObject.SetActive(true);
			break;
		case UIAudioImport.AudioImportMode.AddSongPlaylist:
			this.ImportLabel.text = "ADD";
			this.TitleLabel.text = "ADD AUDIO";
			break;
		}
		if (this.ImportAudio == UIAudioImport.AudioImportMode.EditSong || this.ImportAudio == UIAudioImport.AudioImportMode.EditSongPlaylist)
		{
			this.NameInput.value = this.Name;
			this.URLInput.value = this.URL;
		}
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000F00B8 File Offset: 0x000EE2B8
	private void OnDisable()
	{
		this.Name = "";
		this.URL = "";
		this.NameInput.value = "";
		this.URLInput.value = "";
		this.DeleteButton.gameObject.SetActive(false);
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000F010C File Offset: 0x000EE30C
	public void Import()
	{
		if (string.IsNullOrEmpty(this.NameInput.value))
		{
			this.NameInput.value = "Unknown Title";
		}
		if (string.IsNullOrEmpty(this.URLInput.value))
		{
			Chat.LogError("No url provided.", true);
			return;
		}
		switch (this.ImportAudio)
		{
		case UIAudioImport.AudioImportMode.EditSong:
			this.CustomMp3Player.EditAudio(this.NameInput.value, this.URLInput.value);
			this.CustomMp3Player.PlaylistEntry = -1;
			break;
		case UIAudioImport.AudioImportMode.EditSongPlaylist:
			this.CustomMp3Player.EditAudioPlayist(this.NameInput.value, this.URLInput.value, this.URL);
			break;
		case UIAudioImport.AudioImportMode.AddSongPlaylist:
			this.CustomMp3Player.AddAudio(this.NameInput.value, this.URLInput.value);
			break;
		}
		base.gameObject.SetActive(false);
		this.Name = "";
		this.URL = "";
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000F020F File Offset: 0x000EE40F
	public void Delete()
	{
		this.CustomMp3Player.EditAudioPlayist("", "", this.URL);
		base.gameObject.SetActive(false);
		this.Name = "";
		this.URL = "";
	}

	// Token: 0x0400148A RID: 5258
	public UIAudioImport.AudioImportMode ImportAudio;

	// Token: 0x0400148B RID: 5259
	public string Name = "";

	// Token: 0x0400148C RID: 5260
	public string URL = "";

	// Token: 0x0400148D RID: 5261
	public CustomMusicPlayer CustomMp3Player;

	// Token: 0x0400148E RID: 5262
	public UIInput NameInput;

	// Token: 0x0400148F RID: 5263
	public UIInput URLInput;

	// Token: 0x04001490 RID: 5264
	public UIButton ImportButton;

	// Token: 0x04001491 RID: 5265
	public UILabel ImportLabel;

	// Token: 0x04001492 RID: 5266
	public UILabel TitleLabel;

	// Token: 0x04001493 RID: 5267
	public UIButton DeleteButton;

	// Token: 0x04001494 RID: 5268
	private CustomAssetState OverrideCustomAsset;

	// Token: 0x04001495 RID: 5269
	private XmlUIScript targetXmlUI;

	// Token: 0x02000708 RID: 1800
	public enum AudioImportMode
	{
		// Token: 0x04002A7C RID: 10876
		EditSong,
		// Token: 0x04002A7D RID: 10877
		EditSongPlaylist,
		// Token: 0x04002A7E RID: 10878
		AddSongPlaylist
	}
}
