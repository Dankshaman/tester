using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// Token: 0x02000364 RID: 868
public class UnityAnalytics : MonoBehaviour
{
	// Token: 0x0600290B RID: 10507 RVA: 0x00120DD8 File Offset: 0x0011EFD8
	private void Awake()
	{
		if (UnityAnalytics.instance)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		UnityAnalytics.instance = this;
		EventManager.OnUnityAnalytics += this.unityAnalytics;
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x00120E14 File Offset: 0x0011F014
	private void OnDestroy()
	{
		EventManager.OnUnityAnalytics -= this.unityAnalytics;
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x00120E28 File Offset: 0x0011F028
	public void unityAnalytics(string eventName, IDictionary<string, object> eventData, int limit)
	{
		if (limit > 0)
		{
			int num = 0;
			if (UnityAnalytics.AnalyticsLimitDictionary.TryGetValue(eventName, out num))
			{
				if (num >= limit)
				{
					return;
				}
				UnityAnalytics.AnalyticsLimitDictionary[eventName] = num + 1;
			}
			else
			{
				UnityAnalytics.AnalyticsLimitDictionary.Add(eventName, 1);
			}
		}
		AnalyticsResult analyticsResult = Analytics.CustomEvent(eventName, eventData);
		if (analyticsResult == AnalyticsResult.Ok)
		{
			Debug.Log("Unity Analytics: " + eventName);
			return;
		}
		if (analyticsResult == AnalyticsResult.NotInitialized)
		{
			base.StartCoroutine(this.delayUnityAnalytics(eventName, eventData));
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			"Unity Analytics Error: ",
			eventName,
			" ",
			analyticsResult
		}));
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x00120EC8 File Offset: 0x0011F0C8
	public IEnumerator delayUnityAnalytics(string eventName, IDictionary<string, object> eventData)
	{
		yield return null;
		this.unityAnalytics(eventName, eventData, 0);
		yield break;
	}

	// Token: 0x04001AEF RID: 6895
	public static UnityAnalytics instance;

	// Token: 0x04001AF0 RID: 6896
	public static Dictionary<string, int> AnalyticsLimitDictionary = new Dictionary<string, int>();
}
