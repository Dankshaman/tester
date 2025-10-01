using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
	// Token: 0x06000157 RID: 343 RVA: 0x0000917C File Offset: 0x0000737C
	private void OnEnable()
	{
		this.mTrans = base.transform;
		if (this.scrollView == null && this.draggablePanel != null)
		{
			this.scrollView = this.draggablePanel;
			this.draggablePanel = null;
		}
		if (this.mStarted && (this.mAutoFind || this.mScroll == null))
		{
			this.FindScrollView();
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000091E8 File Offset: 0x000073E8
	private void Start()
	{
		this.mStarted = true;
		this.FindScrollView();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x000091F8 File Offset: 0x000073F8
	private void FindScrollView()
	{
		UIScrollView uiscrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
		if (this.scrollView == null || (this.mAutoFind && uiscrollView != this.scrollView))
		{
			this.scrollView = uiscrollView;
			this.mAutoFind = true;
		}
		else if (this.scrollView == uiscrollView)
		{
			this.mAutoFind = true;
		}
		this.mScroll = this.scrollView;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00009266 File Offset: 0x00007466
	private void OnDisable()
	{
		if (this.mPressed && this.mScroll != null && this.mScroll.GetComponentInChildren<UIWrapContent>() == null)
		{
			this.mScroll.Press(false);
			this.mScroll = null;
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000092A4 File Offset: 0x000074A4
	private void OnPress(bool pressed)
	{
		this.mPressed = pressed;
		if (this.mAutoFind && this.mScroll != this.scrollView)
		{
			this.mScroll = this.scrollView;
			this.mAutoFind = false;
		}
		if (this.scrollView && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.scrollView.Press(pressed);
			if (!pressed && this.mAutoFind)
			{
				this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
				this.mScroll = this.scrollView;
			}
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000933C File Offset: 0x0000753C
	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this) && this.bMouseDrag)
		{
			this.scrollView.Drag();
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00009368 File Offset: 0x00007568
	private void OnScroll(float delta)
	{
		if (zInput.GetButton("Alt", ControlType.All))
		{
			return;
		}
		if (this.scrollView == null)
		{
			this.FindScrollView();
		}
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x000093B8 File Offset: 0x000075B8
	public void OnPan(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.OnPan(delta);
		}
	}

	// Token: 0x04000126 RID: 294
	public UIScrollView scrollView;

	// Token: 0x04000127 RID: 295
	public bool bMouseDrag;

	// Token: 0x04000128 RID: 296
	[HideInInspector]
	[SerializeField]
	private UIScrollView draggablePanel;

	// Token: 0x04000129 RID: 297
	private Transform mTrans;

	// Token: 0x0400012A RID: 298
	private UIScrollView mScroll;

	// Token: 0x0400012B RID: 299
	private bool mAutoFind;

	// Token: 0x0400012C RID: 300
	private bool mStarted;

	// Token: 0x0400012D RID: 301
	[NonSerialized]
	private bool mPressed;
}
