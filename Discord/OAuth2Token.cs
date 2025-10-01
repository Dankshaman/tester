using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004D1 RID: 1233
	public struct OAuth2Token
	{
		// Token: 0x04002299 RID: 8857
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string AccessToken;

		// Token: 0x0400229A RID: 8858
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		public string Scopes;

		// Token: 0x0400229B RID: 8859
		public long Expires;
	}
}
