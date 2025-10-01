using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000498 RID: 1176
	public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
	{
		// Token: 0x060034EA RID: 13546 RVA: 0x00161CD6 File Offset: 0x0015FED6
		static LocalizeTarget_UnityStandard_TextMesh()
		{
			LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x00161CDD File Offset: 0x0015FEDD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>
			{
				Name = "TextMesh",
				Priority = 100
			});
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x00014D66 File Offset: 0x00012F66
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x00161CFC File Offset: 0x0015FEFC
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((string.IsNullOrEmpty(Secondary) && this.mTarget.font != null) ? this.mTarget.font.name : null);
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x00161D58 File Offset: 0x0015FF58
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.font != secondaryTranslatedObj)
			{
				this.mTarget.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignment_LTR = (this.mAlignment_RTL = this.mTarget.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == TextAlignment.Right)
				{
					this.mAlignment_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == TextAlignment.Left)
				{
					this.mAlignment_RTL = TextAlignment.Right;
				}
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && this.mTarget.alignment != TextAlignment.Center)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.font.RequestCharactersInTexture(mainTranslation);
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x04002177 RID: 8567
		private TextAlignment mAlignment_RTL = TextAlignment.Right;

		// Token: 0x04002178 RID: 8568
		private TextAlignment mAlignment_LTR;

		// Token: 0x04002179 RID: 8569
		private bool mAlignmentWasRTL;

		// Token: 0x0400217A RID: 8570
		private bool mInitializeAlignment = true;
	}
}
