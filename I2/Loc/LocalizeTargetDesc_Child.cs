using System;

namespace I2.Loc
{
	// Token: 0x02000492 RID: 1170
	public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
	{
		// Token: 0x060034BB RID: 13499 RVA: 0x001618C5 File Offset: 0x0015FAC5
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}
	}
}
