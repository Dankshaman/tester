using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x0200042C RID: 1068
	public static class BoxPoints
	{
		// Token: 0x06003137 RID: 12599 RVA: 0x0014F460 File Offset: 0x0014D660
		static BoxPoints()
		{
			BoxPoints._points = new BoxPoint[BoxPoints._count];
			BoxPoints._points[0] = BoxPoint.Center;
			BoxPoints._points[4] = BoxPoint.FrontBottomLeft;
			BoxPoints._points[3] = BoxPoint.FrontBottomRight;
			BoxPoints._points[1] = BoxPoint.FrontTopLeft;
			BoxPoints._points[2] = BoxPoint.FrontTopRight;
			BoxPoints._points[8] = BoxPoint.BackBottomLeft;
			BoxPoints._points[7] = BoxPoint.BackBottomRight;
			BoxPoints._points[5] = BoxPoint.BackTopLeft;
			BoxPoints._points[6] = BoxPoint.BackTopRight;
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06003138 RID: 12600 RVA: 0x0014F4DD File Offset: 0x0014D6DD
		public static int Count
		{
			get
			{
				return BoxPoints._count;
			}
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x0014F4E4 File Offset: 0x0014D6E4
		public static List<BoxPoint> GetAll()
		{
			return new List<BoxPoint>(BoxPoints._points);
		}

		// Token: 0x04002001 RID: 8193
		private static readonly BoxPoint[] _points;

		// Token: 0x04002002 RID: 8194
		private static readonly int _count = Enum.GetValues(typeof(BoxPoint)).Length;
	}
}
