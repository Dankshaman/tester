using System;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020000D8 RID: 216
public class CustomTextureContainer : CustomContainer
{
	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0004B844 File Offset: 0x00049A44
	// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0004B84C File Offset: 0x00049A4C
	public Texture texture { get; private set; }

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0004B855 File Offset: 0x00049A55
	// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x0004B85D File Offset: 0x00049A5D
	public float aspectRatio { get; private set; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0004B866 File Offset: 0x00049A66
	// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x0004B86E File Offset: 0x00049A6E
	public VideoPlayer videoPlayer { get; private set; }

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0004B877 File Offset: 0x00049A77
	// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0004B87F File Offset: 0x00049A7F
	public AssetBundle assetBundle { get; private set; }

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0004B888 File Offset: 0x00049A88
	// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0004B890 File Offset: 0x00049A90
	public Material material { get; private set; }

	// Token: 0x06000ABB RID: 2747 RVA: 0x0004B899 File Offset: 0x00049A99
	public CustomTextureContainer(string url, Texture texture, float aspectRatio)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Texture.NonCodeStrippedFromURL(url);
		this.texture = texture;
		this.aspectRatio = aspectRatio;
		this.videoPlayer = this.videoPlayer;
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0004B8D8 File Offset: 0x00049AD8
	public CustomTextureContainer(string url, Texture texture, float aspectRatio, VideoPlayer videoPlayer)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Texture.NonCodeStrippedFromURL(url);
		this.texture = texture;
		this.aspectRatio = aspectRatio;
		this.videoPlayer = videoPlayer;
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0004B914 File Offset: 0x00049B14
	public CustomTextureContainer(string url, Texture texture, float aspectRatio, AssetBundle assetBundle, Material material)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Texture.NonCodeStrippedFromURL(url);
		this.texture = texture;
		this.aspectRatio = aspectRatio;
		this.assetBundle = assetBundle;
		this.material = material;
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0004B964 File Offset: 0x00049B64
	public override void Cleanup(bool forceCleanup = false)
	{
		if (!forceCleanup && DLCManager.URLisDLC(base.url))
		{
			return;
		}
		if (this.videoPlayer)
		{
			UnityEngine.Object.Destroy(this.videoPlayer.gameObject);
		}
		RenderTexture renderTexture = this.texture as RenderTexture;
		if (renderTexture)
		{
			renderTexture.Release();
		}
		if (this.assetBundle != null)
		{
			this.assetBundle.Unload(true);
		}
		UnityEngine.Object.Destroy(this.texture);
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0004B9DE File Offset: 0x00049BDE
	public override bool IsError()
	{
		return this.texture == null;
	}
}
