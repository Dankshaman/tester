using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
public static class RendererExtensions
{
	// Token: 0x06001A01 RID: 6657 RVA: 0x000B6667 File Offset: 0x000B4867
	public static bool isVisibleFrom(this Renderer renderer, Camera camera)
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
	}
}
