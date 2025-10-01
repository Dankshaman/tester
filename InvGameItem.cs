using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000014 RID: 20
[Serializable]
public class InvGameItem
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000072 RID: 114 RVA: 0x00004091 File Offset: 0x00002291
	public int baseItemID
	{
		get
		{
			return this.mBaseItemID;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000073 RID: 115 RVA: 0x00004099 File Offset: 0x00002299
	public InvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000074 RID: 116 RVA: 0x000040BA File Offset: 0x000022BA
	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000075 RID: 117 RVA: 0x000040EC File Offset: 0x000022EC
	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				num = 0f;
				break;
			case InvGameItem.Quality.Cursed:
				num = -1f;
				break;
			case InvGameItem.Quality.Damaged:
				num = 0.25f;
				break;
			case InvGameItem.Quality.Worn:
				num = 0.9f;
				break;
			case InvGameItem.Quality.Sturdy:
				num = 1f;
				break;
			case InvGameItem.Quality.Polished:
				num = 1.1f;
				break;
			case InvGameItem.Quality.Improved:
				num = 1.25f;
				break;
			case InvGameItem.Quality.Crafted:
				num = 1.5f;
				break;
			case InvGameItem.Quality.Superior:
				num = 1.75f;
				break;
			case InvGameItem.Quality.Enchanted:
				num = 2f;
				break;
			case InvGameItem.Quality.Epic:
				num = 2.5f;
				break;
			case InvGameItem.Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000076 RID: 118 RVA: 0x000041BC File Offset: 0x000023BC
	public Color color
	{
		get
		{
			Color result = Color.white;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				result = new Color(0.4f, 0.2f, 0.2f);
				break;
			case InvGameItem.Quality.Cursed:
				result = Color.red;
				break;
			case InvGameItem.Quality.Damaged:
				result = new Color(0.4f, 0.4f, 0.4f);
				break;
			case InvGameItem.Quality.Worn:
				result = new Color(0.7f, 0.7f, 0.7f);
				break;
			case InvGameItem.Quality.Sturdy:
				result = new Color(1f, 1f, 1f);
				break;
			case InvGameItem.Quality.Polished:
				result = NGUIMath.HexToColor(3774856959U);
				break;
			case InvGameItem.Quality.Improved:
				result = NGUIMath.HexToColor(2480359935U);
				break;
			case InvGameItem.Quality.Crafted:
				result = NGUIMath.HexToColor(1325334783U);
				break;
			case InvGameItem.Quality.Superior:
				result = NGUIMath.HexToColor(12255231U);
				break;
			case InvGameItem.Quality.Enchanted:
				result = NGUIMath.HexToColor(1937178111U);
				break;
			case InvGameItem.Quality.Epic:
				result = NGUIMath.HexToColor(2516647935U);
				break;
			case InvGameItem.Quality.Legendary:
				result = NGUIMath.HexToColor(4287627519U);
				break;
			}
			return result;
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x000042DC File Offset: 0x000024DC
	public InvGameItem(int id)
	{
		this.mBaseItemID = id;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000042F9 File Offset: 0x000024F9
	public InvGameItem(int id, InvBaseItem bi)
	{
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004320 File Offset: 0x00002520
	public List<InvStat> CalculateStats()
	{
		List<InvStat> list = new List<InvStat>();
		if (this.baseItem != null)
		{
			float statMultiplier = this.statMultiplier;
			List<InvStat> stats = this.baseItem.stats;
			int i = 0;
			int count = stats.Count;
			while (i < count)
			{
				InvStat invStat = stats[i];
				int num = Mathf.RoundToInt(statMultiplier * (float)invStat.amount);
				if (num != 0)
				{
					bool flag = false;
					int j = 0;
					int count2 = list.Count;
					while (j < count2)
					{
						InvStat invStat2 = list[j];
						if (invStat2.id == invStat.id && invStat2.modifier == invStat.modifier)
						{
							invStat2.amount += num;
							flag = true;
							break;
						}
						j++;
					}
					if (!flag)
					{
						list.Add(new InvStat
						{
							id = invStat.id,
							amount = num,
							modifier = invStat.modifier
						});
					}
				}
				i++;
			}
			list.Sort(new Comparison<InvStat>(InvStat.CompareArmor));
		}
		return list;
	}

	// Token: 0x04000048 RID: 72
	[SerializeField]
	private int mBaseItemID;

	// Token: 0x04000049 RID: 73
	public InvGameItem.Quality quality = InvGameItem.Quality.Sturdy;

	// Token: 0x0400004A RID: 74
	public int itemLevel = 1;

	// Token: 0x0400004B RID: 75
	private InvBaseItem mBaseItem;

	// Token: 0x020004FC RID: 1276
	public enum Quality
	{
		// Token: 0x04002369 RID: 9065
		Broken,
		// Token: 0x0400236A RID: 9066
		Cursed,
		// Token: 0x0400236B RID: 9067
		Damaged,
		// Token: 0x0400236C RID: 9068
		Worn,
		// Token: 0x0400236D RID: 9069
		Sturdy,
		// Token: 0x0400236E RID: 9070
		Polished,
		// Token: 0x0400236F RID: 9071
		Improved,
		// Token: 0x04002370 RID: 9072
		Crafted,
		// Token: 0x04002371 RID: 9073
		Superior,
		// Token: 0x04002372 RID: 9074
		Enchanted,
		// Token: 0x04002373 RID: 9075
		Epic,
		// Token: 0x04002374 RID: 9076
		Legendary,
		// Token: 0x04002375 RID: 9077
		_LastDoNotUse
	}
}
