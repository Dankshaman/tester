using System;
using System.Collections.Generic;

// Token: 0x020000FC RID: 252
public class Developer
{
	// Token: 0x06000C69 RID: 3177 RVA: 0x00054855 File Offset: 0x00052A55
	public Developer(string name, string steamID)
	{
		this.name = name;
		this.steamID = steamID;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x0005486C File Offset: 0x00052A6C
	public static bool HasName(string name)
	{
		for (int i = 0; i < Developer.Developers.Count; i++)
		{
			if (Developer.Developers[i].name == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x000548AC File Offset: 0x00052AAC
	public static bool HasSteamID(string steamID)
	{
		for (int i = 0; i < Developer.Developers.Count; i++)
		{
			if (Developer.Developers[i].steamID == steamID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000862 RID: 2146
	public string name;

	// Token: 0x04000863 RID: 2147
	public string steamID;

	// Token: 0x04000864 RID: 2148
	public static readonly List<Developer> Developers = new List<Developer>
	{
		new Developer("Knil", "76561197997553139"),
		new Developer("Chry", "76561198010185056"),
		new Developer("onelivesleft", "76561197966339174"),
		new Developer("Gikerl", "76561198007838147")
	};
}
