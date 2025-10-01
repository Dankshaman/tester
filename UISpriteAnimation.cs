using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008F RID: 143
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000360B2 File Offset: 0x000342B2
	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000360BF File Offset: 0x000342BF
	// (set) Token: 0x060007B3 RID: 1971 RVA: 0x000360C7 File Offset: 0x000342C7
	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060007B4 RID: 1972 RVA: 0x000360D0 File Offset: 0x000342D0
	// (set) Token: 0x060007B5 RID: 1973 RVA: 0x000360D8 File Offset: 0x000342D8
	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000360F5 File Offset: 0x000342F5
	// (set) Token: 0x060007B7 RID: 1975 RVA: 0x000360FD File Offset: 0x000342FD
	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00036106 File Offset: 0x00034306
	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0003610E File Offset: 0x0003430E
	protected virtual void Start()
	{
		this.RebuildSpriteList();
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00036118 File Offset: 0x00034318
	protected virtual void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && this.mFPS > 0)
		{
			this.mDelta += Mathf.Min(1f, RealTime.deltaTime);
			float num = 1f / (float)this.mFPS;
			while (num < this.mDelta)
			{
				this.mDelta = ((num > 0f) ? (this.mDelta - num) : 0f);
				int num2 = this.frameIndex + 1;
				this.frameIndex = num2;
				if (num2 >= this.mSpriteNames.Count)
				{
					this.frameIndex = 0;
					this.mActive = this.mLoop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.frameIndex];
					if (this.mSnap)
					{
						this.mSprite.MakePixelPerfect();
					}
				}
			}
		}
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00036218 File Offset: 0x00034418
	public void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uispriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.mPrefix) || uispriteData.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(uispriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x000362D3 File Offset: 0x000344D3
	public void Play()
	{
		this.mActive = true;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x000362DC File Offset: 0x000344DC
	public void Pause()
	{
		this.mActive = false;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x000362E8 File Offset: 0x000344E8
	public void ResetToBeginning()
	{
		this.mActive = true;
		this.frameIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.frameIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x04000546 RID: 1350
	public int frameIndex;

	// Token: 0x04000547 RID: 1351
	[HideInInspector]
	[SerializeField]
	protected int mFPS = 30;

	// Token: 0x04000548 RID: 1352
	[HideInInspector]
	[SerializeField]
	protected string mPrefix = "";

	// Token: 0x04000549 RID: 1353
	[HideInInspector]
	[SerializeField]
	protected bool mLoop = true;

	// Token: 0x0400054A RID: 1354
	[HideInInspector]
	[SerializeField]
	protected bool mSnap = true;

	// Token: 0x0400054B RID: 1355
	protected UISprite mSprite;

	// Token: 0x0400054C RID: 1356
	protected float mDelta;

	// Token: 0x0400054D RID: 1357
	protected bool mActive = true;

	// Token: 0x0400054E RID: 1358
	protected List<string> mSpriteNames = new List<string>();
}
