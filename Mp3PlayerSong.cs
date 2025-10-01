using System;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class Mp3PlayerSong : MonoBehaviour
{
	// Token: 0x06001632 RID: 5682 RVA: 0x0009A5C0 File Offset: 0x000987C0
	private void Update()
	{
		if (this.audio && this.audio.clip && this.PlayingSong.activeSelf)
		{
			this.ProgressSlider.value = this.audio.time / this.audio.clip.length;
			this.clipTime = new TimeSpan(0, 0, 0, (int)this.audio.time);
			this.currentTimeLabel.text = this.clipTime.ToString();
			this.clipTime = new TimeSpan(0, 0, 0, (int)this.audio.clip.length);
			this.totalTimeLabel.text = this.clipTime.ToString();
		}
		if (this.audio && this.audio.clip && this.isPlaying && !this.audio.isPlaying && !this.isPaused)
		{
			if (this.mp3PlayerScript.loopOne)
			{
				this.audio.Play();
				return;
			}
			this.mp3PlayerScript.PlayNextSong();
		}
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x0009A6FA File Offset: 0x000988FA
	public void SelectSong()
	{
		if (!PlayerScript.Pointer)
		{
			return;
		}
		this.mp3PlayerScript.SelectSong(this.songName, this.genre);
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x0009A720 File Offset: 0x00098920
	public void Play()
	{
		if (this.audio)
		{
			this.audio.time = 0f;
			if (this.audio.clip != null)
			{
				Resources.UnloadAsset(this.audio.clip);
			}
			this.audio.clip = (AudioClip)Resources.Load(this.songName);
			this.audio.Play();
			this.isPaused = false;
			this.isPlaying = true;
		}
		if (this.PlayingSong.activeSelf)
		{
			this.TitleLabel.text = this.songName.Replace("MP3_", "");
			this.TitleLabel.text = this.TitleLabel.text.Replace("_", " ");
		}
	}

	// Token: 0x04000C8F RID: 3215
	public string songName;

	// Token: 0x04000C90 RID: 3216
	public string genre = "";

	// Token: 0x04000C91 RID: 3217
	public AudioSource audio;

	// Token: 0x04000C92 RID: 3218
	public GameObject PlayingSong;

	// Token: 0x04000C93 RID: 3219
	public UILabel TitleLabel;

	// Token: 0x04000C94 RID: 3220
	public Mp3PlayerScript mp3PlayerScript;

	// Token: 0x04000C95 RID: 3221
	public UISlider ProgressSlider;

	// Token: 0x04000C96 RID: 3222
	public UILabel currentTimeLabel;

	// Token: 0x04000C97 RID: 3223
	public UILabel totalTimeLabel;

	// Token: 0x04000C98 RID: 3224
	public bool isPaused;

	// Token: 0x04000C99 RID: 3225
	public bool isPlaying;

	// Token: 0x04000C9A RID: 3226
	private TimeSpan clipTime;
}
