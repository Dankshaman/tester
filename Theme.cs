using System;
using System.Collections.Generic;

// Token: 0x02000311 RID: 785
public class Theme
{
	// Token: 0x06002629 RID: 9769 RVA: 0x0010CF9B File Offset: 0x0010B19B
	public Theme(int id, string name, Dictionary<UIPalette.UI, Colour> colours)
	{
		this.id = id;
		this.name = name;
		this.colours = colours;
	}

	// Token: 0x040018D1 RID: 6353
	public int id;

	// Token: 0x040018D2 RID: 6354
	public string name;

	// Token: 0x040018D3 RID: 6355
	public Dictionary<UIPalette.UI, Colour> colours;
}
