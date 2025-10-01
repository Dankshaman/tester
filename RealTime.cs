using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class RealTime : MonoBehaviour
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001DF87 File Offset: 0x0001C187
	public static float time
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000424 RID: 1060 RVA: 0x0001DF8E File Offset: 0x0001C18E
	public static float deltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}
}
