using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	// Token: 0x0600050E RID: 1294 RVA: 0x00024EE2 File Offset: 0x000230E2
	private void Start()
	{
		this.mTrans = base.transform;
		if (this.updateScrollView)
		{
			this.mSv = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00024F09 File Offset: 0x00023109
	private void OnEnable()
	{
		this.mThreshold = 0f;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00024F18 File Offset: 0x00023118
	private void Update()
	{
		float deltaTime = this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).sqrMagnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.position).sqrMagnitude)
			{
				this.mTrans.position = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).sqrMagnitude * 1E-05f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).sqrMagnitude)
			{
				this.mTrans.localPosition = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		if (this.mSv != null)
		{
			this.mSv.UpdateScrollbars(true);
		}
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000250B0 File Offset: 0x000232B0
	private void NotifyListeners()
	{
		SpringPosition.current = this;
		if (this.onFinished != null)
		{
			this.onFinished();
		}
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
		}
		SpringPosition.current = null;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0002510C File Offset: 0x0002330C
	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.enabled = true;
		}
		return springPosition;
	}

	// Token: 0x04000385 RID: 901
	public static SpringPosition current;

	// Token: 0x04000386 RID: 902
	public Vector3 target = Vector3.zero;

	// Token: 0x04000387 RID: 903
	public float strength = 10f;

	// Token: 0x04000388 RID: 904
	public bool worldSpace;

	// Token: 0x04000389 RID: 905
	public bool ignoreTimeScale;

	// Token: 0x0400038A RID: 906
	public bool updateScrollView;

	// Token: 0x0400038B RID: 907
	public SpringPosition.OnFinished onFinished;

	// Token: 0x0400038C RID: 908
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x0400038D RID: 909
	[SerializeField]
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x0400038E RID: 910
	private Transform mTrans;

	// Token: 0x0400038F RID: 911
	private float mThreshold;

	// Token: 0x04000390 RID: 912
	private UIScrollView mSv;

	// Token: 0x02000546 RID: 1350
	// (Invoke) Token: 0x060037D5 RID: 14293
	public delegate void OnFinished();
}
