using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000049 RID: 73
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar")]
public class UIProgressBar : UIWidgetContainer
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600022C RID: 556 RVA: 0x0000DBCE File Offset: 0x0000BDCE
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600022D RID: 557 RVA: 0x0000DBF0 File Offset: 0x0000BDF0
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600022E RID: 558 RVA: 0x0000DC1C File Offset: 0x0000BE1C
	// (set) Token: 0x0600022F RID: 559 RVA: 0x0000DC24 File Offset: 0x0000BE24
	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000230 RID: 560 RVA: 0x0000DC42 File Offset: 0x0000BE42
	// (set) Token: 0x06000231 RID: 561 RVA: 0x0000DC4A File Offset: 0x0000BE4A
	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000232 RID: 562 RVA: 0x0000DC68 File Offset: 0x0000BE68
	// (set) Token: 0x06000233 RID: 563 RVA: 0x0000DC70 File Offset: 0x0000BE70
	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				if (this.mStarted)
				{
					this.ForceUpdate();
				}
			}
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000234 RID: 564 RVA: 0x0000DC90 File Offset: 0x0000BE90
	// (set) Token: 0x06000235 RID: 565 RVA: 0x0000DCC1 File Offset: 0x0000BEC1
	public float value
	{
		get
		{
			if (this.numberOfSteps > 1)
			{
				return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return this.mValue;
		}
		set
		{
			this.Set(value, true);
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000236 RID: 566 RVA: 0x0000DCCB File Offset: 0x0000BECB
	// (set) Token: 0x06000237 RID: 567 RVA: 0x0000DD08 File Offset: 0x0000BF08
	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 1f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.GetComponent<Collider>() != null)
				{
					this.mFG.GetComponent<Collider>().enabled = (this.mFG.alpha > 0.001f);
				}
				else if (this.mFG.GetComponent<Collider2D>() != null)
				{
					this.mFG.GetComponent<Collider2D>().enabled = (this.mFG.alpha > 0.001f);
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.GetComponent<Collider>() != null)
				{
					this.mBG.GetComponent<Collider>().enabled = (this.mBG.alpha > 0.001f);
				}
				else if (this.mBG.GetComponent<Collider2D>() != null)
				{
					this.mBG.GetComponent<Collider2D>().enabled = (this.mBG.alpha > 0.001f);
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.GetComponent<Collider>() != null)
					{
						component.GetComponent<Collider>().enabled = (component.alpha > 0.001f);
						return;
					}
					if (component.GetComponent<Collider2D>() != null)
					{
						component.GetComponent<Collider2D>().enabled = (component.alpha > 0.001f);
					}
				}
			}
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000238 RID: 568 RVA: 0x0000DE98 File Offset: 0x0000C098
	protected bool isHorizontal
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.LeftToRight || this.mFill == UIProgressBar.FillDirection.RightToLeft;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000239 RID: 569 RVA: 0x0000DEAD File Offset: 0x0000C0AD
	protected bool isInverted
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.RightToLeft || this.mFill == UIProgressBar.FillDirection.TopToBottom;
		}
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000DEC4 File Offset: 0x0000C0C4
	public void Set(float val, bool notify = true)
	{
		val = Mathf.Clamp01(val);
		if (this.mValue != val)
		{
			float value = this.value;
			this.mValue = val;
			if (this.mStarted && value != this.value)
			{
				if (notify && NGUITools.GetActive(this) && EventDelegate.IsValid(this.onChange))
				{
					UIProgressBar.current = this;
					EventDelegate.Execute(this.onChange);
					UIProgressBar.current = null;
				}
				this.ForceUpdate();
			}
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000DF38 File Offset: 0x0000C138
	public void Start()
	{
		if (this.mStarted)
		{
			return;
		}
		this.mStarted = true;
		this.Upgrade();
		if (Application.isPlaying)
		{
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (UIProgressBar.current == null && this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	// Token: 0x0600023C RID: 572 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void Upgrade()
	{
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnStart()
	{
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000DFB4 File Offset: 0x0000C1B4
	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
	protected void OnValidate()
	{
		if (NGUITools.GetActive(this))
		{
			this.Upgrade();
			this.mIsDirty = true;
			float num = Mathf.Clamp01(this.mValue);
			if (this.mValue != num)
			{
				this.mValue = num;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 21)
			{
				this.numberOfSteps = 21;
			}
			this.ForceUpdate();
			return;
		}
		float num2 = Mathf.Clamp01(this.mValue);
		if (this.mValue != num2)
		{
			this.mValue = num2;
		}
		if (this.numberOfSteps < 0)
		{
			this.numberOfSteps = 0;
			return;
		}
		if (this.numberOfSteps > 21)
		{
			this.numberOfSteps = 21;
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000E06C File Offset: 0x0000C26C
	protected float ScreenToValue(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float distance;
		if (!plane.Raycast(ray, out distance))
		{
			return this.value;
		}
		return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(distance)));
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000E0DC File Offset: 0x0000C2DC
	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return this.value;
		}
		Vector3[] localCorners = this.mFG.localCorners;
		Vector3 vector = localCorners[2] - localCorners[0];
		if (this.isHorizontal)
		{
			float num = (localPos.x - localCorners[0].x) / vector.x;
			if (!this.isInverted)
			{
				return num;
			}
			return 1f - num;
		}
		else
		{
			float num2 = (localPos.y - localCorners[0].y) / vector.y;
			if (!this.isInverted)
			{
				return num2;
			}
			return 1f - num2;
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000E184 File Offset: 0x0000C384
	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		bool flag = false;
		if (this.mFG != null)
		{
			UIBasicSprite uibasicSprite = this.mFG as UIBasicSprite;
			if (this.isHorizontal)
			{
				if (uibasicSprite != null && uibasicSprite.type == UIBasicSprite.Type.Filled)
				{
					if (uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
					{
						uibasicSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
						uibasicSprite.invert = this.isInverted;
					}
					uibasicSprite.fillAmount = this.value;
				}
				else
				{
					this.mFG.drawRegion = (this.isInverted ? new Vector4(1f - this.value, 0f, 1f, 1f) : new Vector4(0f, 0f, this.value, 1f));
					this.mFG.enabled = true;
					flag = (this.value < 0.001f);
				}
			}
			else if (uibasicSprite != null && uibasicSprite.type == UIBasicSprite.Type.Filled)
			{
				if (uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
				{
					uibasicSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
					uibasicSprite.invert = this.isInverted;
				}
				uibasicSprite.fillAmount = this.value;
			}
			else
			{
				this.mFG.drawRegion = (this.isInverted ? new Vector4(0f, 1f - this.value, 1f, 1f) : new Vector4(0f, 0f, 1f, this.value));
				this.mFG.enabled = true;
				flag = (this.value < 0.001f);
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] array = (this.mFG != null) ? this.mFG.localCorners : this.mBG.localCorners;
			Vector4 vector = (this.mFG != null) ? this.mFG.border : this.mBG.border;
			Vector3[] array2 = array;
			int num = 0;
			array2[num].x = array2[num].x + vector.x;
			Vector3[] array3 = array;
			int num2 = 1;
			array3[num2].x = array3[num2].x + vector.x;
			Vector3[] array4 = array;
			int num3 = 2;
			array4[num3].x = array4[num3].x - vector.z;
			Vector3[] array5 = array;
			int num4 = 3;
			array5[num4].x = array5[num4].x - vector.z;
			Vector3[] array6 = array;
			int num5 = 0;
			array6[num5].y = array6[num5].y + vector.y;
			Vector3[] array7 = array;
			int num6 = 1;
			array7[num6].y = array7[num6].y - vector.w;
			Vector3[] array8 = array;
			int num7 = 2;
			array8[num7].y = array8[num7].y - vector.w;
			Vector3[] array9 = array;
			int num8 = 3;
			array9[num8].y = array9[num8].y + vector.y;
			Transform transform = (this.mFG != null) ? this.mFG.cachedTransform : this.mBG.cachedTransform;
			for (int i = 0; i < 4; i++)
			{
				array[i] = transform.TransformPoint(array[i]);
			}
			if (this.isHorizontal)
			{
				Vector3 a = Vector3.Lerp(array[0], array[1], 0.5f);
				Vector3 b = Vector3.Lerp(array[2], array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(a, b, this.isInverted ? (1f - this.value) : this.value));
			}
			else
			{
				Vector3 a2 = Vector3.Lerp(array[0], array[3], 0.5f);
				Vector3 b2 = Vector3.Lerp(array[1], array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(a2, b2, this.isInverted ? (1f - this.value) : this.value));
			}
		}
		if (flag)
		{
			this.mFG.enabled = false;
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000E578 File Offset: 0x0000C778
	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform parent = this.thumb.parent;
		if (parent != null)
		{
			worldPos = parent.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
			{
				this.thumb.localPosition = worldPos;
				return;
			}
		}
		else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
		{
			this.thumb.position = worldPos;
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000E61C File Offset: 0x0000C81C
	public virtual void OnPan(Vector2 delta)
	{
		if (base.enabled)
		{
			switch (this.mFill)
			{
			case UIProgressBar.FillDirection.LeftToRight:
			{
				float value = Mathf.Clamp01(this.mValue + delta.x);
				this.value = value;
				this.mValue = value;
				return;
			}
			case UIProgressBar.FillDirection.RightToLeft:
			{
				float value2 = Mathf.Clamp01(this.mValue - delta.x);
				this.value = value2;
				this.mValue = value2;
				return;
			}
			case UIProgressBar.FillDirection.BottomToTop:
			{
				float value3 = Mathf.Clamp01(this.mValue + delta.y);
				this.value = value3;
				this.mValue = value3;
				return;
			}
			case UIProgressBar.FillDirection.TopToBottom:
			{
				float value4 = Mathf.Clamp01(this.mValue - delta.y);
				this.value = value4;
				this.mValue = value4;
				break;
			}
			default:
				return;
			}
		}
	}

	// Token: 0x040001E8 RID: 488
	public static UIProgressBar current;

	// Token: 0x040001E9 RID: 489
	public UIProgressBar.OnDragFinished onDragFinished;

	// Token: 0x040001EA RID: 490
	public UIProgressBar.OnDragStart onDragStart;

	// Token: 0x040001EB RID: 491
	public Transform thumb;

	// Token: 0x040001EC RID: 492
	[HideInInspector]
	[SerializeField]
	protected UIWidget mBG;

	// Token: 0x040001ED RID: 493
	[HideInInspector]
	[SerializeField]
	protected UIWidget mFG;

	// Token: 0x040001EE RID: 494
	[HideInInspector]
	[SerializeField]
	protected float mValue = 1f;

	// Token: 0x040001EF RID: 495
	[HideInInspector]
	[SerializeField]
	protected UIProgressBar.FillDirection mFill;

	// Token: 0x040001F0 RID: 496
	[NonSerialized]
	protected bool mStarted;

	// Token: 0x040001F1 RID: 497
	[NonSerialized]
	protected Transform mTrans;

	// Token: 0x040001F2 RID: 498
	[NonSerialized]
	protected bool mIsDirty;

	// Token: 0x040001F3 RID: 499
	[NonSerialized]
	protected Camera mCam;

	// Token: 0x040001F4 RID: 500
	[NonSerialized]
	protected float mOffset;

	// Token: 0x040001F5 RID: 501
	[NonSerialized]
	protected bool mDragStarted;

	// Token: 0x040001F6 RID: 502
	public int numberOfSteps;

	// Token: 0x040001F7 RID: 503
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x02000515 RID: 1301
	public enum FillDirection
	{
		// Token: 0x040023DB RID: 9179
		LeftToRight,
		// Token: 0x040023DC RID: 9180
		RightToLeft,
		// Token: 0x040023DD RID: 9181
		BottomToTop,
		// Token: 0x040023DE RID: 9182
		TopToBottom
	}

	// Token: 0x02000516 RID: 1302
	// (Invoke) Token: 0x0600375F RID: 14175
	public delegate void OnDragFinished();

	// Token: 0x02000517 RID: 1303
	// (Invoke) Token: 0x06003763 RID: 14179
	public delegate void OnDragStart();
}
