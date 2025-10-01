using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000445 RID: 1093
	public class MeshPool : SingletonBase<MeshPool>
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x0015224C File Offset: 0x0015044C
		public Mesh ConeMesh
		{
			get
			{
				if (this._coneMesh == null)
				{
					this._coneMesh = ProceduralMeshGenerator.CreateConeMesh(1f, 1f, 30, 30, 5);
				}
				return this._coneMesh;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x0015227C File Offset: 0x0015047C
		public Mesh XYSquareMesh
		{
			get
			{
				if (this._XYSquareMesh == null)
				{
					this._XYSquareMesh = ProceduralMeshGenerator.CreatePlaneMesh(1f, 1f);
				}
				return this._XYSquareMesh;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x001522A7 File Offset: 0x001504A7
		public Mesh SphereMesh
		{
			get
			{
				if (this._sphereMesh == null)
				{
					this._sphereMesh = ProceduralMeshGenerator.CreateSphereMesh(1f, 50, 50);
				}
				return this._sphereMesh;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x001522D1 File Offset: 0x001504D1
		public Mesh BoxMesh
		{
			get
			{
				if (this._boxMesh == null)
				{
					this._boxMesh = ProceduralMeshGenerator.CreateBoxMesh(1f, 1f, 1f);
				}
				return this._boxMesh;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x00152301 File Offset: 0x00150501
		public Mesh RightAngledTriangleMesh
		{
			get
			{
				if (this._rightAngledTriangleMesh == null)
				{
					this._rightAngledTriangleMesh = ProceduralMeshGenerator.CreateRightAngledTriangleMesh(1f, 1f);
				}
				return this._rightAngledTriangleMesh;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x0015232C File Offset: 0x0015052C
		public Mesh XGridLineMesh
		{
			get
			{
				if (this._xzGridLineMesh == null)
				{
					this._xzGridLineMesh = ProceduralMeshGenerator.CreateXZGridLineMesh(1f, 1f, 150, 150);
				}
				return this._xzGridLineMesh;
			}
		}

		// Token: 0x04002044 RID: 8260
		private Mesh _coneMesh;

		// Token: 0x04002045 RID: 8261
		private Mesh _XYSquareMesh;

		// Token: 0x04002046 RID: 8262
		private Mesh _sphereMesh;

		// Token: 0x04002047 RID: 8263
		private Mesh _boxMesh;

		// Token: 0x04002048 RID: 8264
		private Mesh _rightAngledTriangleMesh;

		// Token: 0x04002049 RID: 8265
		private Mesh _xzGridLineMesh;
	}
}
