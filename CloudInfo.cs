using System;

// Token: 0x02000243 RID: 579
public struct CloudInfo
{
	// Token: 0x06001D06 RID: 7430 RVA: 0x000C74BC File Offset: 0x000C56BC
	public CloudInfo(string name, string uRL, int size, string date, string folder)
	{
		this.Name = name;
		this.URL = uRL;
		this.Size = size;
		this.Date = date;
		this.Folder = folder;
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x000C74E4 File Offset: 0x000C56E4
	public CloudInfo(string Name, string URL, int Size, string folder = null)
	{
		this.Name = Name;
		this.URL = URL;
		this.Size = Size;
		this.Date = DateTime.Now.ToString();
		this.Folder = folder;
	}

	// Token: 0x04001286 RID: 4742
	public string Name;

	// Token: 0x04001287 RID: 4743
	public string URL;

	// Token: 0x04001288 RID: 4744
	public int Size;

	// Token: 0x04001289 RID: 4745
	public string Date;

	// Token: 0x0400128A RID: 4746
	public string Folder;
}
