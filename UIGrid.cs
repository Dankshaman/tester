using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	// Token: 0x1700001E RID: 30
	// (set) Token: 0x06000190 RID: 400 RVA: 0x0000A326 File Offset: 0x00008526
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

	// Token: 0x06000191 RID: 401 RVA: 0x0000A33C File Offset: 0x0000853C
	public List<Transform> GetChildList()
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && child.gameObject.activeSelf))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
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

	// Token: 0x06000192 RID: 402 RVA: 0x0000A41C File Offset: 0x0000861C
	public Transform GetChild(int index)
	{
		List<Transform> childList = this.GetChildList();
		if (index >= childList.Count)
		{
			return null;
		}
		return childList[index];
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000A442 File Offset: 0x00008642
	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000A450 File Offset: 0x00008650
	[Obsolete("Use gameObject.AddChild or transform.parent = gridTransform")]
	public void AddChild(Transform trans)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000A450 File Offset: 0x00008650
	[Obsolete("Use gameObject.AddChild or transform.parent = gridTransform")]
	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000A474 File Offset: 0x00008674
	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000A49B File Offset: 0x0000869B
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000A4B8 File Offset: 0x000086B8
	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000A4F5 File Offset: 0x000086F5
	protected virtual void Update()
	{
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000A504 File Offset: 0x00008704
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000A51B File Offset: 0x0000871B
	public static int SortByName(Transform a, Transform b)
	{
		if (a.name == b.name)
		{
			return UIGrid.SortVertical(a, b);
		}
		return string.Compare(a.name, b.name);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000A54C File Offset: 0x0000874C
	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000A578 File Offset: 0x00008778
	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void Sort(List<Transform> list)
	{
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000A5A4 File Offset: 0x000087A4
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(base.gameObject))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		List<Transform> childList = this.GetChildList();
		this.ResetPosition(childList);
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000A628 File Offset: 0x00008828
	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000A674 File Offset: 0x00008874
	protected virtual void ResetPosition(List<Transform> list)
	{
		this.mReposition = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.transform;
		int i = 0;
		int count = list.Count;
		while (i < count)
		{
			Transform transform2 = list[i];
			Vector3 vector = transform2.localPosition;
			float z = vector.z;
			if (this.arrangement == UIGrid.Arrangement.CellSnap)
			{
				if (this.cellWidth > 0f)
				{
					vector.x = Mathf.Round(vector.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector.y = Mathf.Round(vector.y / this.cellHeight) * this.cellHeight;
				}
			}
			else
			{
				vector = ((this.arrangement == UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z) : new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z));
			}
			if (this.animateSmoothly && Application.isPlaying && (this.pivot != UIWidget.Pivot.TopLeft || Vector3.SqrMagnitude(transform2.localPosition - vector) >= 0.0001f))
			{
				SpringPosition springPosition = SpringPosition.Begin(transform2.gameObject, vector, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			else
			{
				transform2.localPosition = vector;
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (++num >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
			i++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int j = 0; j < transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
					SpringPosition springPosition2 = component;
					springPosition2.target.x = springPosition2.target.x - num5;
					SpringPosition springPosition3 = component;
					springPosition3.target.y = springPosition3.target.y - num6;
					component.enabled = true;
				}
				else
				{
					Vector3 localPosition = child.localPosition;
					localPosition.x -= num5;
					localPosition.y -= num6;
					child.localPosition = localPosition;
				}
			}
		}
	}

	// Token: 0x04000162 RID: 354
	public UIGrid.Arrangement arrangement;

	// Token: 0x04000163 RID: 355
	public UIGrid.Sorting sorting;

	// Token: 0x04000164 RID: 356
	public UIWidget.Pivot pivot;

	// Token: 0x04000165 RID: 357
	public int maxPerLine;

	// Token: 0x04000166 RID: 358
	public float cellWidth = 200f;

	// Token: 0x04000167 RID: 359
	public float cellHeight = 200f;

	// Token: 0x04000168 RID: 360
	public bool animateSmoothly;

	// Token: 0x04000169 RID: 361
	public bool hideInactive;

	// Token: 0x0400016A RID: 362
	public bool keepWithinPanel;

	// Token: 0x0400016B RID: 363
	public UIGrid.OnReposition onReposition;

	// Token: 0x0400016C RID: 364
	public Comparison<Transform> onCustomSort;

	// Token: 0x0400016D RID: 365
	[HideInInspector]
	[SerializeField]
	private bool sorted;

	// Token: 0x0400016E RID: 366
	protected bool mReposition;

	// Token: 0x0400016F RID: 367
	protected UIPanel mPanel;

	// Token: 0x04000170 RID: 368
	protected bool mInitDone;

	// Token: 0x02000509 RID: 1289
	// (Invoke) Token: 0x0600374B RID: 14155
	public delegate void OnReposition();

	// Token: 0x0200050A RID: 1290
	public enum Arrangement
	{
		// Token: 0x040023A9 RID: 9129
		Horizontal,
		// Token: 0x040023AA RID: 9130
		Vertical,
		// Token: 0x040023AB RID: 9131
		CellSnap
	}

	// Token: 0x0200050B RID: 1291
	public enum Sorting
	{
		// Token: 0x040023AD RID: 9133
		None,
		// Token: 0x040023AE RID: 9134
		Alphabetic,
		// Token: 0x040023AF RID: 9135
		Horizontal,
		// Token: 0x040023B0 RID: 9136
		Vertical,
		// Token: 0x040023B1 RID: 9137
		Custom
	}
}
