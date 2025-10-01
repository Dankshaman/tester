using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x0200007F RID: 127
public abstract class UITweener : MonoBehaviour
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600059F RID: 1439 RVA: 0x00026AE8 File Offset: 0x00024CE8
	public float amountPerDelta
	{
		get
		{
			if (this.duration == 0f)
			{
				return 1000f;
			}
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs(1f / this.duration) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00026B4B File Offset: 0x00024D4B
	// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00026B53 File Offset: 0x00024D53
	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00026B61 File Offset: 0x00024D61
	public Direction direction
	{
		get
		{
			if (this.amountPerDelta >= 0f)
			{
				return Direction.Forward;
			}
			return Direction.Reverse;
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00026B73 File Offset: 0x00024D73
	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00026B89 File Offset: 0x00024D89
	protected virtual void Start()
	{
		this.DoUpdate();
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00026B91 File Offset: 0x00024D91
	protected void Update()
	{
		if (!this.useFixedUpdate)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00026BA1 File Offset: 0x00024DA1
	protected void FixedUpdate()
	{
		if (this.useFixedUpdate)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00026BB4 File Offset: 0x00024DB4
	protected void DoUpdate()
	{
		float num = (this.ignoreTimeScale && !this.useFixedUpdate) ? Time.unscaledDeltaTime : Time.deltaTime;
		float num2 = (this.ignoreTimeScale && !this.useFixedUpdate) ? Time.unscaledTime : Time.time;
		if (!this.mStarted)
		{
			num = 0f;
			this.mStarted = true;
			this.mStartTime = num2 + this.delay;
		}
		if (num2 < this.mStartTime)
		{
			return;
		}
		this.mFactor += ((this.duration == 0f) ? 1f : (this.amountPerDelta * num));
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			base.enabled = false;
			if (UITweener.current != this)
			{
				UITweener uitweener = UITweener.current;
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate eventDelegate = this.mTemp[i];
						if (eventDelegate != null && !eventDelegate.oneShot)
						{
							EventDelegate.Add(this.onFinished, eventDelegate, eventDelegate.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				UITweener.current = uitweener;
				return;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00026E37 File Offset: 0x00025037
	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00026E46 File Offset: 0x00025046
	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00026E54 File Offset: 0x00025054
	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00026E63 File Offset: 0x00025063
	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00026E71 File Offset: 0x00025071
	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00026E9D File Offset: 0x0002509D
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00026EA8 File Offset: 0x000250A8
	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.5707964f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.5707964f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.2831855f) / 6.2831855f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		this.OnUpdate((this.animationCurve != null) ? this.animationCurve.Evaluate(num) : num, isFinished);
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00026FDC File Offset: 0x000251DC
	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00027061 File Offset: 0x00025261
	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00027061 File Offset: 0x00025261
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0002706A File Offset: 0x0002526A
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00027073 File Offset: 0x00025273
	public virtual void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		if (!base.enabled)
		{
			base.enabled = true;
			this.mStarted = false;
		}
		this.DoUpdate();
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x000270B2 File Offset: 0x000252B2
	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = ((this.amountPerDelta < 0f) ? 1f : 0f);
		this.Sample(this.mFactor, false);
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x000270E7 File Offset: 0x000252E7
	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	// Token: 0x060005B6 RID: 1462
	protected abstract void OnUpdate(float factor, bool isFinished);

	// Token: 0x060005B7 RID: 1463 RVA: 0x00027120 File Offset: 0x00025320
	public static T Begin<T>(GameObject go, float duration, float delay = 0f) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t != null && t.tweenGroup != 0)
		{
			t = default(!!0);
			T[] components = go.GetComponents<T>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				t = components[i];
				if (t != null && t.tweenGroup == 0)
				{
					break;
				}
				t = default(!!0);
				i++;
			}
		}
		if (t == null)
		{
			t = go.AddComponent<T>();
			if (t == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to add ",
					typeof(!!0),
					" to ",
					NGUITools.GetHierarchy(go)
				}), go);
				return default(!!0);
			}
		}
		t.mStarted = false;
		t.mFactor = 0f;
		t.duration = duration;
		t.mDuration = duration;
		t.delay = delay;
		t.mAmountPerDelta = ((duration > 0f) ? Mathf.Abs(1f / duration) : 1000f);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.onFinished.Clear();
		if (t.mTemp != null)
		{
			t.mTemp.Clear();
		}
		t.enabled = true;
		return t;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void SetStartToCurrentValue()
	{
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void SetEndToCurrentValue()
	{
	}

	// Token: 0x040003D3 RID: 979
	public static UITweener current;

	// Token: 0x040003D4 RID: 980
	[HideInInspector]
	public UITweener.Method method;

	// Token: 0x040003D5 RID: 981
	[HideInInspector]
	public UITweener.Style style;

	// Token: 0x040003D6 RID: 982
	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x040003D7 RID: 983
	[HideInInspector]
	public bool ignoreTimeScale = true;

	// Token: 0x040003D8 RID: 984
	[HideInInspector]
	public float delay;

	// Token: 0x040003D9 RID: 985
	[HideInInspector]
	public float duration = 1f;

	// Token: 0x040003DA RID: 986
	[HideInInspector]
	public bool steeperCurves;

	// Token: 0x040003DB RID: 987
	[HideInInspector]
	public int tweenGroup;

	// Token: 0x040003DC RID: 988
	[Tooltip("By default, Update() will be used for tweening. Setting this to 'true' will make the tween happen in FixedUpdate() insted.")]
	public bool useFixedUpdate;

	// Token: 0x040003DD RID: 989
	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x040003DE RID: 990
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x040003DF RID: 991
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x040003E0 RID: 992
	private bool mStarted;

	// Token: 0x040003E1 RID: 993
	private float mStartTime;

	// Token: 0x040003E2 RID: 994
	private float mDuration;

	// Token: 0x040003E3 RID: 995
	private float mAmountPerDelta = 1000f;

	// Token: 0x040003E4 RID: 996
	private float mFactor;

	// Token: 0x040003E5 RID: 997
	private List<EventDelegate> mTemp;

	// Token: 0x0200054A RID: 1354
	public enum Method
	{
		// Token: 0x04002469 RID: 9321
		Linear,
		// Token: 0x0400246A RID: 9322
		EaseIn,
		// Token: 0x0400246B RID: 9323
		EaseOut,
		// Token: 0x0400246C RID: 9324
		EaseInOut,
		// Token: 0x0400246D RID: 9325
		BounceIn,
		// Token: 0x0400246E RID: 9326
		BounceOut
	}

	// Token: 0x0200054B RID: 1355
	public enum Style
	{
		// Token: 0x04002470 RID: 9328
		Once,
		// Token: 0x04002471 RID: 9329
		Loop,
		// Token: 0x04002472 RID: 9330
		PingPong
	}
}
