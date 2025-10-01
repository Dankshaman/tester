﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000080 RID: 128
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite")]
public class UI2DSprite : UIBasicSprite
{
	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x000273A4 File Offset: 0x000255A4
	// (set) Token: 0x060005BC RID: 1468 RVA: 0x000273AC File Offset: 0x000255AC
	public Sprite sprite2D
	{
		get
		{
			return this.mSprite;
		}
		set
		{
			if (this.mSprite != value)
			{
				base.RemoveFromPanel();
				this.mSprite = value;
				this.nextSprite = null;
				base.CreatePanel();
			}
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x00023937 File Offset: 0x00021B37
	// (set) Token: 0x060005BE RID: 1470 RVA: 0x000273D7 File Offset: 0x000255D7
	public override Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				base.RemoveFromPanel();
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x00027401 File Offset: 0x00025601
	// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00027441 File Offset: 0x00025641
	public override Shader shader
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.shader;
			}
			if (this.mShader == null)
			{
				this.mShader = Shader.Find("Unlit/Transparent Colored");
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				base.RemoveFromPanel();
				this.mShader = value;
				if (this.mMat == null)
				{
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00027479 File Offset: 0x00025679
	public override Texture mainTexture
	{
		get
		{
			if (this.mSprite != null)
			{
				return this.mSprite.texture;
			}
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060005C2 RID: 1474 RVA: 0x000274B0 File Offset: 0x000256B0
	// (set) Token: 0x060005C3 RID: 1475 RVA: 0x000274B8 File Offset: 0x000256B8
	public bool fixedAspect
	{
		get
		{
			return this.mFixedAspect;
		}
		set
		{
			if (this.mFixedAspect != value)
			{
				this.mFixedAspect = value;
				this.mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060005C4 RID: 1476 RVA: 0x000274F0 File Offset: 0x000256F0
	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Shader shader = this.shader;
				this.mPMA = ((shader != null && shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0002753B File Offset: 0x0002573B
	public override float pixelSize
	{
		get
		{
			return this.mPixelSize;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00027544 File Offset: 0x00025744
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.mSprite != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num5 = Mathf.RoundToInt(this.mSprite.rect.width);
				int num6 = Mathf.RoundToInt(this.mSprite.rect.height);
				int num7 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
				int num8 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
				int num9 = Mathf.RoundToInt(this.mSprite.rect.width - this.mSprite.textureRect.width - this.mSprite.textureRectOffset.x);
				int num10 = Mathf.RoundToInt(this.mSprite.rect.height - this.mSprite.textureRect.height - this.mSprite.textureRectOffset.y);
				float num11 = 1f;
				float num12 = 1f;
				if (num5 > 0 && num6 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num5 & 1) != 0)
					{
						num9++;
					}
					if ((num6 & 1) != 0)
					{
						num10++;
					}
					num11 = 1f / (float)num5 * (float)this.mWidth;
					num12 = 1f / (float)num6 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num9 * num11;
					num3 -= (float)num7 * num11;
				}
				else
				{
					num += (float)num7 * num11;
					num3 -= (float)num9 * num11;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num10 * num12;
					num4 -= (float)num8 * num12;
				}
				else
				{
					num2 += (float)num8 * num12;
					num4 -= (float)num10 * num12;
				}
			}
			float num13;
			float num14;
			if (this.mFixedAspect)
			{
				num13 = 0f;
				num14 = 0f;
			}
			else
			{
				Vector4 vector = this.border * this.pixelSize;
				num13 = vector.x + vector.z;
				num14 = vector.y + vector.w;
			}
			float x = Mathf.Lerp(num, num3 - num13, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num13, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w);
			return new Vector4(x, y, z, w);
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00027816 File Offset: 0x00025A16
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0002781E File Offset: 0x00025A1E
	public override Vector4 border
	{
		get
		{
			return this.mBorder;
		}
		set
		{
			if (this.mBorder != value)
			{
				this.mBorder = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002783C File Offset: 0x00025A3C
	protected override void OnUpdate()
	{
		if (this.nextSprite != null)
		{
			if (this.nextSprite != this.mSprite)
			{
				this.sprite2D = this.nextSprite;
			}
			this.nextSprite = null;
		}
		base.OnUpdate();
		if (this.mFixedAspect && this.mainTexture != null)
		{
			float num = (float)Mathf.RoundToInt(this.mSprite.rect.width);
			int num2 = Mathf.RoundToInt(this.mSprite.rect.height);
			int num3 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
			int num4 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
			int num5 = Mathf.RoundToInt(this.mSprite.rect.width - this.mSprite.textureRect.width - this.mSprite.textureRectOffset.x);
			int num6 = Mathf.RoundToInt(this.mSprite.rect.height - this.mSprite.textureRect.height - this.mSprite.textureRectOffset.y);
			float num7 = num + (float)(num3 + num5);
			num2 += num6 + num4;
			float num8 = (float)this.mWidth;
			float num9 = (float)this.mHeight;
			float num10 = num8 / num9;
			float num11 = num7 / (float)num2;
			if (num11 < num10)
			{
				float num12 = (num8 - num9 * num11) / num8 * 0.5f;
				base.drawRegion = new Vector4(num12, 0f, 1f - num12, 1f);
				return;
			}
			float num13 = (num9 - num8 / num11) / num9 * 0.5f;
			base.drawRegion = new Vector4(0f, num13, 1f, 1f - num13);
		}
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00027A14 File Offset: 0x00025C14
	public override void MakePixelPerfect()
	{
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !base.hasBorder) && mainTexture != null)
		{
			Rect rect = this.mSprite.rect;
			int num = Mathf.RoundToInt(this.pixelSize * rect.width);
			int num2 = Mathf.RoundToInt(this.pixelSize * rect.height);
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

	// Token: 0x060005CB RID: 1483 RVA: 0x00027ABC File Offset: 0x00025CBC
	public override void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Rect rect = (this.mSprite != null) ? this.mSprite.textureRect : new Rect(0f, 0f, (float)mainTexture.width, (float)mainTexture.height);
		Rect inner = rect;
		Vector4 border = this.border;
		inner.xMin += border.x;
		inner.yMin += border.y;
		inner.xMax -= border.z;
		inner.yMax -= border.w;
		float num = 1f / (float)mainTexture.width;
		float num2 = 1f / (float)mainTexture.height;
		rect.xMin *= num;
		rect.xMax *= num;
		rect.yMin *= num2;
		rect.yMax *= num2;
		inner.xMin *= num;
		inner.xMax *= num;
		inner.yMin *= num2;
		inner.yMax *= num2;
		int count = verts.Count;
		base.Fill(verts, uvs, cols, rect, inner);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, count, verts, uvs, cols);
		}
	}

	// Token: 0x040003E6 RID: 998
	[HideInInspector]
	[SerializeField]
	private Sprite mSprite;

	// Token: 0x040003E7 RID: 999
	[HideInInspector]
	[SerializeField]
	private Shader mShader;

	// Token: 0x040003E8 RID: 1000
	[HideInInspector]
	[SerializeField]
	private Vector4 mBorder = Vector4.zero;

	// Token: 0x040003E9 RID: 1001
	[HideInInspector]
	[SerializeField]
	private bool mFixedAspect;

	// Token: 0x040003EA RID: 1002
	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	// Token: 0x040003EB RID: 1003
	public Sprite nextSprite;

	// Token: 0x040003EC RID: 1004
	[NonSerialized]
	private int mPMA = -1;
}
