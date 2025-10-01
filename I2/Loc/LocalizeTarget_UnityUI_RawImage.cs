using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x0200049A RID: 1178
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		// Token: 0x060034FE RID: 13566 RVA: 0x00161F82 File Offset: 0x00160182
		static LocalizeTarget_UnityUI_RawImage()
		{
			LocalizeTarget_UnityUI_RawImage.AutoRegister();
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x00161F89 File Offset: 0x00160189
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>
			{
				Name = "RawImage",
				Priority = 100
			});
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x00024D16 File Offset: 0x00022F16
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x00161FA8 File Offset: 0x001601A8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			secondaryTerm = null;
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x00161FDC File Offset: 0x001601DC
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Texture texture = this.mTarget.texture;
			if (texture == null || texture.name != mainTranslation)
			{
				this.mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
			}
		}
	}
}
