using System;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class TokenMeshJob : ThreadedJob
{
	// Token: 0x06001FC1 RID: 8129 RVA: 0x000E26E0 File Offset: 0x000E08E0
	protected override void ThreadFunction()
	{
		try
		{
			this.TGO = TTSMeshCreatorThreadSafe.CreateMesh(this.TGO);
		}
		catch (Exception error)
		{
			base.SetError(error);
		}
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000E271C File Offset: 0x000E091C
	protected override void OnFinished()
	{
		if (base.isError)
		{
			return;
		}
		if (!this.GO)
		{
			Debug.LogWarning("Object is null for CustomTokenMeshJob OnFinished");
			return;
		}
		this.mesh = new Mesh();
		this.mesh.vertices = this.TGO.meshes[0].vertices;
		if (this.TGO.meshes[0].uv != null)
		{
			this.mesh.uv = this.TGO.meshes[0].uv;
		}
		if (this.TGO.meshes[0].normals != null)
		{
			this.mesh.normals = this.TGO.meshes[0].normals;
		}
		if (this.TGO.meshes[0].tangents != null)
		{
			this.mesh.tangents = this.TGO.meshes[0].tangents;
		}
		if (this.TGO.meshes[0].triangles != null)
		{
			this.mesh.triangles = this.TGO.meshes[0].triangles;
		}
		else
		{
			Debug.LogError("Error Loading Model/Mesh, no triangles detected.");
		}
		if (this.TGO.meshes[0].subMeshCount > 1)
		{
			this.mesh.subMeshCount = this.TGO.meshes[0].subMeshCount;
			this.mesh.SetTriangles(this.TGO.meshes[0].submesh_triangles[0], 1);
			this.mesh.SetTriangles(this.TGO.meshes[0].submesh_triangles[0], 2);
		}
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateBounds();
		Bounds bounds = this.mesh.bounds;
		this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(bounds.size.x, bounds.size.y, bounds.size.z));
		Quaternion q = Quaternion.Euler(270f, 0f, 0f);
		Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
		Vector3[] vertices = this.mesh.vertices;
		Vector3[] array = new Vector3[vertices.Length];
		Vector3[] normals = this.mesh.normals;
		Vector3[] array2 = new Vector3[normals.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			array[i] = matrix4x.MultiplyPoint3x4(vertices[i]);
			array2[i] = matrix4x.MultiplyPoint3x4(normals[i]);
		}
		this.mesh.vertices = array;
		this.mesh.normals = array2;
		this.mesh.RecalculateBounds();
		bounds = this.mesh.bounds;
		this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(bounds.size.x, bounds.size.y, bounds.size.z));
		MeshFilter meshFilter = this.GO.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = this.GO.AddComponent<MeshFilter>();
		}
		meshFilter.sharedMesh = this.mesh;
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000E2A87 File Offset: 0x000E0C87
	public override void Reset()
	{
		base.Reset();
		this.GO = null;
		this.TGO = new TTSGameObject(true);
	}

	// Token: 0x04001377 RID: 4983
	public GameObject GO;

	// Token: 0x04001378 RID: 4984
	public TTSGameObject TGO;

	// Token: 0x04001379 RID: 4985
	public Mesh mesh;
}
