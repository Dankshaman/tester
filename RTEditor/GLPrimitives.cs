using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000415 RID: 1045
	public static class GLPrimitives
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x0014B507 File Offset: 0x00149707
		public static Camera Camera
		{
			get
			{
				return MonoSingletonBase<EditorCamera>.Instance.Camera;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x0014B514 File Offset: 0x00149714
		public static Matrix4x4 CameraViewMatrix
		{
			get
			{
				Matrix4x4 identity = Matrix4x4.identity;
				identity[2, 2] = -1f;
				return identity * GLPrimitives.Camera.transform.localToWorldMatrix.inverse;
			}
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x0014B554 File Offset: 0x00149754
		public static void Draw3DLine(Vector3 firstPoint, Vector3 secondPoint, Color lineColor, Material lineMaterial)
		{
			lineMaterial.SetColor("_Color", lineColor);
			lineMaterial.SetPass(0);
			GL.Begin(1);
			GL.Color(lineColor);
			GL.Vertex3(firstPoint.x, firstPoint.y, firstPoint.z);
			GL.Vertex3(secondPoint.x, secondPoint.y, secondPoint.z);
			GL.End();
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x0014B5B4 File Offset: 0x001497B4
		public static void Draw3DLines(Vector3[] linePoints, Color[] lineColors, bool drawConnectedLines, Material lineMaterial, bool loop, Color loopLineColor)
		{
			lineMaterial.SetPass(0);
			int num = drawConnectedLines ? (linePoints.Length - 1) : (linePoints.Length / 2);
			GL.Begin(1);
			if (!drawConnectedLines)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = i * 2;
					Vector3 vector = linePoints[num2];
					Vector3 vector2 = linePoints[num2 + 1];
					lineMaterial.SetColor("_Color", lineColors[i]);
					GL.Color(lineColors[i]);
					GL.Vertex3(vector.x, vector.y, vector.z);
					GL.Vertex3(vector2.x, vector2.y, vector2.z);
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					Vector3 vector3 = linePoints[j];
					Vector3 vector4 = linePoints[j + 1];
					lineMaterial.SetColor("_Color", lineColors[j]);
					GL.Color(lineColors[j]);
					GL.Vertex3(vector3.x, vector3.y, vector3.z);
					GL.Vertex3(vector4.x, vector4.y, vector4.z);
				}
			}
			if (loop)
			{
				Vector3 vector5 = linePoints[0];
				Vector3 vector6 = linePoints[linePoints.Length - 1];
				lineMaterial.SetColor("_Color", loopLineColor);
				GL.Color(loopLineColor);
				GL.Vertex3(vector5.x, vector5.y, vector5.z);
				GL.Vertex3(vector6.x, vector6.y, vector6.z);
			}
			GL.End();
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x0014B738 File Offset: 0x00149938
		public static void Draw3DLine(Vector3 firstPoint, Vector3 secondPoint, Color firstPointColor, Color secondPointColor, Material lineMaterial)
		{
			lineMaterial.SetColor("_Color", firstPointColor);
			lineMaterial.SetPass(0);
			GL.Begin(1);
			GL.Color(firstPointColor);
			GL.Vertex3(firstPoint.x, firstPoint.y, firstPoint.z);
			GL.Color(secondPointColor);
			GL.Vertex3(secondPoint.x, secondPoint.y, secondPoint.z);
			GL.End();
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x0014B7A0 File Offset: 0x001499A0
		public static void Draw2DLine(Vector2 firstPoint, Vector2 secondPoint, Color lineColor, Material lineMaterial, Camera camera)
		{
			lineMaterial.SetColor("_Color", lineColor);
			lineMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			firstPoint = camera.ScreenToViewportPoint(firstPoint);
			secondPoint = camera.ScreenToViewportPoint(secondPoint);
			GL.Begin(1);
			GL.Color(lineColor);
			GL.Vertex(new Vector3(firstPoint.x, firstPoint.y, 0f));
			GL.Vertex(new Vector3(secondPoint.x, secondPoint.y, 0f));
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x0014B840 File Offset: 0x00149A40
		public static void Draw2DCircleBorderLines(Vector3[] borderLinePoints, Vector3 circleCenter, Color borderLineColor, float radiusScale, Material borderLineMaterial, Camera camera)
		{
			borderLineMaterial.SetColor("_Color", borderLineColor);
			borderLineMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			float d = (borderLinePoints[0] - circleCenter).magnitude * radiusScale;
			GL.Begin(1);
			GL.Color(borderLineColor);
			for (int i = 0; i < borderLinePoints.Length; i++)
			{
				Vector3 a = borderLinePoints[i] - circleCenter;
				Vector3 a2 = borderLinePoints[(i + 1) % borderLinePoints.Length] - circleCenter;
				a.Normalize();
				a2.Normalize();
				Vector3 vector = circleCenter + a * d;
				Vector3 vector2 = circleCenter + a2 * d;
				vector = camera.ScreenToViewportPoint(vector);
				vector2 = camera.ScreenToViewportPoint(vector2);
				GL.Vertex(new Vector3(vector.x, vector.y, 0f));
				GL.Vertex(new Vector3(vector2.x, vector2.y, 0f));
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x0014B950 File Offset: 0x00149B50
		public static void Draw2DRectangleBorderLines(Vector2[] borderLinePoints, Color borderLineColor, Material borderLineMaterial, Camera camera)
		{
			borderLineMaterial.SetColor("_Color", borderLineColor);
			borderLineMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(1);
			GL.Color(borderLineColor);
			for (int i = 0; i < borderLinePoints.Length; i++)
			{
				Vector3 vector = borderLinePoints[i];
				Vector3 vector2 = borderLinePoints[(i + 1) % borderLinePoints.Length];
				vector = camera.ScreenToViewportPoint(vector);
				vector2 = camera.ScreenToViewportPoint(vector2);
				GL.Vertex(new Vector3(vector.x, vector.y, 0f));
				GL.Vertex(new Vector3(vector2.x, vector2.y, 0f));
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x0014BA08 File Offset: 0x00149C08
		public static void Draw2DRectangleBorderLines(Rect rectangle, Color borderLineColor, Material borderLineMaterial, Camera camera)
		{
			GLPrimitives.Draw2DRectangleBorderLines(new Vector2[]
			{
				new Vector2(rectangle.xMin, rectangle.yMin),
				new Vector2(rectangle.xMax, rectangle.yMin),
				new Vector2(rectangle.xMax, rectangle.yMax),
				new Vector2(rectangle.xMin, rectangle.yMax)
			}, borderLineColor, borderLineMaterial, camera);
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x0014BA8C File Offset: 0x00149C8C
		public static void Draw2DFilledRectangle(Rect rectangle, Color rectangleColor, Material rectangleMaterial, Camera camera)
		{
			rectangleMaterial.SetColor("_Color", rectangleColor);
			rectangleMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(7);
			GL.Color(rectangleColor);
			Vector3 vector = camera.ScreenToViewportPoint(new Vector3(rectangle.xMin, rectangle.yMin, 0f));
			Vector3 vector2 = camera.ScreenToViewportPoint(new Vector3(rectangle.xMax, rectangle.yMin, 0f));
			Vector3 vector3 = camera.ScreenToViewportPoint(new Vector3(rectangle.xMax, rectangle.yMax, 0f));
			Vector3 vector4 = camera.ScreenToViewportPoint(new Vector3(rectangle.xMin, rectangle.yMax, 0f));
			GL.Vertex3(vector.x, vector.y, 0f);
			GL.Vertex3(vector2.x, vector2.y, 0f);
			GL.Vertex3(vector3.x, vector3.y, 0f);
			GL.Vertex3(vector4.x, vector4.y, 0f);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x0014BBA4 File Offset: 0x00149DA4
		public static void Draw3DFilledDisc(Vector3 discCenter, Vector3 firstPoint, Vector3 secondPoint, Vector3 discPlaneNormal, Color discColor, Material discMaterial)
		{
			Vector3 vector = firstPoint - discCenter;
			Vector3 rhs = secondPoint - discCenter;
			float magnitude = vector.magnitude;
			vector.Normalize();
			rhs.Normalize();
			float num;
			if (Vector3.Dot(Vector3.Cross(vector, rhs), discPlaneNormal) < 0f)
			{
				num = -1f;
			}
			else
			{
				num = 1f;
			}
			discPlaneNormal.Normalize();
			float num2 = MathHelper.SafeAcos(Vector3.Dot(vector, rhs)) * 57.29578f * num;
			Quaternion b = Quaternion.AngleAxis(num2, discPlaneNormal);
			int num3 = (int)(180f * Mathf.Abs(num2) / 180f);
			if (num3 < 2)
			{
				return;
			}
			discMaterial.SetColor("_Color", discColor);
			discMaterial.SetPass(0);
			GL.Begin(4);
			GL.Color(discColor);
			float num4 = 1f / (float)(num3 - 1);
			Vector3 v = discCenter + vector * magnitude;
			for (int i = 0; i < num3; i++)
			{
				Vector3 a = Quaternion.Slerp(Quaternion.identity, b, num4 * (float)i) * vector;
				a.Normalize();
				Vector3 vector2 = discCenter + a * magnitude;
				GL.Vertex(discCenter);
				GL.Vertex(vector2);
				GL.Vertex(v);
				v = vector2;
			}
			GL.End();
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x0014BCDC File Offset: 0x00149EDC
		public static void Draw2DFilledDisc(Vector2 discCenter, Vector2 firstPoint, Vector2 secondPoint, Color discColor, Material discMaterial, Camera camera)
		{
			Vector2 vector = firstPoint - discCenter;
			Vector2 v = secondPoint - discCenter;
			float magnitude = vector.magnitude;
			vector.Normalize();
			v.Normalize();
			float num;
			if (Vector3.Dot(Vector3.Cross(vector, v), Vector3.forward) < 0f)
			{
				num = -1f;
			}
			else
			{
				num = 1f;
			}
			float num2 = MathHelper.SafeAcos(Vector3.Dot(vector, v)) * 57.29578f * num;
			Quaternion b = Quaternion.AngleAxis(num2, Vector3.forward);
			int num3 = (int)(180f * Mathf.Abs(num2) / 180f);
			if (num3 < 2)
			{
				return;
			}
			discMaterial.SetColor("_Color", discColor);
			discMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(4);
			GL.Color(discColor);
			float num4 = 1f / (float)(num3 - 1);
			Vector3 vector2 = discCenter + vector * magnitude;
			for (int i = 0; i < num3; i++)
			{
				Vector2 a = Quaternion.Slerp(Quaternion.identity, b, num4 * (float)i) * vector;
				a.Normalize();
				Vector3 vector3 = discCenter + a * magnitude;
				GL.Vertex(camera.ScreenToViewportPoint(new Vector3(discCenter.x, discCenter.y, 0f)));
				GL.Vertex(camera.ScreenToViewportPoint(new Vector3(vector3.x, vector3.y, 0f)));
				GL.Vertex(camera.ScreenToViewportPoint(new Vector3(vector2.x, vector2.y, 0f)));
				vector2 = vector3;
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x0014BEAC File Offset: 0x0014A0AC
		public static void DrawWireSelectionBoxes(List<ObjectSelectionBox> selectionBoxes, float boxSizeAdd, Camera camera, Color lineColor, Material boxLineMaterial)
		{
			GL.PushMatrix();
			boxLineMaterial.SetColor("_Color", lineColor);
			boxLineMaterial.SetPass(0);
			Matrix4x4 identity = Matrix4x4.identity;
			ref Matrix4x4 ptr = ref identity;
			ptr[2, 2] = ptr[2, 2] * -1f;
			Matrix4x4 worldToLocalMatrix = camera.transform.worldToLocalMatrix;
			for (int i = 0; i < selectionBoxes.Count; i++)
			{
				ObjectSelectionBox objectSelectionBox = selectionBoxes[i];
				Box modelSpaceBox = objectSelectionBox.ModelSpaceBox;
				Matrix4x4 transformMatrix = objectSelectionBox.TransformMatrix;
				modelSpaceBox.Size += Vector3.one * boxSizeAdd;
				GL.LoadIdentity();
				GL.MultMatrix(identity * worldToLocalMatrix * transformMatrix);
				GL.Begin(1);
				GL.Color(lineColor);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				GL.Vertex3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x0014C350 File Offset: 0x0014A550
		public static void DrawCornerLinesForSelectionBoxes(List<ObjectSelectionBox> selectionBoxes, float boxSizeAdd, float cornerLinePercentage, Camera camera, Color cornerLineColor, Material boxLineMaterial)
		{
			GL.PushMatrix();
			boxLineMaterial.SetColor("_Color", cornerLineColor);
			boxLineMaterial.SetPass(0);
			Matrix4x4 identity = Matrix4x4.identity;
			ref Matrix4x4 ptr = ref identity;
			ptr[2, 2] = ptr[2, 2] * -1f;
			Matrix4x4 worldToLocalMatrix = camera.transform.worldToLocalMatrix;
			for (int i = 0; i < selectionBoxes.Count; i++)
			{
				ObjectSelectionBox objectSelectionBox = selectionBoxes[i];
				Box modelSpaceBox = objectSelectionBox.ModelSpaceBox;
				Matrix4x4 matrix4x = objectSelectionBox.TransformMatrix;
				modelSpaceBox.Size += Vector3.one * boxSizeAdd;
				Vector3 xyzscale = matrix4x.GetXYZScale();
				modelSpaceBox.Size = Vector3.Scale(modelSpaceBox.Size, xyzscale);
				modelSpaceBox.Center = Vector3.Scale(modelSpaceBox.Center, xyzscale);
				matrix4x = matrix4x.SetScaleToOneOnAllAxes();
				GL.LoadIdentity();
				GL.MultMatrix(identity * worldToLocalMatrix * matrix4x);
				GL.Begin(1);
				GL.Color(cornerLineColor);
				float d = cornerLinePercentage * modelSpaceBox.Extents.x;
				float d2 = cornerLinePercentage * modelSpaceBox.Extents.y;
				float d3 = cornerLinePercentage * modelSpaceBox.Extents.z;
				Vector3 vector = new Vector3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				Vector3 v = vector + Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v);
				Vector3 v2 = vector - Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v2);
				Vector3 v3 = vector + Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v3);
				vector = new Vector3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Min.z);
				Vector3 v4 = vector - Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v4);
				Vector3 v5 = vector - Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v5);
				Vector3 v6 = vector + Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v6);
				vector = new Vector3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				Vector3 v7 = vector - Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v7);
				Vector3 v8 = vector + Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v8);
				Vector3 v9 = vector + Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v9);
				vector = new Vector3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Min.z);
				Vector3 v10 = vector + Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v10);
				Vector3 v11 = vector + Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v11);
				Vector3 v12 = vector + Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v12);
				vector = new Vector3(modelSpaceBox.Min.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				Vector3 v13 = vector + Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v13);
				Vector3 v14 = vector - Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v14);
				Vector3 v15 = vector - Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v15);
				vector = new Vector3(modelSpaceBox.Max.x, modelSpaceBox.Max.y, modelSpaceBox.Max.z);
				Vector3 v16 = vector - Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v16);
				Vector3 v17 = vector - Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v17);
				Vector3 v18 = vector - Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v18);
				vector = new Vector3(modelSpaceBox.Max.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				Vector3 v19 = vector - Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v19);
				Vector3 v20 = vector + Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v20);
				Vector3 v21 = vector - Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v21);
				vector = new Vector3(modelSpaceBox.Min.x, modelSpaceBox.Min.y, modelSpaceBox.Max.z);
				Vector3 v22 = vector + Vector3.right * d;
				GL.Vertex(vector);
				GL.Vertex(v22);
				Vector3 v23 = vector + Vector3.up * d2;
				GL.Vertex(vector);
				GL.Vertex(v23);
				Vector3 v24 = vector - Vector3.forward * d3;
				GL.Vertex(vector);
				GL.Vertex(v24);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x0014C8CC File Offset: 0x0014AACC
		public static void DrawGridLines(float cellSizeX, float cellSizeZ, Camera camera, Material material, Color color)
		{
			Bounds aabb = camera.GetViewVolume(camera.farClipPlane).AABB;
			float x = aabb.min.x;
			float z = aabb.min.z;
			float x2 = aabb.max.x;
			float z2 = aabb.max.z;
			float num = 0.5f * cellSizeX;
			float num2 = 0.5f * cellSizeZ;
			int num3 = Mathf.FloorToInt((x + num) / cellSizeX) - 1;
			int num4 = Mathf.FloorToInt((x2 + num) / cellSizeX) + 1;
			int num5 = Mathf.FloorToInt((z + num2) / cellSizeZ) - 1;
			int num6 = Mathf.FloorToInt((z2 + num2) / cellSizeZ) + 1;
			material.SetColor("_Color", color);
			material.SetPass(0);
			object obj = (num3 < num5) ? num3 : num5;
			int num7 = (num4 > num6) ? num4 : num6;
			GL.Begin(1);
			object obj2 = obj;
			float d = obj2 * cellSizeZ;
			float d2 = (float)(num7 + 1) * cellSizeZ;
			float d3 = obj2 * cellSizeX;
			float d4 = (float)(num7 + 1) * cellSizeX;
			for (int i = obj2; i <= num7; i++)
			{
				Vector3 a = (float)i * Vector3.right * cellSizeX;
				GL.Vertex(a + Vector3.forward * d);
				GL.Vertex(a + Vector3.forward * d2);
				Vector3 a2 = (float)i * Vector3.forward * cellSizeZ;
				GL.Vertex(a2 + Vector3.right * d3);
				GL.Vertex(a2 + Vector3.right * d4);
			}
			GL.End();
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x0014CA54 File Offset: 0x0014AC54
		public static void DrawWireOOBB(OrientedBox oobb, Color wireColor, Material material)
		{
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.MultMatrix(GLPrimitives.CameraViewMatrix * oobb.TransformMatrix);
			List<Vector3> cornerPoints = oobb.ModelSpaceBox.GetCornerPoints();
			int passCount = material.passCount;
			for (int i = 0; i < passCount; i++)
			{
				material.SetPass(i);
				GL.Begin(1);
				GL.Color(wireColor);
				GL.Vertex(cornerPoints[0]);
				GL.Vertex(cornerPoints[1]);
				GL.Vertex(cornerPoints[1]);
				GL.Vertex(cornerPoints[2]);
				GL.Vertex(cornerPoints[2]);
				GL.Vertex(cornerPoints[3]);
				GL.Vertex(cornerPoints[3]);
				GL.Vertex(cornerPoints[0]);
				GL.Vertex(cornerPoints[4]);
				GL.Vertex(cornerPoints[5]);
				GL.Vertex(cornerPoints[5]);
				GL.Vertex(cornerPoints[6]);
				GL.Vertex(cornerPoints[6]);
				GL.Vertex(cornerPoints[7]);
				GL.Vertex(cornerPoints[7]);
				GL.Vertex(cornerPoints[4]);
				GL.Vertex(cornerPoints[5]);
				GL.Vertex(cornerPoints[0]);
				GL.Vertex(cornerPoints[6]);
				GL.Vertex(cornerPoints[3]);
				GL.Vertex(cornerPoints[4]);
				GL.Vertex(cornerPoints[1]);
				GL.Vertex(cornerPoints[7]);
				GL.Vertex(cornerPoints[2]);
				GL.End();
			}
			GL.PopMatrix();
		}
	}
}
