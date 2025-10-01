using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class CustomUIAssetbundleContainer : CustomContainer
{
	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0004BDAC File Offset: 0x00049FAC
	// (set) Token: 0x06000AEB RID: 2795 RVA: 0x0004BDB4 File Offset: 0x00049FB4
	public AssetBundle assetBundle { get; private set; }

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0004BDBD File Offset: 0x00049FBD
	// (set) Token: 0x06000AED RID: 2797 RVA: 0x0004BDC5 File Offset: 0x00049FC5
	public UnityEngine.Object[] resources { get; private set; }

	// Token: 0x06000AEE RID: 2798 RVA: 0x0004BDCE File Offset: 0x00049FCE
	public CustomUIAssetbundleContainer(string url, AssetBundle assetBundle, UnityEngine.Object[] resources)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Assetbundle.NonCodeStrippedFromURL(url);
		this.assetBundle = assetBundle;
		this.resources = resources;
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0004BE04 File Offset: 0x0004A004
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
		if (this.resources != null)
		{
			UnityEngine.Object[] resources = this.resources;
			for (int i = 0; i < resources.Length; i++)
			{
				UnityEngine.Object.Destroy(resources[i]);
			}
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0004BE61 File Offset: 0x0004A061
	public override bool IsError()
	{
		return this.assetBundle == null;
	}
}
