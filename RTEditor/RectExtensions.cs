using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003FA RID: 1018
	public static class RectExtensions
	{
		// Token: 0x06002ED3 RID: 11987 RVA: 0x001410A8 File Offset: 0x0013F2A8
		public static Vector2 GetClosestPointToPoint(this Rect rectangle, Vector2 point)
		{
			Vector2[] cornerAndCenterPoints = rectangle.GetCornerAndCenterPoints();
			int num = 0;
			float num2 = float.MaxValue;
			for (int i = 0; i < cornerAndCenterPoints.Length; i++)
			{
				float magnitude = (cornerAndCenterPoints[i] - point).magnitude;
				if (magnitude < num2)
				{
					num2 = magnitude;
					num = i;
				}
			}
			return cornerAndCenterPoints[num];
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x001410FC File Offset: 0x0013F2FC
		public static Vector2[] GetCornerAndCenterPoints(this Rect rectangle)
		{
			return new Vector2[]
			{
				new Vector2(rectangle.xMin, rectangle.yMin),
				new Vector2(rectangle.xMax, rectangle.yMin),
				new Vector2(rectangle.xMax, rectangle.yMax),
				new Vector2(rectangle.xMin, rectangle.yMax),
				rectangle.center
			};
		}
	}
}
