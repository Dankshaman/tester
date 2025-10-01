using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004D7 RID: 1239
	public struct ActivityParty
	{
		// Token: 0x040022A9 RID: 8873
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Id;

		// Token: 0x040022AA RID: 8874
		public PartySize Size;
	}
}
