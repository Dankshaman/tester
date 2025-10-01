using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000039 RID: 57
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000139 RID: 313 RVA: 0x00007F98 File Offset: 0x00006198
	public static UISprite dropShadowSprite
	{
		get
		{
			if (UIDragObject._dropShadowSprite == null)
			{
				UIDragObject._dropShadowSprite = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.AddChild<UISprite>();
				UIDragObject._dropShadowSprite.type = UIBasicSprite.Type.Sliced;
				UIDragObject._dropShadowSprite.name = "DropShadow";
				UIDragObject._dropShadowSprite.spriteName = "DropShadow";
				UIDragObject._dropShadowSprite.color = Color.black;
			}
			return UIDragObject._dropShadowSprite;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600013A RID: 314 RVA: 0x00008003 File Offset: 0x00006203
	// (set) Token: 0x0600013B RID: 315 RVA: 0x0000800B File Offset: 0x0000620B
	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00008014 File Offset: 0x00006214
	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.rootPanel = NGUIHelper.GetTopMostParent<UIPanel>(base.gameObject, true);
		if (this.rootPanel == null)
		{
			GameObject parent = null;
			if (base.gameObject.transform.parent)
			{
				parent = base.gameObject.transform.parent.gameObject;
			}
			GameObject gameObject = NGUITools.AddChild(parent);
			Transform transform = base.gameObject.transform;
			transform.parent = gameObject.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.name = base.gameObject.name;
			this.rootPanel = gameObject.AddComponent<UIPanel>();
		}
		foreach (UIPanel uipanel in this.rootPanel.GetComponentsInChildren<UIPanel>(true))
		{
			if (!(uipanel == this.rootPanel))
			{
				UIDragObject.PanelOffset item = new UIDragObject.PanelOffset
				{
					panel = uipanel,
					offset = uipanel.depth - this.rootPanel.depth
				};
				this.childPanelOffsets.Add(item);
			}
		}
		this.backgroundSprite = base.GetComponent<UISprite>();
		this.buttons = base.GetComponentsInChildren<UIButton>(true);
		for (int j = 0; j < this.buttons.Length; j++)
		{
			UIButton uibutton = this.buttons[j];
			if (!uibutton.GetComponent<UIDragObject>())
			{
				EventDelegate.Add(uibutton.onClick, new EventDelegate.Callback(this.UpdateDepth));
			}
		}
		if (!this.HasDropShadow)
		{
			UIDragObject.dropShadowSprite.enabled = false;
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000081B8 File Offset: 0x000063B8
	private void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Length; i++)
		{
			UIButton uibutton = this.buttons[i];
			if (uibutton && !uibutton.GetComponent<UIDragObject>())
			{
				EventDelegate.Remove(uibutton.onClick, new EventDelegate.Callback(this.UpdateDepth));
			}
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00008216 File Offset: 0x00006416
	public void DelayUpdateDepth(bool Check = false)
	{
		base.StartCoroutine(this.CoroutineUpdateDepth(Check));
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00008228 File Offset: 0x00006428
	private void KeepOnScreen()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		Vector3[] worldCorners = base.GetComponent<UIWidget>().worldCorners;
		Camera mainCamera = UICamera.mainCamera;
		float num = 0.015f;
		for (int i = 0; i < worldCorners.Length; i++)
		{
			Vector3 vector = mainCamera.WorldToViewportPoint(worldCorners[i]);
			if (vector.x < 1f - num && vector.x > num && vector.y < 1f - num && vector.y > num)
			{
				return;
			}
		}
		this.target.transform.position = Vector3.zero;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000082BC File Offset: 0x000064BC
	private void TweenIn()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.TweenInDuration <= 0f)
		{
			return;
		}
		this.target.transform.localScale = Vector3.one * 0.02f;
		TweenScale tweenScale = TweenScale.Begin(this.target.gameObject, this.TweenInDuration, Vector3.one);
		tweenScale.method = UITweener.Method.EaseOut;
		tweenScale.AddOnFinished(new EventDelegate.Callback(this.FixSliders));
		Wait.Time(new Action(this.FixSliders), 0.05f, 1);
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000834C File Offset: 0x0000654C
	private void FixSliders()
	{
		if (this.target)
		{
			UISlider[] componentsInChildren = this.target.GetComponentsInChildren<UISlider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ForceUpdate();
			}
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00008388 File Offset: 0x00006588
	private IEnumerator CoroutineUpdateDepth(bool Check)
	{
		yield return null;
		if (Check && UIDragObject.FocusedDragObject != null && UIDragObject.FocusedDragObject.gameObject.activeInHierarchy)
		{
			yield break;
		}
		this.UpdateDepth();
		yield break;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x000083A0 File Offset: 0x000065A0
	private void UpdateDepth()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.ResetDepth)
		{
			if (UIDragObject.maxPanelDepth > 9999999)
			{
				Debug.Log("Reset Depth!");
				UIDragObject.maxPanelDepth = 1001;
			}
			UIDragObject.maxPanelDepth++;
			int num = -1;
			this.rootPanel.depth = UIDragObject.maxPanelDepth;
			foreach (UIDragObject.PanelOffset panelOffset in this.childPanelOffsets)
			{
				panelOffset.panel.depth = UIDragObject.maxPanelDepth + panelOffset.offset;
				if (panelOffset.panel.depth > UIDragObject.maxPanelDepth)
				{
					num = panelOffset.panel.depth;
				}
			}
			if (num != -1)
			{
				UIDragObject.maxPanelDepth = num;
			}
		}
		this.Focus();
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00008484 File Offset: 0x00006684
	private void Focus()
	{
		Transform parent = UIDragObject.dropShadowSprite.transform.parent;
		if (parent && parent != base.transform && parent.GetComponent<UIDrawAbove3D>())
		{
			parent.GetComponent<UIDrawAbove3D>().enabled = false;
		}
		base.gameObject.AddMissingComponent<UIDrawAbove3D>().enabled = true;
		this.UpdateDropShadow();
		EventManager.TriggerOnFocusUI(this);
		if (UIDragObject.ActiveDragObjects.Contains(this))
		{
			UIDragObject.ActiveDragObjects.Remove(this);
		}
		UIDragObject.ActiveDragObjects.Add(this);
		UIDragObject.FocusedDragObject = this;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00008518 File Offset: 0x00006718
	public void UpdateDropShadow()
	{
		UIDragObject.dropShadowSprite.transform.parent = this.backgroundSprite.transform;
		UIDragObject.dropShadowSprite.transform.Reset();
		UIDragObject.dropShadowSprite.transform.localPosition = new Vector3(0f, -2f, 0f);
		UIDragObject.dropShadowSprite.ParentHasChanged();
		UIDragObject.dropShadowSprite.atlas = this.backgroundSprite.atlas;
		UIDragObject.dropShadowSprite.depth = this.backgroundSprite.depth - 1;
		UIDragObject.dropShadowSprite.width = this.backgroundSprite.width + 40;
		UIDragObject.dropShadowSprite.height = this.backgroundSprite.height + 40;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000085D8 File Offset: 0x000067D8
	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.isPlaying)
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
		this.mTargetPos = ((this.target != null) ? this.target.position : Vector3.zero);
		this.KeepOnScreen();
		this.DelayUpdateDepth(false);
		if (this.DoTweenIn)
		{
			this.TweenIn();
			return;
		}
		this.target.transform.localScale = Vector3.one;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x000086AC File Offset: 0x000068AC
	private void OnDisable()
	{
		this.mStarted = false;
		if (UIDragObject.ActiveDragObjects.Contains(this))
		{
			if (UIDragObject.FocusedDragObject == this)
			{
				for (int i = UIDragObject.ActiveDragObjects.Count - 1; i >= 0; i--)
				{
					UIDragObject uidragObject = UIDragObject.ActiveDragObjects[i];
					if (!(uidragObject == this) && uidragObject && uidragObject.gameObject.activeInHierarchy)
					{
						uidragObject.DelayUpdateDepth(true);
						break;
					}
				}
				UIDragObject.FocusedDragObject = null;
			}
			UIDragObject.ActiveDragObjects.Remove(this);
		}
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00008738 File Offset: 0x00006938
	private void FindPanel()
	{
		this.panelRegion = ((this.target != null) ? UIPanel.Find(this.target.transform.parent) : null);
		if (this.panelRegion == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00008788 File Offset: 0x00006988
	private void UpdateBounds()
	{
		if (this.contentRect)
		{
			Matrix4x4 worldToLocalMatrix = this.panelRegion.cachedTransform.worldToLocalMatrix;
			Vector3[] worldCorners = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
			}
			this.mBounds = new Bounds(worldCorners[0], Vector3.zero);
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(worldCorners[j]);
			}
			return;
		}
		this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00008834 File Offset: 0x00006A34
	private void OnPress(bool pressed)
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		float timeScale = Time.timeScale;
		if (timeScale < 0.01f && timeScale != 0f)
		{
			return;
		}
		if (pressed)
		{
			this.UpdateDepth();
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.panelRegion == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transform = UICamera.currentCamera.transform;
					this.mPlane = new Plane(((this.panelRegion != null) ? this.panelRegion.cachedTransform.rotation : transform.rotation) * Vector3.back, UICamera.lastWorldPosition);
					return;
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000899C File Offset: 0x00006B9C
	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.zero;
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				Vector3 localPosition = this.target.localPosition;
				this.Move(vector);
				if (this.restrictWithinPanel)
				{
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - localPosition);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00008B50 File Offset: 0x00006D50
	private void Move(Vector3 worldDelta)
	{
		if (this.panelRegion != null)
		{
			this.mTargetPos += worldDelta;
			Transform parent = this.target.parent;
			Rigidbody component = this.target.GetComponent<Rigidbody>();
			if (parent != null)
			{
				Vector3 vector = parent.worldToLocalMatrix.MultiplyPoint3x4(this.mTargetPos);
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
				if (component != null)
				{
					vector = parent.localToWorldMatrix.MultiplyPoint3x4(vector);
					component.position = vector;
				}
				else
				{
					this.target.localPosition = vector;
				}
			}
			else if (component != null)
			{
				component.position = this.mTargetPos;
			}
			else
			{
				this.target.position = this.mTargetPos;
			}
			UIScrollView component2 = this.panelRegion.GetComponent<UIScrollView>();
			if (component2 != null)
			{
				component2.UpdateScrollbars(true);
				return;
			}
		}
		else
		{
			this.target.position += worldDelta;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00008C68 File Offset: 0x00006E68
	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, deltaTime);
		if (this.mMomentum.magnitude < 0.0001f)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.panelRegion == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
			if (this.restrictWithinPanel && this.panelRegion != null)
			{
				this.UpdateBounds();
				if (this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelMovement();
				}
				else
				{
					this.CancelSpring();
				}
			}
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
			if (this.mMomentum.magnitude < 0.0001f)
			{
				this.CancelMovement();
				return;
			}
		}
		else
		{
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00008D90 File Offset: 0x00006F90
	public void CancelMovement()
	{
		if (this.target != null)
		{
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			this.target.localPosition = localPosition;
		}
		this.mTargetPos = ((this.target != null) ? this.target.position : Vector3.zero);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00008E38 File Offset: 0x00007038
	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00008E61 File Offset: 0x00007061
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.mScroll -= this.scrollMomentum * (delta * 0.05f);
		}
	}

	// Token: 0x040000FA RID: 250
	public bool HasDropShadow = true;

	// Token: 0x040000FB RID: 251
	public bool DoTweenIn = true;

	// Token: 0x040000FC RID: 252
	public float TweenInDuration = 0.15f;

	// Token: 0x040000FD RID: 253
	private const int startMaxPanelDepth = 1001;

	// Token: 0x040000FE RID: 254
	private static int maxPanelDepth = 1001;

	// Token: 0x040000FF RID: 255
	private UIPanel rootPanel;

	// Token: 0x04000100 RID: 256
	public bool ResetDepth = true;

	// Token: 0x04000101 RID: 257
	private readonly List<UIDragObject.PanelOffset> childPanelOffsets = new List<UIDragObject.PanelOffset>();

	// Token: 0x04000102 RID: 258
	private UIButton[] buttons;

	// Token: 0x04000103 RID: 259
	private UISprite backgroundSprite;

	// Token: 0x04000104 RID: 260
	private static UISprite _dropShadowSprite = null;

	// Token: 0x04000105 RID: 261
	public Transform target;

	// Token: 0x04000106 RID: 262
	public UIPanel panelRegion;

	// Token: 0x04000107 RID: 263
	public Vector3 scrollMomentum = Vector3.zero;

	// Token: 0x04000108 RID: 264
	public bool restrictWithinPanel;

	// Token: 0x04000109 RID: 265
	public UIRect contentRect;

	// Token: 0x0400010A RID: 266
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x0400010B RID: 267
	public float momentumAmount = 35f;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	protected Vector3 scale = new Vector3(1f, 1f, 0f);

	// Token: 0x0400010D RID: 269
	[SerializeField]
	[HideInInspector]
	private float scrollWheelFactor;

	// Token: 0x0400010E RID: 270
	private Plane mPlane;

	// Token: 0x0400010F RID: 271
	private Vector3 mTargetPos;

	// Token: 0x04000110 RID: 272
	private Vector3 mLastPos;

	// Token: 0x04000111 RID: 273
	private Vector3 mMomentum = Vector3.zero;

	// Token: 0x04000112 RID: 274
	private Vector3 mScroll = Vector3.zero;

	// Token: 0x04000113 RID: 275
	private Bounds mBounds;

	// Token: 0x04000114 RID: 276
	private int mTouchID;

	// Token: 0x04000115 RID: 277
	private bool mStarted;

	// Token: 0x04000116 RID: 278
	private bool mPressed;

	// Token: 0x04000117 RID: 279
	public static List<UIDragObject> ActiveDragObjects = new List<UIDragObject>();

	// Token: 0x04000118 RID: 280
	public static UIDragObject FocusedDragObject = null;

	// Token: 0x02000506 RID: 1286
	private struct PanelOffset
	{
		// Token: 0x0400239E RID: 9118
		public UIPanel panel;

		// Token: 0x0400239F RID: 9119
		public int offset;
	}

	// Token: 0x02000507 RID: 1287
	public enum DragEffect
	{
		// Token: 0x040023A1 RID: 9121
		None,
		// Token: 0x040023A2 RID: 9122
		Momentum,
		// Token: 0x040023A3 RID: 9123
		MomentumAndSpring
	}
}
