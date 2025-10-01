using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004DF RID: 1247
	public struct SkuPrice
	{
		// Token: 0x040022C9 RID: 8905
		public uint Amount;

		// Token: 0x040022CA RID: 8906
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string Currency;
	}
}
