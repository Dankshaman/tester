using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000011 RID: 17
[Serializable]
public class InvBaseItem
{
	// Token: 0x04000036 RID: 54
	public int id16;

	// Token: 0x04000037 RID: 55
	public string name;

	// Token: 0x04000038 RID: 56
	public string description;

	// Token: 0x04000039 RID: 57
	public InvBaseItem.Slot slot;

	// Token: 0x0400003A RID: 58
	public int minItemLevel = 1;

	// Token: 0x0400003B RID: 59
	public int maxItemLevel = 50;

	// Token: 0x0400003C RID: 60
	public List<InvStat> stats = new List<InvStat>();

	// Token: 0x0400003D RID: 61
	public GameObject attachment;

	// Token: 0x0400003E RID: 62
	public Color color = Color.white;

	// Token: 0x0400003F RID: 63
	public UIAtlas iconAtlas;

	// Token: 0x04000040 RID: 64
	public string iconName = "";

	// Token: 0x020004FB RID: 1275
	public enum Slot
	{
		// Token: 0x0400235F RID: 9055
		None,
		// Token: 0x04002360 RID: 9056
		Weapon,
		// Token: 0x04002361 RID: 9057
		Shield,
		// Token: 0x04002362 RID: 9058
		Body,
		// Token: 0x04002363 RID: 9059
		Shoulders,
		// Token: 0x04002364 RID: 9060
		Bracers,
		// Token: 0x04002365 RID: 9061
		Boots,
		// Token: 0x04002366 RID: 9062
		Trinket,
		// Token: 0x04002367 RID: 9063
		_LastDoNotUse
	}
}
