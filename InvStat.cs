using System;

// Token: 0x02000015 RID: 21
[Serializable]
public class InvStat
{
	// Token: 0x0600007A RID: 122 RVA: 0x00004433 File Offset: 0x00002633
	public static string GetName(InvStat.Identifier i)
	{
		return i.ToString();
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004444 File Offset: 0x00002644
	public static string GetDescription(InvStat.Identifier i)
	{
		switch (i)
		{
		case InvStat.Identifier.Strength:
			return "Strength increases melee damage";
		case InvStat.Identifier.Constitution:
			return "Constitution increases health";
		case InvStat.Identifier.Agility:
			return "Agility increases armor";
		case InvStat.Identifier.Intelligence:
			return "Intelligence increases mana";
		case InvStat.Identifier.Damage:
			return "Damage adds to the amount of damage done in combat";
		case InvStat.Identifier.Crit:
			return "Crit increases the chance of landing a critical strike";
		case InvStat.Identifier.Armor:
			return "Armor protects from damage";
		case InvStat.Identifier.Health:
			return "Health prolongs life";
		case InvStat.Identifier.Mana:
			return "Mana increases the number of spells that can be cast";
		default:
			return null;
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000044B4 File Offset: 0x000026B4
	public static int CompareArmor(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Armor)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Damage)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004564 File Offset: 0x00002764
	public static int CompareWeapon(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Damage)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Armor)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0400004C RID: 76
	public InvStat.Identifier id;

	// Token: 0x0400004D RID: 77
	public InvStat.Modifier modifier;

	// Token: 0x0400004E RID: 78
	public int amount;

	// Token: 0x020004FD RID: 1277
	public enum Identifier
	{
		// Token: 0x04002377 RID: 9079
		Strength,
		// Token: 0x04002378 RID: 9080
		Constitution,
		// Token: 0x04002379 RID: 9081
		Agility,
		// Token: 0x0400237A RID: 9082
		Intelligence,
		// Token: 0x0400237B RID: 9083
		Damage,
		// Token: 0x0400237C RID: 9084
		Crit,
		// Token: 0x0400237D RID: 9085
		Armor,
		// Token: 0x0400237E RID: 9086
		Health,
		// Token: 0x0400237F RID: 9087
		Mana,
		// Token: 0x04002380 RID: 9088
		Other
	}

	// Token: 0x020004FE RID: 1278
	public enum Modifier
	{
		// Token: 0x04002382 RID: 9090
		Added,
		// Token: 0x04002383 RID: 9091
		Percent
	}
}
