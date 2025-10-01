using System;
using NewNet;

// Token: 0x020001EA RID: 490
public class RAWMSaverJob : ThreadedJob
{
	// Token: 0x060019DB RID: 6619 RVA: 0x000B4F4C File Offset: 0x000B314C
	protected override void ThreadFunction()
	{
		try
		{
			this.bitStream.WriteByte(Convert.ToByte('r'));
			this.bitStream.WriteByte(Convert.ToByte('a'));
			this.bitStream.WriteByte(Convert.ToByte('w'));
			this.bitStream.WriteByte(Convert.ToByte('m'));
			this.bitStream.WriteInt(this.rawMeshData.vertices.Length);
			for (int i = 0; i < this.rawMeshData.vertices.Length; i++)
			{
				this.bitStream.WriteVector3(this.rawMeshData.vertices[i]);
			}
			this.bitStream.WriteInt(this.rawMeshData.uv.Length);
			for (int j = 0; j < this.rawMeshData.uv.Length; j++)
			{
				this.bitStream.WriteVector2(this.rawMeshData.uv[j]);
			}
			this.bitStream.WriteInt(this.rawMeshData.normals.Length);
			for (int k = 0; k < this.rawMeshData.normals.Length; k++)
			{
				this.bitStream.WriteVector3(this.rawMeshData.normals[k]);
			}
			this.bitStream.WriteInt(this.rawMeshData.tangents.Length);
			for (int l = 0; l < this.rawMeshData.tangents.Length; l++)
			{
				this.bitStream.WriteVector4(this.rawMeshData.tangents[l]);
			}
			this.bitStream.WriteInt(this.rawMeshData.triangles.Length);
			for (int m = 0; m < this.rawMeshData.triangles.Length; m++)
			{
				int[] array = this.rawMeshData.triangles[m];
				this.bitStream.WriteInt(array.Length);
				for (int n = 0; n < array.Length; n++)
				{
					this.bitStream.WriteInt(array[n]);
				}
			}
		}
		catch (Exception error)
		{
			base.SetError(error);
		}
	}

	// Token: 0x04000FDD RID: 4061
	public RawMeshData rawMeshData;

	// Token: 0x04000FDE RID: 4062
	public BitStream bitStream;
}
