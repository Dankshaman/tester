using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000497 RID: 1175
	public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
	{
		// Token: 0x060034E0 RID: 13536 RVA: 0x00161C32 File Offset: 0x0015FE32
		static LocalizeTarget_UnityStandard_SpriteRenderer()
		{
			LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x00161C39 File Offset: 0x0015FE39
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>
			{
				Name = "SpriteRenderer",
				Priority = 100
			});
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x0015F348 File Offset: 0x0015D548
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x00161C58 File Offset: 0x0015FE58
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((this.mTarget.sprite != null) ? this.mTarget.sprite.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x00161C8C File Offset: 0x0015FE8C
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Sprite sprite = this.mTarget.sprite;
			if (sprite == null || sprite.name != mainTranslation)
			{
				this.mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
			}
		}
	}
}
