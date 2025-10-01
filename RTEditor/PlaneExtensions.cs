using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F7 RID: 1015
	public static class PlaneExtensions
	{
		// Token: 0x06002EB3 RID: 11955 RVA: 0x00140174 File Offset: 0x0013E374
		public static Plane Transform(this Plane plane, Matrix4x4 transformMatrix, Vector3 pointOnPlane)
		{
			Vector3 inPoint = transformMatrix.MultiplyPoint(pointOnPlane);
			Vector3 inNormal = transformMatrix.MultiplyVector(plane.normal);
			inNormal.Normalize();
			return new Plane(inNormal, inPoint);
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x001401A7 File Offset: 0x0013E3A7
		public static Vector3 ProjectPoint(this Plane plane, Vector3 point)
		{
			return point - plane.normal * plane.GetDistanceToPoint(point);
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x001401C4 File Offset: 0x0013E3C4
		public static List<Vector3> ProjectAllPoints(this Plane plane, List<Vector3> pointsToProject)
		{
			if (pointsToProject.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = new List<Vector3>(pointsToProject.Count);
			foreach (Vector3 point in pointsToProject)
			{
				list.Add(plane.ProjectPoint(point));
			}
			return list;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00140234 File Offset: 0x0013E434
		public static bool AreAllPointsInFrontOrOnPlane(this Plane plane, List<Vector3> points)
		{
			foreach (Vector3 point in points)
			{
				if (plane.IsPointBehind(point))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x0014028C File Offset: 0x0013E48C
		public static bool AreAllPointsInFrontOrBehindPlane(this Plane plane, List<Vector3> points)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (Vector3 point in points)
			{
				if (plane.IsPointOnPlane(point, 0.0001f))
				{
					return false;
				}
				if (plane.IsPointInFront(point))
				{
					if (flag2)
					{
						return false;
					}
					flag = true;
				}
				else if (plane.IsPointBehind(point))
				{
					if (flag)
					{
						return false;
					}
					flag2 = true;
				}
			}
			return true;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x00140318 File Offset: 0x0013E518
		public static List<Vector3> GetAllPointsInFront(this Plane plane, List<Vector3> points)
		{
			if (points.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = new List<Vector3>(points.Count);
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointInFront(vector))
				{
					list.Add(vector);
				}
			}
			return list;
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x0014038C File Offset: 0x0013E58C
		public static List<Vector3> GetAllPointsBehind(this Plane plane, List<Vector3> points)
		{
			if (points.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = new List<Vector3>(points.Count);
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointBehind(vector))
				{
					list.Add(vector);
				}
			}
			return list;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x00140400 File Offset: 0x0013E600
		public static Vector3 GetClosestPointToPlane(this Plane plane, List<Vector3> points)
		{
			float num = float.MaxValue;
			Vector3 result = Vector3.zero;
			foreach (Vector3 vector in points)
			{
				float num2 = Mathf.Abs(plane.GetDistanceToPoint(vector));
				if (num2 < num)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x00140470 File Offset: 0x0013E670
		public static bool GetClosestPointInFront(this Plane plane, List<Vector3> points, out Vector3 closestPointInFront)
		{
			float num = float.MaxValue;
			closestPointInFront = Vector3.zero;
			bool result = false;
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointInFront(vector))
				{
					result = true;
					float num2 = Mathf.Abs(plane.GetDistanceToPoint(vector));
					if (num2 < num)
					{
						num = num2;
						closestPointInFront = vector;
					}
				}
			}
			return result;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x001404F4 File Offset: 0x0013E6F4
		public static bool GetClosestPointBehind(this Plane plane, List<Vector3> points, out Vector3 closestPointBehind)
		{
			float num = float.MaxValue;
			closestPointBehind = Vector3.zero;
			bool result = false;
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointBehind(vector))
				{
					result = true;
					float num2 = Mathf.Abs(plane.GetDistanceToPoint(vector));
					if (num2 < num)
					{
						num = num2;
						closestPointBehind = vector;
					}
				}
			}
			return result;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x00140578 File Offset: 0x0013E778
		public static Vector3 GetFurthestPointFromPlane(this Plane plane, List<Vector3> points)
		{
			float num = float.MinValue;
			Vector3 result = Vector3.zero;
			foreach (Vector3 vector in points)
			{
				float num2 = Mathf.Abs(plane.GetDistanceToPoint(vector));
				if (num2 > num)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x001405E8 File Offset: 0x0013E7E8
		public static bool GetFurthestPointInFront(this Plane plane, List<Vector3> points, out Vector3 furthestPointInFront)
		{
			float num = float.MinValue;
			furthestPointInFront = Vector3.zero;
			bool result = false;
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointInFront(vector))
				{
					result = true;
					float num2 = Mathf.Abs(plane.GetDistanceToPoint(vector));
					if (num2 > num)
					{
						num = num2;
						furthestPointInFront = vector;
					}
				}
			}
			return result;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x0014066C File Offset: 0x0013E86C
		public static int GetIndexOfFurthestPointInFront(this Plane plane, List<Vector3> points)
		{
			float num = float.MinValue;
			int result = -1;
			for (int i = 0; i < points.Count; i++)
			{
				Vector3 point = points[i];
				if (plane.IsPointInFront(point))
				{
					float num2 = Mathf.Abs(plane.GetDistanceToPoint(point));
					if (num2 > num)
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x001406C0 File Offset: 0x0013E8C0
		public static int GetIndexOfFurthestPointBehind(this Plane plane, List<Vector3> points)
		{
			float num = float.MinValue;
			int result = -1;
			for (int i = 0; i < points.Count; i++)
			{
				Vector3 point = points[i];
				if (plane.IsPointBehind(point))
				{
					float num2 = Mathf.Abs(plane.GetDistanceToPoint(point));
					if (num2 > num)
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x00140714 File Offset: 0x0013E914
		public static bool GetFurthestPointBehind(this Plane plane, List<Vector3> points, out Vector3 furthestPointBehind)
		{
			float num = float.MinValue;
			furthestPointBehind = Vector3.zero;
			bool result = false;
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointBehind(vector))
				{
					result = true;
					float num2 = Mathf.Abs(plane.GetDistanceToPoint(vector));
					if (num2 > num)
					{
						num = num2;
						furthestPointBehind = vector;
					}
				}
			}
			return result;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x00140798 File Offset: 0x0013E998
		public static bool GetFirstPointOnPlane(this Plane plane, List<Vector3> points, out Vector3 firstPointInPlane)
		{
			firstPointInPlane = Vector3.zero;
			bool result = false;
			foreach (Vector3 vector in points)
			{
				if (plane.IsPointOnPlane(vector, 0.0001f))
				{
					result = true;
					firstPointInPlane = vector;
					break;
				}
			}
			return result;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x00140808 File Offset: 0x0013EA08
		public static bool IsAnyPointOnPlane(this Plane plane, List<Vector3> points)
		{
			bool result = false;
			foreach (Vector3 point in points)
			{
				if (plane.IsPointOnPlane(point, 0.0001f))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00140864 File Offset: 0x0013EA64
		public static bool IsPointBehind(this Plane plane, Vector3 point)
		{
			return !plane.IsPointOnPlane(point, 0.0001f) && plane.GetDistanceToPoint(point) < 0f;
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00140885 File Offset: 0x0013EA85
		public static bool IsPointInFront(this Plane plane, Vector3 point)
		{
			return !plane.IsPointOnPlane(point, 0.0001f) && plane.GetDistanceToPoint(point) > 0f;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x001408A6 File Offset: 0x0013EAA6
		public static bool IsPointOnPlane(this Plane plane, Vector3 point, float epsilon = 0.0001f)
		{
			return Mathf.Abs(plane.GetDistanceToPoint(point)) < epsilon;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x001408B8 File Offset: 0x0013EAB8
		public static Plane AdjustSoBoxSitsInFront(this Plane plane, OrientedBox orientedBox)
		{
			List<Vector3> cornerPoints = orientedBox.GetCornerPoints();
			Vector3 inPoint;
			if (plane.GetFurthestPointBehind(cornerPoints, out inPoint))
			{
				return new Plane(plane.normal, inPoint);
			}
			Vector3 inPoint2;
			if (plane.GetFirstPointOnPlane(cornerPoints, out inPoint2))
			{
				return new Plane(plane.normal, inPoint2);
			}
			Vector3 inPoint3;
			if (plane.GetClosestPointInFront(cornerPoints, out inPoint3))
			{
				return new Plane(plane.normal, inPoint3);
			}
			return plane;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x00140918 File Offset: 0x0013EB18
		public static Plane AdjustSoBoxSitsBehind(this Plane plane, OrientedBox orientedBox)
		{
			List<Vector3> cornerPoints = orientedBox.GetCornerPoints();
			Vector3 inPoint;
			if (plane.GetFurthestPointInFront(cornerPoints, out inPoint))
			{
				return new Plane(plane.normal, inPoint);
			}
			Vector3 inPoint2;
			if (plane.GetFirstPointOnPlane(cornerPoints, out inPoint2))
			{
				return new Plane(plane.normal, inPoint2);
			}
			Vector3 inPoint3;
			if (plane.GetClosestPointBehind(cornerPoints, out inPoint3))
			{
				return new Plane(plane.normal, inPoint3);
			}
			return plane;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x00140978 File Offset: 0x0013EB78
		public static Plane GetPlaneWhichFacesNormal(List<Plane> planes, Vector3 normal, out int planeIndex)
		{
			if (planes.Count == 0)
			{
				planeIndex = -1;
				return default(Plane);
			}
			normal.Normalize();
			float num = 1f;
			planeIndex = -1;
			for (int i = 0; i < planes.Count; i++)
			{
				float num2 = Vector3.Dot(planes[i].normal, normal);
				if (num2 < 0f && num2 < num)
				{
					num = num2;
					planeIndex = i;
					if (Mathf.Abs(num2 + 1f) < 0.0001f)
					{
						break;
					}
				}
			}
			return planes[planeIndex];
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x00140A04 File Offset: 0x0013EC04
		public static Plane GetPlaneMostAlignedWithNormal(List<Plane> planes, Vector3 normal, out int planeIndex)
		{
			if (planes.Count == 0)
			{
				planeIndex = -1;
				return default(Plane);
			}
			normal.Normalize();
			float num = -1f;
			planeIndex = -1;
			for (int i = 0; i < planes.Count; i++)
			{
				float num2 = Vector3.Dot(planes[i].normal, normal);
				if (num2 > 0f && num2 > num)
				{
					num = num2;
					planeIndex = i;
					if (Mathf.Abs(num2 - 1f) < 0.0001f)
					{
						break;
					}
				}
			}
			return planes[planeIndex];
		}
	}
}
