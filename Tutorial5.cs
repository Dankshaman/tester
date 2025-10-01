using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class Tutorial5 : MonoBehaviour
{
	// Token: 0x060000AC RID: 172 RVA: 0x00005098 File Offset: 0x00003298
	public void SetDurationToCurrentProgress()
	{
		UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].duration = Mathf.Lerp(2f, 0.5f, UIProgressBar.current.value);
		}
	}
}
