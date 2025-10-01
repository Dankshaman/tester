using System;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class Mp3PlayerSongList : MonoBehaviour
{
	// Token: 0x06001636 RID: 5686 RVA: 0x0009A808 File Offset: 0x00098A08
	public void PopulateSongList(string[] songs, string genre)
	{
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			this.Grid.transform.GetChild(i).gameObject.SetActive(false);
		}
		this.songs = songs;
		if (songs.Length > 20)
		{
			Debug.LogError("Trying to create more than 20 songs, this will blow up since we hardcode max of 20 song buttons.");
		}
		for (int j = 0; j < songs.Length; j++)
		{
			string text = songs[j];
			GameObject gameObject = this.Grid.transform.GetChild(j).gameObject;
			gameObject.GetComponent<UILabel>().text = text.Replace("MP3_", " ");
			gameObject.GetComponent<UILabel>().text = gameObject.GetComponent<UILabel>().text.Replace("_", " ");
			gameObject.GetComponent<Mp3PlayerSong>().songName = text;
			gameObject.GetComponent<Mp3PlayerSong>().genre = genre;
			gameObject.SetActive(true);
		}
		this.GridScript.repositionNow = true;
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x0009A8FC File Offset: 0x00098AFC
	public int GetSongIndex(string title)
	{
		if (this.songs.Length < 0)
		{
			return -1;
		}
		for (int i = 0; i < this.songs.Length; i++)
		{
			if (this.songs[i] == title)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x0009A93C File Offset: 0x00098B3C
	public void PlayPreviousSong()
	{
		int num = this.GetSongIndex(this.audio.clip.name);
		num--;
		if (num < 0)
		{
			num = this.songs.Length - 1;
		}
		else if (num > this.songs.Length - 1)
		{
			num = 0;
		}
		Mp3PlayerSong component = this.PlayingSong.GetComponent<Mp3PlayerSong>();
		component.songName = this.songs[num];
		component.Play();
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x0009A9A4 File Offset: 0x00098BA4
	public void PlayNextSong()
	{
		int num = this.GetSongIndex(this.audio.clip.name);
		num++;
		if (num < 0)
		{
			num = this.songs.Length - 1;
		}
		else if (num > this.songs.Length - 1)
		{
			num = 0;
		}
		Mp3PlayerSong component = this.PlayingSong.GetComponent<Mp3PlayerSong>();
		component.songName = this.songs[num];
		component.Play();
	}

	// Token: 0x04000C9B RID: 3227
	public GameObject Grid;

	// Token: 0x04000C9C RID: 3228
	public GameObject PlayingSong;

	// Token: 0x04000C9D RID: 3229
	public UIGrid GridScript;

	// Token: 0x04000C9E RID: 3230
	public string[] songs;

	// Token: 0x04000C9F RID: 3231
	public AudioSource audio;
}
