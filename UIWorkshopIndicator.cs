using System;
using UnityEngine;

// Token: 0x0200035D RID: 861
public class UIWorkshopIndicator : MonoBehaviour
{
	// Token: 0x060028C1 RID: 10433 RVA: 0x0011FA66 File Offset: 0x0011DC66
	private void Awake()
	{
		this.LoadingWheel.SetActive(true);
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x0011FA74 File Offset: 0x0011DC74
	private void Cleanup()
	{
		if (this.hovering)
		{
			this.hovering = false;
			UIHoverText.text = "";
		}
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x0011FA90 File Offset: 0x0011DC90
	private void Update()
	{
		if (!Singleton<SteamManager>.Instance.bCheckingSubscribe && !Singleton<SteamManager>.Instance.bDownloading && !Singleton<SteamManager>.Instance.bDownloadingThumbnails)
		{
			this.Cleanup();
			base.gameObject.SetActive(false);
			return;
		}
		if (UICamera.HoveredUIObject == base.gameObject)
		{
			this.hovering = true;
			if (Singleton<SteamManager>.Instance.bDownloading)
			{
				UIHoverText.text = string.Format("Downloading Workshop Saves ({0})", Singleton<SteamManager>.Instance.PendingModDownloads.Count);
				return;
			}
			if (Singleton<SteamManager>.Instance.bDownloadingThumbnails)
			{
				UIHoverText.text = string.Format("Downloading Workshop Thumbnails ({0})", Singleton<SteamManager>.Instance.PendingModThumbnailDownloads.Count);
				return;
			}
			if (Singleton<SteamManager>.Instance.bCheckingSubscribe)
			{
				UIHoverText.text = string.Format("Checking Workshop ({0}/{1})", Singleton<SteamManager>.Instance.SubscribeCounter, Singleton<SteamManager>.Instance.SubscribeNumber);
				return;
			}
		}
		else
		{
			this.Cleanup();
		}
	}

	// Token: 0x04001ACF RID: 6863
	private bool hovering;

	// Token: 0x04001AD0 RID: 6864
	public GameObject LoadingWheel;
}
