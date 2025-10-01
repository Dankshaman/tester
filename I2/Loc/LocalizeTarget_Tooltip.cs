using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000463 RID: 1123
	public class LocalizeTarget_Tooltip : LocalizeTarget<UITooltipScript>
	{
		// Token: 0x06003302 RID: 13058 RVA: 0x00155162 File Offset: 0x00153362
		static LocalizeTarget_Tooltip()
		{
			LocalizeTarget_Tooltip.AutoRegister();
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x00155169 File Offset: 0x00153369
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<UITooltipScript, LocalizeTarget_Tooltip>
			{
				Name = "UI Tooltip",
				Priority = 101
			});
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x00155188 File Offset: 0x00153388
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.Tooltip : null);
			secondaryTerm = (this.mTarget ? this.mTarget.DelayTooltip : null);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x001551C6 File Offset: 0x001533C6
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			this.mTarget.Tooltip = mainTranslation;
			this.mTarget.DelayTooltip = secondaryTranslation;
		}
	}
}
