using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000487 RID: 1159
	public abstract class ILocalizeTarget : ScriptableObject
	{
		// Token: 0x06003466 RID: 13414
		public abstract bool IsValid(Localize cmp);

		// Token: 0x06003467 RID: 13415
		public abstract void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		// Token: 0x06003468 RID: 13416
		public abstract void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation);

		// Token: 0x06003469 RID: 13417
		public abstract bool CanUseSecondaryTerm();

		// Token: 0x0600346A RID: 13418
		public abstract bool AllowMainTermToBeRTL();

		// Token: 0x0600346B RID: 13419
		public abstract bool AllowSecondTermToBeRTL();

		// Token: 0x0600346C RID: 13420
		public abstract eTermType GetPrimaryTermType(Localize cmp);

		// Token: 0x0600346D RID: 13421
		public abstract eTermType GetSecondaryTermType(Localize cmp);
	}
}
