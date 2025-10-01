using System;
using UnityEngine;

// Token: 0x02000343 RID: 835
public class UISyncGridAxis : MonoBehaviour
{
	// Token: 0x060027A8 RID: 10152 RVA: 0x0011977D File Offset: 0x0011797D
	private void OnEnable()
	{
		this.SyncTarget.SetActive(base.GetComponent<UIToggle>().value);
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x0011977D File Offset: 0x0011797D
	private void Update()
	{
		this.SyncTarget.SetActive(base.GetComponent<UIToggle>().value);
	}

	// Token: 0x04001A09 RID: 6665
	public GameObject SyncTarget;
}
