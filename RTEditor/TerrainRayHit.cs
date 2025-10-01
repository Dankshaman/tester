using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200045A RID: 1114
	public class TerrainRayHit
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x00154AE4 File Offset: 0x00152CE4
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x00154AEC File Offset: 0x00152CEC
		public float HitEnter
		{
			get
			{
				return this._hitEnter;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x00154AF4 File Offset: 0x00152CF4
		public Vector3 HitPoint
		{
			get
			{
				return this._hitPoint;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x00154AFC File Offset: 0x00152CFC
		public Vector3 HitNormal
		{
			get
			{
				return this._hitNormal;
			}
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x00154B04 File Offset: 0x00152D04
		public TerrainRayHit(Ray ray, RaycastHit raycastHit)
		{
			this._ray = ray;
			this._hitEnter = raycastHit.distance;
			this._hitPoint = raycastHit.point;
			this._hitNormal = raycastHit.normal;
		}

		// Token: 0x040020A0 RID: 8352
		private Ray _ray;

		// Token: 0x040020A1 RID: 8353
		private float _hitEnter;

		// Token: 0x040020A2 RID: 8354
		private Vector3 _hitPoint;

		// Token: 0x040020A3 RID: 8355
		private Vector3 _hitNormal;
	}
}
