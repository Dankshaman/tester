using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000089 RID: 137
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Label")]
public class UILabel : UIWidget
{
	// Token: 0x060006AF RID: 1711 RVA: 0x0003045E File Offset: 0x0002E65E
	public void DoNotInitTheme()
	{
		this.BeenInit = true;
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00030467 File Offset: 0x0002E667
	public int finalFontSize
	{
		get
		{
			if (this.trueTypeFont)
			{
				return Mathf.RoundToInt(this.mScale * (float)this.mFinalFontSize);
			}
			return Mathf.RoundToInt((float)this.mFinalFontSize * this.mScale);
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0003049D File Offset: 0x0002E69D
	// (set) Token: 0x060006B2 RID: 1714 RVA: 0x000304A5 File Offset: 0x0002E6A5
	private bool shouldBeProcessed
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
				return;
			}
			this.mShouldBeProcessed = false;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x060006B3 RID: 1715 RVA: 0x000304C0 File Offset: 0x0002E6C0
	public override bool isAnchoredHorizontally
	{
		get
		{
			return base.isAnchoredHorizontally || this.mOverflow == UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000304D5 File Offset: 0x0002E6D5
	public override bool isAnchoredVertically
	{
		get
		{
			return base.isAnchoredVertically || this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060006B5 RID: 1717 RVA: 0x000304F4 File Offset: 0x0002E6F4
	// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0003054B File Offset: 0x0002E74B
	public override Material material
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			base.material = value;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00030554 File Offset: 0x0002E754
	// (set) Token: 0x060006B8 RID: 1720 RVA: 0x000305A6 File Offset: 0x0002E7A6
	public override Texture mainTexture
	{
		get
		{
			if (this.mFont != null)
			{
				return this.mFont.texture;
			}
			if (this.mTrueTypeFont != null)
			{
				Material material = this.mTrueTypeFont.material;
				if (material != null)
				{
					return material.mainTexture;
				}
			}
			return null;
		}
		set
		{
			base.mainTexture = value;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000305AF File Offset: 0x0002E7AF
	// (set) Token: 0x060006BA RID: 1722 RVA: 0x000305B7 File Offset: 0x0002E7B7
	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060006BB RID: 1723 RVA: 0x000305C0 File Offset: 0x0002E7C0
	// (set) Token: 0x060006BC RID: 1724 RVA: 0x000305C8 File Offset: 0x0002E7C8
	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				base.RemoveFromPanel();
				this.mFont = value;
				this.mTrueTypeFont = null;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x000305F2 File Offset: 0x0002E7F2
	// (set) Token: 0x060006BE RID: 1726 RVA: 0x00030624 File Offset: 0x0002E824
	public Font trueTypeFont
	{
		get
		{
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont;
			}
			if (!(this.mFont != null))
			{
				return null;
			}
			return this.mFont.dynamicFont;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.shouldBeProcessed = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x060006BF RID: 1727 RVA: 0x00030682 File Offset: 0x0002E882
	// (set) Token: 0x060006C0 RID: 1728 RVA: 0x00030694 File Offset: 0x0002E894
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			return this.mFont ?? this.mTrueTypeFont;
		}
		set
		{
			UIFont uifont = value as UIFont;
			if (uifont != null)
			{
				this.bitmapFont = uifont;
				return;
			}
			this.trueTypeFont = (value as Font);
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x060006C1 RID: 1729 RVA: 0x000306C5 File Offset: 0x0002E8C5
	// (set) Token: 0x060006C2 RID: 1730 RVA: 0x000306D0 File Offset: 0x0002E8D0
	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (this.mText == value)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = "";
					this.MarkAsChanged();
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
			if (this.autoResizeBoxCollider)
			{
				base.ResizeCollider();
			}
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00030749 File Offset: 0x0002E949
	public int defaultFontSize
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.mFontSize;
			}
			if (!(this.mFont != null))
			{
				return 16;
			}
			return this.mFont.defaultSize;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0003077C File Offset: 0x0002E97C
	// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00030784 File Offset: 0x0002E984
	public int fontSize
	{
		get
		{
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 256);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x060006C6 RID: 1734 RVA: 0x000307B1 File Offset: 0x0002E9B1
	// (set) Token: 0x060006C7 RID: 1735 RVA: 0x000307B9 File Offset: 0x0002E9B9
	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x060006C8 RID: 1736 RVA: 0x000307D8 File Offset: 0x0002E9D8
	// (set) Token: 0x060006C9 RID: 1737 RVA: 0x000307E0 File Offset: 0x0002E9E0
	public NGUIText.Alignment alignment
	{
		get
		{
			return this.mAlignment;
		}
		set
		{
			if (this.mAlignment != value)
			{
				this.mAlignment = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060006CA RID: 1738 RVA: 0x000307FF File Offset: 0x0002E9FF
	// (set) Token: 0x060006CB RID: 1739 RVA: 0x00030807 File Offset: 0x0002EA07
	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060006CC RID: 1740 RVA: 0x0003081F File Offset: 0x0002EA1F
	// (set) Token: 0x060006CD RID: 1741 RVA: 0x00030827 File Offset: 0x0002EA27
	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060006CE RID: 1742 RVA: 0x0003084C File Offset: 0x0002EA4C
	// (set) Token: 0x060006CF RID: 1743 RVA: 0x00030854 File Offset: 0x0002EA54
	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00030879 File Offset: 0x0002EA79
	// (set) Token: 0x060006D1 RID: 1745 RVA: 0x00030881 File Offset: 0x0002EA81
	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00030899 File Offset: 0x0002EA99
	// (set) Token: 0x060006D3 RID: 1747 RVA: 0x000308A1 File Offset: 0x0002EAA1
	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x000308B9 File Offset: 0x0002EAB9
	// (set) Token: 0x060006D5 RID: 1749 RVA: 0x000308C1 File Offset: 0x0002EAC1
	public bool useFloatSpacing
	{
		get
		{
			return this.mUseFloatSpacing;
		}
		set
		{
			if (this.mUseFloatSpacing != value)
			{
				this.mUseFloatSpacing = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000308DA File Offset: 0x0002EADA
	// (set) Token: 0x060006D7 RID: 1751 RVA: 0x000308E2 File Offset: 0x0002EAE2
	public float floatSpacingX
	{
		get
		{
			return this.mFloatSpacingX;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingX, value))
			{
				this.mFloatSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060006D8 RID: 1752 RVA: 0x000308FF File Offset: 0x0002EAFF
	// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00030907 File Offset: 0x0002EB07
	public float floatSpacingY
	{
		get
		{
			return this.mFloatSpacingY;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingY, value))
			{
				this.mFloatSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060006DA RID: 1754 RVA: 0x00030924 File Offset: 0x0002EB24
	public float effectiveSpacingY
	{
		get
		{
			if (!this.mUseFloatSpacing)
			{
				return (float)this.mSpacingY;
			}
			return this.mFloatSpacingY;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060006DB RID: 1755 RVA: 0x0003093C File Offset: 0x0002EB3C
	public float effectiveSpacingX
	{
		get
		{
			if (!this.mUseFloatSpacing)
			{
				return (float)this.mSpacingX;
			}
			return this.mFloatSpacingX;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x00030954 File Offset: 0x0002EB54
	// (set) Token: 0x060006DD RID: 1757 RVA: 0x0003095C File Offset: 0x0002EB5C
	public bool overflowEllipsis
	{
		get
		{
			return this.mOverflowEllipsis;
		}
		set
		{
			if (this.mOverflowEllipsis != value)
			{
				this.mOverflowEllipsis = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x00030974 File Offset: 0x0002EB74
	// (set) Token: 0x060006DF RID: 1759 RVA: 0x0003097C File Offset: 0x0002EB7C
	public int overflowWidth
	{
		get
		{
			return this.mOverflowWidth;
		}
		set
		{
			if (this.mOverflowWidth != value)
			{
				this.mOverflowWidth = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00030994 File Offset: 0x0002EB94
	private bool keepCrisp
	{
		get
		{
			return this.trueTypeFont != null && this.keepCrispWhenShrunk != UILabel.Crispness.Never;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060006E1 RID: 1761 RVA: 0x000309AF File Offset: 0x0002EBAF
	// (set) Token: 0x060006E2 RID: 1762 RVA: 0x000309B7 File Offset: 0x0002EBB7
	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060006E3 RID: 1763 RVA: 0x000309D0 File Offset: 0x0002EBD0
	// (set) Token: 0x060006E4 RID: 1764 RVA: 0x000309D8 File Offset: 0x0002EBD8
	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000309F1 File Offset: 0x0002EBF1
	// (set) Token: 0x060006E6 RID: 1766 RVA: 0x000309F9 File Offset: 0x0002EBF9
	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00030A12 File Offset: 0x0002EC12
	// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00030A1A File Offset: 0x0002EC1A
	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00030A23 File Offset: 0x0002EC23
	// (set) Token: 0x060006EA RID: 1770 RVA: 0x00030A2B File Offset: 0x0002EC2B
	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x00030A34 File Offset: 0x0002EC34
	// (set) Token: 0x060006EC RID: 1772 RVA: 0x00030A42 File Offset: 0x0002EC42
	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = (value ? 0 : 1);
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x00030A67 File Offset: 0x0002EC67
	public override Vector3[] localCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText(false, true);
			}
			return base.localCorners;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x00030A7F File Offset: 0x0002EC7F
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText(false, true);
			}
			return base.worldCorners;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x00030A97 File Offset: 0x0002EC97
	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText(false, true);
			}
			return base.drawingDimensions;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00030AAF File Offset: 0x0002ECAF
	// (set) Token: 0x060006F1 RID: 1777 RVA: 0x00030AB7 File Offset: 0x0002ECB7
	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.shouldBeProcessed = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00030AE4 File Offset: 0x0002ECE4
	// (set) Token: 0x060006F3 RID: 1779 RVA: 0x00030AEC File Offset: 0x0002ECEC
	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00030B05 File Offset: 0x0002ED05
	// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00030B0D File Offset: 0x0002ED0D
	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.shouldBeProcessed = true;
				}
			}
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00030B33 File Offset: 0x0002ED33
	// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00030B3B File Offset: 0x0002ED3B
	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00030B59 File Offset: 0x0002ED59
	public int quadsPerCharacter
	{
		get
		{
			if (this.mEffectStyle == UILabel.Effect.Shadow)
			{
				return 2;
			}
			if (this.mEffectStyle == UILabel.Effect.Outline)
			{
				return 5;
			}
			if (this.mEffectStyle == UILabel.Effect.Outline8)
			{
				return 9;
			}
			return 1;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00030B7E File Offset: 0x0002ED7E
	// (set) Token: 0x060006FA RID: 1786 RVA: 0x00030B89 File Offset: 0x0002ED89
	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x00030B98 File Offset: 0x0002ED98
	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.shouldBeProcessed)
			{
				this.ProcessText(false, true);
			}
			return this.mProcessedText;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x00030BF6 File Offset: 0x0002EDF6
	public Vector2 printedSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText(false, true);
			}
			return this.mCalculatedSize;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x00030C0E File Offset: 0x0002EE0E
	public override Vector2 localSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText(false, true);
			}
			return base.localSize;
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x00030C26 File Offset: 0x0002EE26
	private bool isValid
	{
		get
		{
			return this.mFont != null || this.mTrueTypeFont != null;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x00030C44 File Offset: 0x0002EE44
	// (set) Token: 0x06000700 RID: 1792 RVA: 0x00030C4C File Offset: 0x0002EE4C
	public UILabel.Modifier modifier
	{
		get
		{
			return this.mModifier;
		}
		set
		{
			if (this.mModifier != value)
			{
				this.mModifier = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00030C6A File Offset: 0x0002EE6A
	protected override void OnInit()
	{
		base.OnInit();
		UILabel.mList.Add(this);
		this.SetActiveFont(this.trueTypeFont);
		if (!this.BeenInit)
		{
			Singleton<UIPalette>.Instance.InitTheme(this, null);
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00030C9D File Offset: 0x0002EE9D
	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		UILabel.mList.Remove(this);
		base.OnDisable();
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00030CB8 File Offset: 0x0002EEB8
	protected void SetActiveFont(Font fnt)
	{
		if (this.mActiveTTF != fnt)
		{
			Font font = this.mActiveTTF;
			int num;
			if (font != null && UILabel.mFontUsage.TryGetValue(font, out num))
			{
				num = Mathf.Max(0, --num);
				if (num == 0)
				{
					UILabel.mFontUsage.Remove(font);
				}
				else
				{
					UILabel.mFontUsage[font] = num;
				}
			}
			this.mActiveTTF = fnt;
			if (fnt != null)
			{
				int num2 = 0;
				UILabel.mFontUsage[fnt] = num2 + 1;
			}
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x00030D40 File Offset: 0x0002EF40
	public string printedText
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mText))
			{
				if (this.mModifier == UILabel.Modifier.None)
				{
					return this.mText;
				}
				if (this.mModifier == UILabel.Modifier.ToLowercase)
				{
					return this.mText.ToLower();
				}
				if (this.mModifier == UILabel.Modifier.ToUppercase)
				{
					return this.mText.ToUpper();
				}
				if (this.mModifier == UILabel.Modifier.Custom && this.customModifier != null)
				{
					return this.customModifier(this.mText);
				}
			}
			return this.mText;
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00030DC0 File Offset: 0x0002EFC0
	private static void OnFontChanged(Font font)
	{
		for (int i = 0; i < UILabel.mList.size; i++)
		{
			UILabel uilabel = UILabel.mList[i];
			if (uilabel != null)
			{
				Font trueTypeFont = uilabel.trueTypeFont;
				if (trueTypeFont == font)
				{
					trueTypeFont.RequestCharactersInTexture(uilabel.mText, uilabel.mFinalFontSize, uilabel.mFontStyle);
					uilabel.MarkAsChanged();
					if (uilabel.panel == null)
					{
						uilabel.CreatePanel();
					}
					if (UILabel.mTempDrawcalls == null)
					{
						UILabel.mTempDrawcalls = new BetterList<UIDrawCall>();
					}
					if (uilabel.drawCall != null && !UILabel.mTempDrawcalls.Contains(uilabel.drawCall))
					{
						UILabel.mTempDrawcalls.Add(uilabel.drawCall);
					}
				}
			}
		}
		if (UILabel.mTempDrawcalls != null)
		{
			int j = 0;
			int size = UILabel.mTempDrawcalls.size;
			while (j < size)
			{
				UIDrawCall uidrawCall = UILabel.mTempDrawcalls[j];
				if (uidrawCall.panel != null)
				{
					uidrawCall.panel.FillDrawCall(uidrawCall);
				}
				j++;
			}
			UILabel.mTempDrawcalls.Clear();
		}
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00030ED9 File Offset: 0x0002F0D9
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.shouldBeProcessed)
		{
			this.ProcessText(false, true);
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00030EF4 File Offset: 0x0002F0F4
	protected override void UpgradeFrom265()
	{
		this.ProcessText(true, true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount > 0) ? UILabel.Overflow.ResizeHeight : UILabel.Overflow.ShrinkContent);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int defaultSize = this.mFont.defaultSize;
			if (base.height < defaultSize)
			{
				base.height = defaultSize;
			}
			this.fontSize = defaultSize;
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00030FB8 File Offset: 0x0002F1B8
	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			if (base.isFullyAnchored)
			{
				this.mOverflow = UILabel.Overflow.ShrinkContent;
			}
		}
		else if (this.mOverflow == UILabel.Overflow.ResizeHeight && this.topAnchor.target != null && this.bottomAnchor.target != null)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0003101B File Offset: 0x0002F21B
	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText(false, true);
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00031033 File Offset: 0x0002F233
	protected override void OnEnable()
	{
		base.OnEnable();
		if (!UILabel.mTexRebuildAdded)
		{
			UILabel.mTexRebuildAdded = true;
			Font.textureRebuilt += UILabel.OnFontChanged;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0003105C File Offset: 0x0002F25C
	protected override void OnStart()
	{
		this.mApplyGradient = false;
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (this.material != null && this.material.shader != null && this.material.shader.name.Contains("Premultiplied"));
		this.ProcessAndRequest();
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x000310FF File Offset: 0x0002F2FF
	public override void MarkAsChanged()
	{
		this.shouldBeProcessed = true;
		base.MarkAsChanged();
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00031110 File Offset: 0x0002F310
	public void ProcessText(bool legacyMode = false, bool full = true)
	{
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.shouldBeProcessed = false;
		float num = this.mDrawRegion.z - this.mDrawRegion.x;
		float num2 = this.mDrawRegion.w - this.mDrawRegion.y;
		NGUIText.rectWidth = (legacyMode ? ((this.mMaxLineWidth != 0) ? this.mMaxLineWidth : 1000000) : base.width);
		NGUIText.rectHeight = (legacyMode ? ((this.mMaxLineHeight != 0) ? this.mMaxLineHeight : 1000000) : base.height);
		NGUIText.regionWidth = ((num != 1f) ? Mathf.RoundToInt((float)NGUIText.rectWidth * num) : NGUIText.rectWidth);
		NGUIText.regionHeight = ((num2 != 1f) ? Mathf.RoundToInt((float)NGUIText.rectHeight * num2) : NGUIText.rectHeight);
		this.mFinalFontSize = Mathf.Abs(legacyMode ? Mathf.RoundToInt(base.cachedTransform.localScale.x) : this.defaultFontSize);
		this.mScale = 1f;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
		{
			this.mProcessedText = "";
			return;
		}
		bool flag = this.trueTypeFont != null;
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				this.mDensity = ((root != null) ? root.pixelSizeAdjustment : 1f);
			}
		}
		else
		{
			this.mDensity = 1f;
		}
		if (full)
		{
			this.UpdateNGUIText();
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			NGUIText.rectWidth = 1000000;
			NGUIText.regionWidth = 1000000;
			if (this.mOverflowWidth > 0)
			{
				NGUIText.rectWidth = Mathf.Min(NGUIText.rectWidth, this.mOverflowWidth);
				NGUIText.regionWidth = Mathf.Min(NGUIText.regionWidth, this.mOverflowWidth);
			}
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
		}
		if (this.mFinalFontSize > 0)
		{
			bool keepCrisp = this.keepCrisp;
			int i = this.mFinalFontSize;
			while (i > 0)
			{
				if (keepCrisp)
				{
					this.mFinalFontSize = i;
					NGUIText.fontSize = this.mFinalFontSize;
				}
				else
				{
					this.mScale = (float)i / (float)this.mFinalFontSize;
					NGUIText.fontScale = (flag ? this.mScale : ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale));
				}
				NGUIText.Update(false);
				bool flag2 = NGUIText.WrapText(this.printedText, out this.mProcessedText, false, false, this.mOverflow == UILabel.Overflow.ClampContent && this.mOverflowEllipsis);
				if (this.mOverflow == UILabel.Overflow.ShrinkContent && !flag2)
				{
					if (--i <= 1)
					{
						break;
					}
					i--;
				}
				else
				{
					if (this.mOverflow == UILabel.Overflow.ResizeFreely)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						int num3 = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
						if (num != 1f)
						{
							num3 = Mathf.RoundToInt((float)num3 / num);
						}
						int num4 = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							num4 = Mathf.RoundToInt((float)num4 / num2);
						}
						if ((num3 & 1) == 1)
						{
							num3++;
						}
						if ((num4 & 1) == 1)
						{
							num4++;
						}
						if (this.mWidth != num3 || this.mHeight != num4)
						{
							this.mWidth = num3;
							this.mHeight = num4;
							if (this.onChange != null)
							{
								this.onChange();
							}
						}
					}
					else if (this.mOverflow == UILabel.Overflow.ResizeHeight)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						int num5 = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							num5 = Mathf.RoundToInt((float)num5 / num2);
						}
						if ((num5 & 1) == 1)
						{
							num5++;
						}
						this.mHeightSetByLabelOn = Time.frameCount;
						if (this.mHeight != num5)
						{
							this.mHeight = num5;
							if (this.onChange != null)
							{
								this.onChange();
							}
						}
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
					}
					if (legacyMode)
					{
						base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
						base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
						base.cachedTransform.localScale = Vector3.one;
						break;
					}
					break;
				}
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = "";
			this.mScale = 1f;
		}
		if (full)
		{
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x000315D4 File Offset: 0x0002F7D4
	public override void MakePixelPerfect()
	{
		if (!(this.ambigiousFont != null))
		{
			base.MakePixelPerfect();
			return;
		}
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
		localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
		localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
		base.cachedTransform.localPosition = localPosition;
		base.cachedTransform.localScale = Vector3.one;
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			this.AssumeNaturalSize();
			return;
		}
		int width = base.width;
		int height = base.height;
		UILabel.Overflow overflow = this.mOverflow;
		if (overflow != UILabel.Overflow.ResizeHeight)
		{
			this.mWidth = 100000;
		}
		this.mHeight = 100000;
		this.mOverflow = UILabel.Overflow.ShrinkContent;
		this.ProcessText(false, true);
		this.mOverflow = overflow;
		int num = Mathf.RoundToInt(this.mCalculatedSize.x);
		int num2 = Mathf.RoundToInt(this.mCalculatedSize.y);
		num = Mathf.Max(num, base.minWidth);
		num2 = Mathf.Max(num2, base.minHeight);
		if ((num & 1) == 1)
		{
			num++;
		}
		if ((num2 & 1) == 1)
		{
			num2++;
		}
		this.mWidth = Mathf.Max(width, num);
		this.mHeight = Mathf.Max(height, num2);
		this.MarkAsChanged();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00031730 File Offset: 0x0002F930
	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.mWidth = 100000;
			this.mHeight = 100000;
			this.ProcessText(false, true);
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
			if ((this.mWidth & 1) == 1)
			{
				this.mWidth++;
			}
			if ((this.mHeight & 1) == 1)
			{
				this.mHeight++;
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x000317D0 File Offset: 0x0002F9D0
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector3 worldPos)
	{
		return this.GetCharacterIndexAtPosition(worldPos, false);
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x000317DA File Offset: 0x0002F9DA
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector2 localPos)
	{
		return this.GetCharacterIndexAtPosition(localPos, false);
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x000317E4 File Offset: 0x0002F9E4
	public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
	{
		Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
		return this.GetCharacterIndexAtPosition(localPos, precise);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0003180C File Offset: 0x0002FA0C
	public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			this.UpdateNGUIText();
			if (precise)
			{
				NGUIText.PrintExactCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			else
			{
				NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			if (UILabel.mTempVerts.Count > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int result = precise ? NGUIText.GetExactCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos) : NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos);
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
				NGUIText.bitmapFont = null;
				NGUIText.dynamicFont = null;
				return result;
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
		return 0;
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x000318D0 File Offset: 0x0002FAD0
	public string GetWordAtPosition(Vector3 worldPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(worldPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x000318F0 File Offset: 0x0002FAF0
	public string GetWordAtPosition(Vector2 localPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(localPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00031910 File Offset: 0x0002FB10
	public string GetWordAtCharacterIndex(int characterIndex)
	{
		string printedText = this.printedText;
		if (characterIndex != -1 && characterIndex < printedText.Length)
		{
			int num = printedText.LastIndexOfAny(new char[]
			{
				' ',
				'\n'
			}, characterIndex) + 1;
			int num2 = printedText.IndexOfAny(new char[]
			{
				' ',
				'\n',
				',',
				'.'
			}, characterIndex);
			if (num2 == -1)
			{
				num2 = printedText.Length;
			}
			if (num != num2)
			{
				int num3 = num2 - num;
				if (num3 > 0)
				{
					return NGUIText.StripSymbols(printedText.Substring(num, num3));
				}
			}
		}
		return null;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0003198A File Offset: 0x0002FB8A
	public string GetUrlAtPosition(Vector3 worldPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0003199A File Offset: 0x0002FB9A
	public string GetUrlAtPosition(Vector2 localPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x000319AC File Offset: 0x0002FBAC
	public string GetUrlAtCharacterIndex(int characterIndex)
	{
		string printedText = this.printedText;
		if (characterIndex != -1 && characterIndex < printedText.Length - 6)
		{
			int num;
			if (printedText[characterIndex] == '[' && printedText[characterIndex + 1] == 'u' && printedText[characterIndex + 2] == 'r' && printedText[characterIndex + 3] == 'l' && printedText[characterIndex + 4] == '=')
			{
				num = characterIndex;
			}
			else
			{
				num = printedText.LastIndexOf("[url=", characterIndex);
			}
			if (num == -1)
			{
				return null;
			}
			num += 5;
			int num2 = printedText.IndexOf("]", num);
			if (num2 == -1)
			{
				return null;
			}
			int num3 = printedText.IndexOf("[/url]", num2);
			if (num3 == -1 || characterIndex <= num3)
			{
				return printedText.Substring(num, num2 - num);
			}
		}
		return null;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00031A64 File Offset: 0x0002FC64
	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			int defaultFontSize = this.defaultFontSize;
			this.UpdateNGUIText();
			NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			if (UILabel.mTempVerts.Count > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int i = 0;
				int count = UILabel.mTempIndices.Count;
				while (i < count)
				{
					if (UILabel.mTempIndices[i] == currentIndex)
					{
						Vector2 pos = UILabel.mTempVerts[i];
						if (key == KeyCode.UpArrow)
						{
							pos.y += (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.DownArrow)
						{
							pos.y -= (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.Home)
						{
							pos.x -= 1000f;
						}
						else if (key == KeyCode.End)
						{
							pos.x += 1000f;
						}
						int approximateCharacterIndex = NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, pos);
						if (approximateCharacterIndex != currentIndex)
						{
							UILabel.mTempVerts.Clear();
							UILabel.mTempIndices.Clear();
							return approximateCharacterIndex;
						}
						break;
					}
					else
					{
						i++;
					}
				}
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
			if (key == KeyCode.UpArrow || key == KeyCode.Home)
			{
				return 0;
			}
			if (key == KeyCode.DownArrow || key == KeyCode.End)
			{
				return processedText.Length;
			}
		}
		return currentIndex;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00031BF0 File Offset: 0x0002FDF0
	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!this.isValid)
		{
			return;
		}
		string processedText = this.processedText;
		this.UpdateNGUIText();
		int count = caret.verts.Count;
		Vector2 item = new Vector2(0.5f, 0.5f);
		float finalAlpha = this.finalAlpha;
		if (highlight != null && start != end)
		{
			int count2 = highlight.verts.Count;
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, highlight.verts);
			if (highlight.verts.Count > count2)
			{
				this.ApplyOffset(highlight.verts, count2);
				Color item2 = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * finalAlpha);
				int i = count2;
				int count3 = highlight.verts.Count;
				while (i < count3)
				{
					highlight.uvs.Add(item);
					highlight.cols.Add(item2);
					i++;
				}
			}
		}
		else
		{
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, null);
		}
		this.ApplyOffset(caret.verts, count);
		Color item3 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * finalAlpha);
		int j = count;
		int count4 = caret.verts.Count;
		while (j < count4)
		{
			caret.uvs.Add(item);
			caret.cols.Add(item3);
			j++;
		}
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00031D88 File Offset: 0x0002FF88
	public override void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.Count;
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlphaShader)
		{
			color = NGUITools.ApplyPMA(color);
		}
		string text = this.processedText;
		if (text.Length > 10000)
		{
			int num2 = this.GetCharacterIndexAtPosition(new Vector3(0f, 0f, 0f), false) - 5000;
			int num3 = num2 + 10000;
			string str = "";
			string str2 = "";
			if (num2 > 0)
			{
				str = Regex.Replace(text.Substring(0, num2), "[^\0- ]", " ");
			}
			else
			{
				num2 = 0;
			}
			if (num3 < text.Length)
			{
				str2 = Regex.Replace(text.Substring(num3), "[^\0- ]", " ");
			}
			else
			{
				num3 = text.Length;
			}
			string str3 = text.Substring(num2, num3 - num2);
			text = str + str3 + str2;
		}
		int count = verts.Count;
		this.UpdateNGUIText();
		NGUIText.tint = color;
		NGUIText.Print(text, verts, uvs, cols);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		Vector2 vector = this.ApplyOffset(verts, count);
		if (this.mFont != null && this.mFont.packedFontShader)
		{
			return;
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int count2 = verts.Count;
			vector.x = this.mEffectDistance.x;
			vector.y = this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, count2, vector.x, -vector.y);
			if (this.effectStyle == UILabel.Effect.Outline || this.effectStyle == UILabel.Effect.Outline8)
			{
				num = count2;
				count2 = verts.Count;
				this.ApplyShadow(verts, uvs, cols, num, count2, -vector.x, vector.y);
				num = count2;
				count2 = verts.Count;
				this.ApplyShadow(verts, uvs, cols, num, count2, vector.x, vector.y);
				num = count2;
				count2 = verts.Count;
				this.ApplyShadow(verts, uvs, cols, num, count2, -vector.x, -vector.y);
				if (this.effectStyle == UILabel.Effect.Outline8)
				{
					num = count2;
					count2 = verts.Count;
					this.ApplyShadow(verts, uvs, cols, num, count2, -vector.x, 0f);
					num = count2;
					count2 = verts.Count;
					this.ApplyShadow(verts, uvs, cols, num, count2, vector.x, 0f);
					num = count2;
					count2 = verts.Count;
					this.ApplyShadow(verts, uvs, cols, num, count2, 0f, vector.y);
					num = count2;
					count2 = verts.Count;
					this.ApplyShadow(verts, uvs, cols, num, count2, 0f, -vector.y);
				}
			}
		}
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0003207C File Offset: 0x0003027C
	public Vector2 ApplyOffset(List<Vector3> verts, int start)
	{
		Vector2 pivotOffset = base.pivotOffset;
		float num = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num2 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y) + Mathf.Lerp(this.mCalculatedSize.y - (float)this.mHeight, 0f, pivotOffset.y);
		num = Mathf.Round(num);
		num2 = Mathf.Round(num2);
		int i = start;
		int count = verts.Count;
		while (i < count)
		{
			Vector3 value = verts[i];
			value.x += num;
			value.y += num2;
			verts[i] = value;
			i++;
		}
		return new Vector2(num, num2);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00032140 File Offset: 0x00030340
	public void ApplyShadow(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= this.finalAlpha;
		if (this.bitmapFont != null && this.bitmapFont.premultipliedAlphaShader)
		{
			color = NGUITools.ApplyPMA(color);
		}
		Color value = color;
		bool flag = this.mEffectColor == Color.black;
		for (int i = start; i < end; i++)
		{
			verts.Add(verts[i]);
			uvs.Add(uvs[i]);
			cols.Add(cols[i]);
			Vector3 value2 = verts[i];
			value2.x += x;
			value2.y += y;
			verts[i] = value2;
			Color color2 = cols[i];
			if (color2.a == 1f)
			{
				cols[i] = value;
			}
			else
			{
				Color value3 = color;
				value3.a = color2.a * color.a;
				cols[i] = value3;
			}
			if (flag && color2.grayscale < 0.1f)
			{
				float num = 0.45f;
				cols[i] = new Color(num, num, num, cols[i].a);
			}
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00032279 File Offset: 0x00030479
	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		int result = NGUIText.CalculateOffsetToFit(text);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x000322A0 File Offset: 0x000304A0
	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			this.text = UIProgressBar.current.value.ToString("F");
		}
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x000322D7 File Offset: 0x000304D7
	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00032310 File Offset: 0x00030510
	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			Language.UpdateUILabel(this, UIPopupList.current.value);
		}
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0003232F File Offset: 0x0003052F
	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0003233E File Offset: 0x0003053E
	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.rectHeight = height;
		NGUIText.regionHeight = height;
		bool result = NGUIText.WrapText(text, out final, false);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00032368 File Offset: 0x00030568
	public void UpdateNGUIText()
	{
		Font trueTypeFont = this.trueTypeFont;
		bool flag = trueTypeFont != null;
		NGUIText.fontSize = this.mFinalFontSize;
		NGUIText.fontStyle = this.mFontStyle;
		NGUIText.rectWidth = this.mWidth;
		NGUIText.rectHeight = this.mHeight;
		NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
		NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		NGUIText.gradient = (this.mApplyGradient && (this.mFont == null || !this.mFont.packedFontShader));
		NGUIText.gradientTop = this.mGradientTop;
		NGUIText.gradientBottom = this.mGradientBottom;
		NGUIText.encoding = this.mEncoding;
		NGUIText.premultiply = this.mPremultiply;
		NGUIText.symbolStyle = this.mSymbols;
		NGUIText.maxLines = this.mMaxLineCount;
		NGUIText.spacingX = this.effectiveSpacingX;
		NGUIText.spacingY = this.effectiveSpacingY;
		NGUIText.fontScale = (flag ? this.mScale : ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale));
		if (this.mFont != null)
		{
			NGUIText.bitmapFont = this.mFont;
			for (;;)
			{
				UIFont replacement = NGUIText.bitmapFont.replacement;
				if (replacement == null)
				{
					break;
				}
				NGUIText.bitmapFont = replacement;
			}
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
		}
		else
		{
			NGUIText.dynamicFont = trueTypeFont;
			NGUIText.bitmapFont = null;
		}
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				NGUIText.pixelDensity = ((root != null) ? root.pixelSizeAdjustment : 1f);
			}
		}
		else
		{
			NGUIText.pixelDensity = 1f;
		}
		if (this.mDensity != NGUIText.pixelDensity)
		{
			this.ProcessText(false, false);
			NGUIText.rectWidth = this.mWidth;
			NGUIText.rectHeight = this.mHeight;
			NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
			NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		}
		if (this.alignment == NGUIText.Alignment.Automatic)
		{
			UIWidget.Pivot pivot = base.pivot;
			if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
			{
				NGUIText.alignment = NGUIText.Alignment.Left;
			}
			else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				NGUIText.alignment = NGUIText.Alignment.Right;
			}
			else
			{
				NGUIText.alignment = NGUIText.Alignment.Center;
			}
		}
		else
		{
			NGUIText.alignment = this.alignment;
		}
		NGUIText.Update();
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00032634 File Offset: 0x00030834
	private void OnApplicationPause(bool paused)
	{
		if (!paused && this.mTrueTypeFont != null)
		{
			this.Invalidate(false);
		}
	}

	// Token: 0x040004CE RID: 1230
	public const int DISPLAYED_TEXT_CHARS = 10000;

	// Token: 0x040004CF RID: 1231
	public UIPalette.UI ThemeAsSetting;

	// Token: 0x040004D0 RID: 1232
	public UIPalette.UI ThemeAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040004D1 RID: 1233
	[NonSerialized]
	public bool BeenInit;

	// Token: 0x040004D2 RID: 1234
	public UILabel.Crispness keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;

	// Token: 0x040004D3 RID: 1235
	[HideInInspector]
	[SerializeField]
	private Font mTrueTypeFont;

	// Token: 0x040004D4 RID: 1236
	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	// Token: 0x040004D5 RID: 1237
	[Multiline(6)]
	[HideInInspector]
	[SerializeField]
	private string mText = "";

	// Token: 0x040004D6 RID: 1238
	[HideInInspector]
	[SerializeField]
	private int mFontSize = 16;

	// Token: 0x040004D7 RID: 1239
	[HideInInspector]
	[SerializeField]
	private FontStyle mFontStyle;

	// Token: 0x040004D8 RID: 1240
	[HideInInspector]
	[SerializeField]
	private NGUIText.Alignment mAlignment;

	// Token: 0x040004D9 RID: 1241
	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	// Token: 0x040004DA RID: 1242
	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	// Token: 0x040004DB RID: 1243
	[HideInInspector]
	[SerializeField]
	private UILabel.Effect mEffectStyle;

	// Token: 0x040004DC RID: 1244
	[HideInInspector]
	[SerializeField]
	private Color mEffectColor = Color.black;

	// Token: 0x040004DD RID: 1245
	[HideInInspector]
	[SerializeField]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;

	// Token: 0x040004DE RID: 1246
	[HideInInspector]
	[SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	// Token: 0x040004DF RID: 1247
	[HideInInspector]
	[SerializeField]
	private UILabel.Overflow mOverflow;

	// Token: 0x040004E0 RID: 1248
	[HideInInspector]
	[SerializeField]
	private bool mApplyGradient;

	// Token: 0x040004E1 RID: 1249
	[HideInInspector]
	[SerializeField]
	private Color mGradientTop = Color.white;

	// Token: 0x040004E2 RID: 1250
	[HideInInspector]
	[SerializeField]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x040004E3 RID: 1251
	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	// Token: 0x040004E4 RID: 1252
	[HideInInspector]
	[SerializeField]
	private int mSpacingY;

	// Token: 0x040004E5 RID: 1253
	[HideInInspector]
	[SerializeField]
	private bool mUseFloatSpacing;

	// Token: 0x040004E6 RID: 1254
	[HideInInspector]
	[SerializeField]
	private float mFloatSpacingX;

	// Token: 0x040004E7 RID: 1255
	[HideInInspector]
	[SerializeField]
	private float mFloatSpacingY;

	// Token: 0x040004E8 RID: 1256
	[HideInInspector]
	[SerializeField]
	private bool mOverflowEllipsis;

	// Token: 0x040004E9 RID: 1257
	[HideInInspector]
	[SerializeField]
	private int mOverflowWidth;

	// Token: 0x040004EA RID: 1258
	[HideInInspector]
	[SerializeField]
	private UILabel.Modifier mModifier;

	// Token: 0x040004EB RID: 1259
	[HideInInspector]
	[SerializeField]
	private bool mShrinkToFit;

	// Token: 0x040004EC RID: 1260
	[HideInInspector]
	[SerializeField]
	private int mMaxLineWidth;

	// Token: 0x040004ED RID: 1261
	[HideInInspector]
	[SerializeField]
	private int mMaxLineHeight;

	// Token: 0x040004EE RID: 1262
	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	// Token: 0x040004EF RID: 1263
	[HideInInspector]
	[SerializeField]
	private bool mMultiline = true;

	// Token: 0x040004F0 RID: 1264
	[NonSerialized]
	private Font mActiveTTF;

	// Token: 0x040004F1 RID: 1265
	[NonSerialized]
	private float mDensity = 1f;

	// Token: 0x040004F2 RID: 1266
	[NonSerialized]
	private bool mShouldBeProcessed = true;

	// Token: 0x040004F3 RID: 1267
	[NonSerialized]
	private string mProcessedText;

	// Token: 0x040004F4 RID: 1268
	[NonSerialized]
	private bool mPremultiply;

	// Token: 0x040004F5 RID: 1269
	[NonSerialized]
	private Vector2 mCalculatedSize = Vector2.zero;

	// Token: 0x040004F6 RID: 1270
	[NonSerialized]
	private float mScale = 1f;

	// Token: 0x040004F7 RID: 1271
	[NonSerialized]
	private int mFinalFontSize;

	// Token: 0x040004F8 RID: 1272
	[NonSerialized]
	private int mLastWidth;

	// Token: 0x040004F9 RID: 1273
	[NonSerialized]
	private int mLastHeight;

	// Token: 0x040004FA RID: 1274
	public UILabel.ModifierFunc customModifier;

	// Token: 0x040004FB RID: 1275
	private static BetterList<UILabel> mList = new BetterList<UILabel>();

	// Token: 0x040004FC RID: 1276
	private static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();

	// Token: 0x040004FD RID: 1277
	[NonSerialized]
	private static BetterList<UIDrawCall> mTempDrawcalls;

	// Token: 0x040004FE RID: 1278
	private static bool mTexRebuildAdded = false;

	// Token: 0x040004FF RID: 1279
	private static List<Vector3> mTempVerts = new List<Vector3>();

	// Token: 0x04000500 RID: 1280
	private static List<int> mTempIndices = new List<int>();

	// Token: 0x0200056F RID: 1391
	public enum Effect
	{
		// Token: 0x040024D3 RID: 9427
		None,
		// Token: 0x040024D4 RID: 9428
		Shadow,
		// Token: 0x040024D5 RID: 9429
		Outline,
		// Token: 0x040024D6 RID: 9430
		Outline8
	}

	// Token: 0x02000570 RID: 1392
	public enum Overflow
	{
		// Token: 0x040024D8 RID: 9432
		ShrinkContent,
		// Token: 0x040024D9 RID: 9433
		ClampContent,
		// Token: 0x040024DA RID: 9434
		ResizeFreely,
		// Token: 0x040024DB RID: 9435
		ResizeHeight
	}

	// Token: 0x02000571 RID: 1393
	public enum Crispness
	{
		// Token: 0x040024DD RID: 9437
		Never,
		// Token: 0x040024DE RID: 9438
		OnDesktop,
		// Token: 0x040024DF RID: 9439
		Always
	}

	// Token: 0x02000572 RID: 1394
	public enum Modifier
	{
		// Token: 0x040024E1 RID: 9441
		None,
		// Token: 0x040024E2 RID: 9442
		ToUppercase,
		// Token: 0x040024E3 RID: 9443
		ToLowercase,
		// Token: 0x040024E4 RID: 9444
		Custom = 255
	}

	// Token: 0x02000573 RID: 1395
	// (Invoke) Token: 0x0600383B RID: 14395
	public delegate string ModifierFunc(string s);
}
