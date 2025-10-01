using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200044C RID: 1100
	public class MeshSphereTree
	{
		// Token: 0x06003251 RID: 12881 RVA: 0x001531F0 File Offset: 0x001513F0
		public MeshSphereTree(EditorMesh editorMesh)
		{
			this._editorMesh = editorMesh;
			this._sphereTree = new SphereTree<MeshSphereTreeTriangle>(2);
			this._buildJob = new MeshSphereTreeBuildJob(this._editorMesh.GetAllTriangles(), this._sphereTree);
			this._buildJob.ValidateTriangle = new MeshSphereTreeBuildJob.TriangleValidation(this.IsTriangleValid);
			this._buildJob.SilentBuildFinished += this.OnSilentBuildFinished;
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06003252 RID: 12882 RVA: 0x00153260 File Offset: 0x00151460
		public bool IsBuildingSilent
		{
			get
			{
				return this._buildJob.IsRunning;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x0015326D File Offset: 0x0015146D
		public bool WasBuilt
		{
			get
			{
				return this._wasBuilt;
			}
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x00153278 File Offset: 0x00151478
		public List<Vector3> GetOverlappedWorldVerts(OrientedBox box, Matrix4x4 meshTransformMatrix)
		{
			if (this.IsBuildingSilent)
			{
				while (this.IsBuildingSilent)
				{
				}
			}
			if (!this._wasBuilt)
			{
				this.Build();
			}
			Matrix4x4 inverse = meshTransformMatrix.inverse;
			box.Transform(inverse);
			List<SphereTreeNode<MeshSphereTreeTriangle>> list = this._sphereTree.OverlapBox(box);
			if (list.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list2 = new List<Vector3>();
			foreach (SphereTreeNode<MeshSphereTreeTriangle> sphereTreeNode in list)
			{
				int triangleIndex = sphereTreeNode.Data.TriangleIndex;
				foreach (Vector3 point in this._editorMesh.GetTriangle(triangleIndex).GetPoints())
				{
					if (box.ContainsPoint(point))
					{
						list2.Add(meshTransformMatrix.MultiplyPoint(point));
					}
				}
			}
			return list2;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0015337C File Offset: 0x0015157C
		public MeshRayHit Raycast(Ray ray, Matrix4x4 meshTransformMatrix)
		{
			if (this.IsBuildingSilent)
			{
				while (this.IsBuildingSilent)
				{
				}
			}
			if (!this._wasBuilt)
			{
				this.Build();
			}
			Ray ray2 = ray.InverseTransform(meshTransformMatrix);
			List<SphereTreeNodeRayHit<MeshSphereTreeTriangle>> list = this._sphereTree.RaycastAll(ray2);
			if (list.Count == 0)
			{
				return null;
			}
			float num = float.MaxValue;
			Triangle3D triangle3D = null;
			int hitTriangleIndex = -1;
			Vector3 vector = Vector3.zero;
			foreach (SphereTreeNodeRayHit<MeshSphereTreeTriangle> sphereTreeNodeRayHit in list)
			{
				MeshSphereTreeTriangle data = sphereTreeNodeRayHit.HitNode.Data;
				Triangle3D triangle = this._editorMesh.GetTriangle(data.TriangleIndex);
				float num2;
				if (triangle.Raycast(ray2, out num2) && num2 < num)
				{
					num = num2;
					triangle3D = triangle;
					hitTriangleIndex = data.TriangleIndex;
					vector = ray2.GetPoint(num2);
				}
			}
			if (triangle3D != null)
			{
				vector = meshTransformMatrix.MultiplyPoint(vector);
				num = (ray.origin - vector).magnitude;
				Vector3 hitNormal = meshTransformMatrix.MultiplyVector(triangle3D.Normal);
				return new MeshRayHit(ray, num, hitTriangleIndex, vector, hitNormal);
			}
			return null;
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x001534A8 File Offset: 0x001516A8
		public void Build()
		{
			if (this._wasBuilt || this._buildJob.IsRunning)
			{
				return;
			}
			for (int i = 0; i < this._editorMesh.NumberOfTriangles; i++)
			{
				this.RegisterTriangle(i);
			}
			this._sphereTree.PerformPendingUpdates();
			this._wasBuilt = true;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x001534FB File Offset: 0x001516FB
		public void BuildSilent()
		{
			if (this._wasBuilt || this._buildJob.IsRunning)
			{
				return;
			}
			this._buildJob.Start();
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x00153520 File Offset: 0x00151720
		private bool RegisterTriangle(int triangleIndex)
		{
			Triangle3D triangle = this._editorMesh.GetTriangle(triangleIndex);
			if (!this.IsTriangleValid(triangle))
			{
				return false;
			}
			MeshSphereTreeTriangle data = new MeshSphereTreeTriangle(triangleIndex);
			this._sphereTree.AddTerminalNode(triangle.GetEncapsulatingSphere(), data);
			return true;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x00153560 File Offset: 0x00151760
		private bool IsTriangleValid(Triangle3D triangle)
		{
			return !triangle.IsDegenerate;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x0015356B File Offset: 0x0015176B
		private void OnSilentBuildFinished()
		{
			this._sphereTree.PerformPendingUpdates();
			this._wasBuilt = true;
		}

		// Token: 0x04002066 RID: 8294
		private SphereTree<MeshSphereTreeTriangle> _sphereTree;

		// Token: 0x04002067 RID: 8295
		private EditorMesh _editorMesh;

		// Token: 0x04002068 RID: 8296
		private bool _wasBuilt;

		// Token: 0x04002069 RID: 8297
		private MeshSphereTreeBuildJob _buildJob;
	}
}
