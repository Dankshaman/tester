using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class CustomTokenContainer : CustomContainer
{
	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0004BD3A File Offset: 0x00049F3A
	// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x0004BD42 File Offset: 0x00049F42
	public Mesh mesh { get; private set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0004BD4B File Offset: 0x00049F4B
	// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x0004BD53 File Offset: 0x00049F53
	public BoxColliderState[] boxColliders { get; private set; }

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0004BD5C File Offset: 0x00049F5C
	// (set) Token: 0x06000AE6 RID: 2790 RVA: 0x0004BD64 File Offset: 0x00049F64
	public TokenSettings settings { get; private set; }

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0004BD6D File Offset: 0x00049F6D
	public CustomTokenContainer(TokenSettings settings, Mesh mesh, BoxColliderState[] boxColliders)
	{
		this.settings = settings;
		this.mesh = mesh;
		this.boxColliders = boxColliders;
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0004BD8A File Offset: 0x00049F8A
	public override void Cleanup(bool forceCleanup = false)
	{
		UnityEngine.Object.Destroy(this.mesh);
		this.boxColliders = null;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0004BD9E File Offset: 0x00049F9E
	public override bool IsError()
	{
		return this.mesh == null;
	}
}
