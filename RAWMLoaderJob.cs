using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class RAWMLoaderJob : ThreadedJob
{
	// Token: 0x060019D9 RID: 6617 RVA: 0x000B4DC8 File Offset: 0x000B2FC8
	protected override void ThreadFunction()
	{
		try
		{
			BitStream bitStream = new BitStream(this.inData);
			bitStream.ReadUint();
			int num = bitStream.ReadInt();
			this.vertices = new List<Vector3>(num);
			for (int i = 0; i < num; i++)
			{
				this.vertices.Add(bitStream.ReadVector3());
			}
			int num2 = bitStream.ReadInt();
			this.uv = new List<Vector2>(num2);
			for (int j = 0; j < num2; j++)
			{
				this.uv.Add(bitStream.ReadVector2());
			}
			int num3 = bitStream.ReadInt();
			this.normals = new List<Vector3>(num3);
			for (int k = 0; k < num3; k++)
			{
				this.normals.Add(bitStream.ReadVector3());
			}
			int num4 = bitStream.ReadInt();
			this.tangents = new List<Vector4>(num4);
			for (int l = 0; l < num4; l++)
			{
				this.tangents.Add(bitStream.ReadVector4());
			}
			this.triangles = new List<int>[bitStream.ReadInt()];
			for (int m = 0; m < this.triangles.Length; m++)
			{
				int num5 = bitStream.ReadInt();
				this.triangles[m] = new List<int>(num5);
				for (int n = 0; n < num5; n++)
				{
					this.triangles[m].Add(bitStream.ReadInt());
				}
			}
		}
		catch (Exception error)
		{
			base.SetError(error);
		}
	}

	// Token: 0x04000FD7 RID: 4055
	public byte[] inData;

	// Token: 0x04000FD8 RID: 4056
	public List<Vector3> vertices;

	// Token: 0x04000FD9 RID: 4057
	public List<Vector2> uv;

	// Token: 0x04000FDA RID: 4058
	public List<Vector3> normals;

	// Token: 0x04000FDB RID: 4059
	public List<Vector4> tangents;

	// Token: 0x04000FDC RID: 4060
	public List<int>[] triangles;
}
