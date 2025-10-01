using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000BCAC File Offset: 0x00009EAC
	private bool canPlay
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			UIButton component = base.GetComponent<UIButton>();
			return component == null || component.isEnabled;
		}
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000BCDB File Offset: 0x00009EDB
	private void OnEnable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnEnable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000BCFE File Offset: 0x00009EFE
	private void OnDisable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnDisable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000BD24 File Offset: 0x00009F24
	private void OnHover(bool isOver)
	{
		if (this.trigger == UIPlaySound.Trigger.OnMouseOver)
		{
			if (this.mIsOver == isOver)
			{
				return;
			}
			this.mIsOver = isOver;
		}
		if (this.canPlay && ((isOver && this.trigger == UIPlaySound.Trigger.OnMouseOver) || (!isOver && this.trigger == UIPlaySound.Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000BD84 File Offset: 0x00009F84
	private void OnPress(bool isPressed)
	{
		if (this.trigger == UIPlaySound.Trigger.OnPress)
		{
			if (this.mIsOver == isPressed)
			{
				return;
			}
			this.mIsOver = isPressed;
		}
		if (this.canPlay && ((isPressed && this.trigger == UIPlaySound.Trigger.OnPress) || (!isPressed && this.trigger == UIPlaySound.Trigger.OnRelease)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000BDE3 File Offset: 0x00009FE3
	private void OnClick()
	{
		if (this.canPlay && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000BE0D File Offset: 0x0000A00D
	private void OnSelect(bool isSelected)
	{
		if (this.canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000BE29 File Offset: 0x0000A029
	public void Play()
	{
		NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
	}

	// Token: 0x0400019A RID: 410
	public AudioClip audioClip;

	// Token: 0x0400019B RID: 411
	public UIPlaySound.Trigger trigger;

	// Token: 0x0400019C RID: 412
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x0400019D RID: 413
	[Range(0f, 2f)]
	public float pitch = 1f;

	// Token: 0x0400019E RID: 414
	private bool mIsOver;

	// Token: 0x0200050F RID: 1295
	public enum Trigger
	{
		// Token: 0x040023C2 RID: 9154
		OnClick,
		// Token: 0x040023C3 RID: 9155
		OnMouseOver,
		// Token: 0x040023C4 RID: 9156
		OnMouseOut,
		// Token: 0x040023C5 RID: 9157
		OnPress,
		// Token: 0x040023C6 RID: 9158
		OnRelease,
		// Token: 0x040023C7 RID: 9159
		Custom,
		// Token: 0x040023C8 RID: 9160
		OnEnable,
		// Token: 0x040023C9 RID: 9161
		OnDisable
	}
}
