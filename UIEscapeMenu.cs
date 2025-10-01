using System;
using UnityEngine;

// Token: 0x020002C5 RID: 709
public class UIEscapeMenu : MonoBehaviour
{
	// Token: 0x060022F4 RID: 8948 RVA: 0x000F955D File Offset: 0x000F775D
	private void OnEnable()
	{
		EventManager.TriggerEscapeMenu(true);
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000F9565 File Offset: 0x000F7765
	private void OnDisable()
	{
		EventManager.TriggerEscapeMenu(false);
	}
}
