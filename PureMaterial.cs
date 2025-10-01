using System;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class PureMaterial : MonoBehaviour
{
	// Token: 0x060019D6 RID: 6614 RVA: 0x000B4D72 File Offset: 0x000B2F72
	private void Start()
	{
		base.GetComponent<Renderer>().sharedMaterial = this.pureMaterial();
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x000B4D88 File Offset: 0x000B2F88
	private Material pureMaterial()
	{
		PureMaterialType pureMaterialType = this.pureMaterialType;
		if (pureMaterialType == PureMaterialType.Secondary)
		{
			return Singleton<SystemConsole>.Instance.PureModeSecondaryMaterial;
		}
		if (pureMaterialType != PureMaterialType.Splash)
		{
			return Singleton<SystemConsole>.Instance.PureModePrimaryMaterial;
		}
		return Singleton<SystemConsole>.Instance.PureModeSplashMaterial;
	}

	// Token: 0x04000FD6 RID: 4054
	public PureMaterialType pureMaterialType;
}
