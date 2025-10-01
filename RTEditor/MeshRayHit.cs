using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000457 RID: 1111
	public class MeshRayHit
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x00154908 File Offset: 0x00152B08
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060032CF RID: 13007 RVA: 0x00154910 File Offset: 0x00152B10
		public float HitEnter
		{
			get
			{
				return this._hitEnter;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x00154918 File Offset: 0x00152B18
		public int HitTriangleIndex
		{
			get
			{
				return this._hitTraingleIndex;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x00154920 File Offset: 0x00152B20
		public Vector3 HitPoint
		{
			get
			{
				return this._hitPoint;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x00154928 File Offset: 0x00152B28
		public Vector3 HitNormal
		{
			get
			{
				return this._hitNormal;
			}
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x00154930 File Offset: 0x00152B30
		public MeshRayHit(Ray ray, float hitEnter, int hitTriangleIndex, Vector3 hitPoint, Vector3 hitNormal)
		{
			this._ray = ray;
			this._hitEnter = hitEnter;
			this._hitTraingleIndex = hitTriangleIndex;
			this._hitPoint = hitPoint;
			this._hitNormal = hitNormal;
			this._hitNormal.Normalize();
		}

		// Token: 0x04002094 RID: 8340
		private Ray _ray;

		// Token: 0x04002095 RID: 8341
		private float _hitEnter;

		// Token: 0x04002096 RID: 8342
		private int _hitTraingleIndex;

		// Token: 0x04002097 RID: 8343
		private Vector3 _hitPoint;

		// Token: 0x04002098 RID: 8344
		private Vector3 _hitNormal;
	}
}
