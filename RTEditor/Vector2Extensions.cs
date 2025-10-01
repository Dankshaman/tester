using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003FD RID: 1021
	public static class Vector2Extensions
	{
		// Token: 0x06002EDD RID: 11997 RVA: 0x001414D0 File Offset: 0x0013F6D0
		public static Bounds GetPointCloudAABB(List<Vector2> pointCloud)
		{
			if (pointCloud.Count == 0)
			{
				return BoundsExtensions.GetInvalidBoundsInstance();
			}
			Vector2 vector = pointCloud[0];
			Vector2 vector2 = pointCloud[0];
			for (int i = 1; i < pointCloud.Count; i++)
			{
				Vector2 lhs = pointCloud[i];
				vector = Vector2.Min(lhs, vector);
				vector2 = Vector2.Max(lhs, vector2);
			}
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}
	}
}
