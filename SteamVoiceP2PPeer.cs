using System;
using Steamworks;

// Token: 0x02000004 RID: 4
public struct SteamVoiceP2PPeer : IEquatable<SteamVoiceP2PPeer>
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000011 RID: 17 RVA: 0x00002821 File Offset: 0x00000A21
	// (set) Token: 0x06000012 RID: 18 RVA: 0x00002829 File Offset: 0x00000A29
	public CSteamID steamID { get; private set; }

	// Token: 0x06000013 RID: 19 RVA: 0x00002832 File Offset: 0x00000A32
	public SteamVoiceP2PPeer(CSteamID steamID)
	{
		this.steamID = steamID;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000283B File Offset: 0x00000A3B
	public bool Equals(SteamVoiceP2PPeer other)
	{
		return this.steamID == other.steamID;
	}
}
