using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200042E RID: 1070
	public class OrientedBoxRayHit
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x00150460 File Offset: 0x0014E660
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x00150468 File Offset: 0x0014E668
		public float HitEnter
		{
			get
			{
				return this._hitEnter;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x00150470 File Offset: 0x0014E670
		public OrientedBox HitBox
		{
			get
			{
				return new OrientedBox(this._hitBox);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x0015047D File Offset: 0x0014E67D
		public Vector3 HitPoint
		{
			get
			{
				return this._hitPoint;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x00150485 File Offset: 0x0014E685
		public Vector3 HitNormal
		{
			get
			{
				return this._hitNormal;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x0015048D File Offset: 0x0014E68D
		public BoxFace HitFace
		{
			get
			{
				return this._hitFace;
			}
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x00150498 File Offset: 0x0014E698
		public OrientedBoxRayHit(Ray ray, float hitEnter, OrientedBox hitBox)
		{
			this._ray = ray;
			this._hitEnter = hitEnter;
			this._hitBox = new OrientedBox(hitBox);
			this._hitPoint = ray.GetPoint(hitEnter);
			this._hitFace = hitBox.GetBoxFaceClosestToPoint(this._hitPoint);
			this._hitNormal = hitBox.GetBoxFacePlane(this._hitFace).normal;
		}

		// Token: 0x04002008 RID: 8200
		private Ray _ray;

		// Token: 0x04002009 RID: 8201
		private float _hitEnter;

		// Token: 0x0400200A RID: 8202
		private OrientedBox _hitBox;

		// Token: 0x0400200B RID: 8203
		private Vector3 _hitPoint;

		// Token: 0x0400200C RID: 8204
		private Vector3 _hitNormal;

		// Token: 0x0400200D RID: 8205
		private BoxFace _hitFace;
	}
}
