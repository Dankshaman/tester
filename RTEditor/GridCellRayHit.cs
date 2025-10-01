using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000456 RID: 1110
	public class GridCellRayHit
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060032C8 RID: 13000 RVA: 0x0015488B File Offset: 0x00152A8B
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x00154893 File Offset: 0x00152A93
		public float HitEnter
		{
			get
			{
				return this._hitEnter;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060032CA RID: 13002 RVA: 0x0015489B File Offset: 0x00152A9B
		public XZGridCell HitCell
		{
			get
			{
				return this._hitCell;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x001548A3 File Offset: 0x00152AA3
		public Vector3 HitPoint
		{
			get
			{
				return this._hitPoint;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x001548AB File Offset: 0x00152AAB
		public Vector3 HitNormal
		{
			get
			{
				return this._hitNormal;
			}
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x001548B4 File Offset: 0x00152AB4
		public GridCellRayHit(Ray ray, float hitEnter, XZGridCell hitCell)
		{
			this._ray = ray;
			this._hitEnter = hitEnter;
			this._hitCell = hitCell;
			this._hitPoint = ray.GetPoint(hitEnter);
			this._hitNormal = this._hitCell.ParentGrid.Plane.normal;
		}

		// Token: 0x0400208F RID: 8335
		private Ray _ray;

		// Token: 0x04002090 RID: 8336
		private float _hitEnter;

		// Token: 0x04002091 RID: 8337
		private XZGridCell _hitCell;

		// Token: 0x04002092 RID: 8338
		private Vector3 _hitPoint;

		// Token: 0x04002093 RID: 8339
		private Vector3 _hitNormal;
	}
}
