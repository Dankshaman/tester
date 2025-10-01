using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
public class UISlider : UIProgressBar
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600027E RID: 638 RVA: 0x000107F8 File Offset: 0x0000E9F8
	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600027F RID: 639 RVA: 0x0000E996 File Offset: 0x0000CB96
	// (set) Token: 0x06000280 RID: 640 RVA: 0x0000E99E File Offset: 0x0000CB9E
	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000281 RID: 641 RVA: 0x00010834 File Offset: 0x0000EA34
	// (set) Token: 0x06000282 RID: 642 RVA: 0x000025B8 File Offset: 0x000007B8
	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0001083C File Offset: 0x0000EA3C
	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = (this.mInverted ? UIProgressBar.FillDirection.RightToLeft : UIProgressBar.FillDirection.LeftToRight);
			}
			else
			{
				this.mFill = (this.mInverted ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000108B4 File Offset: 0x0000EAB4
	protected override void OnStart()
	{
		UIEventListener uieventListener = UIEventListener.Get((this.mBG != null && (this.mBG.GetComponent<Collider>() != null || this.mBG.GetComponent<Collider2D>() != null)) ? this.mBG.gameObject : base.gameObject);
		uieventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uieventListener2 = UIEventListener.Get(this.thumb.gameObject);
			uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00010A00 File Offset: 0x0000EC00
	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (this.ShouldCancel(isPressed))
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
			this.mDragStarted = false;
		}
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00010A59 File Offset: 0x0000EC59
	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (this.ShouldCancel(true))
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00010A8C File Offset: 0x0000EC8C
	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (!isPressed)
		{
			if (this.onDragFinished != null)
			{
				if (this.ShouldCancel(isPressed))
				{
					return;
				}
				this.onDragFinished();
				this.mDragStarted = false;
			}
			return;
		}
		if (zInput.GetButton("Tap", ControlType.All))
		{
			if (this.attachedSliderRange)
			{
				this.attachedSliderRange.SetWithDialog();
			}
			return;
		}
		if (!this.mDragStarted)
		{
			this.mDragStarted = true;
			if (this.onDragStart != null)
			{
				this.onDragStart();
			}
		}
		this.mOffset = ((this.mFG == null) ? 0f : (base.value - base.ScreenToValue(UICamera.lastEventPosition)));
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00010B4A File Offset: 0x0000ED4A
	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (this.ShouldCancel(true))
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastEventPosition);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00010B82 File Offset: 0x0000ED82
	public override void OnPan(Vector2 delta)
	{
		if (this.ShouldCancel(true))
		{
			return;
		}
		if (base.enabled && this.isColliderEnabled)
		{
			base.OnPan(delta);
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00010BA5 File Offset: 0x0000EDA5
	private bool ShouldCancel(bool isPress = true)
	{
		if (isPress)
		{
			this.rightClick = zInput.GetButton("Tap", ControlType.All);
			return this.rightClick;
		}
		return this.rightClick;
	}

	// Token: 0x04000229 RID: 553
	[NonSerialized]
	public UISliderRange attachedSliderRange;

	// Token: 0x0400022A RID: 554
	[HideInInspector]
	[SerializeField]
	private Transform foreground;

	// Token: 0x0400022B RID: 555
	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	// Token: 0x0400022C RID: 556
	[HideInInspector]
	[SerializeField]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	// Token: 0x0400022D RID: 557
	[HideInInspector]
	[SerializeField]
	protected bool mInverted;

	// Token: 0x0400022E RID: 558
	private bool rightClick;

	// Token: 0x0200051D RID: 1309
	private enum Direction
	{
		// Token: 0x040023F1 RID: 9201
		Horizontal,
		// Token: 0x040023F2 RID: 9202
		Vertical,
		// Token: 0x040023F3 RID: 9203
		Upgraded
	}
}
