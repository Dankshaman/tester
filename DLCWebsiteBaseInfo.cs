using System;
using Newtonsoft.Json;

// Token: 0x020000EC RID: 236
[Serializable]
public class DLCWebsiteBaseInfo
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00050B36 File Offset: 0x0004ED36
	[JsonIgnore]
	public string SaveTag
	{
		get
		{
			return "<" + this.Name + ">";
		}
	}

	// Token: 0x0400081E RID: 2078
	public string Name;

	// Token: 0x0400081F RID: 2079
	public string DisplayName;

	// Token: 0x04000820 RID: 2080
	public int AppId;

	// Token: 0x04000821 RID: 2081
	public string SaveURL;
}
