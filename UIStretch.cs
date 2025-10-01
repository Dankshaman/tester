using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Stretch")]
public class UIStretch : MonoBehaviour
{
	// Token: 0x060007C8 RID: 1992 RVA: 0x00036518 File Offset: 0x00034718
	private void Awake()
	{
		this.mAnim = base.GetComponent<Animation>();
		this.mRect = default(Rect);
		this.mTrans = base.transform;
		this.mWidget = base.GetComponent<UIWidget>();
		this.mSprite = base.GetComponent<UISprite>();
		this.mPanel = base.GetComponent<UIPanel>();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0003658D File Offset: 0x0003478D
	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000365AF File Offset: 0x000347AF
	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000365C8 File Offset: 0x000347C8
	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		this.Update();
		this.mStarted = true;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0003664C File Offset: 0x0003484C
	private void Update()
	{
		if (this.mAnim != null && this.mAnim.isPlaying)
		{
			return;
		}
		if (this.style != UIStretch.Style.None)
		{
			UIWidget uiwidget = (this.container == null) ? null : this.container.GetComponent<UIWidget>();
			UIPanel uipanel = (this.container == null && uiwidget == null) ? null : this.container.GetComponent<UIPanel>();
			float num = 1f;
			if (uiwidget != null)
			{
				Bounds bounds = uiwidget.CalculateBounds(base.transform.parent);
				this.mRect.x = bounds.min.x;
				this.mRect.y = bounds.min.y;
				this.mRect.width = bounds.size.x;
				this.mRect.height = bounds.size.y;
			}
			else if (uipanel != null)
			{
				if (uipanel.clipping == UIDrawCall.Clipping.None)
				{
					float num2 = (this.mRoot != null) ? ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f) : 0.5f;
					this.mRect.xMin = (float)(-(float)Screen.width) * num2;
					this.mRect.yMin = (float)(-(float)Screen.height) * num2;
					this.mRect.xMax = -this.mRect.xMin;
					this.mRect.yMax = -this.mRect.yMin;
				}
				else
				{
					Vector4 finalClipRegion = uipanel.finalClipRegion;
					this.mRect.x = finalClipRegion.x - finalClipRegion.z * 0.5f;
					this.mRect.y = finalClipRegion.y - finalClipRegion.w * 0.5f;
					this.mRect.width = finalClipRegion.z;
					this.mRect.height = finalClipRegion.w;
				}
			}
			else if (this.container != null)
			{
				Transform parent = base.transform.parent;
				Bounds bounds2 = (parent != null) ? NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(this.container.transform);
				this.mRect.x = bounds2.min.x;
				this.mRect.y = bounds2.min.y;
				this.mRect.width = bounds2.size.x;
				this.mRect.height = bounds2.size.y;
			}
			else
			{
				if (!(this.uiCamera != null))
				{
					return;
				}
				this.mRect = this.uiCamera.pixelRect;
				if (this.mRoot != null)
				{
					num = this.mRoot.pixelSizeAdjustment;
				}
			}
			float num3 = this.mRect.width;
			float num4 = this.mRect.height;
			if (num != 1f && num4 > 1f)
			{
				float num5 = (float)this.mRoot.activeHeight / num4;
				num3 *= num5;
				num4 *= num5;
			}
			Vector3 vector = (this.mWidget != null) ? new Vector3((float)this.mWidget.width, (float)this.mWidget.height) : this.mTrans.localScale;
			if (this.style == UIStretch.Style.BasedOnHeight)
			{
				vector.x = this.relativeSize.x * num4;
				vector.y = this.relativeSize.y * num4;
			}
			else if (this.style == UIStretch.Style.FillKeepingRatio)
			{
				float num6 = num3 / num4;
				if (this.initialSize.x / this.initialSize.y < num6)
				{
					float num7 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num7;
				}
				else
				{
					float num8 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num8;
					vector.y = num4;
				}
			}
			else if (this.style == UIStretch.Style.FitInternalKeepingRatio)
			{
				float num9 = num3 / num4;
				if (this.initialSize.x / this.initialSize.y > num9)
				{
					float num10 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num10;
				}
				else
				{
					float num11 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num11;
					vector.y = num4;
				}
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x = this.relativeSize.x * num3;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y = this.relativeSize.y * num4;
				}
			}
			if (this.mSprite != null)
			{
				float num12 = (this.mSprite.atlas != null) ? this.mSprite.atlas.pixelSize : 1f;
				vector.x -= this.borderPadding.x * num12;
				vector.y -= this.borderPadding.y * num12;
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mSprite.width = Mathf.RoundToInt(vector.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mSprite.height = Mathf.RoundToInt(vector.y);
				}
				vector = Vector3.one;
			}
			else if (this.mWidget != null)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mWidget.width = Mathf.RoundToInt(vector.x - this.borderPadding.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mWidget.height = Mathf.RoundToInt(vector.y - this.borderPadding.y);
				}
				vector = Vector3.one;
			}
			else if (this.mPanel != null)
			{
				Vector4 baseClipRegion = this.mPanel.baseClipRegion;
				if (this.style != UIStretch.Style.Vertical)
				{
					baseClipRegion.z = vector.x - this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					baseClipRegion.w = vector.y - this.borderPadding.y;
				}
				this.mPanel.baseClipRegion = baseClipRegion;
				vector = Vector3.one;
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x -= this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y -= this.borderPadding.y;
				}
			}
			if (this.mTrans.localScale != vector)
			{
				this.mTrans.localScale = vector;
			}
			if (this.runOnlyOnce && Application.isPlaying)
			{
				base.enabled = false;
			}
		}
	}

	// Token: 0x0400055C RID: 1372
	public Camera uiCamera;

	// Token: 0x0400055D RID: 1373
	public GameObject container;

	// Token: 0x0400055E RID: 1374
	public UIStretch.Style style;

	// Token: 0x0400055F RID: 1375
	public bool runOnlyOnce = true;

	// Token: 0x04000560 RID: 1376
	public Vector2 relativeSize = Vector2.one;

	// Token: 0x04000561 RID: 1377
	public Vector2 initialSize = Vector2.one;

	// Token: 0x04000562 RID: 1378
	public Vector2 borderPadding = Vector2.zero;

	// Token: 0x04000563 RID: 1379
	[HideInInspector]
	[SerializeField]
	private UIWidget widgetContainer;

	// Token: 0x04000564 RID: 1380
	private Transform mTrans;

	// Token: 0x04000565 RID: 1381
	private UIWidget mWidget;

	// Token: 0x04000566 RID: 1382
	private UISprite mSprite;

	// Token: 0x04000567 RID: 1383
	private UIPanel mPanel;

	// Token: 0x04000568 RID: 1384
	private UIRoot mRoot;

	// Token: 0x04000569 RID: 1385
	private Animation mAnim;

	// Token: 0x0400056A RID: 1386
	private Rect mRect;

	// Token: 0x0400056B RID: 1387
	private bool mStarted;

	// Token: 0x0200057A RID: 1402
	public enum Style
	{
		// Token: 0x040024F3 RID: 9459
		None,
		// Token: 0x040024F4 RID: 9460
		Horizontal,
		// Token: 0x040024F5 RID: 9461
		Vertical,
		// Token: 0x040024F6 RID: 9462
		Both,
		// Token: 0x040024F7 RID: 9463
		BasedOnHeight,
		// Token: 0x040024F8 RID: 9464
		FillKeepingRatio,
		// Token: 0x040024F9 RID: 9465
		FitInternalKeepingRatio
	}
}
