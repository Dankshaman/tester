using System;

// Token: 0x0200014B RID: 331
public static class LibTime
{
	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060010EF RID: 4335 RVA: 0x000754C4 File Offset: 0x000736C4
	public static double DaysSinceTTSEpoch
	{
		get
		{
			return (DateTime.Today - LibTime.TTSEpoch).TotalDays;
		}
	}

	// Token: 0x04000AC6 RID: 2758
	public static DateTime TTSEpoch = new DateTime(2015, 6, 5);
}
