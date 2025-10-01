using System;

// Token: 0x0200026C RID: 620
public struct HsvColor
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x060020B4 RID: 8372 RVA: 0x000ECE59 File Offset: 0x000EB059
	// (set) Token: 0x060020B5 RID: 8373 RVA: 0x000ECE67 File Offset: 0x000EB067
	public float normalizedH
	{
		get
		{
			return this.H / 360f;
		}
		set
		{
			this.H = value * 360f;
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x060020B6 RID: 8374 RVA: 0x000ECE76 File Offset: 0x000EB076
	// (set) Token: 0x060020B7 RID: 8375 RVA: 0x000ECE7E File Offset: 0x000EB07E
	public float normalizedS
	{
		get
		{
			return this.S;
		}
		set
		{
			this.S = value;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000ECE87 File Offset: 0x000EB087
	// (set) Token: 0x060020B9 RID: 8377 RVA: 0x000ECE8F File Offset: 0x000EB08F
	public float normalizedV
	{
		get
		{
			return this.V;
		}
		set
		{
			this.V = value;
		}
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x000ECE98 File Offset: 0x000EB098
	public HsvColor(float h, float s, float v, float a)
	{
		this.H = h;
		this.S = s;
		this.V = v;
		this.A = a;
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x000ECEB8 File Offset: 0x000EB0B8
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"{",
			this.H.ToString("f2"),
			",",
			this.S.ToString("f2"),
			",",
			this.V.ToString("f2"),
			"}"
		});
	}

	// Token: 0x0400143A RID: 5178
	public float H;

	// Token: 0x0400143B RID: 5179
	public float S;

	// Token: 0x0400143C RID: 5180
	public float V;

	// Token: 0x0400143D RID: 5181
	public float A;
}
