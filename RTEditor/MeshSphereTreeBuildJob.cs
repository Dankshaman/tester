using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x020003B5 RID: 949
	public class MeshSphereTreeBuildJob : SilentJob
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06002CDE RID: 11486 RVA: 0x0013A148 File Offset: 0x00138348
		// (remove) Token: 0x06002CDF RID: 11487 RVA: 0x0013A180 File Offset: 0x00138380
		public event MeshSphereTreeBuildJob.SilentBuildFinishedHandler SilentBuildFinished;

		// Token: 0x06002CE0 RID: 11488 RVA: 0x0013A1B5 File Offset: 0x001383B5
		public MeshSphereTreeBuildJob(List<Triangle3D> meshTriangles, SphereTree<MeshSphereTreeTriangle> sphereTree)
		{
			this._meshTriangles = new List<Triangle3D>(meshTriangles);
			this._sphereTree = sphereTree;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x0013A1D0 File Offset: 0x001383D0
		protected override void DoJob()
		{
			for (int i = 0; i < this._meshTriangles.Count; i++)
			{
				Triangle3D triangle3D = this._meshTriangles[i];
				if (this.ValidateTriangle == null || this.ValidateTriangle(triangle3D))
				{
					MeshSphereTreeTriangle data = new MeshSphereTreeTriangle(i);
					this._sphereTree.AddTerminalNode(triangle3D.GetEncapsulatingSphere(), data);
				}
			}
			if (this.SilentBuildFinished != null)
			{
				this.SilentBuildFinished();
			}
		}

		// Token: 0x04001E18 RID: 7704
		private List<Triangle3D> _meshTriangles;

		// Token: 0x04001E19 RID: 7705
		private SphereTree<MeshSphereTreeTriangle> _sphereTree;

		// Token: 0x04001E1A RID: 7706
		public MeshSphereTreeBuildJob.TriangleValidation ValidateTriangle;

		// Token: 0x020007ED RID: 2029
		// (Invoke) Token: 0x0600405A RID: 16474
		public delegate bool TriangleValidation(Triangle3D triangle);

		// Token: 0x020007EE RID: 2030
		// (Invoke) Token: 0x0600405E RID: 16478
		public delegate void SilentBuildFinishedHandler();
	}
}
