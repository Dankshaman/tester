using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000282 RID: 642
public class UIButtonHold : MonoBehaviour
{
	// Token: 0x06002153 RID: 8531 RVA: 0x000F0794 File Offset: 0x000EE994
	private void OnPress(bool press)
	{
		if (this.repeatingCoroutione != null)
		{
			base.StopCoroutine(this.repeatingCoroutione);
		}
		if (press)
		{
			this.PressedCount = 1;
			this.PressedTimeHolder = Time.time;
			this.repeatingCoroutione = base.StartCoroutine(this.RepeatingPress());
		}
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000F07D1 File Offset: 0x000EE9D1
	private IEnumerator RepeatingPress()
	{
		for (;;)
		{
			float num = this.PressedSpeed / Mathf.Sqrt((float)this.PressedCount);
			num = Mathf.Max(0.1f, num);
			if (Time.time > this.PressedTimeHolder + num)
			{
				this.PressedTimeHolder = Time.time;
				this.PressedCount++;
				base.SendMessage("OnClick");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040014A3 RID: 5283
	public float PressedSpeed = 0.5f;

	// Token: 0x040014A4 RID: 5284
	private int PressedCount;

	// Token: 0x040014A5 RID: 5285
	private float PressedTimeHolder;

	// Token: 0x040014A6 RID: 5286
	private Coroutine repeatingCoroutione;
}
