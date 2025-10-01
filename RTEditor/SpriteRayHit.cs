using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000459 RID: 1113
	public class SpriteRayHit
	{
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060032DC RID: 13020 RVA: 0x00154A84 File Offset: 0x00152C84
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x00154A8C File Offset: 0x00152C8C
		public float HitEnter
		{
			get
			{
				return this._hitEnter;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x00154A94 File Offset: 0x00152C94
		public SpriteRenderer HitSpriteRenderer
		{
			get
			{
				return this._hitSpriteRenderer;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x00154A9C File Offset: 0x00152C9C
		public Vector3 HitPoint
		{
			get
			{
				return this._hitPoint;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x00154AA4 File Offset: 0x00152CA4
		public Vector3 HitNormal
		{
			get
			{
				return this._hitNormal;
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x00154AAC File Offset: 0x00152CAC
		public SpriteRayHit(Ray ray, float hitEnter, SpriteRenderer hitSpriteRenderer, Vector3 hitPoint, Vector3 hitNormal)
		{
			this._ray = ray;
			this._hitEnter = hitEnter;
			this._hitSpriteRenderer = hitSpriteRenderer;
			this._hitPoint = hitPoint;
			this._hitNormal = hitNormal;
			this._hitNormal.Normalize();
		}

		// Token: 0x0400209B RID: 8347
		private Ray _ray;

		// Token: 0x0400209C RID: 8348
		private float _hitEnter;

		// Token: 0x0400209D RID: 8349
		private SpriteRenderer _hitSpriteRenderer;

		// Token: 0x0400209E RID: 8350
		private Vector3 _hitPoint;

		// Token: 0x0400209F RID: 8351
		private Vector3 _hitNormal;
	}
}
