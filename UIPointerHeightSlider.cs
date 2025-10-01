using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class UIPointerHeightSlider : MonoBehaviour
{
	// Token: 0x06002676 RID: 9846 RVA: 0x001126A5 File Offset: 0x001108A5
	private void Start()
	{
		this.pointerLiftHeight = base.transform.parent.parent.gameObject.GetComponent<UIPointerLiftHeight>();
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x001126C7 File Offset: 0x001108C7
	private void Update()
	{
		if (UICamera.HoveredUIObject == base.gameObject)
		{
			this.pointerLiftHeight.ShowSlider();
		}
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x001126E6 File Offset: 0x001108E6
	private void OnDrag()
	{
		this.pointerLiftHeight.ShowSlider();
	}

	// Token: 0x04001915 RID: 6421
	private UIPointerLiftHeight pointerLiftHeight;
}
