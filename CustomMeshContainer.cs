using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class CustomMeshContainer : CustomContainer
{
	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0004B9EC File Offset: 0x00049BEC
	// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x0004B9F4 File Offset: 0x00049BF4
	public Mesh mesh { get; private set; }

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0004B9FD File Offset: 0x00049BFD
	public CustomMeshContainer(string url, Mesh mesh)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Mesh.NonCodeStrippedFromURL(url);
		this.mesh = mesh;
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0004BA29 File Offset: 0x00049C29
	public override void Cleanup(bool forceCleanup = false)
	{
		if (!forceCleanup && DLCManager.URLisDLC(base.url))
		{
			return;
		}
		UnityEngine.Object.Destroy(this.mesh);
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0004BA47 File Offset: 0x00049C47
	public override bool IsError()
	{
		return this.mesh == null;
	}
}
