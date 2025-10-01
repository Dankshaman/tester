using System;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class UIPrivateNotepadAchievement : MonoBehaviour
{
	// Token: 0x06002695 RID: 9877 RVA: 0x001130F4 File Offset: 0x001112F4
	private void OnClick()
	{
		Achievements.Set("ACH_USE_PRIVATE_NOTEPAD");
	}
}
