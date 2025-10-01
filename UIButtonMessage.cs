using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour
{
	// Token: 0x060000EF RID: 239 RVA: 0x00006727 File Offset: 0x00004927
	private void Start()
	{
		this.mStarted = true;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00006730 File Offset: 0x00004930
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000674B File Offset: 0x0000494B
	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006773 File Offset: 0x00004973
	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x0000679B File Offset: 0x0000499B
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000067B7 File Offset: 0x000049B7
	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000067CF File Offset: 0x000049CF
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000067E8 File Offset: 0x000049E8
	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
				i++;
			}
			return;
		}
		this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x040000C6 RID: 198
	public GameObject target;

	// Token: 0x040000C7 RID: 199
	public string functionName;

	// Token: 0x040000C8 RID: 200
	public UIButtonMessage.Trigger trigger;

	// Token: 0x040000C9 RID: 201
	public bool includeChildren;

	// Token: 0x040000CA RID: 202
	private bool mStarted;

	// Token: 0x02000503 RID: 1283
	public enum Trigger
	{
		// Token: 0x04002393 RID: 9107
		OnClick,
		// Token: 0x04002394 RID: 9108
		OnMouseOver,
		// Token: 0x04002395 RID: 9109
		OnMouseOut,
		// Token: 0x04002396 RID: 9110
		OnPress,
		// Token: 0x04002397 RID: 9111
		OnRelease,
		// Token: 0x04002398 RID: 9112
		OnDoubleClick
	}
}
