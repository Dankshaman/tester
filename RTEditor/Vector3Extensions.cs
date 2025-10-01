using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003FE RID: 1022
	public static class Vector3Extensions
	{
		// Token: 0x06002EDE RID: 11998 RVA: 0x0014154C File Offset: 0x0013F74C
		public static bool IsInsideTriangle(this Vector3 point, Vector3[] trianglePoints)
		{
			Vector3 lhs = trianglePoints[1] - trianglePoints[0];
			Vector3 rhs = trianglePoints[2] - trianglePoints[0];
			Vector3 rhs2 = Vector3.Cross(lhs, rhs);
			for (int i = 0; i < 3; i++)
			{
				Vector3 lhs2 = Vector3.Cross(trianglePoints[(i + 1) % 3] - trianglePoints[i], rhs2);
				Vector3 rhs3 = point - trianglePoints[i];
				if (Vector3.Dot(lhs2, rhs3) > 0f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x001415CE File Offset: 0x0013F7CE
		public static Vector3 GetInverse(this Vector3 vector)
		{
			return new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x001415F9 File Offset: 0x0013F7F9
		public static Vector3 GetVectorWithAbsComponents(this Vector3 vector)
		{
			return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00141621 File Offset: 0x0013F821
		public static float GetAbsDot(this Vector3 v1, Vector3 v2)
		{
			return Mathf.Abs(Vector3.Dot(v1, v2));
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x00141630 File Offset: 0x0013F830
		public static void GetMinMaxPoints(List<Vector3> points, out Vector3 min, out Vector3 max)
		{
			min = points[0];
			max = points[0];
			for (int i = 0; i < points.Count; i++)
			{
				min = Vector3.Min(min, points[i]);
				max = Vector3.Max(max, points[i]);
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00141698 File Offset: 0x0013F898
		public static Box GetPointCloudBox(List<Vector3> points)
		{
			Vector3 vector;
			Vector3 vector2;
			Vector3Extensions.GetMinMaxPoints(points, out vector, out vector2);
			return new Box((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x001416CC File Offset: 0x0013F8CC
		public static List<Vector3> GetTransformedPoints(List<Vector3> pointsToTransform, Matrix4x4 transformMatrix)
		{
			if (pointsToTransform.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = new List<Vector3>(pointsToTransform.Count);
			foreach (Vector3 point in pointsToTransform)
			{
				list.Add(transformMatrix.MultiplyPoint(point));
			}
			return list;
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x0014173C File Offset: 0x0013F93C
		public static List<Vector3> ApplyOffsetToPoints(List<Vector3> pointsToOffset, Vector3 offsetVector)
		{
			if (pointsToOffset.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = new List<Vector3>(pointsToOffset.Count);
			foreach (Vector3 a in pointsToOffset)
			{
				list.Add(a + offsetVector);
			}
			return list;
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x001417AC File Offset: 0x0013F9AC
		public static Vector3 GetAveragePoint(List<Vector3> pointsToAverage)
		{
			Vector3 a = Vector3.zero;
			foreach (Vector3 b in pointsToAverage)
			{
				a += b;
			}
			if (pointsToAverage.Count != 0)
			{
				return a * (1f / (float)pointsToAverage.Count);
			}
			return Vector3.zero;
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x00141824 File Offset: 0x0013FA24
		public static Vector3 GetPointClosestToPoint(List<Vector3> points, Vector3 point)
		{
			float num = float.MaxValue;
			Vector3 result = Vector3.zero;
			foreach (Vector3 vector in points)
			{
				float magnitude = (vector - point).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					result = vector;
				}
			}
			return result;
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x00141894 File Offset: 0x0013FA94
		public static Vector3 GetMostAlignedVector(List<Vector3> vectors, Vector3 refVec)
		{
			float num = float.MinValue;
			Vector3 result = Vector3.zero;
			foreach (Vector3 vector in vectors)
			{
				float num2 = Mathf.Abs(Vector3.Dot(vector, refVec));
				if (num2 > num)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00141900 File Offset: 0x0013FB00
		public static Vector3 ReadBinary(BinaryReader reader)
		{
			float x = reader.ReadSingle();
			float y = reader.ReadSingle();
			float z = reader.ReadSingle();
			return new Vector3(x, y, z);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x00141928 File Offset: 0x0013FB28
		public static void WriteBinary(this Vector3 vector, BinaryWriter writer)
		{
			writer.Write(vector.x);
			writer.Write(vector.y);
			writer.Write(vector.z);
		}
	}
}
