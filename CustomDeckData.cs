using System;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class CustomDeckData
{
	// Token: 0x060008A6 RID: 2214 RVA: 0x0003DC3D File Offset: 0x0003BE3D
	public CustomDeckData()
	{
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0003DC60 File Offset: 0x0003BE60
	public CustomDeckData(string FaceURL, string BackURL, int NumWidth, int NumHeight, bool BackIsHidden, bool UniqueBack, CardType Type)
	{
		this.FaceURL = FaceURL;
		this.BackURL = BackURL;
		this.NumWidth = NumWidth;
		this.NumHeight = NumHeight;
		this.BackIsHidden = BackIsHidden;
		this.UniqueBack = UniqueBack;
		this.Type = Type;
	}

	// Token: 0x0400063A RID: 1594
	public string FaceURL;

	// Token: 0x0400063B RID: 1595
	public string BackURL;

	// Token: 0x0400063C RID: 1596
	[NonSerialized]
	public Texture FaceTexture;

	// Token: 0x0400063D RID: 1597
	[NonSerialized]
	public Texture BackTexture;

	// Token: 0x0400063E RID: 1598
	[NonSerialized]
	public float AspectRatio = 1f;

	// Token: 0x0400063F RID: 1599
	public int NumWidth = 10;

	// Token: 0x04000640 RID: 1600
	public int NumHeight = 7;

	// Token: 0x04000641 RID: 1601
	public bool BackIsHidden;

	// Token: 0x04000642 RID: 1602
	public bool UniqueBack;

	// Token: 0x04000643 RID: 1603
	public CardType Type;

	// Token: 0x04000644 RID: 1604
	[NonSerialized]
	public Material FaceMaterial;

	// Token: 0x04000645 RID: 1605
	[NonSerialized]
	public Material BackMaterial;
}
