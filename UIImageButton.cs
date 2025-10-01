using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000A950 File Offset: 0x00008B50
	// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000A97C File Offset: 0x00008B7C
	public bool isEnabled
	{
		get
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			return component && component.enabled;
		}
		set
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			if (!component)
			{
				return;
			}
			if (component.enabled != value)
			{
				component.enabled = value;
				this.UpdateImage();
			}
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000A9B4 File Offset: 0x00008BB4
	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000A9D8 File Offset: 0x00008BD8
	private void OnValidate()
	{
		if (this.target != null)
		{
			if (string.IsNullOrEmpty(this.normalSprite))
			{
				this.normalSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.hoverSprite))
			{
				this.hoverSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.pressedSprite))
			{
				this.pressedSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.disabledSprite))
			{
				this.disabledSprite = this.target.spriteName;
			}
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000AA6C File Offset: 0x00008C6C
	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.SetSprite(UICamera.IsHighlighted(base.gameObject) ? this.hoverSprite : this.normalSprite);
				return;
			}
			this.SetSprite(this.disabledSprite);
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000AABD File Offset: 0x00008CBD
	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.SetSprite(isOver ? this.hoverSprite : this.normalSprite);
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000AAEC File Offset: 0x00008CEC
	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.SetSprite(this.pressedSprite);
			return;
		}
		this.UpdateImage();
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000AB04 File Offset: 0x00008D04
	private void SetSprite(string sprite)
	{
		if (this.target.atlas == null || this.target.atlas.GetSprite(sprite) == null)
		{
			return;
		}
		this.target.spriteName = sprite;
		if (this.pixelSnap)
		{
			this.target.MakePixelPerfect();
		}
	}

	// Token: 0x04000171 RID: 369
	public UISprite target;

	// Token: 0x04000172 RID: 370
	public string normalSprite;

	// Token: 0x04000173 RID: 371
	public string hoverSprite;

	// Token: 0x04000174 RID: 372
	public string pressedSprite;

	// Token: 0x04000175 RID: 373
	public string disabledSprite;

	// Token: 0x04000176 RID: 374
	public bool pixelSnap = true;
}
