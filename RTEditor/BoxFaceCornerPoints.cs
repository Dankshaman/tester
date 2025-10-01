using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x02000427 RID: 1063
	public static class BoxFaceCornerPoints
	{
		// Token: 0x06003126 RID: 12582 RVA: 0x0014F14C File Offset: 0x0014D34C
		static BoxFaceCornerPoints()
		{
			BoxFaceCornerPoints._faceCornerPoints = new BoxFaceCornerPoint[BoxFaceCornerPoints._count];
			BoxFaceCornerPoints._faceCornerPoints[0] = BoxFaceCornerPoint.TopLeft;
			BoxFaceCornerPoints._faceCornerPoints[1] = BoxFaceCornerPoint.TopRight;
			BoxFaceCornerPoints._faceCornerPoints[2] = BoxFaceCornerPoint.BottomRight;
			BoxFaceCornerPoints._faceCornerPoints[3] = BoxFaceCornerPoint.BottomLeft;
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x0014F1A1 File Offset: 0x0014D3A1
		public static int Count
		{
			get
			{
				return BoxFaceCornerPoints._count;
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x0014F1A8 File Offset: 0x0014D3A8
		public static List<BoxFaceCornerPoint> GetAll()
		{
			return new List<BoxFaceCornerPoint>(BoxFaceCornerPoints._faceCornerPoints);
		}

		// Token: 0x04001FED RID: 8173
		private static readonly BoxFaceCornerPoint[] _faceCornerPoints;

		// Token: 0x04001FEE RID: 8174
		private static readonly int _count = Enum.GetValues(typeof(BoxFaceCornerPoint)).Length;
	}
}
