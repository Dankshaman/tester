using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000050 RID: 80
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	// Token: 0x17000047 RID: 71
	// (set) Token: 0x0600028F RID: 655 RVA: 0x00010C1D File Offset: 0x0000EE1D
	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00010C30 File Offset: 0x0000EE30
	public List<Transform> GetChildList()
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.gameObject)))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UITable.Sorting.None)
		{
			if (this.sorting == UITable.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UITable.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UITable.Sorting.Vertical)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
			}
			else if (this.onCustomSort != null)
			{
				list.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(list);
			}
		}
		return list;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00010D04 File Offset: 0x0000EF04
	protected virtual void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(UIGrid.SortByName));
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00010D18 File Offset: 0x0000EF18
	protected virtual void Start()
	{
		this.Init();
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00010D2D File Offset: 0x0000EF2D
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00010D47 File Offset: 0x0000EF47
	protected virtual void LateUpdate()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00010D5E File Offset: 0x0000EF5E
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00010D78 File Offset: 0x0000EF78
	protected void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		object obj = (this.columns > 0) ? (children.Count / this.columns + 1) : 1;
		int num3 = (this.columns > 0) ? this.columns : children.Count;
		object obj2 = obj;
		Bounds[,] array = new Bounds[obj2, num3];
		Bounds[] array2 = new Bounds[num3];
		Bounds[] array3 = new Bounds[obj2];
		int num4 = 0;
		int num5 = 0;
		int i = 0;
		int count = children.Count;
		while (i < count)
		{
			Transform transform = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, transform, !this.hideInactive, !this.ignoreChildren);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num5, num4] = bounds;
			array2[num4].Encapsulate(bounds);
			array3[num5].Encapsulate(bounds);
			if (++num4 >= this.columns && this.columns > 0)
			{
				num4 = 0;
				num5++;
			}
			i++;
		}
		num4 = 0;
		num5 = 0;
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.cellAlignment);
		int j = 0;
		int count2 = children.Count;
		while (j < count2)
		{
			Transform transform2 = children[j];
			Bounds bounds2 = array[num5, num4];
			Bounds bounds3 = array2[num4];
			Bounds bounds4 = array3[num5];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x -= Mathf.Lerp(0f, bounds2.max.x - bounds2.min.x - bounds3.max.x + bounds3.min.x, pivotOffset.x) - this.padding.x;
			if (this.direction == UITable.Direction.Down)
			{
				localPosition.y = -num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += Mathf.Lerp(bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, 0f, pivotOffset.y) - this.padding.y;
			}
			else
			{
				localPosition.y = num2 + bounds2.extents.y - bounds2.center.y;
				localPosition.y -= Mathf.Lerp(0f, bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, pivotOffset.y) - this.padding.y;
			}
			num += bounds3.size.x + this.padding.x * 2f;
			transform2.localPosition = localPosition;
			if (++num4 >= this.columns && this.columns > 0)
			{
				num4 = 0;
				num5++;
				num = 0f;
				num2 += bounds4.size.y + this.padding.y * 2f;
			}
			j++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			Bounds bounds5 = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			float num6 = Mathf.Lerp(0f, bounds5.size.x, pivotOffset.x);
			float num7 = Mathf.Lerp(-bounds5.size.y, 0f, pivotOffset.y);
			Transform transform3 = base.transform;
			for (int k = 0; k < transform3.childCount; k++)
			{
				Transform child = transform3.GetChild(k);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
					SpringPosition springPosition = component;
					springPosition.target.x = springPosition.target.x - num6;
					SpringPosition springPosition2 = component;
					springPosition2.target.y = springPosition2.target.y - num7;
					component.enabled = true;
				}
				else
				{
					Vector3 localPosition2 = child.localPosition;
					localPosition2.x -= num6;
					localPosition2.y -= num7;
					child.localPosition = localPosition2;
				}
			}
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00011204 File Offset: 0x0000F404
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transform = base.transform;
		List<Transform> childList = this.GetChildList();
		if (childList.Count > 0)
		{
			this.RepositionVariableSize(childList);
		}
		if (this.keepWithinPanel && this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x0400022F RID: 559
	public int columns;

	// Token: 0x04000230 RID: 560
	public UITable.Direction direction;

	// Token: 0x04000231 RID: 561
	public UITable.Sorting sorting;

	// Token: 0x04000232 RID: 562
	public UIWidget.Pivot pivot;

	// Token: 0x04000233 RID: 563
	public UIWidget.Pivot cellAlignment;

	// Token: 0x04000234 RID: 564
	public bool hideInactive = true;

	// Token: 0x04000235 RID: 565
	public bool keepWithinPanel;

	// Token: 0x04000236 RID: 566
	public Vector2 padding = Vector2.zero;

	// Token: 0x04000237 RID: 567
	public bool ignoreChildren;

	// Token: 0x04000238 RID: 568
	public UITable.OnReposition onReposition;

	// Token: 0x04000239 RID: 569
	public Comparison<Transform> onCustomSort;

	// Token: 0x0400023A RID: 570
	protected UIPanel mPanel;

	// Token: 0x0400023B RID: 571
	protected bool mInitDone;

	// Token: 0x0400023C RID: 572
	protected bool mReposition;

	// Token: 0x0200051E RID: 1310
	// (Invoke) Token: 0x0600376B RID: 14187
	public delegate void OnReposition();

	// Token: 0x0200051F RID: 1311
	public enum Direction
	{
		// Token: 0x040023F5 RID: 9205
		Down,
		// Token: 0x040023F6 RID: 9206
		Up
	}

	// Token: 0x02000520 RID: 1312
	public enum Sorting
	{
		// Token: 0x040023F8 RID: 9208
		None,
		// Token: 0x040023F9 RID: 9209
		Alphabetic,
		// Token: 0x040023FA RID: 9210
		Horizontal,
		// Token: 0x040023FB RID: 9211
		Vertical,
		// Token: 0x040023FC RID: 9212
		Custom
	}
}
