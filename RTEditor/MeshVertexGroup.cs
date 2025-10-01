using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000410 RID: 1040
	public class MeshVertexGroup
	{
		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x0014A622 File Offset: 0x00148822
		public Bounds GroupAABB
		{
			get
			{
				return this._groupAABB;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x0014A62A File Offset: 0x0014882A
		public List<Vector3> ModelSpaceVertices
		{
			get
			{
				return new List<Vector3>(this._modelSpaceVertices);
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06003053 RID: 12371 RVA: 0x0014A637 File Offset: 0x00148837
		public bool IsClosed
		{
			get
			{
				return this._isClosed;
			}
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x0014A63F File Offset: 0x0014883F
		public void AddVertex(Vector3 vertex)
		{
			if (this._isClosed)
			{
				return;
			}
			this._modelSpaceVertices.Add(vertex);
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x0014A656 File Offset: 0x00148856
		public void Close()
		{
			this._isClosed = true;
			this.CalculateGroupAABB();
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x0014A668 File Offset: 0x00148868
		private void CalculateGroupAABB()
		{
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			foreach (Vector3 rhs in this._modelSpaceVertices)
			{
				vector = Vector3.Min(vector, rhs);
				vector2 = Vector3.Max(vector2, rhs);
			}
			this._groupAABB = default(Bounds);
			this._groupAABB.SetMinMax(vector, vector2);
			if (this._groupAABB.size.magnitude < 1E-05f)
			{
				this._groupAABB.size = Vector3.one * 0.3f;
			}
		}

		// Token: 0x04001F92 RID: 8082
		private List<Vector3> _modelSpaceVertices = new List<Vector3>();

		// Token: 0x04001F93 RID: 8083
		private Bounds _groupAABB;

		// Token: 0x04001F94 RID: 8084
		private bool _isClosed;
	}
}
