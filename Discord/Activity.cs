using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004D9 RID: 1241
	public struct Activity
	{
		// Token: 0x040022AE RID: 8878
		public ActivityType Type;

		// Token: 0x040022AF RID: 8879
		public long ApplicationId;

		// Token: 0x040022B0 RID: 8880
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Name;

		// Token: 0x040022B1 RID: 8881
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string State;

		// Token: 0x040022B2 RID: 8882
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Details;

		// Token: 0x040022B3 RID: 8883
		public ActivityTimestamps Timestamps;

		// Token: 0x040022B4 RID: 8884
		public ActivityAssets Assets;

		// Token: 0x040022B5 RID: 8885
		public ActivityParty Party;

		// Token: 0x040022B6 RID: 8886
		public ActivitySecrets Secrets;

		// Token: 0x040022B7 RID: 8887
		public bool Instance;
	}
}
