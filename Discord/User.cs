using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004D0 RID: 1232
	public struct User
	{
		// Token: 0x04002294 RID: 8852
		public long Id;

		// Token: 0x04002295 RID: 8853
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Username;

		// Token: 0x04002296 RID: 8854
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string Discriminator;

		// Token: 0x04002297 RID: 8855
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Avatar;

		// Token: 0x04002298 RID: 8856
		public bool Bot;
	}
}
