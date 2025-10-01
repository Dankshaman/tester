using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000033 RID: 51
[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600010F RID: 271 RVA: 0x00006E62 File Offset: 0x00005062
	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00006E6A File Offset: 0x0000506A
	private void Start()
	{
		this.Recenter();
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00006E72 File Offset: 0x00005072
	private void OnEnable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = this;
			this.Recenter();
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00006E93 File Offset: 0x00005093
	private void OnDisable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = null;
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00006EAE File Offset: 0x000050AE
	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00006EBE File Offset: 0x000050BE
	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00006ED4 File Offset: 0x000050D4
	[ContextMenu("Execute")]
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.enabled = false;
				return;
			}
			if (this.mScrollView)
			{
				this.mScrollView.centerOnChild = this;
				UIScrollView uiscrollView = this.mScrollView;
				uiscrollView.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(uiscrollView.onDragFinished, new UIScrollView.OnDragNotification(this.OnDragFinished));
			}
			if (this.mScrollView.horizontalScrollBar != null)
			{
				UIProgressBar horizontalScrollBar = this.mScrollView.horizontalScrollBar;
				horizontalScrollBar.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(horizontalScrollBar.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				UIProgressBar verticalScrollBar = this.mScrollView.verticalScrollBar;
				verticalScrollBar.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(verticalScrollBar.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 a = NGUIMath.SpringDampen(ref vector2, 9f, 2f);
		Vector3 b = vector - a * 0.01f;
		float num = float.MaxValue;
		Transform target = null;
		int index = 0;
		int num2 = 0;
		UIGrid component = base.GetComponent<UIGrid>();
		List<Transform> list = null;
		if (component != null)
		{
			list = component.GetChildList();
			int i = 0;
			int count = list.Count;
			int num3 = 0;
			while (i < count)
			{
				Transform transform2 = list[i];
				if (transform2.gameObject.activeInHierarchy)
				{
					float num4 = Vector3.SqrMagnitude(transform2.position - b);
					if (num4 < num)
					{
						num = num4;
						target = transform2;
						index = i;
						num2 = num3;
					}
					num3++;
				}
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount = transform.childCount;
			int num5 = 0;
			while (j < childCount)
			{
				Transform child = transform.GetChild(j);
				if (child.gameObject.activeInHierarchy)
				{
					float num6 = Vector3.SqrMagnitude(child.position - b);
					if (num6 < num)
					{
						num = num6;
						target = child;
						index = j;
						num2 = num5;
					}
					num5++;
				}
				j++;
			}
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == ((list != null) ? list[index] : transform.GetChild(index)))
		{
			Vector3 vector3 = UICamera.currentTouch.totalDelta;
			vector3 = base.transform.rotation * vector3;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num7;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num7 = vector3.magnitude;
				}
				else
				{
					num7 = -vector3.y;
				}
			}
			else
			{
				num7 = vector3.x;
			}
			if (Mathf.Abs(num7) > this.nextPageThreshold)
			{
				if (num7 > this.nextPageThreshold)
				{
					if (list != null)
					{
						if (num2 > 0)
						{
							target = list[num2 - 1];
						}
						else
						{
							target = ((base.GetComponent<UIWrapContent>() == null) ? list[0] : list[list.Count - 1]);
						}
					}
					else if (num2 > 0)
					{
						target = transform.GetChild(num2 - 1);
					}
					else
					{
						target = ((base.GetComponent<UIWrapContent>() == null) ? transform.GetChild(0) : transform.GetChild(transform.childCount - 1));
					}
				}
				else if (num7 < -this.nextPageThreshold)
				{
					if (list != null)
					{
						if (num2 < list.Count - 1)
						{
							target = list[num2 + 1];
						}
						else
						{
							target = ((base.GetComponent<UIWrapContent>() == null) ? list[list.Count - 1] : list[0]);
						}
					}
					else if (num2 < transform.childCount - 1)
					{
						target = transform.GetChild(num2 + 1);
					}
					else
					{
						target = ((base.GetComponent<UIWrapContent>() == null) ? transform.GetChild(transform.childCount - 1) : transform.GetChild(0));
					}
				}
			}
		}
		this.CenterOn(target, vector);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000073AC File Offset: 0x000055AC
	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 a = cachedTransform.InverseTransformPoint(target.position);
			Vector3 b = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 b2 = a - b;
			if (!this.mScrollView.canMoveHorizontally)
			{
				b2.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				b2.y = 0f;
			}
			b2.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - b2, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000074B8 File Offset: 0x000056B8
	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}

	// Token: 0x040000DE RID: 222
	public float springStrength = 8f;

	// Token: 0x040000DF RID: 223
	public float nextPageThreshold;

	// Token: 0x040000E0 RID: 224
	public SpringPanel.OnFinished onFinished;

	// Token: 0x040000E1 RID: 225
	public UICenterOnChild.OnCenterCallback onCenter;

	// Token: 0x040000E2 RID: 226
	private UIScrollView mScrollView;

	// Token: 0x040000E3 RID: 227
	private GameObject mCenteredObject;

	// Token: 0x02000504 RID: 1284
	// (Invoke) Token: 0x06003741 RID: 14145
	public delegate void OnCenterCallback(GameObject centeredObject);
}
