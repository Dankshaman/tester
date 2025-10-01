using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000083 RID: 131
[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060005DD RID: 1501 RVA: 0x00028628 File Offset: 0x00026828
	// (set) Token: 0x060005DE RID: 1502 RVA: 0x0002864C File Offset: 0x0002684C
	public Material spriteMaterial
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.material;
			}
			return this.mReplacement.spriteMaterial;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
				return;
			}
			if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
				return;
			}
			this.MarkAsChanged();
			this.mPMA = -1;
			this.material = value;
			this.MarkAsChanged();
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x000286AC File Offset: 0x000268AC
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material spriteMaterial = this.spriteMaterial;
				this.mPMA = ((spriteMaterial != null && spriteMaterial.shader != null && spriteMaterial.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00028724 File Offset: 0x00026924
	// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0002875A File Offset: 0x0002695A
	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
				return;
			}
			this.mSprites = value;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0002877E File Offset: 0x0002697E
	public Texture texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			if (!(this.material != null))
			{
				return null;
			}
			return this.material.mainTexture;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000287B5 File Offset: 0x000269B5
	// (set) Token: 0x060005E4 RID: 1508 RVA: 0x000287D8 File Offset: 0x000269D8
	public float pixelSize
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mPixelSize;
			}
			return this.mReplacement.pixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
				return;
			}
			float num = Mathf.Clamp(value, 0.25f, 4f);
			if (this.mPixelSize != num)
			{
				this.mPixelSize = num;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00028827 File Offset: 0x00026A27
	// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00028830 File Offset: 0x00026A30
	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uiatlas = value;
			if (uiatlas == this)
			{
				uiatlas = null;
			}
			if (this.mReplacement != uiatlas)
			{
				if (uiatlas != null && uiatlas.replacement == this)
				{
					uiatlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uiatlas;
				if (uiatlas != null)
				{
					this.material = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x000288A8 File Offset: 0x00026AA8
	public UISpriteData GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			if (this.mSprites.Count == 0)
			{
				return null;
			}
			if (this.mSpriteIndices.Count != this.mSprites.Count)
			{
				this.MarkSpriteListAsChanged();
			}
			int num;
			if (this.mSpriteIndices.TryGetValue(name, out num))
			{
				if (num > -1 && num < this.mSprites.Count)
				{
					return this.mSprites[num];
				}
				this.MarkSpriteListAsChanged();
				if (!this.mSpriteIndices.TryGetValue(name, out num))
				{
					return null;
				}
				return this.mSprites[num];
			}
			else
			{
				int i = 0;
				int count = this.mSprites.Count;
				while (i < count)
				{
					UISpriteData uispriteData = this.mSprites[i];
					if (!string.IsNullOrEmpty(uispriteData.name) && name == uispriteData.name)
					{
						this.MarkSpriteListAsChanged();
						return uispriteData;
					}
					i++;
				}
			}
		}
		return null;
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x000289BC File Offset: 0x00026BBC
	public string GetRandomSprite(string startsWith)
	{
		if (this.GetSprite(startsWith) != null)
		{
			return startsWith;
		}
		List<UISpriteData> spriteList = this.spriteList;
		List<string> list = new List<string>();
		foreach (UISpriteData uispriteData in spriteList)
		{
			if (uispriteData.name.StartsWith(startsWith))
			{
				list.Add(uispriteData.name);
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00028A50 File Offset: 0x00026C50
	public void MarkSpriteListAsChanged()
	{
		this.mSpriteIndices.Clear();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			this.mSpriteIndices[this.mSprites[i].name] = i;
			i++;
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x00028A9D File Offset: 0x00026C9D
	public void SortAlphabetically()
	{
		this.mSprites.Sort((UISpriteData s1, UISpriteData s2) => s1.name.CompareTo(s2.name));
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x00028ACC File Offset: 0x00026CCC
	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name))
			{
				betterList.Add(uispriteData.name);
			}
			i++;
		}
		return betterList;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00028B50 File Offset: 0x00026D50
	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name) && string.Equals(match, uispriteData.name, StringComparison.OrdinalIgnoreCase))
			{
				betterList.Add(uispriteData.name);
				return betterList;
			}
			i++;
		}
		string[] array = match.Split(new char[]
		{
			' '
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int count2 = this.mSprites.Count;
		while (k < count2)
		{
			UISpriteData uispriteData2 = this.mSprites[k];
			if (uispriteData2 != null && !string.IsNullOrEmpty(uispriteData2.name))
			{
				string text = uispriteData2.name.ToLower();
				int num = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					betterList.Add(uispriteData2.name);
				}
			}
			k++;
		}
		return betterList;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x00028CB3 File Offset: 0x00026EB3
	private bool References(UIAtlas atlas)
	{
		return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x00028CE7 File Offset: 0x00026EE7
	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x00028D18 File Offset: 0x00026F18
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UISprite uisprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uisprite.atlas))
			{
				UIAtlas atlas = uisprite.atlas;
				uisprite.atlas = null;
				uisprite.atlas = atlas;
			}
			i++;
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			UIFont uifont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uifont.atlas))
			{
				UIAtlas atlas2 = uifont.atlas;
				uifont.atlas = null;
				uifont.atlas = atlas2;
			}
			j++;
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		int num3 = array3.Length;
		while (k < num3)
		{
			UILabel uilabel = array3[k];
			if (uilabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uilabel.bitmapFont.atlas))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			k++;
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00028E40 File Offset: 0x00027040
	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.Count == 0 && this.sprites.Count > 0 && this.material)
		{
			Texture mainTexture = this.material.mainTexture;
			int width = (mainTexture != null) ? mainTexture.width : 512;
			int height = (mainTexture != null) ? mainTexture.height : 512;
			for (int i = 0; i < this.sprites.Count; i++)
			{
				UIAtlas.Sprite sprite = this.sprites[i];
				Rect outer = sprite.outer;
				Rect inner = sprite.inner;
				if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
				{
					NGUIMath.ConvertToPixels(outer, width, height, true);
					NGUIMath.ConvertToPixels(inner, width, height, true);
				}
				UISpriteData uispriteData = new UISpriteData();
				uispriteData.name = sprite.name;
				uispriteData.x = Mathf.RoundToInt(outer.xMin);
				uispriteData.y = Mathf.RoundToInt(outer.yMin);
				uispriteData.width = Mathf.RoundToInt(outer.width);
				uispriteData.height = Mathf.RoundToInt(outer.height);
				uispriteData.paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width);
				uispriteData.paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width);
				uispriteData.paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height);
				uispriteData.paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height);
				uispriteData.borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin);
				uispriteData.borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax);
				uispriteData.borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax);
				uispriteData.borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin);
				this.mSprites.Add(uispriteData);
			}
			this.sprites.Clear();
			return true;
		}
		return false;
	}

	// Token: 0x04000401 RID: 1025
	[HideInInspector]
	[SerializeField]
	private Material material;

	// Token: 0x04000402 RID: 1026
	[HideInInspector]
	[SerializeField]
	private List<UISpriteData> mSprites = new List<UISpriteData>();

	// Token: 0x04000403 RID: 1027
	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	// Token: 0x04000404 RID: 1028
	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	// Token: 0x04000405 RID: 1029
	[HideInInspector]
	[SerializeField]
	private UIAtlas.Coordinates mCoordinates;

	// Token: 0x04000406 RID: 1030
	[HideInInspector]
	[SerializeField]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	// Token: 0x04000407 RID: 1031
	private int mPMA = -1;

	// Token: 0x04000408 RID: 1032
	private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

	// Token: 0x0200054D RID: 1357
	[Serializable]
	private class Sprite
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060037DA RID: 14298 RVA: 0x0016B4C9 File Offset: 0x001696C9
		public bool hasPadding
		{
			get
			{
				return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
			}
		}

		// Token: 0x0400247D RID: 9341
		public string name = "Unity Bug";

		// Token: 0x0400247E RID: 9342
		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400247F RID: 9343
		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04002480 RID: 9344
		public bool rotated;

		// Token: 0x04002481 RID: 9345
		public float paddingLeft;

		// Token: 0x04002482 RID: 9346
		public float paddingRight;

		// Token: 0x04002483 RID: 9347
		public float paddingTop;

		// Token: 0x04002484 RID: 9348
		public float paddingBottom;
	}

	// Token: 0x0200054E RID: 1358
	private enum Coordinates
	{
		// Token: 0x04002486 RID: 9350
		Pixels,
		// Token: 0x04002487 RID: 9351
		TexCoords
	}
}
