using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200042A RID: 1066
	public static class BoxFaces
	{
		// Token: 0x0600312C RID: 12588 RVA: 0x0014F224 File Offset: 0x0014D424
		static BoxFaces()
		{
			BoxFaces._faces = new BoxFace[BoxFaces._count];
			BoxFaces._faces[1] = BoxFace.Back;
			BoxFaces._faces[0] = BoxFace.Front;
			BoxFaces._faces[4] = BoxFace.Left;
			BoxFaces._faces[5] = BoxFace.Right;
			BoxFaces._faces[2] = BoxFace.Top;
			BoxFaces._faces[3] = BoxFace.Bottom;
			BoxFaces._faceNormals = new Vector3[BoxFaces._count];
			BoxFaces._faceNormals[1] = Vector3.forward;
			BoxFaces._faceNormals[0] = Vector3.back;
			BoxFaces._faceNormals[4] = Vector3.left;
			BoxFaces._faceNormals[5] = Vector3.right;
			BoxFaces._faceNormals[2] = Vector3.up;
			BoxFaces._faceNormals[3] = Vector3.down;
			BoxFaces._faceRightAxes = new Vector3[BoxFaces._count];
			BoxFaces._faceRightAxes[1] = Vector3.left;
			BoxFaces._faceRightAxes[0] = Vector3.right;
			BoxFaces._faceRightAxes[4] = Vector3.back;
			BoxFaces._faceRightAxes[5] = Vector3.forward;
			BoxFaces._faceRightAxes[2] = Vector3.right;
			BoxFaces._faceRightAxes[3] = Vector3.right;
			BoxFaces._faceLookAxes = new Vector3[BoxFaces._count];
			BoxFaces._faceLookAxes[1] = Vector3.up;
			BoxFaces._faceLookAxes[0] = Vector3.up;
			BoxFaces._faceLookAxes[4] = Vector3.up;
			BoxFaces._faceLookAxes[5] = Vector3.up;
			BoxFaces._faceLookAxes[2] = Vector3.forward;
			BoxFaces._faceLookAxes[3] = Vector3.back;
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x0014F3D6 File Offset: 0x0014D5D6
		public static int Count
		{
			get
			{
				return BoxFaces._count;
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x0014F3DD File Offset: 0x0014D5DD
		public static List<BoxFace> GetAll()
		{
			return new List<BoxFace>(BoxFaces._faces);
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x0014F3E9 File Offset: 0x0014D5E9
		public static BoxFace GetNext(BoxFace boxFace)
		{
			return (boxFace + 1) % (BoxFace)BoxFaces._count;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x0014F3F4 File Offset: 0x0014D5F4
		public static List<Vector3> GetAllFaceNormals()
		{
			return new List<Vector3>(BoxFaces._faceNormals);
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x0014F400 File Offset: 0x0014D600
		public static List<Vector3> GetAllFaceRightAxes()
		{
			return new List<Vector3>(BoxFaces._faceRightAxes);
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x0014F40C File Offset: 0x0014D60C
		public static List<Vector3> GetAllFaceLookAxes()
		{
			return new List<Vector3>(BoxFaces._faceLookAxes);
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x0014F418 File Offset: 0x0014D618
		public static Vector3 GetFaceNormal(BoxFace boxFace)
		{
			return BoxFaces._faceNormals[(int)boxFace];
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x0014F425 File Offset: 0x0014D625
		public static Vector3 GetFaceRightAxis(BoxFace boxFace)
		{
			return BoxFaces._faceRightAxes[(int)boxFace];
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x0014F432 File Offset: 0x0014D632
		public static Vector3 GetFaceLookAxis(BoxFace boxFace)
		{
			return BoxFaces._faceLookAxes[(int)boxFace];
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x0014F43F File Offset: 0x0014D63F
		public static BoxFace GetOpposite(BoxFace boxFace)
		{
			if (boxFace == BoxFace.Back)
			{
				return BoxFace.Front;
			}
			if (boxFace == BoxFace.Front)
			{
				return BoxFace.Back;
			}
			if (boxFace == BoxFace.Left)
			{
				return BoxFace.Right;
			}
			if (boxFace == BoxFace.Right)
			{
				return BoxFace.Left;
			}
			if (boxFace == BoxFace.Bottom)
			{
				return BoxFace.Top;
			}
			return BoxFace.Bottom;
		}

		// Token: 0x04001FF7 RID: 8183
		private static readonly BoxFace[] _faces;

		// Token: 0x04001FF8 RID: 8184
		private static readonly Vector3[] _faceNormals;

		// Token: 0x04001FF9 RID: 8185
		private static readonly Vector3[] _faceRightAxes;

		// Token: 0x04001FFA RID: 8186
		private static readonly Vector3[] _faceLookAxes;

		// Token: 0x04001FFB RID: 8187
		private static readonly int _count = Enum.GetValues(typeof(BoxFace)).Length;
	}
}
