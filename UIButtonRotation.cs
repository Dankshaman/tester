using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	// Token: 0x06000101 RID: 257 RVA: 0x00006AA9 File Offset: 0x00004CA9
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mRot = this.tweenTarget.localRotation;
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006AE5 File Offset: 0x00004CE5
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00006B00 File Offset: 0x00004D00
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.value = this.mRot;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006B4C File Offset: 0x00004D4C
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? (this.mRot * Quaternion.Euler(this.pressed)) : (UICamera.IsHighlighted(base.gameObject) ? (this.mRot * Quaternion.Euler(this.hover)) : this.mRot)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00006BD4 File Offset: 0x00004DD4
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, isOver ? (this.mRot * Quaternion.Euler(this.hover)) : this.mRot).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00006C34 File Offset: 0x00004E34
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040000D2 RID: 210
	public Transform tweenTarget;

	// Token: 0x040000D3 RID: 211
	public Vector3 hover = Vector3.zero;

	// Token: 0x040000D4 RID: 212
	public Vector3 pressed = Vector3.zero;

	// Token: 0x040000D5 RID: 213
	public float duration = 0.2f;

	// Token: 0x040000D6 RID: 214
	private Quaternion mRot;

	// Token: 0x040000D7 RID: 215
	private bool mStarted;
}
