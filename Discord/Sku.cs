using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E0 RID: 1248
	public struct Sku
	{
		// Token: 0x040022CB RID: 8907
		public long Id;

		// Token: 0x040022CC RID: 8908
		public SkuType Type;

		// Token: 0x040022CD RID: 8909
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Name;

		// Token: 0x040022CE RID: 8910
		public SkuPrice Price;
	}
}
