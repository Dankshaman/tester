using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000045 RID: 69
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Animation")]
public class UIPlayAnimation : MonoBehaviour
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060001CE RID: 462 RVA: 0x0000B6BD File Offset: 0x000098BD
	private bool dualState
	{
		get
		{
			return this.trigger == Trigger.OnPress || this.trigger == Trigger.OnHover;
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000B6D4 File Offset: 0x000098D4
	private void Awake()
	{
		UIButton component = base.GetComponent<UIButton>();
		if (component != null)
		{
			this.dragHighlight = component.dragHighlight;
		}
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000B728 File Offset: 0x00009928
	private void Start()
	{
		this.mStarted = true;
		if (this.target == null && this.animator == null)
		{
			this.animator = base.GetComponentInChildren<Animator>();
		}
		if (this.animator != null)
		{
			if (this.animator.enabled)
			{
				this.animator.enabled = false;
			}
			return;
		}
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<Animation>();
		}
		if (this.target != null && this.target.enabled)
		{
			this.target.enabled = false;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000B7D0 File Offset: 0x000099D0
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (this.trigger == Trigger.OnPress || this.trigger == Trigger.OnPressTrue)
			{
				this.mActivated = (UICamera.currentTouch.pressed == base.gameObject);
			}
			if (this.trigger == Trigger.OnHover || this.trigger == Trigger.OnHoverTrue)
			{
				this.mActivated = (UICamera.currentTouch.current == base.gameObject);
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000B880 File Offset: 0x00009A80
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000B8B5 File Offset: 0x00009AB5
	private void OnHover(bool isOver)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
		{
			this.Play(isOver, this.dualState);
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000B8F0 File Offset: 0x00009AF0
	private void OnPress(bool isPressed)
	{
		if (!base.enabled)
		{
			return;
		}
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed))
		{
			this.Play(isPressed, this.dualState);
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000B948 File Offset: 0x00009B48
	private void OnClick()
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000B975 File Offset: 0x00009B75
	private void OnDoubleClick()
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000B9A4 File Offset: 0x00009BA4
	private void OnSelect(bool isSelected)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected))
		{
			this.Play(isSelected, this.dualState);
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000B9E4 File Offset: 0x00009BE4
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value, this.dualState);
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000BA54 File Offset: 0x00009C54
	private void OnDragOver()
	{
		if (base.enabled && this.dualState)
		{
			if (UICamera.currentTouch.dragged == base.gameObject)
			{
				this.Play(true, true);
				return;
			}
			if (this.dragHighlight && this.trigger == Trigger.OnPress)
			{
				this.Play(true, true);
			}
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000BAAA File Offset: 0x00009CAA
	private void OnDragOut()
	{
		if (base.enabled && this.dualState && UICamera.hoveredObject != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000BAD6 File Offset: 0x00009CD6
	private void OnDrop(GameObject go)
	{
		if (base.enabled && this.trigger == Trigger.OnPress && UICamera.currentTouch.dragged != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000BB08 File Offset: 0x00009D08
	public void Play(bool forward)
	{
		this.Play(forward, true);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000BB14 File Offset: 0x00009D14
	public void Play(bool forward, bool onlyIfDifferent)
	{
		if (this.target || this.animator)
		{
			if (onlyIfDifferent)
			{
				if (this.mActivated == forward)
				{
					return;
				}
				this.mActivated = forward;
			}
			if (this.clearSelection && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			int num = (int)(-(int)this.playDirection);
			Direction direction = forward ? this.playDirection : ((Direction)num);
			ActiveAnimation activeAnimation = this.target ? ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.animator, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
			if (activeAnimation != null)
			{
				if (this.resetOnPlay)
				{
					activeAnimation.Reset();
				}
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
			}
		}
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000BC19 File Offset: 0x00009E19
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000BC22 File Offset: 0x00009E22
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000BC2C File Offset: 0x00009E2C
	private void OnFinished()
	{
		if (UIPlayAnimation.current == null)
		{
			UIPlayAnimation.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayAnimation.current = null;
		}
	}

	// Token: 0x0400018A RID: 394
	public static UIPlayAnimation current;

	// Token: 0x0400018B RID: 395
	public Animation target;

	// Token: 0x0400018C RID: 396
	public Animator animator;

	// Token: 0x0400018D RID: 397
	public string clipName;

	// Token: 0x0400018E RID: 398
	public Trigger trigger;

	// Token: 0x0400018F RID: 399
	public Direction playDirection = Direction.Forward;

	// Token: 0x04000190 RID: 400
	public bool resetOnPlay;

	// Token: 0x04000191 RID: 401
	public bool clearSelection;

	// Token: 0x04000192 RID: 402
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x04000193 RID: 403
	public DisableCondition disableWhenFinished;

	// Token: 0x04000194 RID: 404
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04000195 RID: 405
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x04000196 RID: 406
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x04000197 RID: 407
	private bool mStarted;

	// Token: 0x04000198 RID: 408
	private bool mActivated;

	// Token: 0x04000199 RID: 409
	private bool dragHighlight;
}
