﻿using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000443 RID: 1091
	public static class ProceduralMeshGenerator
	{
		// Token: 0x060031F9 RID: 12793 RVA: 0x00151508 File Offset: 0x0014F708
		public static Mesh CreateXZGridLineMesh(float cellSizeX, float cellSizeZ, int cellCountX, int cellCountZ)
		{
			cellSizeX = Mathf.Max(0.0001f, cellSizeX);
			cellSizeZ = Mathf.Max(0.0001f, cellSizeZ);
			cellCountX = Mathf.Max(1, cellCountX);
			cellCountZ = Mathf.Max(1, cellCountZ);
			int num = cellCountX + 1;
			int num2 = cellCountZ + 1;
			int num3 = num * num2 * 2;
			Vector3[] array = new Vector3[num3];
			Vector3[] array2 = new Vector3[num3];
			for (int i = 0; i < num3; i++)
			{
				array2[i] = Vector3.up;
			}
			int[] array3 = new int[num3];
			int num4 = 0;
			for (int j = 0; j < num; j++)
			{
				Vector3 vector = Vector3.right * cellSizeX * (float)j;
				Vector3 vector2 = vector + Vector3.forward * cellSizeZ * (float)(num2 - 1);
				array3[num4] = num4;
				array[num4++] = vector;
				array3[num4] = num4;
				array[num4++] = vector2;
			}
			for (int k = 0; k < num2; k++)
			{
				Vector3 vector3 = Vector3.forward * cellSizeZ * (float)k;
				Vector3 vector4 = vector3 + Vector3.right * cellSizeX * (float)(num - 1);
				array3[num4] = num4;
				array[num4++] = vector3;
				array3[num4] = num4;
				array[num4++] = vector4;
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.SetIndices(array3, MeshTopology.Lines, 0);
			return mesh;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x00151684 File Offset: 0x0014F884
		public static Mesh CreateRightAngledTriangleMesh(float lengthOfXAxisSide, float lengthOfYAxisSide)
		{
			lengthOfYAxisSide = Mathf.Max(lengthOfYAxisSide, 0.0001f);
			lengthOfXAxisSide = Mathf.Max(lengthOfXAxisSide, 0.0001f);
			Vector3[] array = new Vector3[3];
			Vector3[] array2 = new Vector3[3];
			array[0] = Vector3.zero;
			array[1] = Vector3.up * lengthOfYAxisSide;
			array[2] = Vector3.right * lengthOfXAxisSide;
			array2[0] = -Vector3.forward;
			array2[1] = array2[0];
			array2[2] = array2[0];
			int[] indices = new int[]
			{
				0,
				1,
				2
			};
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
			return mesh;
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x00151744 File Offset: 0x0014F944
		public static Mesh CreateBoxMesh(float width, float height, float depth)
		{
			width = Mathf.Max(width, 0.0001f);
			height = Mathf.Max(height, 0.0001f);
			depth = Mathf.Max(depth, 0.0001f);
			float num = width * 0.5f;
			float num2 = height * 0.5f;
			float num3 = depth * 0.5f;
			Vector3[] array = new Vector3[24];
			Vector3[] array2 = new Vector3[24];
			array[0] = new Vector3(-num, -num2, -num3);
			array[1] = new Vector3(-num, num2, -num3);
			array[2] = new Vector3(num, num2, -num3);
			array[3] = new Vector3(num, -num2, -num3);
			array2[0] = -Vector3.forward;
			array2[1] = array2[0];
			array2[2] = array2[0];
			array2[3] = array2[0];
			array[4] = new Vector3(-num, -num2, num3);
			array[5] = new Vector3(num, -num2, num3);
			array[6] = new Vector3(num, num2, num3);
			array[7] = new Vector3(-num, num2, num3);
			array2[4] = Vector3.forward;
			array2[5] = array2[4];
			array2[6] = array2[4];
			array2[7] = array2[4];
			array[8] = new Vector3(-num, -num2, num3);
			array[9] = new Vector3(-num, num2, num3);
			array[10] = new Vector3(-num, num2, -num3);
			array[11] = new Vector3(-num, -num2, -num3);
			array2[8] = -Vector3.right;
			array2[9] = array2[8];
			array2[10] = array2[8];
			array2[11] = array2[8];
			array[12] = new Vector3(num, -num2, -num3);
			array[13] = new Vector3(num, num2, -num3);
			array[14] = new Vector3(num, num2, num3);
			array[15] = new Vector3(num, -num2, num3);
			array2[12] = Vector3.right;
			array2[13] = array2[12];
			array2[14] = array2[12];
			array2[15] = array2[12];
			array[16] = new Vector3(-num, num2, -num3);
			array[17] = new Vector3(-num, num2, num3);
			array[18] = new Vector3(num, num2, num3);
			array[19] = new Vector3(num, num2, -num3);
			array2[16] = Vector3.up;
			array2[17] = array2[16];
			array2[18] = array2[16];
			array2[19] = array2[16];
			array[20] = new Vector3(-num, -num2, -num3);
			array[21] = new Vector3(num, -num2, -num3);
			array[22] = new Vector3(num, -num2, num3);
			array[23] = new Vector3(-num, -num2, num3);
			array2[20] = -Vector3.up;
			array2[21] = array2[20];
			array2[22] = array2[20];
			array2[23] = array2[20];
			int[] indices = new int[]
			{
				0,
				1,
				2,
				0,
				2,
				3,
				4,
				5,
				6,
				4,
				6,
				7,
				8,
				9,
				10,
				8,
				10,
				11,
				12,
				13,
				14,
				12,
				14,
				15,
				16,
				17,
				18,
				16,
				18,
				19,
				20,
				21,
				22,
				20,
				22,
				23
			};
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
			return mesh;
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x00151B04 File Offset: 0x0014FD04
		public static Mesh CreatePlaneMesh(float sizeAlongX, float sizeAlongY)
		{
			sizeAlongX = Mathf.Max(sizeAlongX, 0.0001f);
			sizeAlongY = Mathf.Max(sizeAlongY, 0.0001f);
			float num = sizeAlongX * 0.5f;
			float num2 = sizeAlongY * 0.5f;
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			Vector2[] array3 = new Vector2[4];
			array[0] = new Vector3(-num, -num2, 0f);
			array[1] = new Vector3(-num, num2, 0f);
			array[2] = new Vector3(num, num2, 0f);
			array[3] = new Vector3(num, -num2, 0f);
			array2[0] = new Vector3(0f, 0f, -1f);
			array2[1] = array2[0];
			array2[2] = array2[0];
			array2[3] = array2[0];
			array3[0] = new Vector2(0f, 0f);
			array3[1] = new Vector2(0f, 1f);
			array3[2] = new Vector2(1f, 1f);
			array3[3] = new Vector2(1f, 0f);
			int[] indices = new int[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			};
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.uv = array3;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
			return mesh;
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x00151C7C File Offset: 0x0014FE7C
		public static Mesh CreateSphereMesh(float sphereRadius, int numberOfVerticalSlices, int numberOfStacks)
		{
			sphereRadius = Mathf.Max(0.01f, sphereRadius);
			numberOfVerticalSlices = Mathf.Max(3, numberOfVerticalSlices);
			numberOfStacks = Mathf.Max(2, numberOfStacks);
			int num = numberOfStacks + 1;
			int num2 = numberOfVerticalSlices + 1;
			int num3 = num2 * num;
			Vector3[] array = new Vector3[num3];
			Vector3[] array2 = new Vector3[num3];
			int num4 = 0;
			float num5 = 2f * sphereRadius;
			float num6 = num5 / (float)numberOfStacks;
			float num7 = 360f / (float)numberOfVerticalSlices;
			for (int i = 0; i < num; i++)
			{
				float num8 = sphereRadius - num6 * (float)i;
				float num9 = (sphereRadius - num8) / num5 * 180f;
				for (int j = 0; j < num2; j++)
				{
					array2[num4] = Vector3.up;
					Quaternion rotation = Quaternion.Euler(-num9, -num7 * (float)j, 0f);
					array2[num4] = rotation * array2[num4];
					array2[num4].Normalize();
					array[num4] = array2[num4] * sphereRadius;
					num4++;
				}
			}
			int[] array3 = new int[numberOfStacks * numberOfVerticalSlices * 2 * 3];
			int num10 = 0;
			for (int k = 0; k < num - 1; k++)
			{
				for (int l = 0; l < num2 - 1; l++)
				{
					int num11 = k * num2 + l;
					array3[num10++] = num11;
					array3[num10++] = num11 + num2 + 1;
					array3[num10++] = num11 + num2;
					array3[num10++] = num11;
					array3[num10++] = num11 + 1;
					array3[num10++] = num11 + num2 + 1;
				}
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.SetIndices(array3, MeshTopology.Triangles, 0);
			return mesh;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x00151E30 File Offset: 0x00150030
		public static Mesh CreateConeMesh(float bottomRadius, float height, int numberOfVerticalSlices, int numberOfHorizontalSlices, int numberOfBottomCapRings)
		{
			bottomRadius = Mathf.Abs(bottomRadius);
			height = Mathf.Abs(height);
			numberOfVerticalSlices = Mathf.Max(3, numberOfVerticalSlices);
			numberOfHorizontalSlices = Mathf.Max(1, numberOfHorizontalSlices);
			numberOfBottomCapRings = Mathf.Max(numberOfBottomCapRings, 2);
			int num = numberOfHorizontalSlices + 1;
			int num2 = numberOfVerticalSlices + 1;
			int num3 = (numberOfVerticalSlices + 1) * numberOfBottomCapRings;
			int num4 = num * num2 + num3;
			Vector3[] array = new Vector3[num4];
			Vector3[] array2 = new Vector3[num4];
			float num5 = -bottomRadius / (float)numberOfHorizontalSlices;
			float num6 = 360f / (float)numberOfVerticalSlices;
			int num7 = 0;
			for (int i = 0; i < num; i++)
			{
				float num8 = bottomRadius + num5 * (float)i;
				float y = height * (float)i / (float)(num - 1);
				for (int j = 0; j < num2; j++)
				{
					float num9 = Mathf.Cos(0.017453292f * num6 * (float)j);
					float num10 = Mathf.Sin(0.017453292f * num6 * (float)j);
					array[num7] = new Vector3(num9 * num8, y, num10 * num8);
					array2[num7] = new Vector3(num9, 0f, num10);
					array2[num7].Normalize();
					num7++;
				}
			}
			num5 = bottomRadius / (float)(numberOfBottomCapRings - 1);
			for (int k = 0; k < numberOfBottomCapRings; k++)
			{
				float num11 = (float)k * num5;
				for (int l = 0; l < num2; l++)
				{
					array[num7] = new Vector3(Mathf.Cos(0.017453292f * num6 * (float)l) * num11, 0f, Mathf.Sin(0.017453292f * num6 * (float)l) * num11);
					array2[num7] = -Vector3.up;
					num7++;
				}
			}
			int num12 = numberOfVerticalSlices * (numberOfBottomCapRings - 1) * 2;
			int[] array3 = new int[(numberOfVerticalSlices * 2 * numberOfHorizontalSlices + num12) * 3];
			int num13 = 0;
			for (int m = 0; m < num - 1; m++)
			{
				for (int n = 0; n < num2 - 1; n++)
				{
					int num14 = m * num2 + n;
					array3[num13++] = num14;
					array3[num13++] = num14 + num2;
					array3[num13++] = num14 + 1;
					array3[num13++] = num14 + num2;
					array3[num13++] = num14 + num2 + 1;
					array3[num13++] = num14 + 1;
				}
			}
			int num15 = num4 - num3;
			for (int num16 = 0; num16 < numberOfBottomCapRings - 1; num16++)
			{
				for (int num17 = 0; num17 < num2 - 1; num17++)
				{
					int num18 = num15 + num16 * num2 + num17;
					array3[num13++] = num18;
					array3[num13++] = num18 + num2;
					array3[num13++] = num18 + 1;
					array3[num13++] = num18 + num2;
					array3[num13++] = num18 + num2 + 1;
					array3[num13++] = num18 + 1;
				}
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.SetIndices(array3, MeshTopology.Triangles, 0);
			return mesh;
		}
	}
}
