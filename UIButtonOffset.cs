using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	// Token: 0x060000F8 RID: 248 RVA: 0x00006874 File Offset: 0x00004A74
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mPos = this.tweenTarget.localPosition;
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x000068B0 File Offset: 0x00004AB0
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000068CC File Offset: 0x00004ACC
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.enabled = false;
			}
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006918 File Offset: 0x00004B18
	private void OnPress(bool isPressed)
	{
		this.mPressed = isPressed;
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? (this.mPos + this.pressed) : (UICamera.IsHighlighted(base.gameObject) ? (this.mPos + this.hover) : this.mPos)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x0000699C File Offset: 0x00004B9C
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, isOver ? (this.mPos + this.hover) : this.mPos).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000069F7 File Offset: 0x00004BF7
	private void OnDragOver()
	{
		if (this.mPressed)
		{
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos + this.hover).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00006A2E File Offset: 0x00004C2E
	private void OnDragOut()
	{
		if (this.mPressed)
		{
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006A5A File Offset: 0x00004C5A
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040000CB RID: 203
	public Transform tweenTarget;

	// Token: 0x040000CC RID: 204
	public Vector3 hover = Vector3.zero;

	// Token: 0x040000CD RID: 205
	public Vector3 pressed = new Vector3(2f, -2f);

	// Token: 0x040000CE RID: 206
	public float duration = 0.2f;

	// Token: 0x040000CF RID: 207
	[NonSerialized]
	private Vector3 mPos;

	// Token: 0x040000D0 RID: 208
	[NonSerialized]
	private bool mStarted;

	// Token: 0x040000D1 RID: 209
	[NonSerialized]
	private bool mPressed;
}
