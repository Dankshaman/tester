using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200048E RID: 1166
	public class LocalizeTarget_NGUI_Texture : LocalizeTarget<UITexture>
	{
		// Token: 0x0600348F RID: 13455 RVA: 0x00160FBA File Offset: 0x0015F1BA
		static LocalizeTarget_NGUI_Texture()
		{
			LocalizeTarget_NGUI_Texture.AutoRegister();
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00160FC1 File Offset: 0x0015F1C1
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<UITexture, LocalizeTarget_NGUI_Texture>
			{
				Name = "NGUI UITexture",
				Priority = 100
			});
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x00024D16 File Offset: 0x00022F16
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x00160FE0 File Offset: 0x0015F1E0
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((this.mTarget != null && this.mTarget.mainTexture != null) ? this.mTarget.mainTexture.name : null);
			secondaryTerm = null;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x00161020 File Offset: 0x0015F220
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Texture mainTexture = this.mTarget.mainTexture;
			if (mainTexture == null || mainTexture.name != mainTranslation)
			{
				this.mTarget.mainTexture = cmp.FindTranslatedObject<Texture>(mainTranslation);
				this.mTarget.MakePixelPerfect();
			}
		}
	}
}
