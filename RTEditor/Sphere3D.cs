using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000433 RID: 1075
	public struct Sphere3D
	{
		// Token: 0x06003193 RID: 12691 RVA: 0x0015085C File Offset: 0x0014EA5C
		public Sphere3D(Vector3 center)
		{
			this._radius = 1f;
			this._center = center;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x00150870 File Offset: 0x0014EA70
		public Sphere3D(float radius)
		{
			this._radius = radius;
			this._center = Vector3.zero;
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x00150884 File Offset: 0x0014EA84
		public Sphere3D(Vector3 center, float radius)
		{
			this._radius = radius;
			this._center = center;
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x00150894 File Offset: 0x0014EA94
		public Sphere3D(Sphere3D source)
		{
			this._radius = source._radius;
			this._center = source._center;
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x001508AE File Offset: 0x0014EAAE
		// (set) Token: 0x06003198 RID: 12696 RVA: 0x001508B6 File Offset: 0x0014EAB6
		public float Radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this._radius = value;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x001508BF File Offset: 0x0014EABF
		// (set) Token: 0x0600319A RID: 12698 RVA: 0x001508C7 File Offset: 0x0014EAC7
		public Vector3 Center
		{
			get
			{
				return this._center;
			}
			set
			{
				this._center = value;
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x001508D0 File Offset: 0x0014EAD0
		public bool Raycast(Ray ray)
		{
			float num;
			return this.Raycast(ray, out num);
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x001508E8 File Offset: 0x0014EAE8
		public bool Raycast(Ray ray, out float t)
		{
			t = 0f;
			Vector3 vector = ray.origin - this._center;
			float a = Vector3.SqrMagnitude(ray.direction);
			float b = 2f * Vector3.Dot(ray.direction, vector);
			float c = Vector3.SqrMagnitude(vector) - this._radius * this._radius;
			float num;
			float num2;
			if (!Equation.SolveQuadratic(a, b, c, out num, out num2))
			{
				return false;
			}
			if (num < 0f && num2 < 0f)
			{
				return false;
			}
			if (num < 0f)
			{
				float num3 = num;
				num = num2;
				num2 = num3;
			}
			t = num;
			return true;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x00150978 File Offset: 0x0014EB78
		public float GetDistanceBetweenCenters(Sphere3D sphere)
		{
			return (this._center - sphere.Center).magnitude;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x001509A0 File Offset: 0x0014EBA0
		public float GetDistanceBetweenCentersSq(Sphere3D sphere)
		{
			return (this.Center - sphere.Center).sqrMagnitude;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x001509C7 File Offset: 0x0014EBC7
		public bool FullyOverlaps(Sphere3D sphere)
		{
			return this.GetDistanceBetweenCenters(sphere) + sphere.Radius <= this._radius;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x001509E4 File Offset: 0x0014EBE4
		public bool OverlapsFullyOrPartially(Sphere3D sphere)
		{
			float distanceBetweenCenters = this.GetDistanceBetweenCenters(sphere);
			return distanceBetweenCenters + sphere.Radius <= this._radius || distanceBetweenCenters - sphere.Radius <= this._radius;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x00150A20 File Offset: 0x0014EC20
		public bool OverlapsFullyOrPartially(OrientedBox orientedBox)
		{
			return (orientedBox.GetClosestPointToPoint(this._center) - this._center).sqrMagnitude <= this._radius * this._radius;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x00150A60 File Offset: 0x0014EC60
		public Sphere3D Encapsulate(Sphere3D sphere)
		{
			float num = (this.GetDistanceBetweenCenters(sphere) + this._radius + sphere.Radius) * 0.5f;
			Vector3 a = sphere.Center - this.Center;
			a.Normalize();
			return new Sphere3D(this.Center - a * this._radius + a * num, num);
		}

		// Token: 0x04002018 RID: 8216
		private float _radius;

		// Token: 0x04002019 RID: 8217
		private Vector3 _center;
	}
}
