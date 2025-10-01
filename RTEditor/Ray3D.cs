using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000431 RID: 1073
	public struct Ray3D
	{
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x00150637 File Offset: 0x0014E837
		// (set) Token: 0x0600317F RID: 12671 RVA: 0x0015063F File Offset: 0x0014E83F
		public Vector3 Origin
		{
			get
			{
				return this._origin;
			}
			set
			{
				this._origin = value;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x00150648 File Offset: 0x0014E848
		// (set) Token: 0x06003181 RID: 12673 RVA: 0x00150650 File Offset: 0x0014E850
		public Vector3 Direction
		{
			get
			{
				return this._direction;
			}
			set
			{
				this._direction = value;
				this._normalizedDirection = this._direction;
				this._normalizedDirection.Normalize();
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x00150670 File Offset: 0x0014E870
		public Vector3 NormalizedDirection
		{
			get
			{
				return this._normalizedDirection;
			}
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x00150678 File Offset: 0x0014E878
		public Ray3D(Vector3 origin, Vector3 direction)
		{
			this._origin = origin;
			this._direction = direction;
			this._normalizedDirection = direction;
			this._normalizedDirection.Normalize();
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x0015069A File Offset: 0x0014E89A
		public Ray3D(Ray source)
		{
			this._origin = source.origin;
			this._direction = source.direction;
			this._normalizedDirection = this._direction;
			this._normalizedDirection.Normalize();
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x001506CD File Offset: 0x0014E8CD
		public Ray ToRayWithNormalizedDirection()
		{
			return new Ray(this._origin, this._direction);
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x001506E0 File Offset: 0x0014E8E0
		public void Transform(Matrix4x4 transformMatrix)
		{
			this._origin = transformMatrix.MultiplyPoint(this._origin);
			this.Direction = transformMatrix.MultiplyVector(this._direction);
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x00150708 File Offset: 0x0014E908
		public void InverseTransform(Matrix4x4 transformMatrix)
		{
			this.Transform(transformMatrix.inverse);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x00150717 File Offset: 0x0014E917
		public Vector3 GetPoint(float t)
		{
			return this._origin + this._direction * t;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x00150730 File Offset: 0x0014E930
		public bool IntersectsPlane(Plane plane, out float t)
		{
			t = 0f;
			float distanceToPoint = plane.GetDistanceToPoint(this._origin);
			float num = Vector3.Dot(plane.normal, this._direction);
			if (Mathf.Abs(num) < 1E-05f)
			{
				return false;
			}
			t = -(distanceToPoint / num);
			return t >= 0f && t <= 1f;
		}

		// Token: 0x04002010 RID: 8208
		private Vector3 _origin;

		// Token: 0x04002011 RID: 8209
		private Vector3 _direction;

		// Token: 0x04002012 RID: 8210
		private Vector3 _normalizedDirection;
	}
}
