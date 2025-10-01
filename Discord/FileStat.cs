using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004DD RID: 1245
	public struct FileStat
	{
		// Token: 0x040022C3 RID: 8899
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Filename;

		// Token: 0x040022C4 RID: 8900
		public ulong Size;

		// Token: 0x040022C5 RID: 8901
		public ulong LastModified;
	}
}
