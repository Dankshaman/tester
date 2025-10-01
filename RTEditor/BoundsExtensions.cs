using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F2 RID: 1010
	public static class BoundsExtensions
	{
		// Token: 0x06002E5F RID: 11871 RVA: 0x0013E784 File Offset: 0x0013C984
		public static Bounds GetInvalidBoundsInstance()
		{
			return new Bounds(Vector3.zero, BoundsExtensions.GetInvalidBoundsSize());
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0013E795 File Offset: 0x0013C995
		public static bool IsValid(this Bounds bounds)
		{
			return bounds.size != BoundsExtensions.GetInvalidBoundsSize();
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x0013E7A8 File Offset: 0x0013C9A8
		public static Bounds Transform(this Bounds bounds, Matrix4x4 transformMatrix)
		{
			Vector3 a = transformMatrix.GetColumn(0);
			Vector3 a2 = transformMatrix.GetColumn(1);
			Vector3 a3 = transformMatrix.GetColumn(2);
			Vector3 vector = a * bounds.extents.x;
			Vector3 vector2 = a2 * bounds.extents.y;
			Vector3 vector3 = a3 * bounds.extents.z;
			float x = (Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x)) * 2f;
			float y = (Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y)) * 2f;
			float z = (Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z)) * 2f;
			return new Bounds
			{
				center = transformMatrix.MultiplyPoint(bounds.center),
				size = new Vector3(x, y, z)
			};
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x0013E8C8 File Offset: 0x0013CAC8
		public static Vector2[] GetScreenSpaceCornerPoints(this Bounds bounds, Camera camera)
		{
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;
			return new Vector2[]
			{
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z))
			};
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x0013EADC File Offset: 0x0013CCDC
		public static Rect GetScreenRectangle(this Bounds bounds, Camera camera)
		{
			Vector2[] screenSpaceCornerPoints = bounds.GetScreenSpaceCornerPoints(camera);
			Vector3 vector = screenSpaceCornerPoints[0];
			Vector3 vector2 = screenSpaceCornerPoints[0];
			for (int i = 1; i < screenSpaceCornerPoints.Length; i++)
			{
				vector = Vector3.Min(vector, screenSpaceCornerPoints[i]);
				vector2 = Vector3.Max(vector2, screenSpaceCornerPoints[i]);
			}
			return new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x0013EB6C File Offset: 0x0013CD6C
		public static Bounds FromPointCloud(List<Vector3> pointCloud)
		{
			if (pointCloud.Count == 0)
			{
				return BoundsExtensions.GetInvalidBoundsInstance();
			}
			Vector3 vector = pointCloud[0];
			Vector3 vector2 = pointCloud[0];
			for (int i = 1; i < pointCloud.Count; i++)
			{
				Vector3 lhs = pointCloud[i];
				vector = Vector3.Min(lhs, vector);
				vector2 = Vector3.Max(lhs, vector2);
			}
			Vector3 center = (vector + vector2) * 0.5f;
			Vector3 size = vector2 - vector;
			return new Bounds(center, size);
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x0013EBDD File Offset: 0x0013CDDD
		private static Vector3 GetInvalidBoundsSize()
		{
			return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		}
	}
}
