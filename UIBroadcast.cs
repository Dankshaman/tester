using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class UIBroadcast : MonoBehaviour
{
	// Token: 0x06002149 RID: 8521 RVA: 0x000F0502 File Offset: 0x000EE702
	private void Awake()
	{
		UIBroadcast.broadcast = this;
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x000F050C File Offset: 0x000EE70C
	private void OnDisable()
	{
		foreach (KeyValuePair<UILabel, Coroutine> keyValuePair in this.LabelCoroutines)
		{
			if (keyValuePair.Key)
			{
				keyValuePair.Key.alpha = 0f;
			}
		}
		UIBroadcast.spamMsgs.Clear();
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000F0584 File Offset: 0x000EE784
	private void SetupCoroutineDictionary()
	{
		if (this.LabelCoroutines.Count > 0)
		{
			return;
		}
		List<Transform> childList = this.grid.GetChildList();
		for (int i = 0; i < childList.Count; i++)
		{
			this.LabelCoroutines.Add(childList[i].GetComponent<UILabel>(), null);
		}
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000F05D8 File Offset: 0x000EE7D8
	private void Msg(string Msg, Color color, float duration)
	{
		UILabel label = this.GetLabel();
		label.text = Colour.RGBHexFromColour(color) + Msg;
		label.color = Colour.White;
		this.SetupCoroutineDictionary();
		if (this.LabelCoroutines[label] != null)
		{
			base.StopCoroutine(this.LabelCoroutines[label]);
		}
		this.LabelCoroutines[label] = base.StartCoroutine(this.LabelFade(label, duration));
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000F0653 File Offset: 0x000EE853
	private IEnumerator LabelFade(UILabel label, float duration)
	{
		label.alpha = 1f;
		yield return new WaitForSeconds(duration);
		while (label.alpha > 0f)
		{
			label.alpha -= Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x000F066C File Offset: 0x000EE86C
	private UILabel GetLabel()
	{
		List<Transform> childList = this.grid.GetChildList();
		for (int i = 0; i < childList.Count - 1; i++)
		{
			childList[i].name = (i + 1).ToString();
		}
		Transform transform = childList[childList.Count - 1];
		transform.name = "0";
		transform.localPosition = new Vector3(0f, 72f, 0f);
		this.grid.Reposition();
		return transform.gameObject.GetComponent<UILabel>();
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000F06F7 File Offset: 0x000EE8F7
	private IEnumerator AddSpam(string Msg, float time)
	{
		UIBroadcast.spamMsgs.Add(Msg);
		yield return new WaitForSeconds(time);
		UIBroadcast.spamMsgs.Remove(Msg);
		yield break;
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000F0710 File Offset: 0x000EE910
	public static void Log(string Msg, Color color, float duration = 2f, float limitSpamTime = 0f)
	{
		if (!UIBroadcast.broadcast || !UIBroadcast.broadcast.gameObject.activeInHierarchy)
		{
			return;
		}
		if (limitSpamTime > 0f)
		{
			if (UIBroadcast.spamMsgs.Contains(Msg))
			{
				return;
			}
			UIBroadcast.broadcast.StartCoroutine(UIBroadcast.broadcast.AddSpam(Msg, limitSpamTime));
		}
		UIBroadcast.broadcast.Msg(Msg, color, duration);
	}

	// Token: 0x0400149F RID: 5279
	private static UIBroadcast broadcast;

	// Token: 0x040014A0 RID: 5280
	public UIGrid grid;

	// Token: 0x040014A1 RID: 5281
	private Dictionary<UILabel, Coroutine> LabelCoroutines = new Dictionary<UILabel, Coroutine>();

	// Token: 0x040014A2 RID: 5282
	private static List<string> spamMsgs = new List<string>();
}
