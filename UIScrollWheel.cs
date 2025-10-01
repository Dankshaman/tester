using System;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class UIScrollWheel : MonoBehaviour
{
	// Token: 0x060026DD RID: 9949 RVA: 0x001143AC File Offset: 0x001125AC
	private void OnScroll()
	{
		float axis = zInput.GetAxis("Rotate", ControlType.All, false);
		if (this.ScrollBar.GetComponent<UIScrollBar>())
		{
			this.ScrollBar.GetComponent<UIScrollBar>().value = this.ScrollBar.GetComponent<UIScrollBar>().value + axis * -0.1f;
		}
		if (this.ScrollBar.GetComponent<UISlider>())
		{
			this.ScrollBar.GetComponent<UISlider>().value = this.ScrollBar.GetComponent<UISlider>().value + axis * -0.1f;
		}
	}

	// Token: 0x04001956 RID: 6486
	public GameObject ScrollBar;
}
