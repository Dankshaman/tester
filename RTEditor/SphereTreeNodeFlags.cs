using System;

namespace RTEditor
{
	// Token: 0x02000450 RID: 1104
	[Flags]
	public enum SphereTreeNodeFlags
	{
		// Token: 0x04002076 RID: 8310
		None = 0,
		// Token: 0x04002077 RID: 8311
		Root = 1,
		// Token: 0x04002078 RID: 8312
		SuperSphere = 2,
		// Token: 0x04002079 RID: 8313
		Terminal = 4,
		// Token: 0x0400207A RID: 8314
		MustRecompute = 8,
		// Token: 0x0400207B RID: 8315
		MustIntegrate = 16
	}
}
