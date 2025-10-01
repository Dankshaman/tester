using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200006D RID: 109
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Widget")]
public class UIWidget : UIRect
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060004BB RID: 1211 RVA: 0x0002302F File Offset: 0x0002122F
	// (set) Token: 0x060004BC RID: 1212 RVA: 0x00023038 File Offset: 0x00021238
	public UIDrawCall.OnRenderCallback onRender
	{
		get
		{
			return this.mOnRender;
		}
		set
		{
			if (this.mOnRender != value)
			{
				if (this.drawCall != null && this.drawCall.onRender != null && this.mOnRender != null)
				{
					UIDrawCall uidrawCall = this.drawCall;
					uidrawCall.onRender = (UIDrawCall.OnRenderCallback)Delegate.Remove(uidrawCall.onRender, this.mOnRender);
				}
				this.mOnRender = value;
				if (this.drawCall != null)
				{
					UIDrawCall uidrawCall2 = this.drawCall;
					uidrawCall2.onRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(uidrawCall2.onRender, value);
				}
			}
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060004BD RID: 1213 RVA: 0x000230C8 File Offset: 0x000212C8
	// (set) Token: 0x060004BE RID: 1214 RVA: 0x000230D0 File Offset: 0x000212D0
	public Vector4 drawRegion
	{
		get
		{
			return this.mDrawRegion;
		}
		set
		{
			if (this.mDrawRegion != value)
			{
				this.mDrawRegion = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x000230FB File Offset: 0x000212FB
	public Vector2 pivotOffset
	{
		get
		{
			return NGUIMath.GetPivotOffset(this.pivot);
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00023108 File Offset: 0x00021308
	// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00023110 File Offset: 0x00021310
	public int width
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			int minWidth = this.minWidth;
			if (value < minWidth)
			{
				value = minWidth;
			}
			if (this.mWidth != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnHeight)
			{
				if (this.isAnchoredHorizontally)
				{
					if (this.leftAnchor.target != null && this.rightAnchor.target != null)
					{
						if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Left || this.mPivot == UIWidget.Pivot.TopLeft)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
							return;
						}
						if (this.mPivot == UIWidget.Pivot.BottomRight || this.mPivot == UIWidget.Pivot.Right || this.mPivot == UIWidget.Pivot.TopRight)
						{
							NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
							return;
						}
						int num = value - this.mWidth;
						num -= (num & 1);
						if (num != 0)
						{
							NGUIMath.AdjustWidget(this, (float)(-(float)num) * 0.5f, 0f, (float)num * 0.5f, 0f);
							return;
						}
					}
					else
					{
						if (this.leftAnchor.target != null)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
							return;
						}
						NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
						return;
					}
				}
				else
				{
					this.SetDimensions(value, this.mHeight);
				}
			}
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00023282 File Offset: 0x00021482
	// (set) Token: 0x060004C3 RID: 1219 RVA: 0x0002328C File Offset: 0x0002148C
	public int height
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			int minHeight = this.minHeight;
			if (value < minHeight)
			{
				value = minHeight;
			}
			if (this.mHeight != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnWidth)
			{
				if (this.isAnchoredVertically)
				{
					if (this.bottomAnchor.target != null && this.topAnchor.target != null)
					{
						if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Bottom || this.mPivot == UIWidget.Pivot.BottomRight)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
							return;
						}
						if (this.mPivot == UIWidget.Pivot.TopLeft || this.mPivot == UIWidget.Pivot.Top || this.mPivot == UIWidget.Pivot.TopRight)
						{
							NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
							return;
						}
						int num = value - this.mHeight;
						num -= (num & 1);
						if (num != 0)
						{
							NGUIMath.AdjustWidget(this, 0f, (float)(-(float)num) * 0.5f, 0f, (float)num * 0.5f);
							return;
						}
					}
					else
					{
						if (this.bottomAnchor.target != null)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
							return;
						}
						NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
						return;
					}
				}
				else
				{
					this.SetDimensions(this.mWidth, value);
				}
			}
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000233FE File Offset: 0x000215FE
	// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00023408 File Offset: 0x00021608
	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (this.mColor != value)
			{
				bool includeChildren = this.mColor.a != value.a;
				this.mColor = value;
				this.Invalidate(includeChildren);
			}
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00023448 File Offset: 0x00021648
	// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00023455 File Offset: 0x00021655
	public override float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			if (this.mColor.a != value)
			{
				this.mColor.a = value;
				this.Invalidate(true);
			}
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00023478 File Offset: 0x00021678
	public bool isVisible
	{
		get
		{
			return this.mIsVisibleByPanel && this.mIsVisibleByAlpha && this.mIsInFront && this.finalAlpha > 0.001f && NGUITools.GetActive(this);
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000234A7 File Offset: 0x000216A7
	public bool hasVertices
	{
		get
		{
			return this.geometry != null && this.geometry.hasVertices;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060004CA RID: 1226 RVA: 0x000234BE File Offset: 0x000216BE
	// (set) Token: 0x060004CB RID: 1227 RVA: 0x000234C6 File Offset: 0x000216C6
	public UIWidget.Pivot rawPivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				this.mPivot = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x000234BE File Offset: 0x000216BE
	// (set) Token: 0x060004CD RID: 1229 RVA: 0x000234EC File Offset: 0x000216EC
	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector = this.worldCorners[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector2 = this.worldCorners[0];
				Transform cachedTransform = base.cachedTransform;
				Vector3 vector3 = cachedTransform.position;
				float z = cachedTransform.localPosition.z;
				vector3.x += vector.x - vector2.x;
				vector3.y += vector.y - vector2.y;
				base.cachedTransform.position = vector3;
				vector3 = base.cachedTransform.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				vector3.z = z;
				base.cachedTransform.localPosition = vector3;
			}
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060004CE RID: 1230 RVA: 0x000235C5 File Offset: 0x000217C5
	// (set) Token: 0x060004CF RID: 1231 RVA: 0x000235D0 File Offset: 0x000217D0
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
				if (this.panel != null)
				{
					this.panel.RemoveWidget(this);
				}
				this.mDepth = value;
				if (this.panel != null)
				{
					this.panel.AddWidget(this);
					if (!Application.isPlaying)
					{
						this.panel.SortWidgets();
						this.panel.RebuildAllDrawCalls();
					}
				}
			}
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00023640 File Offset: 0x00021840
	public int raycastDepth
	{
		get
		{
			if (this.panel == null)
			{
				this.CreatePanel();
			}
			if (!(this.panel != null))
			{
				return this.mDepth;
			}
			return this.mDepth + this.panel.depth * 1000;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00023690 File Offset: 0x00021890
	public override Vector3[] localCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			this.mCorners[0] = new Vector3(num, num2);
			this.mCorners[1] = new Vector3(num, y);
			this.mCorners[2] = new Vector3(x, y);
			this.mCorners[3] = new Vector3(x, num2);
			return this.mCorners;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00023728 File Offset: 0x00021928
	public virtual Vector2 localSize
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return localCorners[2] - localCorners[0];
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00023754 File Offset: 0x00021954
	public Vector3 localCenter
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00023780 File Offset: 0x00021980
	public override Vector3[] worldCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			this.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
			this.mCorners[1] = cachedTransform.TransformPoint(num, y, 0f);
			this.mCorners[2] = cachedTransform.TransformPoint(x, y, 0f);
			this.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
			return this.mCorners;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0002383C File Offset: 0x00021A3C
	public Vector3 worldCenter
	{
		get
		{
			return base.cachedTransform.TransformPoint(this.localCenter);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00023850 File Offset: 0x00021A50
	public virtual Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			return new Vector4((this.mDrawRegion.x == 0f) ? num : Mathf.Lerp(num, num3, this.mDrawRegion.x), (this.mDrawRegion.y == 0f) ? num2 : Mathf.Lerp(num2, num4, this.mDrawRegion.y), (this.mDrawRegion.z == 1f) ? num3 : Mathf.Lerp(num, num3, this.mDrawRegion.z), (this.mDrawRegion.w == 1f) ? num4 : Mathf.Lerp(num2, num4, this.mDrawRegion.w));
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00023937 File Offset: 0x00021B37
	// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0002393F File Offset: 0x00021B3F
	public virtual Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				this.RemoveFromPanel();
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00023964 File Offset: 0x00021B64
	// (set) Token: 0x060004DA RID: 1242 RVA: 0x00023989 File Offset: 0x00021B89
	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			if (!(material != null))
			{
				return null;
			}
			return material.mainTexture;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no mainTexture setter");
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x000239A0 File Offset: 0x00021BA0
	// (set) Token: 0x060004DC RID: 1244 RVA: 0x000239C5 File Offset: 0x00021BC5
	public virtual Shader shader
	{
		get
		{
			Material material = this.material;
			if (!(material != null))
			{
				return null;
			}
			return material.shader;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no shader setter");
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060004DD RID: 1245 RVA: 0x000239DC File Offset: 0x00021BDC
	[Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
	public Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x000239E3 File Offset: 0x00021BE3
	public bool hasBoxCollider
	{
		get
		{
			return base.GetComponent<Collider>() as BoxCollider != null || base.GetComponent<BoxCollider2D>() != null;
		}
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00023A08 File Offset: 0x00021C08
	public void SetDimensions(int w, int h)
	{
		if (this.mWidth != w || this.mHeight != h)
		{
			this.mWidth = w;
			this.mHeight = h;
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnWidth)
			{
				this.mHeight = Mathf.RoundToInt((float)this.mWidth / this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				this.mWidth = Mathf.RoundToInt((float)this.mHeight * this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.Free)
			{
				this.aspectRatio = (float)this.mWidth / (float)this.mHeight;
			}
			this.mMoved = true;
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00023AB8 File Offset: 0x00021CB8
	public override Vector3[] GetSides(Transform relativeTo)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = -pivotOffset.x * (float)this.mWidth;
		float num2 = -pivotOffset.y * (float)this.mHeight;
		float num3 = num + (float)this.mWidth;
		float num4 = num2 + (float)this.mHeight;
		float x = (num + num3) * 0.5f;
		float y = (num2 + num4) * 0.5f;
		Transform cachedTransform = base.cachedTransform;
		this.mCorners[0] = cachedTransform.TransformPoint(num, y, 0f);
		this.mCorners[1] = cachedTransform.TransformPoint(x, num4, 0f);
		this.mCorners[2] = cachedTransform.TransformPoint(num3, y, 0f);
		this.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.mCorners[i] = relativeTo.InverseTransformPoint(this.mCorners[i]);
			}
		}
		return this.mCorners;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00023BC7 File Offset: 0x00021DC7
	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			this.UpdateFinalAlpha(frameID);
		}
		return this.finalAlpha;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00023BE8 File Offset: 0x00021DE8
	protected void UpdateFinalAlpha(int frameID)
	{
		if (!this.mIsVisibleByAlpha || !this.mIsInFront)
		{
			this.finalAlpha = 0f;
			return;
		}
		UIRect parent = base.parent;
		this.finalAlpha = ((parent != null) ? (parent.CalculateFinalAlpha(frameID) * this.mColor.a) : this.mColor.a);
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00023C48 File Offset: 0x00021E48
	public override void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		this.mAlphaFrameID = -1;
		if (this.panel != null)
		{
			bool visibleByPanel = (!this.hideIfOffScreen && !this.panel.hasCumulativeClipping) || this.panel.IsVisible(this);
			this.UpdateVisibility(this.CalculateCumulativeAlpha(Time.frameCount) > 0.001f, visibleByPanel);
			this.UpdateFinalAlpha(Time.frameCount);
			if (includeChildren)
			{
				base.Invalidate(true);
			}
		}
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00023CC8 File Offset: 0x00021EC8
	public float CalculateCumulativeAlpha(int frameID)
	{
		UIRect parent = base.parent;
		if (!(parent != null))
		{
			return this.mColor.a;
		}
		return parent.CalculateFinalAlpha(frameID) * this.mColor.a;
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00023D04 File Offset: 0x00021F04
	public override void SetRect(float x, float y, float width, float height)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = Mathf.Lerp(x, x + width, pivotOffset.x);
		float num2 = Mathf.Lerp(y, y + height, pivotOffset.y);
		int num3 = Mathf.FloorToInt(width + 0.5f);
		int num4 = Mathf.FloorToInt(height + 0.5f);
		if (pivotOffset.x == 0.5f)
		{
			num3 = num3 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num4 = num4 >> 1 << 1;
		}
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(num + 0.5f);
		localPosition.y = Mathf.Floor(num2 + 0.5f);
		if (num3 < this.minWidth)
		{
			num3 = this.minWidth;
		}
		if (num4 < this.minHeight)
		{
			num4 = this.minHeight;
		}
		transform.localPosition = localPosition;
		this.width = num3;
		this.height = num4;
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

	// Token: 0x060004E6 RID: 1254 RVA: 0x00023E87 File Offset: 0x00022087
	public void ResizeCollider()
	{
		NGUITools.UpdateWidgetCollider(base.gameObject);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00023E94 File Offset: 0x00022094
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int FullCompareFunc(UIWidget left, UIWidget right)
	{
		int num = UIPanel.CompareFunc(left.panel, right.panel);
		if (num != 0)
		{
			return num;
		}
		return UIWidget.PanelCompareFunc(left, right);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00023EC0 File Offset: 0x000220C0
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int PanelCompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		Material material = left.material;
		Material material2 = right.material;
		if (material == material2)
		{
			return 0;
		}
		if (material == null)
		{
			return 1;
		}
		if (material2 == null)
		{
			return -1;
		}
		if (material.GetInstanceID() >= material2.GetInstanceID())
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00023F2D File Offset: 0x0002212D
	public Bounds CalculateBounds()
	{
		return this.CalculateBounds(null);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00023F38 File Offset: 0x00022138
	public Bounds CalculateBounds(Transform relativeParent)
	{
		if (relativeParent == null)
		{
			Vector3[] localCorners = this.localCorners;
			Bounds result = new Bounds(localCorners[0], Vector3.zero);
			for (int i = 1; i < 4; i++)
			{
				result.Encapsulate(localCorners[i]);
			}
			return result;
		}
		Matrix4x4 worldToLocalMatrix = relativeParent.worldToLocalMatrix;
		Vector3[] worldCorners = this.worldCorners;
		Bounds result2 = new Bounds(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[0]), Vector3.zero);
		for (int j = 1; j < 4; j++)
		{
			result2.Encapsulate(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[j]));
		}
		return result2;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00023FD7 File Offset: 0x000221D7
	public void SetDirty()
	{
		if (this.drawCall != null)
		{
			this.drawCall.isDirty = true;
			return;
		}
		if (this.isVisible && this.hasVertices)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0002400B File Offset: 0x0002220B
	public void RemoveFromPanel()
	{
		if (this.panel != null)
		{
			this.panel.RemoveWidget(this);
			this.panel = null;
		}
		this.drawCall = null;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00024038 File Offset: 0x00022238
	public virtual void MarkAsChanged()
	{
		if (NGUITools.GetActive(this))
		{
			this.mChanged = true;
			if (this.panel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !this.mPlayMode)
			{
				this.SetDirty();
				this.CheckLayer();
			}
		}
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0002408C File Offset: 0x0002228C
	public UIPanel CreatePanel()
	{
		if (this.mStarted && this.panel == null && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.panel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != null)
			{
				this.mParentFound = false;
				this.panel.AddWidget(this);
				this.CheckLayer();
				this.Invalidate(true);
			}
		}
		return this.panel;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00024118 File Offset: 0x00022318
	public void CheckLayer()
	{
		if (this.panel != null && this.panel.gameObject.layer != base.gameObject.layer)
		{
			UnityEngine.Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.panel.gameObject.layer;
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00024178 File Offset: 0x00022378
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		if (this.panel != null)
		{
			UIPanel y = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != y)
			{
				this.RemoveFromPanel();
				this.CreatePanel();
			}
		}
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000241CC File Offset: 0x000223CC
	protected override void Awake()
	{
		base.Awake();
		this.mPlayMode = Application.isPlaying;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x000241DF File Offset: 0x000223DF
	protected override void OnInit()
	{
		base.OnInit();
		this.RemoveFromPanel();
		this.mMoved = true;
		base.Update();
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x000241FC File Offset: 0x000223FC
	protected virtual void UpgradeFrom265()
	{
		Vector3 localScale = base.cachedTransform.localScale;
		this.mWidth = Mathf.Abs(Mathf.RoundToInt(localScale.x));
		this.mHeight = Mathf.Abs(Mathf.RoundToInt(localScale.y));
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0002424D File Offset: 0x0002244D
	protected override void OnStart()
	{
		this.CreatePanel();
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00024258 File Offset: 0x00022458
	protected override void OnAnchor()
	{
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector3 localPosition = cachedTransform.localPosition;
		Vector2 pivotOffset = this.pivotOffset;
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
				this.mIsInFront = true;
			}
			else
			{
				Vector3 localPos = base.GetLocalPos(this.leftAnchor, parent);
				num = localPos.x + (float)this.leftAnchor.absolute;
				num3 = localPos.y + (float)this.bottomAnchor.absolute;
				num2 = localPos.x + (float)this.rightAnchor.absolute;
				num4 = localPos.y + (float)this.topAnchor.absolute;
				this.mIsInFront = (!this.hideIfOffScreen || localPos.z >= 0f);
			}
		}
		else
		{
			this.mIsInFront = true;
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
				num = localPosition.x - pivotOffset.x * (float)this.mWidth;
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
				num2 = localPosition.x - pivotOffset.x * (float)this.mWidth + (float)this.mWidth;
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
				num3 = localPosition.y - pivotOffset.y * (float)this.mHeight;
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
				num4 = localPosition.y - pivotOffset.y * (float)this.mHeight + (float)this.mHeight;
			}
		}
		Vector3 vector = new Vector3(Mathf.Lerp(num, num2, pivotOffset.x), Mathf.Lerp(num3, num4, pivotOffset.y), localPosition.z);
		vector.x = Mathf.Round(vector.x);
		vector.y = Mathf.Round(vector.y);
		int num5 = Mathf.FloorToInt(num2 - num + 0.5f);
		int num6 = Mathf.FloorToInt(num4 - num3 + 0.5f);
		if (this.keepAspectRatio != UIWidget.AspectRatioSource.Free && this.aspectRatio != 0f)
		{
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				num5 = Mathf.RoundToInt((float)num6 * this.aspectRatio);
			}
			else
			{
				num6 = Mathf.RoundToInt((float)num5 / this.aspectRatio);
			}
		}
		if (num5 < this.minWidth)
		{
			num5 = this.minWidth;
		}
		if (num6 < this.minHeight)
		{
			num6 = this.minHeight;
		}
		if (Vector3.SqrMagnitude(localPosition - vector) > 0.001f)
		{
			base.cachedTransform.localPosition = vector;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
		}
		if (this.mHeightSetByLabelOn + 1 >= Time.frameCount)
		{
			num6 = this.mHeight;
		}
		if (this.mWidth != num5 || this.mHeight != num6)
		{
			this.mWidth = num5;
			this.mHeight = num6;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0002484D File Offset: 0x00022A4D
	protected override void OnUpdate()
	{
		if (this.panel == null)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00024864 File Offset: 0x00022A64
	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			this.MarkAsChanged();
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0002486F File Offset: 0x00022A6F
	protected override void OnDisable()
	{
		this.RemoveFromPanel();
		base.OnDisable();
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0002487D File Offset: 0x00022A7D
	private void OnDestroy()
	{
		this.RemoveFromPanel();
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00024885 File Offset: 0x00022A85
	public bool UpdateVisibility(bool visibleByAlpha, bool visibleByPanel)
	{
		if (this.mIsVisibleByAlpha != visibleByAlpha || this.mIsVisibleByPanel != visibleByPanel)
		{
			this.mChanged = true;
			this.mIsVisibleByAlpha = visibleByAlpha;
			this.mIsVisibleByPanel = visibleByPanel;
			return true;
		}
		return false;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000248B4 File Offset: 0x00022AB4
	public bool UpdateTransform(int frame)
	{
		Transform cachedTransform = base.cachedTransform;
		this.mPlayMode = Application.isPlaying;
		if (this.mMoved)
		{
			this.mMoved = true;
			this.mMatrixFrame = -1;
			cachedTransform.hasChanged = false;
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			this.mOldV0 = this.panel.worldToLocal.MultiplyPoint3x4(cachedTransform.TransformPoint(num, num2, 0f));
			this.mOldV1 = this.panel.worldToLocal.MultiplyPoint3x4(cachedTransform.TransformPoint(x, y, 0f));
		}
		else if (!this.panel.widgetsAreStatic && cachedTransform.hasChanged)
		{
			this.mMatrixFrame = -1;
			cachedTransform.hasChanged = false;
			Vector2 pivotOffset2 = this.pivotOffset;
			float num3 = -pivotOffset2.x * (float)this.mWidth;
			float num4 = -pivotOffset2.y * (float)this.mHeight;
			float x2 = num3 + (float)this.mWidth;
			float y2 = num4 + (float)this.mHeight;
			Vector3 b = this.panel.worldToLocal.MultiplyPoint3x4(cachedTransform.TransformPoint(num3, num4, 0f));
			Vector3 b2 = this.panel.worldToLocal.MultiplyPoint3x4(cachedTransform.TransformPoint(x2, y2, 0f));
			if (Vector3.SqrMagnitude(this.mOldV0 - b) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - b2) > 1E-06f)
			{
				this.mMoved = true;
				this.mOldV0 = b;
				this.mOldV1 = b2;
			}
		}
		if (this.mMoved && this.onChange != null)
		{
			this.onChange();
		}
		return this.mMoved || this.mChanged;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00024A98 File Offset: 0x00022C98
	public bool UpdateGeometry(int frame)
	{
		float num = this.CalculateFinalAlpha(frame);
		if (this.mIsVisibleByAlpha && this.mLastAlpha != num)
		{
			this.mChanged = true;
		}
		this.mLastAlpha = num;
		if (this.mChanged)
		{
			if (this.mIsVisibleByAlpha && num > 0.001f && this.shader != null)
			{
				bool hasVertices = this.geometry.hasVertices;
				if (this.fillGeometry)
				{
					this.geometry.Clear();
					this.OnFill(this.geometry.verts, this.geometry.uvs, this.geometry.cols);
				}
				if (this.geometry.hasVertices)
				{
					if (this.mMatrixFrame != frame)
					{
						this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
						this.mMatrixFrame = frame;
					}
					this.geometry.ApplyTransform(this.mLocalToPanel, this.panel.generateNormals);
					this.mMoved = false;
					this.mChanged = false;
					return true;
				}
				this.mChanged = false;
				return hasVertices;
			}
			else if (this.geometry.hasVertices)
			{
				if (this.fillGeometry)
				{
					this.geometry.Clear();
				}
				this.mMoved = false;
				this.mChanged = false;
				return true;
			}
		}
		else if (this.mMoved && this.geometry.hasVertices)
		{
			if (this.mMatrixFrame != frame)
			{
				this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
				this.mMatrixFrame = frame;
			}
			this.geometry.ApplyTransform(this.mLocalToPanel, this.panel.generateNormals);
			this.mMoved = false;
			this.mChanged = false;
			return true;
		}
		this.mMoved = false;
		this.mChanged = false;
		return false;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00024C6B File Offset: 0x00022E6B
	public void WriteToBuffers(List<Vector3> v, List<Vector2> u, List<Color> c, List<Vector3> n, List<Vector4> t, List<Vector4> u2)
	{
		this.geometry.WriteToBuffers(v, u, c, n, t, u2);
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00024C84 File Offset: 0x00022E84
	public virtual void MakePixelPerfect()
	{
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.z = Mathf.Round(localPosition.z);
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.y = Mathf.Round(localPosition.y);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		base.cachedTransform.localScale = new Vector3(Mathf.Sign(localScale.x), Mathf.Sign(localScale.y), 1f);
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x00024D16 File Offset: 0x00022F16
	public virtual int minWidth
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000500 RID: 1280 RVA: 0x00024D16 File Offset: 0x00022F16
	public virtual int minHeight
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000501 RID: 1281 RVA: 0x00024D19 File Offset: 0x00022F19
	// (set) Token: 0x06000502 RID: 1282 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
		set
		{
		}
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	{
	}

	// Token: 0x0400035C RID: 860
	[HideInInspector]
	[SerializeField]
	protected Color mColor = Color.white;

	// Token: 0x0400035D RID: 861
	[HideInInspector]
	[SerializeField]
	protected UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	// Token: 0x0400035E RID: 862
	[HideInInspector]
	[SerializeField]
	protected int mWidth = 100;

	// Token: 0x0400035F RID: 863
	[HideInInspector]
	[SerializeField]
	protected int mHeight = 100;

	// Token: 0x04000360 RID: 864
	[HideInInspector]
	[SerializeField]
	protected int mDepth;

	// Token: 0x04000361 RID: 865
	[Tooltip("Custom material, if desired")]
	[HideInInspector]
	[SerializeField]
	protected Material mMat;

	// Token: 0x04000362 RID: 866
	[HideInInspector]
	public bool RefuseFocus;

	// Token: 0x04000363 RID: 867
	[NonSerialized]
	protected int mHeightSetByLabelOn;

	// Token: 0x04000364 RID: 868
	public UIWidget.OnDimensionsChanged onChange;

	// Token: 0x04000365 RID: 869
	public UIWidget.OnPostFillCallback onPostFill;

	// Token: 0x04000366 RID: 870
	public UIDrawCall.OnRenderCallback mOnRender;

	// Token: 0x04000367 RID: 871
	public bool autoResizeBoxCollider;

	// Token: 0x04000368 RID: 872
	public bool hideIfOffScreen;

	// Token: 0x04000369 RID: 873
	public UIWidget.AspectRatioSource keepAspectRatio;

	// Token: 0x0400036A RID: 874
	public float aspectRatio = 1f;

	// Token: 0x0400036B RID: 875
	public UIWidget.HitCheck hitCheck;

	// Token: 0x0400036C RID: 876
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x0400036D RID: 877
	[NonSerialized]
	public UIGeometry geometry = new UIGeometry();

	// Token: 0x0400036E RID: 878
	[NonSerialized]
	public bool fillGeometry = true;

	// Token: 0x0400036F RID: 879
	[NonSerialized]
	protected bool mPlayMode = true;

	// Token: 0x04000370 RID: 880
	[NonSerialized]
	protected Vector4 mDrawRegion = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04000371 RID: 881
	[NonSerialized]
	private Matrix4x4 mLocalToPanel;

	// Token: 0x04000372 RID: 882
	[NonSerialized]
	private bool mIsVisibleByAlpha = true;

	// Token: 0x04000373 RID: 883
	[NonSerialized]
	private bool mIsVisibleByPanel = true;

	// Token: 0x04000374 RID: 884
	[NonSerialized]
	private bool mIsInFront = true;

	// Token: 0x04000375 RID: 885
	[NonSerialized]
	private float mLastAlpha;

	// Token: 0x04000376 RID: 886
	[NonSerialized]
	private bool mMoved;

	// Token: 0x04000377 RID: 887
	[NonSerialized]
	public UIDrawCall drawCall;

	// Token: 0x04000378 RID: 888
	[NonSerialized]
	protected Vector3[] mCorners = new Vector3[4];

	// Token: 0x04000379 RID: 889
	[NonSerialized]
	private int mAlphaFrameID = -1;

	// Token: 0x0400037A RID: 890
	private int mMatrixFrame = -1;

	// Token: 0x0400037B RID: 891
	private Vector3 mOldV0;

	// Token: 0x0400037C RID: 892
	private Vector3 mOldV1;

	// Token: 0x02000541 RID: 1345
	public enum Pivot
	{
		// Token: 0x0400244B RID: 9291
		TopLeft,
		// Token: 0x0400244C RID: 9292
		Top,
		// Token: 0x0400244D RID: 9293
		TopRight,
		// Token: 0x0400244E RID: 9294
		Left,
		// Token: 0x0400244F RID: 9295
		Center,
		// Token: 0x04002450 RID: 9296
		Right,
		// Token: 0x04002451 RID: 9297
		BottomLeft,
		// Token: 0x04002452 RID: 9298
		Bottom,
		// Token: 0x04002453 RID: 9299
		BottomRight
	}

	// Token: 0x02000542 RID: 1346
	// (Invoke) Token: 0x060037C9 RID: 14281
	public delegate void OnDimensionsChanged();

	// Token: 0x02000543 RID: 1347
	// (Invoke) Token: 0x060037CD RID: 14285
	public delegate void OnPostFillCallback(UIWidget widget, int bufferOffset, List<Vector3> verts, List<Vector2> uvs, List<Color> cols);

	// Token: 0x02000544 RID: 1348
	public enum AspectRatioSource
	{
		// Token: 0x04002455 RID: 9301
		Free,
		// Token: 0x04002456 RID: 9302
		BasedOnWidth,
		// Token: 0x04002457 RID: 9303
		BasedOnHeight
	}

	// Token: 0x02000545 RID: 1349
	// (Invoke) Token: 0x060037D1 RID: 14289
	public delegate bool HitCheck(Vector3 worldPos);
}
