using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000474 RID: 1140 RVA: 0x00021CF4 File Offset: 0x0001FEF4
	private bool isColliderEnabled
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

	// Token: 0x06000475 RID: 1141 RVA: 0x00021D30 File Offset: 0x0001FF30
	private void OnSubmit()
	{
		if (this.isColliderEnabled && this.onSubmit != null)
		{
			this.onSubmit(base.gameObject);
		}
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00021D53 File Offset: 0x0001FF53
	private void OnClick()
	{
		if (this.isColliderEnabled && this.onClick != null)
		{
			this.onClick(base.gameObject);
		}
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x00021D76 File Offset: 0x0001FF76
	private void OnDoubleClick()
	{
		if (this.isColliderEnabled && this.onDoubleClick != null)
		{
			this.onDoubleClick(base.gameObject);
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00021D99 File Offset: 0x0001FF99
	private void OnHover(bool isOver)
	{
		if (this.isColliderEnabled && this.onHover != null)
		{
			this.onHover(base.gameObject, isOver);
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00021DBD File Offset: 0x0001FFBD
	private void OnPress(bool isPressed)
	{
		if (this.isColliderEnabled && this.onPress != null)
		{
			this.onPress(base.gameObject, isPressed);
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00021DE1 File Offset: 0x0001FFE1
	private void OnSelect(bool selected)
	{
		if (this.isColliderEnabled && this.onSelect != null)
		{
			this.onSelect(base.gameObject, selected);
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00021E05 File Offset: 0x00020005
	private void OnScroll(float delta)
	{
		if (this.isColliderEnabled && this.onScroll != null)
		{
			this.onScroll(base.gameObject, delta);
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00021E29 File Offset: 0x00020029
	private void OnDragStart()
	{
		if (this.onDragStart != null)
		{
			this.onDragStart(base.gameObject);
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00021E44 File Offset: 0x00020044
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.gameObject, delta);
		}
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00021E60 File Offset: 0x00020060
	private void OnDragOver()
	{
		if (this.isColliderEnabled && this.onDragOver != null)
		{
			this.onDragOver(base.gameObject);
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00021E83 File Offset: 0x00020083
	private void OnDragOut()
	{
		if (this.isColliderEnabled && this.onDragOut != null)
		{
			this.onDragOut(base.gameObject);
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00021EA6 File Offset: 0x000200A6
	private void OnDragEnd()
	{
		if (this.onDragEnd != null)
		{
			this.onDragEnd(base.gameObject);
		}
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00021EC1 File Offset: 0x000200C1
	private void OnDrop(GameObject go)
	{
		if (this.isColliderEnabled && this.onDrop != null)
		{
			this.onDrop(base.gameObject, go);
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00021EE5 File Offset: 0x000200E5
	private void OnKey(KeyCode key)
	{
		if (this.isColliderEnabled && this.onKey != null)
		{
			this.onKey(base.gameObject, key);
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00021F09 File Offset: 0x00020109
	private void OnTooltip(bool show)
	{
		if (this.isColliderEnabled && this.onTooltip != null)
		{
			this.onTooltip(base.gameObject, show);
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00021F30 File Offset: 0x00020130
	public void Clear()
	{
		this.onSubmit = null;
		this.onClick = null;
		this.onDoubleClick = null;
		this.onHover = null;
		this.onPress = null;
		this.onSelect = null;
		this.onScroll = null;
		this.onDragStart = null;
		this.onDrag = null;
		this.onDragOver = null;
		this.onDragOut = null;
		this.onDragEnd = null;
		this.onDrop = null;
		this.onKey = null;
		this.onTooltip = null;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00021FA8 File Offset: 0x000201A8
	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uieventListener = go.GetComponent<UIEventListener>();
		if (uieventListener == null)
		{
			uieventListener = go.AddComponent<UIEventListener>();
		}
		return uieventListener;
	}

	// Token: 0x0400032B RID: 811
	public object parameter;

	// Token: 0x0400032C RID: 812
	public UIEventListener.VoidDelegate onSubmit;

	// Token: 0x0400032D RID: 813
	public UIEventListener.VoidDelegate onClick;

	// Token: 0x0400032E RID: 814
	public UIEventListener.VoidDelegate onDoubleClick;

	// Token: 0x0400032F RID: 815
	public UIEventListener.BoolDelegate onHover;

	// Token: 0x04000330 RID: 816
	public UIEventListener.BoolDelegate onPress;

	// Token: 0x04000331 RID: 817
	public UIEventListener.BoolDelegate onSelect;

	// Token: 0x04000332 RID: 818
	public UIEventListener.FloatDelegate onScroll;

	// Token: 0x04000333 RID: 819
	public UIEventListener.VoidDelegate onDragStart;

	// Token: 0x04000334 RID: 820
	public UIEventListener.VectorDelegate onDrag;

	// Token: 0x04000335 RID: 821
	public UIEventListener.VoidDelegate onDragOver;

	// Token: 0x04000336 RID: 822
	public UIEventListener.VoidDelegate onDragOut;

	// Token: 0x04000337 RID: 823
	public UIEventListener.VoidDelegate onDragEnd;

	// Token: 0x04000338 RID: 824
	public UIEventListener.ObjectDelegate onDrop;

	// Token: 0x04000339 RID: 825
	public UIEventListener.KeyCodeDelegate onKey;

	// Token: 0x0400033A RID: 826
	public UIEventListener.BoolDelegate onTooltip;

	// Token: 0x02000538 RID: 1336
	// (Invoke) Token: 0x060037A4 RID: 14244
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x02000539 RID: 1337
	// (Invoke) Token: 0x060037A8 RID: 14248
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x0200053A RID: 1338
	// (Invoke) Token: 0x060037AC RID: 14252
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x0200053B RID: 1339
	// (Invoke) Token: 0x060037B0 RID: 14256
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x0200053C RID: 1340
	// (Invoke) Token: 0x060037B4 RID: 14260
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x0200053D RID: 1341
	// (Invoke) Token: 0x060037B8 RID: 14264
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
