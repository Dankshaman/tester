using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class UIPressedButtonCall : MonoBehaviour
{
	// Token: 0x06002691 RID: 9873 RVA: 0x00112F4C File Offset: 0x0011114C
	public void AddPressAction(GameObject addButton, Action action, float interval = 0.2f, float maxExponentialInterval = 0f)
	{
		for (int i = 0; i < this.PressList.Count; i++)
		{
			if (this.PressList[i].button == addButton)
			{
				return;
			}
		}
		UIPressedButtonCall.PressAction pressAction = new UIPressedButtonCall.PressAction();
		pressAction.button = addButton;
		pressAction.action = action;
		pressAction.interval = interval;
		pressAction.maxExponentialInterval = maxExponentialInterval;
		this.PressList.Add(pressAction);
	}

	// Token: 0x06002692 RID: 9874 RVA: 0x00112FB8 File Offset: 0x001111B8
	public void RemovePressAction(GameObject removeButton)
	{
		for (int i = 0; i < this.PressList.Count; i++)
		{
			if (this.PressList[i].button == removeButton)
			{
				this.PressList.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x00113004 File Offset: 0x00111204
	private void Update()
	{
		for (int i = 0; i < this.PressList.Count; i++)
		{
			UIPressedButtonCall.PressAction pressAction = this.PressList[i];
			if (UICamera.IsPressed(pressAction.button))
			{
				float num = pressAction.interval;
				if (pressAction.maxExponentialInterval != 0f)
				{
					num /= Mathf.Sqrt(pressAction.pressCount);
					num = Mathf.Max(pressAction.maxExponentialInterval, num);
				}
				if (pressAction.pressTime > num || pressAction.pressTime == -1f)
				{
					pressAction.action();
					pressAction.pressTime = 0f;
					pressAction.pressCount += 1f;
				}
				pressAction.pressTime += Time.deltaTime;
			}
			else
			{
				pressAction.pressTime = -1f;
				pressAction.pressCount = 0f;
			}
		}
	}

	// Token: 0x04001927 RID: 6439
	private List<UIPressedButtonCall.PressAction> PressList = new List<UIPressedButtonCall.PressAction>();

	// Token: 0x0200077E RID: 1918
	private class PressAction
	{
		// Token: 0x04002C6D RID: 11373
		public GameObject button;

		// Token: 0x04002C6E RID: 11374
		public Action action;

		// Token: 0x04002C6F RID: 11375
		public float interval = 0.2f;

		// Token: 0x04002C70 RID: 11376
		public float maxExponentialInterval;

		// Token: 0x04002C71 RID: 11377
		public float pressTime = -1f;

		// Token: 0x04002C72 RID: 11378
		public float pressCount;
	}
}
