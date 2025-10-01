using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00037BE1 File Offset: 0x00035DE1
	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00037C03 File Offset: 0x00035E03
	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x00037C0B File Offset: 0x00035E0B
	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00037C14 File Offset: 0x00035E14
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00037C7C File Offset: 0x00035E7C
	protected virtual void Update()
	{
		if (this.mTooltip != UICamera.tooltipObject)
		{
			this.mTooltip = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector = this.mSize * 0.25f;
				vector.y = -vector.y;
				Vector3 localScale = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
				this.mTrans.localPosition = localPosition;
				this.mTrans.localScale = localScale;
			}
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00037D94 File Offset: 0x00035F94
	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uiwidget = this.mWidgets[i];
			Color color = uiwidget.color;
			color.a = val;
			uiwidget.color = color;
			i++;
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00037DD4 File Offset: 0x00035FD4
	protected virtual void SetText(string tooltipText)
	{
		if (!(this.text != null) || string.IsNullOrEmpty(tooltipText))
		{
			this.mTooltip = null;
			this.mTarget = 0f;
			return;
		}
		this.mTarget = 1f;
		this.mTooltip = UICamera.tooltipObject;
		this.text.text = tooltipText;
		this.mPos = UICamera.lastEventPosition;
		Transform transform = this.text.transform;
		Vector3 localPosition = transform.localPosition;
		Vector3 localScale = transform.localScale;
		this.mSize = this.text.printedSize;
		this.mSize.x = this.mSize.x * localScale.x;
		this.mSize.y = this.mSize.y * localScale.y;
		if (this.background != null)
		{
			Vector4 border = this.background.border;
			this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
			this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
			this.background.width = Mathf.RoundToInt(this.mSize.x);
			this.background.height = Mathf.RoundToInt(this.mSize.y);
		}
		if (this.uiCamera != null)
		{
			this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
			this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
			float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
			float num2 = (float)Screen.height * 0.5f / num;
			Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
			this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
			this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
			this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
			this.mPos = this.mTrans.localPosition;
			this.mPos.x = Mathf.Round(this.mPos.x);
			this.mPos.y = Mathf.Round(this.mPos.y);
		}
		else
		{
			if (this.mPos.x + this.mSize.x > (float)Screen.width)
			{
				this.mPos.x = (float)Screen.width - this.mSize.x;
			}
			if (this.mPos.y - this.mSize.y < 0f)
			{
				this.mPos.y = this.mSize.y;
			}
			this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
			this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
		}
		this.mTrans.localPosition = this.mPos;
		if (this.tooltipRoot != null)
		{
			this.tooltipRoot.BroadcastMessage("UpdateAnchors");
			return;
		}
		this.text.BroadcastMessage("UpdateAnchors");
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0003819E File Offset: 0x0003639E
	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0003819E File Offset: 0x0003639E
	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x000381B8 File Offset: 0x000363B8
	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mTooltip = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}

	// Token: 0x0400057D RID: 1405
	protected static UITooltip mInstance;

	// Token: 0x0400057E RID: 1406
	public Camera uiCamera;

	// Token: 0x0400057F RID: 1407
	public UILabel text;

	// Token: 0x04000580 RID: 1408
	public GameObject tooltipRoot;

	// Token: 0x04000581 RID: 1409
	public UISprite background;

	// Token: 0x04000582 RID: 1410
	public float appearSpeed = 10f;

	// Token: 0x04000583 RID: 1411
	public bool scalingTransitions = true;

	// Token: 0x04000584 RID: 1412
	protected GameObject mTooltip;

	// Token: 0x04000585 RID: 1413
	protected Transform mTrans;

	// Token: 0x04000586 RID: 1414
	protected float mTarget;

	// Token: 0x04000587 RID: 1415
	protected float mCurrent;

	// Token: 0x04000588 RID: 1416
	protected Vector3 mPos;

	// Token: 0x04000589 RID: 1417
	protected Vector3 mSize = Vector3.zero;

	// Token: 0x0400058A RID: 1418
	protected UIWidget[] mWidgets;
}
