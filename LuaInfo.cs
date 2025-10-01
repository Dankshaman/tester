using System;
using System.Collections.Generic;

// Token: 0x0200019D RID: 413
public class LuaInfo
{
	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001433 RID: 5171 RVA: 0x00084E1C File Offset: 0x0008301C
	private GameOptions gameOptions
	{
		get
		{
			return NetworkSingleton<GameOptions>.Instance;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001434 RID: 5172 RVA: 0x00084E23 File Offset: 0x00083023
	// (set) Token: 0x06001435 RID: 5173 RVA: 0x00084E30 File Offset: 0x00083030
	public string name
	{
		get
		{
			return this.gameOptions.GameName;
		}
		set
		{
			this.gameOptions.GameName = value;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001436 RID: 5174 RVA: 0x00084E3E File Offset: 0x0008303E
	// (set) Token: 0x06001437 RID: 5175 RVA: 0x00084E4B File Offset: 0x0008304B
	public string type
	{
		get
		{
			return this.gameOptions.GameType;
		}
		set
		{
			this.gameOptions.GameType = value;
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001438 RID: 5176 RVA: 0x00084E59 File Offset: 0x00083059
	// (set) Token: 0x06001439 RID: 5177 RVA: 0x00084E66 File Offset: 0x00083066
	public string complexity
	{
		get
		{
			return this.gameOptions.GameComplexity;
		}
		set
		{
			this.gameOptions.GameComplexity = value;
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x0600143A RID: 5178 RVA: 0x00084E74 File Offset: 0x00083074
	// (set) Token: 0x0600143B RID: 5179 RVA: 0x00084E81 File Offset: 0x00083081
	public int[] playing_time
	{
		get
		{
			return this.gameOptions.PlayingTime;
		}
		set
		{
			this.gameOptions.PlayingTime = value;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x0600143C RID: 5180 RVA: 0x00084E8F File Offset: 0x0008308F
	// (set) Token: 0x0600143D RID: 5181 RVA: 0x00084E9C File Offset: 0x0008309C
	public int[] number_of_players
	{
		get
		{
			return this.gameOptions.PlayerCounts;
		}
		set
		{
			this.gameOptions.PlayerCounts = value;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x0600143E RID: 5182 RVA: 0x00084EAA File Offset: 0x000830AA
	// (set) Token: 0x0600143F RID: 5183 RVA: 0x00084EB7 File Offset: 0x000830B7
	public List<string> tags
	{
		get
		{
			return this.gameOptions.Tags;
		}
		set
		{
			this.gameOptions.Tags = value;
		}
	}
}
