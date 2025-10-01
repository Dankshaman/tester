using System;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class UIAnalytics : MonoBehaviour
{
	// Token: 0x0600212C RID: 8492 RVA: 0x000EFBE3 File Offset: 0x000EDDE3
	private void OnEnable()
	{
		if (this.FireOnEnable)
		{
			EventManager.TriggerUnityAnalytic(this.EventName + "_OnEnable", null, this.Limit);
		}
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000EFC09 File Offset: 0x000EDE09
	private void OnDisable()
	{
		if (this.FireOnDisable)
		{
			EventManager.TriggerUnityAnalytic(this.EventName + "_OnDisable", null, this.Limit);
		}
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000EFC2F File Offset: 0x000EDE2F
	private void OnClick()
	{
		if (this.FireOnClick)
		{
			EventManager.TriggerUnityAnalytic(this.EventName + "_OnClick", null, this.Limit);
		}
	}

	// Token: 0x04001479 RID: 5241
	public string EventName = "";

	// Token: 0x0400147A RID: 5242
	public int Limit;

	// Token: 0x0400147B RID: 5243
	public bool FireOnEnable;

	// Token: 0x0400147C RID: 5244
	public bool FireOnDisable;

	// Token: 0x0400147D RID: 5245
	public bool FireOnClick;
}
