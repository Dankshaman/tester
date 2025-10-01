using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000451 RID: 1105
	public struct SphereTreeNodeRayHit<T>
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x00153F9B File Offset: 0x0015219B
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x00153FA3 File Offset: 0x001521A3
		public SphereTreeNode<T> HitNode
		{
			get
			{
				return this._hitNode;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x00153FAB File Offset: 0x001521AB
		public Vector3 HitPoint
		{
			get
			{
				return this._hitPoint;
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x00153FB3 File Offset: 0x001521B3
		public SphereTreeNodeRayHit(Ray ray, float t, SphereTreeNode<T> hitNode)
		{
			this._ray = ray;
			this._hitNode = hitNode;
			this._hitPoint = this._ray.GetPoint(t);
		}

		// Token: 0x0400207C RID: 8316
		private Ray _ray;

		// Token: 0x0400207D RID: 8317
		private SphereTreeNode<T> _hitNode;

		// Token: 0x0400207E RID: 8318
		private Vector3 _hitPoint;
	}
}
