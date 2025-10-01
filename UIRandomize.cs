using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class UIRandomize : MonoBehaviour
{
	// Token: 0x060026A2 RID: 9890 RVA: 0x001133DC File Offset: 0x001115DC
	private void OnEnable()
	{
		if (this.RandomizeQueue.Count > 0)
		{
			this.TargetRandomize = this.RandomizeQueue[0];
		}
		if (!this.TargetRandomize)
		{
			this.Close();
			return;
		}
		base.GetComponent<UIHighlightTargets>().Add(this.TargetRandomize.gameObject);
	}

	// Token: 0x060026A3 RID: 9891 RVA: 0x00113433 File Offset: 0x00111633
	public void Queue(RandomizeZone randomizeZone)
	{
		if (!this.RandomizeQueue.Contains(randomizeZone))
		{
			Debug.Log("Queue");
			this.RandomizeQueue.Add(randomizeZone);
			this.UpdateQueueLabel();
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060026A4 RID: 9892 RVA: 0x0011346C File Offset: 0x0011166C
	public void Close()
	{
		if (this.RandomizeQueue.Contains(this.TargetRandomize))
		{
			this.RandomizeQueue.Remove(this.TargetRandomize);
		}
		if (this.TargetRandomize)
		{
			base.GetComponent<UIHighlightTargets>().Remove(this.TargetRandomize.gameObject);
		}
		this.TargetRandomize = null;
		this.UpdateQueueLabel();
		base.gameObject.SetActive(false);
		if (this.RandomizeQueue.Count > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x001134F4 File Offset: 0x001116F4
	private void UpdateQueueLabel()
	{
		int num = this.RandomizeQueue.Count - 1;
		this.QueueLabel.text = "(" + num + ")";
		this.QueueLabel.gameObject.SetActive(num > 0);
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x00113543 File Offset: 0x00111743
	public void Yes()
	{
		if (this.TargetRandomize)
		{
			this.TargetRandomize.Randomize();
		}
		this.Close();
	}

	// Token: 0x060026A7 RID: 9895 RVA: 0x00113563 File Offset: 0x00111763
	private void Update()
	{
		if (this.TargetRandomize == null)
		{
			this.Close();
			return;
		}
	}

	// Token: 0x04001938 RID: 6456
	public UILabel QueueLabel;

	// Token: 0x04001939 RID: 6457
	private List<RandomizeZone> RandomizeQueue = new List<RandomizeZone>();

	// Token: 0x0400193A RID: 6458
	private RandomizeZone TargetRandomize;
}
