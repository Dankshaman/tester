using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004D8 RID: 1240
	public struct ActivitySecrets
	{
		// Token: 0x040022AB RID: 8875
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Match;

		// Token: 0x040022AC RID: 8876
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Join;

		// Token: 0x040022AD RID: 8877
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Spectate;
	}
}
