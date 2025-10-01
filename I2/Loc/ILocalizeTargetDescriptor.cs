using System;

namespace I2.Loc
{
	// Token: 0x02000489 RID: 1161
	public abstract class ILocalizeTargetDescriptor
	{
		// Token: 0x06003471 RID: 13425
		public abstract bool CanLocalize(Localize cmp);

		// Token: 0x06003472 RID: 13426
		public abstract ILocalizeTarget CreateTarget(Localize cmp);

		// Token: 0x06003473 RID: 13427
		public abstract Type GetTargetType();

		// Token: 0x04002169 RID: 8553
		public string Name;

		// Token: 0x0400216A RID: 8554
		public int Priority;
	}
}
