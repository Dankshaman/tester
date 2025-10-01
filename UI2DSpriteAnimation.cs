using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class UI2DSpriteAnimation : MonoBehaviour
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060005CD RID: 1485 RVA: 0x00006128 File Offset: 0x00004328
	public bool isPlaying
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060005CE RID: 1486 RVA: 0x00027C5B File Offset: 0x00025E5B
	// (set) Token: 0x060005CF RID: 1487 RVA: 0x00027C63 File Offset: 0x00025E63
	public int framesPerSecond
	{
		get
		{
			return this.framerate;
		}
		set
		{
			this.framerate = value;
		}
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00027C6C File Offset: 0x00025E6C
	public void Play()
	{
		if (this.frames != null && this.frames.Length != 0)
		{
			if (!base.enabled && !this.loop)
			{
				int num = (this.framerate > 0) ? (this.frameIndex + 1) : (this.frameIndex - 1);
				if (num < 0 || num >= this.frames.Length)
				{
					this.frameIndex = ((this.framerate < 0) ? (this.frames.Length - 1) : 0);
				}
			}
			base.enabled = true;
			this.UpdateSprite();
		}
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00027CEE File Offset: 0x00025EEE
	public void Pause()
	{
		base.enabled = false;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00027CF7 File Offset: 0x00025EF7
	public void ResetToBeginning()
	{
		this.frameIndex = ((this.framerate < 0) ? (this.frames.Length - 1) : 0);
		this.UpdateSprite();
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00027D1B File Offset: 0x00025F1B
	private void Start()
	{
		this.Play();
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00027D24 File Offset: 0x00025F24
	private void Update()
	{
		if (this.frames == null || this.frames.Length == 0)
		{
			base.enabled = false;
			return;
		}
		if (this.framerate != 0)
		{
			float num = this.ignoreTimeScale ? RealTime.time : Time.time;
			if (this.mUpdate < num)
			{
				this.mUpdate = num;
				int num2 = (this.framerate > 0) ? (this.frameIndex + 1) : (this.frameIndex - 1);
				if (!this.loop && (num2 < 0 || num2 >= this.frames.Length))
				{
					base.enabled = false;
					return;
				}
				this.frameIndex = NGUIMath.RepeatIndex(num2, this.frames.Length);
				this.UpdateSprite();
			}
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00027DCC File Offset: 0x00025FCC
	private void UpdateSprite()
	{
		if (this.mUnitySprite == null && this.mNguiSprite == null)
		{
			this.mUnitySprite = base.GetComponent<SpriteRenderer>();
			this.mNguiSprite = base.GetComponent<UI2DSprite>();
			if (this.mUnitySprite == null && this.mNguiSprite == null)
			{
				base.enabled = false;
				return;
			}
		}
		float num = this.ignoreTimeScale ? RealTime.time : Time.time;
		if (this.framerate != 0)
		{
			this.mUpdate = num + Mathf.Abs(1f / (float)this.framerate);
		}
		if (this.mUnitySprite != null)
		{
			this.mUnitySprite.sprite = this.frames[this.frameIndex];
			return;
		}
		if (this.mNguiSprite != null)
		{
			this.mNguiSprite.nextSprite = this.frames[this.frameIndex];
		}
	}

	// Token: 0x040003ED RID: 1005
	public int frameIndex;

	// Token: 0x040003EE RID: 1006
	[SerializeField]
	protected int framerate = 20;

	// Token: 0x040003EF RID: 1007
	public bool ignoreTimeScale = true;

	// Token: 0x040003F0 RID: 1008
	public bool loop = true;

	// Token: 0x040003F1 RID: 1009
	public Sprite[] frames;

	// Token: 0x040003F2 RID: 1010
	private SpriteRenderer mUnitySprite;

	// Token: 0x040003F3 RID: 1011
	private UI2DSprite mNguiSprite;

	// Token: 0x040003F4 RID: 1012
	private float mUpdate;
}
