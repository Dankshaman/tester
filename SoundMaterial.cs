using System;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class SoundMaterial : MonoBehaviour
{
	// Token: 0x06001BF7 RID: 7159 RVA: 0x000C00A8 File Offset: 0x000BE2A8
	public static SoundMaterialType GetSurfaceVersion(SoundMaterialType materialType)
	{
		switch (materialType)
		{
		case SoundMaterialType.Wood:
		case SoundMaterialType.WoodSurface:
			return SoundMaterialType.WoodSurface;
		case SoundMaterialType.Metal:
		case SoundMaterialType.MetalSurface:
			return SoundMaterialType.MetalSurface;
		case SoundMaterialType.Plastic:
		case SoundMaterialType.PlasticSurface:
			return SoundMaterialType.PlasticSurface;
		case SoundMaterialType.Cardboard:
		case SoundMaterialType.CardboardSurface:
			return SoundMaterialType.CardboardSurface;
		case SoundMaterialType.Glass:
		case SoundMaterialType.GlassSurface:
			return SoundMaterialType.GlassSurface;
		case SoundMaterialType.Felt:
		case SoundMaterialType.FeltSurface:
			return SoundMaterialType.FeltSurface;
		default:
			return SoundMaterialType.None;
		}
	}

	// Token: 0x0400119C RID: 4508
	public SoundMaterialType soundMaterialType;
}
