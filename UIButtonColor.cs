using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : UIWidgetContainer
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000D9 RID: 217 RVA: 0x000060C8 File Offset: 0x000042C8
	// (set) Token: 0x060000DA RID: 218 RVA: 0x000060D0 File Offset: 0x000042D0
	public UIButtonColor.State state
	{
		get
		{
			return this.mState;
		}
		set
		{
			this.SetState(value, false);
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000DB RID: 219 RVA: 0x000060DA File Offset: 0x000042DA
	// (set) Token: 0x060000DC RID: 220 RVA: 0x000060F0 File Offset: 0x000042F0
	public Color defaultColor
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mDefaultColor;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			this.mDefaultColor = value;
			UIButtonColor.State state = this.mState;
			this.mState = UIButtonColor.State.Disabled;
			this.SetState(state, false);
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000DD RID: 221 RVA: 0x00006128 File Offset: 0x00004328
	// (set) Token: 0x060000DE RID: 222 RVA: 0x00006130 File Offset: 0x00004330
	public virtual bool isEnabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00006139 File Offset: 0x00004339
	public void ResetDefaultColor()
	{
		this.defaultColor = this.mStartingColor;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00006147 File Offset: 0x00004347
	public void CacheDefaultColor()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00006157 File Offset: 0x00004357
	private void Start()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
		if (!this.isEnabled)
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00006178 File Offset: 0x00004378
	protected virtual void OnInit()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null && !Application.isPlaying)
		{
			this.tweenTarget = base.gameObject;
		}
		if (this.tweenTarget != null)
		{
			this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
		}
		if (this.mWidget != null)
		{
			this.mDefaultColor = this.mWidget.color;
			this.mStartingColor = this.mDefaultColor;
			return;
		}
		if (this.tweenTarget != null)
		{
			Renderer component = this.tweenTarget.GetComponent<Renderer>();
			if (component != null)
			{
				this.mDefaultColor = (Application.isPlaying ? component.material.color : component.sharedMaterial.color);
				this.mStartingColor = this.mDefaultColor;
				return;
			}
			Light component2 = this.tweenTarget.GetComponent<Light>();
			if (component2 != null)
			{
				this.mDefaultColor = component2.color;
				this.mStartingColor = this.mDefaultColor;
				return;
			}
			this.tweenTarget = null;
			this.mInitDone = false;
		}
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000628C File Offset: 0x0000448C
	protected virtual void OnEnable()
	{
		if (this.mInitDone)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (UICamera.currentTouch.pressed == base.gameObject)
			{
				this.OnPress(true);
				return;
			}
			if (UICamera.currentTouch.current == base.gameObject)
			{
				this.OnHover(true);
			}
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000062F8 File Offset: 0x000044F8
	protected virtual void OnDisable()
	{
		if (this.mInitDone && this.mState != UIButtonColor.State.Normal)
		{
			this.SetState(UIButtonColor.State.Normal, true);
			if (this.tweenTarget != null)
			{
				TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
				if (component != null)
				{
					component.value = this.mDefaultColor;
					component.enabled = false;
				}
			}
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00006353 File Offset: 0x00004553
	protected virtual void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(isOver ? UIButtonColor.State.Hover : UIButtonColor.State.Normal, false);
			}
		}
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00006388 File Offset: 0x00004588
	protected virtual void OnPress(bool isPressed)
	{
		if (this.isEnabled && UICamera.currentTouch != null)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				if (isPressed)
				{
					this.SetState(UIButtonColor.State.Pressed, false);
					return;
				}
				if (UICamera.currentTouch.current == base.gameObject)
				{
					if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
					{
						this.SetState(UIButtonColor.State.Hover, false);
						return;
					}
					if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.HoveredUIObject == base.gameObject)
					{
						this.SetState(UIButtonColor.State.Hover, false);
						return;
					}
					this.SetState(UIButtonColor.State.Normal, false);
					return;
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000642D File Offset: 0x0000462D
	protected virtual void OnDragOver()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Pressed, false);
			}
		}
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000645B File Offset: 0x0000465B
	protected virtual void OnDragOut()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Normal, false);
			}
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00006489 File Offset: 0x00004689
	public virtual void SetState(UIButtonColor.State state, bool instant)
	{
		if (!this.mInitDone)
		{
			this.mInitDone = true;
			this.OnInit();
		}
		if (this.mState != state)
		{
			this.mState = state;
			this.UpdateColor(instant);
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000064B8 File Offset: 0x000046B8
	public void UpdateColor(bool instant)
	{
		if (this.tweenTarget != null)
		{
			TweenColor tweenColor;
			switch (this.mState)
			{
			case UIButtonColor.State.Hover:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
				break;
			case UIButtonColor.State.Pressed:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
				break;
			case UIButtonColor.State.Disabled:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.disabledColor);
				break;
			default:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.defaultColor);
				break;
			}
			if (instant && tweenColor != null)
			{
				tweenColor.value = tweenColor.to;
				tweenColor.enabled = false;
			}
		}
	}

	// Token: 0x040000B7 RID: 183
	public GameObject tweenTarget;

	// Token: 0x040000B8 RID: 184
	public Color hover = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x040000B9 RID: 185
	public Color pressed = new Color(0.7176471f, 0.6392157f, 0.48235294f, 1f);

	// Token: 0x040000BA RID: 186
	public Color disabledColor = Color.grey;

	// Token: 0x040000BB RID: 187
	public float duration = 0.2f;

	// Token: 0x040000BC RID: 188
	[NonSerialized]
	protected Color mStartingColor;

	// Token: 0x040000BD RID: 189
	[NonSerialized]
	protected Color mDefaultColor;

	// Token: 0x040000BE RID: 190
	[NonSerialized]
	protected bool mInitDone;

	// Token: 0x040000BF RID: 191
	[NonSerialized]
	protected UIWidget mWidget;

	// Token: 0x040000C0 RID: 192
	[NonSerialized]
	protected UIButtonColor.State mState;

	// Token: 0x02000502 RID: 1282
	public enum State
	{
		// Token: 0x0400238E RID: 9102
		Normal,
		// Token: 0x0400238F RID: 9103
		Hover,
		// Token: 0x04002390 RID: 9104
		Pressed,
		// Token: 0x04002391 RID: 9105
		Disabled
	}
}
