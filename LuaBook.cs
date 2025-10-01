using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class LuaBook : LuaComponent
{
	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001184 RID: 4484 RVA: 0x00078F24 File Offset: 0x00077124
	private CustomPDF pdf
	{
		get
		{
			return this.LGOS.NPO.customPDF;
		}
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaBook(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06001186 RID: 4486 RVA: 0x00078F36 File Offset: 0x00077136
	// (set) Token: 0x06001187 RID: 4487 RVA: 0x00078F52 File Offset: 0x00077152
	public int page_offset
	{
		get
		{
			if (!this.pdf)
			{
				return 0;
			}
			return this.pdf.PageDisplayOffset;
		}
		set
		{
			if (this.pdf && this.pdf.PageDisplayOffset != value)
			{
				this.pdf.PageDisplayOffset = value;
			}
		}
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00078F7B File Offset: 0x0007717B
	public int GetPage(bool use_page_offset = false)
	{
		if (!this.pdf)
		{
			return 0;
		}
		if (use_page_offset)
		{
			return this.pdf.CurrentPDFPage + this.pdf.PageDisplayOffset + 1;
		}
		return this.pdf.CurrentPDFPage;
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x00078FB4 File Offset: 0x000771B4
	public bool SetPage(int page, bool use_page_offset = false)
	{
		if (this.pdf)
		{
			if (use_page_offset)
			{
				page -= this.pdf.PageDisplayOffset + 1;
			}
			if (this.pdf.CurrentPDFPage != page && this.pdf.HasPage(page))
			{
				this.pdf.CurrentPDFPage = page;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x0007900D File Offset: 0x0007720D
	public bool SetHighlight(float x1, float y1, float x2, float y2)
	{
		if (this.pdf)
		{
			this.pdf.HighlightBox = new Vector4(x1, y1, x2, y2);
			return true;
		}
		return false;
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x00079034 File Offset: 0x00077234
	public bool ClearHighlight()
	{
		if (this.pdf)
		{
			this.pdf.HighlightBox = Vector4.zero;
			return true;
		}
		return false;
	}
}
