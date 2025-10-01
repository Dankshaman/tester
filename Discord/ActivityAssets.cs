using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004D5 RID: 1237
	public struct ActivityAssets
	{
		// Token: 0x040022A3 RID: 8867
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeImage;

		// Token: 0x040022A4 RID: 8868
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeText;

		// Token: 0x040022A5 RID: 8869
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallImage;

		// Token: 0x040022A6 RID: 8870
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallText;
	}
}
