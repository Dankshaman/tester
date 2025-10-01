using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004E2 RID: 1250
	public struct UserAchievement
	{
		// Token: 0x040022D1 RID: 8913
		public long UserId;

		// Token: 0x040022D2 RID: 8914
		public long AchievementId;

		// Token: 0x040022D3 RID: 8915
		public byte PercentComplete;

		// Token: 0x040022D4 RID: 8916
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string UnlockedAt;
	}
}
