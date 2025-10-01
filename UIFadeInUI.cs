using System;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class UIFadeInUI : MonoBehaviour
{
	// Token: 0x060022F7 RID: 8951 RVA: 0x000F956D File Offset: 0x000F776D
	private void Awake()
	{
		UIRect rect = base.GetComponent<UIRect>();
		rect.alpha = this.StartAlpha;
		Wait.Frames(delegate
		{
			TweenAlpha.Begin(rect.gameObject, this.Duration, this.EndAlpha, 0f).method = UITweener.Method.EaseInOut;
		}, 3);
	}

	// Token: 0x04001629 RID: 5673
	private float StartAlpha = 0.01f;

	// Token: 0x0400162A RID: 5674
	private float EndAlpha = 1f;

	// Token: 0x0400162B RID: 5675
	private float Duration = 0.5f;
}
