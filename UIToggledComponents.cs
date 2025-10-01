using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
[ExecuteInEditMode]
[RequireComponent(typeof(UIToggle))]
[AddComponentMenu("NGUI/Interaction/Toggled Components")]
public class UIToggledComponents : MonoBehaviour
{
	// Token: 0x060002A7 RID: 679 RVA: 0x000119E4 File Offset: 0x0000FBE4
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

	// Token: 0x060002A8 RID: 680 RVA: 0x00011A6C File Offset: 0x0000FC6C
	public void Toggle()
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.activate[i].enabled = UIToggle.current.value;
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.deactivate[j].enabled = !UIToggle.current.value;
			}
		}
	}

	// Token: 0x04000251 RID: 593
	public List<MonoBehaviour> activate;

	// Token: 0x04000252 RID: 594
	public List<MonoBehaviour> deactivate;

	// Token: 0x04000253 RID: 595
	[HideInInspector]
	[SerializeField]
	private MonoBehaviour target;

	// Token: 0x04000254 RID: 596
	[HideInInspector]
	[SerializeField]
	private bool inverse;
}
