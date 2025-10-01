using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002B RID: 43
[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005B24 File Offset: 0x00003D24
	// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005B70 File Offset: 0x00003D70
	public override bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider component = base.gameObject.GetComponent<Collider>();
			if (component && component.enabled)
			{
				return true;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 && component2.enabled;
		}
		set
		{
			if (this.isEnabled != value)
			{
				Collider component = base.gameObject.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = value;
					UIButton[] components = base.GetComponents<UIButton>();
					for (int i = 0; i < components.Length; i++)
					{
						components[i].SetState(value ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, false);
					}
					return;
				}
				Collider2D component2 = base.GetComponent<Collider2D>();
				if (component2 != null)
				{
					component2.enabled = value;
					UIButton[] components = base.GetComponents<UIButton>();
					for (int i = 0; i < components.Length; i++)
					{
						components[i].SetState(value ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, false);
					}
					return;
				}
				base.enabled = value;
			}
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005C0F File Offset: 0x00003E0F
	// (set) Token: 0x060000CA RID: 202 RVA: 0x00005C28 File Offset: 0x00003E28
	public string normalSprite
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite != null && !string.IsNullOrEmpty(this.mNormalSprite) && this.mNormalSprite == this.mSprite.spriteName)
			{
				this.mNormalSprite = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
				return;
			}
			this.mNormalSprite = value;
			if (this.mState == UIButtonColor.State.Normal)
			{
				this.SetSprite(value);
			}
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000CB RID: 203 RVA: 0x00005CA6 File Offset: 0x00003EA6
	// (set) Token: 0x060000CC RID: 204 RVA: 0x00005CBC File Offset: 0x00003EBC
	public Sprite normalSprite2D
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite2D;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite2D != null && this.mNormalSprite2D == this.mSprite2D.sprite2D)
			{
				this.mNormalSprite2D = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
				return;
			}
			this.mNormalSprite2D = value;
			if (this.mState == UIButtonColor.State.Normal)
			{
				this.SetSprite(value);
			}
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005D2D File Offset: 0x00003F2D
	public void DoNotInitTheme()
	{
		this.BeenInit = true;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005D38 File Offset: 0x00003F38
	protected override void OnInit()
	{
		base.OnInit();
		this.mSprite = (this.mWidget as UISprite);
		this.mSprite2D = (this.mWidget as UI2DSprite);
		if (this.mSprite != null)
		{
			this.mNormalSprite = this.mSprite.spriteName;
		}
		if (this.mSprite2D != null)
		{
			this.mNormalSprite2D = this.mSprite2D.sprite2D;
		}
		if (!this.BeenInit)
		{
			Singleton<UIPalette>.Instance.InitTheme(this, null);
		}
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005DBF File Offset: 0x00003FBF
	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mInitDone)
			{
				this.OnHover(UICamera.HoveredUIObject == base.gameObject);
				return;
			}
		}
		else
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005DF0 File Offset: 0x00003FF0
	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOver();
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005E1F File Offset: 0x0000401F
	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOut();
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005E4E File Offset: 0x0000404E
	protected virtual void OnClick()
	{
		if (UIButton.current == null && this.isEnabled && UICamera.currentTouchID != -2 && UICamera.currentTouchID != -3)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005E90 File Offset: 0x00004090
	public override void SetState(UIButtonColor.State state, bool immediate)
	{
		base.SetState(state, immediate);
		if (!(this.mSprite != null))
		{
			if (this.mSprite2D != null)
			{
				switch (state)
				{
				case UIButtonColor.State.Normal:
					this.SetSprite(this.mNormalSprite2D);
					return;
				case UIButtonColor.State.Hover:
					this.SetSprite((this.hoverSprite2D == null) ? this.mNormalSprite2D : this.hoverSprite2D);
					return;
				case UIButtonColor.State.Pressed:
					this.SetSprite(this.pressedSprite2D);
					return;
				case UIButtonColor.State.Disabled:
					this.SetSprite(this.disabledSprite2D);
					break;
				default:
					return;
				}
			}
			return;
		}
		switch (state)
		{
		case UIButtonColor.State.Normal:
			this.SetSprite(this.mNormalSprite);
			return;
		case UIButtonColor.State.Hover:
			this.SetSprite(string.IsNullOrEmpty(this.hoverSprite) ? this.mNormalSprite : this.hoverSprite);
			return;
		case UIButtonColor.State.Pressed:
			this.SetSprite(this.pressedSprite);
			return;
		case UIButtonColor.State.Disabled:
			this.SetSprite(this.disabledSprite);
			return;
		default:
			return;
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005F84 File Offset: 0x00004184
	protected void SetSprite(string sp)
	{
		if (this.mSprite != null && !string.IsNullOrEmpty(sp) && this.mSprite.spriteName != sp)
		{
			this.mSprite.spriteName = sp;
			if (this.pixelSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00005FDC File Offset: 0x000041DC
	protected void SetSprite(Sprite sp)
	{
		if (sp != null && this.mSprite2D != null && this.mSprite2D.sprite2D != sp)
		{
			this.mSprite2D.sprite2D = sp;
			if (this.pixelSnap)
			{
				this.mSprite2D.MakePixelPerfect();
			}
		}
	}

	// Token: 0x0400009D RID: 157
	public static UIButton current;

	// Token: 0x0400009E RID: 158
	public bool dragHighlight;

	// Token: 0x0400009F RID: 159
	public string hoverSprite;

	// Token: 0x040000A0 RID: 160
	public string pressedSprite;

	// Token: 0x040000A1 RID: 161
	public string disabledSprite;

	// Token: 0x040000A2 RID: 162
	public Sprite hoverSprite2D;

	// Token: 0x040000A3 RID: 163
	public Sprite pressedSprite2D;

	// Token: 0x040000A4 RID: 164
	public Sprite disabledSprite2D;

	// Token: 0x040000A5 RID: 165
	public bool pixelSnap;

	// Token: 0x040000A6 RID: 166
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x040000A7 RID: 167
	[NonSerialized]
	private UISprite mSprite;

	// Token: 0x040000A8 RID: 168
	[NonSerialized]
	private UI2DSprite mSprite2D;

	// Token: 0x040000A9 RID: 169
	[NonSerialized]
	private string mNormalSprite;

	// Token: 0x040000AA RID: 170
	[NonSerialized]
	private Sprite mNormalSprite2D;

	// Token: 0x040000AB RID: 171
	public UIPalette.UI ThemeNormalAsSetting;

	// Token: 0x040000AC RID: 172
	public UIPalette.UI ThemeHoverAsSetting;

	// Token: 0x040000AD RID: 173
	public UIPalette.UI ThemePressedAsSetting;

	// Token: 0x040000AE RID: 174
	public UIPalette.UI ThemeDisabledAsSetting;

	// Token: 0x040000AF RID: 175
	public UIPalette.UI ThemeNormalAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040000B0 RID: 176
	public UIPalette.UI ThemeHoverAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040000B1 RID: 177
	public UIPalette.UI ThemePressedAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040000B2 RID: 178
	public UIPalette.UI ThemeDisabledAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040000B3 RID: 179
	[NonSerialized]
	public bool BeenInit;
}
