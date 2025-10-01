using System;
using System.Collections.Generic;

// Token: 0x02000058 RID: 88
[Serializable]
public class BMGlyph
{
	// Token: 0x060002D7 RID: 727 RVA: 0x00012D88 File Offset: 0x00010F88
	public int GetKerning(int previousChar)
	{
		if (this.kerning != null && previousChar != 0)
		{
			int i = 0;
			int count = this.kerning.Count;
			while (i < count)
			{
				if (this.kerning[i] == previousChar)
				{
					return this.kerning[i + 1];
				}
				i += 2;
			}
		}
		return 0;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00012DD8 File Offset: 0x00010FD8
	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00012E48 File Offset: 0x00011048
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num2 = this.y + this.height;
		if (this.x < xMin)
		{
			int num3 = xMin - this.x;
			this.x += num3;
			this.width -= num3;
			this.offsetX += num3;
		}
		if (this.y < yMin)
		{
			int num4 = yMin - this.y;
			this.y += num4;
			this.height -= num4;
			this.offsetY += num4;
		}
		if (num > xMax)
		{
			this.width -= num - xMax;
		}
		if (num2 > yMax)
		{
			this.height -= num2 - yMax;
		}
	}

	// Token: 0x04000277 RID: 631
	public int index;

	// Token: 0x04000278 RID: 632
	public int x;

	// Token: 0x04000279 RID: 633
	public int y;

	// Token: 0x0400027A RID: 634
	public int width;

	// Token: 0x0400027B RID: 635
	public int height;

	// Token: 0x0400027C RID: 636
	public int offsetX;

	// Token: 0x0400027D RID: 637
	public int offsetY;

	// Token: 0x0400027E RID: 638
	public int advance;

	// Token: 0x0400027F RID: 639
	public int channel;

	// Token: 0x04000280 RID: 640
	public List<int> kerning;
}
