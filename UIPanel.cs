using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008C RID: 140
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Panel")]
public class UIPanel : UIRect
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x000328EF File Offset: 0x00030AEF
	// (set) Token: 0x06000732 RID: 1842 RVA: 0x000328F7 File Offset: 0x00030AF7
	public string sortingLayerName
	{
		get
		{
			return this.mSortingLayerName;
		}
		set
		{
			if (this.mSortingLayerName != value)
			{
				this.mSortingLayerName = value;
				this.UpdateDrawCalls(UIPanel.list.IndexOf(this));
			}
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000733 RID: 1843 RVA: 0x00032920 File Offset: 0x00030B20
	public static int nextUnusedDepth
	{
		get
		{
			int num = int.MinValue;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				num = Mathf.Max(num, UIPanel.list[i].depth);
				i++;
			}
			if (num != -2147483648)
			{
				return num + 1;
			}
			return 0;
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x0003296E File Offset: 0x00030B6E
	public override bool canBeAnchored
	{
		get
		{
			return this.mClipping > UIDrawCall.Clipping.None;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000735 RID: 1845 RVA: 0x00032979 File Offset: 0x00030B79
	// (set) Token: 0x06000736 RID: 1846 RVA: 0x00032984 File Offset: 0x00030B84
	public override float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				bool flag = this.mAlpha > 0.001f;
				this.mAlphaFrameID = -1;
				this.mResized = true;
				this.mAlpha = num;
				int i = 0;
				int count = this.drawCalls.Count;
				while (i < count)
				{
					this.drawCalls[i].isDirty = true;
					i++;
				}
				this.Invalidate(!flag && this.mAlpha > 0.001f);
			}
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x00032A07 File Offset: 0x00030C07
	// (set) Token: 0x06000738 RID: 1848 RVA: 0x00032A0F File Offset: 0x00030C0F
	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				this.mDepth = value;
				UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
			}
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000739 RID: 1849 RVA: 0x00032A37 File Offset: 0x00030C37
	// (set) Token: 0x0600073A RID: 1850 RVA: 0x00032A3F File Offset: 0x00030C3F
	public int sortingOrder
	{
		get
		{
			return this.mSortingOrder;
		}
		set
		{
			if (this.mSortingOrder != value)
			{
				this.mSortingOrder = value;
				this.UpdateDrawCalls(UIPanel.list.IndexOf(this));
			}
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00032A64 File Offset: 0x00030C64
	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (!(a != b) || !(a != null) || !(b != null))
		{
			return 0;
		}
		if (a.mDepth < b.mDepth)
		{
			return -1;
		}
		if (a.mDepth > b.mDepth)
		{
			return 1;
		}
		if (a.GetInstanceID() >= b.GetInstanceID())
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00032ABF File Offset: 0x00030CBF
	public float width
	{
		get
		{
			return this.GetViewSize().x;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x00032ACC File Offset: 0x00030CCC
	public float height
	{
		get
		{
			return this.GetViewSize().y;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
	public bool halfPixelOffset
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x0600073F RID: 1855 RVA: 0x00032AD9 File Offset: 0x00030CD9
	public bool usedForUI
	{
		get
		{
			return base.anchorCamera != null && this.mCam.orthographic;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x00032AF8 File Offset: 0x00030CF8
	public Vector3 drawCallOffset
	{
		get
		{
			if (base.anchorCamera != null && this.mCam.orthographic)
			{
				Vector2 windowSize = this.GetWindowSize();
				float num = ((base.root != null) ? base.root.pixelSizeAdjustment : 1f) / windowSize.y / this.mCam.orthographicSize;
				bool flag = false;
				bool flag2 = false;
				if ((Mathf.RoundToInt(windowSize.x) & 1) == 1)
				{
					flag = !flag;
				}
				if ((Mathf.RoundToInt(windowSize.y) & 1) == 1)
				{
					flag2 = !flag2;
				}
				return new Vector3(flag ? (-num) : 0f, flag2 ? num : 0f);
			}
			return Vector3.zero;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x00032BB1 File Offset: 0x00030DB1
	// (set) Token: 0x06000742 RID: 1858 RVA: 0x00032BB9 File Offset: 0x00030DB9
	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mResized = true;
				this.mClipping = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x00032BD9 File Offset: 0x00030DD9
	public UIPanel parentPanel
	{
		get
		{
			return this.mParentPanel;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x00032BE4 File Offset: 0x00030DE4
	public int clipCount
	{
		get
		{
			int num = 0;
			UIPanel uipanel = this;
			while (uipanel != null)
			{
				if (uipanel.mClipping == UIDrawCall.Clipping.SoftClip || uipanel.mClipping == UIDrawCall.Clipping.TextureMask)
				{
					num++;
				}
				uipanel = uipanel.mParentPanel;
			}
			return num;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x00032C1E File Offset: 0x00030E1E
	public bool hasClipping
	{
		get
		{
			return this.mClipping == UIDrawCall.Clipping.SoftClip || this.mClipping == UIDrawCall.Clipping.TextureMask;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000746 RID: 1862 RVA: 0x00032C34 File Offset: 0x00030E34
	public bool hasCumulativeClipping
	{
		get
		{
			return this.clipCount != 0;
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x00032C3F File Offset: 0x00030E3F
	[Obsolete("Use 'hasClipping' or 'hasCumulativeClipping' instead")]
	public bool clipsChildren
	{
		get
		{
			return this.hasCumulativeClipping;
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000748 RID: 1864 RVA: 0x00032C47 File Offset: 0x00030E47
	// (set) Token: 0x06000749 RID: 1865 RVA: 0x00032C50 File Offset: 0x00030E50
	public Vector2 clipOffset
	{
		get
		{
			return this.mClipOffset;
		}
		set
		{
			if (Mathf.Abs(this.mClipOffset.x - value.x) > 0.001f || Mathf.Abs(this.mClipOffset.y - value.y) > 0.001f)
			{
				this.mClipOffset = value;
				this.InvalidateClipping();
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00032CBC File Offset: 0x00030EBC
	private void InvalidateClipping()
	{
		this.mResized = true;
		this.mMatrixFrame = -1;
		int i = 0;
		int count = UIPanel.list.Count;
		while (i < count)
		{
			UIPanel uipanel = UIPanel.list[i];
			if (uipanel != this && uipanel.parentPanel == this)
			{
				uipanel.InvalidateClipping();
			}
			i++;
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600074B RID: 1867 RVA: 0x00032D17 File Offset: 0x00030F17
	// (set) Token: 0x0600074C RID: 1868 RVA: 0x00032D1F File Offset: 0x00030F1F
	public Texture2D clipTexture
	{
		get
		{
			return this.mClipTexture;
		}
		set
		{
			if (this.mClipTexture != value)
			{
				this.mClipTexture = value;
			}
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600074D RID: 1869 RVA: 0x00032D36 File Offset: 0x00030F36
	// (set) Token: 0x0600074E RID: 1870 RVA: 0x00032D3E File Offset: 0x00030F3E
	[Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
	public Vector4 clipRange
	{
		get
		{
			return this.baseClipRegion;
		}
		set
		{
			this.baseClipRegion = value;
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x0600074F RID: 1871 RVA: 0x00032D47 File Offset: 0x00030F47
	// (set) Token: 0x06000750 RID: 1872 RVA: 0x00032D50 File Offset: 0x00030F50
	public Vector4 baseClipRegion
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (Mathf.Abs(this.mClipRange.x - value.x) > 0.001f || Mathf.Abs(this.mClipRange.y - value.y) > 0.001f || Mathf.Abs(this.mClipRange.z - value.z) > 0.001f || Mathf.Abs(this.mClipRange.w - value.w) > 0.001f)
			{
				this.mResized = true;
				this.mClipRange = value;
				this.mMatrixFrame = -1;
				UIScrollView component = base.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.UpdatePosition();
				}
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000751 RID: 1873 RVA: 0x00032E14 File Offset: 0x00031014
	public Vector4 finalClipRegion
	{
		get
		{
			Vector2 viewSize = this.GetViewSize();
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
			}
			return new Vector4(0f, 0f, viewSize.x, viewSize.y);
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000752 RID: 1874 RVA: 0x00032E8B File Offset: 0x0003108B
	// (set) Token: 0x06000753 RID: 1875 RVA: 0x00032E93 File Offset: 0x00031093
	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
			}
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000754 RID: 1876 RVA: 0x00032EAC File Offset: 0x000310AC
	public override Vector3[] localCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector3[] worldCorners = this.worldCorners;
				Transform cachedTransform = base.cachedTransform;
				for (int i = 0; i < 4; i++)
				{
					worldCorners[i] = cachedTransform.InverseTransformPoint(worldCorners[i]);
				}
				return worldCorners;
			}
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float x = num + this.mClipRange.z;
			float y = num2 + this.mClipRange.w;
			UIPanel.mCorners[0] = new Vector3(num, num2);
			UIPanel.mCorners[1] = new Vector3(num, y);
			UIPanel.mCorners[2] = new Vector3(x, y);
			UIPanel.mCorners[3] = new Vector3(x, num2);
			return UIPanel.mCorners;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x00032FB8 File Offset: 0x000311B8
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float x = num + this.mClipRange.z;
				float y = num2 + this.mClipRange.w;
				Transform cachedTransform = base.cachedTransform;
				UIPanel.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
				UIPanel.mCorners[1] = cachedTransform.TransformPoint(num, y, 0f);
				UIPanel.mCorners[2] = cachedTransform.TransformPoint(x, y, 0f);
				UIPanel.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
			}
			else
			{
				if (base.anchorCamera != null)
				{
					return this.mCam.GetWorldCorners(base.cameraRayDistance);
				}
				Vector2 viewSize = this.GetViewSize();
				float num3 = -0.5f * viewSize.x;
				float num4 = -0.5f * viewSize.y;
				float x2 = num3 + viewSize.x;
				float y2 = num4 + viewSize.y;
				UIPanel.mCorners[0] = new Vector3(num3, num4);
				UIPanel.mCorners[1] = new Vector3(num3, y2);
				UIPanel.mCorners[2] = new Vector3(x2, y2);
				UIPanel.mCorners[3] = new Vector3(x2, num4);
				if (this.anchorOffset && (this.mCam == null || this.mCam.transform.parent != base.cachedTransform))
				{
					Vector3 position = base.cachedTransform.position;
					for (int i = 0; i < 4; i++)
					{
						UIPanel.mCorners[i] += position;
					}
				}
			}
			return UIPanel.mCorners;
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x000331D4 File Offset: 0x000313D4
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float num3 = num + this.mClipRange.z;
			float num4 = num2 + this.mClipRange.w;
			float x = (num + num3) * 0.5f;
			float y = (num2 + num4) * 0.5f;
			Transform cachedTransform = base.cachedTransform;
			UIRect.mSides[0] = cachedTransform.TransformPoint(num, y, 0f);
			UIRect.mSides[1] = cachedTransform.TransformPoint(x, num4, 0f);
			UIRect.mSides[2] = cachedTransform.TransformPoint(num3, y, 0f);
			UIRect.mSides[3] = cachedTransform.TransformPoint(x, num2, 0f);
			if (relativeTo != null)
			{
				for (int i = 0; i < 4; i++)
				{
					UIRect.mSides[i] = relativeTo.InverseTransformPoint(UIRect.mSides[i]);
				}
			}
			return UIRect.mSides;
		}
		if (base.anchorCamera != null && this.anchorOffset)
		{
			Vector3[] sides = this.mCam.GetSides(base.cameraRayDistance);
			Vector3 position = base.cachedTransform.position;
			for (int j = 0; j < 4; j++)
			{
				sides[j] += position;
			}
			if (relativeTo != null)
			{
				for (int k = 0; k < 4; k++)
				{
					sides[k] = relativeTo.InverseTransformPoint(sides[k]);
				}
			}
			return sides;
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x000333BE File Offset: 0x000315BE
	public override void Invalidate(bool includeChildren)
	{
		this.mAlphaFrameID = -1;
		base.Invalidate(includeChildren);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x000333D0 File Offset: 0x000315D0
	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			UIRect parent = base.parent;
			this.finalAlpha = ((base.parent != null) ? (parent.CalculateFinalAlpha(frameID) * this.mAlpha) : this.mAlpha);
		}
		return this.finalAlpha;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00033424 File Offset: 0x00031624
	public override void SetRect(float x, float y, float width, float height)
	{
		int num = Mathf.FloorToInt(width + 0.5f);
		int num2 = Mathf.FloorToInt(height + 0.5f);
		num = num >> 1 << 1;
		num2 = num2 >> 1 << 1;
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(x + 0.5f);
		localPosition.y = Mathf.Floor(y + 0.5f);
		if (num < 2)
		{
			num = 2;
		}
		if (num2 < 2)
		{
			num2 = 2;
		}
		this.baseClipRegion = new Vector4(localPosition.x, localPosition.y, (float)num, (float)num2);
		if (base.isAnchored)
		{
			transform = transform.parent;
			if (this.leftAnchor.target)
			{
				this.leftAnchor.SetHorizontal(transform, x);
			}
			if (this.rightAnchor.target)
			{
				this.rightAnchor.SetHorizontal(transform, x + width);
			}
			if (this.bottomAnchor.target)
			{
				this.bottomAnchor.SetVertical(transform, y);
			}
			if (this.topAnchor.target)
			{
				this.topAnchor.SetVertical(transform, y + height);
			}
		}
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00033548 File Offset: 0x00031748
	public bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		return num2 >= this.mMin.x && num4 >= this.mMin.y && num <= this.mMax.x && num3 <= this.mMax.y;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0003366C File Offset: 0x0003186C
	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
		return vector.x >= this.mMin.x && vector.y >= this.mMin.y && vector.x <= this.mMax.x && vector.y <= this.mMax.y;
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00033704 File Offset: 0x00031904
	public bool IsVisible(UIWidget w)
	{
		UIPanel uipanel = this;
		Vector3[] array = null;
		while (uipanel != null)
		{
			if ((uipanel.mClipping == UIDrawCall.Clipping.None || uipanel.mClipping == UIDrawCall.Clipping.ConstrainButDontClip) && !w.hideIfOffScreen)
			{
				uipanel = uipanel.mParentPanel;
			}
			else
			{
				if (array == null)
				{
					array = w.worldCorners;
				}
				if (!uipanel.IsVisible(array[0], array[1], array[2], array[3]))
				{
					return false;
				}
				uipanel = uipanel.mParentPanel;
			}
		}
		return true;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0003377C File Offset: 0x0003197C
	public bool Affects(UIWidget w)
	{
		if (w == null)
		{
			return false;
		}
		UIPanel panel = w.panel;
		if (panel == null)
		{
			return false;
		}
		UIPanel uipanel = this;
		while (uipanel != null)
		{
			if (uipanel == panel)
			{
				return true;
			}
			if (!uipanel.hasCumulativeClipping)
			{
				return false;
			}
			uipanel = uipanel.mParentPanel;
		}
		return false;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x000337D0 File Offset: 0x000319D0
	[ContextMenu("Force Refresh")]
	public void RebuildAllDrawCalls()
	{
		this.mRebuild = true;
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x000337DC File Offset: 0x000319DC
	public void SetDirty()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			this.drawCalls[i].isDirty = true;
			i++;
		}
		this.Invalidate(true);
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x0003381A File Offset: 0x00031A1A
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x00033824 File Offset: 0x00031A24
	private void FindParent()
	{
		Transform parent = base.cachedTransform.parent;
		this.mParentPanel = ((parent != null) ? NGUITools.FindInParents<UIPanel>(parent.gameObject) : null);
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0003385A File Offset: 0x00031A5A
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		this.FindParent();
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00033868 File Offset: 0x00031A68
	protected override void OnStart()
	{
		this.mLayer = base.cachedGameObject.layer;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0003387B File Offset: 0x00031A7B
	protected override void OnEnable()
	{
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		this.OnStart();
		base.OnEnable();
		this.mMatrixFrame = -1;
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x000338A8 File Offset: 0x00031AA8
	protected override void OnInit()
	{
		if (UIPanel.list.Contains(this))
		{
			return;
		}
		base.OnInit();
		this.FindParent();
		if (base.GetComponentInParent<Rigidbody>() == null && this.mParentPanel == null)
		{
			UICamera uicamera = (base.anchorCamera != null) ? this.mCam.GetComponent<UICamera>() : null;
			if (uicamera != null && (uicamera.eventType == UICamera.EventType.UI_3D || uicamera.eventType == UICamera.EventType.World_3D))
			{
				Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		UIPanel.list.Add(this);
		UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00033970 File Offset: 0x00031B70
	protected override void OnDisable()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			if (uidrawCall != null)
			{
				UIDrawCall.Destroy(uidrawCall);
			}
			i++;
		}
		this.drawCalls.Clear();
		UIPanel.list.Remove(this);
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		if (UIPanel.list.Count == 0)
		{
			UIDrawCall.ReleaseAll();
			UIPanel.mUpdateFrame = -1;
		}
		base.OnDisable();
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x000339F4 File Offset: 0x00031BF4
	private void UpdateTransformMatrix()
	{
		int frameCount = Time.frameCount;
		if (this.mHasMoved || this.mMatrixFrame != frameCount)
		{
			this.mMatrixFrame = frameCount;
			this.worldToLocal = base.cachedTransform.worldToLocalMatrix;
			Vector2 vector = this.GetViewSize() * 0.5f;
			float num = this.mClipOffset.x + this.mClipRange.x;
			float num2 = this.mClipOffset.y + this.mClipRange.y;
			this.mMin.x = num - vector.x;
			this.mMin.y = num2 - vector.y;
			this.mMax.x = num + vector.x;
			this.mMax.y = num2 + vector.y;
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00033AC0 File Offset: 0x00031CC0
	protected override void OnAnchor()
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector2 viewSize = this.GetViewSize();
		Vector2 vector = cachedTransform.localPosition;
		float num;
		float num2;
		float num3;
		float num4;
		if (this.leftAnchor.target == this.bottomAnchor.target && this.leftAnchor.target == this.rightAnchor.target && this.leftAnchor.target == this.topAnchor.target)
		{
			Vector3[] sides = this.leftAnchor.GetSides(parent);
			if (sides != null)
			{
				num = NGUIMath.Lerp(sides[0].x, sides[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				num2 = NGUIMath.Lerp(sides[0].x, sides[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				num3 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
			}
			else
			{
				Vector2 vector2 = base.GetLocalPos(this.leftAnchor, parent);
				num = vector2.x + (float)this.leftAnchor.absolute;
				num3 = vector2.y + (float)this.bottomAnchor.absolute;
				num2 = vector2.x + (float)this.rightAnchor.absolute;
				num4 = vector2.y + (float)this.topAnchor.absolute;
			}
		}
		else
		{
			if (this.leftAnchor.target)
			{
				Vector3[] sides2 = this.leftAnchor.GetSides(parent);
				if (sides2 != null)
				{
					num = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				}
				else
				{
					num = base.GetLocalPos(this.leftAnchor, parent).x + (float)this.leftAnchor.absolute;
				}
			}
			else
			{
				num = this.mClipRange.x - 0.5f * viewSize.x;
			}
			if (this.rightAnchor.target)
			{
				Vector3[] sides3 = this.rightAnchor.GetSides(parent);
				if (sides3 != null)
				{
					num2 = NGUIMath.Lerp(sides3[0].x, sides3[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
				else
				{
					num2 = base.GetLocalPos(this.rightAnchor, parent).x + (float)this.rightAnchor.absolute;
				}
			}
			else
			{
				num2 = this.mClipRange.x + 0.5f * viewSize.x;
			}
			if (this.bottomAnchor.target)
			{
				Vector3[] sides4 = this.bottomAnchor.GetSides(parent);
				if (sides4 != null)
				{
					num3 = NGUIMath.Lerp(sides4[3].y, sides4[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
				else
				{
					num3 = base.GetLocalPos(this.bottomAnchor, parent).y + (float)this.bottomAnchor.absolute;
				}
			}
			else
			{
				num3 = this.mClipRange.y - 0.5f * viewSize.y;
			}
			if (this.topAnchor.target)
			{
				Vector3[] sides5 = this.topAnchor.GetSides(parent);
				if (sides5 != null)
				{
					num4 = NGUIMath.Lerp(sides5[3].y, sides5[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				}
				else
				{
					num4 = base.GetLocalPos(this.topAnchor, parent).y + (float)this.topAnchor.absolute;
				}
			}
			else
			{
				num4 = this.mClipRange.y + 0.5f * viewSize.y;
			}
		}
		num -= vector.x + this.mClipOffset.x;
		num2 -= vector.x + this.mClipOffset.x;
		num3 -= vector.y + this.mClipOffset.y;
		num4 -= vector.y + this.mClipOffset.y;
		float x = Mathf.Lerp(num, num2, 0.5f);
		float y = Mathf.Lerp(num3, num4, 0.5f);
		float num5 = num2 - num;
		float num6 = num4 - num3;
		float num7 = Mathf.Max(2f, this.mClipSoftness.x);
		float num8 = Mathf.Max(2f, this.mClipSoftness.y);
		if (num5 < num7)
		{
			num5 = num7;
		}
		if (num6 < num8)
		{
			num6 = num8;
		}
		this.baseClipRegion = new Vector4(x, y, num5, num6);
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00034004 File Offset: 0x00032204
	public int ForceUpdate(int rq = 3000)
	{
		this.UpdateSelf();
		int sortOrder = UIPanel.list.IndexOf(this);
		if (this.renderQueue == UIPanel.RenderQueue.Automatic)
		{
			this.startingRenderQueue = rq;
			this.UpdateDrawCalls(sortOrder);
			rq += this.drawCalls.Count;
		}
		else if (this.renderQueue == UIPanel.RenderQueue.StartAt)
		{
			this.UpdateDrawCalls(sortOrder);
			if (this.drawCalls.Count != 0)
			{
				rq = Mathf.Max(rq, this.startingRenderQueue + this.drawCalls.Count);
			}
		}
		else
		{
			this.UpdateDrawCalls(sortOrder);
			if (this.drawCalls.Count != 0)
			{
				rq = Mathf.Max(rq, this.startingRenderQueue + 1);
			}
		}
		return rq;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000340AA File Offset: 0x000322AA
	public void ForceUpdateAll()
	{
		UIPanel.mUpdateFrame = -1;
		this.LateUpdate();
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000340B8 File Offset: 0x000322B8
	public static void ForceUpdate(List<UIPanel> panels)
	{
		int rq = 3000;
		for (int i = 0; i < panels.Count; i++)
		{
			rq = panels[i].ForceUpdate(rq);
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000340EC File Offset: 0x000322EC
	private void LateUpdate()
	{
		if (UIPanel.mUpdateFrame != Time.frameCount)
		{
			UIPanel.mUpdateFrame = Time.frameCount;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				UIPanel.list[i].UpdateSelf();
				i++;
			}
			int num = 3000;
			int j = 0;
			int count2 = UIPanel.list.Count;
			while (j < count2)
			{
				UIPanel uipanel = UIPanel.list[j];
				if (uipanel.renderQueue == UIPanel.RenderQueue.Automatic)
				{
					uipanel.startingRenderQueue = num;
					uipanel.UpdateDrawCalls(j);
					num += uipanel.drawCalls.Count;
				}
				else if (uipanel.renderQueue == UIPanel.RenderQueue.StartAt)
				{
					uipanel.UpdateDrawCalls(j);
					if (uipanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uipanel.startingRenderQueue + uipanel.drawCalls.Count);
					}
				}
				else
				{
					uipanel.UpdateDrawCalls(j);
					if (uipanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uipanel.startingRenderQueue + 1);
					}
				}
				j++;
			}
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000341F8 File Offset: 0x000323F8
	private void UpdateSelf()
	{
		this.mHasMoved = base.cachedTransform.hasChanged;
		this.UpdateTransformMatrix();
		this.UpdateLayers();
		this.UpdateWidgets();
		if (this.mRebuild)
		{
			this.mRebuild = false;
			this.FillAllDrawCalls();
		}
		else
		{
			int i = 0;
			while (i < this.drawCalls.Count)
			{
				UIDrawCall uidrawCall = this.drawCalls[i];
				if (uidrawCall.isDirty && !this.FillDrawCall(uidrawCall))
				{
					UIDrawCall.Destroy(uidrawCall);
					this.drawCalls.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		if (this.mUpdateScroll)
		{
			this.mUpdateScroll = false;
			UIScrollView component = base.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars();
			}
		}
		if (this.mHasMoved)
		{
			this.mHasMoved = false;
			this.mTrans.hasChanged = false;
		}
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x000342C7 File Offset: 0x000324C7
	public void SortWidgets()
	{
		this.mSortWidgets = false;
		this.widgets.Sort(new Comparison<UIWidget>(UIWidget.PanelCompareFunc));
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x000342E8 File Offset: 0x000324E8
	private void FillAllDrawCalls()
	{
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall.Destroy(this.drawCalls[i]);
		}
		this.drawCalls.Clear();
		Material material = null;
		Texture texture = null;
		Shader shader = null;
		UIDrawCall uidrawCall = null;
		int num = 0;
		if (this.mSortWidgets)
		{
			this.SortWidgets();
		}
		for (int j = 0; j < this.widgets.Count; j++)
		{
			UIWidget uiwidget = this.widgets[j];
			if (uiwidget.isVisible && uiwidget.hasVertices)
			{
				Material material2 = uiwidget.material;
				if (this.onCreateMaterial != null)
				{
					material2 = this.onCreateMaterial(uiwidget, material2);
				}
				Texture mainTexture = uiwidget.mainTexture;
				Shader shader2 = uiwidget.shader;
				if (material != material2 || texture != mainTexture || shader != shader2)
				{
					if (uidrawCall != null && uidrawCall.verts.Count != 0)
					{
						this.drawCalls.Add(uidrawCall);
						uidrawCall.UpdateGeometry(num);
						uidrawCall.onRender = this.mOnRender;
						this.mOnRender = null;
						num = 0;
						uidrawCall = null;
					}
					material = material2;
					texture = mainTexture;
					shader = shader2;
				}
				if (material != null || shader != null || texture != null)
				{
					if (uidrawCall == null)
					{
						uidrawCall = UIDrawCall.Create(this, material, texture, shader);
						uidrawCall.depthStart = uiwidget.depth;
						uidrawCall.depthEnd = uidrawCall.depthStart;
						uidrawCall.panel = this;
						uidrawCall.onCreateDrawCall = this.onCreateDrawCall;
					}
					else
					{
						int depth = uiwidget.depth;
						if (depth < uidrawCall.depthStart)
						{
							uidrawCall.depthStart = depth;
						}
						if (depth > uidrawCall.depthEnd)
						{
							uidrawCall.depthEnd = depth;
						}
					}
					uiwidget.drawCall = uidrawCall;
					num++;
					if (this.generateNormals)
					{
						uiwidget.WriteToBuffers(uidrawCall.verts, uidrawCall.uvs, uidrawCall.cols, uidrawCall.norms, uidrawCall.tans, this.generateUV2 ? uidrawCall.uv2 : null);
					}
					else
					{
						uiwidget.WriteToBuffers(uidrawCall.verts, uidrawCall.uvs, uidrawCall.cols, null, null, this.generateUV2 ? uidrawCall.uv2 : null);
					}
					if (uiwidget.mOnRender != null)
					{
						if (this.mOnRender == null)
						{
							this.mOnRender = uiwidget.mOnRender;
						}
						else
						{
							this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uiwidget.mOnRender);
						}
					}
				}
			}
			else
			{
				uiwidget.drawCall = null;
			}
		}
		if (uidrawCall != null && uidrawCall.verts.Count != 0)
		{
			this.drawCalls.Add(uidrawCall);
			uidrawCall.UpdateGeometry(num);
			uidrawCall.onRender = this.mOnRender;
			this.mOnRender = null;
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x000345B4 File Offset: 0x000327B4
	public bool FillDrawCall(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int num = 0;
			int i = 0;
			while (i < this.widgets.Count)
			{
				UIWidget uiwidget = this.widgets[i];
				if (uiwidget == null)
				{
					this.widgets.RemoveAt(i);
				}
				else
				{
					if (uiwidget.drawCall == dc)
					{
						if (uiwidget.isVisible && uiwidget.hasVertices)
						{
							num++;
							if (this.generateNormals)
							{
								uiwidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, dc.norms, dc.tans, this.generateUV2 ? dc.uv2 : null);
							}
							else
							{
								uiwidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null, this.generateUV2 ? dc.uv2 : null);
							}
							if (uiwidget.mOnRender != null)
							{
								if (this.mOnRender == null)
								{
									this.mOnRender = uiwidget.mOnRender;
								}
								else
								{
									this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uiwidget.mOnRender);
								}
							}
						}
						else
						{
							uiwidget.drawCall = null;
						}
					}
					i++;
				}
			}
			if (dc.verts.Count != 0)
			{
				dc.UpdateGeometry(num);
				dc.onRender = this.mOnRender;
				this.mOnRender = null;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0003471C File Offset: 0x0003291C
	private void UpdateDrawCalls(int sortOrder)
	{
		Transform cachedTransform = base.cachedTransform;
		bool usedForUI = this.usedForUI;
		if (this.clipping != UIDrawCall.Clipping.None)
		{
			this.drawCallClipRange = this.finalClipRegion;
			this.drawCallClipRange.z = this.drawCallClipRange.z * 0.5f;
			this.drawCallClipRange.w = this.drawCallClipRange.w * 0.5f;
		}
		else
		{
			this.drawCallClipRange = Vector4.zero;
		}
		int width = Screen.width;
		int height = Screen.height;
		if (this.drawCallClipRange.z == 0f)
		{
			this.drawCallClipRange.z = (float)width * 0.5f;
		}
		if (this.drawCallClipRange.w == 0f)
		{
			this.drawCallClipRange.w = (float)height * 0.5f;
		}
		if (this.halfPixelOffset)
		{
			this.drawCallClipRange.x = this.drawCallClipRange.x - 0.5f;
			this.drawCallClipRange.y = this.drawCallClipRange.y + 0.5f;
		}
		Vector3 vector;
		if (usedForUI)
		{
			Transform parent = base.cachedTransform.parent;
			vector = base.cachedTransform.localPosition;
			if (this.clipping != UIDrawCall.Clipping.None)
			{
				vector.x = (float)Mathf.RoundToInt(vector.x);
				vector.y = (float)Mathf.RoundToInt(vector.y);
			}
			if (parent != null)
			{
				vector = parent.TransformPoint(vector);
			}
			vector += this.drawCallOffset;
		}
		else
		{
			vector = cachedTransform.position;
		}
		Quaternion rotation = cachedTransform.rotation;
		Vector3 lossyScale = cachedTransform.lossyScale;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			Transform cachedTransform2 = uidrawCall.cachedTransform;
			cachedTransform2.position = vector;
			cachedTransform2.rotation = rotation;
			cachedTransform2.localScale = lossyScale;
			uidrawCall.renderQueue = ((this.renderQueue == UIPanel.RenderQueue.Explicit) ? this.startingRenderQueue : (this.startingRenderQueue + i));
			uidrawCall.alwaysOnScreen = (this.alwaysOnScreen && (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip));
			uidrawCall.sortingOrder = (this.useSortingOrder ? ((this.mSortingOrder == 0 && this.renderQueue == UIPanel.RenderQueue.Automatic) ? sortOrder : this.mSortingOrder) : 0);
			uidrawCall.sortingLayerName = (this.useSortingOrder ? this.mSortingLayerName : null);
			uidrawCall.clipTexture = this.mClipTexture;
			uidrawCall.shadowMode = this.shadowMode;
		}
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00034970 File Offset: 0x00032B70
	public void UpdateLayers()
	{
		if (this.mLayer != base.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			int i = 0;
			int count = this.widgets.Count;
			while (i < count)
			{
				UIWidget uiwidget = this.widgets[i];
				if (uiwidget && uiwidget.parent == this)
				{
					uiwidget.gameObject.layer = this.mLayer;
				}
				i++;
			}
			base.ResetAnchors();
			for (int j = 0; j < this.drawCalls.Count; j++)
			{
				this.drawCalls[j].gameObject.layer = this.mLayer;
			}
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00034A28 File Offset: 0x00032C28
	private void UpdateWidgets()
	{
		bool flag = false;
		bool flag2 = false;
		bool hasCumulativeClipping = this.hasCumulativeClipping;
		if (!this.cullWhileDragging)
		{
			for (int i = 0; i < UIScrollView.list.size; i++)
			{
				UIScrollView uiscrollView = UIScrollView.list[i];
				if (uiscrollView.panel == this && uiscrollView.isDragging)
				{
					flag2 = true;
				}
			}
		}
		if (this.mForced != flag2)
		{
			this.mForced = flag2;
			this.mResized = true;
		}
		int frameCount = Time.frameCount;
		int j = 0;
		int count = this.widgets.Count;
		while (j < count)
		{
			UIWidget uiwidget;
			try
			{
				uiwidget = this.widgets[j];
			}
			catch (ArgumentOutOfRangeException)
			{
				goto IL_15F;
			}
			goto IL_9E;
			IL_15F:
			j++;
			continue;
			IL_9E:
			if (!(uiwidget.panel == this) || !uiwidget.enabled)
			{
				goto IL_15F;
			}
			if (uiwidget.UpdateTransform(frameCount) || this.mResized || (this.mHasMoved && !this.alwaysOnScreen))
			{
				bool visibleByAlpha = flag2 || uiwidget.CalculateCumulativeAlpha(frameCount) > 0.001f;
				uiwidget.UpdateVisibility(visibleByAlpha, flag2 || this.alwaysOnScreen || (!hasCumulativeClipping && !uiwidget.hideIfOffScreen) || this.IsVisible(uiwidget));
			}
			if (!uiwidget.UpdateGeometry(frameCount))
			{
				goto IL_15F;
			}
			flag = true;
			if (this.mRebuild)
			{
				goto IL_15F;
			}
			if (uiwidget.drawCall != null)
			{
				uiwidget.drawCall.isDirty = true;
				goto IL_15F;
			}
			this.FindDrawCall(uiwidget);
			goto IL_15F;
		}
		if (flag && this.onGeometryUpdated != null)
		{
			this.onGeometryUpdated();
		}
		this.mResized = false;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00034BD0 File Offset: 0x00032DD0
	public UIDrawCall FindDrawCall(UIWidget w)
	{
		Material material = w.material;
		Texture mainTexture = w.mainTexture;
		Shader shader = w.shader;
		int depth = w.depth;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			int num = (i == 0) ? int.MinValue : (this.drawCalls[i - 1].depthEnd + 1);
			int num2 = (i + 1 == this.drawCalls.Count) ? int.MaxValue : (this.drawCalls[i + 1].depthStart - 1);
			if (num <= depth && num2 >= depth)
			{
				if (uidrawCall.baseMaterial == material && uidrawCall.shader == shader && uidrawCall.mainTexture == mainTexture)
				{
					if (w.isVisible)
					{
						w.drawCall = uidrawCall;
						if (w.hasVertices)
						{
							uidrawCall.isDirty = true;
						}
						return uidrawCall;
					}
				}
				else
				{
					this.mRebuild = true;
				}
				return null;
			}
		}
		this.mRebuild = true;
		return null;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00034CE4 File Offset: 0x00032EE4
	public void AddWidget(UIWidget w)
	{
		this.mUpdateScroll = true;
		if (this.widgets.Count == 0)
		{
			this.widgets.Add(w);
		}
		else if (this.mSortWidgets)
		{
			this.widgets.Add(w);
			this.SortWidgets();
		}
		else if (UIWidget.PanelCompareFunc(w, this.widgets[0]) == -1)
		{
			this.widgets.Insert(0, w);
		}
		else
		{
			int i = this.widgets.Count;
			while (i > 0)
			{
				if (UIWidget.PanelCompareFunc(w, this.widgets[--i]) != -1)
				{
					this.widgets.Insert(i + 1, w);
					break;
				}
			}
		}
		this.FindDrawCall(w);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00034D98 File Offset: 0x00032F98
	public void RemoveWidget(UIWidget w)
	{
		if (this.widgets.Remove(w) && w.drawCall != null)
		{
			int depth = w.depth;
			if (depth == w.drawCall.depthStart || depth == w.drawCall.depthEnd)
			{
				this.mRebuild = true;
			}
			w.drawCall.isDirty = true;
			w.drawCall = null;
		}
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00034DFE File Offset: 0x00032FFE
	public void Refresh()
	{
		this.mRebuild = true;
		UIPanel.mUpdateFrame = -1;
		if (UIPanel.list.Count > 0)
		{
			UIPanel.list[0].LateUpdate();
		}
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00034E2C File Offset: 0x0003302C
	public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		Vector4 finalClipRegion = this.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
		Vector2 maxArea = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
		if (this.softBorderPadding && this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += this.mClipSoftness.x;
			minArea.y += this.mClipSoftness.y;
			maxArea.x -= this.mClipSoftness.x;
			maxArea.y -= this.mClipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00034F24 File Offset: 0x00033124
	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = targetBounds.min;
		Vector3 vector2 = targetBounds.max;
		float num = 1f;
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				num = root.pixelSizeAdjustment;
			}
		}
		if (num != 1f)
		{
			vector /= num;
			vector2 /= num;
		}
		Vector3 b = this.CalculateConstrainOffset(vector, vector2) * num;
		if (b.sqrMagnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += b;
				targetBounds.center += b;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + b, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00035018 File Offset: 0x00033218
	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bounds, immediate);
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x0003503C File Offset: 0x0003323C
	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, false, -1);
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00035046 File Offset: 0x00033246
	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		return UIPanel.Find(trans, createIfMissing, -1);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00035050 File Offset: 0x00033250
	public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(trans);
		if (uipanel != null)
		{
			return uipanel;
		}
		while (trans.parent != null)
		{
			trans = trans.parent;
		}
		if (!createIfMissing)
		{
			return null;
		}
		return NGUITools.CreateUI(trans, false, layer);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00035094 File Offset: 0x00033294
	public Vector2 GetWindowSize()
	{
		UIRoot root = base.root;
		Vector2 vector = NGUITools.screenSize;
		if (root != null)
		{
			vector *= root.GetPixelSizeAdjustment(Mathf.RoundToInt(vector.y));
		}
		return vector;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x000350D0 File Offset: 0x000332D0
	public Vector2 GetViewSize()
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			return new Vector2(this.mClipRange.z, this.mClipRange.w);
		}
		return NGUITools.screenSize;
	}

	// Token: 0x04000505 RID: 1285
	public static List<UIPanel> list = new List<UIPanel>();

	// Token: 0x04000506 RID: 1286
	public UIPanel.OnGeometryUpdated onGeometryUpdated;

	// Token: 0x04000507 RID: 1287
	public bool showInPanelTool = true;

	// Token: 0x04000508 RID: 1288
	public bool generateNormals;

	// Token: 0x04000509 RID: 1289
	public bool generateUV2;

	// Token: 0x0400050A RID: 1290
	public UIDrawCall.ShadowMode shadowMode;

	// Token: 0x0400050B RID: 1291
	public bool widgetsAreStatic;

	// Token: 0x0400050C RID: 1292
	public bool cullWhileDragging = true;

	// Token: 0x0400050D RID: 1293
	public bool alwaysOnScreen;

	// Token: 0x0400050E RID: 1294
	public bool anchorOffset;

	// Token: 0x0400050F RID: 1295
	public bool softBorderPadding = true;

	// Token: 0x04000510 RID: 1296
	public UIPanel.RenderQueue renderQueue;

	// Token: 0x04000511 RID: 1297
	public int startingRenderQueue = 3000;

	// Token: 0x04000512 RID: 1298
	[NonSerialized]
	public List<UIWidget> widgets = new List<UIWidget>();

	// Token: 0x04000513 RID: 1299
	[NonSerialized]
	public List<UIDrawCall> drawCalls = new List<UIDrawCall>();

	// Token: 0x04000514 RID: 1300
	[NonSerialized]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	// Token: 0x04000515 RID: 1301
	[NonSerialized]
	public Vector4 drawCallClipRange = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04000516 RID: 1302
	public UIPanel.OnClippingMoved onClipMove;

	// Token: 0x04000517 RID: 1303
	public UIPanel.OnCreateMaterial onCreateMaterial;

	// Token: 0x04000518 RID: 1304
	public UIDrawCall.OnCreateDrawCall onCreateDrawCall;

	// Token: 0x04000519 RID: 1305
	[HideInInspector]
	[SerializeField]
	private Texture2D mClipTexture;

	// Token: 0x0400051A RID: 1306
	[HideInInspector]
	[SerializeField]
	private float mAlpha = 1f;

	// Token: 0x0400051B RID: 1307
	[HideInInspector]
	[SerializeField]
	private UIDrawCall.Clipping mClipping;

	// Token: 0x0400051C RID: 1308
	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = new Vector4(0f, 0f, 300f, 200f);

	// Token: 0x0400051D RID: 1309
	[HideInInspector]
	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(4f, 4f);

	// Token: 0x0400051E RID: 1310
	[HideInInspector]
	[SerializeField]
	private int mDepth;

	// Token: 0x0400051F RID: 1311
	[HideInInspector]
	[SerializeField]
	private int mSortingOrder;

	// Token: 0x04000520 RID: 1312
	[HideInInspector]
	[SerializeField]
	private string mSortingLayerName;

	// Token: 0x04000521 RID: 1313
	private bool mRebuild;

	// Token: 0x04000522 RID: 1314
	private bool mResized;

	// Token: 0x04000523 RID: 1315
	[SerializeField]
	private Vector2 mClipOffset = Vector2.zero;

	// Token: 0x04000524 RID: 1316
	private int mMatrixFrame = -1;

	// Token: 0x04000525 RID: 1317
	private int mAlphaFrameID;

	// Token: 0x04000526 RID: 1318
	private int mLayer = -1;

	// Token: 0x04000527 RID: 1319
	private static float[] mTemp = new float[4];

	// Token: 0x04000528 RID: 1320
	private Vector2 mMin = Vector2.zero;

	// Token: 0x04000529 RID: 1321
	private Vector2 mMax = Vector2.zero;

	// Token: 0x0400052A RID: 1322
	private bool mSortWidgets;

	// Token: 0x0400052B RID: 1323
	private bool mUpdateScroll;

	// Token: 0x0400052C RID: 1324
	public bool useSortingOrder;

	// Token: 0x0400052D RID: 1325
	private UIPanel mParentPanel;

	// Token: 0x0400052E RID: 1326
	private static Vector3[] mCorners = new Vector3[4];

	// Token: 0x0400052F RID: 1327
	private static int mUpdateFrame = -1;

	// Token: 0x04000530 RID: 1328
	[NonSerialized]
	private bool mHasMoved;

	// Token: 0x04000531 RID: 1329
	private UIDrawCall.OnRenderCallback mOnRender;

	// Token: 0x04000532 RID: 1330
	private bool mForced;

	// Token: 0x02000574 RID: 1396
	public enum RenderQueue
	{
		// Token: 0x040024E6 RID: 9446
		Automatic,
		// Token: 0x040024E7 RID: 9447
		StartAt,
		// Token: 0x040024E8 RID: 9448
		Explicit
	}

	// Token: 0x02000575 RID: 1397
	// (Invoke) Token: 0x0600383F RID: 14399
	public delegate void OnGeometryUpdated();

	// Token: 0x02000576 RID: 1398
	// (Invoke) Token: 0x06003843 RID: 14403
	public delegate void OnClippingMoved(UIPanel panel);

	// Token: 0x02000577 RID: 1399
	// (Invoke) Token: 0x06003847 RID: 14407
	public delegate Material OnCreateMaterial(UIWidget widget, Material mat);
}
