using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class ThreadedJob
{
	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x000E2324 File Offset: 0x000E0524
	// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x000E2368 File Offset: 0x000E0568
	public bool IsDone
	{
		get
		{
			object handle = this.m_Handle;
			bool isDone;
			lock (handle)
			{
				isDone = this.m_IsDone;
			}
			return isDone;
		}
		set
		{
			object handle = this.m_Handle;
			lock (handle)
			{
				this.m_IsDone = value;
			}
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x000E23AC File Offset: 0x000E05AC
	// (set) Token: 0x06001FAA RID: 8106 RVA: 0x000E23B3 File Offset: 0x000E05B3
	public static int MaxNumberJobs
	{
		get
		{
			return ThreadedJob._MaxNumberJobs;
		}
		set
		{
			ThreadedJob._MaxNumberJobs = Mathf.Max(value, 1);
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000E23C1 File Offset: 0x000E05C1
	// (set) Token: 0x06001FAC RID: 8108 RVA: 0x000E23C9 File Offset: 0x000E05C9
	public bool isError { get; protected set; }

	// Token: 0x06001FAD RID: 8109 RVA: 0x000E23D2 File Offset: 0x000E05D2
	protected void SetError(Exception exception)
	{
		Debug.LogException(exception);
		this.SetError(exception.Message);
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000E23E6 File Offset: 0x000E05E6
	protected void SetError(string message)
	{
		this.isError = true;
		this.errorMessage = message;
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000E23F8 File Offset: 0x000E05F8
	public virtual void Start(bool threadPool = true, bool jumpQueue = false)
	{
		if (!threadPool)
		{
			this.m_Thread = new Thread(new ThreadStart(this.Run));
			this.m_Thread.Start();
			return;
		}
		if (ThreadedJob.runningJobs.Count < ThreadedJob.MaxNumberJobs)
		{
			this.RealStart();
			return;
		}
		if (jumpQueue)
		{
			ThreadedJob.queuedJobs.AddFirst(this);
			return;
		}
		ThreadedJob.queuedJobs.AddLast(this);
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000E2460 File Offset: 0x000E0660
	private async void RealStart()
	{
		ThreadedJob.runningJobs.Add(this);
		await Task.Run(new Action(this.Run));
		ThreadedJob.runningJobs.Remove(this);
		if (ThreadedJob.queuedJobs.Count > 0)
		{
			ThreadedJob value = ThreadedJob.queuedJobs.First.Value;
			ThreadedJob.queuedJobs.RemoveFirst();
			value.RealStart();
		}
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000E2499 File Offset: 0x000E0699
	public virtual void Abort()
	{
		this.Reset();
		this.m_Thread.Abort();
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void Reset()
	{
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void ThreadFunction()
	{
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x000E24AC File Offset: 0x000E06AC
	// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x000E24B4 File Offset: 0x000E06B4
	public bool isFinished { get; private set; }

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x000E24BD File Offset: 0x000E06BD
	// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x000E24C5 File Offset: 0x000E06C5
	public float finishTime { get; private set; }

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnFinished()
	{
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000E24CE File Offset: 0x000E06CE
	public virtual bool Update()
	{
		if (this.IsDone)
		{
			if (!this.isFinished)
			{
				this.isFinished = true;
				this.finishTime = Time.time;
				this.OnFinished();
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000E24FB File Offset: 0x000E06FB
	private void Run()
	{
		this.ThreadFunction();
		this.IsDone = true;
	}

	// Token: 0x0400136B RID: 4971
	private bool m_IsDone;

	// Token: 0x0400136C RID: 4972
	private object m_Handle = new object();

	// Token: 0x0400136D RID: 4973
	private Thread m_Thread;

	// Token: 0x0400136E RID: 4974
	private static int _MaxNumberJobs = Mathf.Max(SystemInfo.processorCount - 1, 1);

	// Token: 0x0400136F RID: 4975
	private static LinkedList<ThreadedJob> queuedJobs = new LinkedList<ThreadedJob>();

	// Token: 0x04001370 RID: 4976
	private static List<ThreadedJob> runningJobs = new List<ThreadedJob>(ThreadedJob.MaxNumberJobs);

	// Token: 0x04001372 RID: 4978
	public string errorMessage;
}
