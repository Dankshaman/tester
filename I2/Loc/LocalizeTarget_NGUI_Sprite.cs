using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200048D RID: 1165
	public class LocalizeTarget_NGUI_Sprite : LocalizeTarget<UISprite>
	{
		// Token: 0x06003485 RID: 13445 RVA: 0x00160E96 File Offset: 0x0015F096
		static LocalizeTarget_NGUI_Sprite()
		{
			LocalizeTarget_NGUI_Sprite.AutoRegister();
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00160E9D File Offset: 0x0015F09D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<UISprite, LocalizeTarget_NGUI_Sprite>
			{
				Name = "NGUI UISprite",
				Priority = 100
			});
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x0015F348 File Offset: 0x0015D548
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00160EBC File Offset: 0x0015F0BC
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.UIAtlas;
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00160EC0 File Offset: 0x0015F0C0
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.spriteName : null);
			secondaryTerm = (this.mTarget.atlas ? this.mTarget.atlas.name : string.Empty);
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00160F18 File Offset: 0x0015F118
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (this.mTarget.spriteName == mainTranslation)
			{
				return;
			}
			UIAtlas secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<UIAtlas>(ref mainTranslation, ref secondaryTranslation);
			bool flag = false;
			if (secondaryTranslatedObj != null && this.mTarget.atlas != secondaryTranslatedObj)
			{
				this.mTarget.atlas = secondaryTranslatedObj;
				flag = true;
			}
			if (this.mTarget.spriteName != mainTranslation && this.mTarget.atlas.GetSprite(mainTranslation) != null)
			{
				this.mTarget.spriteName = mainTranslation;
				flag = true;
			}
			if (flag)
			{
				this.mTarget.MakePixelPerfect();
			}
		}
	}
}
