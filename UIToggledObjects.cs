using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000053 RID: 83
[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	// Token: 0x060002AA RID: 682 RVA: 0x00011AE8 File Offset: 0x0000FCE8
	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count == 0 && this.deactivate.Count == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		EventDelegate.Add(base.GetComponent<UIToggle>().onChange, new EventDelegate.Callback(this.Toggle));
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00011B70 File Offset: 0x0000FD70
	public void Toggle()
	{
		if (this.bFirstToggle)
		{
			this.bFirstToggle = false;
			return;
		}
		bool value = UIToggle.current.value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], value);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !value);
			}
		}
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00011BF5 File Offset: 0x0000FDF5
	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	// Token: 0x04000255 RID: 597
	public List<GameObject> activate;

	// Token: 0x04000256 RID: 598
	public List<GameObject> deactivate;

	// Token: 0x04000257 RID: 599
	[HideInInspector]
	[SerializeField]
	private GameObject target;

	// Token: 0x04000258 RID: 600
	[HideInInspector]
	[SerializeField]
	private bool inverse;

	// Token: 0x04000259 RID: 601
	private bool bFirstToggle = true;
}
