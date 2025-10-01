using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000327 RID: 807
public abstract class UIReactiveMenu : MonoBehaviour
{
	// Token: 0x060026AC RID: 9900 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void Awake()
	{
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x001135CF File Offset: 0x001117CF
	protected virtual IEnumerator Start()
	{
		yield return null;
		this.BlockUpdateSource = true;
		for (int i = 0; i < this.ReactiveElements.Count; i++)
		{
			EventDelegate.Add(this.ReactiveElements[i], new EventDelegate.Callback(this.ElementsOnChange));
		}
		for (int j = 0; j < this.ReactiveInputs.Count; j++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.ReactiveInputs[j]);
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnSelected));
		}
		this.BlockUpdateSource = false;
		yield break;
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x001135E0 File Offset: 0x001117E0
	protected virtual void OnDestroy()
	{
		for (int i = 0; i < this.ReactiveElements.Count; i++)
		{
			EventDelegate.Remove(this.ReactiveElements[i], new EventDelegate.Callback(this.ElementsOnChange));
		}
		for (int j = 0; j < this.ReactiveInputs.Count; j++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.ReactiveInputs[j]);
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnSelected));
		}
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x00113669 File Offset: 0x00111869
	protected virtual void OnEnable()
	{
		base.StartCoroutine(this.DelayTriggerReloadUI());
	}

	// Token: 0x060026B0 RID: 9904 RVA: 0x00113678 File Offset: 0x00111878
	private IEnumerator DelayTriggerReloadUI()
	{
		yield return null;
		this.TriggerReloadUI();
		yield break;
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x00113687 File Offset: 0x00111887
	protected void TriggerReloadUI()
	{
		this.BlockUpdateSource = true;
		this.ReloadUI();
		this.BlockUpdateSource = false;
	}

	// Token: 0x060026B2 RID: 9906 RVA: 0x0011369D File Offset: 0x0011189D
	private void OnSelected(GameObject go, bool selected)
	{
		if (!selected)
		{
			this.ElementsOnChange();
		}
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x001136A8 File Offset: 0x001118A8
	protected void ElementsOnChange()
	{
		if (!this.BlockUpdateSource)
		{
			this.UpdateSource();
			base.CancelInvoke("NetworkSync");
			base.Invoke("NetworkSync", 0.1f);
		}
	}

	// Token: 0x060026B4 RID: 9908
	protected abstract void ReloadUI();

	// Token: 0x060026B5 RID: 9909
	protected abstract void UpdateSource();

	// Token: 0x060026B6 RID: 9910
	protected abstract void NetworkSync();

	// Token: 0x0400193B RID: 6459
	public List<List<EventDelegate>> ReactiveElements = new List<List<EventDelegate>>();

	// Token: 0x0400193C RID: 6460
	[HideInInspector]
	public List<GameObject> ReactiveInputs = new List<GameObject>();

	// Token: 0x0400193D RID: 6461
	protected bool BlockUpdateSource;
}
