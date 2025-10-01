using System;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public class UILabelBracketModifier : MonoBehaviour
{
	// Token: 0x060024C3 RID: 9411 RVA: 0x0010393C File Offset: 0x00101B3C
	private void Awake()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component)
		{
			component.customModifier = ((string s) => "(" + s.ToLower() + ")");
		}
	}
}
