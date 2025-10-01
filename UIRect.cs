using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public abstract class UIRect : MonoBehaviour
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x00022299 File Offset: 0x00020499
	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x000222BB File Offset: 0x000204BB
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

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x000222DD File Offset: 0x000204DD
	public Camera anchorCamera
	{
		get
		{
			if (!this.mAnchorsCached)
			{
				this.ResetAnchors();
			}
			return this.mCam;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x000222F4 File Offset: 0x000204F4
	public bool isFullyAnchored
	{
		get
		{
			return this.leftAnchor.target && this.rightAnchor.target && this.topAnchor.target && this.bottomAnchor.target;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000491 RID: 1169 RVA: 0x00022349 File Offset: 0x00020549
	public virtual bool isAnchoredHorizontally
	{
		get
		{
			return this.leftAnchor.target || this.rightAnchor.target;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000492 RID: 1170 RVA: 0x0002236F File Offset: 0x0002056F
	public virtual bool isAnchoredVertically
	{
		get
		{
			return this.bottomAnchor.target || this.topAnchor.target;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000493 RID: 1171 RVA: 0x00014D66 File Offset: 0x00012F66
	public virtual bool canBeAnchored
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000494 RID: 1172 RVA: 0x00022395 File Offset: 0x00020595
	public UIRect parent
	{
		get
		{
			if (!this.mParentFound)
			{
				this.mParentFound = true;
				this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
			}
			return this.mParent;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000495 RID: 1173 RVA: 0x000223C4 File Offset: 0x000205C4
	public UIRoot root
	{
		get
		{
			if (this.parent != null)
			{
				return this.mParent.root;
			}
			if (!this.mRootSet)
			{
				this.mRootSet = true;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
			}
			return this.mRoot;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x00022414 File Offset: 0x00020614
	public bool isAnchored
	{
		get
		{
			return (this.leftAnchor.target || this.rightAnchor.target || this.topAnchor.target || this.bottomAnchor.target) && this.canBeAnchored;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000497 RID: 1175
	// (set) Token: 0x06000498 RID: 1176
	public abstract float alpha { get; set; }

	// Token: 0x06000499 RID: 1177
	public abstract float CalculateFinalAlpha(int frameID);

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600049A RID: 1178
	public abstract Vector3[] localCorners { get; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600049B RID: 1179
	public abstract Vector3[] worldCorners { get; }

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600049C RID: 1180 RVA: 0x00022474 File Offset: 0x00020674
	protected float cameraRayDistance
	{
		get
		{
			if (this.anchorCamera == null)
			{
				return 0f;
			}
			if (!this.mCam.orthographic)
			{
				Transform cachedTransform = this.cachedTransform;
				Transform transform = this.mCam.transform;
				Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
				Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
				float result;
				if (plane.Raycast(ray, out result))
				{
					return result;
				}
			}
			return Mathf.Lerp(this.mCam.nearClipPlane, this.mCam.farClipPlane, 0.5f);
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00022520 File Offset: 0x00020720
	public virtual void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		if (includeChildren)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				this.mChildren.buffer[i].Invalidate(true);
			}
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00022560 File Offset: 0x00020760
	public virtual Vector3[] GetSides(Transform relativeTo)
	{
		if (this.anchorCamera != null)
		{
			return this.mCam.GetSides(this.cameraRayDistance, relativeTo);
		}
		Vector3 position = this.cachedTransform.position;
		for (int i = 0; i < 4; i++)
		{
			UIRect.mSides[i] = position;
		}
		if (relativeTo != null)
		{
			for (int j = 0; j < 4; j++)
			{
				UIRect.mSides[j] = relativeTo.InverseTransformPoint(UIRect.mSides[j]);
			}
		}
		return UIRect.mSides;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000225E8 File Offset: 0x000207E8
	protected Vector3 GetLocalPos(UIRect.AnchorPoint ac, Transform trans)
	{
		if (ac.targetCam == null)
		{
			this.FindCameraFor(ac);
		}
		if (this.anchorCamera == null || ac.targetCam == null)
		{
			return this.cachedTransform.localPosition;
		}
		Rect rect = ac.targetCam.rect;
		Vector3 vector = ac.targetCam.WorldToViewportPoint(ac.target.position);
		Vector3 vector2 = new Vector3(vector.x * rect.width + rect.x, vector.y * rect.height + rect.y, vector.z);
		vector2 = this.mCam.ViewportToWorldPoint(vector2);
		if (trans != null)
		{
			vector2 = trans.InverseTransformPoint(vector2);
		}
		vector2.x = Mathf.Floor(vector2.x + 0.5f);
		vector2.y = Mathf.Floor(vector2.y + 0.5f);
		return vector2;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x000226DD File Offset: 0x000208DD
	protected virtual void OnEnable()
	{
		this.mUpdateFrame = -1;
		if (this.updateAnchors == UIRect.AnchorUpdate.OnEnable)
		{
			this.mAnchorsCached = false;
			this.mUpdateAnchors = true;
		}
		if (this.mStarted)
		{
			this.OnInit();
		}
		this.mUpdateFrame = -1;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00022711 File Offset: 0x00020911
	protected virtual void OnInit()
	{
		this.mChanged = true;
		this.mRootSet = false;
		this.mParentFound = false;
		if (this.parent != null)
		{
			this.mParent.mChildren.Add(this);
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00022747 File Offset: 0x00020947
	protected virtual void OnDisable()
	{
		if (this.mParent)
		{
			this.mParent.mChildren.Remove(this);
		}
		this.mParent = null;
		this.mRoot = null;
		this.mRootSet = false;
		this.mParentFound = false;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00022784 File Offset: 0x00020984
	protected virtual void Awake()
	{
		this.mStarted = false;
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000227A5 File Offset: 0x000209A5
	protected void Start()
	{
		this.mStarted = true;
		this.OnInit();
		this.OnStart();
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000227BC File Offset: 0x000209BC
	public void Update()
	{
		if (!this.mAnchorsCached)
		{
			this.ResetAnchors();
		}
		int frameCount = Time.frameCount;
		if (this.mUpdateFrame != frameCount)
		{
			if (this.updateAnchors == UIRect.AnchorUpdate.OnUpdate || this.mUpdateAnchors)
			{
				this.UpdateAnchorsInternal(frameCount);
			}
			this.OnUpdate();
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00022804 File Offset: 0x00020A04
	protected void UpdateAnchorsInternal(int frame)
	{
		this.mUpdateFrame = frame;
		this.mUpdateAnchors = false;
		bool flag = false;
		if (this.leftAnchor.target)
		{
			flag = true;
			if (this.leftAnchor.rect != null && this.leftAnchor.rect.mUpdateFrame != frame)
			{
				this.leftAnchor.rect.Update();
			}
		}
		if (this.bottomAnchor.target)
		{
			flag = true;
			if (this.bottomAnchor.rect != null && this.bottomAnchor.rect.mUpdateFrame != frame)
			{
				this.bottomAnchor.rect.Update();
			}
		}
		if (this.rightAnchor.target)
		{
			flag = true;
			if (this.rightAnchor.rect != null && this.rightAnchor.rect.mUpdateFrame != frame)
			{
				this.rightAnchor.rect.Update();
			}
		}
		if (this.topAnchor.target)
		{
			flag = true;
			if (this.topAnchor.rect != null && this.topAnchor.rect.mUpdateFrame != frame)
			{
				this.topAnchor.rect.Update();
			}
		}
		if (flag)
		{
			this.OnAnchor();
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00022952 File Offset: 0x00020B52
	public void UpdateAnchors()
	{
		if (this.isAnchored)
		{
			this.mUpdateFrame = -1;
			this.mUpdateAnchors = true;
			this.UpdateAnchorsInternal(Time.frameCount);
		}
	}

	// Token: 0x060004A8 RID: 1192
	protected abstract void OnAnchor();

	// Token: 0x060004A9 RID: 1193 RVA: 0x00022975 File Offset: 0x00020B75
	public void SetAnchor(Transform t)
	{
		this.leftAnchor.target = t;
		this.rightAnchor.target = t;
		this.topAnchor.target = t;
		this.bottomAnchor.target = t;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x000229B4 File Offset: 0x00020BB4
	public void SetAnchor(GameObject go)
	{
		Transform target = (go != null) ? go.transform : null;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00022A10 File Offset: 0x00020C10
	public void SetAnchor(GameObject go, int left, int bottom, int right, int top)
	{
		Transform target = (go != null) ? go.transform : null;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.leftAnchor.relative = 0f;
		this.rightAnchor.relative = 1f;
		this.bottomAnchor.relative = 0f;
		this.topAnchor.relative = 1f;
		this.leftAnchor.absolute = left;
		this.rightAnchor.absolute = right;
		this.bottomAnchor.absolute = bottom;
		this.topAnchor.absolute = top;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00022AE0 File Offset: 0x00020CE0
	public void SetAnchor(GameObject go, float left, float bottom, float right, float top)
	{
		Transform target = (go != null) ? go.transform : null;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.leftAnchor.relative = left;
		this.rightAnchor.relative = right;
		this.bottomAnchor.relative = bottom;
		this.topAnchor.relative = top;
		this.leftAnchor.absolute = 0;
		this.rightAnchor.absolute = 0;
		this.bottomAnchor.absolute = 0;
		this.topAnchor.absolute = 0;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00022BA0 File Offset: 0x00020DA0
	public void SetAnchor(GameObject go, float left, int leftOffset, float bottom, int bottomOffset, float right, int rightOffset, float top, int topOffset)
	{
		Transform target = (go != null) ? go.transform : null;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.leftAnchor.relative = left;
		this.rightAnchor.relative = right;
		this.bottomAnchor.relative = bottom;
		this.topAnchor.relative = top;
		this.leftAnchor.absolute = leftOffset;
		this.rightAnchor.absolute = rightOffset;
		this.bottomAnchor.absolute = bottomOffset;
		this.topAnchor.absolute = topOffset;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00022C64 File Offset: 0x00020E64
	public void SetAnchor(float left, int leftOffset, float bottom, int bottomOffset, float right, int rightOffset, float top, int topOffset)
	{
		Transform parent = this.cachedTransform.parent;
		this.leftAnchor.target = parent;
		this.rightAnchor.target = parent;
		this.topAnchor.target = parent;
		this.bottomAnchor.target = parent;
		this.leftAnchor.relative = left;
		this.rightAnchor.relative = right;
		this.bottomAnchor.relative = bottom;
		this.topAnchor.relative = top;
		this.leftAnchor.absolute = leftOffset;
		this.rightAnchor.absolute = rightOffset;
		this.bottomAnchor.absolute = bottomOffset;
		this.topAnchor.absolute = topOffset;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00022D20 File Offset: 0x00020F20
	public void SetScreenRect(int left, int top, int width, int height)
	{
		this.SetAnchor(0f, left, 1f, -top - height, 0f, left + width, 1f, -top);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00022D54 File Offset: 0x00020F54
	public void ResetAnchors()
	{
		this.mAnchorsCached = true;
		this.leftAnchor.rect = (this.leftAnchor.target ? this.leftAnchor.target.GetComponent<UIRect>() : null);
		this.bottomAnchor.rect = (this.bottomAnchor.target ? this.bottomAnchor.target.GetComponent<UIRect>() : null);
		this.rightAnchor.rect = (this.rightAnchor.target ? this.rightAnchor.target.GetComponent<UIRect>() : null);
		this.topAnchor.rect = (this.topAnchor.target ? this.topAnchor.target.GetComponent<UIRect>() : null);
		this.mCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
		this.FindCameraFor(this.leftAnchor);
		this.FindCameraFor(this.bottomAnchor);
		this.FindCameraFor(this.rightAnchor);
		this.FindCameraFor(this.topAnchor);
		this.mUpdateAnchors = true;
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00022E75 File Offset: 0x00021075
	public void ResetAndUpdateAnchors()
	{
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060004B2 RID: 1202
	public abstract void SetRect(float x, float y, float width, float height);

	// Token: 0x060004B3 RID: 1203 RVA: 0x00022E84 File Offset: 0x00021084
	private void FindCameraFor(UIRect.AnchorPoint ap)
	{
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
			return;
		}
		ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00022ED0 File Offset: 0x000210D0
	public virtual void ParentHasChanged()
	{
		this.mParentFound = false;
		UIRect y = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
		if (this.mParent != y)
		{
			if (this.mParent)
			{
				this.mParent.mChildren.Remove(this);
			}
			this.mParent = y;
			if (this.mParent)
			{
				this.mParent.mChildren.Add(this);
			}
			this.mRootSet = false;
		}
	}

	// Token: 0x060004B5 RID: 1205
	protected abstract void OnStart();

	// Token: 0x060004B6 RID: 1206 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x04000342 RID: 834
	public UIRect.AnchorPoint leftAnchor = new UIRect.AnchorPoint();

	// Token: 0x04000343 RID: 835
	public UIRect.AnchorPoint rightAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04000344 RID: 836
	public UIRect.AnchorPoint bottomAnchor = new UIRect.AnchorPoint();

	// Token: 0x04000345 RID: 837
	public UIRect.AnchorPoint topAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04000346 RID: 838
	public UIRect.AnchorUpdate updateAnchors = UIRect.AnchorUpdate.OnUpdate;

	// Token: 0x04000347 RID: 839
	[NonSerialized]
	protected GameObject mGo;

	// Token: 0x04000348 RID: 840
	[NonSerialized]
	protected Transform mTrans;

	// Token: 0x04000349 RID: 841
	[NonSerialized]
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();

	// Token: 0x0400034A RID: 842
	[NonSerialized]
	protected bool mChanged = true;

	// Token: 0x0400034B RID: 843
	[NonSerialized]
	protected bool mParentFound;

	// Token: 0x0400034C RID: 844
	[NonSerialized]
	private bool mUpdateAnchors = true;

	// Token: 0x0400034D RID: 845
	[NonSerialized]
	private int mUpdateFrame = -1;

	// Token: 0x0400034E RID: 846
	[NonSerialized]
	private bool mAnchorsCached;

	// Token: 0x0400034F RID: 847
	[NonSerialized]
	private UIRoot mRoot;

	// Token: 0x04000350 RID: 848
	[NonSerialized]
	private UIRect mParent;

	// Token: 0x04000351 RID: 849
	[NonSerialized]
	private bool mRootSet;

	// Token: 0x04000352 RID: 850
	[NonSerialized]
	protected Camera mCam;

	// Token: 0x04000353 RID: 851
	protected bool mStarted;

	// Token: 0x04000354 RID: 852
	[NonSerialized]
	public float finalAlpha = 1f;

	// Token: 0x04000355 RID: 853
	protected static Vector3[] mSides = new Vector3[4];

	// Token: 0x0200053F RID: 1343
	[Serializable]
	public class AnchorPoint
	{
		// Token: 0x060037BF RID: 14271 RVA: 0x00002594 File Offset: 0x00000794
		public AnchorPoint()
		{
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x0016B218 File Offset: 0x00169418
		public AnchorPoint(float relative)
		{
			this.relative = relative;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x0016B227 File Offset: 0x00169427
		public void Set(float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0016B242 File Offset: 0x00169442
		public void Set(Transform target, float relative, float absolute)
		{
			this.target = target;
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0016B264 File Offset: 0x00169464
		public void SetToNearest(float abs0, float abs1, float abs2)
		{
			this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x0016B280 File Offset: 0x00169480
		public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float num = Mathf.Abs(abs0);
			float num2 = Mathf.Abs(abs1);
			float num3 = Mathf.Abs(abs2);
			if (num < num2 && num < num3)
			{
				this.Set(rel0, abs0);
				return;
			}
			if (num2 < num && num2 < num3)
			{
				this.Set(rel1, abs1);
				return;
			}
			this.Set(rel2, abs2);
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0016B2D4 File Offset: 0x001694D4
		public void SetHorizontal(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
				return;
			}
			Vector3 vector = this.target.position;
			if (parent != null)
			{
				vector = parent.InverseTransformPoint(vector);
			}
			this.absolute = Mathf.FloorToInt(localPos - vector.x + 0.5f);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0016B36C File Offset: 0x0016956C
		public void SetVertical(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
				return;
			}
			Vector3 vector = this.target.position;
			if (parent != null)
			{
				vector = parent.InverseTransformPoint(vector);
			}
			this.absolute = Mathf.FloorToInt(localPos - vector.y + 0.5f);
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x0016B404 File Offset: 0x00169604
		public Vector3[] GetSides(Transform relativeTo)
		{
			if (this.target != null)
			{
				if (this.rect != null)
				{
					return this.rect.GetSides(relativeTo);
				}
				Camera component = this.target.GetComponent<Camera>();
				if (component != null)
				{
					return component.GetSides(relativeTo);
				}
			}
			return null;
		}

		// Token: 0x04002441 RID: 9281
		public Transform target;

		// Token: 0x04002442 RID: 9282
		public float relative;

		// Token: 0x04002443 RID: 9283
		public int absolute;

		// Token: 0x04002444 RID: 9284
		[NonSerialized]
		public UIRect rect;

		// Token: 0x04002445 RID: 9285
		[NonSerialized]
		public Camera targetCam;
	}

	// Token: 0x02000540 RID: 1344
	public enum AnchorUpdate
	{
		// Token: 0x04002447 RID: 9287
		OnEnable,
		// Token: 0x04002448 RID: 9288
		OnUpdate,
		// Token: 0x04002449 RID: 9289
		OnStart
	}
}
