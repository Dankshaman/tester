using System;

// Token: 0x0200023D RID: 573
public static class Achievements
{
	// Token: 0x06001C59 RID: 7257 RVA: 0x000C3C9A File Offset: 0x000C1E9A
	public static void Set(string AchievementName)
	{
		Singleton<SteamManager>.Instance.SetAchievement(AchievementName);
	}

	// Token: 0x040011FA RID: 4602
	public const string PLAY_50_HOURS = "ACH_PLAY_50_HOURS";

	// Token: 0x040011FB RID: 4603
	public const string PLAY_100_HOURS = "ACH_PLAY_100_HOURS";

	// Token: 0x040011FC RID: 4604
	public const string PLAY_200_HOURS = "ACH_PLAY_200_HOURS";

	// Token: 0x040011FD RID: 4605
	public const string PLAY_500_HOURS = "ACH_PLAY_500_HOURS";

	// Token: 0x040011FE RID: 4606
	public const string PLAY_1000_HOURS = "ACH_PLAY_1000_HOURS";

	// Token: 0x040011FF RID: 4607
	public const string CHANGE_COLOR_50_TIMES = "ACH_CHANGE_COLOR_50_TIMES";

	// Token: 0x04001200 RID: 4608
	public const string FLIP_TABLE_1000_TIMES = "ACH_FLIP_TABLE_1000_TIMES";

	// Token: 0x04001201 RID: 4609
	public const string FLIP_TABLE_FAILED = "ACH_FLIP_TABLE_FAILED";

	// Token: 0x04001202 RID: 4610
	public const string PLAY_CHESS_1_HOUR = "ACH_PLAY_CHESS_1_HOUR";

	// Token: 0x04001203 RID: 4611
	public const string COMPLETE_TUTORIAL_UNDER_30_SECONDS = "ACH_COMPLETE_TUTORIAL_UNDER_30_SECONDS";

	// Token: 0x04001204 RID: 4612
	public const string FLIP_TABLE_100_TIMES = "ACH_FLIP_TABLE_100_TIMES";

	// Token: 0x04001205 RID: 4613
	public const string SPAWN_10000_CHIPS = "ACH_SPAWN_10,000_CHIPS";

	// Token: 0x04001206 RID: 4614
	public const string UPLOAD_3_STEAM_WORKSHOP_ITEMS = "ACH_UPLOAD_3_STEAM_WORKSHOP_ITEMS";

	// Token: 0x04001207 RID: 4615
	public const string JOIN_LOBBY = "ACH_JOIN_LOBBY";

	// Token: 0x04001208 RID: 4616
	public const string RPG_FIGURINES_ATTACK = "ACH_RPG_FIGURINES_ATTACK";

	// Token: 0x04001209 RID: 4617
	public const string FULL_GAME_OF_8_PLAYERS = "ACH_FULL_GAME_OF_8_PLAYERS";

	// Token: 0x0400120A RID: 4618
	public const string PLAY_GAME_ZERO_GRAVITY = "ACH_PLAY_GAME_ZERO_GRAVITY";

	// Token: 0x0400120B RID: 4619
	public const string PLAY_GAME_1_HOUR_NO_FLIP = "ACH_PLAY_GAME_1_HOUR_NO_FLIP";

	// Token: 0x0400120C RID: 4620
	public const string USE_PRIVATE_NOTEPAD = "ACH_USE_PRIVATE_NOTEPAD";

	// Token: 0x0400120D RID: 4621
	public const string SAVE_100_ITEMS_TO_CHEST = "ACH_SAVE_100_ITEMS_TO_CHEST";

	// Token: 0x0400120E RID: 4622
	public const string LASER_LIGHT_SHOW_8_PLAYER = "ACH_LASER_LIGHT_SHOW_8_PLAYER";

	// Token: 0x0400120F RID: 4623
	public const string LOCK_5000_ITEMS = "ACH_LOCK_5000_ITEMS";

	// Token: 0x04001210 RID: 4624
	public const string TINT_1000_OBJECTS = "ACH_TINT_1000_OBJECTS";

	// Token: 0x04001211 RID: 4625
	public const string HIDDEN_KNIL_KIMI = "ACH_HIDDEN_KNIL_KIMI";

	// Token: 0x04001212 RID: 4626
	public const string PLAY_WITH_STEAM_FRIEND = "ACH_PLAY_WITH_STEAM_FRIEND";

	// Token: 0x04001213 RID: 4627
	public const string COMPLETE_TUTORIAL = "ACH_COMPLETE_TUTORIAL";
}
