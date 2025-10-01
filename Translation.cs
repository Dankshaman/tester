using System;
using System.IO;

// Token: 0x0200013B RID: 315
public class Translation
{
	// Token: 0x170002EC RID: 748
	// (get) Token: 0x0600104D RID: 4173 RVA: 0x0006F960 File Offset: 0x0006DB60
	public bool fromWorkshop
	{
		get
		{
			return this.workshopID > 0UL;
		}
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0006F96C File Offset: 0x0006DB6C
	public Translation(string code, string title, string author, string filename, bool fromWorkshop)
	{
		this.code = code;
		this.title = title;
		this.author = author;
		this.filename = filename;
		this.workshopID = 0UL;
		ulong num;
		if (fromWorkshop && ulong.TryParse(Path.GetFileNameWithoutExtension(filename), out num))
		{
			this.workshopID = num;
		}
	}

	// Token: 0x04000A52 RID: 2642
	public string code;

	// Token: 0x04000A53 RID: 2643
	public string title;

	// Token: 0x04000A54 RID: 2644
	public string author;

	// Token: 0x04000A55 RID: 2645
	public string filename;

	// Token: 0x04000A56 RID: 2646
	public ulong workshopID;
}
