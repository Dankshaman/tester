using System;
using System.Collections.Generic;

// Token: 0x0200011B RID: 283
public class GlobalBanList
{
	// Token: 0x06000F13 RID: 3859 RVA: 0x00066D94 File Offset: 0x00064F94
	public static bool IsBanned(string Name, string StringSteamId, string DeviceIdentifier)
	{
		string text = Name.ToLower();
		text = text.Trim();
		return GlobalBanList.BannedNames.Contains(text) || GlobalBanList.BannedSteamIds.Contains(StringSteamId) || GlobalBanList.BannedDeviceIdentifier.Contains(DeviceIdentifier);
	}

	// Token: 0x04000958 RID: 2392
	public static List<string> BannedNames = new List<string>
	{
		"opcode void"
	};

	// Token: 0x04000959 RID: 2393
	public static List<string> BannedSteamIds = new List<string>
	{
		"76561198032428123",
		"76561198012566027",
		"76561198241487532",
		"76561198242219780",
		"76561198070621882",
		"76561198210626443",
		"76561198210620427",
		"76561198012538925",
		"7656119807870499",
		"76561198435816733",
		"76561198045080632"
	};

	// Token: 0x0400095A RID: 2394
	public static List<string> BannedDeviceIdentifier = new List<string>
	{
		"d6ec1765d87700492a1ac3f1901bf3fab27a2418",
		"8cd547bddbfd74dcfd08851334a3c8bf2a877e42"
	};
}
