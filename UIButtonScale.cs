using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	// Token: 0x06000108 RID: 264 RVA: 0x00006C79 File Offset: 0x00004E79
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mScale = this.tweenTarget.localScale;
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00006CB5 File Offset: 0x00004EB5
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00006CD0 File Offset: 0x00004ED0
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.value = this.mScale;
				component.enabled = false;
			}
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00006D1C File Offset: 0x00004F1C
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, isPressed ? Vector3.Scale(this.mScale, this.pressed) : (UICamera.IsHighlighted(base.gameObject) ? Vector3.Scale(this.mScale, this.hover) : this.mScale)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00006D98 File Offset: 0x00004F98
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, isOver ? Vector3.Scale(this.mScale, this.hover) : this.mScale).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00006DF3 File Offset: 0x00004FF3
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040000D8 RID: 216
	public Transform tweenTarget;

	// Token: 0x040000D9 RID: 217
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	// Token: 0x040000DA RID: 218
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	// Token: 0x040000DB RID: 219
	public float duration = 0.2f;

	// Token: 0x040000DC RID: 220
	private Vector3 mScale;

	// Token: 0x040000DD RID: 221
	private bool mStarted;
}
