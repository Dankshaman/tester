using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020004DC RID: 1244
	public struct Lobby
	{
		// Token: 0x040022BD RID: 8893
		public long Id;

		// Token: 0x040022BE RID: 8894
		public LobbyType Type;

		// Token: 0x040022BF RID: 8895
		public long OwnerId;

		// Token: 0x040022C0 RID: 8896
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Secret;

		// Token: 0x040022C1 RID: 8897
		public uint Capacity;

		// Token: 0x040022C2 RID: 8898
		public bool Locked;
	}
}
