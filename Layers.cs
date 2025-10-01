using System;
using UnityEngine;

// Token: 0x0200013D RID: 317
public static class Layers
{
	// Token: 0x06001076 RID: 4214 RVA: 0x0007101C File Offset: 0x0006F21C
	public static int Mask(params int[] layers)
	{
		int num = 0;
		foreach (int num2 in layers)
		{
			num |= 1 << num2;
		}
		return num;
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x00071049 File Offset: 0x0006F249
	public static bool IsUI3D(int layer)
	{
		return layer == 17;
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x00071050 File Offset: 0x0006F250
	public static bool IsUI3D(GameObject go)
	{
		return go.layer == 17;
	}

	// Token: 0x04000A60 RID: 2656
	public const int Default = 0;

	// Token: 0x04000A61 RID: 2657
	public const int TransparentFX = 1;

	// Token: 0x04000A62 RID: 2658
	public const int IgnoreRaycast = 2;

	// Token: 0x04000A63 RID: 2659
	public const int Water = 4;

	// Token: 0x04000A64 RID: 2660
	public const int UI = 5;

	// Token: 0x04000A65 RID: 2661
	public const int RenderTop = 8;

	// Token: 0x04000A66 RID: 2662
	public const int Held = 9;

	// Token: 0x04000A67 RID: 2663
	public const int Grabbable = 10;

	// Token: 0x04000A68 RID: 2664
	public const int UI3D = 17;

	// Token: 0x04000A69 RID: 2665
	public const int Bounds = 18;

	// Token: 0x04000A6A RID: 2666
	public const int Hand = 19;

	// Token: 0x04000A6B RID: 2667
	public const int NoCollision = 20;

	// Token: 0x04000A6C RID: 2668
	public const int Grid = 21;

	// Token: 0x04000A6D RID: 2669
	public const int AltZoom = 22;

	// Token: 0x04000A6E RID: 2670
	public const int Spectator = 29;

	// Token: 0x04000A6F RID: 2671
	public const int PostProcessing = 30;

	// Token: 0x04000A70 RID: 2672
	public const int FogOfWar = 31;
}
