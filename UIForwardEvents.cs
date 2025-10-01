using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[AddComponentMenu("NGUI/Interaction/Forward Events (Legacy)")]
public class UIForwardEvents : MonoBehaviour
{
	// Token: 0x06000186 RID: 390 RVA: 0x0000A196 File Offset: 0x00008396
	private void OnHover(bool isOver)
	{
		if (this.onHover && this.target != null)
		{
			this.target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A1C5 File Offset: 0x000083C5
	private void OnPress(bool pressed)
	{
		if (this.onPress && this.target != null)
		{
			this.target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000A1F4 File Offset: 0x000083F4
	private void OnClick()
	{
		if (this.onClick && this.target != null)
		{
			this.target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000A21D File Offset: 0x0000841D
	private void OnDoubleClick()
	{
		if (this.onDoubleClick && this.target != null)
		{
			this.target.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000A246 File Offset: 0x00008446
	private void OnSelect(bool selected)
	{
		if (this.onSelect && this.target != null)
		{
			this.target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000A275 File Offset: 0x00008475
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag && this.target != null)
		{
			this.target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000A2A4 File Offset: 0x000084A4
	private void OnDrop(GameObject go)
	{
		if (this.onDrop && this.target != null)
		{
			this.target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000A2CE File Offset: 0x000084CE
	private void OnSubmit()
	{
		if (this.onSubmit && this.target != null)
		{
			this.target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000A2F7 File Offset: 0x000084F7
	private void OnScroll(float delta)
	{
		if (this.onScroll && this.target != null)
		{
			this.target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x04000158 RID: 344
	public GameObject target;

	// Token: 0x04000159 RID: 345
	public bool onHover;

	// Token: 0x0400015A RID: 346
	public bool onPress;

	// Token: 0x0400015B RID: 347
	public bool onClick;

	// Token: 0x0400015C RID: 348
	public bool onDoubleClick;

	// Token: 0x0400015D RID: 349
	public bool onSelect;

	// Token: 0x0400015E RID: 350
	public bool onDrag;

	// Token: 0x0400015F RID: 351
	public bool onDrop;

	// Token: 0x04000160 RID: 352
	public bool onSubmit;

	// Token: 0x04000161 RID: 353
	public bool onScroll;
}
