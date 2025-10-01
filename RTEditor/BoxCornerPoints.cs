using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x02000424 RID: 1060
	public static class BoxCornerPoints
	{
		// Token: 0x06003123 RID: 12579 RVA: 0x0014F0C4 File Offset: 0x0014D2C4
		static BoxCornerPoints()
		{
			BoxCornerPoints._cornerPoints = new BoxCornerPoint[BoxCornerPoints._count];
			BoxCornerPoints._cornerPoints[3] = BoxCornerPoint.FrontBottomLeft;
			BoxCornerPoints._cornerPoints[2] = BoxCornerPoint.FrontBottomRight;
			BoxCornerPoints._cornerPoints[0] = BoxCornerPoint.FrontTopLeft;
			BoxCornerPoints._cornerPoints[1] = BoxCornerPoint.FrontTopRight;
			BoxCornerPoints._cornerPoints[7] = BoxCornerPoint.BackBottomLeft;
			BoxCornerPoints._cornerPoints[6] = BoxCornerPoint.BackBottomRight;
			BoxCornerPoints._cornerPoints[4] = BoxCornerPoint.BackTopLeft;
			BoxCornerPoints._cornerPoints[5] = BoxCornerPoint.BackTopRight;
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x0014F139 File Offset: 0x0014D339
		public static int Count
		{
			get
			{
				return BoxCornerPoints._count;
			}
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x0014F140 File Offset: 0x0014D340
		public static List<BoxCornerPoint> GetAll()
		{
			return new List<BoxCornerPoint>(BoxCornerPoints._cornerPoints);
		}

		// Token: 0x04001FDF RID: 8159
		private static readonly BoxCornerPoint[] _cornerPoints;

		// Token: 0x04001FE0 RID: 8160
		private static readonly int _count = Enum.GetValues(typeof(BoxCornerPoint)).Length;
	}
}
