using System;
using System.Runtime.InteropServices;

// Token: 0x02000097 RID: 151
public class MouseLibWin
{
	// Token: 0x0600080E RID: 2062
	[DllImport("user32.dll")]
	private static extern int SetCursorPos(int x, int y);

	// Token: 0x0600080F RID: 2063
	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(out MouseLibWin.POINT mousePos);

	// Token: 0x06000810 RID: 2064 RVA: 0x000384CD File Offset: 0x000366CD
	public static void _MoveCursorToPoint(int x, int y)
	{
		MouseLibWin.SetCursorPos(x, y);
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x000384D8 File Offset: 0x000366D8
	public static int _GetGlobalMousePositionHorizontal()
	{
		MouseLibWin.POINT point;
		MouseLibWin.GetCursorPos(out point);
		return point.X;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x000384F4 File Offset: 0x000366F4
	public static int _GetGlobalMousePositionVertical()
	{
		MouseLibWin.POINT point;
		MouseLibWin.GetCursorPos(out point);
		return point.Y;
	}

	// Token: 0x0200057D RID: 1405
	private struct POINT
	{
		// Token: 0x040024FF RID: 9471
		public int X;

		// Token: 0x04002500 RID: 9472
		public int Y;
	}
}
