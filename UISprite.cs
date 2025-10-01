using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008E RID: 142
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Sprite")]
public class UISprite : UIBasicSprite
{
	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x00035660 File Offset: 0x00033860
	// (set) Token: 0x06000793 RID: 1939 RVA: 0x000305A6 File Offset: 0x0002E7A6
	public override Texture mainTexture
	{
		get
		{
			Material material = (this.mAtlas != null) ? this.mAtlas.spriteMaterial : null;
			if (!(material != null))
			{
				return null;
			}
			return material.mainTexture;
		}
		set
		{
			base.mainTexture = value;
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x0003569C File Offset: 0x0003389C
	// (set) Token: 0x06000795 RID: 1941 RVA: 0x0003054B File Offset: 0x0002E74B
	public override Material material
	{
		get
		{
			Material material = base.material;
			if (material != null)
			{
				return material;
			}
			if (!(this.mAtlas != null))
			{
				return null;
			}
			return this.mAtlas.spriteMaterial;
		}
		set
		{
			base.material = value;
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x000356D6 File Offset: 0x000338D6
	// (set) Token: 0x06000797 RID: 1943 RVA: 0x000356E0 File Offset: 0x000338E0
	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string spriteName = this.mSpriteName;
					this.mSpriteName = "";
					this.spriteName = spriteName;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x0003579B File Offset: 0x0003399B
	// (set) Token: 0x06000799 RID: 1945 RVA: 0x000357A4 File Offset: 0x000339A4
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (this.mSpriteName != value)
				{
					this.mSpriteName = value;
					this.mSprite = null;
					this.mChanged = true;
					this.mSpriteSet = false;
				}
				return;
			}
			if (string.IsNullOrEmpty(this.mSpriteName))
			{
				return;
			}
			this.mSpriteName = "";
			this.mSprite = null;
			this.mChanged = true;
			this.mSpriteSet = false;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x00035812 File Offset: 0x00033A12
	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x0600079B RID: 1947 RVA: 0x0003581D File Offset: 0x00033A1D
	// (set) Token: 0x0600079C RID: 1948 RVA: 0x00035828 File Offset: 0x00033A28
	[Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return this.centerType > UIBasicSprite.AdvancedType.Invisible;
		}
		set
		{
			if (value != this.centerType > UIBasicSprite.AdvancedType.Invisible)
			{
				this.centerType = (value ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible);
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x00035849 File Offset: 0x00033A49
	// (set) Token: 0x0600079E RID: 1950 RVA: 0x00035851 File Offset: 0x00033A51
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

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x00035869 File Offset: 0x00033A69
	// (set) Token: 0x060007A0 RID: 1952 RVA: 0x00035871 File Offset: 0x00033A71
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

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00035896 File Offset: 0x00033A96
	// (set) Token: 0x060007A2 RID: 1954 RVA: 0x0003589E File Offset: 0x00033A9E
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

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x000358C4 File Offset: 0x00033AC4
	public override Vector4 border
	{
		get
		{
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return base.border;
			}
			return new Vector4((float)atlasSprite.borderLeft, (float)atlasSprite.borderBottom, (float)atlasSprite.borderRight, (float)atlasSprite.borderTop);
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00035903 File Offset: 0x00033B03
	public override float pixelSize
	{
		get
		{
			if (!(this.mAtlas != null))
			{
				return 1f;
			}
			return this.mAtlas.pixelSize;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00035924 File Offset: 0x00033B24
	public override int minWidth
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				float pixelSize = this.pixelSize;
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.x + vector.z);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += Mathf.RoundToInt(pixelSize * (float)(atlasSprite.paddingLeft + atlasSprite.paddingRight));
				}
				return Mathf.Max(base.minWidth, ((num & 1) == 1) ? (num + 1) : num);
			}
			return base.minWidth;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000359B0 File Offset: 0x00033BB0
	public override int minHeight
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				float pixelSize = this.pixelSize;
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.y + vector.w);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += Mathf.RoundToInt(pixelSize * (float)(atlasSprite.paddingTop + atlasSprite.paddingBottom));
				}
				return Mathf.Max(base.minHeight, ((num & 1) == 1) ? (num + 1) : num);
			}
			return base.minHeight;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00035A3C File Offset: 0x00033C3C
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.GetAtlasSprite() != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num5 = this.mSprite.paddingLeft;
				int num6 = this.mSprite.paddingBottom;
				int num7 = this.mSprite.paddingRight;
				int num8 = this.mSprite.paddingTop;
				if (this.mType != UIBasicSprite.Type.Simple)
				{
					float pixelSize = this.pixelSize;
					if (pixelSize != 1f)
					{
						num5 = Mathf.RoundToInt(pixelSize * (float)num5);
						num6 = Mathf.RoundToInt(pixelSize * (float)num6);
						num7 = Mathf.RoundToInt(pixelSize * (float)num7);
						num8 = Mathf.RoundToInt(pixelSize * (float)num8);
					}
				}
				int num9 = this.mSprite.width + num5 + num7;
				int num10 = this.mSprite.height + num6 + num8;
				float num11 = 1f;
				float num12 = 1f;
				if (num9 > 0 && num10 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num9 & 1) != 0)
					{
						num7++;
					}
					if ((num10 & 1) != 0)
					{
						num8++;
					}
					num11 = 1f / (float)num9 * (float)this.mWidth;
					num12 = 1f / (float)num10 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num7 * num11;
					num3 -= (float)num5 * num11;
				}
				else
				{
					num += (float)num5 * num11;
					num3 -= (float)num7 * num11;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num8 * num12;
					num4 -= (float)num6 * num12;
				}
				else
				{
					num2 += (float)num6 * num12;
					num4 -= (float)num8 * num12;
				}
			}
			Vector4 vector = (this.mAtlas != null) ? (this.border * this.pixelSize) : Vector4.zero;
			float num13 = vector.x + vector.z;
			float num14 = vector.y + vector.w;
			float x = Mathf.Lerp(num, num3 - num13, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num13, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w);
			return new Vector4(x, y, z, w);
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00035CC6 File Offset: 0x00033EC6
	public override bool premultipliedAlpha
	{
		get
		{
			return this.mAtlas != null && this.mAtlas.premultipliedAlpha;
		}
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00035CE4 File Offset: 0x00033EE4
	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData uispriteData = this.mAtlas.spriteList[0];
				if (uispriteData == null)
				{
					return null;
				}
				this.SetAtlasSprite(uispriteData);
				if (this.mSprite == null)
				{
					Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00035DBC File Offset: 0x00033FBC
	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
			return;
		}
		this.mSpriteName = ((this.mSprite != null) ? this.mSprite.name : "");
		this.mSprite = sp;
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00035E1C File Offset: 0x0003401C
	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		UISpriteData atlasSprite = this.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !atlasSprite.hasBorder) && mainTexture != null)
		{
			int num = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
			int num2 = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00035EE6 File Offset: 0x000340E6
	public void DoNotInitTheme()
	{
		this.BeenInit = true;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00035EEF File Offset: 0x000340EF
	protected override void OnInit()
	{
		if (!this.mFillCenter)
		{
			this.mFillCenter = true;
			this.centerType = UIBasicSprite.AdvancedType.Invisible;
		}
		base.OnInit();
		if (!this.BeenInit)
		{
			Singleton<UIPalette>.Instance.InitTheme(this, null);
		}
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00035F21 File Offset: 0x00034121
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x00035F50 File Offset: 0x00034150
	public override void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if (this.mSprite == null)
		{
			this.mSprite = this.atlas.GetSprite(this.spriteName);
		}
		if (this.mSprite == null)
		{
			return;
		}
		Rect rect = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
		Rect rect2 = new Rect((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
		rect = NGUIMath.ConvertToTexCoords(rect, mainTexture.width, mainTexture.height);
		rect2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
		int count = verts.Count;
		base.Fill(verts, uvs, cols, rect, rect2);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, count, verts, uvs, cols);
		}
	}

	// Token: 0x0400053E RID: 1342
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	// Token: 0x0400053F RID: 1343
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x04000540 RID: 1344
	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	// Token: 0x04000541 RID: 1345
	[NonSerialized]
	protected UISpriteData mSprite;

	// Token: 0x04000542 RID: 1346
	[NonSerialized]
	private bool mSpriteSet;

	// Token: 0x04000543 RID: 1347
	public UIPalette.UI ThemeAsSetting;

	// Token: 0x04000544 RID: 1348
	public UIPalette.UI ThemeAs = UIPalette.UI.DoNotTheme;

	// Token: 0x04000545 RID: 1349
	[NonSerialized]
	public bool BeenInit;
}
