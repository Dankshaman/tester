using System;

namespace I2.Loc
{
	// Token: 0x02000476 RID: 1142
	public class TranslationJob : IDisposable
	{
		// Token: 0x0600337C RID: 13180 RVA: 0x0015AD32 File Offset: 0x00158F32
		public virtual TranslationJob.eJobState GetState()
		{
			return this.mJobState;
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void Dispose()
		{
		}

		// Token: 0x040020E5 RID: 8421
		public TranslationJob.eJobState mJobState;

		// Token: 0x02000818 RID: 2072
		public enum eJobState
		{
			// Token: 0x04002E37 RID: 11831
			Running,
			// Token: 0x04002E38 RID: 11832
			Succeeded,
			// Token: 0x04002E39 RID: 11833
			Failed
		}
	}
}
