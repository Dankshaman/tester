using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000047 RID: 71
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Tween")]
public class UIPlayTween : MonoBehaviour
{
	// Token: 0x060001EC RID: 492 RVA: 0x0000BE61 File Offset: 0x0000A061
	private void Awake()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000BE8C File Offset: 0x0000A08C
	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000BEB0 File Offset: 0x0000A0B0
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

	// Token: 0x060001EF RID: 495 RVA: 0x0000BF60 File Offset: 0x0000A160
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000BF95 File Offset: 0x0000A195
	private void OnDragOver()
	{
		if (this.trigger == Trigger.OnHover)
		{
			this.OnHover(true);
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
	private void OnHover(bool isOver)
	{
		if (base.enabled && (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver)))
		{
			if (isOver == this.mActivated)
			{
				return;
			}
			if (!isOver && UICamera.hoveredObject != null && UICamera.hoveredObject.transform.IsChildOf(base.transform))
			{
				UICamera.onHover = (UICamera.BoolDelegate)Delegate.Combine(UICamera.onHover, new UICamera.BoolDelegate(this.CustomHoverListener));
				isOver = true;
				if (this.mActivated)
				{
					return;
				}
			}
			this.mActivated = (isOver && this.trigger == Trigger.OnHover);
			this.Play(isOver);
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000C05C File Offset: 0x0000A25C
	private void CustomHoverListener(GameObject go, bool isOver)
	{
		if (!this)
		{
			return;
		}
		GameObject gameObject = base.gameObject;
		if (!gameObject || !go || (!(go == gameObject) && !go.transform.IsChildOf(base.transform)))
		{
			this.OnHover(false);
			UICamera.onHover = (UICamera.BoolDelegate)Delegate.Remove(UICamera.onHover, new UICamera.BoolDelegate(this.CustomHoverListener));
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000C0D2 File Offset: 0x0000A2D2
	private void OnDragOut()
	{
		if (base.enabled && this.mActivated)
		{
			this.mActivated = false;
			this.Play(false);
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000C0F4 File Offset: 0x0000A2F4
	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.mActivated = (isPressed && this.trigger == Trigger.OnPress);
			this.Play(isPressed);
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000C147 File Offset: 0x0000A347
	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000C160 File Offset: 0x0000A360
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000C17C File Offset: 0x0000A37C
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.mActivated = (isSelected && this.trigger == Trigger.OnSelect);
			this.Play(isSelected);
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000C1D4 File Offset: 0x0000A3D4
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value);
		}
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000C23C File Offset: 0x0000A43C
	private void Update()
	{
		if (this.disableWhenFinished != DisableCondition.DoNotDisable && this.mTweens != null)
		{
			bool flag = true;
			bool flag2 = true;
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (uitweener.enabled)
					{
						flag = false;
						break;
					}
					if (uitweener.direction != (Direction)this.disableWhenFinished)
					{
						flag2 = false;
					}
				}
				i++;
			}
			if (flag)
			{
				if (flag2)
				{
					NGUITools.SetActive(this.tweenTarget, false);
				}
				this.mTweens = null;
			}
		}
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
	public void Play(bool forward)
	{
		this.mActive = 0;
		GameObject gameObject = (this.tweenTarget == null) ? base.gameObject : this.tweenTarget;
		if (!NGUITools.GetActive(gameObject))
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		this.mTweens = (this.includeChildren ? gameObject.GetComponentsInChildren<UITweener>() : gameObject.GetComponents<UITweener>());
		if (this.mTweens.Length == 0)
		{
			if (this.disableWhenFinished != DisableCondition.DoNotDisable)
			{
				NGUITools.SetActive(this.tweenTarget, false);
				return;
			}
		}
		else
		{
			bool flag = false;
			if (this.playDirection == Direction.Reverse)
			{
				forward = !forward;
			}
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (!flag && !NGUITools.GetActive(gameObject))
					{
						flag = true;
						NGUITools.SetActive(gameObject, true);
					}
					this.mActive++;
					if (this.playDirection == Direction.Toggle)
					{
						EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uitweener.Toggle();
					}
					else
					{
						if (this.resetOnPlay || (this.resetIfDisabled && !uitweener.enabled))
						{
							uitweener.Play(forward);
							uitweener.ResetToBeginning();
						}
						EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uitweener.Play(forward);
					}
				}
				i++;
			}
		}
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000C42C File Offset: 0x0000A62C
	private void OnFinished()
	{
		int num = this.mActive - 1;
		this.mActive = num;
		if (num == 0 && UIPlayTween.current == null)
		{
			UIPlayTween.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayTween.current = null;
		}
	}

	// Token: 0x0400019F RID: 415
	public static UIPlayTween current;

	// Token: 0x040001A0 RID: 416
	public GameObject tweenTarget;

	// Token: 0x040001A1 RID: 417
	public int tweenGroup;

	// Token: 0x040001A2 RID: 418
	public Trigger trigger;

	// Token: 0x040001A3 RID: 419
	public Direction playDirection = Direction.Forward;

	// Token: 0x040001A4 RID: 420
	public bool resetOnPlay;

	// Token: 0x040001A5 RID: 421
	public bool resetIfDisabled;

	// Token: 0x040001A6 RID: 422
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x040001A7 RID: 423
	public DisableCondition disableWhenFinished;

	// Token: 0x040001A8 RID: 424
	public bool includeChildren;

	// Token: 0x040001A9 RID: 425
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x040001AA RID: 426
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x040001AB RID: 427
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x040001AC RID: 428
	private UITweener[] mTweens;

	// Token: 0x040001AD RID: 429
	private bool mStarted;

	// Token: 0x040001AE RID: 430
	private int mActive;

	// Token: 0x040001AF RID: 431
	private bool mActivated;
}
