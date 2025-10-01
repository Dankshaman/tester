using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	// Token: 0x06000426 RID: 1062 RVA: 0x0001DF95 File Offset: 0x0001C195
	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001DFBB File Offset: 0x0001C1BB
	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		bool flag = false;
		Vector3 localPosition = this.mTrans.localPosition;
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
		if ((vector - this.target).sqrMagnitude < 0.01f)
		{
			vector = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector;
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001E0C0 File Offset: 0x0001C2C0
	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.enabled = true;
		return springPanel;
	}

	// Token: 0x040002E7 RID: 743
	public static SpringPanel current;

	// Token: 0x040002E8 RID: 744
	public Vector3 target = Vector3.zero;

	// Token: 0x040002E9 RID: 745
	public float strength = 10f;

	// Token: 0x040002EA RID: 746
	public SpringPanel.OnFinished onFinished;

	// Token: 0x040002EB RID: 747
	private UIPanel mPanel;

	// Token: 0x040002EC RID: 748
	private Transform mTrans;

	// Token: 0x040002ED RID: 749
	private UIScrollView mDrag;

	// Token: 0x0200052F RID: 1327
	// (Invoke) Token: 0x06003798 RID: 14232
	public delegate void OnFinished();
}
