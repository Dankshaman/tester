using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000037 RID: 55
[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	// Token: 0x06000122 RID: 290 RVA: 0x000076C8 File Offset: 0x000058C8
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
		this.mCollider = base.gameObject.GetComponent<Collider>();
		this.mCollider2D = base.gameObject.GetComponent<Collider2D>();
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000076F8 File Offset: 0x000058F8
	protected virtual void OnDisable()
	{
		if (this.mDragging)
		{
			this.StopDragging(UICamera.hoveredObject);
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000770D File Offset: 0x0000590D
	protected virtual void Start()
	{
		this.mButton = base.GetComponent<UIButton>();
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00007728 File Offset: 0x00005928
	protected virtual void OnPress(bool isPressed)
	{
		if (!this.interactable || UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (isPressed)
		{
			if (!this.mPressed)
			{
				this.mTouch = UICamera.currentTouch;
				this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
				this.mPressed = true;
				return;
			}
		}
		else if (this.mPressed && this.mTouch == UICamera.currentTouch)
		{
			this.mPressed = false;
			this.mTouch = null;
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x000077A3 File Offset: 0x000059A3
	protected virtual void Update()
	{
		if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressed && !this.mDragging && this.mDragStartTime < RealTime.time)
		{
			this.StartDragging();
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x000077D4 File Offset: 0x000059D4
	protected virtual void OnDragStart()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
			{
				return;
			}
		}
		this.StartDragging();
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00007874 File Offset: 0x00005A74
	public virtual void StartDragging()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging)
		{
			if (this.cloneOnDrag)
			{
				this.mPressed = false;
				GameObject gameObject = base.transform.parent.gameObject.AddChild(base.gameObject);
				gameObject.transform.localPosition = base.transform.localPosition;
				gameObject.transform.localRotation = base.transform.localRotation;
				gameObject.transform.localScale = base.transform.localScale;
				UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
				if (component != null)
				{
					component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
				}
				if (this.mTouch != null && this.mTouch.pressed == base.gameObject)
				{
					this.mTouch.current = gameObject;
					this.mTouch.pressed = gameObject;
					this.mTouch.dragged = gameObject;
					this.mTouch.last = gameObject;
				}
				if (base.GetComponent<UIInventoryItem>())
				{
					UIInventoryItem component2 = base.GetComponent<UIInventoryItem>();
					UIInventoryItem component3 = gameObject.GetComponent<UIInventoryItem>();
					component3.objectState = component2.objectState;
					component3.RemoveItem = component2.RemoveItem;
					component3.IncreaseSort = component2.IncreaseSort;
					component3.InsertItem = component2.InsertItem;
					component3.ScrollInventory = component2.ScrollInventory;
					component3.MoveOverInventoryBackground = component2.MoveOverInventoryBackground;
					component3.searchFilter = component2.searchFilter;
					component3.sort = component2.sort;
					component3.sourceInventoryItem = component2;
					component2.sort.gameObject.SetActive(false);
					component3.sourceInfo.gameObject.transform.parent = component3.gameObject.transform;
				}
				UIDragDropItem component4 = gameObject.GetComponent<UIDragDropItem>();
				component4.mTouch = this.mTouch;
				component4.mPressed = true;
				component4.mDragging = true;
				component4.Start();
				component4.OnClone(base.gameObject);
				component4.OnDragDropStart();
				if (UICamera.currentTouch == null)
				{
					UICamera.currentTouch = this.mTouch;
				}
				this.mTouch = null;
				UICamera.Notify(base.gameObject, "OnPress", false);
				UICamera.Notify(base.gameObject, "OnHover", false);
				return;
			}
			this.mDragging = true;
			this.OnDragDropStart();
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnClone(GameObject original)
	{
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00007AB8 File Offset: 0x00005CB8
	protected virtual void OnDrag(Vector2 delta)
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging || !base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		if (this.mRoot != null)
		{
			this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
			return;
		}
		this.OnDragDropMove(delta);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00007B19 File Offset: 0x00005D19
	protected virtual void OnDragEnd()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.StopDragging(UICamera.hoveredObject);
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00007B45 File Offset: 0x00005D45
	public void StopDragging(GameObject go = null)
	{
		if (this.mDragging)
		{
			this.mDragging = false;
			this.OnDragDropRelease(go);
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00007B60 File Offset: 0x00005D60
	protected virtual void OnDragDropStart()
	{
		if (!UIDragDropItem.draggedItems.Contains(this))
		{
			UIDragDropItem.draggedItems.Add(this);
		}
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = false;
		}
		if (this.mButton != null)
		{
			this.mButton.isEnabled = false;
		}
		else if (this.mCollider != null)
		{
			this.mCollider.enabled = false;
		}
		else if (this.mCollider2D != null)
		{
			this.mCollider2D.enabled = false;
		}
		this.mParent = this.mTrans.parent;
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = this.mTrans.localPosition;
		localPosition.z = 0f;
		this.mTrans.localPosition = localPosition;
		TweenPosition component = base.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
		SpringPosition component2 = base.GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00007CE3 File Offset: 0x00005EE3
	protected virtual void OnDragDropMove(Vector2 delta)
	{
		this.mTrans.localPosition += this.mTrans.InverseTransformDirection(delta);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00007D0C File Offset: 0x00005F0C
	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!this.cloneOnDrag)
		{
			UIDragScrollView[] componentsInChildren = base.GetComponentsInChildren<UIDragScrollView>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].scrollView = null;
			}
			if (this.mButton != null)
			{
				this.mButton.isEnabled = true;
			}
			else if (this.mCollider != null)
			{
				this.mCollider.enabled = true;
			}
			else if (this.mCollider2D != null)
			{
				this.mCollider2D.enabled = true;
			}
			UIDragDropContainer uidragDropContainer = surface ? NGUITools.FindInParents<UIDragDropContainer>(surface) : null;
			if (uidragDropContainer != null)
			{
				this.mTrans.parent = ((uidragDropContainer.reparentTarget != null) ? uidragDropContainer.reparentTarget : uidragDropContainer.transform);
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.z = 0f;
				this.mTrans.localPosition = localPosition;
			}
			else
			{
				this.mTrans.parent = this.mParent;
			}
			this.mParent = this.mTrans.parent;
			this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
			this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
			if (this.mDragScrollView != null)
			{
				base.Invoke("EnableDragScrollView", 0.001f);
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			if (this.mTable != null)
			{
				this.mTable.repositionNow = true;
			}
			if (this.mGrid != null)
			{
				this.mGrid.repositionNow = true;
			}
		}
		else
		{
			if (base.GetComponent<UIInventoryItem>())
			{
				UIInventoryItem sourceInventoryItem = base.GetComponent<UIInventoryItem>().sourceInventoryItem;
				sourceInventoryItem.sourceInfo.gameObject.transform.parent = sourceInventoryItem.gameObject.transform;
			}
			NGUITools.Destroy(base.gameObject);
		}
		this.OnDragDropEnd();
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00007EEC File Offset: 0x000060EC
	protected virtual void OnDragDropEnd()
	{
		UIDragDropItem.draggedItems.Remove(this);
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00007EFA File Offset: 0x000060FA
	protected void EnableDragScrollView()
	{
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = true;
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00007F16 File Offset: 0x00006116
	protected void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			this.StopDragging(null);
		}
	}

	// Token: 0x040000E6 RID: 230
	public UIDragDropItem.Restriction restriction;

	// Token: 0x040000E7 RID: 231
	public bool cloneOnDrag;

	// Token: 0x040000E8 RID: 232
	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	// Token: 0x040000E9 RID: 233
	public bool interactable = true;

	// Token: 0x040000EA RID: 234
	[NonSerialized]
	protected Transform mTrans;

	// Token: 0x040000EB RID: 235
	[NonSerialized]
	protected Transform mParent;

	// Token: 0x040000EC RID: 236
	[NonSerialized]
	protected Collider mCollider;

	// Token: 0x040000ED RID: 237
	[NonSerialized]
	protected Collider2D mCollider2D;

	// Token: 0x040000EE RID: 238
	[NonSerialized]
	protected UIButton mButton;

	// Token: 0x040000EF RID: 239
	[NonSerialized]
	protected UIRoot mRoot;

	// Token: 0x040000F0 RID: 240
	[NonSerialized]
	protected UIGrid mGrid;

	// Token: 0x040000F1 RID: 241
	[NonSerialized]
	protected UITable mTable;

	// Token: 0x040000F2 RID: 242
	[NonSerialized]
	protected float mDragStartTime;

	// Token: 0x040000F3 RID: 243
	[NonSerialized]
	protected UIDragScrollView mDragScrollView;

	// Token: 0x040000F4 RID: 244
	[NonSerialized]
	protected bool mPressed;

	// Token: 0x040000F5 RID: 245
	[NonSerialized]
	protected bool mDragging;

	// Token: 0x040000F6 RID: 246
	[NonSerialized]
	protected UICamera.MouseOrTouch mTouch;

	// Token: 0x040000F7 RID: 247
	public static List<UIDragDropItem> draggedItems = new List<UIDragDropItem>();

	// Token: 0x02000505 RID: 1285
	public enum Restriction
	{
		// Token: 0x0400239A RID: 9114
		None,
		// Token: 0x0400239B RID: 9115
		Horizontal,
		// Token: 0x0400239C RID: 9116
		Vertical,
		// Token: 0x0400239D RID: 9117
		PressAndHold
	}
}
