using System;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class UIInputBBCode : MonoBehaviour
{
	// Token: 0x06002486 RID: 9350 RVA: 0x00101677 File Offset: 0x000FF877
	private void Awake()
	{
		this.Input = base.GetComponent<UIInput>();
		this.SetBBCode(true);
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x0010168C File Offset: 0x000FF88C
	private void OnSelect(bool select)
	{
		this.SetBBCode(!select);
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x00101698 File Offset: 0x000FF898
	private void SetBBCode(bool enable)
	{
		if (this.Input && this.Input.label)
		{
			this.Input.label.supportEncoding = enable;
		}
	}

	// Token: 0x04001781 RID: 6017
	private UIInput Input;
}
