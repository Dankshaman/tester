using System;

namespace Discord
{
	// Token: 0x020004E6 RID: 1254
	public class ResultException : Exception
	{
		// Token: 0x06003613 RID: 13843 RVA: 0x00166755 File Offset: 0x00164955
		public ResultException(Result result) : base(result.ToString())
		{
		}

		// Token: 0x040022DB RID: 8923
		public readonly Result Result;
	}
}
