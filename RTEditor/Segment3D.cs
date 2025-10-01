using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000432 RID: 1074
	public struct Segment3D
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x0015078F File Offset: 0x0014E98F
		public Vector3 StartPoint
		{
			get
			{
				return this._startPoint;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x00150797 File Offset: 0x0014E997
		public Vector3 EndPoint
		{
			get
			{
				return this._startPoint + this._normalizedDirection * this._length;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600318C RID: 12684 RVA: 0x001507B5 File Offset: 0x0014E9B5
		public Vector3 NormalizedDirection
		{
			get
			{
				return this._normalizedDirection;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x001507BD File Offset: 0x0014E9BD
		public Vector3 Direction
		{
			get
			{
				return this._direction;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x001507C5 File Offset: 0x0014E9C5
		public float Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x001507CD File Offset: 0x0014E9CD
		public float SqrLength
		{
			get
			{
				return this._sqrLength;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x001507D5 File Offset: 0x0014E9D5
		public float HalfLength
		{
			get
			{
				return this._length * 0.5f;
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x001507E4 File Offset: 0x0014E9E4
		public Segment3D(Vector3 startPoint, Vector3 endPoint)
		{
			this._startPoint = startPoint;
			this._direction = endPoint - this._startPoint;
			this._length = this._direction.magnitude;
			this._sqrLength = this._direction.sqrMagnitude;
			this._normalizedDirection = this._direction;
			this._normalizedDirection.Normalize();
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x00150843 File Offset: 0x0014EA43
		public Vector3 GetPoint(float t)
		{
			return this._startPoint + t * this._direction;
		}

		// Token: 0x04002013 RID: 8211
		private Vector3 _startPoint;

		// Token: 0x04002014 RID: 8212
		private Vector3 _normalizedDirection;

		// Token: 0x04002015 RID: 8213
		private Vector3 _direction;

		// Token: 0x04002016 RID: 8214
		private float _length;

		// Token: 0x04002017 RID: 8215
		private float _sqrLength;
	}
}
