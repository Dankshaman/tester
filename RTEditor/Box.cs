using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000422 RID: 1058
	public struct Box
	{
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x0014DEC8 File Offset: 0x0014C0C8
		// (set) Token: 0x060030FB RID: 12539 RVA: 0x0014DED0 File Offset: 0x0014C0D0
		public Vector3 Min
		{
			get
			{
				return this._min;
			}
			set
			{
				this._min = value;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x0014DED9 File Offset: 0x0014C0D9
		// (set) Token: 0x060030FD RID: 12541 RVA: 0x0014DEE1 File Offset: 0x0014C0E1
		public Vector3 Max
		{
			get
			{
				return this._max;
			}
			set
			{
				this._max = value;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x0014DEEA File Offset: 0x0014C0EA
		public Vector3 Extents
		{
			get
			{
				return this.Size * 0.5f;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x0014DEFC File Offset: 0x0014C0FC
		// (set) Token: 0x06003100 RID: 12544 RVA: 0x0014DF10 File Offset: 0x0014C110
		public Vector3 Size
		{
			get
			{
				return this._max - this._min;
			}
			set
			{
				Vector3 center = this.Center;
				Vector3 vectorWithAbsComponents = (value * 0.5f).GetVectorWithAbsComponents();
				this._max = center + vectorWithAbsComponents;
				this._min = center - vectorWithAbsComponents;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x0014DF4F File Offset: 0x0014C14F
		// (set) Token: 0x06003102 RID: 12546 RVA: 0x0014DF6C File Offset: 0x0014C16C
		public Vector3 Center
		{
			get
			{
				return (this._min + this._max) * 0.5f;
			}
			set
			{
				Vector3 extents = this.Extents;
				this._max = value + extents;
				this._min = value - extents;
			}
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x0014DF9A File Offset: 0x0014C19A
		public Box(Bounds bounds)
		{
			this._min = bounds.min;
			this._max = bounds.max;
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x0014DFB6 File Offset: 0x0014C1B6
		public Box(Vector3 center, Vector3 size)
		{
			this._min = Vector3.zero;
			this._max = Vector3.zero;
			this.Size = size;
			this.Center = center;
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x0014DFDC File Offset: 0x0014C1DC
		public Box(Box source)
		{
			this._min = source.Min;
			this._max = source.Max;
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x0014DFF8 File Offset: 0x0014C1F8
		public static Box GetInvalid()
		{
			Box result = default(Box);
			result.MakeInvalid();
			return result;
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x0014E018 File Offset: 0x0014C218
		public static Box FromPoints(List<Vector3> points, float sizeScale = 1f)
		{
			if (points.Count == 0)
			{
				return Box.GetInvalid();
			}
			Vector3 vector = points[0];
			Vector3 vector2 = points[0];
			for (int i = 1; i < points.Count; i++)
			{
				Vector3 rhs = points[i];
				vector = Vector3.Min(vector, rhs);
				vector2 = Vector3.Max(vector2, rhs);
			}
			Vector3 center = (vector + vector2) * 0.5f;
			Vector3 size = (vector2 - vector) * sizeScale;
			return new Box(center, size);
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x0014E094 File Offset: 0x0014C294
		public static Box FromBounds(Bounds bounds)
		{
			return new Box(bounds);
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x0014E09C File Offset: 0x0014C29C
		public Bounds ToBounds()
		{
			return new Bounds(this.Center, this.Size);
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x0014E0AF File Offset: 0x0014C2AF
		public OrientedBox ToOrientedBox()
		{
			return new OrientedBox(new Box(Vector3.zero, this.Size), Quaternion.identity)
			{
				Center = this.Center
			};
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x0014E0D8 File Offset: 0x0014C2D8
		public Sphere3D GetEncapsulatingSphere()
		{
			return new Sphere3D(this.Center, this.Extents.magnitude);
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x0014E0FE File Offset: 0x0014C2FE
		public void Encapsulate(Bounds bounds)
		{
			this.Encapsulate(new Box(bounds));
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x0014E10C File Offset: 0x0014C30C
		public void Encapsulate(Box box)
		{
			this.AddPoint(box.Min);
			this.AddPoint(box.Max);
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x0014E128 File Offset: 0x0014C328
		public void AddPoint(Vector3 point)
		{
			if (point.x < this._min.x)
			{
				this._min.x = point.x;
			}
			if (point.y < this._min.y)
			{
				this._min.y = point.y;
			}
			if (point.z < this._min.z)
			{
				this._min.z = point.z;
			}
			if (point.x > this._max.x)
			{
				this._max.x = point.x;
			}
			if (point.y > this._max.y)
			{
				this._max.y = point.y;
			}
			if (point.z > this._max.z)
			{
				this._max.z = point.z;
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x0014E210 File Offset: 0x0014C410
		public bool IntersectsBox(Box box, bool allowFacesToTouch = false, float intersectionEpsilon = 1E-05f)
		{
			Vector3 center = this.Center;
			Vector3 extents = this.Extents;
			Vector3 extents2 = box.Extents;
			Vector3 center2 = box.Center;
			float num = Mathf.Abs(center.x - center2.x);
			float num2 = Mathf.Abs(center.y - center2.y);
			float num3 = Mathf.Abs(center.z - center2.z);
			float num4 = extents.x + extents2.x;
			float num5 = extents.y + extents2.y;
			float num6 = extents.z + extents2.z;
			if (!allowFacesToTouch)
			{
				if (num >= num4)
				{
					return false;
				}
				if (num2 >= num5)
				{
					return false;
				}
				if (num3 >= num6)
				{
					return false;
				}
			}
			else
			{
				if (num + intersectionEpsilon > num4)
				{
					return false;
				}
				if (num2 + intersectionEpsilon > num5)
				{
					return false;
				}
				if (num3 + intersectionEpsilon > num6)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x0014E2DC File Offset: 0x0014C4DC
		public bool TouchesFacesWith(Box box)
		{
			Vector3 extents = this.Extents;
			Vector3 extents2 = box.Extents;
			Vector3 center = this.Center;
			Vector3 center2 = box.Center;
			float num = Mathf.Abs(center.x - center2.x);
			float num2 = Mathf.Abs(center.y - center2.y);
			float num3 = Mathf.Abs(center.z - center2.z);
			float num4 = extents.x + extents2.x;
			float num5 = extents.y + extents2.y;
			float num6 = extents.z + extents2.z;
			if (Mathf.Abs(num - num4) < 0.0001f)
			{
				return num2 <= num5 && Mathf.Abs(num2 - num5) >= 0.0001f && num3 <= num6 && Mathf.Abs(num3 - num6) >= 0.0001f;
			}
			if (num > num4)
			{
				return false;
			}
			if (Mathf.Abs(num2 - num5) < 0.0001f)
			{
				return num <= num4 && Mathf.Abs(num - num4) >= 0.0001f && num3 <= num6 && Mathf.Abs(num3 - num6) >= 0.0001f;
			}
			return num2 <= num5 && Mathf.Abs(num3 - num6) < 0.0001f && num <= num4 && Mathf.Abs(num - num4) >= 0.0001f && num2 <= num5 && Mathf.Abs(num2 - num5) >= 0.0001f;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x0014E444 File Offset: 0x0014C644
		public bool ContainsPoint(Vector3 point)
		{
			return point.x >= this._min.x && point.x <= this._max.x && point.y >= this._min.y && point.y <= this._max.y && point.z >= this._min.z && point.z <= this._max.z;
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x0014E4C8 File Offset: 0x0014C6C8
		public List<Vector3> GetCenterAndCornerPoints()
		{
			Vector3[] array = new Vector3[BoxPoints.Count];
			array[0] = this.GetBoxPoint(BoxPoint.Center);
			array[1] = this.GetBoxPoint(BoxPoint.FrontTopLeft);
			array[2] = this.GetBoxPoint(BoxPoint.FrontTopRight);
			array[3] = this.GetBoxPoint(BoxPoint.FrontBottomRight);
			array[4] = this.GetBoxPoint(BoxPoint.FrontBottomLeft);
			array[5] = this.GetBoxPoint(BoxPoint.BackTopLeft);
			array[6] = this.GetBoxPoint(BoxPoint.BackTopRight);
			array[7] = this.GetBoxPoint(BoxPoint.BackBottomRight);
			array[8] = this.GetBoxPoint(BoxPoint.BackBottomLeft);
			return new List<Vector3>(array);
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x0014E564 File Offset: 0x0014C764
		public List<Vector3> GetCornerPoints()
		{
			Vector3[] array = new Vector3[BoxCornerPoints.Count];
			array[0] = this.GetBoxPoint(BoxPoint.FrontTopLeft);
			array[1] = this.GetBoxPoint(BoxPoint.FrontTopRight);
			array[2] = this.GetBoxPoint(BoxPoint.FrontBottomRight);
			array[3] = this.GetBoxPoint(BoxPoint.FrontBottomLeft);
			array[4] = this.GetBoxPoint(BoxPoint.BackTopLeft);
			array[5] = this.GetBoxPoint(BoxPoint.BackTopRight);
			array[6] = this.GetBoxPoint(BoxPoint.BackBottomRight);
			array[7] = this.GetBoxPoint(BoxPoint.BackBottomLeft);
			return new List<Vector3>(array);
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x0014E5F0 File Offset: 0x0014C7F0
		public Vector3 GetBoxPoint(BoxPoint boxPoint)
		{
			Vector3 center = this.Center;
			Vector3 extents = this.Extents;
			switch (boxPoint)
			{
			case BoxPoint.Center:
				return center;
			case BoxPoint.FrontTopLeft:
				return center - BoxFaces.GetFaceRightAxis(BoxFace.Front) * extents.x + BoxFaces.GetFaceLookAxis(BoxFace.Front) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Front) * extents.z;
			case BoxPoint.FrontTopRight:
				return center + BoxFaces.GetFaceRightAxis(BoxFace.Front) * extents.x + BoxFaces.GetFaceLookAxis(BoxFace.Front) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Front) * extents.z;
			case BoxPoint.FrontBottomRight:
				return center + BoxFaces.GetFaceRightAxis(BoxFace.Front) * extents.x - BoxFaces.GetFaceLookAxis(BoxFace.Front) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Front) * extents.z;
			case BoxPoint.FrontBottomLeft:
				return center - BoxFaces.GetFaceRightAxis(BoxFace.Front) * extents.x - BoxFaces.GetFaceLookAxis(BoxFace.Front) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Front) * extents.z;
			case BoxPoint.BackTopLeft:
				return center - BoxFaces.GetFaceRightAxis(BoxFace.Back) * extents.x + BoxFaces.GetFaceLookAxis(BoxFace.Back) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Back) * extents.z;
			case BoxPoint.BackTopRight:
				return center + BoxFaces.GetFaceRightAxis(BoxFace.Back) * extents.x + BoxFaces.GetFaceLookAxis(BoxFace.Back) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Back) * extents.z;
			case BoxPoint.BackBottomRight:
				return center + BoxFaces.GetFaceRightAxis(BoxFace.Back) * extents.x - BoxFaces.GetFaceLookAxis(BoxFace.Back) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Back) * extents.z;
			case BoxPoint.BackBottomLeft:
				return center - BoxFaces.GetFaceRightAxis(BoxFace.Back) * extents.x - BoxFaces.GetFaceLookAxis(BoxFace.Back) * extents.y + BoxFaces.GetFaceNormal(BoxFace.Back) * extents.z;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x0014E864 File Offset: 0x0014CA64
		public BoxFace GetBoxFaceClosestToPoint(Vector3 point)
		{
			List<Plane> boxFacePlanes = this.GetBoxFacePlanes();
			float num = float.MaxValue;
			BoxFace result = BoxFace.Back;
			for (int i = 0; i < boxFacePlanes.Count; i++)
			{
				float num2 = Mathf.Abs(boxFacePlanes[i].GetDistanceToPoint(point));
				if (num2 < num)
				{
					num = num2;
					result = (BoxFace)i;
				}
			}
			return result;
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x0014E8B4 File Offset: 0x0014CAB4
		public List<Plane> GetBoxFacePlanes()
		{
			Plane[] array = new Plane[Enum.GetValues(typeof(BoxFace)).Length];
			array[1] = this.GetBoxFacePlane(BoxFace.Back);
			array[0] = this.GetBoxFacePlane(BoxFace.Front);
			array[4] = this.GetBoxFacePlane(BoxFace.Left);
			array[5] = this.GetBoxFacePlane(BoxFace.Right);
			array[2] = this.GetBoxFacePlane(BoxFace.Top);
			array[3] = this.GetBoxFacePlane(BoxFace.Bottom);
			return new List<Plane>(array);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x0014E934 File Offset: 0x0014CB34
		public List<Vector3> GetBoxFaceCenterAndCornerPoints(BoxFace boxFace)
		{
			Vector3[] array = new Vector3[BoxFacePoints.Count];
			Vector3 boxFaceCenter = this.GetBoxFaceCenter(boxFace);
			Vector2 vector = this.GetBoxFaceSizeAlongFaceLocalXZAxes(boxFace, Vector3.one) * 0.5f;
			array[0] = boxFaceCenter;
			array[1] = boxFaceCenter - BoxFaces.GetFaceRightAxis(boxFace) * vector.x + BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			array[2] = boxFaceCenter + BoxFaces.GetFaceRightAxis(boxFace) * vector.x + BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			array[3] = boxFaceCenter + BoxFaces.GetFaceRightAxis(boxFace) * vector.x - BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			array[4] = boxFaceCenter - BoxFaces.GetFaceRightAxis(boxFace) * vector.x - BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			return new List<Vector3>(array);
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x0014EA48 File Offset: 0x0014CC48
		public List<Vector3> GetBoxFaceCornerPoints(BoxFace boxFace)
		{
			Vector3[] array = new Vector3[BoxFaceCornerPoints.Count];
			Vector3 boxFaceCenter = this.GetBoxFaceCenter(boxFace);
			Vector2 vector = this.GetBoxFaceSizeAlongFaceLocalXZAxes(boxFace, Vector3.one) * 0.5f;
			array[0] = boxFaceCenter - BoxFaces.GetFaceRightAxis(boxFace) * vector.x + BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			array[1] = boxFaceCenter + BoxFaces.GetFaceRightAxis(boxFace) * vector.x + BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			array[2] = boxFaceCenter + BoxFaces.GetFaceRightAxis(boxFace) * vector.x - BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			array[3] = boxFaceCenter - BoxFaces.GetFaceRightAxis(boxFace) * vector.x - BoxFaces.GetFaceLookAxis(boxFace) * vector.y;
			return new List<Vector3>(array);
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x0014EB54 File Offset: 0x0014CD54
		public Plane GetBoxFacePlane(BoxFace boxFace)
		{
			switch (boxFace)
			{
			case BoxFace.Front:
				return new Plane(BoxFaces.GetFaceNormal(BoxFace.Front), this.GetBoxPoint(BoxPoint.FrontBottomLeft));
			case BoxFace.Back:
				return new Plane(BoxFaces.GetFaceNormal(BoxFace.Back), this.GetBoxPoint(BoxPoint.BackBottomLeft));
			case BoxFace.Top:
				return new Plane(BoxFaces.GetFaceNormal(BoxFace.Top), this.GetBoxPoint(BoxPoint.FrontTopLeft));
			case BoxFace.Bottom:
				return new Plane(BoxFaces.GetFaceNormal(BoxFace.Bottom), this.GetBoxPoint(BoxPoint.FrontBottomLeft));
			case BoxFace.Left:
				return new Plane(BoxFaces.GetFaceNormal(BoxFace.Left), this.GetBoxPoint(BoxPoint.FrontBottomLeft));
			case BoxFace.Right:
				return new Plane(BoxFaces.GetFaceNormal(BoxFace.Right), this.GetBoxPoint(BoxPoint.FrontBottomRight));
			default:
				return default(Plane);
			}
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x0014EBFC File Offset: 0x0014CDFC
		public Vector2 GetBoxFaceSizeAlongFaceLocalXZAxes(BoxFace boxFace, Vector3 boxXYZScale)
		{
			Vector3 size = this.Size;
			switch (boxFace)
			{
			case BoxFace.Front:
			case BoxFace.Back:
				return new Vector2(size.x * boxXYZScale.x, size.y * boxXYZScale.y);
			case BoxFace.Top:
			case BoxFace.Bottom:
				return new Vector2(size.x * boxXYZScale.x, size.z * boxXYZScale.z);
			case BoxFace.Left:
			case BoxFace.Right:
				return new Vector2(size.z * boxXYZScale.z, size.y * boxXYZScale.y);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x0014EC98 File Offset: 0x0014CE98
		public Vector3 GetBoxFaceCenter(BoxFace boxFace)
		{
			Vector3 center = this.Center;
			Vector3 extents = this.Extents;
			switch (boxFace)
			{
			case BoxFace.Front:
				return center + BoxFaces.GetFaceNormal(BoxFace.Front) * extents.z;
			case BoxFace.Back:
				return center + BoxFaces.GetFaceNormal(BoxFace.Back) * extents.z;
			case BoxFace.Top:
				return center + BoxFaces.GetFaceNormal(BoxFace.Top) * extents.y;
			case BoxFace.Bottom:
				return center + BoxFaces.GetFaceNormal(BoxFace.Bottom) * extents.y;
			case BoxFace.Left:
				return center + BoxFaces.GetFaceNormal(BoxFace.Left) * extents.x;
			case BoxFace.Right:
				return center + BoxFaces.GetFaceNormal(BoxFace.Right) * extents.x;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x0014ED6C File Offset: 0x0014CF6C
		public List<Vector2> GetScreenCornerPoints(Camera camera)
		{
			Vector3 center = this.Center;
			Vector3 extents = this.Extents;
			return new List<Vector2>
			{
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z))
			};
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x0014EF74 File Offset: 0x0014D174
		public Rect GetScreenRectangle(Camera camera)
		{
			List<Vector2> screenCornerPoints = this.GetScreenCornerPoints(camera);
			Vector2 vector = screenCornerPoints[0];
			Vector2 vector2 = screenCornerPoints[0];
			foreach (Vector2 rhs in screenCornerPoints)
			{
				vector = Vector2.Min(vector, rhs);
				vector2 = Vector2.Max(vector2, rhs);
			}
			return new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x0014F00C File Offset: 0x0014D20C
		public bool Raycast(Ray ray, out float t)
		{
			return this.ToBounds().IntersectRay(ray, out t);
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x0014F029 File Offset: 0x0014D229
		public Box Transform(Matrix4x4 transformMatrix)
		{
			return new Box(this.ToBounds().Transform(transformMatrix));
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x0014F03C File Offset: 0x0014D23C
		public void MakeInvalid()
		{
			this._min = Vector3.one;
			this._max = -Vector3.one;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0014F05C File Offset: 0x0014D25C
		public bool IsValid()
		{
			return this._min.x <= this._max.x && this._min.y <= this._max.y && this._min.z <= this._max.z;
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x0014F0B6 File Offset: 0x0014D2B6
		public bool IsInvalid()
		{
			return !this.IsValid();
		}

		// Token: 0x04001FD4 RID: 8148
		private Vector3 _min;

		// Token: 0x04001FD5 RID: 8149
		private Vector3 _max;
	}
}
