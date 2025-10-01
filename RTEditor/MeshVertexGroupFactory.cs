using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000411 RID: 1041
	public static class MeshVertexGroupFactory
	{
		// Token: 0x06003058 RID: 12376 RVA: 0x0014A754 File Offset: 0x00148954
		public static List<MeshVertexGroup> Create(Mesh mesh)
		{
			if (!mesh.isReadable)
			{
				return new List<MeshVertexGroup>();
			}
			Vector3 size = mesh.bounds.size;
			Vector3[] vertices = mesh.vertices;
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			float num3 = size.z / 2f;
			Dictionary<MeshVertexGroupFactory.VertexGroupIndices, MeshVertexGroup> dictionary = new Dictionary<MeshVertexGroupFactory.VertexGroupIndices, MeshVertexGroup>();
			foreach (Vector3 vector in vertices)
			{
				int xIndex = Mathf.FloorToInt(vector.x / num);
				int yIndex = Mathf.FloorToInt(vector.y / num2);
				int zIndex = Mathf.FloorToInt(vector.z / num3);
				MeshVertexGroupFactory.VertexGroupIndices key = new MeshVertexGroupFactory.VertexGroupIndices(xIndex, yIndex, zIndex);
				if (dictionary.ContainsKey(key))
				{
					dictionary[key].AddVertex(vector);
				}
				else
				{
					MeshVertexGroup meshVertexGroup = new MeshVertexGroup();
					meshVertexGroup.AddVertex(vector);
					dictionary.Add(key, meshVertexGroup);
				}
			}
			if (dictionary.Count == 0)
			{
				return new List<MeshVertexGroup>();
			}
			List<MeshVertexGroup> list = new List<MeshVertexGroup>(dictionary.Count);
			foreach (KeyValuePair<MeshVertexGroupFactory.VertexGroupIndices, MeshVertexGroup> keyValuePair in dictionary)
			{
				MeshVertexGroup value = keyValuePair.Value;
				value.Close();
				list.Add(value);
			}
			return list;
		}

		// Token: 0x02000803 RID: 2051
		private struct VertexGroupIndices
		{
			// Token: 0x060040BE RID: 16574 RVA: 0x00183B31 File Offset: 0x00181D31
			public VertexGroupIndices(int xIndex, int yIndex, int zIndex)
			{
				this.XIndex = xIndex;
				this.YIndex = yIndex;
				this.ZIndex = zIndex;
			}

			// Token: 0x04002DFC RID: 11772
			public int XIndex;

			// Token: 0x04002DFD RID: 11773
			public int YIndex;

			// Token: 0x04002DFE RID: 11774
			public int ZIndex;
		}
	}
}
