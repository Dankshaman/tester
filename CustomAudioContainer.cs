using System;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class CustomAudioContainer : CustomContainer
{
	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0004BA55 File Offset: 0x00049C55
	// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x0004BA5D File Offset: 0x00049C5D
	public AudioClip AudioClip { get; private set; }

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0004BA66 File Offset: 0x00049C66
	public CustomAudioContainer(string url, AudioClip audioClip)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Audio.NonCodeStrippedFromURL(url);
		this.AudioClip = audioClip;
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x0004BA92 File Offset: 0x00049C92
	public override void Cleanup(bool forceCleanup = false)
	{
		if (!forceCleanup && DLCManager.URLisDLC(base.url))
		{
			return;
		}
		UnityEngine.Object.Destroy(this.AudioClip);
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0004BAB0 File Offset: 0x00049CB0
	public override bool IsError()
	{
		return this.AudioClip == null;
	}
}
