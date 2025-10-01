using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000057 RID: 87
[Serializable]
public class BMFont
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060002C5 RID: 709 RVA: 0x00012BDE File Offset: 0x00010DDE
	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060002C6 RID: 710 RVA: 0x00012BEE File Offset: 0x00010DEE
	// (set) Token: 0x060002C7 RID: 711 RVA: 0x00012BF6 File Offset: 0x00010DF6
	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060002C8 RID: 712 RVA: 0x00012BFF File Offset: 0x00010DFF
	// (set) Token: 0x060002C9 RID: 713 RVA: 0x00012C07 File Offset: 0x00010E07
	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060002CA RID: 714 RVA: 0x00012C10 File Offset: 0x00010E10
	// (set) Token: 0x060002CB RID: 715 RVA: 0x00012C18 File Offset: 0x00010E18
	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060002CC RID: 716 RVA: 0x00012C21 File Offset: 0x00010E21
	// (set) Token: 0x060002CD RID: 717 RVA: 0x00012C29 File Offset: 0x00010E29
	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060002CE RID: 718 RVA: 0x00012C32 File Offset: 0x00010E32
	public int glyphCount
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			return this.mSaved.Count;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060002CF RID: 719 RVA: 0x00012C49 File Offset: 0x00010E49
	// (set) Token: 0x060002D0 RID: 720 RVA: 0x00012C51 File Offset: 0x00010E51
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060002D1 RID: 721 RVA: 0x00012C5A File Offset: 0x00010E5A
	public List<BMGlyph> glyphs
	{
		get
		{
			return this.mSaved;
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00012C64 File Offset: 0x00010E64
	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bmglyph = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph2 = this.mSaved[i];
				this.mDict.Add(bmglyph2.index, bmglyph2);
				i++;
			}
		}
		if (!this.mDict.TryGetValue(index, out bmglyph) && createIfMissing)
		{
			bmglyph = new BMGlyph();
			bmglyph.index = index;
			this.mSaved.Add(bmglyph);
			this.mDict.Add(index, bmglyph);
		}
		return bmglyph;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00012CF3 File Offset: 0x00010EF3
	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00012CFD File Offset: 0x00010EFD
	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00012D18 File Offset: 0x00010F18
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph = this.mSaved[i];
				if (bmglyph != null)
				{
					bmglyph.Trim(xMin, yMin, xMax, yMax);
				}
				i++;
			}
		}
	}

	// Token: 0x04000270 RID: 624
	[HideInInspector]
	[SerializeField]
	private int mSize = 16;

	// Token: 0x04000271 RID: 625
	[HideInInspector]
	[SerializeField]
	private int mBase;

	// Token: 0x04000272 RID: 626
	[HideInInspector]
	[SerializeField]
	private int mWidth;

	// Token: 0x04000273 RID: 627
	[HideInInspector]
	[SerializeField]
	private int mHeight;

	// Token: 0x04000274 RID: 628
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x04000275 RID: 629
	[HideInInspector]
	[SerializeField]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	// Token: 0x04000276 RID: 630
	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
}
