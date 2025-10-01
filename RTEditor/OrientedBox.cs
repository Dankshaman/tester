using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200042D RID: 1069
	public class OrientedBox
	{
		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x0014F4F0 File Offset: 0x0014D6F0
		public Box ModelSpaceBox
		{
			get
			{
				return this._modelSpaceBox;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x0014F4F8 File Offset: 0x0014D6F8
		public Vector3 ModelSpaceExtents
		{
			get
			{
				return this._modelSpaceBox.Extents;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x0014F505 File Offset: 0x0014D705
		public Vector3 ScaledExtents
		{
			get
			{
				return Vector3.Scale(this.ModelSpaceExtents, this.Scale);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x0014F518 File Offset: 0x0014D718
		// (set) Token: 0x0600313E RID: 12606 RVA: 0x0014F525 File Offset: 0x0014D725
		public Vector3 ModelSpaceSize
		{
			get
			{
				return this._modelSpaceBox.Size;
			}
			set
			{
				this._modelSpaceBox.Size = value;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x0014F533 File Offset: 0x0014D733
		public Vector3 ScaledSize
		{
			get
			{
				return Vector3.Scale(this._modelSpaceBox.Size, this.Scale);
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x0014F54B File Offset: 0x0014D74B
		// (set) Token: 0x06003141 RID: 12609 RVA: 0x0014F553 File Offset: 0x0014D753
		public Quaternion Rotation
		{
			get
			{
				return this._rotation;
			}
			set
			{
				this._rotation = value;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x0014F55C File Offset: 0x0014D75C
		// (set) Token: 0x06003143 RID: 12611 RVA: 0x0014F564 File Offset: 0x0014D764
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

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x0014F56D File Offset: 0x0014D76D
		public Matrix4x4 TransformMatrix
		{
			get
			{
				return Matrix4x4.TRS(this.Center - this.Rotation * Vector3.Scale(this._modelSpaceBox.Center, this.Scale), this.Rotation, this.Scale);
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x0014F5AC File Offset: 0x0014D7AC
		// (set) Token: 0x06003146 RID: 12614 RVA: 0x0014F5B4 File Offset: 0x0014D7B4
		public bool AllowNegativeScale
		{
			get
			{
				return this._allowNegativeScale;
			}
			set
			{
				this._allowNegativeScale = value;
				if (!this._allowNegativeScale)
				{
					this.Scale = this.Scale.GetVectorWithAbsComponents();
				}
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06003147 RID: 12615 RVA: 0x0014F5D6 File Offset: 0x0014D7D6
		// (set) Token: 0x06003148 RID: 12616 RVA: 0x0014F5F2 File Offset: 0x0014D7F2
		public Vector3 Scale
		{
			get
			{
				if (!this._allowNegativeScale)
				{
					return this._scale.GetVectorWithAbsComponents();
				}
				return this._scale;
			}
			set
			{
				this._scale = value;
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x0014F5FB File Offset: 0x0014D7FB
		public OrientedBox()
		{
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x0014F624 File Offset: 0x0014D824
		public OrientedBox(Box modelSpaceBox)
		{
			this._modelSpaceBox = modelSpaceBox;
			this.Center = this._modelSpaceBox.Center;
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x0014F670 File Offset: 0x0014D870
		public OrientedBox(Box modelSpaceBox, Quaternion rotation)
		{
			this._modelSpaceBox = modelSpaceBox;
			this.Center = this._modelSpaceBox.Center;
			this.Rotation = rotation;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x0014F6C4 File Offset: 0x0014D8C4
		public OrientedBox(Box modelSpaceBox, Transform transform)
		{
			this._modelSpaceBox = modelSpaceBox;
			this._center = this._modelSpaceBox.Center;
			this.Transform(transform);
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x0014F718 File Offset: 0x0014D918
		public OrientedBox(OrientedBox source)
		{
			this._modelSpaceBox = source.ModelSpaceBox;
			this._center = source.Center;
			this._rotation = source.Rotation;
			this._scale = source._scale;
			this._allowNegativeScale = source.AllowNegativeScale;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0014F788 File Offset: 0x0014D988
		public static OrientedBox GetInvalid()
		{
			OrientedBox orientedBox = new OrientedBox();
			orientedBox.MakeInvalid();
			return orientedBox;
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x0014F798 File Offset: 0x0014D998
		public void Transform(Transform transform)
		{
			this.Rotation = transform.rotation * this.Rotation;
			this._scale = Vector3.Scale(transform.lossyScale, this._scale);
			this.Center = transform.localToWorldMatrix.MultiplyPoint(this.Center);
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x0014F7F0 File Offset: 0x0014D9F0
		public void Transform(Matrix4x4 transformMatrix)
		{
			this.Rotation = transformMatrix.GetRotation() * this.Rotation;
			this._scale = Vector3.Scale(transformMatrix.GetXYZScale(), this._scale);
			this.Center = transformMatrix.MultiplyPoint(this.Center);
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x0014F840 File Offset: 0x0014DA40
		public void Encapsulate(OrientedBox orientedBox)
		{
			foreach (Vector3 point in orientedBox.GetCenterAndCornerPoints())
			{
				this.AddPoint(point);
			}
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x0014F894 File Offset: 0x0014DA94
		public Sphere3D GetEncapsulatingSphere()
		{
			return new Sphere3D(this.Center, this.ScaledExtents.magnitude);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x0014F8BA File Offset: 0x0014DABA
		public Box GetEncapsulatingBox()
		{
			return Box.FromPoints(this.GetCenterAndCornerPoints(), 1f);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x0014F8CC File Offset: 0x0014DACC
		public void AddPoint(Vector3 point)
		{
			this._modelSpaceBox.AddPoint(this.GetPointInModelSpace(point));
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0014F8E0 File Offset: 0x0014DAE0
		public bool Intersects(OrientedBox otherBox)
		{
			Vector3 scale = this.Scale;
			Vector3 scale2 = otherBox.Scale;
			this.Scale = scale.GetVectorWithAbsComponents();
			otherBox.Scale = scale2.GetVectorWithAbsComponents();
			Matrix4x4 transformMatrix = this.TransformMatrix;
			Vector3 axis = transformMatrix.GetAxis(0);
			Vector3 axis2 = transformMatrix.GetAxis(1);
			Vector3 axis3 = transformMatrix.GetAxis(2);
			Vector3[] array = new Vector3[]
			{
				axis,
				axis2,
				axis3
			};
			Matrix4x4 transformMatrix2 = otherBox.TransformMatrix;
			Vector3 axis4 = transformMatrix2.GetAxis(0);
			Vector3 axis5 = transformMatrix2.GetAxis(1);
			Vector3 axis6 = transformMatrix2.GetAxis(2);
			Vector3[] array2 = new Vector3[]
			{
				axis4,
				axis5,
				axis6
			};
			float[,] array3 = new float[3, 3];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					array3[i, j] = Vector3.Dot(array[i], array2[j]);
				}
			}
			Vector3 scaledExtents = this.ScaledExtents;
			Vector3 vector = new Vector3(scaledExtents.x, scaledExtents.y, scaledExtents.z);
			scaledExtents = otherBox.ScaledExtents;
			Vector3 vector2 = new Vector3(scaledExtents.x, scaledExtents.y, scaledExtents.z);
			float[,] array4 = new float[3, 3];
			for (int k = 0; k < 3; k++)
			{
				for (int l = 0; l < 3; l++)
				{
					array4[k, l] = Mathf.Abs(array3[k, l]) + 0.0001f;
				}
			}
			Vector3 lhs = otherBox.Center - this.Center;
			Vector3 vector3 = new Vector3(Vector3.Dot(lhs, axis), Vector3.Dot(lhs, axis2), Vector3.Dot(lhs, axis3));
			for (int m = 0; m < 3; m++)
			{
				float num = vector2[0] * array4[m, 0] + vector2[1] * array4[m, 1] + vector2[2] * array4[m, 2];
				if (Mathf.Abs(vector3[m]) > vector[m] + num)
				{
					return false;
				}
			}
			for (int n = 0; n < 3; n++)
			{
				float num2 = vector[0] * array4[0, n] + vector[1] * array4[1, n] + vector[2] * array4[2, n];
				if (Mathf.Abs(vector3[0] * array3[0, n] + vector3[1] * array3[1, n] + vector3[2] * array3[2, n]) > num2 + vector2[n])
				{
					return false;
				}
			}
			float num3 = vector[1] * array4[2, 0] + vector[2] * array4[1, 0];
			float num4 = vector2[1] * array4[0, 2] + vector2[2] * array4[0, 1];
			if (Mathf.Abs(vector3[2] * array3[1, 0] - vector3[1] * array3[2, 0]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[1] * array4[2, 1] + vector[2] * array4[1, 1];
			num4 = vector2[0] * array4[0, 2] + vector2[2] * array4[0, 0];
			if (Mathf.Abs(vector3[2] * array3[1, 1] - vector3[1] * array3[2, 1]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[1] * array4[2, 2] + vector[2] * array4[1, 2];
			num4 = vector2[0] * array4[0, 1] + vector2[1] * array4[0, 0];
			if (Mathf.Abs(vector3[2] * array3[1, 2] - vector3[1] * array3[2, 2]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[0] * array4[2, 0] + vector[2] * array4[0, 0];
			num4 = vector2[1] * array4[1, 2] + vector2[2] * array4[1, 1];
			if (Mathf.Abs(vector3[0] * array3[2, 0] - vector3[2] * array3[0, 0]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[0] * array4[2, 1] + vector[2] * array4[0, 1];
			num4 = vector2[0] * array4[1, 2] + vector2[2] * array4[1, 0];
			if (Mathf.Abs(vector3[0] * array3[2, 1] - vector3[2] * array3[0, 1]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[0] * array4[2, 2] + vector[2] * array4[0, 2];
			num4 = vector2[0] * array4[1, 1] + vector2[1] * array4[1, 0];
			if (Mathf.Abs(vector3[0] * array3[2, 2] - vector3[2] * array3[0, 2]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[0] * array4[1, 0] + vector[1] * array4[0, 0];
			num4 = vector2[1] * array4[2, 2] + vector2[2] * array4[2, 1];
			if (Math.Abs(vector3[1] * array3[0, 0] - vector3[0] * array3[1, 0]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[0] * array4[1, 1] + vector[1] * array4[0, 1];
			num4 = vector2[0] * array4[2, 2] + vector2[2] * array4[2, 0];
			if (Math.Abs(vector3[1] * array3[0, 1] - vector3[0] * array3[1, 1]) > num3 + num4)
			{
				return false;
			}
			num3 = vector[0] * array4[1, 2] + vector[1] * array4[0, 2];
			num4 = vector2[0] * array4[2, 1] + vector2[1] * array4[2, 0];
			if (Math.Abs(vector3[1] * array3[0, 2] - vector3[0] * array3[1, 2]) > num3 + num4)
			{
				return false;
			}
			this.Scale = scale;
			otherBox.Scale = scale2;
			return true;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x0015004C File Offset: 0x0014E24C
		public bool AreAllBoxPointsOnOrInFrontOfAnyFacePlane(OrientedBox otherBox)
		{
			List<Vector3> centerAndCornerPoints = otherBox.GetCenterAndCornerPoints();
			using (List<Plane>.Enumerator enumerator = this.GetBoxFacePlanes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AreAllPointsInFrontOrOnPlane(centerAndCornerPoints))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x001500B0 File Offset: 0x0014E2B0
		public Vector3 GetClosestPointToPoint(Vector3 point)
		{
			Vector3 rhs = point - this.Center;
			Vector3 vector = this.Center;
			Vector3 scaledExtents = this.ScaledExtents;
			Vector3[] allAxes = this.TransformMatrix.GetAllAxes();
			for (int i = 0; i < 3; i++)
			{
				Vector3 vector2 = allAxes[i];
				float num = scaledExtents[i];
				float num2 = Vector3.Dot(vector2, rhs);
				if (num2 > num)
				{
					num2 = num;
				}
				else if (num2 < -num)
				{
					num2 = -num;
				}
				vector += vector2 * num2;
			}
			return vector;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x0015013C File Offset: 0x0014E33C
		public Vector3 GetPointInModelSpace(Vector3 point)
		{
			return this.TransformMatrix.inverse.MultiplyPoint(point);
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x00150160 File Offset: 0x0014E360
		public Vector3 GetDirectionInModelSpace(Vector3 direction)
		{
			return this.TransformMatrix.inverse.MultiplyVector(direction);
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x00150184 File Offset: 0x0014E384
		public Vector3 GetRotatedAndScaledSize()
		{
			return this.TransformMatrix.MultiplyVector(this.ModelSpaceSize);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x001501A5 File Offset: 0x0014E3A5
		public float GetRotatedAndScaledSizeAlongDirection(Vector3 direction)
		{
			direction.Normalize();
			return Mathf.Abs(Vector3.Dot(this.GetRotatedAndScaledSize(), direction));
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x001501BF File Offset: 0x0014E3BF
		public float GetSizeAlongDirection(Vector3 direction)
		{
			direction.Normalize();
			return direction.GetAbsDot(this.GetRotatedAndScaledSize());
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x001501D4 File Offset: 0x0014E3D4
		public List<Vector3> GetCenterAndCornerPoints()
		{
			return Vector3Extensions.GetTransformedPoints(this._modelSpaceBox.GetCenterAndCornerPoints(), this.TransformMatrix);
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x001501EC File Offset: 0x0014E3EC
		public List<Vector3> GetCornerPoints()
		{
			return Vector3Extensions.GetTransformedPoints(this._modelSpaceBox.GetCornerPoints(), this.TransformMatrix);
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x00150204 File Offset: 0x0014E404
		public List<Vector3> GetCornerPointsProjectedOnPlane(Plane plane)
		{
			return plane.ProjectAllPoints(this.GetCornerPoints());
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x00150214 File Offset: 0x0014E414
		public BoxFace GetBoxFaceClosestToPoint(Vector3 point)
		{
			Vector3 pointInModelSpace = this.GetPointInModelSpace(point);
			return this._modelSpaceBox.GetBoxFaceClosestToPoint(pointInModelSpace);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x00150238 File Offset: 0x0014E438
		public List<Plane> GetBoxFacePlanes()
		{
			List<Plane> list = new List<Plane>();
			foreach (object obj in Enum.GetValues(typeof(BoxFace)))
			{
				BoxFace boxFace = (BoxFace)obj;
				list.Add(this.GetBoxFacePlane(boxFace));
			}
			return list;
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x001502A8 File Offset: 0x0014E4A8
		public Plane GetBoxFacePlane(BoxFace boxFace)
		{
			Plane boxFacePlane = this._modelSpaceBox.GetBoxFacePlane(boxFace);
			Vector3 boxFaceCenter = this._modelSpaceBox.GetBoxFaceCenter(boxFace);
			return boxFacePlane.Transform(this.TransformMatrix, boxFaceCenter);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x001502DA File Offset: 0x0014E4DA
		public Vector2 GetBoxFaceSizeAlongFaceLocalXZAxes(BoxFace boxFace)
		{
			return this._modelSpaceBox.GetBoxFaceSizeAlongFaceLocalXZAxes(boxFace, this._scale);
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x001502F0 File Offset: 0x0014E4F0
		public Vector3 GetBoxFaceCenter(BoxFace boxFace)
		{
			Vector3 boxFaceCenter = this._modelSpaceBox.GetBoxFaceCenter(boxFace);
			return this.TransformMatrix.MultiplyPoint(boxFaceCenter);
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x0015031C File Offset: 0x0014E51C
		public BoxFace GetBoxFaceWhichFacesNormal(Vector3 normal)
		{
			int result;
			PlaneExtensions.GetPlaneWhichFacesNormal(this.GetBoxFacePlanes(), normal, out result);
			return (BoxFace)result;
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x0015033C File Offset: 0x0014E53C
		public BoxFace GetBoxFaceMostAlignedWithNormal(Vector3 normal)
		{
			int result;
			PlaneExtensions.GetPlaneMostAlignedWithNormal(this.GetBoxFacePlanes(), normal, out result);
			return (BoxFace)result;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x00150359 File Offset: 0x0014E559
		public List<Vector3> GetBoxFaceCenterAndCornerPoints(BoxFace boxFace)
		{
			return Vector3Extensions.GetTransformedPoints(this._modelSpaceBox.GetBoxFaceCenterAndCornerPoints(boxFace), this.TransformMatrix);
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x00150372 File Offset: 0x0014E572
		public List<Vector3> GetBoxFaceCornerPoints(BoxFace boxFace)
		{
			return Vector3Extensions.GetTransformedPoints(this._modelSpaceBox.GetBoxFaceCornerPoints(boxFace), this.TransformMatrix);
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x0015038C File Offset: 0x0014E58C
		public bool Raycast(Ray ray, out OrientedBoxRayHit boxRayHit)
		{
			boxRayHit = null;
			float hitEnter;
			if (this.Raycast(ray, out hitEnter))
			{
				boxRayHit = new OrientedBoxRayHit(ray, hitEnter, this);
				return true;
			}
			return false;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x001503B4 File Offset: 0x0014E5B4
		public bool Raycast(Ray ray, out float t)
		{
			Matrix4x4 transformMatrix = this.TransformMatrix;
			Ray ray2 = ray.InverseTransform(transformMatrix);
			float distance;
			if (this._modelSpaceBox.Raycast(ray2, out distance))
			{
				Vector3 point = ray2.GetPoint(distance);
				Vector3 b = transformMatrix.MultiplyPoint(point);
				t = (ray.origin - b).magnitude;
				return true;
			}
			t = 0f;
			return false;
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x00150418 File Offset: 0x0014E618
		public bool ContainsPoint(Vector3 point)
		{
			Vector3 pointInModelSpace = this.GetPointInModelSpace(point);
			return this._modelSpaceBox.ContainsPoint(pointInModelSpace);
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x00150439 File Offset: 0x0014E639
		public void MakeInvalid()
		{
			this._modelSpaceBox.MakeInvalid();
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x00150446 File Offset: 0x0014E646
		public bool IsValid()
		{
			return this._modelSpaceBox.IsValid();
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x00150453 File Offset: 0x0014E653
		public bool IsInvalid()
		{
			return this._modelSpaceBox.IsInvalid();
		}

		// Token: 0x04002003 RID: 8195
		private Box _modelSpaceBox;

		// Token: 0x04002004 RID: 8196
		private Vector3 _center = Vector3.zero;

		// Token: 0x04002005 RID: 8197
		private Quaternion _rotation = Quaternion.identity;

		// Token: 0x04002006 RID: 8198
		private Vector3 _scale = Vector3.one;

		// Token: 0x04002007 RID: 8199
		private bool _allowNegativeScale;
	}
}
