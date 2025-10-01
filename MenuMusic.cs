using System;
using System.Collections;
using NewNet;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class MenuMusic : MonoBehaviour
{
	// Token: 0x060015E5 RID: 5605 RVA: 0x00098D40 File Offset: 0x00096F40
	private void Start()
	{
		this.volume = this.audio.volume;
		this.audio.volume = this.volume * SoundScript.GLOBAL_MUSIC_MULTI;
		this.audio.Play();
		NetworkEvents.OnServerInitialized += this.ServerInitialized;
		NetworkEvents.OnConnectedToServer += this.ConnectedToServer;
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x00098DA2 File Offset: 0x00096FA2
	private void OnDestroy()
	{
		NetworkEvents.OnServerInitialized -= this.ServerInitialized;
		NetworkEvents.OnConnectedToServer -= this.ConnectedToServer;
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x00098DC6 File Offset: 0x00096FC6
	private void Update()
	{
		if (!this.stopUpdating)
		{
			this.audio.volume = this.volume * SoundScript.GLOBAL_MUSIC_MULTI;
		}
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x00098DE7 File Offset: 0x00096FE7
	private void ServerInitialized()
	{
		base.StartCoroutine(this.FadeToSelfDestruct());
		this.stopUpdating = true;
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x00098DE7 File Offset: 0x00096FE7
	private void ConnectedToServer()
	{
		base.StartCoroutine(this.FadeToSelfDestruct());
		this.stopUpdating = true;
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00098DFD File Offset: 0x00096FFD
	private IEnumerator FadeToSelfDestruct()
	{
		while (this.audio.volume >= 0.021f)
		{
			this.audio.volume -= 0.02f;
			yield return new WaitForSeconds(0.1f);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		UnityEngine.Object.Destroy(this);
		yield break;
	}

	// Token: 0x04000C49 RID: 3145
	public AudioSource audio;

	// Token: 0x04000C4A RID: 3146
	public float volume;

	// Token: 0x04000C4B RID: 3147
	private bool stopUpdating;
}
