using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000176 RID: 374 RVA: 0x00009CBC File Offset: 0x00007EBC
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

	// Token: 0x06000177 RID: 375 RVA: 0x00009CF8 File Offset: 0x00007EF8
	private void OnHover(bool isOver)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (isOver)
		{
			EventDelegate.Execute(this.onHoverOver);
		}
		else
		{
			EventDelegate.Execute(this.onHoverOut);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00009D37 File Offset: 0x00007F37
	private void OnPress(bool pressed)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (pressed)
		{
			EventDelegate.Execute(this.onPress);
		}
		else
		{
			EventDelegate.Execute(this.onRelease);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00009D76 File Offset: 0x00007F76
	private void OnSelect(bool selected)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (selected)
		{
			EventDelegate.Execute(this.onSelect);
		}
		else
		{
			EventDelegate.Execute(this.onDeselect);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00009DB5 File Offset: 0x00007FB5
	private void OnClick()
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00009DE4 File Offset: 0x00007FE4
	private void OnDoubleClick()
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDoubleClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00009E13 File Offset: 0x00008013
	private void OnDragStart()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragStart);
		UIEventTrigger.current = null;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00009E3A File Offset: 0x0000803A
	private void OnDragEnd()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragEnd);
		UIEventTrigger.current = null;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00009E61 File Offset: 0x00008061
	private void OnDragOver(GameObject go)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOver);
		UIEventTrigger.current = null;
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00009E90 File Offset: 0x00008090
	private void OnDragOut(GameObject go)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x00009EBF File Offset: 0x000080BF
	private void OnDrag(Vector2 delta)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDrag);
		UIEventTrigger.current = null;
	}

	// Token: 0x04000146 RID: 326
	public static UIEventTrigger current;

	// Token: 0x04000147 RID: 327
	public List<EventDelegate> onHoverOver = new List<EventDelegate>();

	// Token: 0x04000148 RID: 328
	public List<EventDelegate> onHoverOut = new List<EventDelegate>();

	// Token: 0x04000149 RID: 329
	public List<EventDelegate> onPress = new List<EventDelegate>();

	// Token: 0x0400014A RID: 330
	public List<EventDelegate> onRelease = new List<EventDelegate>();

	// Token: 0x0400014B RID: 331
	public List<EventDelegate> onSelect = new List<EventDelegate>();

	// Token: 0x0400014C RID: 332
	public List<EventDelegate> onDeselect = new List<EventDelegate>();

	// Token: 0x0400014D RID: 333
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x0400014E RID: 334
	public List<EventDelegate> onDoubleClick = new List<EventDelegate>();

	// Token: 0x0400014F RID: 335
	public List<EventDelegate> onDragStart = new List<EventDelegate>();

	// Token: 0x04000150 RID: 336
	public List<EventDelegate> onDragEnd = new List<EventDelegate>();

	// Token: 0x04000151 RID: 337
	public List<EventDelegate> onDragOver = new List<EventDelegate>();

	// Token: 0x04000152 RID: 338
	public List<EventDelegate> onDragOut = new List<EventDelegate>();

	// Token: 0x04000153 RID: 339
	public List<EventDelegate> onDrag = new List<EventDelegate>();
}
