using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E1 RID: 1249
	public struct InputMode
	{
		// Token: 0x040022CF RID: 8911
		public InputModeType Type;

		// Token: 0x040022D0 RID: 8912
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Shortcut;
	}
}
