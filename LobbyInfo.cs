using System;
using Steamworks;

// Token: 0x02000240 RID: 576
public class LobbyInfo
{
	// Token: 0x04001227 RID: 4647
	public string Server = "";

	// Token: 0x04001228 RID: 4648
	public string Game = "";

	// Token: 0x04001229 RID: 4649
	public string Host = "";

	// Token: 0x0400122A RID: 4650
	public string Version = "";

	// Token: 0x0400122B RID: 4651
	public string Comment = "";

	// Token: 0x0400122C RID: 4652
	public string Country = "";

	// Token: 0x0400122D RID: 4653
	public bool Passworded;

	// Token: 0x0400122E RID: 4654
	public int CurrentPlayers;

	// Token: 0x0400122F RID: 4655
	public int MaxPlayers;

	// Token: 0x04001230 RID: 4656
	public CSteamID LobbyID;

	// Token: 0x04001231 RID: 4657
	public CSteamID OwnerID;
}
