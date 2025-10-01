using System;

// Token: 0x0200023E RID: 574
public static class Stats
{
	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06001C5A RID: 7258 RVA: 0x000C3CA7 File Offset: 0x000C1EA7
	// (set) Token: 0x06001C5B RID: 7259 RVA: 0x000C3CAE File Offset: 0x000C1EAE
	public static int INT_CHANGE_COLOR_TIMES
	{
		get
		{
			return Stats._INT_CHANGE_COLOR_TIMES;
		}
		set
		{
			Stats._INT_CHANGE_COLOR_TIMES = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_CHANGE_COLOR", value);
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06001C5C RID: 7260 RVA: 0x000C3CC6 File Offset: 0x000C1EC6
	// (set) Token: 0x06001C5D RID: 7261 RVA: 0x000C3CCD File Offset: 0x000C1ECD
	public static int INT_FLIP_TABLE_TIMES
	{
		get
		{
			return Stats._INT_FLIP_TABLE_TIMES;
		}
		set
		{
			Stats._INT_FLIP_TABLE_TIMES = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_FLIP_TABLE", value);
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06001C5E RID: 7262 RVA: 0x000C3CE5 File Offset: 0x000C1EE5
	// (set) Token: 0x06001C5F RID: 7263 RVA: 0x000C3CEC File Offset: 0x000C1EEC
	public static int INT_SPAWN_CHIPS
	{
		get
		{
			return Stats._INT_SPAWN_CHIPS;
		}
		set
		{
			Stats._INT_SPAWN_CHIPS = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_SPAWN_CHIPS", value);
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06001C60 RID: 7264 RVA: 0x000C3D04 File Offset: 0x000C1F04
	// (set) Token: 0x06001C61 RID: 7265 RVA: 0x000C3D0B File Offset: 0x000C1F0B
	public static int INT_SAVE_ITEMS_TO_CHEST
	{
		get
		{
			return Stats._INT_SAVE_ITEMS_TO_CHEST;
		}
		set
		{
			Stats._INT_SAVE_ITEMS_TO_CHEST = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_SAVE_ITEMS_TO_CHEST", value);
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06001C62 RID: 7266 RVA: 0x000C3D23 File Offset: 0x000C1F23
	// (set) Token: 0x06001C63 RID: 7267 RVA: 0x000C3D2A File Offset: 0x000C1F2A
	public static int INT_LOCK_ITEMS
	{
		get
		{
			return Stats._INT_LOCK_ITEMS;
		}
		set
		{
			Stats._INT_LOCK_ITEMS = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_LOCK_ITEMS", value);
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06001C64 RID: 7268 RVA: 0x000C3D42 File Offset: 0x000C1F42
	// (set) Token: 0x06001C65 RID: 7269 RVA: 0x000C3D49 File Offset: 0x000C1F49
	public static int INT_TINT_OBJECTS
	{
		get
		{
			return Stats._INT_TINT_OBJECTS;
		}
		set
		{
			Stats._INT_TINT_OBJECTS = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_TINT_OBJECTS", value);
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06001C66 RID: 7270 RVA: 0x000C3D61 File Offset: 0x000C1F61
	// (set) Token: 0x06001C67 RID: 7271 RVA: 0x000C3D68 File Offset: 0x000C1F68
	public static int INT_UPLOAD_STEAM_WORKSHOP_ITEMS
	{
		get
		{
			return Stats._INT_UPLOAD_STEAM_WORKSHOP_ITEMS;
		}
		set
		{
			Stats._INT_UPLOAD_STEAM_WORKSHOP_ITEMS = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_UPLOAD_STEAM_WORKSHOP_ITEMS", value);
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06001C68 RID: 7272 RVA: 0x000C3D80 File Offset: 0x000C1F80
	// (set) Token: 0x06001C69 RID: 7273 RVA: 0x000C3D87 File Offset: 0x000C1F87
	public static int INT_HOURS_PLAYED
	{
		get
		{
			return Stats._INT_HOURS_PLAYED;
		}
		set
		{
			Stats._INT_HOURS_PLAYED = value;
			Singleton<SteamManager>.Instance.SetStat("STAT_HOURS_PLAYED", value);
		}
	}

	// Token: 0x04001214 RID: 4628
	public const string CHANGE_COLOR_TIMES = "STAT_CHANGE_COLOR";

	// Token: 0x04001215 RID: 4629
	private static int _INT_CHANGE_COLOR_TIMES;

	// Token: 0x04001216 RID: 4630
	public const string FLIP_TABLE_TIMES = "STAT_FLIP_TABLE";

	// Token: 0x04001217 RID: 4631
	private static int _INT_FLIP_TABLE_TIMES;

	// Token: 0x04001218 RID: 4632
	public const string SPAWN_CHIPS = "STAT_SPAWN_CHIPS";

	// Token: 0x04001219 RID: 4633
	private static int _INT_SPAWN_CHIPS;

	// Token: 0x0400121A RID: 4634
	public const string SAVE_ITEMS_TO_CHEST = "STAT_SAVE_ITEMS_TO_CHEST";

	// Token: 0x0400121B RID: 4635
	private static int _INT_SAVE_ITEMS_TO_CHEST;

	// Token: 0x0400121C RID: 4636
	public const string LOCK_ITEMS = "STAT_LOCK_ITEMS";

	// Token: 0x0400121D RID: 4637
	private static int _INT_LOCK_ITEMS;

	// Token: 0x0400121E RID: 4638
	public const string TINT_OBJECTS = "STAT_TINT_OBJECTS";

	// Token: 0x0400121F RID: 4639
	private static int _INT_TINT_OBJECTS;

	// Token: 0x04001220 RID: 4640
	public const string UPLOAD_STEAM_WORKSHOP_ITEMS = "STAT_UPLOAD_STEAM_WORKSHOP_ITEMS";

	// Token: 0x04001221 RID: 4641
	private static int _INT_UPLOAD_STEAM_WORKSHOP_ITEMS;

	// Token: 0x04001222 RID: 4642
	public const string HOURS_PLAYED = "STAT_HOURS_PLAYED";

	// Token: 0x04001223 RID: 4643
	private static int _INT_HOURS_PLAYED;
}
