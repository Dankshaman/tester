using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000086 RID: 134
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Font")]
public class UIFont : MonoBehaviour
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000649 RID: 1609 RVA: 0x0002D549 File Offset: 0x0002B749
	// (set) Token: 0x0600064A RID: 1610 RVA: 0x0002D56B File Offset: 0x0002B76B
	public BMFont bmFont
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mFont;
			}
			return this.mReplacement.bmFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.bmFont = value;
				return;
			}
			this.mFont = value;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x0002D58F File Offset: 0x0002B78F
	// (set) Token: 0x0600064C RID: 1612 RVA: 0x0002D5C0 File Offset: 0x0002B7C0
	public int texWidth
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texWidth;
			}
			if (this.mFont == null)
			{
				return 1;
			}
			return this.mFont.texWidth;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texWidth = value;
				return;
			}
			if (this.mFont != null)
			{
				this.mFont.texWidth = value;
			}
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x0002D5F1 File Offset: 0x0002B7F1
	// (set) Token: 0x0600064E RID: 1614 RVA: 0x0002D622 File Offset: 0x0002B822
	public int texHeight
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texHeight;
			}
			if (this.mFont == null)
			{
				return 1;
			}
			return this.mFont.texHeight;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texHeight = value;
				return;
			}
			if (this.mFont != null)
			{
				this.mFont.texHeight = value;
			}
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x0002D653 File Offset: 0x0002B853
	public bool hasSymbols
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mSymbols != null && this.mSymbols.Count != 0;
			}
			return this.mReplacement.hasSymbols;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x0002D687 File Offset: 0x0002B887
	public List<BMSymbol> symbols
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mSymbols;
			}
			return this.mReplacement.symbols;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002D6A9 File Offset: 0x0002B8A9
	// (set) Token: 0x06000652 RID: 1618 RVA: 0x0002D6CC File Offset: 0x0002B8CC
	public UIAtlas atlas
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mAtlas;
			}
			return this.mReplacement.atlas;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
				return;
			}
			if (this.mAtlas != value)
			{
				this.mPMA = -1;
				this.mAtlas = value;
				if (this.mAtlas != null)
				{
					this.mMat = this.mAtlas.spriteMaterial;
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000653 RID: 1619 RVA: 0x0002D74C File Offset: 0x0002B94C
	// (set) Token: 0x06000654 RID: 1620 RVA: 0x0002D7FE File Offset: 0x0002B9FE
	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat != null)
			{
				if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
				{
					this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
				}
				return this.mMat;
			}
			if (this.mDynamicFont != null)
			{
				return this.mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
				return;
			}
			if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x0002D83D File Offset: 0x0002BA3D
	[Obsolete("Use UIFont.premultipliedAlphaShader instead")]
	public bool premultipliedAlpha
	{
		get
		{
			return this.premultipliedAlphaShader;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x0002D848 File Offset: 0x0002BA48
	public bool premultipliedAlphaShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlphaShader;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((material != null && material.shader != null && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000657 RID: 1623 RVA: 0x0002D8DC File Offset: 0x0002BADC
	public bool packedFontShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.packedFontShader;
			}
			if (this.mAtlas != null)
			{
				return false;
			}
			if (this.mPacked == -1)
			{
				Material material = this.material;
				this.mPacked = ((material != null && material.shader != null && material.shader.name.Contains("Packed")) ? 1 : 0);
			}
			return this.mPacked == 1;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x0002D964 File Offset: 0x0002BB64
	public Texture2D texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			if (!(material != null))
			{
				return null;
			}
			return material.mainTexture as Texture2D;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000659 RID: 1625 RVA: 0x0002D9A8 File Offset: 0x0002BBA8
	// (set) Token: 0x0600065A RID: 1626 RVA: 0x0002DA05 File Offset: 0x0002BC05
	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			if (!(this.mAtlas != null) || this.sprite == null)
			{
				return new Rect(0f, 0f, 1f, 1f);
			}
			return this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
				return;
			}
			if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600065B RID: 1627 RVA: 0x0002DA45 File Offset: 0x0002BC45
	// (set) Token: 0x0600065C RID: 1628 RVA: 0x0002DA6C File Offset: 0x0002BC6C
	public string spriteName
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mFont.spriteName;
			}
			return this.mReplacement.spriteName;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
				return;
			}
			if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x0002DAB9 File Offset: 0x0002BCB9
	public bool isValid
	{
		get
		{
			return this.mDynamicFont != null || this.mFont.isValid;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x0002DAD6 File Offset: 0x0002BCD6
	// (set) Token: 0x0600065F RID: 1631 RVA: 0x0002DADE File Offset: 0x0002BCDE
	[Obsolete("Use UIFont.defaultSize instead")]
	public int size
	{
		get
		{
			return this.defaultSize;
		}
		set
		{
			this.defaultSize = value;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000660 RID: 1632 RVA: 0x0002DAE7 File Offset: 0x0002BCE7
	// (set) Token: 0x06000661 RID: 1633 RVA: 0x0002DB25 File Offset: 0x0002BD25
	public int defaultSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.defaultSize;
			}
			if (this.isDynamic || this.mFont == null)
			{
				return this.mDynamicFontSize;
			}
			return this.mFont.charSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.defaultSize = value;
				return;
			}
			this.mDynamicFontSize = value;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x0002DB4C File Offset: 0x0002BD4C
	public UISpriteData sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (this.mSprite == null && this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
			{
				this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
				if (this.mSprite == null)
				{
					this.mSprite = this.mAtlas.GetSprite(base.name);
				}
				if (this.mSprite == null)
				{
					this.mFont.spriteName = null;
				}
				else
				{
					this.UpdateUVRect();
				}
				int i = 0;
				int count = this.mSymbols.Count;
				while (i < count)
				{
					this.symbols[i].MarkAsChanged();
					i++;
				}
			}
			return this.mSprite;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000663 RID: 1635 RVA: 0x0002DC2A File Offset: 0x0002BE2A
	// (set) Token: 0x06000664 RID: 1636 RVA: 0x0002DC34 File Offset: 0x0002BE34
	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uifont = value;
			if (uifont == this)
			{
				uifont = null;
			}
			if (this.mReplacement != uifont)
			{
				if (uifont != null && uifont.replacement == this)
				{
					uifont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uifont;
				if (uifont != null)
				{
					this.mPMA = -1;
					this.mMat = null;
					this.mFont = null;
					this.mDynamicFont = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000665 RID: 1637 RVA: 0x0002DCC0 File Offset: 0x0002BEC0
	public bool isDynamic
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mDynamicFont != null;
			}
			return this.mReplacement.isDynamic;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000666 RID: 1638 RVA: 0x0002DCE8 File Offset: 0x0002BEE8
	// (set) Token: 0x06000667 RID: 1639 RVA: 0x0002DD0C File Offset: 0x0002BF0C
	public Font dynamicFont
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mDynamicFont;
			}
			return this.mReplacement.dynamicFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
				return;
			}
			if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000668 RID: 1640 RVA: 0x0002DD64 File Offset: 0x0002BF64
	// (set) Token: 0x06000669 RID: 1641 RVA: 0x0002DD86 File Offset: 0x0002BF86
	public FontStyle dynamicFontStyle
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mDynamicFontStyle;
			}
			return this.mReplacement.dynamicFontStyle;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
				return;
			}
			if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0002DDBC File Offset: 0x0002BFBC
	private void Trim()
	{
		if (this.mAtlas.texture != null && this.mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect2 = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			this.mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002DEAB File Offset: 0x0002C0AB
	private bool References(UIFont font)
	{
		return !(font == null) && (font == this || (this.mReplacement != null && this.mReplacement.References(font)));
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0002DEE0 File Offset: 0x0002C0E0
	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		return !(a == null) && !(b == null) && ((a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]) || a == b || a.References(b) || b.References(a));
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x0600066D RID: 1645 RVA: 0x0002DF4F File Offset: 0x0002C14F
	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0002DF84 File Offset: 0x0002C184
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		this.mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UILabel uilabel = array[i];
			if (uilabel.enabled && NGUITools.GetActive(uilabel.gameObject) && UIFont.CheckIfRelated(this, uilabel.bitmapFont))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			i++;
		}
		int j = 0;
		int count = this.symbols.Count;
		while (j < count)
		{
			this.symbols[j].MarkAsChanged();
			j++;
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0002E038 File Offset: 0x0002C238
	public void UpdateUVRect()
	{
		if (this.mAtlas == null)
		{
			return;
		}
		Texture texture = this.mAtlas.texture;
		if (texture != null)
		{
			this.mUVRect = new Rect((float)(this.mSprite.x - this.mSprite.paddingLeft), (float)(this.mSprite.y - this.mSprite.paddingTop), (float)(this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float)(this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
			this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
			if (this.mSprite.hasPadding)
			{
				this.Trim();
			}
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0002E120 File Offset: 0x0002C320
	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		int count = this.mSymbols.Count;
		while (i < count)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			if (bmsymbol.sequence == sequence)
			{
				return bmsymbol;
			}
			i++;
		}
		if (createIfMissing)
		{
			BMSymbol bmsymbol2 = new BMSymbol();
			bmsymbol2.sequence = sequence;
			this.mSymbols.Add(bmsymbol2);
			return bmsymbol2;
		}
		return null;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0002E184 File Offset: 0x0002C384
	public BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			int length = bmsymbol.length;
			if (length != 0 && textLength >= length)
			{
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (text[offset + j] != bmsymbol.sequence[j])
					{
						flag = false;
						break;
					}
				}
				if (flag && bmsymbol.Validate(this.atlas))
				{
					return bmsymbol;
				}
			}
		}
		return null;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0002E214 File Offset: 0x0002C414
	public void AddSymbol(string sequence, string spriteName)
	{
		this.GetSymbol(sequence, true).spriteName = spriteName;
		this.MarkAsChanged();
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0002E22C File Offset: 0x0002C42C
	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsChanged();
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002E258 File Offset: 0x0002C458
	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsChanged();
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0002E280 File Offset: 0x0002C480
	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int i = 0;
			int count = this.symbols.Count;
			while (i < count)
			{
				BMSymbol bmsymbol = this.symbols[i];
				if (s.Equals(bmsymbol.spriteName))
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04000487 RID: 1159
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	// Token: 0x04000488 RID: 1160
	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04000489 RID: 1161
	[HideInInspector]
	[SerializeField]
	private BMFont mFont = new BMFont();

	// Token: 0x0400048A RID: 1162
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	// Token: 0x0400048B RID: 1163
	[HideInInspector]
	[SerializeField]
	private UIFont mReplacement;

	// Token: 0x0400048C RID: 1164
	[HideInInspector]
	[SerializeField]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	// Token: 0x0400048D RID: 1165
	[HideInInspector]
	[SerializeField]
	private Font mDynamicFont;

	// Token: 0x0400048E RID: 1166
	[HideInInspector]
	[SerializeField]
	private int mDynamicFontSize = 16;

	// Token: 0x0400048F RID: 1167
	[HideInInspector]
	[SerializeField]
	private FontStyle mDynamicFontStyle;

	// Token: 0x04000490 RID: 1168
	[NonSerialized]
	private UISpriteData mSprite;

	// Token: 0x04000491 RID: 1169
	private int mPMA = -1;

	// Token: 0x04000492 RID: 1170
	private int mPacked = -1;
}
