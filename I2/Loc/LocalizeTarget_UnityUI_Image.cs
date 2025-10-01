using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000499 RID: 1177
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		// Token: 0x060034F4 RID: 13556 RVA: 0x00161E6D File Offset: 0x0016006D
		static LocalizeTarget_UnityUI_Image()
		{
			LocalizeTarget_UnityUI_Image.AutoRegister();
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x00161E74 File Offset: 0x00160074
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>
			{
				Name = "Image",
				Priority = 100
			});
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x00161E93 File Offset: 0x00160093
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			if (!(this.mTarget.sprite == null))
			{
				return eTermType.Sprite;
			}
			return eTermType.Texture;
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x00161EAC File Offset: 0x001600AC
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			if (this.mTarget.sprite != null && this.mTarget.sprite.name != primaryTerm)
			{
				primaryTerm = primaryTerm + "." + this.mTarget.sprite.name;
			}
			secondaryTerm = null;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x00161F38 File Offset: 0x00160138
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
