using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200048C RID: 1164
	public class LocalizeTarget_NGUI_Label : LocalizeTarget<UILabel>
	{
		// Token: 0x0600347B RID: 13435 RVA: 0x00160C32 File Offset: 0x0015EE32
		static LocalizeTarget_NGUI_Label()
		{
			LocalizeTarget_NGUI_Label.AutoRegister();
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x00160C39 File Offset: 0x0015EE39
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<UILabel, LocalizeTarget_NGUI_Label>
			{
				Name = "NGUI Label",
				Priority = 100
			});
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x0014CFE3 File Offset: 0x0014B1E3
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.UIFont;
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x00160C58 File Offset: 0x0015EE58
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.ambigiousFont != null) ? this.mTarget.ambigiousFont.name : string.Empty);
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00160CB0 File Offset: 0x0015EEB0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && secondaryTranslatedObj != this.mTarget.ambigiousFont)
			{
				this.mTarget.ambigiousFont = secondaryTranslatedObj;
			}
			if (secondaryTranslatedObj == null)
			{
				UIFont secondaryTranslatedObj2 = cmp.GetSecondaryTranslatedObj<UIFont>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj2 != null && this.mTarget.ambigiousFont != secondaryTranslatedObj2)
				{
					this.mTarget.ambigiousFont = secondaryTranslatedObj2;
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignment_LTR = (this.mAlignment_RTL = this.mTarget.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == NGUIText.Alignment.Right)
				{
					this.mAlignment_LTR = NGUIText.Alignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == NGUIText.Alignment.Left)
				{
					this.mAlignment_RTL = NGUIText.Alignment.Right;
				}
			}
			UIInput uiinput = NGUITools.FindInParents<UIInput>(this.mTarget.gameObject);
			if (uiinput != null && uiinput.label == this.mTarget)
			{
				if (mainTranslation != null && uiinput.defaultText != mainTranslation)
				{
					if (cmp.CorrectAlignmentForRTL && (uiinput.label.alignment == NGUIText.Alignment.Left || uiinput.label.alignment == NGUIText.Alignment.Right))
					{
						uiinput.label.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
					}
					uiinput.defaultText = mainTranslation;
					return;
				}
			}
			else if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && (this.mTarget.alignment == NGUIText.Alignment.Left || this.mTarget.alignment == NGUIText.Alignment.Right))
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x0400216B RID: 8555
		private NGUIText.Alignment mAlignment_RTL = NGUIText.Alignment.Right;

		// Token: 0x0400216C RID: 8556
		private NGUIText.Alignment mAlignment_LTR = NGUIText.Alignment.Left;

		// Token: 0x0400216D RID: 8557
		private bool mAlignmentWasRTL;

		// Token: 0x0400216E RID: 8558
		private bool mInitializeAlignment = true;
	}
}
