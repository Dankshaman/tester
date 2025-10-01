using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x0200005C RID: 92
[Serializable]
public class EventDelegate
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000300 RID: 768 RVA: 0x000138E8 File Offset: 0x00011AE8
	// (set) Token: 0x06000301 RID: 769 RVA: 0x000138F0 File Offset: 0x00011AF0
	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mParameters = null;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000302 RID: 770 RVA: 0x00013923 File Offset: 0x00011B23
	// (set) Token: 0x06000303 RID: 771 RVA: 0x0001392B File Offset: 0x00011B2B
	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mParameters = null;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000304 RID: 772 RVA: 0x0001395E File Offset: 0x00011B5E
	public EventDelegate.Parameter[] parameters
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return this.mParameters;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000305 RID: 773 RVA: 0x00013974 File Offset: 0x00011B74
	public bool isValid
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName));
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000306 RID: 774 RVA: 0x000139B4 File Offset: 0x00011BB4
	public bool isEnabled
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRawDelegate && this.mCachedCallback != null)
			{
				return true;
			}
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00002594 File Offset: 0x00000794
	public EventDelegate()
	{
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00013A09 File Offset: 0x00011C09
	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00013A18 File Offset: 0x00011C18
	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00013A28 File Offset: 0x00011C28
	private static string GetMethodName(EventDelegate.Callback callback)
	{
		return callback.Method.Name;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00013A35 File Offset: 0x00011C35
	private static bool IsValid(EventDelegate.Callback callback)
	{
		return callback != null && callback.Method != null;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00013A48 File Offset: 0x00011C48
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is EventDelegate.Callback)
		{
			EventDelegate.Callback callback = obj as EventDelegate.Callback;
			if (callback.Equals(this.mCachedCallback))
			{
				return true;
			}
			MonoBehaviour y = callback.Target as MonoBehaviour;
			return this.mTarget == y && string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback));
		}
		else
		{
			if (obj is EventDelegate)
			{
				EventDelegate eventDelegate = obj as EventDelegate;
				return this.mTarget == eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
			}
			return false;
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00013AE6 File Offset: 0x00011CE6
	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00013AF0 File Offset: 0x00011CF0
	private void Set(EventDelegate.Callback call)
	{
		this.Clear();
		if (call != null && EventDelegate.IsValid(call))
		{
			this.mTarget = (call.Target as MonoBehaviour);
			if (this.mTarget == null)
			{
				this.mRawDelegate = true;
				this.mCachedCallback = call;
				this.mMethodName = null;
				return;
			}
			this.mMethodName = EventDelegate.GetMethodName(call);
			this.mRawDelegate = false;
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00013B56 File Offset: 0x00011D56
	public void Set(MonoBehaviour target, string methodName)
	{
		this.Clear();
		this.mTarget = target;
		this.mMethodName = methodName;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00013B6C File Offset: 0x00011D6C
	private void Cache()
	{
		this.mCached = true;
		if (this.mRawDelegate)
		{
			return;
		}
		if ((this.mCachedCallback == null || this.mCachedCallback.Target as MonoBehaviour != this.mTarget || EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) && this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName))
		{
			Type type = this.mTarget.GetType();
			this.mMethod = null;
			while (type != null)
			{
				try
				{
					this.mMethod = type.GetMethod(this.mMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (this.mMethod != null)
					{
						break;
					}
				}
				catch (Exception)
				{
				}
				type = type.BaseType;
			}
			if (this.mMethod == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Could not find method '",
					this.mMethodName,
					"' on ",
					this.mTarget.GetType()
				}), this.mTarget);
				return;
			}
			if (this.mMethod.ReturnType != typeof(void))
			{
				Debug.LogError(string.Concat(new object[]
				{
					this.mTarget.GetType(),
					".",
					this.mMethodName,
					" must have a 'void' return type."
				}), this.mTarget);
				return;
			}
			this.mParameterInfos = this.mMethod.GetParameters();
			if (this.mParameterInfos.Length == 0)
			{
				this.mCachedCallback = (EventDelegate.Callback)Delegate.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget, this.mMethodName);
				this.mArgs = null;
				this.mParameters = null;
				return;
			}
			this.mCachedCallback = null;
			if (this.mParameters == null || this.mParameters.Length != this.mParameterInfos.Length)
			{
				this.mParameters = new EventDelegate.Parameter[this.mParameterInfos.Length];
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mParameters[i] = new EventDelegate.Parameter();
					i++;
				}
			}
			int j = 0;
			int num2 = this.mParameters.Length;
			while (j < num2)
			{
				this.mParameters[j].expectedType = this.mParameterInfos[j].ParameterType;
				j++;
			}
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00013DC8 File Offset: 0x00011FC8
	public bool Execute()
	{
		if (!this.mCached)
		{
			this.Cache();
		}
		if (this.mCachedCallback != null)
		{
			this.mCachedCallback();
			return true;
		}
		if (this.mMethod != null)
		{
			if (this.mParameters == null || this.mParameters.Length == 0)
			{
				this.mMethod.Invoke(this.mTarget, null);
			}
			else
			{
				if (this.mArgs == null || this.mArgs.Length != this.mParameters.Length)
				{
					this.mArgs = new object[this.mParameters.Length];
				}
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mArgs[i] = this.mParameters[i].value;
					i++;
				}
				try
				{
					this.mMethod.Invoke(this.mTarget, this.mArgs);
				}
				catch (ArgumentException ex)
				{
					string text = "Error calling ";
					if (this.mTarget == null)
					{
						text += this.mMethod.Name;
					}
					else
					{
						text = string.Concat(new object[]
						{
							text,
							this.mTarget.GetType(),
							".",
							this.mMethod.Name
						});
					}
					text = text + ": " + ex.Message;
					text += "\n  Expected: ";
					if (this.mParameterInfos.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += this.mParameterInfos[0];
						for (int j = 1; j < this.mParameterInfos.Length; j++)
						{
							text = text + ", " + this.mParameterInfos[j].ParameterType;
						}
					}
					text += "\n  Received: ";
					if (this.mParameters.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += this.mParameters[0].type;
						for (int k = 1; k < this.mParameters.Length; k++)
						{
							text = text + ", " + this.mParameters[k].type;
						}
					}
					text += "\n";
					Debug.LogError(text);
				}
				int l = 0;
				int num2 = this.mArgs.Length;
				while (l < num2)
				{
					if (this.mParameterInfos[l].IsIn || this.mParameterInfos[l].IsOut)
					{
						this.mParameters[l].value = this.mArgs[l];
					}
					this.mArgs[l] = null;
					l++;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00014074 File Offset: 0x00012274
	public void Clear()
	{
		this.mTarget = null;
		this.mMethodName = null;
		this.mRawDelegate = false;
		this.mCachedCallback = null;
		this.mParameters = null;
		this.mCached = false;
		this.mMethod = null;
		this.mParameterInfos = null;
		this.mArgs = null;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x000140C0 File Offset: 0x000122C0
	public override string ToString()
	{
		if (this.mTarget != null)
		{
			string text = this.mTarget.GetType().ToString();
			int num = text.LastIndexOf('.');
			if (num > 0)
			{
				text = text.Substring(num + 1);
			}
			if (!string.IsNullOrEmpty(this.methodName))
			{
				return text + "/" + this.methodName;
			}
			return text + "/[delegate]";
		}
		else
		{
			if (!this.mRawDelegate)
			{
				return null;
			}
			return "[delegate]";
		}
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00014140 File Offset: 0x00012340
	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null)
				{
					try
					{
						eventDelegate.Execute();
					}
					catch (Exception ex)
					{
						if (ex.InnerException != null)
						{
							Debug.LogException(ex.InnerException);
						}
						else
						{
							Debug.LogException(ex);
						}
					}
					if (i >= list.Count)
					{
						break;
					}
					if (list[i] != eventDelegate)
					{
						continue;
					}
					if (eventDelegate.oneShot)
					{
						list.RemoveAt(i);
						continue;
					}
				}
			}
		}
	}

	// Token: 0x06000315 RID: 789 RVA: 0x000141C8 File Offset: 0x000123C8
	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.isValid)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00014204 File Offset: 0x00012404
	public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			EventDelegate eventDelegate = new EventDelegate(callback);
			list.Clear();
			list.Add(eventDelegate);
			return eventDelegate;
		}
		return null;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0001422B File Offset: 0x0001242B
	public static void Set(List<EventDelegate> list, EventDelegate del)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(del);
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0001423D File Offset: 0x0001243D
	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		return EventDelegate.Add(list, callback, false);
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00014248 File Offset: 0x00012448
	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					return eventDelegate;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(callback);
			eventDelegate2.oneShot = oneShot;
			list.Add(eventDelegate2);
			return eventDelegate2;
		}
		Debug.LogWarning("Attempting to add a callback to a list that's null");
		return null;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x000142A3 File Offset: 0x000124A3
	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, ev.oneShot);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000142B4 File Offset: 0x000124B4
	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
		{
			EventDelegate.Add(list, ev.mCachedCallback, oneShot);
			return;
		}
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					return;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(ev.target, ev.methodName);
			eventDelegate2.oneShot = oneShot;
			if (ev.mParameters != null && ev.mParameters.Length != 0)
			{
				eventDelegate2.mParameters = new EventDelegate.Parameter[ev.mParameters.Length];
				for (int j = 0; j < ev.mParameters.Length; j++)
				{
					eventDelegate2.mParameters[j] = ev.mParameters[j];
				}
			}
			list.Add(eventDelegate2);
			return;
		}
		Debug.LogWarning("Attempting to add a callback to a list that's null");
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0001439C File Offset: 0x0001259C
	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x000143E0 File Offset: 0x000125E0
	public static bool Remove(List<EventDelegate> list, EventDelegate ev)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04000291 RID: 657
	[SerializeField]
	private MonoBehaviour mTarget;

	// Token: 0x04000292 RID: 658
	[SerializeField]
	private string mMethodName;

	// Token: 0x04000293 RID: 659
	[SerializeField]
	private EventDelegate.Parameter[] mParameters;

	// Token: 0x04000294 RID: 660
	public bool oneShot;

	// Token: 0x04000295 RID: 661
	[NonSerialized]
	private EventDelegate.Callback mCachedCallback;

	// Token: 0x04000296 RID: 662
	[NonSerialized]
	private bool mRawDelegate;

	// Token: 0x04000297 RID: 663
	[NonSerialized]
	private bool mCached;

	// Token: 0x04000298 RID: 664
	[NonSerialized]
	private MethodInfo mMethod;

	// Token: 0x04000299 RID: 665
	[NonSerialized]
	private ParameterInfo[] mParameterInfos;

	// Token: 0x0400029A RID: 666
	[NonSerialized]
	private object[] mArgs;

	// Token: 0x0400029B RID: 667
	private static int s_Hash = "EventDelegate".GetHashCode();

	// Token: 0x02000525 RID: 1317
	[Serializable]
	public class Parameter
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x0016B067 File Offset: 0x00169267
		public Parameter()
		{
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0016B07F File Offset: 0x0016927F
		public Parameter(UnityEngine.Object obj, string field)
		{
			this.obj = obj;
			this.field = field;
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0016B0A5 File Offset: 0x001692A5
		public Parameter(object val)
		{
			this.mValue = val;
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x0016B0C4 File Offset: 0x001692C4
		// (set) Token: 0x06003784 RID: 14212 RVA: 0x0016B1D5 File Offset: 0x001693D5
		public object value
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue;
				}
				if (!this.cached)
				{
					this.cached = true;
					this.fieldInfo = null;
					this.propInfo = null;
					if (this.obj != null && !string.IsNullOrEmpty(this.field))
					{
						Type type = this.obj.GetType();
						this.propInfo = type.GetProperty(this.field);
						if (this.propInfo == null)
						{
							this.fieldInfo = type.GetField(this.field);
						}
					}
				}
				if (this.propInfo != null)
				{
					return this.propInfo.GetValue(this.obj, null);
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.GetValue(this.obj);
				}
				if (this.obj != null)
				{
					return this.obj;
				}
				if (this.expectedType != null && this.expectedType.IsValueType)
				{
					return null;
				}
				return Convert.ChangeType(null, this.expectedType);
			}
			set
			{
				this.mValue = value;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x0016B1DE File Offset: 0x001693DE
		public Type type
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue.GetType();
				}
				if (this.obj == null)
				{
					return typeof(void);
				}
				return this.obj.GetType();
			}
		}

		// Token: 0x04002401 RID: 9217
		public UnityEngine.Object obj;

		// Token: 0x04002402 RID: 9218
		public string field;

		// Token: 0x04002403 RID: 9219
		[NonSerialized]
		private object mValue;

		// Token: 0x04002404 RID: 9220
		[NonSerialized]
		public Type expectedType = typeof(void);

		// Token: 0x04002405 RID: 9221
		[NonSerialized]
		public bool cached;

		// Token: 0x04002406 RID: 9222
		[NonSerialized]
		public PropertyInfo propInfo;

		// Token: 0x04002407 RID: 9223
		[NonSerialized]
		public FieldInfo fieldInfo;
	}

	// Token: 0x02000526 RID: 1318
	// (Invoke) Token: 0x06003787 RID: 14215
	public delegate void Callback();
}
