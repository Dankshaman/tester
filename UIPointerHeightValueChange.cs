using System;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class UIPointerHeightValueChange : MonoBehaviour
{
	// Token: 0x0600267A RID: 9850 RVA: 0x001126F3 File Offset: 0x001108F3
	private void Start()
	{
		this.ThisSlider = base.GetComponent<UISlider>();
		this.PrevValue = this.ThisSlider.value;
		base.InvokeRepeating("UpdateSlow", 0.2f, 0.2f);
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x00112728 File Offset: 0x00110928
	private void UpdateSlow()
	{
		if (PlayerScript.Pointer)
		{
			if (this.ThisSlider.value != this.PrevValue)
			{
				this.PrevValue = this.ThisSlider.value;
				PlayerScript.PointerScript.RaiseHeight = this.PrevValue;
			}
			this.ThisSlider.value = PlayerScript.PointerScript.RaiseHeight;
		}
	}

	// Token: 0x04001916 RID: 6422
	private UISlider ThisSlider;

	// Token: 0x04001917 RID: 6423
	private float PrevValue;
}
