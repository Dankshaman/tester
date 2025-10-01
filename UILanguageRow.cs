using System;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class UILanguageRow : MonoBehaviour
{
	// Token: 0x060024C7 RID: 9415 RVA: 0x001039B6 File Offset: 0x00101BB6
	public void SelectLanguage()
	{
		Singleton<UILanguageSettings>.Instance.SelectLanguage(this.LanguageCode);
	}

	// Token: 0x040017CC RID: 6092
	public string LanguageName;

	// Token: 0x040017CD RID: 6093
	public string LanguageCode;
}
