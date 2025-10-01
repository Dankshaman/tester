using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class MeshTangentsJob : ThreadedJob
{
	// Token: 0x060015FB RID: 5627 RVA: 0x0009927C File Offset: 0x0009747C
	protected override void ThreadFunction()
	{
		if (this.vertices != null && this.normals != null && this.uv != null && this.triangles != null)
		{
			try
			{
				this.tangents = TTSObjReader.CalculateTangents(this.vertices, this.normals, this.uv, this.triangles);
				return;
			}
			catch (Exception error)
			{
				base.SetError(error);
				return;
			}
		}
		base.isError = true;
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void OnFinished()
	{
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x000992F0 File Offset: 0x000974F0
	public override void Reset()
	{
		base.Reset();
		this.tangents = null;
		this.vertices = null;
		this.normals = null;
		this.uv = null;
		this.triangles = null;
		base.isError = false;
	}

	// Token: 0x04000C66 RID: 3174
	public Vector4[] tangents;

	// Token: 0x04000C67 RID: 3175
	public Vector3[] vertices;

	// Token: 0x04000C68 RID: 3176
	public Vector3[] normals;

	// Token: 0x04000C69 RID: 3177
	public Vector2[] uv;

	// Token: 0x04000C6A RID: 3178
	public int[] triangles;
}
