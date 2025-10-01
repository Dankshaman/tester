using System;

// Token: 0x02000090 RID: 144
[Serializable]
public class UISpriteData
{
	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00036389 File Offset: 0x00034589
	public bool hasBorder
	{
		get
		{
			return (this.borderLeft | this.borderRight | this.borderTop | this.borderBottom) != 0;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060007C1 RID: 1985 RVA: 0x000363A9 File Offset: 0x000345A9
	public bool hasPadding
	{
		get
		{
			return (this.paddingLeft | this.paddingRight | this.paddingTop | this.paddingBottom) != 0;
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000363C9 File Offset: 0x000345C9
	public void SetRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000363E8 File Offset: 0x000345E8
	public void SetPadding(int left, int bottom, int right, int top)
	{
		this.paddingLeft = left;
		this.paddingBottom = bottom;
		this.paddingRight = right;
		this.paddingTop = top;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00036407 File Offset: 0x00034607
	public void SetBorder(int left, int bottom, int right, int top)
	{
		this.borderLeft = left;
		this.borderBottom = bottom;
		this.borderRight = right;
		this.borderTop = top;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00036428 File Offset: 0x00034628
	public void CopyFrom(UISpriteData sd)
	{
		this.name = sd.name;
		this.x = sd.x;
		this.y = sd.y;
		this.width = sd.width;
		this.height = sd.height;
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
		this.paddingLeft = sd.paddingLeft;
		this.paddingRight = sd.paddingRight;
		this.paddingTop = sd.paddingTop;
		this.paddingBottom = sd.paddingBottom;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000364D1 File Offset: 0x000346D1
	public void CopyBorderFrom(UISpriteData sd)
	{
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
	}

	// Token: 0x0400054F RID: 1359
	public string name = "Sprite";

	// Token: 0x04000550 RID: 1360
	public int x;

	// Token: 0x04000551 RID: 1361
	public int y;

	// Token: 0x04000552 RID: 1362
	public int width;

	// Token: 0x04000553 RID: 1363
	public int height;

	// Token: 0x04000554 RID: 1364
	public int borderLeft;

	// Token: 0x04000555 RID: 1365
	public int borderRight;

	// Token: 0x04000556 RID: 1366
	public int borderTop;

	// Token: 0x04000557 RID: 1367
	public int borderBottom;

	// Token: 0x04000558 RID: 1368
	public int paddingLeft;

	// Token: 0x04000559 RID: 1369
	public int paddingRight;

	// Token: 0x0400055A RID: 1370
	public int paddingTop;

	// Token: 0x0400055B RID: 1371
	public int paddingBottom;
}
