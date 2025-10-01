using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
	// Token: 0x060000D7 RID: 215 RVA: 0x00006064 File Offset: 0x00004264
	private void OnClick()
	{
		if (this.target != null)
		{
			if (!this.toggle)
			{
				NGUITools.SetActive(this.target, this.state);
				return;
			}
			NGUITools.SetActive(this.target, !this.target.activeSelf);
		}
	}

	// Token: 0x040000B4 RID: 180
	public GameObject target;

	// Token: 0x040000B5 RID: 181
	public bool state = true;

	// Token: 0x040000B6 RID: 182
	public bool toggle = true;
}
