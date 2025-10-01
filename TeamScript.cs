using System;
using System.Collections.Generic;

// Token: 0x02000254 RID: 596
public class TeamScript : Singleton<TeamScript>
{
	// Token: 0x06001F6F RID: 8047 RVA: 0x000E08F0 File Offset: 0x000DEAF0
	private void ClearFlags()
	{
		for (int i = 0; i < 5; i++)
		{
			this.TeamFlags[(Team)i] = 0U;
		}
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000E0916 File Offset: 0x000DEB16
	private void Start()
	{
		this.ClearFlags();
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000E0920 File Offset: 0x000DEB20
	public void CalculateAllFlags()
	{
		this.ClearFlags();
		for (int i = 0; i < NetworkSingleton<PlayerManager>.Instance.PlayersList.Count; i++)
		{
			PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayersList[i];
			if (playerState.team != Team.None)
			{
				Dictionary<Team, uint> teamFlags = this.TeamFlags;
				Team team = playerState.team;
				teamFlags[team] |= Colour.FlagFromColor(playerState.color);
			}
		}
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x000E0990 File Offset: 0x000DEB90
	public uint TeamFlagsFromPlayerID(int id)
	{
		if (!NetworkSingleton<PlayerManager>.Instance)
		{
			return 0U;
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
		if (playerState == null || playerState.team == Team.None)
		{
			return 0U;
		}
		return this.TeamFlags[playerState.team];
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x000E09D8 File Offset: 0x000DEBD8
	public static string StringFromTeam(Team team)
	{
		switch (team)
		{
		case Team.Hearts:
			return "Hearts";
		case Team.Diamonds:
			return "Diamonds";
		case Team.Clubs:
			return "Clubs";
		case Team.Spades:
			return "Spades";
		case Team.Jokers:
			return "Jokers";
		default:
			return "";
		}
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x000E0A24 File Offset: 0x000DEC24
	public static Team TeamFromString(string team)
	{
		string a = team.ToLower();
		if (a == "hearts")
		{
			return Team.Hearts;
		}
		if (a == "diamonds")
		{
			return Team.Diamonds;
		}
		if (a == "clubs")
		{
			return Team.Clubs;
		}
		if (a == "spades")
		{
			return Team.Spades;
		}
		if (!(a == "jokers"))
		{
			return Team.None;
		}
		return Team.Jokers;
	}

	// Token: 0x0400134F RID: 4943
	public Dictionary<Team, uint> TeamFlags = new Dictionary<Team, uint>();
}
