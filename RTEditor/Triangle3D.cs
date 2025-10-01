using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000434 RID: 1076
	public class Triangle3D
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x00150ACD File Offset: 0x0014ECCD
		public Vector3 Point0
		{
			get
			{
				return this._points[0];
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x00150ADB File Offset: 0x0014ECDB
		public Vector3 Point1
		{
			get
			{
				return this._points[1];
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060031A5 RID: 12709 RVA: 0x00150AE9 File Offset: 0x0014ECE9
		public Vector3 Point2
		{
			get
			{
				return this._points[2];
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x00150AF7 File Offset: 0x0014ECF7
		public Vector3 Normal
		{
			get
			{
				return this._plane.normal;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060031A7 RID: 12711 RVA: 0x00150B04 File Offset: 0x0014ED04
		public Plane Plane
		{
			get
			{
				return this._plane;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x00150B0C File Offset: 0x0014ED0C
		public float Area
		{
			get
			{
				return this._area;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x00150B14 File Offset: 0x0014ED14
		public bool IsDegenerate
		{
			get
			{
				return this._area == 0f || float.IsNaN(this._area);
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x00150B30 File Offset: 0x0014ED30
		public Triangle3D(Triangle3D source)
		{
			this._points = new Vector3[3];
			this._points[0] = source.Point0;
			this._points[1] = source.Point1;
			this._points[2] = source.Point2;
			this._plane = source._plane;
			this._area = source._area;
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x00150BAC File Offset: 0x0014EDAC
		public Triangle3D(Vector3 point0, Vector3 point1, Vector3 point2)
		{
			this._points = new Vector3[3];
			this._points[0] = point0;
			this._points[1] = point1;
			this._points[2] = point2;
			this.CalculateAreaAndPlane();
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x00150C04 File Offset: 0x0014EE04
		public void TransformPoints(Matrix4x4 transformMatrix)
		{
			this._points[0] = transformMatrix.MultiplyPoint(this._points[0]);
			this._points[1] = transformMatrix.MultiplyPoint(this._points[1]);
			this._points[2] = transformMatrix.MultiplyPoint(this._points[2]);
			this.CalculateAreaAndPlane();
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x00150C74 File Offset: 0x0014EE74
		public Box GetEncapsulatingBox()
		{
			Vector3 vector;
			Vector3 vector2;
			Vector3Extensions.GetMinMaxPoints(this.GetPoints(), out vector, out vector2);
			return new Box((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x00150CB0 File Offset: 0x0014EEB0
		public List<Segment3D> GetSegments()
		{
			return new List<Segment3D>
			{
				new Segment3D(this.Point0, this.Point1),
				new Segment3D(this.Point1, this.Point2),
				new Segment3D(this.Point2, this.Point0)
			};
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x00150D08 File Offset: 0x0014EF08
		public Plane GetSegmentPlane(int segmentIndex)
		{
			Segment3D segment = this.GetSegment(segmentIndex);
			Vector3 inNormal = Vector3.Cross(segment.Direction, this._plane.normal);
			inNormal.Normalize();
			return new Plane(inNormal, segment.StartPoint);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x00150D49 File Offset: 0x0014EF49
		public Segment3D GetSegment(int segmentIndex)
		{
			return new Segment3D(this._points[segmentIndex], this._points[(segmentIndex + 1) % 3]);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x00150D6C File Offset: 0x0014EF6C
		public bool Raycast(Ray ray, out float t)
		{
			if (this._plane.Raycast(ray, out t))
			{
				Vector3 point = ray.GetPoint(t);
				return this.ContainsPoint(point);
			}
			return false;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x00150D9C File Offset: 0x0014EF9C
		public bool Raycast(Ray3D ray, out float t)
		{
			if (ray.IntersectsPlane(this._plane, out t))
			{
				Vector3 point = ray.GetPoint(t);
				return this.ContainsPoint(point);
			}
			return false;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x00150DCC File Offset: 0x0014EFCC
		public bool ContainsPoint(Vector3 point)
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.GetSegmentPlane(i).IsPointInFront(point))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x00150DF8 File Offset: 0x0014EFF8
		public Sphere3D GetEncapsulatingSphere()
		{
			return this.GetEncapsulatingBox().GetEncapsulatingSphere();
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x00150E13 File Offset: 0x0014F013
		public Vector3 GetCenter()
		{
			return (this.Point0 + this.Point1 + this.Point2) / 3f;
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x00150E3B File Offset: 0x0014F03B
		public List<Vector3> GetPoints()
		{
			return new List<Vector3>
			{
				this.Point0,
				this.Point1,
				this.Point2
			};
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x00150E66 File Offset: 0x0014F066
		public Vector3 GetPointClosestToPoint(Vector3 point)
		{
			return Vector3Extensions.GetPointClosestToPoint(this.GetPoints(), point);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x00150E74 File Offset: 0x0014F074
		private void CalculateAreaAndPlane()
		{
			Vector3 lhs = this.Point1 - this.Point0;
			Vector3 rhs = this.Point2 - this.Point0;
			Vector3 inNormal = Vector3.Cross(lhs, rhs);
			if (inNormal.magnitude < 1E-05f)
			{
				this._area = 0f;
				this._plane = new Plane(Vector3.zero, Vector3.zero);
				return;
			}
			this._area = inNormal.magnitude * 0.5f;
			inNormal.Normalize();
			this._plane = new Plane(inNormal, this.Point0);
		}

		// Token: 0x0400201A RID: 8218
		private Vector3[] _points = new Vector3[3];

		// Token: 0x0400201B RID: 8219
		private Plane _plane;

		// Token: 0x0400201C RID: 8220
		private float _area;
	}
}
