using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000453 RID: 1107
	public class EditorMesh
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x00154182 File Offset: 0x00152382
		public Mesh Mesh
		{
			get
			{
				return this._mesh;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x0015418A File Offset: 0x0015238A
		public int NumberOfTriangles
		{
			get
			{
				return this._numberOfTriangles;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x00154192 File Offset: 0x00152392
		public Vector3[] VertexPositions
		{
			get
			{
				return this._vertexPositions.Clone() as Vector3[];
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x001541A4 File Offset: 0x001523A4
		public int[] VertexIndices
		{
			get
			{
				return this._vertexIndices.Clone() as int[];
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x001541B6 File Offset: 0x001523B6
		public bool IsBuildingTreeSilent
		{
			get
			{
				return this._meshSphereTree.IsBuildingSilent;
			}
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x001541C4 File Offset: 0x001523C4
		public EditorMesh(Mesh mesh)
		{
			this._mesh = mesh;
			this._vertexPositions = this._mesh.vertices;
			this._vertexIndices = this._mesh.triangles;
			this._numberOfTriangles = this._vertexIndices.Length / 3;
			this._meshSphereTree = new MeshSphereTree(this);
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x0015421C File Offset: 0x0015241C
		public void StartSilentTreeBuild()
		{
			this._meshSphereTree.BuildSilent();
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x00154229 File Offset: 0x00152429
		public Box GetBox()
		{
			if (this._mesh == null)
			{
				return Box.GetInvalid();
			}
			return new Box(this._mesh.bounds);
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x0015424F File Offset: 0x0015244F
		public OrientedBox GetOrientedBox(Matrix4x4 transformMatrix)
		{
			if (this._mesh == null)
			{
				return OrientedBox.GetInvalid();
			}
			OrientedBox orientedBox = new OrientedBox(this.GetBox());
			orientedBox.Transform(transformMatrix);
			return orientedBox;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x00154278 File Offset: 0x00152478
		public List<Triangle3D> GetAllTriangles()
		{
			if (this.NumberOfTriangles == 0)
			{
				return new List<Triangle3D>();
			}
			List<Triangle3D> list = new List<Triangle3D>(this.NumberOfTriangles);
			for (int i = 0; i < this.NumberOfTriangles; i++)
			{
				list.Add(this.GetTriangle(i));
			}
			return list;
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x001542C0 File Offset: 0x001524C0
		public Triangle3D GetTriangle(int triangleIndex)
		{
			int num = triangleIndex * 3;
			return new Triangle3D(this._vertexPositions[this._vertexIndices[num]], this._vertexPositions[this._vertexIndices[num + 1]], this._vertexPositions[this._vertexIndices[num + 2]]);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00154313 File Offset: 0x00152513
		public MeshRayHit Raycast(Ray ray, Matrix4x4 meshTransformMatrix)
		{
			return this._meshSphereTree.Raycast(ray, meshTransformMatrix);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00154322 File Offset: 0x00152522
		public List<Vector3> GetOverlappedWorldVerts(Box box, Matrix4x4 meshTransformMatrix)
		{
			return this._meshSphereTree.GetOverlappedWorldVerts(box.ToOrientedBox(), meshTransformMatrix);
		}

		// Token: 0x04002080 RID: 8320
		private Mesh _mesh;

		// Token: 0x04002081 RID: 8321
		private Vector3[] _vertexPositions;

		// Token: 0x04002082 RID: 8322
		private int[] _vertexIndices;

		// Token: 0x04002083 RID: 8323
		private int _numberOfTriangles;

		// Token: 0x04002084 RID: 8324
		private MeshSphereTree _meshSphereTree;
	}
}
