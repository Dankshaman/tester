using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
[Serializable]
public class BMSymbol
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060002DB RID: 731 RVA: 0x00012F13 File Offset: 0x00011113
	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060002DC RID: 732 RVA: 0x00012F34 File Offset: 0x00011134
	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060002DD RID: 733 RVA: 0x00012F3C File Offset: 0x0001113C
	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060002DE RID: 734 RVA: 0x00012F44 File Offset: 0x00011144
	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060002DF RID: 735 RVA: 0x00012F4C File Offset: 0x0001114C
	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x00012F54 File Offset: 0x00011154
	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x00012F5C File Offset: 0x0001115C
	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00012F64 File Offset: 0x00011164
	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00012F70 File Offset: 0x00011170
	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((atlas != null) ? atlas.GetSprite(this.spriteName) : null);
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}

	// Token: 0x04000281 RID: 641
	public string sequence;

	// Token: 0x04000282 RID: 642
	public string spriteName;

	// Token: 0x04000283 RID: 643
	private UISpriteData mSprite;

	// Token: 0x04000284 RID: 644
	private bool mIsValid;

	// Token: 0x04000285 RID: 645
	private int mLength;

	// Token: 0x04000286 RID: 646
	private int mOffsetX;

	// Token: 0x04000287 RID: 647
	private int mOffsetY;

	// Token: 0x04000288 RID: 648
	private int mWidth;

	// Token: 0x04000289 RID: 649
	private int mHeight;

	// Token: 0x0400028A RID: 650
	private int mAdvance;

	// Token: 0x0400028B RID: 651
	private Rect mUV;
}
