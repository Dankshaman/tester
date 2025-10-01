using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x02000429 RID: 1065
	public static class BoxFacePoints
	{
		// Token: 0x06003129 RID: 12585 RVA: 0x0014F1B4 File Offset: 0x0014D3B4
		static BoxFacePoints()
		{
			BoxFacePoints._facePoints = new BoxFacePoint[BoxFacePoints._count];
			BoxFacePoints._facePoints[0] = BoxFacePoint.Center;
			BoxFacePoints._facePoints[1] = BoxFacePoint.TopLeft;
			BoxFacePoints._facePoints[2] = BoxFacePoint.TopRight;
			BoxFacePoints._facePoints[3] = BoxFacePoint.BottomRight;
			BoxFacePoints._facePoints[4] = BoxFacePoint.BottomLeft;
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x0014F211 File Offset: 0x0014D411
		public static int Count
		{
			get
			{
				return BoxFacePoints._count;
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x0014F218 File Offset: 0x0014D418
		public static List<BoxFacePoint> GetAll()
		{
			return new List<BoxFacePoint>(BoxFacePoints._facePoints);
		}

		// Token: 0x04001FF5 RID: 8181
		private static readonly BoxFacePoint[] _facePoints;

		// Token: 0x04001FF6 RID: 8182
		private static readonly int _count = Enum.GetValues(typeof(BoxFacePoint)).Length;
	}
}
