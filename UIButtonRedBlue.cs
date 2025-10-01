using System;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class UIButtonRedBlue : MonoBehaviour
{
	// Token: 0x06002156 RID: 8534 RVA: 0x000F07F3 File Offset: 0x000EE9F3
	private void Start()
	{
		UIButton component = base.GetComponent<UIButton>();
		component.hover = Colour.Red;
		component.pressed = Colour.Blue;
	}
}
