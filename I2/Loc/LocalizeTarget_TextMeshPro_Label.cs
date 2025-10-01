using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200048F RID: 1167
	public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
	{
		// Token: 0x06003499 RID: 13465 RVA: 0x00161075 File Offset: 0x0015F275
		static LocalizeTarget_TextMeshPro_Label()
		{
			LocalizeTarget_TextMeshPro_Label.AutoRegister();
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x0016107C File Offset: 0x0015F27C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label>
			{
				Name = "TextMeshPro Label",
				Priority = 100
			});
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x00014D66 File Offset: 0x00012F66
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x0016109C File Offset: 0x0015F29C
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x001610F4 File Offset: 0x0015F2F4
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TMP_FontAsset tmp_FontAsset = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (tmp_FontAsset != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
			}
			else
			{
				Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget.fontMaterial != secondaryTranslatedObj)
				{
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget.font.name, StringComparison.Ordinal))
					{
						tmp_FontAsset = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, secondaryTranslation.EndsWith(secondaryTranslatedObj.name, StringComparison.Ordinal) ? secondaryTranslation : secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
						{
							LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
						}
					}
					LocalizeTarget_TextMeshPro_Label.SetMaterial(this.mTarget, secondaryTranslatedObj);
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAlignmentOptions textAlignmentOptions;
				TextAlignmentOptions textAlignmentOptions2;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out textAlignmentOptions, out textAlignmentOptions2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAlignmentOptions2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAlignmentOptions))
				{
					this.mAlignment_LTR = textAlignmentOptions;
					this.mAlignment_RTL = textAlignmentOptions2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (mainTranslation != null && cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
					this.mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
					if (LocalizationManager.IsRight2Left)
					{
						mainTranslation = I2Utils.ReverseText(mainTranslation);
					}
				}
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x001612AC File Offset: 0x0015F4AC
		internal static TMP_FontAsset GetTMPFontFromMaterial(Localize cmp, string matName)
		{
			string text = " .\\/-[]()";
			int i = matName.Length - 1;
			while (i > 0)
			{
				while (i > 0 && text.IndexOf(matName[i]) >= 0)
				{
					i--;
				}
				if (i <= 0)
				{
					break;
				}
				string translation = matName.Substring(0, i + 1);
				TMP_FontAsset @object = cmp.GetObject<TMP_FontAsset>(translation);
				if (@object != null)
				{
					return @object;
				}
				while (i > 0 && text.IndexOf(matName[i]) < 0)
				{
					i--;
				}
			}
			return null;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00161324 File Offset: 0x0015F524
		internal static void InitAlignment_TMPro(bool isRTL, TextAlignmentOptions alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				if (alignment <= TextAlignmentOptions.BottomRight)
				{
					if (alignment <= TextAlignmentOptions.Left)
					{
						if (alignment == TextAlignmentOptions.TopLeft)
						{
							alignLTR = TextAlignmentOptions.TopRight;
							return;
						}
						if (alignment == TextAlignmentOptions.TopRight)
						{
							alignLTR = TextAlignmentOptions.TopLeft;
							return;
						}
						if (alignment != TextAlignmentOptions.Left)
						{
							return;
						}
						alignLTR = TextAlignmentOptions.Right;
						return;
					}
					else
					{
						if (alignment == TextAlignmentOptions.Right)
						{
							alignLTR = TextAlignmentOptions.Left;
							return;
						}
						if (alignment == TextAlignmentOptions.BottomLeft)
						{
							alignLTR = TextAlignmentOptions.BottomRight;
							return;
						}
						if (alignment != TextAlignmentOptions.BottomRight)
						{
							return;
						}
						alignLTR = TextAlignmentOptions.BottomLeft;
						return;
					}
				}
				else if (alignment <= TextAlignmentOptions.MidlineLeft)
				{
					if (alignment == TextAlignmentOptions.BaselineLeft)
					{
						alignLTR = TextAlignmentOptions.BaselineRight;
						return;
					}
					if (alignment == TextAlignmentOptions.BaselineRight)
					{
						alignLTR = TextAlignmentOptions.BaselineLeft;
						return;
					}
					if (alignment != TextAlignmentOptions.MidlineLeft)
					{
						return;
					}
					alignLTR = TextAlignmentOptions.MidlineRight;
					return;
				}
				else
				{
					if (alignment == TextAlignmentOptions.MidlineRight)
					{
						alignLTR = TextAlignmentOptions.MidlineLeft;
						return;
					}
					if (alignment == TextAlignmentOptions.CaplineLeft)
					{
						alignLTR = TextAlignmentOptions.CaplineRight;
						return;
					}
					if (alignment != TextAlignmentOptions.CaplineRight)
					{
						return;
					}
					alignLTR = TextAlignmentOptions.CaplineLeft;
					return;
				}
			}
			else if (alignment <= TextAlignmentOptions.BottomRight)
			{
				if (alignment <= TextAlignmentOptions.Left)
				{
					if (alignment == TextAlignmentOptions.TopLeft)
					{
						alignRTL = TextAlignmentOptions.TopRight;
						return;
					}
					if (alignment == TextAlignmentOptions.TopRight)
					{
						alignRTL = TextAlignmentOptions.TopLeft;
						return;
					}
					if (alignment != TextAlignmentOptions.Left)
					{
						return;
					}
					alignRTL = TextAlignmentOptions.Right;
					return;
				}
				else
				{
					if (alignment == TextAlignmentOptions.Right)
					{
						alignRTL = TextAlignmentOptions.Left;
						return;
					}
					if (alignment == TextAlignmentOptions.BottomLeft)
					{
						alignRTL = TextAlignmentOptions.BottomRight;
						return;
					}
					if (alignment != TextAlignmentOptions.BottomRight)
					{
						return;
					}
					alignRTL = TextAlignmentOptions.BottomLeft;
					return;
				}
			}
			else if (alignment <= TextAlignmentOptions.MidlineLeft)
			{
				if (alignment == TextAlignmentOptions.BaselineLeft)
				{
					alignRTL = TextAlignmentOptions.BaselineRight;
					return;
				}
				if (alignment == TextAlignmentOptions.BaselineRight)
				{
					alignRTL = TextAlignmentOptions.BaselineLeft;
					return;
				}
				if (alignment != TextAlignmentOptions.MidlineLeft)
				{
					return;
				}
				alignRTL = TextAlignmentOptions.MidlineRight;
				return;
			}
			else
			{
				if (alignment == TextAlignmentOptions.MidlineRight)
				{
					alignRTL = TextAlignmentOptions.MidlineLeft;
					return;
				}
				if (alignment == TextAlignmentOptions.CaplineLeft)
				{
					alignRTL = TextAlignmentOptions.CaplineRight;
					return;
				}
				if (alignment != TextAlignmentOptions.CaplineRight)
				{
					return;
				}
				alignRTL = TextAlignmentOptions.CaplineLeft;
				return;
			}
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x00161508 File Offset: 0x0015F708
		internal static void SetFont(TMP_Text label, TMP_FontAsset newFont)
		{
			if (label.font != newFont)
			{
				label.font = newFont;
			}
			if (label.linkedTextComponent != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(label.linkedTextComponent, newFont);
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00161539 File Offset: 0x0015F739
		internal static void SetMaterial(TMP_Text label, Material newMat)
		{
			if (label.fontSharedMaterial != newMat)
			{
				label.fontSharedMaterial = newMat;
			}
			if (label.linkedTextComponent != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetMaterial(label.linkedTextComponent, newMat);
			}
		}

		// Token: 0x0400216F RID: 8559
		private TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		// Token: 0x04002170 RID: 8560
		private TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		// Token: 0x04002171 RID: 8561
		private bool mAlignmentWasRTL;

		// Token: 0x04002172 RID: 8562
		private bool mInitializeAlignment = true;
	}
}
