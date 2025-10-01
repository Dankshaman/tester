using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
	// Token: 0x02000396 RID: 918
	public static class Tags
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x00130F37 File Offset: 0x0012F137
		public static IEnumerable<string> Type { get; } = new List<string>
		{
			"Game",
			"Objects",
			"Map",
			"Utility",
			"Modding"
		};

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x00130F3E File Offset: 0x0012F13E
		public static IEnumerable<string> Complexity { get; } = new List<string>
		{
			"Low Complexity",
			"Medium Complexity",
			"High Complexity"
		};

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002B17 RID: 11031 RVA: 0x00130F45 File Offset: 0x0012F145
		public static IEnumerable<string> GameCategory { get; } = new List<string>
		{
			"Board Games",
			"Card Games",
			"Dice Games",
			"Strategy Games",
			"Role-playing Games",
			"Original Games",
			"Puzzle Games",
			"Wargames",
			"Memory Games",
			"Party Games",
			"Dexterity Games",
			"Family Games",
			"Abstract Games",
			"Thematic Games",
			"Miniature Games",
			"Cooperative Games",
			"Mature Games"
		};

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x00130F4C File Offset: 0x0012F14C
		public static IEnumerable<string> NumberPlayers { get; } = new List<string>
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10+"
		};

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06002B19 RID: 11033 RVA: 0x00130F53 File Offset: 0x0012F153
		public static IEnumerable<string> PlayTimeString { get; } = new List<string>
		{
			"10 minutes",
			"30 minutes",
			"60 minutes",
			"90 minutes",
			"120 minutes",
			"180+ minutes"
		};

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x00130F5A File Offset: 0x0012F15A
		public static IEnumerable<string> Assets { get; } = new List<string>
		{
			"Scripting",
			"Scripting: Automated",
			"Particle Effects",
			"Animations",
			"Lighting",
			"Sounds",
			"Music",
			"Components",
			"Backgrounds",
			"Dice",
			"Cards",
			"Figurines",
			"Tables",
			"Props",
			"Rules",
			"Rooms",
			"User Interfaces"
		};

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x00130F61 File Offset: 0x0012F161
		public static IEnumerable<string> Language { get; } = new List<string>
		{
			"English",
			"Chinese (Simplified)",
			"Russian",
			"Spanish",
			"Portuguese (Brazil)",
			"German",
			"French",
			"Korean",
			"Polish",
			"Turkish",
			"Japanese",
			"Chinese (Traditional)",
			"Thai",
			"Italian",
			"Portuguese",
			"Czech",
			"Hungarian",
			"Swedish",
			"Dutch",
			"Spanish (Latin America)",
			"Danish",
			"Finnish",
			"Norwegian",
			"Romanian",
			"Ukrainian",
			"Greek",
			"Vietnamese",
			"Bulgarian",
			"Arabic"
		};

		// Token: 0x04001D3F RID: 7487
		public static int[] PlayTime = new int[]
		{
			0,
			10,
			30,
			60,
			90,
			120,
			180
		};
	}
}
