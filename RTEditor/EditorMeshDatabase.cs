using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000454 RID: 1108
	public class EditorMeshDatabase : MonoSingletonBase<EditorMeshDatabase>
	{
		// Token: 0x060032AE RID: 12974 RVA: 0x00154338 File Offset: 0x00152538
		public bool AddMeshToSilentBuild(EditorMesh editorMesh)
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return false;
			}
			if (!this.Contains(editorMesh.Mesh) || this.IsMeshSilentBuilding(editorMesh))
			{
				return false;
			}
			this._sortedSilentBuildCandidates.Add(editorMesh);
			this._sortedSilentBuildCandidates.Sort(delegate(EditorMesh mesh0, EditorMesh mesh1)
			{
				if (mesh0.NumberOfTriangles < mesh1.NumberOfTriangles)
				{
					return 1;
				}
				if (mesh1.NumberOfTriangles < mesh0.NumberOfTriangles)
				{
					return -1;
				}
				return 0;
			});
			return true;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x001543A4 File Offset: 0x001525A4
		public bool AddMeshesToSilentBuild(List<EditorMesh> editorMeshes)
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return false;
			}
			if (editorMeshes == null || editorMeshes.Count == 0)
			{
				return false;
			}
			bool result = true;
			foreach (EditorMesh editorMesh in editorMeshes)
			{
				if (!this.AddMeshToSilentBuild(editorMesh))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x00154414 File Offset: 0x00152614
		public EditorMesh CreateEditorMesh(Mesh mesh)
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return null;
			}
			if (!this.IsMeshValid(mesh))
			{
				return null;
			}
			if (this._meshes.ContainsKey(mesh))
			{
				return null;
			}
			EditorMesh editorMesh = new EditorMesh(mesh);
			this._meshes.Add(mesh, editorMesh);
			return editorMesh;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x00154460 File Offset: 0x00152660
		public List<EditorMesh> CreateEditorMeshes(List<Mesh> meshes)
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return new List<EditorMesh>();
			}
			if (meshes == null || meshes.Count == 0)
			{
				return new List<EditorMesh>();
			}
			List<EditorMesh> list = new List<EditorMesh>(meshes.Count);
			foreach (Mesh mesh in meshes)
			{
				EditorMesh editorMesh = this.CreateEditorMesh(mesh);
				if (editorMesh != null)
				{
					list.Add(editorMesh);
				}
			}
			return list;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x001544EC File Offset: 0x001526EC
		public EditorMesh GetEditorMesh(Mesh mesh)
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return null;
			}
			if (!this.IsMeshValid(mesh))
			{
				return null;
			}
			if (this.Contains(mesh))
			{
				return this._meshes[mesh];
			}
			return this.CreateEditorMesh(mesh);
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00154524 File Offset: 0x00152724
		public bool Contains(Mesh mesh)
		{
			return mesh != null && this._meshes.ContainsKey(mesh);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x0015453D File Offset: 0x0015273D
		public bool IsMeshValid(Mesh mesh)
		{
			return mesh != null && mesh.isReadable;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00154550 File Offset: 0x00152750
		public bool IsMeshSilentBuilding(EditorMesh editorMesh)
		{
			return this._silentBuildMeshes.Contains(editorMesh) || this._sortedSilentBuildCandidates.Contains(editorMesh);
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x0015456E File Offset: 0x0015276E
		private void Start()
		{
			base.StartCoroutine(this.DoEditorMeshSilentBuild());
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00154580 File Offset: 0x00152780
		private void RemoveNullMeshEntries()
		{
			Dictionary<Mesh, EditorMesh> dictionary = new Dictionary<Mesh, EditorMesh>();
			foreach (KeyValuePair<Mesh, EditorMesh> keyValuePair in this._meshes)
			{
				if (keyValuePair.Key != null)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			this._meshes = dictionary;
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x001545FC File Offset: 0x001527FC
		private IEnumerator DoEditorMeshSilentBuild()
		{
			for (;;)
			{
				if (!MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
				{
					if (this._silentBuildMeshes.Count < 1)
					{
						while (this._silentBuildMeshes.Count < 1 && this._sortedSilentBuildCandidates.Count != 0)
						{
							EditorMesh editorMesh = this._sortedSilentBuildCandidates[0];
							this._silentBuildMeshes.Add(editorMesh);
							editorMesh.StartSilentTreeBuild();
							this._sortedSilentBuildCandidates.RemoveAt(0);
						}
					}
					this._silentBuildMeshes.RemoveAll((EditorMesh item) => !item.IsBuildingTreeSilent);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04002085 RID: 8325
		private Dictionary<Mesh, EditorMesh> _meshes = new Dictionary<Mesh, EditorMesh>();

		// Token: 0x04002086 RID: 8326
		private const int _maxNumberOfSilentBuildMeshes = 1;

		// Token: 0x04002087 RID: 8327
		private List<EditorMesh> _sortedSilentBuildCandidates = new List<EditorMesh>();

		// Token: 0x04002088 RID: 8328
		private List<EditorMesh> _silentBuildMeshes = new List<EditorMesh>();
	}
}
