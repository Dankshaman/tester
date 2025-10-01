using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public struct TokenSettings
{
	// Token: 0x06000AF1 RID: 2801 RVA: 0x0004BE6F File Offset: 0x0004A06F
	public TokenSettings(Texture2D Image, float AspectRatio, float Thickness, float MergeDistancePixels)
	{
		this.Image = Image;
		this.AspectRatio = AspectRatio;
		this.Thickness = Thickness;
		this.MergeDistancePixels = MergeDistancePixels;
	}

	// Token: 0x040007AA RID: 1962
	public Texture2D Image;

	// Token: 0x040007AB RID: 1963
	public float AspectRatio;

	// Token: 0x040007AC RID: 1964
	public float Thickness;

	// Token: 0x040007AD RID: 1965
	public float MergeDistancePixels;
}
