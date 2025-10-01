using System;
using NewNet;

// Token: 0x020000B8 RID: 184
public class ClockScript : NetworkBehavior
{
	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x00041D9B File Offset: 0x0003FF9B
	// (set) Token: 0x0600092C RID: 2348 RVA: 0x00041DA3 File Offset: 0x0003FFA3
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int Hours
	{
		get
		{
			return this._Hours;
		}
		set
		{
			if (value != this._Hours)
			{
				this._Hours = value;
				base.DirtySync("Hours");
			}
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x00041DC0 File Offset: 0x0003FFC0
	// (set) Token: 0x0600092E RID: 2350 RVA: 0x00041DC8 File Offset: 0x0003FFC8
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int Minutes
	{
		get
		{
			return this._Minutes;
		}
		set
		{
			if (value != this._Minutes)
			{
				this._Minutes = value;
				base.DirtySync("Minutes");
			}
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x00041DE5 File Offset: 0x0003FFE5
	// (set) Token: 0x06000930 RID: 2352 RVA: 0x00041DED File Offset: 0x0003FFED
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int Seconds
	{
		get
		{
			return this._Seconds;
		}
		set
		{
			if (value != this._Seconds)
			{
				this._Seconds = value;
				base.DirtySync("Seconds");
			}
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00041E0A File Offset: 0x0004000A
	public override void OnSync()
	{
		this.SetClockTime(this.Hours, this.Minutes, this.Seconds);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00041E24 File Offset: 0x00040024
	private void Awake()
	{
		this.Label = NGUITools.GetChildLabel(base.transform.GetChild(0).gameObject);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00041E42 File Offset: 0x00040042
	private void Start()
	{
		if (Network.isServer)
		{
			base.InvokeRepeating("UpdateClock", 0f, 0.06f);
		}
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00041E60 File Offset: 0x00040060
	private void UpdateClock()
	{
		this.DateTimeNow = DateTime.Now;
		if (this.currentClockMode == ClockScript.ClockMode.CurrentTime)
		{
			if (this.bPaused)
			{
				return;
			}
			this.SetClockTime(this.DateTimeNow.Hour, this.DateTimeNow.Minute, this.DateTimeNow.Second);
			return;
		}
		else if (this.currentClockMode == ClockScript.ClockMode.Timer)
		{
			if (this.bPaused)
			{
				this.StartTimer(this.PausedTime, true);
			}
			TimeSpan timeSpan = this.TimerDate - this.DateTimeNow;
			if (timeSpan.TotalSeconds < 1.0)
			{
				base.GetComponent<SoundScript>().TimerSound();
				this.currentClockMode = ClockScript.ClockMode.Zero;
				return;
			}
			this.SetClockTime(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			return;
		}
		else
		{
			if (this.currentClockMode == ClockScript.ClockMode.Stopwatch)
			{
				if (this.bPaused)
				{
					this.StopwatchDate = DateTime.Now.AddSeconds((double)(-(double)this.PausedTime));
				}
				TimeSpan timeSpan2 = this.DateTimeNow - this.StopwatchDate;
				this.SetClockTime(timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
				return;
			}
			if (this.currentClockMode == ClockScript.ClockMode.Zero)
			{
				this.SetClockTime(0, 0, 0);
			}
			return;
		}
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00041F90 File Offset: 0x00040190
	private void SetClockTime(int hours, int minutes, int seconds)
	{
		this.Hours = hours;
		this.Minutes = minutes;
		this.Seconds = seconds;
		this.Label.text = string.Concat(new string[]
		{
			hours.ToString("00"),
			":",
			minutes.ToString("00"),
			":",
			seconds.ToString("00")
		});
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00042005 File Offset: 0x00040205
	[Remote("Permissions/Digital")]
	public void StartCurrentTime()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.StartCurrentTime));
			return;
		}
		this.bPaused = false;
		this.PausedTime = 0;
		this.currentClockMode = ClockScript.ClockMode.CurrentTime;
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0004203C File Offset: 0x0004023C
	[Remote("Permissions/Digital")]
	public void StartTimer(int time, bool bPausing = false)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, bool>(RPCTarget.Server, new Action<int, bool>(this.StartTimer), time, bPausing);
			return;
		}
		this.TimerDate = this.DateTimeNow.AddSeconds((double)time);
		this.currentClockMode = ClockScript.ClockMode.Timer;
		this.bPaused = bPausing;
		if (this.bPaused)
		{
			this.PausedTime = this.GetHowManySecondsPassed();
			return;
		}
		this.PausedTime = 0;
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x000420A8 File Offset: 0x000402A8
	[Remote("Permissions/Digital")]
	public void StartStopwatch()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.StartStopwatch));
			return;
		}
		this.bPaused = false;
		this.PausedTime = 0;
		this.StopwatchDate = this.DateTimeNow;
		this.currentClockMode = ClockScript.ClockMode.Stopwatch;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x000420F8 File Offset: 0x000402F8
	[Remote("Permissions/Digital")]
	public void PauseStart()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.PauseStart));
			return;
		}
		base.GetComponent<SoundScript>().BeepSound();
		this.bPaused = !this.bPaused;
		if (this.bPaused)
		{
			this.PausedTime = this.GetHowManySecondsPassed();
			return;
		}
		this.PausedTime = 0;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0004215C File Offset: 0x0004035C
	public int GetHowManySecondsPassed()
	{
		if (this.currentClockMode == ClockScript.ClockMode.CurrentTime)
		{
			return 0;
		}
		if (this.currentClockMode == ClockScript.ClockMode.Timer)
		{
			return (int)(this.TimerDate - this.DateTimeNow).TotalSeconds;
		}
		if (this.currentClockMode == ClockScript.ClockMode.Stopwatch)
		{
			return (int)(this.DateTimeNow - this.StopwatchDate).TotalSeconds;
		}
		ClockScript.ClockMode clockMode = this.currentClockMode;
		return 0;
	}

	// Token: 0x04000684 RID: 1668
	public UILabel Label;

	// Token: 0x04000685 RID: 1669
	public ClockScript.ClockMode currentClockMode;

	// Token: 0x04000686 RID: 1670
	private DateTime DateTimeNow = DateTime.Now;

	// Token: 0x04000687 RID: 1671
	private DateTime TimerDate;

	// Token: 0x04000688 RID: 1672
	public DateTime StopwatchDate;

	// Token: 0x04000689 RID: 1673
	public bool bPaused;

	// Token: 0x0400068A RID: 1674
	public int PausedTime;

	// Token: 0x0400068B RID: 1675
	private int _Hours;

	// Token: 0x0400068C RID: 1676
	private int _Minutes;

	// Token: 0x0400068D RID: 1677
	private int _Seconds;

	// Token: 0x0200058B RID: 1419
	public enum ClockMode
	{
		// Token: 0x04002543 RID: 9539
		CurrentTime,
		// Token: 0x04002544 RID: 9540
		Timer,
		// Token: 0x04002545 RID: 9541
		Stopwatch,
		// Token: 0x04002546 RID: 9542
		Zero
	}
}
