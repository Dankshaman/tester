using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000412 RID: 1042
	public class MeshVertexGroupMappings : SingletonBase<MeshVertexGroupMappings>
	{
		// Token: 0x06003059 RID: 12377 RVA: 0x0014A8C0 File Offset: 0x00148AC0
		public bool CreateMappingForMesh(Mesh mesh)
		{
			if (mesh == null || !mesh.isReadable)
			{
				return false;
			}
			if (this._meshToVertexGroups.ContainsKey(mesh))
			{
				this._meshToVertexGroups.Remove(mesh);
			}
			List<MeshVertexGroup> list = MeshVertexGroupFactory.Create(mesh);
			if (list.Count != 0)
			{
				this._meshToVertexGroups.Add(mesh, list);
				return true;
			}
			return false;
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x0014A91C File Offset: 0x00148B1C
		public List<MeshVertexGroup> GetMeshVertexGroups(Mesh mesh)
		{
			if (this._meshToVertexGroups.ContainsKey(mesh))
			{
				return new List<MeshVertexGroup>(this._meshToVertexGroups[mesh]);
			}
			if (this.CreateMappingForMesh(mesh))
			{
				return new List<MeshVertexGroup>(this._meshToVertexGroups[mesh]);
			}
			return new List<MeshVertexGroup>();
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x0014A969 File Offset: 0x00148B69
		public bool ContainsMappingForMesh(Mesh mesh)
		{
			return mesh != null && this._meshToVertexGroups.ContainsKey(mesh);
		}

		// Token: 0x04001F95 RID: 8085
		private Dictionary<Mesh, List<MeshVertexGroup>> _meshToVertexGroups = new Dictionary<Mesh, List<MeshVertexGroup>>();
	}
}
