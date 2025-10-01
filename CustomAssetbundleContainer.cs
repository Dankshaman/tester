using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class CustomAssetbundleContainer : CustomContainer
{
	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0004BC74 File Offset: 0x00049E74
	// (set) Token: 0x06000ADB RID: 2779 RVA: 0x0004BC7C File Offset: 0x00049E7C
	public AssetBundle assetBundle { get; private set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0004BC85 File Offset: 0x00049E85
	// (set) Token: 0x06000ADD RID: 2781 RVA: 0x0004BC8D File Offset: 0x00049E8D
	public GameObject[] gameObjects { get; private set; }

	// Token: 0x06000ADE RID: 2782 RVA: 0x0004BC96 File Offset: 0x00049E96
	public CustomAssetbundleContainer(string url, AssetBundle assetBundle, GameObject[] gameObjects)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Assetbundle.NonCodeStrippedFromURL(url);
		this.assetBundle = assetBundle;
		this.gameObjects = gameObjects;
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0004BCCC File Offset: 0x00049ECC
	public override void Cleanup(bool forceCleanup = false)
	{
		if (!forceCleanup && DLCManager.URLisDLC(base.url))
		{
			return;
		}
		if (this.assetBundle != null)
		{
			this.assetBundle.Unload(true);
		}
		if (this.gameObjects != null)
		{
			for (int i = 0; i < this.gameObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(this.gameObjects[i]);
			}
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0004BD2C File Offset: 0x00049F2C
	public override bool IsError()
	{
		return this.assetBundle == null;
	}
}
