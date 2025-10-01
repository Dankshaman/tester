using System;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class UIHideCustomUI : MonoBehaviour
{
	// Token: 0x0600243A RID: 9274 RVA: 0x00100483 File Offset: 0x000FE683
	private void OnEnable()
	{
		UIHideCustomUI.count++;
		if (UIHideCustomUI.count == 1)
		{
			EventManager.TriggerHideCustomUI(true);
		}
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x0010049F File Offset: 0x000FE69F
	private void OnDisable()
	{
		UIHideCustomUI.count--;
		if (UIHideCustomUI.count == 0)
		{
			EventManager.TriggerHideCustomUI(false);
		}
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x001004BA File Offset: 0x000FE6BA
	private void Awake()
	{
		EventManager.OnLanguageChange += delegate(string previousLanguageCode, string newLanguageCode)
		{
		};
	}

	// Token: 0x04001746 RID: 5958
	private static int count;
}
