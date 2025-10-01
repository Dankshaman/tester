using System;
using Steamworks;

// Token: 0x02000101 RID: 257
public struct JoinLobbyDetails
{
	// Token: 0x06000CA3 RID: 3235 RVA: 0x0005666F File Offset: 0x0005486F
	public string Encode()
	{
		return Json.GetJson(this, false);
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00056682 File Offset: 0x00054882
	public JoinLobbyDetails(CSteamID lobbyId, string password)
	{
		this.lobbyID = lobbyId;
		this.password = password;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00056692 File Offset: 0x00054892
	public JoinLobbyDetails(string json)
	{
		this = Json.Load<JoinLobbyDetails>(json);
	}

	// Token: 0x04000895 RID: 2197
	public CSteamID lobbyID;

	// Token: 0x04000896 RID: 2198
	public string password;
}
